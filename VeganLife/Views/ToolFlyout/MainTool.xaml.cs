// <copyright file="MainTool.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.Base;
#if ANDROID
using Android.Widget;
#endif

namespace VeganLife.Views.ToolFlyout
{
    public partial class MainTool
    {
        public MainTool(MainToolViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }

#if ANDROID
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (slider.Handler?.PlatformView is SeekBar seekBar)
            {
                seekBar.ContentDescription = "this is slider";
            }
        }
#endif
    }
}