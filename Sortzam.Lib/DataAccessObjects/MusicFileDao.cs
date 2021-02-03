using IdSharp.Tagging.SimpleTag;
using System;
using Tools.Utils;

namespace Sortzam.Lib.DataAccessObjects
{
    public class MusicFileDao : MusicDao
    {
        public string FileName { get; set; }
        public MusicFileExtension Extension { get; set; }
        public string Path { get; set; }

        public MusicFileDao(string pathFile)
        {
            Path = pathFile;
            if (string.IsNullOrEmpty(Path) || !FileUtils.Exists(Path))
                throw new Exception(string.Format("Can't find file : `{0}`", Path));

            FileName = System.IO.Path.GetFileName(pathFile);

            var extension = System.IO.Path.GetExtension(pathFile).Replace(".", "");
            if (Enum.TryParse(extension, true, out MusicFileExtension extensionOut) && Enum.IsDefined(typeof(MusicFileExtension), extensionOut))
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
            if (!string.IsNullOrEmpty(tag.Artist))
                Artist = tag.Artist;
            else Artist = FileName;
            if (!string.IsNullOrEmpty(tag.Title))
                Title = tag.Title;
            else Title = FileName;
            Album = tag.Album;
            Kind = tag.Genre;
            Comment = tag.Comment;
            if (int.TryParse(tag.Year, out int year))
                Year = year;
        }

        /// <summary>
        /// Save meta datas ID3 into file details, for the current file
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
            tag.Save();
        }
        private SimpleTag _loadTags()
        {
            return new SimpleTag(Path);
        }
    }
}
