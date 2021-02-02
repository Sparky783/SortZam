using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools.Utils;

namespace Sortzam.Lib.Detectors
{
    public class MusicFileDetector
    {
        private List<string> _extensions { get; set; }

        public MusicFileDetector(IEnumerable<MusicFileExtension> searchExtensions = null)
        {
            var extensions = (searchExtensions?.ToList()) ?? EnumUtils.GetValues<MusicFileExtension>(typeof(MusicFileExtension));
            _extensions = extensions.Select(p => "." + p.ToString()).ToList();
        }

        public IEnumerable<FileInfo> Search(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException(string.Format("Can't find directory : '{0}'", directoryPath));
            var directory = new DirectoryInfo(directoryPath);

            var files = directory.GetAllFiles().Where(p => p.ContainsOneOf(_extensions)).Select(p => new FileInfo(p));
            return (files.Count() <= 0) ? null : files;
        }
    }
}
