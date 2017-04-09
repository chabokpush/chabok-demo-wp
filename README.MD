# راهنما پیاده سازی Chabok Push Notification SDK
----------
ابزار پوش چابک تحت چارچوب دات نت بوده و قابل استفاده در سیستم عامل های ویندوز فون 8.1 و ویندوز 8.1 و بالاتر می باشد.

----------
### راهنما دریافت ابزار چابک
.شما می توانید از طریق سایت [ناگت](https://www.nuget.org/packages/ADPPushSDK) اقدام به دریافت این ابزار نمایید

 **نصب از طریق سایت ناگت**
 با استفاده از دستور زیر در Package Manager Console می توانید اقدام به نصب پکیج نمایید.

    PM> Install-Package ADPPushSDK
و یا از طریق Manage NuGet Package اقدام به نصب پگیج [Chabok Push](https://www.nuget.org/packages/ADPPushSDK) نمایید.

![enter image description here](http://uupload.ir/files/cuo4_nuget_package_manager.png)

### پیکربندی ابزار چابک

----------
**دریافت مجوز WNS***

> نکته : اگر نام اپلیکیشن خود را در [داشبورد استور](https://developer.microsoft.com/en-us/dashboard/) Reserve نکرده اید، عملیات زیر را انجام دهید.

بر روی پروژه خود راست کلیک کرده از بخش Store بر روی  Associate your app with the windows store کلیک کنید. سپس در کادر باز شده بر روی دکمه Next کلیک کنید و وارد مرحله بعد شود. سپس وارد اکانت استور خودتان شوید و سپس در کادر متن زیر نام اپلیکیشن خود را وارد کرده و بر روی دکمه reserve کلیک نمایید. حال وارد [داشبورد استور](https://developer.microsoft.com/en-us/dashboard/) شده و بر روی نام اپلیکیشنی که رزرو کرده اید کلیک نمایید. در پنل سمت چپ از بخش Services بر روی Push notifications کلیک کرده و در آخر بر روی WNS/MPNS کلیک نمایید. در صفحه فوق بر روی لینک Live Services site کلیک کنید.

در این صفحه شما به Application Secrets ,  Package SID , Application Identity دسترسی خواهید داشت.

> نکته Application Secrets ,  Package SID :1 را در پنل چابک وارد نمایید.
> ![enter image description here](http://uupload.ir/files/lhg8_application_identity.png)
> 
> ![enter image description here](http://uupload.ir/files/hzbi_application_secret.png)

>  نکته 2 : Application Identity را همانطور که درصفحه فوق بیان شده به فایل Package.appxmanifest اضافه نمایید.
> 
> ![enter image description here](http://uupload.ir/files/lhg8_application_identity.png)

** افزودن تنظیمات به فایل Package.appxmanifest **

فایل فوق را به صورت نمایش کد باز کرده و کدهای زیر را به Application ضمیمه کنید 

    <Extensions>
        <Extension Category="windows.backgroundTasks" EntryPoint="ADPPushBackgroundTask.BackgroundTask.InternetStatusChangeTask">
          <BackgroundTasks>
            <Task Type="systemEvent" />
          </BackgroundTasks>
        </Extension>
        <Extension Category="windows.backgroundTasks" EntryPoint="ADPPushBackgroundTask.BackgroundTask.PushNotificationTask">
          <BackgroundTasks>
            <Task Type="pushNotification" />
          </BackgroundTasks>
        </Extension>
      </Extensions>
   
همچنین کد Application Identity را نیز به تگ Package اضافه نمایید.

     <Identity Name="c7d46ec*-****-****-****-************" Publisher="CN=****" Version="1.0.0.0" />
 جهت فعال سازی Badge و Tile کد زیر را هم به VisualElements اضافه کنید.
 
    <uap:LockScreen Notification="badgeAndTileText" BadgeLogo="Assets\Square44x44Logo.png" />

>  نکته : برای استفاده از ابزار فوق در ویندوز 8.1 باید ویژگی زیر را به تگ VisualElements اضافه کنید.
> `<m2:VisualElements ToastCapable="true" DisplayName="Chabok" .... />`

**مقداری اولیه**

 متد  Init  برای مقدار دهی پارامتر های ضروری استفاده می شود. به فراخوانی متد فوق دقت فرمایید.

    await ADPPushSDK.AdpPushClient.Instance.Init(appId: "", apiKey: "", username: "", password: "");

مقادیر هر یک از پارامترها در پنل سایت چابک موجود می باشد.

> appId : محل خط قرمز در تصویر زیر
> 
> ![enter image description here](http://uupload.ir/files/tno3_appid.png)

apiKey : محل خط قرمز در تصویر زیر

> 
> ![enter image description here](http://uupload.ir/files/cz5d_eaaz_appkey.png)

 نکته : در صورت استفاده از حالت برنامه نویس می توانید از قطعه کد زیر بعد از متد Init استفاده نمایید.
> 
> `ADPPushSDK.AdpPushClient.Instance.SetDevelopment(true);`

در پایان شما باید برای کاربر خود یک userId تعیین نمایید، قطعه کد زیر نحوه استفاده از کانال ها را به شما نشان می دهد.

>  نکته : userId می تواند هر عبارتی باشد ( شماره تلفن، ایمیل، کد ملی، یک آی دی یونیک و ...) اما حتما باید به آن مقدار دهی نمایید.
>  نکته : کانال آرایه می باشد که می خواهید کاربر فقط به کانال های خاصی دسترسی داشته باشد تا در صورت ارسال پیام کاربرانی که عضو آن کانال هستند قادر به دریافت پیام هستند.

    await ADPPushSDK.AdpPushClient.Instance.Register(userId: "", channelNames: new string[] {"news", "wall", "public/sport"});

**دریافت پیام**

برای دریافت پیام از رویداد زیر استفاده نمایید.

    ADPPushSDK.AdpPushClient.Instance.PushNotificationReceived+= delegate(AdpPushClient sender, PushMessageReceive receive)
            {
                //When push message was received successfully
                string message = receive.Body;

            };

**وضعیت اتصال**

جهت دریافت وضعیت اتصال ابزار از رویداد زیر استفاده نمایید.

    AdpPushClient.Instance.ConnectionStatusChanged+= delegate(AdpPushClient sender, ConnectionStatusType status)
    {
	    if (status == ConnectionStatusType.Connected)
	    {
                    
        }
        else if (status == ConnectionStatusType.Connecting)
        {
                    
        }
        else if (status == ConnectionStatusType.Disconnected)
        {
                    
        }
        else
        {
	        //ConnectionStatusType.NotInitialized
        }
    };

### متد های دیگر

----------

> نکته : متد هایی که در زیر آورده ایم، حتما باید بعد از متد Register فراخوانی شوند.

**ارسال اطلاعات کاربر**

با استفاده از متد زیرشما می توانید بعد از دریافت اطلاعات کاربر آن را به متد زیر داده و آن را در پنل چابک ذخیره نمایید.

    await ADPPushSDK.AdpPushClient.Instance.SetUserInfo(userInfo: new Dictionary<string, object>() {{"FirstName", "John"}, {"Email", "John@adpdigital.com"}});
  
**دریافت اطلاعات کاربر**

اگر اطلاعات کاربر را قبلا با استفاده از متد قبل ذخیره کرده باشید، می توانید با استفاده از تابع زیر آن را دریافت نمایید.

    Dictionary<string, object> userInfo = await ADPPushSDK.AdpPushClient.Instance.GetUserInfo();

**ارسال پیام**
 با استفاده از متد زیر شما قادر به ارسال پیام به یک کانال خاص خواهید بود.
 

     PushMessage message = new ADPPushSDK.Model.PushMessage(body: "Hello World!",data: null,channel: "userId/news");
     
     ADPPushSDK.AdpPushClient.Instance.Publish(message, delegate(PushMessage pushMessage)
     {
	     //The message was sent successfully

     }, delegate(Exception exception)
     {
	     //The message was not sent successfully.

     });
**Reset badge**

با استفاده از متد زیر شما می توانید Badge را  Reset نمایید.

    if (AdpPushClient.Instance.Badge > 0)
    {
	    ADPPushSDK.AdpPushClient.Instance.RestBadge();
    }