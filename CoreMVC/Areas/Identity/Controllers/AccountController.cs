using Microsoft.AspNetCore.Mvc;
using CoreMVC.Areas.Identity.Models;
using CoreMVC.Data;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using CoreMvc.Api;

namespace CoreMVC.Areas.Identity.Controllers
{
    [Area(areaName:"Identity")]
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
                    AspControllerName= "Account",
                    AspActionName = "AssignRole",
                    Title="Assign Role",
                    PartialName= "~/Areas/Identity/Views/Account/_AssignRole.cshtml",
                    PartialModel= user
                };
                #endregion

            }
            return View(account);
        }
        #region--Login--
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            AccountModel account = await _userManager.FindByEmailAsync(login.Email);
            if(account != null)
            {
                var passwordOK = await _userManager.CheckPasswordAsync(account, login.Password);
                if (passwordOK)
                {
                    await _signInManager.SignInAsync(account, true);
                    return RedirectToAction("Index");
                }
            }

            ViewData["message"] = "Validator Faild";
            return View();
        }
        #endregion

        #region--Logout--
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region--Register--
        public IActionResult Register(string message)
        {
            ViewData["message"] = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            string returnUrl = $"http://{_config["AppSettings:HostName"]}" ;
            StringBuilder message = new StringBuilder();
            if (ModelState.IsValid)
            {
                AccountModel account = new AccountModel();
                account.CreationDate = DateTime.Now;
                account.UpdateDate = DateTime.Now;
                account.UserName = register.Email;
                account.Email = register.Email;

                var result = await _userManager.CreateAsync(account, register.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(account);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(account);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    Mail.SendGmail("Core MVC Confirm Email", $"Please click the link to confirm your email : {callbackUrl}", register.Email, _config);



                    return await Task.Run<IActionResult>(() => { return RedirectToAction("Login"); });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        message.Append(error.Description + ";");
                    }
                }

            }
            else
            {
                message.Append("Form Valid Faild!");
            }
            return await Task.Run<IActionResult>(() => { return RedirectToAction("Register",new { message = message.ToString()}); });

        }
        #endregion

        #region--Assign Role--
        [HttpPost]
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
    }
}
