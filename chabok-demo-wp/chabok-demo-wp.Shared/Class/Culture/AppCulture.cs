using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace shared.Class.Culture
{
    class AppCulture
    {
        public static void SetCulture()
        {
            var value = new CultureInfo("fa-IR");
            ApplicationLanguages.PrimaryLanguageOverride = value.Name;
            CultureInfo.DefaultThreadCurrentCulture = value;
            CultureInfo.DefaultThreadCurrentUICulture = value;
            //CultureInfo.CurrentCulture = value;
            //CultureInfo.CurrentUICulture = value;
            UpdateLang(value.Name);
        }
        private static async void UpdateLang(string newLang)
        {
            var resourceContext = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView();
            while (true)
            {
                if (resourceContext.Languages[0] == newLang)

                {
                    break;
                }
                await Task.Delay(100);
            }
        }

        private AppCulture()
        {
            
        }
    }
}
