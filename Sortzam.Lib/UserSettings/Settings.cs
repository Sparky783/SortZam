using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Tools.Utils;

namespace Sortzam.Lib.UserSettings
{
    [DataContract]
    public class Settings
    {
        [IgnoreDataMember]
        static public string FILE_NAME = "usersettings.szs";
        [IgnoreDataMember]
        static private Settings settingsInstance;
        [IgnoreDataMember]
        private static readonly object padlock = new object(); // Use to be thread-safe

        #region Saved members
        [DataMember]
        public bool UseAccount { get; set; }

        [DataMember]
        public string ApiHost { get; set; }

        [DataMember]
        public string ApiKey { get; set; }

        [DataMember]
        public string SecretKey { get; set; }
        #endregion

        private Settings()
        {
            ApiHost = "";
            ApiKey = "";
            SecretKey = "";
        }

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
                        if (File.Exists(FILE_NAME))
                            settingsInstance = SerializerUtils<Settings>.XmlDeserialize(FILE_NAME); // szs = SortZam Settings
                        else
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
            SerializerUtils<Settings>.XmlSerialize(settingsInstance, FILE_NAME); // szs = SortZam Settings
        }
        #endregion
    }
}