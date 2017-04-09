using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace chabok_demo_wp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
        }

        private async void BtnLink_OnClick(object sender, RoutedEventArgs e)
        {
            if (BtnLink.Content != null) await Launcher.LaunchUriAsync(new Uri(BtnLink.Content.ToString()));
        }

        private void BtnPhone_OnClick(object sender, RoutedEventArgs e)
        {
            #if WINDOWS_PHONE_APP
                if (BtnPhone.Content != null)
                    Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI("آتیه داده پرداز", BtnPhone.Content.ToString());
             #endif
        }
    }
}
