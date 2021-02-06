using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sortzam.Ihm.Models
{
    /// <summary>
    /// Object to display a music file.
    /// </summary>
    public class MusicItem : Notifier
    {
        public MusicItem()
        {
            Results = new ObservableCollection<MusicDao>();
        }

        #region Properties
        /// <summary>
        /// File name displayed.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File Path
        /// </summary>
        public string Path { get; set; }

        private MusicFileDao file;
        /// <summary>
        /// File managed by this class. 
        /// </summary>
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

        /// <summary>
        /// Music title.
        /// </summary>
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

        /// <summary>
        /// Music artist
        /// </summary>
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

        /// <summary>
        /// Kind of the music
        /// </summary>
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

        /// <summary>
        /// Music album
        /// </summary>
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

        /// <summary>
        /// Music comments
        /// </summary>
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

        /// <summary>
        /// Music year
        /// </summary>
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
        /// <summary>
        /// Say if the music is checked or not by the user.
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        private ObservableCollection<MusicDao> results;
        public ObservableCollection<MusicDao> Results
        {
            get { return results; }
            set
            {
                results = value;
                OnPropertyChanged("Results");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Bind MusicDaoFile to the displayed item.
        /// </summary>
        /// <param name="file"></param>
        public void LoadFromMusicFileDao()
        {
            if(!string.IsNullOrEmpty(Path) && File == null)
                File = new MusicFileDao(Path);
        }

        /// <summary>
        /// Save changes in the binded file.
        /// </summary>
        public void Save()
        {
            if (file != null)
                file.Save();
        }
        #endregion
    }
}
