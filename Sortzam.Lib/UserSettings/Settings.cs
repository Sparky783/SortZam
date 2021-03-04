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

        public Settings()
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
        static public Settings GetInstance()
        {
            if (settingsInstance == null)
            {
                if (File.Exists(FILE_NAME))
                    settingsInstance = SerializerUtils<Settings>.XmlDeserialize(FILE_NAME); // szs = SortZam Settings
                else
                    settingsInstance = new Settings();
            }

            return settingsInstance;
        }

        /// <summary>
        /// Save user settings
        /// </summary>
        public void Save()
        {
            if (settingsInstance == null)
                settingsInstance = new Settings() { ApiHost = ApiHost, UseAccount = UseAccount, ApiKey = ApiKey, SecretKey = SecretKey };
            if (File.Exists(FILE_NAME))
                File.Delete(FILE_NAME);
            SerializerUtils<Settings>.XmlSerialize(settingsInstance, FILE_NAME); // szs = SortZam Settings
        }

        /// <summary>
        /// Clear current Settings to force the next loading
        /// </summary>
        public static void Clear()
        {
            settingsInstance = null;
        }
        #endregion
    }
}