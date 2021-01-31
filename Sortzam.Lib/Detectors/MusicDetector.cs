using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tools.Utils;

namespace Sortzam.Lib.Detectors
{
    public enum MusicFileExtension
    {
        mp3 = 0,        // MPEG Audio Layer III
        wav = 1,        // Waveform Audio File Format
        wave = 1,       // La vague wesh
        m4a = 2,        // Apple Lossless Audio Codec
        flac = 3,       // Free Lossless Audio Codec
        wma = 4,        // Windows Media Audio
    }
    public class MusicDetector
    {
        private List<string> _extensions { get; set; }

        public MusicDetector(IEnumerable<MusicFileExtension> searchExtensions = null)
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
