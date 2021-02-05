using Sortzam.Lib.TagSDK;
using System;
using TagLib;
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
                throw new System.IO.FileNotFoundException(string.Format("Can't find file : `{0}`", Path));

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
            var editor = File.Create(Path)?.Tag;
            if (editor.Performers != null && editor.Performers.Length > 0)
                Artist = editor.Performers.ToString(" & ", false);
            else Artist = FileName;
            if (!string.IsNullOrEmpty(editor.Title))
                Title = editor.Title;
            else Title = FileName;
            if (!string.IsNullOrEmpty(editor.Album))
                Album = editor.Album;
            if (editor.Genres != null && editor.Genres.Length > 0)
                Kind = editor.Genres.ToString(" & ", false);
            if (!string.IsNullOrEmpty(editor.Comment))
                Comment = editor.Comment;
            if (editor.Year > 0)
                Year = (int)editor.Year;
        }

        /// <summary>
        /// Save meta datas ID3 into file details, for the current file
        /// </summary>
        public void Save()
        {
            var editor = File.Create(Path);
            editor.Tag.Performers = Artist?.Split(" & ");
            editor.Tag.Title = Title;
            editor.Tag.Album = Album;
            editor.Tag.Genres = Kind?.Split(" & ");
            editor.Tag.Comment = Comment;
            editor.Tag.Year = (uint)(Year ?? 0);
            editor.Save();
        }

        /// <summary>
        /// Map MusicDao tags into the current MusicFileDao
        /// </summary>
        /// <param name="music"></param>
        public void Map(MusicDao music)
        {
            if (music == null) return;
            this.Album = music.Album;
            this.Artist = music.Artist;
            this.Comment = music.Comment;
            this.Kind = music.Kind;
            this.Title = music.Title;
            this.Year = music.Year;
        }
    }
}
