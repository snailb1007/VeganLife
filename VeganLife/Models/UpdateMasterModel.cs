using SQLite;

namespace VeganLife.Models
{
    internal class UpdateMasterModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public int Version { get; set; }
    }
}
