using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganLife.ViewModels
{
    public class WelcomeImage
    {
        public WelcomeImage(string image)
        {
            this.Image = image;
        }
        public string Image { get; set; }
    }

    public partial class WelcomeViewModel : BaseViewModel
    {
        [ObservableProperty]
        IList<WelcomeImage> _listImage;
        [ObservableProperty]
        private int _currentItem;

        public WelcomeViewModel()
            : base()
        {
            Init();
        }

        void Init()
        {
            ListImage = new List<WelcomeImage>
            {
                new WelcomeImage("slide_image1.jpg"),
                new WelcomeImage("slide_image2.jpg"),
                new WelcomeImage("slide_image3.jpg")
            };
        }

        [RelayCommand]
        void HandleButtonPrevious()
        {
            if (CurrentItem <= 0)
            {
                CurrentItem = 0;
                return;
            }
            else
            {
                CurrentItem--;
            }
        }

        [RelayCommand]
        void HandleButtonNext()
        {
            if (CurrentItem > 3)
            {
                CurrentItem = 3;
                return;
            }
            else
            {
                CurrentItem++;
            }
        }
    }
}
