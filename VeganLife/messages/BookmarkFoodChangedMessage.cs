namespace VeganLife.Messages
{
    using CommunityToolkit.Mvvm.Messaging.Messages;
    using VeganLife.Models.FoodModel;

    public class BookmarkFoodChangedMessage : ValueChangedMessage<FoodPreviewModel>
    {
        public BookmarkFoodChangedMessage(FoodPreviewModel value)
            : base(value)
        {
        }
    }
}
