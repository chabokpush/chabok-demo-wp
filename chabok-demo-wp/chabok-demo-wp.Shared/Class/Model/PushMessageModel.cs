using System;
using System.Collections.Generic;
using System.Text;

namespace chabok_demo_wp.Class.Model
{
    class PushMessageModel
    {
        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int Id { get; set; }
        public string Json { get; set; }

        public PushMessageModel( string json)
        {
            Json = json;
        }

        public PushMessageModel(int id, string json)
        {
            Id = id;
            Json = json;
        }

        public PushMessageModel()
        {
            
        }
    }
}
