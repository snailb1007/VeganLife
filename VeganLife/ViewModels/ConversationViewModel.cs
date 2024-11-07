// <copyright file="ConversationViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using Plugin.MauiMTAdmob;
using PropertyChanged;
using VeganLife.Data.LocalData;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;
using VeganLife.Services.OpenAIService;
using VeganLife.Views.ChatFlyout;
using VeganLife.Views.Controls;

namespace VeganLife.ViewModels
{
    [QueryProperty(nameof(PassedData), nameof(PassedData))]
    public partial class ConversationViewModel : BaseViewModel
    {
        private readonly IOpenAIService _openAIService;
        private readonly IDispatcher _dispatcher;

        private DateTime _startTime;
        private AsyncRelayCommand _currentCommand;

        private Guid _sessionGuid;
        private ChatLogsDataStoreService _chatLogsDataStoreService;
        private GoogleAdValidatorDataStoreService _googleAdValidatorDataStoreService;
        private CancellationTokenSource _cancellationTokenSource;

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

        public string AdRewardedDescription
        {
            get
            {
                if (CurrentAdValidatorData.IsRewardAdAvailable)
                {
                    if (string.IsNullOrEmpty(CountDownText))
                    {
                        return AppResources.getMorePoint_info_label;
                    }
                    else
                    {
                        return AppResources.waitingForCollect_info_label;
                    }
                }
                else
                {
                    return AppResources.cantCollect_info_label;
                }
            }
        }

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

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AdRewardedDescription))]
        private GoogleAdValidatorModel currentAdValidatorData = new GoogleAdValidatorModel();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AdRewardedDescription))]
        private string countDownText = string.Empty;

        [ObservableProperty]
        private bool isAdInProgress;

        public ConversationViewModel(
            IDispatcher dispatcher,
            IOpenAIService openAIService,
            ChatLogsDataStoreService chatLogsDataStoreService,
            GoogleAdValidatorDataStoreService googleAdValidatorDataStoreService)
            : base()
        {
            _openAIService = openAIService;
            _dispatcher = dispatcher;
            _sessionGuid = Guid.Empty;
            this._chatLogsDataStoreService = chatLogsDataStoreService;
            _googleAdValidatorDataStoreService = googleAdValidatorDataStoreService;
            CrossMauiMTAdmob.Current.OnRewardedLoaded += (s, e) =>
            {
                if (CrossMauiMTAdmob.Current.IsRewardedLoaded())
                {
                    CrossMauiMTAdmob.Current.ShowRewarded();
                    IsAdInProgress = false;
                }
            };
            CrossMauiMTAdmob.Current.OnUserEarnedReward += (s, e) =>
            {
                CurrentChat.TimesLimit++;
                _chatLogsDataStoreService.AddOrUpdateItemAsync(CurrentChat).SafeFireAndForget();
            };
            CrossMauiMTAdmob.Current.OnRewardedFailedToLoad += (s, e) =>
            {
                IsAdInProgress = false;
            };
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
                    _ = _chatLogsDataStoreService.AddOrUpdateItemAsync(CurrentChat);
                }
            }

            var adLogs = await _googleAdValidatorDataStoreService.GetItemsAsync();
            if (adLogs.Count() > 0)
            {
                CurrentAdValidatorData = adLogs.Last();
                if (CurrentAdValidatorData.LastTimeRewardOpen.Date < DateTime.Today.Date)
                {
                    await _googleAdValidatorDataStoreService.AddOrUpdateItemAsync(new GoogleAdValidatorModel
                    {
                        LastTimeRewardOpen = DateTime.Today.Date,
                        RewardAdTimesLimit = 0,
                    });
                }
            }
            else
            {
                await _googleAdValidatorDataStoreService.AddOrUpdateItemAsync(new GoogleAdValidatorModel
                {
                    LastTimeRewardOpen = DateTime.Today.Date,
                    RewardAdTimesLimit = 0,
                });
            }

            // re-get the lastest data
            CurrentAdValidatorData = (await _googleAdValidatorDataStoreService.GetItemsAsync())?.LastOrDefault()!;
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

             
            {
                if (CurrentChat.TimesLimit > 0)
                {
                    CurrentChat.TimesLimit -= 1;
                }

                string queryCopy = Query;
                Query = string.Empty;
                AddMessage(message: queryCopy, isUserMessage: true);
                string answer = await queryManager(_sessionGuid, queryCopy);
                AddMessage(message: answer.TrimStart(), isUserMessage: false);
            }
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

        // private ProgressDrawableControl _drawable;

        [RelayCommand]
        private async Task OpenRewardedAdPage()
        {
            if (OpenRewardedAdPageCommand.IsRunning
                || !CurrentAdValidatorData.IsRewardAdAvailable
                || !string.IsNullOrEmpty(CountDownText))
            {
                return;
            }

            //if (_drawable is null)
            //{
            //    var graphicsView = Shell.Current.CurrentPage.FindByName<GraphicsView>("GraphicsViewCountDown");
            //    _drawable = (ProgressDrawableControl)graphicsView.Drawable;
            //}

             
            {
                _startTime = DateTime.Now;
                _cancellationTokenSource = new CancellationTokenSource();
                IsAdInProgress = true;
                CrossMauiMTAdmob.Current.LoadRewarded(ConstantHelper.GoogleAdMob.RewardedId);
                CurrentAdValidatorData.RewardAdTimesLimit += 1;
                await _googleAdValidatorDataStoreService.AddOrUpdateItemAsync(CurrentAdValidatorData);
                _ = UpdateAdProgressCountDown();
            }
        }

        [SuppressPropertyChangedWarnings]
        partial void OnPassedDataChanged(string value)
        {
            this.Query = value;
        }

        private async Task UpdateAdProgressCountDown()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var elapsedTime = DateTime.Now - _startTime;
                if (elapsedTime.TotalMilliseconds >= TimeSpan.FromMinutes(1).TotalMilliseconds)
                {
                    _cancellationTokenSource.Cancel();
                    CountDownText = string.Empty;
                    return;
                }

                var runningTime = TimeSpan.FromMinutes(1) - elapsedTime;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CountDownText = runningTime.ToString("mm\\:ss");

                    //var percent = (int)((60 - runningTime.TotalSeconds) / 60f * 100);
                    //Console.WriteLine("++ percent " + percent);
                    //_drawable.Progress = percent;
                });
                await Task.Delay(500);
            }
        }
    }
}