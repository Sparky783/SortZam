using IdSharp.Tagging.SimpleTag;
using Sortzam.Lib.Detectors;
using System;
using Tools.Utils;

namespace Sortzam.Lib.DataAccessObjects
{
    public class MusicFileDao : MusicDao
    {
        public MusicFileExtension Extension { get; set; }
        public string Path { get; set; }

        public MusicFileDao(string pathFile)
        {
            Path = pathFile;
            if (string.IsNullOrEmpty(Path) || !FileUtils.Exists(Path))
                throw new Exception(string.Format("Can't find file : `{0}`", Path));

            var extension = Path.Substring(Path.LastIndexOf('.'));
            MusicFileExtension extensionOut;
            if (Enum.TryParse(extension, true, out extensionOut) && Enum.IsDefined(typeof(MusicFileExtension), extensionOut))
                Extension = extensionOut;
            else
                throw new Exception(string.Format("Extension `{0}` is not supported", extension));
        }

        /// <summary>
        /// Charge track details from file meta datas ID3
        /// </summary>
        public void Load()
        {
            var tag = _loadTags();
            Album = tag.Album;
            Kind = tag.Genre;
            Artist = tag.Artist;
            Comment = tag.Comment;
            Title = tag.Title;
            int year;
            if (int.TryParse(tag.Year, out year))
                Year = year;
            int trackNumber;
            if (int.TryParse(tag.Year, out trackNumber))
                TrackNumber = trackNumber;
        }

        /// <summary>
        /// Save meta datas ID3 for the current file
        /// </summary>
        public void Save()
        {
            var tag = _loadTags();
            tag.Album = Album;
            tag.Genre = Kind;
            tag.Artist = Artist;
            tag.Comment = Comment;
            tag.Title = Title;
            tag.Year = Year?.ToString();
            tag.TrackNumber = TrackNumber?.ToString();
            tag.Save();
        }
        private SimpleTag _loadTags()
        {
            return new SimpleTag(Path);
        }
    }
}
