using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using chabok_demo_wp.Class.Model;
using Newtonsoft.Json;

namespace chabok_demo_wp.Class.Settings
{
    public class AppSetting
    {
        private static ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        private AppSetting()
        {
            
        }

        public static bool IsRegisterd
        {
            get
            {
                var state = GetValue("isRegistered");
                if (!string.IsNullOrEmpty(state))
                {
                    return bool.Parse(state);
                }

                return false;
            }
            set
            {
                _settings.Values["isRegistered"] = value.ToString();
            }
        }

        public static string UserId
        {
            get
            {
                var state = GetValue("UserID");
                if (!string.IsNullOrEmpty(state))
                {
                    return state;
                }

                return string.Empty;
            }
            set
            {
                _settings.Values["UserID"] = value;
            }
        }

        public static UserInfoModel UserInformation
        {
            get
            {
                var json = GetValue("userInfo");
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<UserInfoModel>(json);
                }

                return null;
            }
            set
            {
                _settings.Values["userInfo"] = JsonConvert.SerializeObject(value);
            }
        }

        private static string GetValue(string key)
        {
            if (_settings.Values.ContainsKey(key))
            {
                var value = _settings.Values[key] as string;

                return value;
            }
            return null;
        }
    }
}
