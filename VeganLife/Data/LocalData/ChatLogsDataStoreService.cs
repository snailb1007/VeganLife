namespace VeganLife.Data.LocalData
{
    using VeganLife.Services.LocalDataServices;
    public class ChatLogsDataStoreService : BaseDataStore<ChatLogsModel>
    {
        public ChatLogsDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
