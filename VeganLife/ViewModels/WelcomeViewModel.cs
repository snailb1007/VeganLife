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
        
        public WelcomeViewModel(INavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            Init();
        }
      
        void Init()
        {
            ListImage = new List<WelcomeImage>
            {
                new WelcomeImage("welcome1.jpg"),
                new WelcomeImage("welcome2.jpg"),
            };
        }
    }
}
