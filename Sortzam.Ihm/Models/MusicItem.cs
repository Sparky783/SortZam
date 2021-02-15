using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace Sortzam.Ihm.Models
{
    /// <summary>
    /// Object to display a music file.
    /// </summary>
    public class MusicItem : Notifier
    {
        public MusicItem()
        {
            Results = new ObservableCollection<AnalyzeResult>();
        }

        #region Properties

        #region Initial Properties
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

                InitialTitle = file.Title;
                InitialAlbum = file.Album;
                InitialArtist = file.Artist;
                InitialKind = file.Kind;
                InitialComment = file.Comment;
                InitialYear = file.Year;

                OnPropertyChanged("Title");
                OnPropertyChanged("Artist");
                OnPropertyChanged("Kind");
                OnPropertyChanged("Album");
                OnPropertyChanged("Comment");
                OnPropertyChanged("Year");

                Status = MusicItemStatus.Loaded;
            }
        }

        private string initialTitle;
        public string InitialTitle
        {
            get { return initialTitle;}
            set
            {
                initialTitle = value;
                OnPropertyChanged("InitialTitle");
            }
        }

        private string initialAlbum;
        public string InitialAlbum
        {
            get { return initialAlbum; }
            set
            {
                initialAlbum = value;
                OnPropertyChanged("InitialAlbum");
            }
        }

        private string initialArtist;
        public string InitialArtist
        {
            get { return initialArtist; }
            set
            {
                initialArtist = value;
                OnPropertyChanged("InitialArtist");
            }
        }

        private string initialKind;
        public string InitialKind
        {
            get { return initialKind; }
            set
            {
                initialKind = value;
                OnPropertyChanged("InitialKind");
            }
        }

        private string initialComment;
        public string InitialComment
        {
            get { return initialComment; }
            set
            {
                initialComment = value;
                OnPropertyChanged("InitialComment");
            }
        }

        private int? initialYear;
        public int? InitialYear
        {
            get { return initialYear; }
            set
            {
                initialYear = value;
                OnPropertyChanged("InitialYear");
            }
        }
        #endregion

        #region Metadata Properties
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
                {
                    file.Title = value;
                    Status = MusicItemStatus.Modified;
                }
                    
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
                {
                    file.Artist = value;
                    Status = MusicItemStatus.Modified;
                }
                    
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
                {
                    file.Kind = value;
                    Status = MusicItemStatus.Modified;
                }

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
                {
                    file.Album = value;
                    Status = MusicItemStatus.Modified;
                }

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
                {
                    file.Comment = value;
                    Status = MusicItemStatus.Modified;
                }
                    
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
                {
                    file.Year = value;
                    Status = MusicItemStatus.Modified;
                }
                    
                OnPropertyChanged("Year");
            }
        }
        #endregion

        #region Others Properties
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

        private bool isModified;
        /// <summary>
        /// Say if the metadata file are modified.
        /// </summary>
        public bool IsModified
        {
            get { return isModified; }
            set
            {
                isModified = value;
                OnPropertyChanged("IsModified");
            }
        }

        private MusicItemStatus status;
        /// <summary>
        /// Say if the metadata file are modified.
        /// </summary>
        public MusicItemStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private ObservableCollection<AnalyzeResult> results;
        public ObservableCollection<AnalyzeResult> Results
        {
            get { return results; }
            set
            {
                results = value;
                OnPropertyChanged("Results");
            }
        }
        #endregion

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
            {
                file.Save();

                InitialTitle = Title;
                InitialAlbum = Album;
                InitialArtist = Artist;
                InitialKind = Kind;
                InitialYear = Year;
                InitialComment = Comment;

                Title = "";
                Album = "";
                Artist = "";
                Kind = "";
                Year = null;
                Comment = "";
            }
        }

        /// <summary>
        /// Restore initial values
        /// </summary>
        public void Restore()
        {
            Title = InitialTitle;
            Artist = InitialArtist;
            Album = InitialAlbum;
            Kind = InitialKind;
            Year = InitialYear;
            Comment = InitialComment;
        }

        public void AddResult(AnalyzeResult newResult)
        {
            Results.Add(newResult);

            if(Results.Count > 1)
            {
                // Looking for the best and the worst result.
                AnalyzeResult bestResult = null;
                AnalyzeResult worstResult = null;
                int bestValue = int.MaxValue;
                int worstValue = int.MinValue;

                foreach (AnalyzeResult result in Results)
                {
                    result.ColorLevel = new SolidColorBrush(Colors.Orange);

                    if (result.MatchLevel < bestValue)
                    {
                        bestValue = result.MatchLevel;
                        bestResult = result;
                    }

                    if (result.MatchLevel > worstValue)
                    {
                        worstValue = result.MatchLevel;
                        worstResult = result;
                    }
                }

                if(bestResult != worstResult)
                {
                    if (bestResult != null)
                        bestResult.ColorLevel = new SolidColorBrush(Colors.Green);

                    if (worstResult != null)
                        worstResult.ColorLevel = new SolidColorBrush(Colors.Red);
                }
            }
        }

        /// <summary>
        /// Set the best result to metadata among all results;
        /// </summary>
        public void SetBestResult()
        {
            if (Results.Count > 0)
            {
                int b = int.MaxValue;
                AnalyzeResult bestResult = null;

                foreach (AnalyzeResult result in Results)
                {
                    if (result.MatchLevel < b)
                    {
                        b = result.MatchLevel;
                        bestResult = result;
                    }
                }

                if (bestResult != null)
                {
                    Title = bestResult.Music.Title;
                    Artist = bestResult.Music.Artist;
                    Album = bestResult.Music.Album;
                    Kind = bestResult.Music.Kind;
                    Year = bestResult.Music.Year;
                    Comment = bestResult.Music.Comment;

                    Status = MusicItemStatus.Modified;
                }
            }
        }
        #endregion
    }
}
