using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools.Utils;

namespace Sortzam.Lib.Detectors
{
    /// <summary>
    /// Music File Searcher
    /// </summary>
    public class MusicFileDetector
    {
        private List<string> _extensions { get; set; }

        /// <summary>
        /// Instance a detector for searching Music File matching an extension list
        /// </summary>
        /// <param name="searchExtensions">Autorized extension list</param>
        public MusicFileDetector(IEnumerable<MusicFileExtension> searchExtensions = null)
        {
            var extensions = (searchExtensions?.ToList()) ?? EnumUtils.GetValues<MusicFileExtension>(typeof(MusicFileExtension));
            _extensions = extensions.Select(p => "." + p.ToString()).ToList();
        }

        /// <summary>
        /// Search for music file path in a directory and subdirectories, where the extension is known
        /// </summary>
        /// <param name="directoryPath">path to start research (recursively subdirectories)</param>
        /// <returns></returns>
        public IEnumerable<FileInfo> SearchPath(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException(string.Format("Can't find directory : '{0}'", directoryPath));
            var directory = new DirectoryInfo(directoryPath);

            var files = directory.GetAllFiles().Where(p => Path.GetExtension(p).ContainsOneOf(_extensions)).Select(p => new FileInfo(p));
            return (files.Count() <= 0) ? null : files;
        }

        /// <summary>
        /// Search for music file in a directory and subdirectories, where the extension is known
        /// </summary>
        /// <param name="directoryPath">path to start research (recursively subdirectories)</param>
        /// <returns></returns>
        public IEnumerable<MusicFileDao> SearchMusic(string directoryPath)
        {
            var search = SearchPath(directoryPath);
            if (search == null)
                return null;
            var result = new List<MusicFileDao>();
            foreach (var i in search.Where(p => p != null && p.Exists))
            {
                var music = new MusicFileDao(i.FullName);
                music.Load();
                result.Add(music);
            }
            return result;
        }
    }
}
