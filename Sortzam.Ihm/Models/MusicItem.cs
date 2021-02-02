using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sortzam.Ihm.Models
{
    public class MusicItem : Notifier
    {
        public string FileName { get; set; }

        public string Path { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
    }
}
