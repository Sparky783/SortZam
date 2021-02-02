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

namespace Sortzam.Ihm.ViewModels
{
    public class MusicListPageViewModel: Notifier
    {
        public MusicListPageViewModel()
        {
            Musics = new ObservableCollection<MusicItem>();

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

        public MusicDao SelectedMusic
        {
            get; set;
        }
        #endregion

        #region Commands
        public ICommand BrowseCommand { get; private set; }
        public ICommand SelectAllCommand { get; private set; }
        public ICommand DeselectAllCommand { get; private set; }

        private void InitCommands()
        {
            BrowseCommand = new RelayCommand(x => { Browse(); });
            SelectAllCommand = new RelayCommand(x => { SelectAll(true); });
            DeselectAllCommand = new RelayCommand(x => { SelectAll(false); });
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
        #endregion

        public void OpenFile(MusicItem music)
        {
            SelectedMusic = new MusicFileDao(music.Path);
        }
    }
}
