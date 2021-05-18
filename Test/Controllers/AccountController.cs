using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Test.Core.Extensions.IdentityExtensions;
using Test.Core.Services.Interfaces;
using Test.Core.Statics;
using Test.Core.ViewModels.Account;
using Test.Extensions;

namespace Test.Controllers
{
    public class AccountController : Controller
    {

        #region Dependancy Injection

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string userName, string phoneNumber, string password)
        {

            var register = new RegisterViewModel()
            {
                Password = password,
                PhoneNumber = phoneNumber,
                UserName = userName
            };

            string referer = Request.Headers["Referer"].ToString();
            var userIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!ModelState.IsValid)
                return Redirect(referer);

            var isUserRegistered = await _accountService.RegisterUser(register, userIpAddress);

            if (isUserRegistered)
            {
                HttpContext.SetMessage(ActionMessageType.Success,"کد تایید ازطریق پیامک برای شما ارسال شد");
                return Redirect(referer);
            }

            HttpContext.SetMessage(ActionMessageType.Error, "شما قبلا با این شماره همراه ثبت نام کرده اید");
            return Redirect(referer);return Redirect(referer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string phoneNumber,string password)
        {
            var userLogin = new UserLoginViewModel()
            {
                Password = password,
                PhoneNumber = phoneNumber
            };

            string referer = Request.Headers["Referer"].ToString();

            var userIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var loginResult = await _accountService.UserLogIn(userLogin, userIpAddress);

            if (loginResult.UserLoginResultEnum==UserLoginResultEnum.Error)
            {
                HttpContext.SetMessage(ActionMessageType.Error,"شماره موبایل یا رمزعبور اشتباه است");
                return Redirect(referer);
            }

            var claims = new List<Claim>()

            {
                new Claim(ClaimTypes.Name, loginResult.User.UserName),
                new Claim(ClaimTypes.NameIdentifier, loginResult.User.UserId.ToString()),
                new Claim("MobileNumber", loginResult.User.PhoneNumber),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties()
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(principal, properties);

            HttpContext.SetMessage(ActionMessageType.Success, "");
            return Redirect(referer);
        }

        public async Task<IActionResult> Logout()
        {
            var referer = Request.Headers["Referer"].ToString();
            await _accountService.LogOutEvent(User.GetUserId());
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(referer);
        }

        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            return Redirect("/");
        }

        public IActionResult Showcase(Guid id)
        {
            return View();
        }
    }
}
