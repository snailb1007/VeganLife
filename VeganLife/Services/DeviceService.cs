// <copyright file="DeviceService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Maui.Platform;
using VeganLife.Helpers.Extensions;

namespace VeganLife.Services
{
    public partial class DeviceService : IDeviceService
    {
        public void HideKeyboard()
        {
            #if ANDROID
            if (Platform.CurrentActivity?.CurrentFocus != null)
            {
                Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
            }
            #endif

        }

        // public bool IsVirtual = DeviceInfo.Current.DeviceType switch { DeviceType.Physical => false, DeviceType.Virtual => true, _ => false };

        // public double WidthScreen => Application.Current?.MainPage?.Width ?? default;
        // public double HeightScreen => Application.Current?.MainPage?.Height ?? default;
        public async Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> ccrecipients = null)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    Cc = ccrecipients,
                    BodyFormat = EmailBodyFormat.PlainText, // Use EmailBodyFormat.Html for HTML content
                };

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                fbsEx.LogError(description: "Email is not supported on this device");
            }
            catch (Exception ex)
            {
                ex.LogError(description: "Some other exception occurred");
            }
        }
    }
}
