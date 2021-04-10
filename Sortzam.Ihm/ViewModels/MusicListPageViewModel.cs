using Microsoft.Win32;
using Sortzam.Ihm.Models;
using Sortzam.Ihm.Models.Command;
using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using Sortzam.Lib.Detectors;
using System.Linq;
using Tools.Utils;
using System.Threading.Tasks;
using System.Windows.Threading;
using Tools.Comparer;
using System.Collections.Concurrent;
using System.Threading;

namespace Sortzam.Ihm.ViewModels
{
    /// <summary>
    /// Class used to be bind with the main view.
    /// </summary>
    public class MusicListPageViewModel : Notifier
    {
        private bool isAnalyzeRunning;

        /// <summary>
        /// Constructor - Init display properties.
        /// </summary>
        public MusicListPageViewModel()
        {
            Musics = new ObservableCollection<MusicItem>();
            SelectedMusic = new MusicItem();
            isAnalyzeRunning = false;
            AnalyseButtonText = "Analyser";

            App.SettingsEvent += OnUpdateSettings;

            InitCommands();
        }

        #region Properties
        /// <summary>
        /// List of musics display for the user.
        /// </summary>
        public ObservableCollection<MusicItem> Musics
        {
            get; set;
        }

        /// <summary>
        /// Folder chosen by the user.
        /// </summary>
        private string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }

        private MusicItem selectedMusic;
        /// <summary>
        /// Current selected music display on the side HMI.
        /// </summary>
        public MusicItem SelectedMusic
        {
            get { return selectedMusic; }
            set
            {
                selectedMusic = value;
                OnPropertyChanged("SelectedMusic");
            }
        }

        /// <summary>
        /// Get the value of Auto set.
        /// </summary>
        private bool autoSet;
        public bool AutoSet
        {
            get { return autoSet; }
            set
            {
                autoSet = value;
                OnPropertyChanged("AutoSet");
            }
        }

        /// <summary>
        /// Percentage of progress during analyze.
        /// </summary>
        private bool enableAnalyzeButton;
        public bool EnableAnalyzeButton
        {
            get { return enableAnalyzeButton; }
            set
            {
                enableAnalyzeButton = value;
                OnPropertyChanged("EnableAnalyzeButton");
            }
        }

        /// <summary>
        /// Percentage of progress during analyze.
        /// </summary>
        private string analyseButtonText;
        public string AnalyseButtonText
        {
            get { return analyseButtonText; }
            set
            { 
                analyseButtonText = value;
                OnPropertyChanged("AnalyseButtonText");
            }
        }

        /// <summary>
        /// Percentage of progress during analyze.
        /// </summary>
        private double percentProgress;
        public double PercentProgress
        {
            get { return percentProgress; }
            set
            {
                percentProgress = Math.Round(value, 2);
                OnPropertyChanged("PercentProgress");
            }
        }
        #endregion

        #region Commands
        public ICommand BrowseCommand { get; private set; }
        public ICommand SelectAllCommand { get; private set; }
        public ICommand DeselectAllCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAllCommand { get; private set; }
        public ICommand RestoreCommand { get; private set; }
        public ICommand PreviousFileCommand { get; private set; }
        public ICommand NextFileCommand { get; private set; }
        public ICommand AnalyzeCommand { get; private set; }

        /// <summary>
        /// Prepare commands for all buttons
        /// </summary>
        private void InitCommands()
        {
            BrowseCommand = new RelayCommand(x => { Browse(); });
            SelectAllCommand = new RelayCommand(x => { SelectAll(true); });
            DeselectAllCommand = new RelayCommand(x => { SelectAll(false); });
            SaveCommand = new RelayCommand(x => { SaveFile(); });
            SaveAllCommand = new RelayCommand(x => { SaveAllFiles(); });
            RestoreCommand = new RelayCommand(x => { Restore(); });
            PreviousFileCommand = new RelayCommand(x => { PreviousFile(); });
            NextFileCommand = new RelayCommand(x => { NextFile(); });
            AnalyzeCommand = new RelayCommand(x => { Analyse(); });
        }
        #endregion

        #region Button Methods
        /// <summary>
        /// Ask to the user to chose a folder and display content. Subfolders are also displayed. 
        /// </summary>
        private void Browse()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                FolderPath = fbd.SelectedPath;
                IEnumerable<MusicFileDao> files = new MusicFileDaoDetector().SearchInDirectory(FolderPath); // Load path files and metas for each
                Musics.Clear();

                IEnumerable<MusicItem> musicFiles = files.Select(p => new MusicItem()
                {
                    FileName = p.FileName,
                    Path = p.Path,
                    File = p
                });

                foreach(MusicItem file in musicFiles)
                    Musics.Add(file);
            }
        }

        /// <summary>
        /// Check or Uncheck all files on the display. 
        /// </summary>
        /// <param name="value"></param>
        private void SelectAll(bool value)
        {
            foreach (MusicItem music in Musics)
                music.IsChecked = value;
        }

        /// <summary>
        /// Save updates into the file.
        /// </summary>
        private void SaveFile()
        {
            SelectedMusic.Save();
        }

        /// <summary>
        /// Save metadata for each files
        /// </summary>
        private void SaveAllFiles()
        {
            foreach(MusicItem music in Musics)
                music.Save();

            MessageBox.Show("Tous les fichiers ont étés sauvegardés.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Restore previous values for metadata.
        /// </summary>
        private void Restore()
        {
            SelectedMusic.Restore();
        }

        /// <summary>
        /// Save updates into the file.
        /// </summary>
        private void PreviousFile()
        {
            int currentIndex = Musics.IndexOf(SelectedMusic) - 1;

            if (currentIndex < 0)
                currentIndex = Musics.Count - 1;

            SelectedMusic = Musics[currentIndex];
        }

        /// <summary>
        /// Save updates into the file.
        /// </summary>
        private void NextFile()
        {
            int currentIndex = Musics.IndexOf(SelectedMusic) + 1;

            if (currentIndex >= Musics.Count)
                currentIndex = 0;

            SelectedMusic = Musics[currentIndex];
        }

        /// <summary>
        /// Launch the analyze function for selected files.
        /// </summary>
        private void Analyse()
        {
            // If the analyze process is not running, launch it.
            if (!isAnalyzeRunning)
            {
                int nbMusics = 0;
                foreach (MusicItem music in Musics)
                    if (music.IsChecked) nbMusics++;

                if (nbMusics > 0)
                {
                    PercentProgress = 0;
                    isAnalyzeRunning = true;
                    AnalyseButtonText = "Arrêter";

                    Task.Factory.StartNew(() =>
                    {
                        double nbAnalysed = 0;

                        foreach (MusicItem music in Musics)
                        {
                            if (!isAnalyzeRunning)
                            {
                                Console.WriteLine("Stop process");
                                break;
                            }

                            if (music.IsChecked)
                            {
                                IEnumerable<MusicDao> results = null;
                                try
                                {
                                    Settings settings = Settings.Instance;
                                    results = new MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(music.Path);
                                }
                                catch
                                {
                                    music.Status = MusicItemStatus.Error;
                                }

                                nbAnalysed++;
                                PercentProgress = nbAnalysed / nbMusics * 100;

                                if (results != null)
                                {
                                    App.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        music.Results.Clear();

                                        foreach (MusicDao result in results)
                                        {
                                            AnalyzeResult aResult = new AnalyzeResult(result);
                                            aResult.MatchLevel = LevenshteinDistance.Compute(aResult.Title, SelectedMusic.Title);

                                            music.AddResult(aResult);
                                        }

                                        music.Status = MusicItemStatus.Analysed;

                                        if (AutoSet)
                                            music.SetBestResult();
                                    });
                                }
                            }
                        }

                        PercentProgress = 100;
                        isAnalyzeRunning = false;
                        AnalyseButtonText = "Analyser";
                    });
                }
                else
                {
                    MessageBox.Show("Veuillez cocher au moins une musique.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else // If the analyze process is running, stop it.
            {
                isAnalyzeRunning = false;
                AnalyseButtonText = "Analyser";
                PercentProgress = 0;
            }
        }
        #endregion

        /// <summary>
        /// Update HMI from settings changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpdateSettings(object sender, EventArgs e)
        {
            Settings settings = Settings.Instance;
            EnableAnalyzeButton = settings.UseAccount;
        }

        /// <summary>
        /// Display the file selected by the user.
        /// </summary>
        /// <param name="music"></param>
        public void SelectFile(MusicItem music)
        {
            SelectedMusic = music;
            SelectedMusic.LoadFromMusicFileDao();
        }

        /// <summary>
        /// Display the file selected by the user.
        /// </summary>
        /// <param name="music"></param>
        public void SelectResult(AnalyzeResult result)
        {
            SelectedMusic.Title = result.Music.Title;
            SelectedMusic.Artist = result.Music.Artist;
            SelectedMusic.Album = result.Music.Album;
            SelectedMusic.Kind = result.Music.Kind;
            SelectedMusic.Year = result.Music.Year;
            SelectedMusic.Comment = result.Music.Comment;
        }
    }
}
