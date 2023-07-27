namespace VeganLife.Models
{
    public class UserInfo
    {
        public string Name { get; set; }
        public byte Age { get; set; }
        public bool IsMale { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public string FoodsRead { get; set; }
        public string VitaminRead { get; set; }
        public uint TotalRead { get; set; }
    }
}
