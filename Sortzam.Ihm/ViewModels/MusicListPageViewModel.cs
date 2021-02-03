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

namespace Sortzam.Ihm.ViewModels
{
    public class MusicListPageViewModel: Notifier
    {
        public MusicListPageViewModel()
        {
            Musics = new ObservableCollection<MusicItem>();
            SelectedMusic = new MusicItem();

            InitCommands();
        }

        #region Properties
        public ObservableCollection<MusicItem> Musics
        {
            get; set;
        }

        public string FolderPath
        {
            get; set;
        }

        public MusicItem SelectedMusic
        {
            get; set;
        }
        #endregion

        #region Commands
        public ICommand BrowseCommand { get; private set; }
        public ICommand SelectAllCommand { get; private set; }
        public ICommand DeselectAllCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }
        public ICommand ShazamCommand { get; private set; }

        private void InitCommands()
        {
            BrowseCommand = new RelayCommand(x => { Browse(); });
            SelectAllCommand = new RelayCommand(x => { SelectAll(true); });
            DeselectAllCommand = new RelayCommand(x => { SelectAll(false); });
            FinishCommand = new RelayCommand(x => { SaveFile(); });
            ShazamCommand = new RelayCommand(x => { RunShazam(); });
        }
        #endregion

        #region Button Methods
        private void Browse()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                FolderPath = fbd.SelectedPath;
                string[] files = Directory.GetFiles(FolderPath);
                Musics.Clear();

                foreach (string filePath in files)
                {
                    string extension = Path.GetExtension(filePath).Remove(0, 1);
                    MusicFileExtension extensionOut;

                    if (Enum.TryParse(extension, true, out extensionOut) && Enum.IsDefined(typeof(MusicFileExtension), extensionOut))
                    {
                        MusicItem musicItem = new MusicItem();
                        musicItem.FileName = Path.GetFileName(filePath);
                        musicItem.Path = filePath;

                        Musics.Add(musicItem);
                    }
                }
            }
        }

        private void SelectAll(bool value)
        {
            foreach (MusicItem music in Musics)
                music.IsChecked = value;
        }

        private void SaveFile()
        {
            SelectedMusic.Save();
        }

        private void RunShazam()
        {
            //TODO : delete Secretkey from here
            string apiHost = "identify-eu-west-1.acrcloud.com";
            string apiKey = "ca88123807e49300eaea0fb9441c1bde";
            string secretKey = "ri9MAp8fXzEXu300Apch3Qj74Hadz2XiJbr9izox";
            List<MusicDao> result = (List<MusicDao>)new MusicTagDetector(apiHost, apiKey, secretKey).Recognize(SelectedMusic.Path);


            MessageBox.Show($"{result.Count} musiques trouvées");
            //SelectedMusic.File.
        }
        #endregion

        public void OpenFile(MusicItem music)
        {
            SelectedMusic.Path = music.Path;
            SelectedMusic.FileName = music.FileName;
            SelectedMusic.LoadFromMusicFileDao(new MusicFileDao(music.Path));
        }
    }
}
