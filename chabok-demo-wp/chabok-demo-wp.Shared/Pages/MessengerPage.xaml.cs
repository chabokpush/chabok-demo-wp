using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ADPPushSDK;
using ADPPushSDK.BaseModel;
using ADPPushSDK.Enum;
using ADPPushSDK.Model;
using chabok_demo_wp.Class.Cache;
using chabok_demo_wp.Class.Constant;
using chabok_demo_wp.Class.Settings;
using Newtonsoft.Json;
using WP_nosilverlight_App;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace chabok_demo_wp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessengerPage : Page
    {
        ObservableCollection<Message> _messagesCollection = new ObservableCollection<Message>();

        public MessengerPage()
        {
            this.InitializeComponent();

            BtnSend.IsEnabled = !string.IsNullOrEmpty(TxtMessage.Text);

            #region Control Events

            Loaded += OnLoaded;

            BtnSend.Click += BtnSendOnClick;

            TxtMessage.TextChanged += TxtSendMessageOnTextChanged;

            TxtMessage.KeyDown += TxtSendMessageOnKeyDown;

            #endregion

            #region AdpPushClient
            
            if (ADPPushSDK.AdpPushClient.Instance.Badge > 0)
            {
                AdpPushClient.Instance.RestBadge();
            }

            #endregion
        }

        #region Event methods

        private void TxtSendMessageOnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Accept)
            {
                BtnSendOnClick(null, null);
                e.Handled = true;
            }
        }

        private void TxtSendMessageOnTextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSend.IsEnabled = !string.IsNullOrEmpty(TxtMessage.Text);
        }

        private void BtnSendOnClick(object sender, RoutedEventArgs e)
        {

            var user = GetUserName();

            var msg = new PushMessage(TxtMessage.Text, user, AppConstant.Channels[0]);

            AdpPushClient.Instance.Publish(msg, delegate (PushMessage message)
            {
                LocalDatabase.Instance.InsertMessageToDatabase(message);

                AddMessageToList(message, "sent");

            }, delegate (Exception exception)
            {
                AddMessageToList(msg, "fail");
            });

            TxtMessage.Text = string.Empty;

            ScrollToLastItem();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, LoadMessages);
            ScrollToLastItem();
            App.MessageReceived += AppOnMessageReceived;
        }
        
        private async void AppOnMessageReceived(object sender)
        {
            ADPPushSDK.Model.PushMessageReceive message = sender as PushMessageReceive;

            if (message != null)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate
                {
                    AddMessageToList(message);

                    LstChabokChat.ItemsSource = _messagesCollection;
                    ScrollToLastItem();
                });
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Scroll listbox to last item when message was added to list
        /// </summary>
        private async void ScrollToLastItem()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate
            {
                if (LstChabokChat.Items != null && LstChabokChat.Items.Count > 0)
                    LstChabokChat.ScrollIntoView(LstChabokChat.Items[LstChabokChat.Items.Count - 1]);
            });
        }

        #region AddMessageToList

        private void AddMessageToList(PushMessageReceive message)
        {
            if (message.Data != null)
            {
                var name = message.Data["name"] as string;
                AddMessageToList(message.Id, message.Body, message.CreatedAt, true, name);
            }
            else
            {
                AddMessageToList(message.Id, message.Body, message.CreatedAt, true,"چابک رسان");
            }
        }

        private void AddMessageToList(PushMessage message, string status)
        {
            AddMessageToList(message.Id, message.Body, message.CreatedAt, false,"", status);
        }

        private void AddMessageToList(PushMessageReceive message, string status)
        {
            AddMessageToList(message.Id, message.Body, message.CreatedAt, false,"", status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <param name="createdAt"></param>
        /// <param name="toFrom"></param>
        /// <param name="sender"></param>
        /// <param name="status"></param>
        private async void AddMessageToList(string id, string body, long? createdAt, bool toFrom,string sender, string status = "")
        {
            try
            {
                if (_messagesCollection.Any(message1 => message1.Id == id))
                    return;

                var msg = new Message()
                {
                    TextMessage = body,
                    Time = PushMessageBaseModel.TimeMillisToDateTime(createdAt).ToLocalTime().ToString("g"),
                    tofrom = toFrom,
                    Id = id,
                    Status = status,
                    UserNameText = sender
                };

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate
                {
                    if (!string.IsNullOrEmpty(msg.TextMessage))
                    {
                        _messagesCollection.Add(msg);
                    }
                });
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion

        private void LoadMessages()
        {
            var messages = LocalDatabase.Instance.GetMessgeFromDatabase();

            if (messages != null)
            {
                foreach (var pushMessageReceive in messages)
                {
                    if (pushMessageReceive.Data!=null && pushMessageReceive.Data.Count > 1)
                    {
                        AddMessageToList(pushMessageReceive, "sent");
                    }
                    else
                    {
                        AddMessageToList(pushMessageReceive); 
                    }
                }

                LstChabokChat.ItemsSource = _messagesCollection;

                ScrollToLastItem();
            }
        }

        private Dictionary<string, object> GetUserName()
        {
            var user = new Dictionary<string, object>() { { "name", AppSetting.UserInformation.FirstName }, {"me","true"} };

            return user;
        }

        #endregion
    }
}
