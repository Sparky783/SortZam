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
        /// Testing the default values of a settings class
        /// </summary>
        [TestMethod]
        public void Constructor_DefaultValuesCheck()
        {
            PreserveExistingSettingsFile(() =>
            {
                var settings = new Settings();
                Assert.IsNotNull(settings);
                Assert.AreEqual(settings.ApiHost, string.Empty);
                Assert.AreEqual(settings.ApiKey, string.Empty);
                Assert.AreEqual(settings.SecretKey, string.Empty);
                Assert.AreEqual(settings.UseAccount, false);
                return null;
            });
        }

        /// <summary>
        /// Testing the load of a SortZam Settings File load with a true file
        /// </summary>
        [TestMethod]
        public void Load_ExistingSettings()
        {
            PreserveExistingSettingsFile(() =>
            {
                // Recreate file into the default directory
                File.Copy("datas/usersettings-test.szs", MySettings.FILE_NAME);

                // Load it
                var settings = new Settings();
                settings.Load();
                File.Delete(MySettings.FILE_NAME);
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
        public void Load_UnexistingSettings()
        {
            PreserveExistingSettingsFile(() =>
            {
                if (File.Exists(MySettings.FILE_NAME))
                    File.Delete(MySettings.FILE_NAME);
                var settings = new Settings();
                settings.Load();
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
                var settings = new Settings
                {
                    ApiHost = "apihost_test",
                    ApiKey = "apikey_test",
                    SecretKey = "secretkey_test"
                };
                settings.Save();
                Assert.AreEqual(File.ReadAllText(MySettings.FILE_NAME), File.ReadAllText("datas/usersettings-test.szs"));

                // Delete the settings file
                File.Delete(MySettings.FILE_NAME);
                return null;
            });
        }

        /// <summary>
        /// Testing the save of a SortZam Settings File if it already exist, overriding it
        /// </summary>
        [TestMethod]
        public void Save_OverrideFile()
        {
            PreserveExistingSettingsFile(() =>
            {
                // Recreate it with test initials values
                File.Copy("datas/usersettings-test.szs", MySettings.FILE_NAME);

                // New settings with new values
                var settings = new Settings
                {
                    ApiHost = "apihost_test2",
                    ApiKey = "apikey_test2",
                    SecretKey = "secretkey_test2"
                };
                settings.Save();

                // Check if file is overrided
                var settings2 = new Settings();
                settings2.Load();
                Assert.IsNotNull(settings2);
                Assert.IsNotNull(settings2.ApiHost);
                Assert.IsNotNull(settings2.ApiKey);
                Assert.IsNotNull(settings2.SecretKey);
                Assert.AreEqual(settings2.ApiHost, "apihost_test2");
                Assert.AreEqual(settings2.ApiKey, "apikey_test2");
                Assert.AreEqual(settings2.SecretKey, "secretkey_test2");
                Assert.AreEqual(settings2.UseAccount, false);

                // Delete the settings file
                File.Delete(MySettings.FILE_NAME);
                return null;
            });
        }

        /// <summary>
        /// Testing the load of a SortZam Settings File load with a true file
        /// </summary>
        [TestMethod]
        public void GetInstance_PersistenceTest()
        {
            PreserveExistingSettingsFile(() =>
            {
                // Recreate it with test values
                File.Copy("datas/usersettings-test.szs", MySettings.FILE_NAME);

                // Check first access values
                MySettings.Clear();
                var settings = MySettings.GetInstance();
                Assert.IsNotNull(settings);
                Assert.IsNotNull(settings.ApiHost);
                Assert.IsNotNull(settings.ApiKey);
                Assert.IsNotNull(settings.SecretKey);
                Assert.AreEqual(settings.ApiHost, "apihost_test");
                Assert.AreEqual(settings.ApiKey, "apikey_test");
                Assert.AreEqual(settings.SecretKey, "secretkey_test");
                Assert.AreEqual(settings.UseAccount, false);

                // Delete the settings file
                File.Delete(MySettings.FILE_NAME);

                // Check persistence 
                var settings2 = MySettings.GetInstance();
                Assert.IsNotNull(settings2);
                Assert.IsNotNull(settings2.ApiHost);
                Assert.IsNotNull(settings2.ApiKey);
                Assert.IsNotNull(settings2.SecretKey);
                Assert.AreEqual(settings2.ApiHost, "apihost_test");
                Assert.AreEqual(settings2.ApiKey, "apikey_test");
                Assert.AreEqual(settings2.SecretKey, "secretkey_test");
                Assert.AreEqual(settings2.UseAccount, false);
                return null;
            });
        }

        private void PreserveExistingSettingsFile(Func<string> p)
        {
            try
            {
                // Backup the true SortZamSettings file
                if (File.Exists(MySettings.FILE_NAME))
                    File.Move(MySettings.FILE_NAME, string.Concat(MySettings.FILE_NAME, ".bak"));

                p.Invoke();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                if (File.Exists(string.Concat(MySettings.FILE_NAME, ".bak")))
                {
                    File.Delete(MySettings.FILE_NAME);
                    File.Move(string.Concat(MySettings.FILE_NAME, ".bak"), MySettings.FILE_NAME);
                }
            }
        }
    }
}
