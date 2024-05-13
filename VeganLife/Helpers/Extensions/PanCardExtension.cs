// <copyright file="PanCardExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Reflection;
using PanCardView;

namespace VeganLife.Helpers.Extensions
{
    public static class PanCardExtension
    {
        public static void CleanUnusedViews(this CardsView cardView)
        {
            var removeUnsedChildrenMethod = typeof(CardsView).GetMethod("CleanUnprocessingChildren", BindingFlags.NonPublic | BindingFlags.Instance);
            removeUnsedChildrenMethod?.Invoke(cardView, null);
        }

        public static void SetSelectedIndexWithShouldAutoNavigateToNext(this CardsView cardView, bool isNext)
        {
            var setSelected = typeof(CardsView).GetMethod("SetSelectedIndexWithShouldAutoNavigateToNext", BindingFlags.NonPublic | BindingFlags.Instance);
            setSelected?.Invoke(cardView, new object[] { isNext });
        }

        public static void RemoveChildren(this CardsView cardView, View[] views)
        {
            var cleanViewMethod = typeof(CardsView).GetMethod("RemoveChildren", BindingFlags.NonPublic | BindingFlags.Instance);
            cleanViewMethod?.Invoke(cardView, new object[] { views });
        }
    }
}
