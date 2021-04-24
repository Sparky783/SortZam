using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Tools.Utils;

namespace Sortzam.Ihm.Models
{
    public class Settings
    {
        static private Settings settingsInstance;
        private static readonly object padlock = new object(); // Use to be thread-safe

        #region Saved members
        public bool UseAccount
        {
            get
            {
                return Ihm.Properties.Settings.Default.UseAccount;
            }

            set
            {
                Ihm.Properties.Settings.Default.UseAccount = value;
            }
        }

        public string ApiHost
        {
            get
            {
                return Ihm.Properties.Settings.Default.ApiHost;
            }

            set
            {
                Ihm.Properties.Settings.Default.ApiHost = value;
            }
        }

        public string ApiKey
        {
            get
            {
                return Ihm.Properties.Settings.Default.ApiKey;
            }

            set
            {
                Ihm.Properties.Settings.Default.ApiKey = value;
            }
        }

        public string SecretKey
        {
            get
            {
                return Ihm.Properties.Settings.Default.SecretKey;
            }

            set
            {
                Ihm.Properties.Settings.Default.SecretKey = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get user settings instance and if the file already exist, load it.
        /// </summary>
        /// <returns></returns>
        static public Settings Instance
        {
            get
            {
                if (settingsInstance == null)
                {
                    lock (padlock)
                    {
                        settingsInstance = new Settings();
                    }
                }

                return settingsInstance;
            }
        }

        /// <summary>
        /// Get user settings instance and if the file already exist, load it.
        /// </summary>
        /// <returns></returns>
        static public void Clear()
        {
            settingsInstance = null;
        }

        /// <summary>
        /// Save user settings
        /// </summary>
        public void Save()
        {
            Ihm.Properties.Settings.Default.Save();
        }
        #endregion
    }
}