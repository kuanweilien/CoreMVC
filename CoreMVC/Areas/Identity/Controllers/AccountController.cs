using Microsoft.AspNetCore.Mvc;
using CoreMVC.Areas.Identity.Models;
using CoreMVC.Data;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using CoreMvc.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace CoreMVC.Areas.Identity.Controllers
{
    [Area(areaName:"Identity")]
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly MariaDBContext _context;
        private readonly SignInManager<AccountModel> _signInManager;
        private readonly UserManager<AccountModel> _userManager;
        private readonly RoleManager<AccountRoleModel> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ConfigurationManager _config;
        public AccountController(MariaDBContext context, 
                                SignInManager<AccountModel> signInManager,
                                UserManager<AccountModel> userManager,
                                RoleManager<AccountRoleModel> roleManager,
                                ILogger<RegisterModel> logger,
                                ConfigurationManager config
                                )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _config = config;
        }
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Index()
        {
            List<AccountModel> account = _userManager.Users.ToList();
            List<AccountRoleModel> roles = _roleManager.Roles.ToList();
            foreach (var user in account)
            {
                #region--Set User Role--
                List<AccountRoleModel> userRoles = new List<AccountRoleModel>(roles.Count());
                roles.ForEach((item) =>
                {
                    userRoles.Add((AccountRoleModel)item.Clone());
                });
                var userRole = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    role.Checked = userRole.Contains(role.Name);
                }
                user.Roles = userRoles;
                #endregion

                #region--Set Dialog--
                user.AssignRoleDialog = new CoreMVC.Models.DialogModel()
                {
                    AspAreaName = "Identity",
                    AspControllerName = "Account",
                    AspActionName = "AssignRole",
                    Title = "Assign Role",
                    DialogButtonName = "Roles",
                    PartialName = "~/Areas/Identity/Views/Account/_AssignRole.cshtml",
                    PartialModel = user
                };
                #endregion

            }
            return View(account);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        #region--Login--
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToHome();
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var account = await _userManager.FindByEmailAsync(login.Email);
            
            if (account != null)
            {
                var passwordOK = await _userManager.CheckPasswordAsync(account, login.Password);
                if (passwordOK)
                {
                    await _signInManager.SignInAsync(account, true);
                    return RedirectToHome();
                }
            }

            ViewData["message"] = "Validator Faild";
            return View();
            
        }
        #endregion

        #region--Logout--
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion

        #region--Register--
        public IActionResult Register(string message)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                ViewData["message"] = message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                AccountModel account = new AccountModel();
                account.CreationDate = DateTime.Now;
                account.UpdateDate = DateTime.Now;
                account.Email = register.Email;
                account.UserName = register.Email.Split('@').FirstOrDefault();

                return await RunRegister(account, register.Password);

            }
            else
            {
                return await Task.Run<IActionResult>(() => { return RedirectToAction("Register", new { message = "Form Valid Faild!" }); });
            }

        }
        private async Task<IActionResult> RunRegister(AccountModel account,string password = "")
        {
            IdentityResult result;
            if (string.IsNullOrEmpty(password))
            {
                result = await _userManager.CreateAsync(account);
            }
            else
            {
                result = await _userManager.CreateAsync(account, password);
            }

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                if (!string.IsNullOrEmpty(password))
                {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(account);
                    SendMail(account, "Core MVC Confirm Email", "Please click the link to confirm your email :", token, "ConfirmEmail");
                }
                else
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(account);
                    await _userManager.ConfirmEmailAsync(account, code);
                }

                //add default role
                await _userManager.AddToRoleAsync(account, "user");

                return RedirectToAction("Login");
            }
            else
            {
                TempData["message"] = GetErrorMessage(result);
            }
            return RedirectToAction("Register");
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string message = "";
            AccountModel account = await _userManager.FindByIdAsync(userId);
            if (account != null)
            {
                token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

                IdentityResult result = await _userManager.ConfirmEmailAsync(account, token);
                if (result.Succeeded)
                {
                    message = "Sueccess";
                    await _signInManager.SignInAsync(account, false);

                }
            }
            else
            {
                message = "Failed";
            }
            return RedirectToHome(message);
        }
        #endregion

        #region--Delete User--
        [HttpPost]
        [Authorize(Roles ="admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string email)
        {
            AccountModel account =await _userManager.FindByEmailAsync(email);
            IdentityResult result = await _userManager.DeleteAsync(account);
            if (!result.Succeeded)
            {
                ViewData["message"] = "DeleteFailed";
            }
            return RedirectToAction("Index", "Account");
        }
        #endregion

        #region--Assign Role--
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="admin")]
        public async Task< IActionResult> AssignRole(IEnumerable<AccountRoleModel> roles,string Id)
        {
            AccountModel account = await _userManager.FindByIdAsync(Id);
            foreach (var role in roles)
            {
                if (role.Checked)
                {
                    await _userManager.AddToRoleAsync(account,role.NormalizedName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(account, role.NormalizedName);
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region--Google OAuth2--
        [HttpPost]
        public IActionResult GoogleRegister()
        {
            return new ChallengeResult(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponseRegister", "Account") // Where google responds back
                });
        }
        public async Task<IActionResult> GoogleResponseRegister()
        {
            //Check authentication response as mentioned on startup file as o.DefaultSignInScheme = "External"
            var authenticateResult = await HttpContext.AuthenticateAsync("External");
            if (!authenticateResult.Succeeded)
            {
                ViewData["message"] = "Validator Faild";
                return RedirectToAction("Login"); // TODO: Handle this better.
            }
            AccountModel info = GetAccountFromGoogle(authenticateResult.Principal);
            AccountModel account = await _userManager.FindByEmailAsync(info.Email);
            if(account == null)
            {
                return await RunRegister(info);
            }
            else
            {
                return await Task.Run<IActionResult>(() => { return RedirectToAction("Register", new { message = "Email Exists!" }); });
            }
            
        }
        [HttpPost]
        public IActionResult GoogleLogin()
        {
            return new ChallengeResult(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponseLogin", "Account") // Where google responds back
                });
        }
        public async Task<IActionResult> GoogleResponseLogin()
        {

            //Check authentication response as mentioned on startup file as o.DefaultSignInScheme = "External"
            var authenticateResult = await HttpContext.AuthenticateAsync("External");
            if (!authenticateResult.Succeeded) 
            {
                ViewData["message"] = "Validator Faild";
                return RedirectToAction("Login"); // TODO: Handle this better.
            }
            //Check Email Exists
            AccountModel account = await _userManager.FindByEmailAsync(GetAccountFromGoogle(authenticateResult.Principal).Email);
            if(account == null)
            {
                return RedirectToAction("Register");
            }
            else
            {
                await _signInManager.SignInAsync(account, isPersistent: false);
                return RedirectToHome();
            }
            
        }
        private AccountModel GetAccountFromGoogle(ClaimsPrincipal claims)
        {
            AccountModel account = new AccountModel();
            account.Email = claims.FindFirstValue(ClaimTypes.Email);
            account.UserName = claims.FindFirstValue(ClaimTypes.Email).Split('@').First();
            return account;
        }
        #endregion

        #region--Reddirect--
        public IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        public IActionResult RedirectToHome(string message)
        {
            return RedirectToAction("Index", "Home", new { Area = "",Message = message });
        }
        #endregion

        #region--Reset Password--
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPasswordSendMail(string email)
        {
            AccountModel account =await _userManager.FindByEmailAsync(email);
            if(account != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(account);
                SendMail(account, "CoreMVC Reset Password", "Please click the link to reset passwrod : ", token, "ResetPassword");
                TempData["message"] = "Go your Email to click reset password link";
            }
            else
            {
                TempData["message"] = "Email not found";
            }

            return RedirectToAction("ForgetPassword");
        }
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            ForgetPasswordModel forget = new ForgetPasswordModel();
            AccountModel account = await _userManager.FindByIdAsync(userId);
            if (account != null)
            {
                token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
                forget.Email = account.Email;
                forget.Token = token;
            }
            return View(forget);
        }
        public async Task<IActionResult> ResetPasswordConfirm(ForgetPasswordModel forget)
        {
            if (ModelState.IsValid)
            {
                AccountModel account = await _userManager.FindByEmailAsync(forget.Email);
                IdentityResult result = await _userManager.ResetPasswordAsync(account, forget.Token, forget.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["message"] = GetErrorMessage(result);
                }
            }
            return RedirectToAction("ResetPassword");
        }
        #endregion

        #region--Send Mail--
        public async void SendMail(AccountModel account,string subject,string content,string token,string returnAction)
        {
            string returnUrl = $"{_config["AppSettings:Protocol"]}://{_config["AppSettings:HostName"]}";

            var userId = await _userManager.GetUserIdAsync(account);
            

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = Url.Action(
                action: returnAction,
                controller: "Account",
                values: new { area = "Identity", userId = userId, token = token, returnUrl = returnUrl },
                protocol: Request.Scheme);

            Mail.SendGmail(subject, content + $" {callbackUrl} ", account.Email, _config);
        }

        #endregion

        private string GetErrorMessage(IdentityResult result,string messageKey = "Message")
        {
            StringBuilder message = new StringBuilder();
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(messageKey, error.Description);
                message.Append(error.Description + ";");
            }
            return message.ToString();
        }
    }
}
