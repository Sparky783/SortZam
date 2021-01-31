using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tools.Utils;

namespace Sortzam.Lib.Detector
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
        public DirectoryInfo Directory { get; set; }
        public List<MusicFileExtension> Extensions { get; set; }
        public string DirectoryPath { get { return Directory.FullName; } }

        public MusicDetector(string directoryPath, IEnumerable<MusicFileExtension> searchExtensions = null)
        {
            if (string.IsNullOrEmpty(directoryPath))
                throw new Exception("Directory to analyze can't be null or empty");
            Directory = new DirectoryInfo(directoryPath);

            Extensions = (searchExtensions?.ToList()) ?? EnumUtils.GetValues<MusicFileExtension>(typeof(MusicFileExtension));
        }

        public IEnumerable<FileInfo> GetFiles()
        {

            throw new NotImplementedException();
        }
    }
}
