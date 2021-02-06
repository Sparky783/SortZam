using System.Collections.Specialized;
using System.ComponentModel;

namespace Sortzam.Ihm.Models
{
    /// <summary>
    /// Class made to implement utilities of ViewModel HMI.
    /// </summary>
    public class Notifier : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Event for property changgr
        /// </summary>
        /// <param name="propertyName">Name of the property to update on the HMI.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Event for collection changed
        /// </summary>
        /// <param name="collectionAction">Name of the collection to update on the HMI.</param>
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
