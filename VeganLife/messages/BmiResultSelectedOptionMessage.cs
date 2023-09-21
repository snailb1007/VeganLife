using CommunityToolkit.Mvvm.Messaging.Messages;

namespace VeganLife.messages
{
    public class BmiResultSelectedOptionMessage : ValueChangedMessage<byte>
    {
        public BmiResultSelectedOptionMessage(byte value)
            : base(value)
        {
        }
    }
}
