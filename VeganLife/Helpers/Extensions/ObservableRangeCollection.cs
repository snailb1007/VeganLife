using PropertyChanged;
using System.Collections.Specialized;

namespace VeganLife.Helpers.Extensions
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification;

        [SuppressPropertyChangedWarnings]
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            _suppressNotification = true;

            foreach (var item in collection)
            {
                Items.Add(item);
            }

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(collection)));
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            _suppressNotification = true;

            foreach (var item in collection)
            {
                Items.Remove(item);
            }

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(collection)));
        }

        // Replace all items in the collection with a new set of items
        public void Replace(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            _suppressNotification = true;

            Items.Clear();
            foreach (var item in collection)
            {
                Items.Add(item);
            }

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
