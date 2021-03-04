using Sortzam.Lib.DataAccessObjects;
using Sortzam.Lib.Detectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Tools.Comparer;
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
            Console.WriteLine("Tap a number to select a function :");
            Console.WriteLine("1 -> Analyze a directory");
            Console.WriteLine("2 -> Rewrite Artists tag using featuring field");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": Analyze(); break;
                case "2": RewriteArtist(); break;
                default: Console.WriteLine("Command unknow"); break;
            }
            Console.ReadLine();
        }
        static void Analyze()
        {
            Console.WriteLine("Please enter the directory path to analyze : ");
            var path = PathUtils.Normalize(Console.ReadLine());
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                throw new Exception("Not found path");

            var files = new MusicFileDaoDetector().Search(new List<string>() { path });
            foreach (var i in files)
            {
                IEnumerable<MusicDao> tag = null;
                try
                {
                    tag = new MusicTagDetector(apiHost, apiKey, secretKey).Recognize(i.Path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("File audio error : " + i.Path);
                    Console.WriteLine(e.Message);
                    continue;
                }
                if (tag == null || tag.Count() <= 0)
                {
                    Console.WriteLine("File not recognized : " + i.Path);
                    continue;
                }
                Console.WriteLine("File recognized : " + i.Path);

                if (tag.Count() == 1)
                    i.Map(tag.First());
                else
                {
                    int nbMin = int.MaxValue;
                    MusicDao tagMin = null;
                    foreach (var j in tag)
                    {
                        var nb = LevenshteinDistance.Compute(i.FileName, string.Format("{0} - {1}", j.Artist, j.Title));
                        if (nb < nbMin)
                        {
                            nbMin = nb;
                            tagMin = j;
                        }
                    }
                    i.Map(tagMin);
                }
                i.Save();
            }
        }

        static void RewriteArtist()
        {
            Console.WriteLine("Please enter the directory path to analyze : ");
            var path = PathUtils.Normalize(Console.ReadLine());
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                throw new Exception("Not found path");

            var searchValues = new List<string>() {
                " featuring. ",
                " featuring ",
                " Feat. ",
                " Feat ",
                " feat. ",
                " feat ",
                " And ",
                " and ",
                " ft. ",
                " vs. ",
                " ft ",
                " Ft. ",
                " Ft ",
                " & ",
                " X ",
                " x ",
                ", ",
            };

            var files = new MusicFileDaoDetector().Search(new List<string>() { path });
            var filesErrors = files.Where(p => searchValues.Any(k => p.Artist.Contains(k))).ToList();

            foreach (var file in filesErrors)
            {
                Console.Clear();
                Console.WriteLine("Choose a proposition OR tap 0 to enter it yourself OR leave empty to continue next :");
                Console.WriteLine("");
                Console.WriteLine(string.Format("Original : {0} - {1}", file.Artist, file.Title));

                // Artist modelize
                var artist = file.Artist;
                while (searchValues.Any(p => artist.Contains(p)))
                {
                    var artistList = new List<string>() { };
                    foreach (var res in artist.Split("/"))
                    {
                        var found = false;
                        foreach (var search in searchValues)
                        {
                            if (res.Contains(search))
                            {
                                found = true;
                                artistList.AddRange(res.Split(search));
                            }
                        }
                        if (!found)
                            artistList.Add(res);
                    }
                    artist = artistList.Where(p => !string.IsNullOrEmpty(p)).Select(p => p.Trim()).Distinct().ToString("/", false);
                }

                Console.WriteLine(string.Format("Nouveau : {0} - {1}", artist, file.Title));
                var choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                    continue;

                if (choice == "0")
                {
                    Console.WriteLine("Artist : ");
                    file.Artist = Console.ReadLine();
                    file.Save();
                }
                if (choice == "1")
                {
                    file.Artist = artist;
                    file.Save();
                }
            }
        }
    }
}
