using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public IEnumerable<FileInfo> GetMusicFiles(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                throw new Exception("Directory to analyze can't be null or empty");
            var directory = new DirectoryInfo(directoryPath);

            return directory.GetAllFiles().Where(p => p.ContainsOneOf(_extensions)).Select(p => new FileInfo(p));
        }
    }
}
