using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using chabok_demo_wp.Class.Constant;
using chabok_demo_wp.Class.Helper;
using chabok_demo_wp.Class.Model;
using chabok_demo_wp.Class.Settings;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace chabok_demo_wp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterationPage : Page
    {
        public RegisterationPage()
        {
            this.InitializeComponent();
        }

        private async void BtnRegister_OnClick(object sender, RoutedEventArgs e)
        {
            var stateValidation = CheckValidations();

            if (stateValidation)
            {
                ShowWaitingDialog();

                var userId = TxtEmailAddress.Text;
                var channel = AppConstant.Channels;
                var userInfo = new UserInfoModel(TxtUserName.Text, TxtEmailAddress.Text, TxtCompany.Text);

                //Register user
                var state = await ADPPushSDK.AdpPushClient.Instance.Register(userId: userId, channelNames: channel);
                    
                if (state)
                {
                    AppSetting.IsRegisterd = true;
                    AppSetting.UserInformation = userInfo;
                    AppSetting.UserId = userId;

                    var userInfoDic = CreateUserInfoDictionary(userInfo);

                    //Set userInfo
                    await ADPPushSDK.AdpPushClient.Instance.SetUserInfo(userInfo: userInfoDic);

                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        delegate
                        {
                            HideWaitingDialog();
                            var frame = (Frame)Window.Current.Content;
                            frame.Navigate(typeof(MainPage));
                        });
                }
                else
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            HideWaitingDialog();
                            ShowMessage("خطا در ارسال اطلاعات به سرور.");
                        });
                }
            }
        }

        /// <summary>
        /// Check Email validation, User name and family was filled and Internet access.
        /// </summary>
        /// <returns>If it was True very thing is good.</returns>
        private bool CheckValidations()
        {
            if (!string.IsNullOrEmpty(TxtUserName.Text))
            {
                if (string.IsNullOrEmpty(TxtEmailAddress.Text))
                {
                    ShowMessage("لطفا آدرس ایمیل خود را وارد کنید.");
                    return false;
                }
                else if (!Regex.IsMatch(TxtEmailAddress.Text, AppConstant.EmailRegex))
                {
                    ShowMessage("آدرس ایمیل وارد شده صحیح نمی باشد.");
                    return false;
                }
            }
            else
            {
                ShowMessage("لطفا اطلاعات خود را وارد کنید.");
                return false;
            }

            if (!NetworkHelper.IsInternetConnected())
            {
                ShowMessage("شما به اینترنت دسترسی ندارید، لطفا مجددا تلاش کنید.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Show popup message with title of Error.
        /// </summary>
        /// <param name="content">Content of your message.</param>
        private async void ShowMessage(string content)
        {
            var messageDialog = new MessageDialog(content, "خطا");
            await messageDialog.ShowAsync();
        }

        /// <summary>
        /// Cast userInfoModel to a dictionary
        /// </summary>
        /// <param name="userInfo">UserInfoModel</param>
        /// <returns>Dictionary</returns>
        private Dictionary<string, object> CreateUserInfoDictionary(UserInfoModel userInfo)
        {
            return new Dictionary<string, object>()
            {
                {"FirstName",userInfo.FirstName},
                {"Company",userInfo.Company },
                {"EmailAddress",userInfo.EmailAddress }
            };
        }

        private async void ShowWaitingDialog()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
            {
                PopDialogWating.Show();
            });
        }

        private async void HideWaitingDialog()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
            {
                PopDialogWating.Hide();
            });
        }

        private void LoadinDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void TxtUserName_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                TxtCompany.Focus(FocusState.Programmatic);
                e.Handled = true;
            }
        }

        private void TxtCompany_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                TxtEmailAddress.Focus(FocusState.Programmatic);
                e.Handled = true;
            }
        }

        private void TxtEmailAddress_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                BtnRegister.Focus(FocusState.Programmatic);
                e.Handled = true;
            }
        }
    }
}
