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
    public class MusicFileDetector_Test
    {
        /// <summary>
        /// Testing search music file from true file
        /// </summary>
        [TestMethod]
        public void SearchPath_RealFiles()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDetector().SearchPath(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Name.Contains("test1.mp3")));
            Assert.IsTrue(result.Count() >= 7);
        }

        /// <summary>
        /// Test search music file from unrecognizable true file
        /// </summary>
        [TestMethod]
        public void SearchPath_UnfoundExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wave }).SearchPath(data);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test search music file from corrupted file
        /// </summary>
        [TestMethod]
        public void SearchPath_EachExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.flac }).SearchPath(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.m4a }).SearchPath(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.mp3 }).SearchPath(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wav }).SearchPath(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wma }).SearchPath(data).Any());
        }

        /// <summary>
        /// Test search music file from unfound file
        /// </summary>
        [TestMethod]
        public void SearchPath_UnexistantDirectory()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "dfeghrtyhrghdfsghdrgfshrh");
            Assert.ThrowsException<DirectoryNotFoundException>(() =>
            {
                new MusicFileDetector().SearchPath(data);
            });
        }

        /// <summary>
        /// Testing search music file from true file
        /// </summary>
        [TestMethod]
        public void SearchMusic_RealFiles()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDetector().SearchMusic(data);
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
    }
}
