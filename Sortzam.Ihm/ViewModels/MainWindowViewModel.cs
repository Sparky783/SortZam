using Sortzam.Ihm.Models;
using Sortzam.Ihm.Models.Command;
using Sortzam.Ihm.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sortzam.Ihm.ViewModels
{
    public class MainWindowViewModel : Notifier
    {
        public MainWindowViewModel()
        {
            InitCommands();
        }

        #region Commands
        public ICommand SettingsCommand { get; private set; }

        /// <summary>
        /// Prepare commands for all buttons
        /// </summary>
        private void InitCommands()
        {
            SettingsCommand = new RelayCommand(x => { OpenSettings(); });
        }
        #endregion

        #region Button Methods
        /// <summary>
        /// Ask to the user to chose a folder and display content. Subfolders are also displayed. 
        /// </summary>
        private void OpenSettings()
        {
            Settings settings = Settings.Instance;

            LoginWindow login = new LoginWindow();
            login.ApiHost = settings.ApiHost;
            login.ApiKey = settings.ApiKey;
            login.SecretKey = settings.SecretKey;


            if (login.ShowDialog() == true)
            {
                if (login.UseAccount)
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
                App.OnSettingsChanged();
            }
        }
        #endregion
    }
}
