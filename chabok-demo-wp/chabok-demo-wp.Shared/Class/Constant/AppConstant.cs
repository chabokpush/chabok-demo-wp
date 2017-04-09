using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media;

namespace chabok_demo_wp.Class.Constant
{
    public class AppConstant
    {
        private AppConstant()
        {
            
        }

        public static readonly string EmailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                          @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static readonly string[] Channels = new[] {"public/wall"};

        public const string AppId = "APPLICATION_ID";
        public const string ApiKey = "API_KEY";
        public const string Username = "USERNAME";
        public const string Password = "PASSWORD";

        public static FontFamily IranSansFontFamily = new FontFamily("/Fonts/IRAN Sans.ttf#IRANSans");
    }
}
