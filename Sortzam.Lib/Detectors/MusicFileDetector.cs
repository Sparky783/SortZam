using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools.Utils;

namespace Sortzam.Lib.Detectors
{
    /// <summary>
    /// Music path File Searcher
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
        public IEnumerable<FileInfo> SearchInDirectory(string directoryPath)
        {
            return SearchInDirectories(new List<string>() { directoryPath });
        }

        /// <summary>
        /// Search for music file path in one or more directories and subdirectories, where the extension is known
        /// </summary>
        /// <param name="directoriesPaths"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> SearchInDirectories(IEnumerable<string> directoriesPaths)
        {
            if (directoriesPaths == null || directoriesPaths.Any(p => string.IsNullOrEmpty(p)) || directoriesPaths.Any(p => !Directory.Exists(p)))
                throw new DirectoryNotFoundException("directoriesPaths parameter cannot be null or one of them is not found");

            var files = new List<FileInfo>();
            foreach (var dir in directoriesPaths)
            {
                files.AddRange(new DirectoryInfo(dir).GetAllFiles().Where(p => p.ContainsOneOf(_extensions)).Select(p => new FileInfo(p)));
            }
            return (files.Count() <= 0) ? null : files;
        }

        /// <summary>
        /// Search for music file path, where the extension is known
        /// </summary>
        /// <param name="directoryPath">path to start research (recursively subdirectories)</param>
        /// <returns></returns>
        public IEnumerable<FileInfo> SearchFiles(IEnumerable<string> pathFiles)
        {
            if (pathFiles == null)
                throw new Exception("pathFiles parameter cannot be null");

            var paths = pathFiles.Where(p => !string.IsNullOrEmpty(p) && FileUtils.Exists(p)).ToList();
            if (paths.Count <= 0)
                throw new FileNotFoundException(string.Format("Can't find files : '{0}'", paths.ToString(",", false)));

            var files = paths.Where(p => p.ContainsOneOf(_extensions)).Select(p => new FileInfo(p));
            return (files.Count() <= 0) ? null : files;
        }

        /// <summary>
        /// Search for direct music files path or files in a directory and subdirectories, where the extension is known
        /// </summary>
        /// <param name="pathsFilesOrDirectories">paths to files or to directories to start research (recursively subdirectories)</param>
        /// <returns></returns>
        public IEnumerable<FileInfo> Search(IEnumerable<string> pathsFilesOrDirectories)
        {
            if (pathsFilesOrDirectories == null 
                || pathsFilesOrDirectories.Any(p => string.IsNullOrEmpty(p)) 
                || pathsFilesOrDirectories.Any(p => !FileUtils.Exists(p) && !Directory.Exists(p)))
                throw new Exception("pathsFilesOrDirectories parameter cannot be null or empty or inexistent");

            var result = new List<FileInfo>();
            var directories = pathsFilesOrDirectories.Where(p => File.GetAttributes(p).HasFlag(FileAttributes.Directory));
            var files = pathsFilesOrDirectories.Where(p => !File.GetAttributes(p).HasFlag(FileAttributes.Directory));

            if (directories != null && directories.Count() > 0)
                result.AddRange(SearchInDirectories(directories));

            if (files != null && files.Count() > 0)
                result.AddRange(SearchFiles(files));
            return result;
        }
    }
}
