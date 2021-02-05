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
            var result = new MusicFileDetector().SearchInDirectory(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.FullName.Contains("test1.mp3")));
            Assert.IsTrue(result.Count() >= 7);
        }

        /// <summary>
        /// Test search music file from unrecognizable true file
        /// </summary>
        [TestMethod]
        public void SearchPath_UnfoundExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wave }).SearchInDirectory(data);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test search music file from corrupted file
        /// </summary>
        [TestMethod]
        public void SearchPath_EachExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.flac }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.m4a }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.mp3 }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wav }).SearchInDirectory(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wma }).SearchInDirectory(data).Any());
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
                new MusicFileDetector().SearchInDirectory(data);
            });
        }
    }
}
