using SQLite;

namespace VeganLife.Models.FoodModel
{
    public class FoodDetailModel
    {
        [PrimaryKey]
        public ushort Id { get; set; }
        public string Decorate { get; set; }
        public string Ingredient { get; set; }
        public string Making { get; set; }
        public string Sauce { get; set; }
    }
}
