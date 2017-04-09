using System;
using System.Collections.Generic;
using System.Text;

namespace chabok_demo_wp.Class.Model
{
    public class UserInfoModel
    {
        public string FirstName { get; set; }

        public string EmailAddress { get; set; }

        public string Company { get; set; }

        /// <summary>
        /// User information model.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="company">Company name.</param>
        public UserInfoModel(string firstName,string emailAddress, string company = "")
        {
            FirstName = firstName;
            EmailAddress = emailAddress;
            Company = company;
        }
    }
}
