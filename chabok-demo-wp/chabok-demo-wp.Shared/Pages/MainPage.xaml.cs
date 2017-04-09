using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ADPPushSDK;
using ADPPushSDK.Enum;
using chabok_demo_wp.Pages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace chabok_demo_wp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            BtnMessage_OnClick(null,null);

            ConnectionState(ADPPushSDK.AdpPushClient.Instance.isConnected);

            ADPPushSDK.AdpPushClient.Instance.ConnectionStatusChanged+=InstanceOnConnectionStatusChanged;
            
        }

        private async void InstanceOnConnectionStatusChanged(AdpPushClient sender, ConnectionStatusType connectionStatus)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    ConnectionState(connectionStatus == ConnectionStatusType.Connected);
                });
        }

        private void ChangeTabVisualState(bool isAbout)
        {
            if (isAbout)
            {
                BtnAbout.BorderThickness = new Thickness(0,0,0,3);
                BtnMessage.BorderThickness = new Thickness(0);
            }
            else
            {
                BtnMessage.BorderThickness = new Thickness(0, 0, 0, 3);
                BtnAbout.BorderThickness = new Thickness(0);
            }
        }

        private void ConnectionState(bool isConnected)
        {
            if (isConnected)
            {
                TxtConnectionState.Text = "آنلاین";
                TxtConnectionState.Foreground = new SolidColorBrush(Colors.LimeGreen);
            }
            else
            {
                TxtConnectionState.Text = "آفلاین";
                TxtConnectionState.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }



        private void BtnAbout_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeTabVisualState(true);

            MainFrame.Navigate(typeof(AboutPage));
        }

        private void BtnMessage_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeTabVisualState(false);

            MainFrame.Navigate(typeof(MessengerPage));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            PopDialogSetting.Show();
        }

        private void BtnDismiss_OnClick(object sender, RoutedEventArgs e)
        {
            PopDialogSetting.Hide();
        }

        private void ToggleSwitchNotificationState_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((ToggleSwitch)sender).IsOn = AdpPushClient.Instance.IsShowToastNotification;
            ((ToggleSwitch)sender).Toggled +=OnToggled;
        }

        private void OnToggled(object sender, RoutedEventArgs routedEventArgs)
        {
            AdpPushClient.Instance.IsShowToastNotification = ((ToggleSwitch)sender).IsOn;
        }
    }
}
