using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Plugin.Toasts;

namespace MdePe.Domain.Services.Api
{
    public class NotificationService
    {
        public async void GetNoticicationAsync(string title, string description)
        {
            var notificator = DependencyService.Get<IToastNotificator>();

            var options = new NotificationOptions()
            {
                Title = title,
                Description = description,
                IsClickable = true,
                WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
                ClearFromHistory = false,
                AllowTapInNotificationCenter = false,
                AndroidOptions = new AndroidOptions()
                {
                    HexColor = "#F99D1C",
                    ForceOpenAppOnNotificationTap = true
                }
            };

            await notificator.Notify(options);
        }
    }
}
