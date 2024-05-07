namespace VeganLife.Views.Controls
{
    using PanCardView;
    using VeganLife.Helpers.Extensions;

    class CardCoverFlowViewControl : CoverFlowView
    {
        private readonly object _locker = new object();
        private CancellationTokenSource _slideShowTokenSource;
        private bool _hasRenderer;
        private bool _isInteracting;

        public CardCoverFlowViewControl()
        {
            UserInteracted += NoAnimationCoverFlowView_UserInteracted;
            this.UserInteractionDelay = 50;
            this.IsUserInteractionEnabled = false;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(this.ItemsCount):
                    this.IsUserInteractionEnabled = this.ItemsCount > 1;
                    break;
                case "Renderer":
                    _hasRenderer = !_hasRenderer;
                    break;
            }
        }

        protected override Task HardSetAsync()
        {
            if (this.ItemsCount > 1)
            {
                SetAllViews(true);
                this.RemoveRedundantViews();
            }

            return Task.FromResult(true);
        }

        private void NoAnimationCoverFlowView_UserInteracted(CardsView view, PanCardView.EventArgs.UserInteractedEventArgs args)
        {
            switch (args.Status)
            {
                case PanCardView.Enums.UserInteractionStatus.Started:
                    _isInteracting = true;
                    break;
                case PanCardView.Enums.UserInteractionStatus.Ended:
                    _isInteracting = false;
                    break;
            }
        }

        /// <summary>
        /// CardsView.AdjustSlideShow is not good, re-implement by Task
        /// </summary>
        protected override void AdjustSlideShow(bool isForceStop = false)
        {
            if (this.ItemsCount > 1)
            {
                _slideShowTokenSource?.Cancel();
                if (isForceStop)
                {
                    return;
                }

                if (SlideShowDuration > 0)
                {
                    _slideShowTokenSource = new CancellationTokenSource();
                    Task.Run(() => SlideShowAsync(_slideShowTokenSource.Token)).ConfigureAwait(false);
                }
            }
            else
            {
                base.AdjustSlideShow(isForceStop);
            }
        }

        private async Task SlideShowAsync(CancellationToken token)
        {
            await Task.Delay(SlideShowDuration).ConfigureAwait(false);
            lock (_locker)
            {
                if (_slideShowTokenSource?.Token != token || token.IsCancellationRequested)
                {
                    return;
                }
            }

            if (ItemsCount > 0 && _hasRenderer)
            {
                // issue 367 - bug only appear on android
                if (DeviceInfo.Current.Platform == DevicePlatform.iOS || !_isInteracting)
                    await MainThread.InvokeOnMainThreadAsync(() => this.SetSelectedIndexWithShouldAutoNavigateToNext(true)).ConfigureAwait(false);
            }

            await SlideShowAsync(token).ConfigureAwait(false);
        }

        private bool RemoveRedundantViews()
        {
            if (this.ItemsCount > 1 && this.Children.Count > this.ItemsCount)
            {
                var unusedViews = new HashSet<View>();
                var childViews = this.Children.Reverse<IView>().ToList();
                var totalChild = childViews.Count;
                var bindingContextsSeen = new HashSet<object>();

                for (int i = 0; i < totalChild; i++)
                {
                    var iViewTarget = childViews[i];
                    if (iViewTarget is not View view || CheckIsProtectedView(iViewTarget) ||
                        iViewTarget == CurrentView || NextViews.Contains(iViewTarget) ||
                        PrevViews.Contains(iViewTarget))
                    {
                        continue;
                    }

                    if (!bindingContextsSeen.Add(view.BindingContext))
                    {
                        // If we can't add the binding context, it means we've seen it before
                        unusedViews.Add(view);
                    }
                }

                if (unusedViews.Any())
                {
                    this.RemoveChildren(unusedViews.ToArray());
                    return true;
                }
            }

            // return false if no views were removed
            return false;
        }
    }
}
