using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortzam.Lib.DataAccessObjects;
using Sortzam.Lib.Detectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sortzam.Tests
{
    [TestClass]
    public class MusicFileDaoDetector_Test
    {
        /// <summary>
        /// Testing search music file from true file
        /// </summary>
        [TestMethod]
        public void SearchMusic_RealFiles()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDaoDetector().SearchInDirectory(data);
            Assert.IsNotNull(result);
            var test = result.SingleOrDefault(p => p.FileName.Contains("test1.mp3"));
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Extension == MusicFileExtension.mp3);
            Assert.IsTrue(test.Title.Contains("I Like It"));
            Assert.IsTrue(test.Kind.Contains("Pop"));
            Assert.IsTrue(test.Year == 2010);
            Assert.IsTrue(test.Artist.Contains("Enrique Iglesias"));
            Assert.IsTrue(test.Album.Contains("I Like It Single"));
        }

        /// <summary>
        /// Test search music file from unrecognizable true file
        /// </summary>
        [TestMethod]
        public void SearchMusic_UnfoundExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDaoDetector(new List<MusicFileExtension>() { MusicFileExtension.wave }).SearchInDirectory(data);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test search music file from corrupted file
        /// </summary>
        [TestMethod]
        public void SearchMusic_EachExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            Assert.IsTrue(new MusicFileDaoDetector(new List<MusicFileExtension>() { MusicFileExtension.flac }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDaoDetector(new List<MusicFileExtension>() { MusicFileExtension.m4a }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDaoDetector(new List<MusicFileExtension>() { MusicFileExtension.mp3 }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDaoDetector(new List<MusicFileExtension>() { MusicFileExtension.wav }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDaoDetector(new List<MusicFileExtension>() { MusicFileExtension.wma }).SearchInDirectory(data).Any());
        }

        /// <summary>
        /// Test search music file from unfound file
        /// </summary>
        [TestMethod]
        public void SearchMusic_UnexistantDirectory()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "dfeghrtyhrghdfsghdrgfshrh");
            Assert.ThrowsException<DirectoryNotFoundException>(() =>
            {
                new MusicFileDaoDetector().SearchInDirectory(data);
            });
        }
    }
}