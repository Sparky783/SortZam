using Sortzam.Ihm.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sortzam.Ihm.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public string ApiHost
        {
            get { return apiHostTB.Text; }
            set { apiHostTB.Text = value; }
        }

        public string ApiKey
        {
            get { return apiKeyTB.Text; }
            set { apiKeyTB.Text = value; }
        }

        public string SecretKey
        {
            get { return secretKeyTB.Password; }
            set { secretKeyTB.Password = value; }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
