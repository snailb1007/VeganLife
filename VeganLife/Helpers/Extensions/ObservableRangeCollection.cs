using System.Collections.Specialized;

namespace VeganLife.Helpers.Extensions
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification;

        // Add a range of items to the collection
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

            // Notify collection changed
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(collection)));
        }

        // Remove a range of items from the collection
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

            // Notify collection changed
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

            // Notify collection changed
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        // Override OnCollectionChanged to prevent excessive notifications
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }
    }
}
