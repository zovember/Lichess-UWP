using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace NavigationApp
{
    class Toaster
    {
        public static void Show(string text)
        {
            ToastContent toastContent = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = text
                            }
                        },
                        /*AppLogoOverride = new ToastGenericAppLogo
                        {
                            Source = "disconnect.png",
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }*/
                    }
                }
            };
            var toast = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
