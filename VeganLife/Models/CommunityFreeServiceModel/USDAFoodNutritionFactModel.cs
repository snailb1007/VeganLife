namespace VeganLife.Models.CommunityFreeServiceModel
{
    public class USDAFoodNutritionFactModel
    {
        public string discontinuedDate { get; set; }
        public List<object> foodComponents { get; set; }
        public List<FoodAttribute> foodAttributes { get; set; }
        public List<object> foodPortions { get; set; }
        public string fdcId { get; set; }
        public string description { get; set; }
        public string publicationDate { get; set; }
        public List<FoodNutrient> foodNutrients { get; set; }
        public string dataType { get; set; }
        public string foodClass { get; set; }
        public string modifiedDate { get; set; }
        public string availableDate { get; set; }
        public string brandOwner { get; set; }
        public string brandName { get; set; }
        public string dataSource { get; set; }
        public string brandedFoodCategory { get; set; }
        public string gtinUpc { get; set; }
        public string ingredients { get; set; }
        public string marketCountry { get; set; }
        public string servingSize { get; set; }
        public string servingSizeUnit { get; set; }
        public string packageWeight { get; set; }
        public List<FoodUpdateLog> foodUpdateLog { get; set; }
        public LabelNutrients labelNutrients { get; set; }
    }

    public class Calcium
    {
        public string value { get; set; }
    }

    public class Calories
    {
        public double value { get; set; }
    }

    public class Carbohydrates
    {
        public string value { get; set; }
    }

    public class Cholesterol
    {
        public string value { get; set; }
    }

    public class Fat
    {
        public double value { get; set; }
    }

    public class Fiber
    {
        public double value { get; set; }
    }

    public class FoodAttribute
    {
        public string id { get; set; }
        public string value { get; set; }
        public string name { get; set; }
    }

    public class FoodNutrient
    {
        public string type { get; set; }
        public Nutrient nutrient { get; set; }
        public FoodNutrientDerivation foodNutrientDerivation { get; set; }
        public string id { get; set; }
        public double amount { get; set; }
    }

    public class FoodNutrientDerivation
    {
        public string id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class FoodUpdateLog
    {
        public string discontinuedDate { get; set; }
        public List<object> foodAttributes { get; set; }
        public string fdcId { get; set; }
        public string description { get; set; }
        public string publicationDate { get; set; }
        public string dataType { get; set; }
        public string foodClass { get; set; }
        public string modifiedDate { get; set; }
        public string availableDate { get; set; }
        public string brandOwner { get; set; }
        public string brandName { get; set; }
        public string dataSource { get; set; }
        public string brandedFoodCategory { get; set; }
        public string gtinUpc { get; set; }
        public string ingredients { get; set; }
        public string marketCountry { get; set; }
        public string servingSize { get; set; }
        public string servingSizeUnit { get; set; }
        public string packageWeight { get; set; }
        public string subbrandName { get; set; }
        public string notaSignificantSourceOf { get; set; }
    }

    public class Iron
    {
        public double value { get; set; }
    }

    public class LabelNutrients
    {
        public Fat fat { get; set; }
        public SaturatedFat saturatedFat { get; set; }
        public TransFat transFat { get; set; }
        public Cholesterol cholesterol { get; set; }
        public Sodium sodium { get; set; }
        public Carbohydrates carbohydrates { get; set; }
        public Fiber fiber { get; set; }
        public Sugars sugars { get; set; }
        public Protein protein { get; set; }
        public Calcium calcium { get; set; }
        public Iron iron { get; set; }
        public Potassium potassium { get; set; }
        public Calories calories { get; set; }
    }

    public class Nutrient
    {
        public string id { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public string rank { get; set; }
        public string unitName { get; set; }
    }

    public class Potassium
    {
        public string value { get; set; }
    }

    public class Protein
    {
        public string value { get; set; }
    }

    public class SaturatedFat
    {
        public double value { get; set; }
    }

    public class Sodium
    {
        public double value { get; set; }
    }

    public class Sugars
    {
        public string value { get; set; }
    }

    public class TransFat
    {
        public string value { get; set; }
    }
}
