using SQLite;

namespace VeganLife.Models
{
    internal class UpdateMasterModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public int Version { get; set; }

        public DateTime LastUpdated { get; set; } // Timestamp to track the last update
    }
}
