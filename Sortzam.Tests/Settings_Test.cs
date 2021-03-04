using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortzam.Lib.Detectors;
using Sortzam.Lib.UserSettings;
using System;
using System.IO;
using System.Linq;

namespace Sortzam.Tests
{
    [TestClass]
    public class Settings_Test
    {
        /// <summary>
        /// Testing the load of a SortZam Settings File load with a true file
        /// </summary>
        [TestMethod]
        public void GetInstance_ExistingSettings()
        {
            PreserveExistingSettingsFile(() =>
            {
                // Recreate it with test values
                var pathToTestFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "usersettings-test.szs");
                File.Copy(pathToTestFile, Settings.FILE_NAME);

                // Read the file
                Settings.Clear();
                var settings = Settings.GetInstance();
                Assert.IsNotNull(settings);
                Assert.IsNotNull(settings.ApiHost);
                Assert.IsNotNull(settings.ApiKey);
                Assert.IsNotNull(settings.SecretKey);
                Assert.AreEqual(settings.ApiHost, "apihost_test");
                Assert.AreEqual(settings.ApiKey, "apikey_test");
                Assert.AreEqual(settings.SecretKey, "secretkey_test");
                Assert.AreEqual(settings.UseAccount, false);
                return null;
            });
        }

        /// <summary>
        /// Testing the load of a SortZam Settings File load with an unfound file
        /// </summary>
        [TestMethod]
        public void GetInstance_UnexistingSettings()
        {
            PreserveExistingSettingsFile(() =>
            {
                Settings.Clear();
                var settings = Settings.GetInstance();
                Assert.IsNotNull(settings);
                Assert.AreEqual(settings.ApiHost, string.Empty);
                Assert.AreEqual(settings.ApiKey, string.Empty);
                Assert.AreEqual(settings.SecretKey, string.Empty);
                Assert.AreEqual(settings.UseAccount, false);
                return null;
            });
        }


        /// <summary>
        /// Testing the save of a SortZam Settings File if it doesn't exists yet
        /// </summary>
        [TestMethod]
        public void Save_UnexistentFile()
        {
            PreserveExistingSettingsFile(() =>
            {
                Settings.Clear();
                var settings = new Settings
                {
                    ApiHost = "apihost_test",
                    ApiKey = "apikey_test",
                    SecretKey = "secretkey_test"
                };
                settings.Save();

                var pathToTestFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "usersettings-test.szs");
                Assert.AreEqual(File.ReadAllText(Settings.FILE_NAME), File.ReadAllText(pathToTestFile));
                return null;
            });
        }

        /// <summary>
        /// Testing the save of a SortZam Settings File if it already exist
        /// </summary>
        [TestMethod]
        public void Save_OverrideFile()
        {
            PreserveExistingSettingsFile(() =>
            {
                // Recreate it with test values
                var pathToTestFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datas", "usersettings-test.szs");
                File.Copy(pathToTestFile, Settings.FILE_NAME);

                Settings.Clear();
                var settings = new Settings
                {
                    ApiHost = "apihost_test2",
                    ApiKey = "apikey_test2",
                    SecretKey = "secretkey_test2"
                };
                settings.Save();

                Settings.Clear();
                var settings2 = Settings.GetInstance();
                Assert.IsNotNull(settings2);
                Assert.IsNotNull(settings2.ApiHost);
                Assert.IsNotNull(settings2.ApiKey);
                Assert.IsNotNull(settings2.SecretKey);
                Assert.AreEqual(settings2.ApiHost, "apihost_test2");
                Assert.AreEqual(settings2.ApiKey, "apikey_test2");
                Assert.AreEqual(settings2.SecretKey, "secretkey_test2");
                Assert.AreEqual(settings2.UseAccount, false);
                return null;
            });
        }
        private void PreserveExistingSettingsFile(Func<string> p)
        {
            try
            {
                // Backup the true SortZamSettings file
                if (File.Exists(Settings.FILE_NAME))
                    File.Move(Settings.FILE_NAME, string.Concat(Settings.FILE_NAME, ".bak"));

                p.Invoke();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                if (File.Exists(string.Concat(Settings.FILE_NAME, ".bak")))
                {
                    File.Delete(Settings.FILE_NAME);
                    File.Move(string.Concat(Settings.FILE_NAME, ".bak"), Settings.FILE_NAME);
                }
            }
        }
    }
}
