
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using ANDP.ProvisionCenter.Mvc.Models;
using BrockAllen.MembershipReboot;
using Common.Lib.Security;

namespace ANDP.ProvisionCenter.Mvc.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private UserAccountService _userAccountService;
        private AuthenticationService _authService;
        private readonly Oauth2AuthenticationSettings _oauth2AuthenticationSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="oauth2AuthenticationSettings">The oauth2 authentication settings.</param>
        public AuthController(AuthenticationService authService, Oauth2AuthenticationSettings oauth2AuthenticationSettings)
        {
            _userAccountService = authService.UserAccountService;
            _authService = authService;
            _oauth2AuthenticationSettings = oauth2AuthenticationSettings;
        }

        [HttpGet]
        public ActionResult LogIn(string returnUrl, string renewSession)
        {
            bool value = false;
            bool.TryParse(renewSession, out value);

            var model = new LogInModel
            {
                ReturnUrl = returnUrl,
                RememberMe = "",
                RenewSession = value
            };

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                BrockAllen.MembershipReboot.UserAccount account;
                if (_userAccountService.Authenticate(model.TenantName, model.Username, model.Password, out account))
                {
                    _authService.SignIn(account);

                    if (account.RequiresTwoFactorAuthCodeToSignIn())
                    {
                        return RedirectToAction("TwoFactorAuthCodeLogin");
                    }
                    if (account.RequiresTwoFactorCertificateToSignIn())
                    {
                        return RedirectToAction("CertificateLogin");
                    }

                    if (_userAccountService.IsPasswordExpired(account))
                    {
                        return RedirectToAction("Index", "ChangePassword");
                    }

                    try
                    {
                        //Retrieve Web APi token
                        _oauth2AuthenticationSettings.Username = model.Username;
                        _oauth2AuthenticationSettings.TenantName = model.TenantName;
                        _oauth2AuthenticationSettings.Password = model.Password;
                        RetrieveWebApiTokenAndCreateCookie(_oauth2AuthenticationSettings);
                    }
                    catch (Exception ex)
                    {
                        _authService.SignOut();
                        ModelState.AddModelError("", ex.Message);
                        return View(model);
                    }

                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            //By setting to time in past cookie expires.
            var cookie = new HttpCookie(AuthenticationConstants.AngularAuthToken)
            {
                Value = "",
                Expires = DateTime.Now.AddMinutes(-1)
            };
            Response.Cookies.Add(cookie);

            _authService.SignOut();
            return RedirectToAction("index", "home");
        }

        private void RetrieveWebApiTokenAndCreateCookie(Oauth2AuthenticationSettings oauth2AuthenticationSettings)
        {
            var accessTokenResponse = BearerTokenHelper.RetrieveNewBearToken(oauth2AuthenticationSettings);

            var cookie = new HttpCookie(AuthenticationConstants.AngularAuthToken)
            {
                Value = accessTokenResponse.AccessToken,
                Expires = accessTokenResponse.ExpiresOn
            };
            Response.Cookies.Add(cookie);
        }
    }
}
