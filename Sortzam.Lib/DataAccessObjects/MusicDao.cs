using Id3;
using Sortzam.Lib.Detectors;
using System;
using Tools.Utils;

namespace Sortzam.Lib.DataTransferObjects
{
    public class MusicDto
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public MusicFileExtension Extension { get; set; }
        public string Kind { get; set; }
        public string Album { get; set; }
        public string Path { get; set; }
        public int Year { get; set; }
        public MusicDto() { }
        public void Save()
        {
            if (!FileUtils.Exists(Path))
                throw new System.Exception(string.Format("Can't find file : '{0}'", Path));
            throw new NotImplementedException();
        }
    }
}
