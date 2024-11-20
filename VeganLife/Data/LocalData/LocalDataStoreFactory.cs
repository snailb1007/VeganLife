using FFImageLoading.Helpers;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    public class LocalDataStoreFactory
    {
        public static LocalDataStoreFactory Instance => _instance.Value;

        private static readonly Lazy<LocalDataStoreFactory> _instance =
            new(() => new LocalDataStoreFactory());

        private readonly ISQLite _sqlite;
        private readonly Dictionary<Type, object> _dataStores = new();

        public LocalDataStoreFactory()
        {
            _sqlite = ServiceHelper.GetService<ISQLite>();
        }

        public BaseDataStore<T> GetDataStore<T>() where T : new()
        {
            var type = typeof(T);
            if (!_dataStores.ContainsKey(type))
            {
                var dataStore = new BaseDataStore<T>(_sqlite);
                _dataStores[type] = dataStore;
            }

            return (BaseDataStore<T>)_dataStores[type];
        }
    }
}
