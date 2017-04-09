using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;

namespace chabok_demo_wp.Class.Helper
{
    public class NetworkHelper
    {
        private NetworkHelper()
        {
            
        }

        public static bool IsInternetConnected()
        {
            bool isConnected = NetworkInterface.GetIsNetworkAvailable();
            if (!isConnected)
            {
                return false;
            }
            else
            {
                ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                NetworkConnectivityLevel connection = internetConnectionProfile.GetNetworkConnectivityLevel();
                if (connection == NetworkConnectivityLevel.None || connection == NetworkConnectivityLevel.LocalAccess)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
