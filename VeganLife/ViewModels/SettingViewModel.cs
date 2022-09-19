using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VeganLife.ViewModels
{
    public partial class SettingViewModel : ObservableObject
    {
        [ObservableProperty]
        bool _isDarkMode = false;

        public SettingViewModel()
        { }
    }
}
