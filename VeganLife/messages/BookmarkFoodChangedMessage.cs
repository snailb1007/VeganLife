using CommunityToolkit.Mvvm.Messaging.Messages;
using VeganLife.Models.FoodModel;

namespace VeganLife.Messages
{
    public class BookmarkFoodChangedMessage : ValueChangedMessage<FoodPreviewModel>
    {
        public BookmarkFoodChangedMessage(FoodPreviewModel value) : base(value)
        {
        }
    }
}
