using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sortzam.Ihm.Models
{
    public class MusicItem : Notifier
    {
        /*public MusicItem()
        {
            File = null;
            FileName = "";
            Path = "";
            Title = "";
            Artist = "";
            Album = "";
            Kind = "";
            Comment = "";
            Year = 0;
        }*/

        #region Properties
        public string FileName { get; set; }

        public string Path { get; set; }

        private MusicFileDao file;
        public MusicFileDao File
        {
            get { return file; }
            set
            {
                file = value;
                file.Load();
                OnPropertyChanged("Title");
                OnPropertyChanged("Artist");
                OnPropertyChanged("Kind");
                OnPropertyChanged("Album");
                OnPropertyChanged("Comment");
                OnPropertyChanged("Year");
            }
        }

        public string Title
        {
            get
            {
                if (file != null)
                    return file.Title;

                return "";
            }

            set
            {
                if (file != null)
                    file.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Artist
        {
            get
            {
                if (file != null)
                    return file.Artist;

                return "";
            }

            set
            {
                if (file != null)
                    file.Artist = value;
                OnPropertyChanged("Artist");
            }
        }

        public string Kind
        {
            get
            {
                if (file != null)
                    return file.Kind;

                return "";
            }

            set
            {
                if (file != null)
                    file.Kind = value;
                OnPropertyChanged("Kind");
            }
        }

        public string Album
        {
            get
            {
                if (file != null)
                    return file.Album;

                return "";
            }

            set
            {
                if (file != null)
                    file.Album = value;
                OnPropertyChanged("Album");
            }
        }

        public string Comment
        {
            get
            {
                if (file != null)
                    return file.Comment;

                return "";
            }

            set
            {
                if (file != null)
                    file.Comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public int? Year
        {
            get
            {
                if (file != null)
                    return file.Year;

                return null;
            }

            set
            {
                if (file != null)
                    file.Year = value;
                OnPropertyChanged("Year");
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        #endregion

        #region Methods
        public void LoadFromMusicFileDao(MusicFileDao file)
        {
            File = file;
        }

        public void Save()
        {
            if (file != null)
                file.Save();
        }
        #endregion
    }
}
