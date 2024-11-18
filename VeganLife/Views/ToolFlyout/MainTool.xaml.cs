// <copyright file="MainTool.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Android.Widget;
using VeganLife.Views.Base;

namespace VeganLife.Views.ToolFlyout
{
    public partial class MainTool
    {
        public MainTool(MainToolViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (slider.Handler?.PlatformView is SeekBar seekBar)
            {
                seekBar.ContentDescription = "this is slider";
            }
        }
    }
}