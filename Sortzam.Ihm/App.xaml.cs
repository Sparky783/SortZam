using Sortzam.Ihm.Models;
using Sortzam.Ihm.Views;
using Sortzam.Lib.UserSettings;
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
        static public event EventHandler SettingsEvent;

        /// <summary>
        /// Load user settings
        /// </summary>
        static public void LoadSettings()
        {
            Settings settings = Settings.Instance;

            // Check if the user config the app.
            if (string.IsNullOrEmpty(settings.ApiHost) || string.IsNullOrEmpty(settings.ApiKey) || string.IsNullOrEmpty(settings.SecretKey))
            {
                LoginWindow login = new LoginWindow();
                login.IsShutdownAction = true;

                if (login.ShowDialog() == true)
                {
                    if(login.UseAccount)
                    {
                        if (!string.IsNullOrEmpty(login.ApiHost) && !string.IsNullOrEmpty(login.ApiKey) && !string.IsNullOrEmpty(login.SecretKey))
                        {
                            settings.ApiHost = login.ApiHost;
                            settings.ApiKey = login.ApiKey;
                            settings.SecretKey = login.SecretKey;
                        }
                    }

                    settings.UseAccount = login.UseAccount;
                    settings.Save();
                }
            }

            OnSettingsChanged();
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
