using Newtonsoft.Json;

namespace VeganLife.Models.CommunityFreeServiceModel
{
    public class FoodsListRequestModel
    {
        [JsonProperty("dataType")]
        public List<string> DataType { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("sortBy")]
        public string SortBy { get; set; }

        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
    }

    public class FoodDetailListRequest
    {
        [JsonProperty("fdcId")]
        public int FdcId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("foodCode")]
        public int? FoodCode { get; set; }

        [JsonProperty("publicationDate")]
        public DateTime PublicationDate { get; set; }

        [JsonProperty("foodCategory")]
        public string FoodCategory { get; set; }

        [JsonProperty("nutrients")]
        public List<NutrientDetailListRequest> Nutrients { get; set; }
    }

    public class NutrientDetailListRequest
    {
        [JsonProperty("nutrientId")]
        public int NutrientId { get; set; }

        [JsonProperty("nutrientName")]
        public string NutrientName { get; set; }

        [JsonProperty("unitName")]
        public string UnitName { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}