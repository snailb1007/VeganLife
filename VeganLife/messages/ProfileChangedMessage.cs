using CommunityToolkit.Mvvm.Messaging.Messages;

namespace VeganLife.messages
{
    public class ProfileChangedMessage : ValueChangedMessage<object>
    {
        public ProfileChangedMessage(object value) : base(value)
        {
        }
    }
}
