using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortzam.Ihm.Models;
using System;
using System.IO;

namespace Sortzam.Tests
{
    [TestClass]
    public sealed class Settings_Test
    {
        static private bool useAccountBak;
        static private string apiHostBak;
        static private string apiKeyBak;
        static private string secretKeyBak;

        /// <summary>
        /// Testing the default values of a Settings class
        /// </summary>
        [TestMethod]
        public void GetInstance_DefaultValuesCheck()
        {
            SaveUserSettings();

            Settings settings = Settings.Instance;

            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.UseAccount, false);
            Assert.AreEqual(settings.ApiHost, string.Empty);
            Assert.AreEqual(settings.ApiKey, string.Empty);
            Assert.AreEqual(settings.SecretKey, string.Empty);

            RestoreUserSettings();
        }

        /// <summary>
        /// Testing to set user Settings
        /// </summary>
        [TestMethod]
        public void GetInstance_SetValues()
        {
            SaveUserSettings();

            // Force values
            Settings settings = Settings.Instance;
            settings.UseAccount = true;
            settings.ApiHost = "apihost_test";
            settings.ApiKey = "apikey_test";
            settings.SecretKey = "secretkey_test";
            settings.Save();

            // Clear settings instance and get a new instance
            Settings.Clear();
            settings = Settings.Instance;

            Assert.IsNotNull(settings);
            Assert.IsNotNull(settings.UseAccount);
            Assert.IsNotNull(settings.ApiHost);
            Assert.IsNotNull(settings.ApiKey);
            Assert.IsNotNull(settings.SecretKey);

            Assert.AreEqual(settings.UseAccount, true);
            Assert.AreEqual(settings.ApiHost, "apihost_test");
            Assert.AreEqual(settings.ApiKey, "apikey_test");
            Assert.AreEqual(settings.SecretKey, "secretkey_test");
            
            RestoreUserSettings();
        }

        /// <summary>
        /// Testing the save of a SortZam Settings if it already exist, overriding it
        /// </summary>
        [TestMethod]
        public void Save_OverrideFile()
        {
            SaveUserSettings();

            // New settings with new values
            Settings.Clear();
            Settings settings = Settings.Instance;
            settings.ApiHost = "apihost_test2";
            settings.ApiKey = "apikey_test2";
            settings.SecretKey = "secretkey_test2";
            settings.Save();

            // Check if file is overrided
            Settings.Clear();
            Settings settings2 = Settings.Instance;

            Assert.IsNotNull(settings2);
            Assert.IsNotNull(settings2.ApiHost);
            Assert.IsNotNull(settings2.ApiKey);
            Assert.IsNotNull(settings2.SecretKey);
            Assert.AreEqual(settings2.ApiHost, "apihost_test2");
            Assert.AreEqual(settings2.ApiKey, "apikey_test2");
            Assert.AreEqual(settings2.SecretKey, "secretkey_test2");
            Assert.AreEqual(settings2.UseAccount, false);

            RestoreUserSettings();
        }

        // Store user settings to avoid to lose them.
        private void SaveUserSettings()
        {
            Settings.Clear();
            Settings settings = Settings.Instance;

            useAccountBak = settings.UseAccount;
            apiHostBak = settings.ApiHost;
            apiKeyBak = settings.ApiKey;
            secretKeyBak = settings.SecretKey;
            settings.Save();

            Settings.Clear();
        }

        // Restore previous user setting for a nomal use.
        private void RestoreUserSettings()
        {
            Settings.Clear();
            Settings settings = Settings.Instance;

            settings.UseAccount = useAccountBak;
            settings.ApiHost = apiHostBak;
            settings.ApiKey = apiKeyBak;
            settings.SecretKey = secretKeyBak;
            settings.Save();

            Settings.Clear();
        }
    }
}
