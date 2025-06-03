using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortzam.Ihm.Models;
using Sortzam.Lib.Detectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sortzam.Tests
{
    [TestClass]
    public class MusicTagDetector_Test
    {
        /// <summary>
        /// Testing recognition music from true mp3 file
        /// </summary>
        [TestMethod]
        public void Recognize_TrueFile_mp3()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "test1.mp3");
            var result = new MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Title.Contains("I Like It")));
            Assert.IsTrue(result.Any(p => p.Artist.Contains("Enrique Iglesias")));
            Assert.IsTrue(result.Any(p => p.Album.Contains("Jersey Shore")));
            Assert.IsTrue(result.Any(p => p.Kind.Contains("Dance")));
            Assert.IsTrue(result.Any(p => p.Year == 2010));
        }

        /// <summary>
        /// Testing recognition music from true file
        /// </summary>
        [TestMethod]
        public void Recognize_TrueFile_wav()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "subdirectory", "Tout le monde Danse.wav");
            var result = new MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Title.Contains("Tout le monde danse")));
            Assert.IsTrue(result.Any(p => p.Artist.Contains("Fally Ipupa")));
            Assert.IsTrue(result.Any(p => p.Album.Contains("Tout le monde danse")));
            Assert.IsTrue(result.Any(p => p.Kind.Contains("Rap")));
            Assert.IsTrue(result.Any(p => p.Year == 0));
        }

        /// <summary>
        /// Testing recognition music from true file
        /// </summary>
        [TestMethod]
        public void Recognize_TrueFile_m4a()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "subdirectory", "05 Meet Her At The Loveparade.m4a");
            var result = new MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Title.Contains("Meet Her At The Loveparade")));
            Assert.IsTrue(result.Any(p => p.Artist.Contains("Da Hool")));
            Assert.IsTrue(result.Any(p => p.Album.Contains("Meet Her At The Loveparade")));
            Assert.IsTrue(result.Any(p => p.Kind.Contains("Dance")));
            Assert.IsTrue(result.Any(p => p.Year == 2009));
        }

        /// <summary>
        /// Testing recognition music from true file
        /// </summary>
        [TestMethod]
        public void Recognize_TrueFile_flac()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "subdirectory", "subsubdirectory", "Robin Schulz - In Your Eyes (feat. Alida).flac");
            var result = new MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Title.Contains("In Your Eyes")));
            Assert.IsTrue(result.Any(p => p.Artist.Contains("Alida/Robin Schulz")));
            Assert.IsTrue(result.Any(p => p.Album.Contains("In Your Eyes")));
            Assert.IsTrue(result.Any(p => p.Year == 2020));
        }

        /// <summary>
        /// Testing recognition music from true file
        /// </summary>
        [TestMethod]
        public void Recognize_TrueFile_wma()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "subdirectory", "Taio Cruz - Break Your Heart.wma");
            var result = new MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Title.Contains("Break Your Heart")));
            Assert.IsTrue(result.Any(p => p.Artist.Contains("Taio Cruz")));
            Assert.IsTrue(result.Any(p => p.Album.Contains("Break Your Heart")));
            Assert.IsTrue(result.Any(p => p.Kind.Contains("Pop")));
            Assert.IsTrue(result.Any(p => p.Year == 2009));
        }

        /// <summary>
        /// Test recognition music from unrecognizable true file
        /// </summary>
        [TestMethod]
        public void Recognize_UnrecognizableFile()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "test2.mp3");
            var result = new MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test recognition music from corrupted file
        /// </summary>
        [TestMethod]
        public void Recognize_CorruptedFile()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "test3.mp3");
            Assert.ThrowsException<Exception>((Action)(() =>
            {
                new Lib.Detectors.MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            }));
        }

        /// <summary>
        /// Test recognition music from unfound file
        /// </summary>
        [TestMethod]
        public void Recognize_UnfoundFile()
        {
            Settings settings = Settings.Instance;

            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "dfeghrtyhrghdfsghdrgfshrh.mp3");
            Assert.ThrowsException<FileNotFoundException>((Action)(() =>
            {
                new Lib.Detectors.MusicTagDetector(settings.ApiHost, settings.ApiKey, settings.SecretKey).Recognize(data);
            }));
        }
    }
}
