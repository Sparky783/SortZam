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
        /// Testing recognition music from true file
        /// </summary>
        [TestMethod]
        public void Search_RealFiles()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDetector().Search(data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Name.Contains("test1.mp3")));
            Assert.IsTrue(result.Count() >= 7);
        }

        /// <summary>
        /// Test recognition music from unrecognizable true file
        /// </summary>
        [TestMethod]
        public void Search_UnfoundExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            var result = new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wave }).Search(data);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test recognition music from corrupted file
        /// </summary>
        [TestMethod]
        public void Search_EachExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas");
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.flac }).Search(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.m4a }).Search(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.mp3 }).Search(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wav }).Search(data).Any());
            Assert.IsTrue(new MusicFileDetector(new List<MusicFileExtension>() { MusicFileExtension.wma }).Search(data).Any());
        }

        /// <summary>
        /// Test recognition music from unfound file
        /// </summary>
        [TestMethod]
        public void Search_UnexistantDirectory()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "dfeghrtyhrghdfsghdrgfshrh");
            Assert.ThrowsException<DirectoryNotFoundException>(() =>
            {
                new MusicFileDetector().Search(data);
            });
        }
    }
}
