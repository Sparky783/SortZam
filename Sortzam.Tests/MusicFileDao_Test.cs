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
    public class MusicFileDao_Test
    {
        /// <summary>
        /// Testing search music file from true file
        /// </summary>
        [TestMethod]
        public void Load_IsOk()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "test1.mp3");
            var result = new MusicFileDao(data);
            Assert.IsNotNull(result);

            // testing loading metas
            result.Load();
            Assert.AreEqual(result.Title, "I Like It (Featuring Pitbull)");
        }

        [TestMethod]
        public void Load_WrongFile()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "dfsghdfghjdgfhj.mp3");
            Assert.ThrowsException<FileNotFoundException>(() =>
            {
                new MusicFileDao(data);
            });
        }

        [TestMethod]
        public void Load_WrongExtension()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "test1.txt");
            Assert.ThrowsException<Exception>(() =>
            {
                new MusicFileDao(data);
            });
        }

        [TestMethod]
        public void Save_IsOk()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "test1.mp3");
            var result = new MusicFileDao(data);
            result.Load();
            var oldTitle = result.Title;

            // testing override metas
            result.Title = "test1";
            result.Save();

            // reloading to see if save is ok
            var result2 = new MusicFileDao(data);
            result2.Load();
            Assert.AreEqual(result2.Title, "test1");

            // back to old value
            result2.Title = oldTitle;
            result2.Save();
        }
    }
}
