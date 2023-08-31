// <copyright file="FoodDetailViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    using Microcharts;
    using SkiaSharp;
    using VeganLife.Data.LocalData;
    using VeganLife.Helpers;
    using VeganLife.Models.FoodModel;
    using VeganLife.Services.LocalDataServices;

    /// <summary>
    /// vm for FoodDetailPage.
    /// </summary>
    public partial class FoodDetailViewModel : BaseViewModel
    {
        private FoodDetailDataStoreService foodDetailDataStoreService;
        [ObservableProperty]
        private FoodPreviewModel foodPreview;

        [ObservableProperty]
        private FoodDetailModel foodDetail;

        [ObservableProperty]
        private FoodNutriFacts foodNutriFacts;

        [ObservableProperty]
        private DonutChart nutriDonutChart;

        [ObservableProperty]
        private RadialGaugeChart dailyRadialGaugeChart;

        [ObservableProperty]
        private bool isExpanded;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoodDetailViewModel"/> class.
        /// </summary>
        public FoodDetailViewModel()
            : base()
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
            {
                this.foodDetailDataStoreService = new FoodDetailDataStoreService(database);
            }
        }

        /// <inheritdoc/>
        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            if (parameter is not null)
            {
                this.FoodPreview = parameter as FoodPreviewModel;
                this.FoodPreview.IsRead = true;

                if (this.IsNetworkConnected)
                {
                    this.FoodDetail = await this.dataService.GetFoodDetail(this.FoodPreview?.Id ?? string.Empty);
                }

                if (this.FoodDetail == null)
                {
                    this.FoodDetail = (await this.foodDetailDataStoreService.GetItemsAsync()).FirstOrDefault();
                }
                else
                {
                    await this.foodDetailDataStoreService.AddOrUpdateItemAsync(this.FoodDetail);
                }
            }

            return base.OnNavigatingTo(parameter);
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

        bool nutriFactsLoaded;

        [RelayCommand]
        private async Task GetNutriFacts()
        {
            if (nutriFactsLoaded || GetNutriFactsCommand.IsRunning)
            {
                return;
            }

            FoodNutriFacts = await this.dataService.GetFoodNutriFacts(this.FoodPreview.Id);
            nutriFactsLoaded = FoodNutriFacts != null;
            if (nutriFactsLoaded)
            {
                NutriDonutChart = new DonutChart()
                {
                    Entries = new ChartEntry[]
                    {
                        new(FoodNutriFacts.Protein * 4)
                        {
                            Label = "Protein",
                            ValueLabel = FoodNutriFacts.Protein.ToString(),
                            Color = SKColor.Parse("#ffa890"),
                            ValueLabelColor = SKColors.Black,
                            TextColor = SKColors.Gray,
                        },
                        new(FoodNutriFacts.Carb * 4)
                        {
                            Label = "Carb",
                            ValueLabel = FoodNutriFacts.Carb.ToString(),
                            Color = SKColor.Parse("#b7affe"),
                            ValueLabelColor = SKColors.Black,
                            TextColor = SKColors.Gray,
                        },
                        new(FoodNutriFacts.Fat * 9)
                        {
                            Label = "Fat",
                            ValueLabel = FoodNutriFacts.Fat.ToString(),
                            Color = SKColor.Parse("#ff7caa"),
                            ValueLabelColor = SKColors.Black,
                            TextColor = SKColors.Gray,
                        },
                    },
                    LabelTextSize = 35,
                    HoleRadius = 0.25f,
                    GraphPosition = GraphPosition.AutoFill,
                    LabelMode = LabelMode.RightOnly
                };
            }
        }

        [RelayCommand]
        private void OnExpandClicked()
        {
            IsExpanded = !IsExpanded;
            if (IsExpanded && nutriFactsLoaded && DailyRadialGaugeChart == null)
            {
                DailyRadialGaugeChart = new RadialGaugeChart()
                {
                    Entries = new ChartEntry[]
                    {
                        new (FoodNutriFacts.Sodium)
                        {
                            Label = nameof(FoodNutriFacts.Sodium),
                            ValueLabel = FoodNutriFacts.Sodium.ToString() + "%",
                            Color = SKColor.Parse("#aaabad"),
                            ValueLabelColor = SKColors.Black,
                            TextColor = SKColors.Gray,
                        },
                        new (FoodNutriFacts.A)
                        {
                            Label = nameof(FoodNutriFacts.A),
                            ValueLabel = FoodNutriFacts.A.ToString() + "%",
                            Color = SKColor.Parse("#fa8c21"),
                            ValueLabelColor = SKColors.Black,
                            TextColor = SKColors.Gray,
                        },
                        //new (FoodNutriFacts.Iron)
                        //{
                        //    Label = nameof(FoodNutriFacts.Iron),
                        //    ValueLabel = FoodNutriFacts.Iron.ToString() + "%",
                        //    Color = SKColor.Parse("#1a1a18"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.Magnesium)
                        //{
                        //    Label = nameof(FoodNutriFacts.Magnesium),
                        //    ValueLabel = FoodNutriFacts.Magnesium.ToString() + "%",
                        //    Color = SKColor.Parse("#365977"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.Zinc)
                        //{
                        //    Label = nameof(FoodNutriFacts.Zinc),
                        //    ValueLabel = FoodNutriFacts.Zinc.ToString() + "%",
                        //    Color = SKColor.Parse("#C6C7B9"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        new (FoodNutriFacts.B1)
                        {
                            Label = nameof(FoodNutriFacts.B1),
                            ValueLabel = FoodNutriFacts.B1.ToString() + "%",
                            Color = SKColor.Parse("#175DBC"),
                            ValueLabelColor = SKColors.Black,
                            TextColor = SKColors.Gray,
                        },
                        //new (FoodNutriFacts.B3)
                        //{
                        //    Label = nameof(FoodNutriFacts.B3),
                        //    ValueLabel = FoodNutriFacts.B3.ToString() + "%",
                        //    Color = SKColor.Parse("#FC0E0E"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.B9)
                        //{
                        //    Label = nameof(FoodNutriFacts.B9),
                        //    ValueLabel = FoodNutriFacts.B9.ToString() + "%",
                        //    Color = SKColor.Parse("#E28A20"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.E)
                        //{
                        //    Label = nameof(FoodNutriFacts.E),
                        //    ValueLabel = FoodNutriFacts.E.ToString() + "%",
                        //    Color = SKColor.Parse("#CFB84D"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.Calcium)
                        //{
                        //    Label = nameof(FoodNutriFacts.Calcium),
                        //    ValueLabel = FoodNutriFacts.Calcium.ToString() + "%",
                        //    Color = SKColor.Parse("#7B7D82"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        new (FoodNutriFacts.D)
                        {
                            Label = nameof(FoodNutriFacts.D),
                            ValueLabel = FoodNutriFacts.D.ToString() + "%",
                            Color = SKColor.Parse("#479F10"),
                            ValueLabelColor = SKColors.Black,
                            TextColor = SKColors.Gray,
                        },
                        //new (FoodNutriFacts.Kali)
                        //{
                        //    Label = nameof(FoodNutriFacts.Kali),
                        //    ValueLabel = FoodNutriFacts.Kali.ToString() + "%",
                        //    Color = SKColor.Parse("#84933C"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.Phosphorus)
                        //{
                        //    Label = nameof(FoodNutriFacts.Phosphorus),
                        //    ValueLabel = FoodNutriFacts.Phosphorus.ToString() + "%",
                        //    Color = SKColor.Parse("#874C40"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.B2)
                        //{
                        //    Label = nameof(FoodNutriFacts.B2),
                        //    ValueLabel = FoodNutriFacts.B2.ToString() + "%",
                        //    Color = SKColor.Parse("#874C40"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.B6)
                        //{
                        //    Label = nameof(FoodNutriFacts.B6),
                        //    ValueLabel = FoodNutriFacts.B6.ToString() + "%",
                        //    Color = SKColors.Yellow,
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.B12)
                        //{
                        //    Label = nameof(FoodNutriFacts.B12),
                        //    ValueLabel = FoodNutriFacts.B12.ToString() + "%",
                        //    Color = SKColor.Parse("#F9C394"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                        //new (FoodNutriFacts.K)
                        //{
                        //    Label = nameof(FoodNutriFacts.K),
                        //    ValueLabel = FoodNutriFacts.K.ToString() + "%",
                        //    Color = SKColor.Parse("#718B5A"),
                        //    ValueLabelColor = SKColors.Black,
                        //    TextColor = SKColors.Gray,
                        //},
                    },
                    MaxValue = 100,
                    LabelTextSize = 35,
                };
            }
        }
    }
}
