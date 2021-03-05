using System.IO;
using System.Runtime.Serialization;
using Tools.Tools;
using Tools.Utils;

namespace Sortzam.Lib.UserSettings
{
    public class MySettings
    {
        static public string FILE_NAME = "usersettings.szs";

        static Settings settingsInstance;
        private static LockersSafeProvider<string> _lockerProvider = new LockersSafeProvider<string>();

        /// <summary>
        /// Get user settings instance and if the file already exist, load it.
        /// </summary>
        /// <returns></returns>
        static public Settings GetInstance()
        {
            if (settingsInstance != null)
                return settingsInstance;

            lock (_lockerProvider["settingsInstance"])
            {
                settingsInstance = new Settings();
                settingsInstance.Load();
            }
            return settingsInstance;
        }
        public static void Clear()
        {
            settingsInstance = null;
        }
    }
    [DataContract]
    public class Settings
    {
        [DataMember]
        public bool UseAccount { get; set; }

        [DataMember]
        public string ApiHost { get; set; }

        [DataMember]
        public string ApiKey { get; set; }

        [DataMember]
        public string SecretKey { get; set; }
        public Settings()
        {
            ApiHost = "";
            ApiKey = "";
            SecretKey = "";
        }
        public void Load()
        {
            if (!File.Exists(MySettings.FILE_NAME))
                return;
            var xml = (Settings)SerializerUtils<Settings>.XmlDeserialize(MySettings.FILE_NAME); // szs = SortZam Settings
            UseAccount = xml.UseAccount;
            ApiHost = xml.ApiHost;
            ApiKey = xml.ApiKey;
            SecretKey = xml.SecretKey;
        }
        public void Save()
        {
            SerializerUtils<Settings>.XmlSerialize(new Settings()
            {
                ApiHost = ApiHost,
                UseAccount = UseAccount,
                ApiKey = ApiKey,
                SecretKey = SecretKey
            }, MySettings.FILE_NAME); // szs = SortZam Settings
        }
    }
}