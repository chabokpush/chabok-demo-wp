using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ADPPushSDK.Model;
using chabok_demo_wp.Class.Model;
using Newtonsoft.Json;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace chabok_demo_wp.Class.Cache
{
    class LocalDatabase
    {
        private int _keepRecords = 15;
        static string _sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "appCache.sqlite");
        private static LocalDatabase _instance;
        public static LocalDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LocalDatabase();
                    CreateDatabase();
                }
                return _instance;
            }
        }
        private LocalDatabase()
        {

        }
        private static void CreateDatabase()
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), _sqlpath))
            {
                conn.CreateTable<PushMessageModel>();
            }
        }
        private void InsertIntoTable<T>(T data, bool removeAll = true)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), _sqlpath))
                {
                    conn.RunInTransaction(delegate
                    {
                        if (removeAll)
                            conn.DeleteAll<T>();

                        conn.Insert(data);

                        Debug.WriteLine(String.Format("Inserted on {0}", typeof(T).Name));
                    });
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                //throw exception;
            }
        }

        public void InsertMessageToDatabase(ADPPushSDK.BaseModel.PushMessageBaseModel pushMessageReceive)
        {
            try
            {
                var json = JsonConvert.SerializeObject(pushMessageReceive);

                var message = new PushMessageModel(json);

                InsertIntoTable(message,false);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        public PushMessageReceive GetMessgeFromDatabaseWithId(string id)
        {
            var messages = GetMessgeFromDatabase();

            foreach (var pushMessageReceive in messages)
            {
                if (pushMessageReceive.Id == id)
                {
                    return pushMessageReceive;
                }
            }

            return null;
        }

        public List<PushMessageReceive> GetMessgeFromDatabase()
        {
            try
            {
                var lstPushMessage = new List<PushMessageReceive>();

                using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), _sqlpath))
                {
                    var allQuery = conn.Table<PushMessageModel>();

                    var listQuery = allQuery?.ToList();

                    foreach (var pushMessageModel in listQuery)
                    {
                        lstPushMessage.Add(JsonConvert.DeserializeObject<PushMessageReceive>(pushMessageModel.Json));
                    }

                    return lstPushMessage;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                return null;
            }
            //return null;
        }

    }
}
