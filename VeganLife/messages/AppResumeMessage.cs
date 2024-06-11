using CommunityToolkit.Mvvm.Messaging.Messages;

namespace VeganLife.messages
{
    public class AppResumeMessage : ValueChangedMessage<string>
    {
        public AppResumeMessage(string value)
            : base(value)
        {
        }
    }
}
