// <copyright file="FoodDetailViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.FoodModel;
using static Android.Telephony.CarrierConfigManager;

namespace VeganLife.ViewModels.ContentViewModels
{
    /// <summary>
    /// vm for FoodDetailPage.
    /// </summary>
    public partial class FoodDetailViewModel : BaseViewModel
    {
        private readonly FoodDetailDataStoreService _foodDetailDataStoreService;

        [ObservableProperty]
        private FoodPreviewModel _foodPreview;

        [ObservableProperty]
        private FoodDetailModel _foodDetail;

        [ObservableProperty]
        private FoodNutrientFacts _foodNutriFacts;

        [ObservableProperty]
        private bool _isExpanded;
        [ObservableProperty]
        private bool _isShowingSwipeAnimation;

        [ObservableProperty]
        private ObservableCollection<string> _foodImage;
        [ObservableProperty]
        private int _selectedViewModelIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoodDetailViewModel"/> class.
        /// </summary>
        public FoodDetailViewModel()
            : base()
        {
            this._foodDetailDataStoreService = ServicesHelper.GetService<FoodDetailDataStoreService>();
            this.FoodImage = [];
        }

        /// <inheritdoc/>
        public override async Task OnNavigatingTo(object parameter)
        {
            await base.OnNavigatingTo(parameter);
            var imgs = new List<string>();

            if (parameter is not null)
            {
                this.FoodPreview = (FoodPreviewModel)parameter;
                if (FoodPreview is null)
                {
                }
                else
                {
                    this.FoodPreview.IsRead = true;
                    this.FoodImage.Add(this.FoodPreview?.Image ?? string.Empty);
                    if (this.IsNetworkConnected)
                    {
                        this.FoodDetail = await this.dataService.GetFoodDetail(this.FoodPreview?.Id ?? string.Empty);
                    }

                    if (this.FoodDetail == null)
                    {
                        this.FoodDetail = (await this._foodDetailDataStoreService.GetItemsAsync())?.FirstOrDefault()!;
                    }
                    else
                    {
                        await this._foodDetailDataStoreService.AddOrUpdateItemAsync(this.FoodDetail);
                    }

                    if (this.IsNetworkConnected)
                    {
                        imgs = await dataService.GetImageLinksAsync(this.FoodDetail.RootLink);
                    }
                }
            }

            foreach (var i in GetMoreImage(imgs))
            {
                FoodImage.Add(i);
            }

            IsShowingSwipeAnimation = true;
            _ = Task.Delay(5000).ContinueWith(t =>
            {
                IsShowingSwipeAnimation = false;
            });
        }

        [RelayCommand]
        private void OnBookmarkClicked()
        {
            if (this.FoodPreview == null)
            {
                return;
            }

            this.FoodPreview.BookmarkClickedCommand.Execute(null);
        }

        private bool _nutriFactsLoaded;

        [RelayCommand]
        private async Task GetNutriFacts()
        {
            if (_nutriFactsLoaded || GetNutriFactsCommand.IsRunning)
            {
                return;
            }

            FoodNutriFacts = await this.dataService.GetFoodNutriFacts(this.FoodPreview.Id);
            _nutriFactsLoaded = FoodNutriFacts != null;
            if (_nutriFactsLoaded)
            {
                // NutriDonutChart = new DonutChart()
                // {
                //    Entries = new ChartEntry[]
                //    {
                //        new(FoodNutriFacts?.Protein * 4)
                //        {
                //            Label = "Protein",
                //            ValueLabel = FoodNutriFacts?.Protein.ToString(),
                //            Color = SKColor.Parse("#ffa890"),
                //            ValueLabelColor = SKColors.Black,
                //            TextColor = SKColors.Gray,
                //        },
                //        new(FoodNutriFacts?.Carb * 4)
                //        {
                //            Label = "Carb",
                //            ValueLabel = FoodNutriFacts?.Carb.ToString(),
                //            Color = SKColor.Parse("#b7affe"),
                //            ValueLabelColor = SKColors.Black,
                //            TextColor = SKColors.Gray,
                //        },
                //        new(FoodNutriFacts?.Fat * 9)
                //        {
                //            Label = "Fat",
                //            ValueLabel = FoodNutriFacts?.Fat.ToString(),
                //            Color = SKColor.Parse("#ff7caa"),
                //            ValueLabelColor = SKColors.Black,
                //            TextColor = SKColors.Gray,
                //        },
                //    },
                //    LabelTextSize = 35,
                //    HoleRadius = 0.25f,
                //    GraphPosition = GraphPosition.AutoFill,
                //    LabelMode = LabelMode.RightOnly
                // };
            }
        }

        [RelayCommand]
        private void OnExpandClicked()
        {
            IsExpanded = !IsExpanded;

            // if (IsExpanded && nutriFactsLoaded && DailyRadialGaugeChart == null)
            // {
            //    DailyRadialGaugeChart = new RadialGaugeChart()
            //    {
            //        Entries = new ChartEntry[]
            //        {
            //            new (FoodNutriFacts.Sodium)
            //            {
            //                Label = nameof(FoodNutriFacts.Sodium),
            //                ValueLabel = FoodNutriFacts.Sodium.ToString() + "%",
            //                Color = SKColor.Parse("#aaabad"),
            //                ValueLabelColor = SKColors.Black,
            //                TextColor = SKColors.Gray,
            //            },
            //            new (FoodNutriFacts.A)
            //            {
            //                Label = nameof(FoodNutriFacts.A),
            //                ValueLabel = FoodNutriFacts.A.ToString() + "%",
            //                Color = SKColor.Parse("#fa8c21"),
            //                ValueLabelColor = SKColors.Black,
            //                TextColor = SKColors.Gray,
            //            },
            //            //new (FoodNutrientFacts.Iron)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.Iron),
            //            //    ValueLabel = FoodNutrientFacts.Iron.ToString() + "%",
            //            //    Color = SKColor.Parse("#1a1a18"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.Magnesium)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.Magnesium),
            //            //    ValueLabel = FoodNutrientFacts.Magnesium.ToString() + "%",
            //            //    Color = SKColor.Parse("#365977"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.Zinc)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.Zinc),
            //            //    ValueLabel = FoodNutrientFacts.Zinc.ToString() + "%",
            //            //    Color = SKColor.Parse("#C6C7B9"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            new (FoodNutriFacts.B1)
            //            {
            //                Label = nameof(FoodNutriFacts.B1),
            //                ValueLabel = FoodNutriFacts.B1.ToString() + "%",
            //                Color = SKColor.Parse("#175DBC"),
            //                ValueLabelColor = SKColors.Black,
            //                TextColor = SKColors.Gray,
            //            },
            //            //new (FoodNutrientFacts.B3)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.B3),
            //            //    ValueLabel = FoodNutrientFacts.B3.ToString() + "%",
            //            //    Color = SKColor.Parse("#FC0E0E"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.B9)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.B9),
            //            //    ValueLabel = FoodNutrientFacts.B9.ToString() + "%",
            //            //    Color = SKColor.Parse("#E28A20"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.E)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.E),
            //            //    ValueLabel = FoodNutrientFacts.E.ToString() + "%",
            //            //    Color = SKColor.Parse("#CFB84D"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.Calcium)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.Calcium),
            //            //    ValueLabel = FoodNutrientFacts.Calcium.ToString() + "%",
            //            //    Color = SKColor.Parse("#7B7D82"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            new (FoodNutriFacts.D)
            //            {
            //                Label = nameof(FoodNutriFacts.D),
            //                ValueLabel = FoodNutriFacts.D.ToString() + "%",
            //                Color = SKColor.Parse("#479F10"),
            //                ValueLabelColor = SKColors.Black,
            //                TextColor = SKColors.Gray,
            //            },
            //            //new (FoodNutrientFacts.Kali)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.Kali),
            //            //    ValueLabel = FoodNutrientFacts.Kali.ToString() + "%",
            //            //    Color = SKColor.Parse("#84933C"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.Phosphorus)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.Phosphorus),
            //            //    ValueLabel = FoodNutrientFacts.Phosphorus.ToString() + "%",
            //            //    Color = SKColor.Parse("#874C40"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.B2)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.B2),
            //            //    ValueLabel = FoodNutrientFacts.B2.ToString() + "%",
            //            //    Color = SKColor.Parse("#874C40"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.B6)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.B6),
            //            //    ValueLabel = FoodNutrientFacts.B6.ToString() + "%",
            //            //    Color = SKColors.Yellow,
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.B12)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.B12),
            //            //    ValueLabel = FoodNutrientFacts.B12.ToString() + "%",
            //            //    Color = SKColor.Parse("#F9C394"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //            //new (FoodNutrientFacts.K)
            //            //{
            //            //    Label = nameof(FoodNutrientFacts.K),
            //            //    ValueLabel = FoodNutrientFacts.K.ToString() + "%",
            //            //    Color = SKColor.Parse("#718B5A"),
            //            //    ValueLabelColor = SKColors.Black,
            //            //    TextColor = SKColors.Gray,
            //            //},
            //        },
            //        MaxValue = 100,
            //        LabelTextSize = 35,
            //    };
            // }
        }

        private List<string> GetMoreImage(List<string> imgs)
        {
            if (!(imgs?.Any() ?? false))
            {
                return [];
            }

            var filteredImages = imgs.Where(item =>
                !string.IsNullOrEmpty(item) &&
                !item.Contains("150") &&
                item.Contains(this.FoodDetail.Key) &&
                !FoodImage.Contains(item)).ToList();
            return filteredImages;

        }
    }
}
