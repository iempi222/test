using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain.Models.Account
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Password { get; set; }
        public GenderEnum Gender { get; set; }
        public bool IsAccountActive { get; set; }
        public bool IsPhoneNumberActive { get; set; }
        public bool IsDelete { get; set; }
        public string IpAddress { get; set; }
        public int? LocationId { get; set; }


        #region Relations

        public Location.Location Location { get; set; }

        #endregion
    }

    public enum GenderEnum
    {
        UnKnown,
        Female,
        Male,
    }
}
