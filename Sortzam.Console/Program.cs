using Sortzam.Lib.DataAccessObjects;
using Sortzam.Lib.Detectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools.Utils;

namespace Sortzam.Cmd
{
    class Program
    {
        public static string apiHost = "identify-eu-west-1.acrcloud.com";
        public static string apiKey = "ca88123807e49300eaea0fb9441c1bde";
        public static string secretKey = "ri9MAp8fXzEXu300Apch3Qj74Hadz2XiJbr9izox";
        static void Main(string[] args)
        {
            Console.WriteLine("Entrez le chemin vers le fichier iTunes Music Library.xml :");
            var path = Console.ReadLine();
            if (string.IsNullOrEmpty(path))
                throw new Exception("Not found path");

            var files = new MusicFileDaoDetector().Search(new List<string>() { path });
            foreach (var i in files)
            {
                var tag = new MusicTagDetector(apiHost, apiKey, secretKey).Recognize(i.Path);
                if (tag == null || tag.Count() <= 0)
                {
                    Console.WriteLine("File not recognized : " + i.Path);
                    break;
                }
                Console.WriteLine("File recognized : " + i.Path);
                var bck = Path.GetFileNameWithoutExtension(i.Path) + "-old" + Path.GetExtension(i.Path);
                if (!File.Exists(bck))
                    File.Copy(i.Path, bck);
                i.Map(tag.First());
                i.Save();
            }
        }
    }
}
