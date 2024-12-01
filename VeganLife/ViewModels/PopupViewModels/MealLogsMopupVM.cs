using VeganLife.Resources.Translations;

namespace VeganLife.ViewModels.PopupViewModels
{
    public partial class MealLogsMopupVM : BaseViewModel
    {
        public DateTime Today => DateTime.Now.Date;

        [ObservableProperty]
        private IList<DateTime> _selectedDate = new List<DateTime>();

        [ObservableProperty]
        private CultureInfo _calendarCultureInfo;

        public MealLogsMopupVM()
        {
            CalendarCultureInfo = new CultureInfo("vi-VN");
        }

        [RelayCommand]
        private async Task ClosePopupAsync()
        {
            busyManager.Increase();
            await this.popupNaviService.PopAsync();
            busyManager.Decrease();
        }

        [RelayCommand]
        private void ClearClicked()
        {
            SelectedDate.Clear();
        }

        partial void OnSelectedDateChanged(IList<DateTime> value)
        {
            if (!value.Any())
            {
                return;
            }

            var x = value.LastOrDefault();
            if (x.Date > DateTime.Now.Date)
            {
                //SelectedDate.Remove(x);
                Console.WriteLine("wrong selected");
            }
        }
    }
}
