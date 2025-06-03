using Sortzam.Ihm.Models;
using Sortzam.Ihm.Models.Command;
using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Forms;
using Sortzam.Lib.Detectors;
using System.Linq;
using System.Threading.Tasks;
using Tools.Comparer;

namespace Sortzam.Ihm.ViewModels
{
    /// <summary>
    /// Class used to be bind with the main view.
    /// </summary>
    public class MusicListPageViewModel : Notifier
    {
        private bool _isAnalyzeRunning;

        /// <summary>
        /// Constructor - Init display properties.
        /// </summary>
        public MusicListPageViewModel()
        {
            _isAnalyzeRunning = false;

            Musics = new ObservableCollection<MusicItem>();
            SelectedMusic = new MusicItem();
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
        private string _folderPath;
        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }

        /// <summary>
        /// Current selected music display on the side HMI.
        /// </summary>
        private MusicItem _selectedMusic;
        public MusicItem SelectedMusic
        {
            get { return _selectedMusic; }
            set
            {
                _selectedMusic = value;
                OnPropertyChanged("SelectedMusic");
            }
        }

        /// <summary>
        /// Get the value of Auto set.
        /// </summary>
        private bool _autoSet;
        public bool AutoSet
        {
            get { return _autoSet; }
            set
            {
                _autoSet = value;
                OnPropertyChanged("AutoSet");
            }
        }

        /// <summary>
        /// Percentage of progress during analyze.
        /// </summary>
        private bool _enableAnalyzeButton;
        public bool EnableAnalyzeButton
        {
            get { return _enableAnalyzeButton; }
            set
            {
                _enableAnalyzeButton = value;
                OnPropertyChanged("EnableAnalyzeButton");
            }
        }

        /// <summary>
        /// Percentage of progress during analyze.
        /// </summary>
        private string _analyseButtonText;
        public string AnalyseButtonText
        {
            get { return _analyseButtonText; }
            set
            {
                _analyseButtonText = value;
                OnPropertyChanged("AnalyseButtonText");
            }
        }

        /// <summary>
        /// Percentage of progress during analyze.
        /// </summary>
        private double _percentProgress;
        public double PercentProgress
        {
            get { return _percentProgress; }
            set
            {
                _percentProgress = Math.Round(value, 2);
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

        #region Public methods
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
        #endregion

        #region Private Methods
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

                if(files != null)
                {
                    IEnumerable<MusicItem> musicFiles = files.Select(p => new MusicItem()
                    {
                        FileName = p.FileName,
                        Path = p.Path,
                        File = p
                    });

                    foreach (MusicItem file in musicFiles)
                        Musics.Add(file);
                }
            }
        }

        /// <summary>
        /// Check or Uncheck all files on the display. 
        /// </summary>
        /// <param name="value"></param>
        private void SelectAll(bool value)
        {
            if (!CheckWorkingFolder())
                return;

            foreach (MusicItem music in Musics)
                music.IsChecked = value;
        }

        /// <summary>
        /// Save updates into the file.
        /// </summary>
        private void SaveFile()
        {
            if (!CheckWorkingFolder())
                return;

            SelectedMusic.Save();
        }

        /// <summary>
        /// Save metadata for each files
        /// </summary>
        private void SaveAllFiles()
        {
            if(!CheckWorkingFolder())
                return;

            foreach (MusicItem music in Musics)
                music.Save();

            MessageBox.Show("Tous les fichiers ont étés sauvegardés.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Restore previous values for metadata.
        /// </summary>
        private void Restore()
        {
            if (!CheckWorkingFolder())
                return;

            SelectedMusic.Restore();
        }

        /// <summary>
        /// Save updates into the file.
        /// </summary>
        private void NextFile()
        {
            if (!CheckWorkingFolder())
                return;

            if (Musics.Count <= 0)
                return;

            int currentIndex = Musics.IndexOf(SelectedMusic) + 1;

            if (currentIndex >= Musics.Count)
                currentIndex = 0;

            SelectedMusic = Musics[currentIndex];
        }

        /// <summary>
        /// Save updates into the file.
        /// </summary>
        private void PreviousFile()
        {
            if (!CheckWorkingFolder())
                return;

            if (Musics.Count <= 0)
                return;

            int currentIndex = Musics.IndexOf(SelectedMusic) - 1;

            if (currentIndex < 0)
                currentIndex = Musics.Count - 1;

            SelectedMusic = Musics[currentIndex];
        }

        /// <summary>
        /// Launch the analyze function for selected files.
        /// </summary>
        private void Analyse()
        {
            // If the analyze process is not running, launch it.
            if (_isAnalyzeRunning) // If the analyze process is running, stop it.
            {
                _isAnalyzeRunning = false;
                AnalyseButtonText = "Analyser";
                PercentProgress = 0;

                return;
            }

            int nbMusics = 0;
            foreach (MusicItem music in Musics)
                if (music.IsChecked) nbMusics++;

            if (nbMusics <= 0)
            {
                MessageBox.Show("Veuillez cocher au moins une musique.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            PercentProgress = 0;
            _isAnalyzeRunning = true;
            AnalyseButtonText = "Arrêter";

            Task.Factory.StartNew(() =>
            {
                double nbAnalysed = 0;

                foreach (MusicItem music in Musics)
                {
                    if (!_isAnalyzeRunning)
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
                _isAnalyzeRunning = false;
                AnalyseButtonText = "Analyser";
            });
            
        }

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

        private bool CheckWorkingFolder()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("Veuillez sélectionner un dossier de travail.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }
        #endregion
    }
}
