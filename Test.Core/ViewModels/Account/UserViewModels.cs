using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Test.Core.Statics;

namespace Test.Core.ViewModels.Account
{
    public class MainUserViewModel
    {
        public Guid UserId { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "{0} اجباری می باشد")]
        public string UserName { get; set; }
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "{0} اجباری می باشد")]
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        [Display(Name = "رمزعبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsAccountActive { get; set; }
        public bool IsPhoneNumberActive { get; set; }
        public bool IsDelete { get; set; }
        public string IpAddress { get; set; }
        [Display(Name = "شهر")]
        public int? LocationId { get; set; }
        [Display(Name = "جنسیت")]
        public GenderEnum GenderEnum { get; set; }
    }

    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginViewModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginResultViewModel
    {
        public MainUserViewModel User { get; set; }
        public UserLoginResultEnum UserLoginResultEnum { get; set; }
    }

    public enum UserLoginResultEnum
    {
        Error,
        Success
    }

    public class ForgotPasswordViewModel
    {
        public string PhoneNumber { get; set; }
    }

}
