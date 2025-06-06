﻿using Sortzam.Lib.TagSDK;
using System;
using TagLib;
using Tools.Utils;

namespace Sortzam.Lib.DataAccessObjects
{
    /// <summary>
    /// Represent a music file
    /// </summary>
    public class MusicFileDao : MusicDao
    {
        #region Properties
        public string FileName { get; set; }
        public MusicFileExtension Extension { get; set; }
        public string Path { get; set; }
        #endregion


        public MusicFileDao(string pathFile)
        {
            Path = pathFile;

            if (string.IsNullOrEmpty(Path) || !FileUtils.Exists(Path))
                throw new System.IO.FileNotFoundException(string.Format("Can't find file : `{0}`", Path));

            FileName = System.IO.Path.GetFileName(pathFile);

            string extension = System.IO.Path.GetExtension(pathFile).Replace(".", "");

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
            Tag editor = File.Create(Path)?.Tag;

            if (editor.Performers != null && editor.Performers.Length > 0)
                Artist = editor.Performers.ToString("/", false);
            else
                Artist = FileName;

            if (!string.IsNullOrEmpty(editor.Title))
                Title = editor.Title;
            else
                Title = FileName;

            if (!string.IsNullOrEmpty(editor.Album))
                Album = editor.Album;

            if (editor.Genres != null && editor.Genres.Length > 0)
                Kind = editor.Genres.ToString("/", false);

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
            File editor = File.Create(Path);
            editor.Tag.Performers = Artist?.Split("/");
            editor.Tag.Title = Title;
            editor.Tag.Album = Album;
            editor.Tag.Genres = Kind?.Split("/");
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
            if (music == null)
                return;

            Album = music.Album;
            Artist = music.Artist;
            Comment = music.Comment;
            Kind = music.Kind;
            Title = music.Title;
            Year = music.Year;
        }
    }
}
