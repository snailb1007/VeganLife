using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    public class ChatLogsDataStoreService : BaseDataStore<ChatLogsModel>
    {
        public ChatLogsDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
