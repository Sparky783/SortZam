using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools.Utils;

namespace Sortzam.Lib.Detectors
{
    /// <summary>
    /// Music File Searcher And Metas Datas Loader
    /// </summary>
    public class MusicFileDaoDetector
    {
        private MusicFileDetector _musicFileDetector { get; set; }

        /// <summary>
        /// Instance a detector for searching Music File matching an extension list
        /// </summary>
        /// <param name="searchExtensions">Autorized extension list</param>
        public MusicFileDaoDetector(IEnumerable<MusicFileExtension> searchExtensions = null)
        {
            _musicFileDetector = new MusicFileDetector(searchExtensions);
        }

        /// <summary>
        /// Load Meta datas for music file in a directory and subdirectories, where the extension is known
        /// </summary>
        /// <param name="directoryPath">path to start research (recursively subdirectories)</param>
        /// <returns></returns>
        public IEnumerable<MusicFileDao> SearchInDirectory(string directoryPath)
        {
            return SearchInDirectories(new List<string>() { directoryPath });
        }

        /// <summary>
        /// Load Meta datas for music file in one or more directories and subdirectories, where the extension is known
        /// </summary>
        /// <param name="directoriesPath">one or more path to start research (recursively subdirectories)</param>
        /// <returns></returns>
        public IEnumerable<MusicFileDao> SearchInDirectories(List<string> directoriesPath)
        {
            return LoadMetaDatas(_musicFileDetector.SearchInDirectories(directoriesPath));
        }

        /// <summary>
        /// Load Meta datas for music file path, where the extension is known
        /// </summary>
        /// <param name="directoryPath">path to start research (recursively subdirectories)</param>
        /// <returns></returns>
        public IEnumerable<MusicFileDao> SearchFiles(List<string> pathFiles)
        {
            return LoadMetaDatas(_musicFileDetector.SearchFiles(pathFiles));
        }

        /// <summary>
        /// Load Meta datas for music file path or files in a directory and subdirectories, where the extension is known
        /// </summary>
        /// <param name="pathFiles"></param>
        /// <returns></returns>
        public IEnumerable<MusicFileDao> Search(List<string> pathFilesOrDirectories)
        {
            return LoadMetaDatas(_musicFileDetector.Search(pathFilesOrDirectories));
        }



        private IEnumerable<MusicFileDao> LoadMetaDatas(IEnumerable<FileInfo> pathFiles)
        {
            if (pathFiles == null)
                return null;
            var result = new List<MusicFileDao>();
            foreach (var i in pathFiles.Where(p => p != null && p.Exists))
            {
                var music = new MusicFileDao(i.FullName);
                try
                {
                    music.Load();
                    result.Add(music);
                }
                catch (Exception e) { } // IF metas datas are not foundable or if file is corrupted
            }
            return result;
        }
    }
}
