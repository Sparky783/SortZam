using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortzam.Lib.Detectors;
using Sortzam.Lib.UserSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sortzam.Tests
{
    [TestClass]
    public sealed class Settings_Test
    {
        /// <summary>
        /// Testing the default values of a settings class
        /// </summary>
        [TestMethod]
        public void GetInstanceUnexistingFile_DefaultValuesCheck()
        {
            PreserveExistingSettingsFile(() =>
            {
                if (File.Exists(Settings.FILE_NAME))
                    File.Delete(Settings.FILE_NAME);

                Settings.Clear();
                Settings settings = Settings.Instance;

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
        public void GetInstanceExistingFile_DefaultValuesCheck()
        {
            PreserveExistingSettingsFile(() =>
            {
                // Recreate file into the default directory
                File.Copy("datas/usersettings-test.szs", Settings.FILE_NAME);
                Settings.Clear();

                // Load it
                Settings settings = Settings.Instance;
                
                File.Delete(Settings.FILE_NAME);
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
        /// Testing the save of a SortZam Settings File if it doesn't exists yet
        /// </summary>
        [TestMethod]
        public void Save_UnexistentFile()
        {
            PreserveExistingSettingsFile(() =>
            {
                if (File.Exists(Settings.FILE_NAME))
                    File.Delete(Settings.FILE_NAME);

                Settings settings = Settings.Instance;

                settings.ApiHost = "apihost_test";
                settings.ApiKey = "apikey_test";
                settings.SecretKey = "secretkey_test";

                settings.Save();
                Assert.AreEqual(File.ReadAllText(Settings.FILE_NAME), File.ReadAllText("datas/usersettings-test.szs"));

                // Delete the settings file
                File.Delete(Settings.FILE_NAME);
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
                File.Copy("datas/usersettings-test.szs", Settings.FILE_NAME);

                // New settings with new values
                Settings settings = Settings.Instance;
                settings.ApiHost = "apihost_test2";
                settings.ApiKey = "apikey_test2";
                settings.SecretKey = "secretkey_test2";
                settings.Save();

                // Check if file is overrided
                Settings settings2 = Settings.Instance;

                Assert.IsNotNull(settings2);
                Assert.IsNotNull(settings2.ApiHost);
                Assert.IsNotNull(settings2.ApiKey);
                Assert.IsNotNull(settings2.SecretKey);
                Assert.AreEqual(settings2.ApiHost, "apihost_test2");
                Assert.AreEqual(settings2.ApiKey, "apikey_test2");
                Assert.AreEqual(settings2.SecretKey, "secretkey_test2");
                Assert.AreEqual(settings2.UseAccount, false);

                // Delete the settings file
                File.Delete(Settings.FILE_NAME);
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
                File.Copy("datas/usersettings-test.szs", Settings.FILE_NAME);

                // Check first access values
                Settings.Clear();
                Settings settings = Settings.Instance;

                Assert.IsNotNull(settings);
                Assert.IsNotNull(settings.ApiHost);
                Assert.IsNotNull(settings.ApiKey);
                Assert.IsNotNull(settings.SecretKey);
                Assert.AreEqual(settings.ApiHost, "apihost_test");
                Assert.AreEqual(settings.ApiKey, "apikey_test");
                Assert.AreEqual(settings.SecretKey, "secretkey_test");
                Assert.AreEqual(settings.UseAccount, false);

                // Delete the settings file
                File.Delete(Settings.FILE_NAME);

                // Check persistence 
                Settings settings2 = Settings.Instance;

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
