using Sortzam.Ihm.Models;
using Sortzam.Ihm.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tools.Utils;

namespace Sortzam
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public Settings Settings = new Settings();
        static public event EventHandler SettingsEvent;

        /// <summary>
        /// Load user settings
        /// </summary>
        static public void LoadSettings()
        {
            if(File.Exists("usersettings.szs"))
                Settings = SerializerUtils<Settings>.XmlDeserialize("usersettings.szs"); // szs = SortZam Settings

            // Check if the user config the app.
            if (string.IsNullOrEmpty(Settings.ApiHost) || string.IsNullOrEmpty(Settings.ApiKey) || string.IsNullOrEmpty(Settings.SecretKey))
            {
                LoginWindow login = new LoginWindow();
                login.IsShutdownAction = true;

                if (login.ShowDialog() == true)
                {
                    if(login.UseAccount)
                    {
                        if (!string.IsNullOrEmpty(login.ApiHost) && !string.IsNullOrEmpty(login.ApiKey) && !string.IsNullOrEmpty(login.SecretKey))
                        {
                            Settings.ApiHost = login.ApiHost;
                            Settings.ApiKey = login.ApiKey;
                            Settings.SecretKey = login.SecretKey;
                        }
                    }

                    Settings.UseAccount = login.UseAccount;

                    SaveSettings();
                }
            }

            OnSettingsChanged();
        }

        /// <summary>
        /// Save user settings
        /// </summary>
        static public void SaveSettings()
        {
            SerializerUtils<Settings>.XmlSerialize(Settings, "usersettings.szs"); // szs = SortZam Settings
        }

        /// <summary>
        /// Update HMI when settings changed.
        /// </summary>
        static public void OnSettingsChanged()
        {
            SettingsEvent?.Invoke(null, null);
        }
    }
}
