using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortzam.Lib.Detectors;
using System;
using System.IO;
using System.Linq;

namespace Sortzam.Tests
{
    [TestClass]
    public class MusicTagDetectorTest
    {
        public static string apiHost = "identify-eu-west-1.acrcloud.com";
        public static string apiKey = "ca88123807e49300eaea0fb9441c1bde";
        public static string secretKey = "ri9MAp8fXzEXu300Apch3Qj74Hadz2XiJbr9izox";

        [TestMethod]
        public void Search_returnShazamResult()
        {
            var data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "test1.mp3");
            var result = new MusicTagDetector(apiHost, apiKey, secretKey).Recognize(data);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(p => p.Title.Contains("I Like It")));
            Assert.IsTrue(result.Any(p => p.Artist.Contains("Enrique Iglesias")));
            Assert.IsTrue(result.Any(p => p.Album.Contains("Jersey Shore")));
            Assert.IsTrue(result.Any(p => p.Kind.Contains("Dance")));
            Assert.IsTrue(result.Any(p => p.Year == 2010));
        }
    }
}
