using Sortzam.Ihm.Models;
using Sortzam.Ihm.ViewModels;
using Sortzam.Lib.DataAccessObjects;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sortzam.Ihm.Views
{
    /// <summary>
    /// Logique d'interaction pour MusicListPage.xaml
    /// </summary>
    public partial class MusicListPage : Page
    {
        public MusicListPage()
        {
            InitializeComponent();
        }

        public MusicListPageViewModel ViewModel
        {
            get { return (MusicListPageViewModel)Resources["ViewModel"]; }
        }

        private void projectorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
                ViewModel.SelectFile((MusicItem)e.AddedItems[0]);
        }

        private void resultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                ViewModel.SelectResult((AnalyzeResult)e.AddedItems[0]);
        }
    }
}
