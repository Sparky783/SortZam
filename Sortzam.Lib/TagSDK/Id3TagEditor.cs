using System;
using TagLib;
using Tools.Utils;

namespace Sortzam.Lib.TagSDK
{
    public class Id3TagEditor
    {
        private string _path;
        private File _file;
        public Id3TagEditor(string path)
        {
            if (string.IsNullOrEmpty(path) || !FileUtils.Exists(path))
                throw new System.IO.FileNotFoundException("File not Found", path);
            _path = path;
            _file = File.Create(_path);
        }
        public Tag Load()
        {
            return _file?.Tag;
        }

        internal void Save(string[] artist, string title, string album, string[] kind, string comment, int year)
        {
        }
    }
}
