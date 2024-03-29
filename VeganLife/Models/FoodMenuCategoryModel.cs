// <copyright file="FoodMenuCategoryModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models
{
    public class FoodMenuCategoryModel : MenuModel
    {
        public string Category
        {
            get
            {
                switch (this.Title)
                {
                    case "breakfast":
                        return "Bữa sáng";
                    case "dessert":
                        return "Tráng miệng";
                    case "dinner":
                        return "Bữa tối";
                    case "drink":
                        return "Đồ uống";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
