// <copyright file="ConversationViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Android.Media;
using VeganLife.Data.LocalData;
using VeganLife.Services.OpenAIService;
using VeganLife.Views.ChatFlyout;

namespace VeganLife.ViewModels
{
    [QueryProperty(nameof(PassedData), nameof(PassedData))]
    public partial class ConversationViewModel : BaseViewModel
    {
        private readonly IOpenAIService _openAIService;
        private readonly IDispatcher _dispatcher;

        [ObservableProperty]
        private string query;

        [ObservableProperty]
        private bool isAnimationVisible = true;

        [ObservableProperty]
        private bool isTrustedSetting = true;

        [ObservableProperty]
        private ObservableCollection<ChatMessageModel> messages = new();

        [ObservableProperty]
        private ContentPage conversationView;

        [ObservableProperty]
        private double opacityModeMessage = 1;

        [ObservableProperty]
        private double opacityModeImage = 0.5;

        [ObservableProperty]
        private ChatMessageModel theMessage;
        [ObservableProperty]
        private ChatLogsModel _currentChat;

        [ObservableProperty]
        private string passedData;

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
            _sessionGuid = Guid.Empty;
            this._chatLogsDataStoreService = chatLogsDataStoreService;
        }

        public override async Task<Task> ViewAppearingVM()
        {
            IsTrustedSetting = deviceService != null
                && deviceService.IsAutomaticTimeZoneEnabled()
                && deviceService.IsAutomaticDateTimeEnabled();
            if (!IsTrustedSetting)
            {
                bool isAcceptedGoSetting = await Shell.Current.DisplayAlert("Settings is invalid!", "Please turn on Automatic Time Zone & DateTime", "ok", "cancel");
                if (isAcceptedGoSetting)
                {
                    deviceService?.OpenDateSettings();
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
                        TimesLimit = 5,
                    };
                    await _chatLogsDataStoreService.AddOrUpdateItemAsync(CurrentChat);
                }
            }

            return base.ViewAppearingVM();
        }

        private void AddMessage(string message, bool isUserMessage)
        {
            // if (Messages.Count <= 0) IsAnimationVisible = false;
            _ = _dispatcher.Dispatch(() => Messages.Add(new ChatMessageModel { Text = message, IsUserMessage = isUserMessage }));
            var collection = (CollectionView)Shell.Current.CurrentPage.FindByName("messCollection");
            if (collection is null)
            {
                return;
            }

            Task.Delay(150).ContinueWith(t =>
            {
                _dispatcher.Dispatch(() =>
                {
                    collection.ScrollTo(
                        item: Messages.Last(),
                        position: ScrollToPosition.End,
                        animate: true);
                });
            });
        }

        private async Task QueryManagerAsync(Func<Guid, string, Task<string>> queryManager)
        {
            if (string.IsNullOrEmpty(Query))
            {
                return;
            }

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
            if (CurrentCommand.IsRunning
                || CurrentChat.TimesLimit < 1)
            {
                return;
            }

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
            {
                return;
            }

            await navigationService.NavigateToPage<ChatGPTDisClaimerPage>();
        }

        [RelayCommand]
        private async Task OpenChatGPTDetailPage()
        {
            if (OpenChatGPTDetailPageCommand.IsRunning)
            {
                return;
            }

            await navigationService.NavigateToPage<ChatGPTDetailPage>();
        }

        partial void OnPassedDataChanged(string value)
        {
            this.Query = value;
        }
    }
}
