using Android.Companion;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Services.OpenAIService;
using VeganLife.Views.ChatFlyout;

namespace VeganLife.ViewModels
{
    public partial class ConversationViewModel : BaseViewModel
    {
        [ObservableProperty]
        string query;

        [ObservableProperty]
        bool isAnimationVisible = true;

        [ObservableProperty]
        bool isTrustedSetting = true;

        [ObservableProperty]
        ObservableCollection<ChatMessageModel> messages = new();

        [ObservableProperty]
        ContentPage conversationView;

        [ObservableProperty]
        double opacityModeMessage = 1;

        [ObservableProperty]
        double opacityModeImage = 0.5;

        [ObservableProperty]
        private ChatMessageModel theMessage;
        [ObservableProperty]
        private ChatLogsModel _currentChat;

        readonly IOpenAIService _openAIService;
        readonly IDispatcher _dispatcher;
        readonly IDeviceService deviceService;

        private AsyncRelayCommand _currentCommand;

        public AsyncRelayCommand CurrentCommand
        {
            get
            {
                return _currentCommand ??= new AsyncRelayCommand(AskQuestionAsync);
            }
            set
            {
                SetProperty(ref _currentCommand, value);
            }
        }

        private Guid _sessionGuid;
        private ChatLogsDataStoreService _chatLogsDataStoreService;


        public ConversationViewModel(IDispatcher dispatcher, IOpenAIService openAIService, ChatLogsDataStoreService chatLogsDataStoreService)
            : base()
        {
            _openAIService = openAIService;
            _dispatcher = dispatcher;
            deviceService = ServicesHelper.GetService<IDeviceService>();
            _sessionGuid = Guid.Empty;
            this._chatLogsDataStoreService = chatLogsDataStoreService;
        }

        public override async Task<Task> ViewAppearingVM()
        {
            IsTrustedSetting = deviceService.IsAutomaticTimeZoneEnabled() && deviceService.IsAutomaticDateTimeEnabled();
            if (!IsTrustedSetting)
            {
                bool isAcceptedGoSetting = await Shell.Current.DisplayAlert("Settings is invalid!", "Please turn on Automatic Time Zone & DateTime", "ok", "cancel");
                if (isAcceptedGoSetting)
                {
                    deviceService.OpenDateSettings();
                }
            }
            else if (CurrentChat is null)
            {
                var lst = await _chatLogsDataStoreService.GetItemsAsync();
                CurrentChat = lst.FirstOrDefault(i => i.ChatDate.Equals(DateTime.Today.Date))!;
                if (CurrentChat is null)
                {
                    CurrentChat = new ChatLogsModel
                    {
                        AdWatchingLimit = 1,
                        ChatDate = DateTime.Today.Date,
                        TimesLimit = 3
                    };
                    await _chatLogsDataStoreService.AddOrUpdateItemAsync(CurrentChat);
                }
            }

            return base.ViewAppearingVM();
        }

        private void AddMessage(string message, bool isUserMessage)
        {
            // if (Messages.Count <= 0) IsAnimationVisible = false;
            _dispatcher.Dispatch(() => Messages.Add(new ChatMessageModel
            {
                Text = message,
                IsUserMessage = isUserMessage,
            }));
            var collection = (CollectionView)Shell.Current.CurrentPage.FindByName("messCollection");
            if (collection is null)
                return;
            Task.Delay(150).ContinueWith(t =>
            {
                _dispatcher.Dispatch(() =>
                {
                        collection.ScrollTo
                        (
                            item: Messages.Last(),
                            position: ScrollToPosition.End,
                            animate: true
                        );
                });
            });
        }

        private async Task QueryManagerAsync(Func<Guid, string, Task<string>> queryManager)
        {
            if (string.IsNullOrEmpty(Query)) return;
            if (CurrentChat.TimesLimit > 0)
            {
                CurrentChat.TimesLimit -= 1;
            }

            string queryCopy = Query;
            Query = string.Empty;
            AddMessage(message: queryCopy, isUserMessage: true);
            IsLoading = true;
            string answer = await queryManager(_sessionGuid, queryCopy);
            AddMessage(message: answer.TrimStart(), isUserMessage: false);
            IsLoading = false;
        }

        private async Task AskQuestionAsync()
        {
            //if (CurrentChat.TimesLimit < 100 || CurrentCommand.IsRunning)
            //    return;
            if (_sessionGuid == Guid.Empty)
            {
                _sessionGuid = Guid.NewGuid();
            }

            await QueryManagerAsync(_openAIService.AskQuestionAsync);
            await _chatLogsDataStoreService.AddOrUpdateItemAsync(CurrentChat);
        }

        [RelayCommand]
        private async Task OpenDisclaimerPopup()
        {
            if (OpenDisclaimerPopupCommand.IsRunning)
                return;
            await navigationService.NavigateToPage<ChatGPTDisClaimerPage>();
        }

        [RelayCommand]
        private async Task OpenChatGPTDetailPage()
        {
            if (OpenChatGPTDetailPageCommand.IsRunning)
                return;
            await navigationService.NavigateToPage<ChatGPTDetailPage>();
        }
    }
}
