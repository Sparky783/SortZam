using FlashLightDMX.Models;
using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sortzam.Ihm.ViewModels
{
    public class MusicListPageViewModel: Notifier
    {
        public MusicListPageViewModel()
        {
            Musics = new ObservableCollection<MusicDao>();
            Musics.Add(new MusicDao() { Title = "Fuck !!!" });
            Musics.Add(new MusicDao() { Title = "Your !!!" });
            Musics.Add(new MusicDao() { Title = "Mum !!!" });
        }

        public ObservableCollection<MusicDao> Musics
        {
            get; set;
        }
    }
}
