using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sortzam.Ihm.Models
{
    [DataContract]
    public class Notifier : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnNotifyCollectionChanged(NotifyCollectionChangedAction collectionAction)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(collectionAction));
            }
        }
    }
}
