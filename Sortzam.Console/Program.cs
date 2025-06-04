using Sortzam.Ihm.Models;
using Sortzam.Lib;
using Sortzam.Lib.ACRCloudSDK;
using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools.Comparer;
using Tools.Utils;

namespace Sortzam.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tap a number to select a function :");
            Console.WriteLine("1 -> Analyze a directory");
            Console.WriteLine("2 -> Rewrite Artists tag using featuring field");

            string choice = Console.ReadLine();

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
            Settings settings = Settings.Instance;

            Console.WriteLine("Please enter the directory path to analyze : ");
            string path = PathUtils.Normalize(Console.ReadLine());

            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                throw new Exception("Not found path");

            IEnumerable<MusicFileDao> musicFiles = new MusicFileSearcher().Search(new List<string>() { path });

            foreach (MusicFileDao musicFile in musicFiles)
            {
                IEnumerable<MusicDao> tags = null;

                try
                {
                    tags = new ACRCloudDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(musicFile.Path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("File audio error : " + musicFile.Path);
                    Console.WriteLine(e.Message);
                    continue;
                }

                if (tags == null || tags.Count() <= 0)
                {
                    Console.WriteLine("File not recognized : " + musicFile.Path);
                    continue;
                }

                Console.WriteLine("File recognized : " + musicFile.Path);

                if (tags.Count() == 1)
                {
                    musicFile.Map(tags.First());
                }
                else
                {
                    int nbMin = int.MaxValue;
                    MusicDao tagMin = null;

                    foreach (MusicDao tag in tags)
                    {
                        int nb = LevenshteinDistance.Compute(musicFile.FileName, string.Format("{0} - {1}", tag.Artist, tag.Title));

                        if (nb < nbMin)
                        {
                            nbMin = nb;
                            tagMin = tag;
                        }
                    }

                    musicFile.Map(tagMin);
                }

                musicFile.Save();
            }
        }

        static void RewriteArtist()
        {
            Console.WriteLine("Please enter the directory path to analyze : ");
            string path = PathUtils.Normalize(Console.ReadLine());

            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                throw new Exception("Not found path");

            List<string> searchValues = new List<string>() {
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

            IEnumerable<MusicFileDao> musicFiles = new MusicFileSearcher().Search(new List<string>() { path });
            List<MusicFileDao> musicFilesErrors = musicFiles.Where(p => searchValues.Any(k => p.Artist.Contains(k))).ToList();

            foreach (MusicFileDao musicFile in musicFilesErrors)
            {
                Console.Clear();
                Console.WriteLine("Choose a proposition OR tap 0 to enter it yourself OR leave empty to continue next :");
                Console.WriteLine("");
                Console.WriteLine(string.Format("Original : {0} - {1}", musicFile.Artist, musicFile.Title));

                // Artist modelize
                string artist = musicFile.Artist;

                while (searchValues.Any(value => artist.Contains(value)))
                {
                    List<string> artistList = new List<string>() { };

                    foreach (string res in artist.Split("/"))
                    {
                        bool found = false;

                        foreach (string search in searchValues)
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

                    artist = artistList.Where(a => !string.IsNullOrEmpty(a)).Select(a => a.Trim()).Distinct().ToString("/", false);
                }

                Console.WriteLine(string.Format("Nouveau : {0} - {1}", artist, musicFile.Title));
                string choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                    continue;

                if (choice == "0")
                {
                    Console.WriteLine("Artist : ");
                    musicFile.Artist = Console.ReadLine();
                    musicFile.Save();
                }

                if (choice == "1")
                {
                    musicFile.Artist = artist;
                    musicFile.Save();
                }
            }
        }
    }
}
