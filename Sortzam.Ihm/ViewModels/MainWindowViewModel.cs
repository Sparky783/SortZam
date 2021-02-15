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
            LoginWindow login = new LoginWindow();
            login.ApiHost = App.Settings.ApiHost;
            login.ApiKey = App.Settings.ApiKey;
            login.SecretKey = App.Settings.SecretKey;


            if (login.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(login.ApiHost) && !string.IsNullOrEmpty(login.ApiKey) && !string.IsNullOrEmpty(login.SecretKey))
                {
                    App.Settings.ApiHost = login.ApiHost;
                    App.Settings.ApiKey = login.ApiKey;
                    App.Settings.SecretKey = login.SecretKey;

                    App.SaveSettings();
                }
            }
        }
        #endregion
    }
}
