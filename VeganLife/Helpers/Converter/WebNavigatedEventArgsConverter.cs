// <copyright file="WebNavigatedEventArgsConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Maui.Converters;

namespace VeganLife.Helpers.Converter
{
    public class WebNavigatedEventArgsConverter : BaseConverterOneWay<WebNavigatedEventArgs, object>
    {
        public override object DefaultConvertReturnValue { get; set; }

        public override object ConvertFrom(WebNavigatedEventArgs value, CultureInfo culture = null!)
            => value switch
                {
                    null => null,
                    _ => value,
                };
    }
}
