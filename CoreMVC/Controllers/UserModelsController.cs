using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreMVC.Data;
using CoreMVC.Models.User;
using System.Text;
using CoreMvc.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace CoreMVC.Controllers
{

    public class UserModelsController : Controller
    {
        private readonly MariaDBContext _context;
        private readonly ConfigurationManager _config;
        private readonly string _passwordKey;

        public UserModelsController(MariaDBContext context,ConfigurationManager config
            )
        {
            _context = context;
            _config = config;
            _passwordKey = _config["AppSettings:PasswordKey"];
        }
        #region--Index--
        // GET: UserModels
        public async Task<IActionResult> Index()
        {
              return _context.UserModel != null ? 
                          View(await _context.UserModel.ToListAsync()) :
                          Problem("Entity set 'MariaDBContext.UserModel'  is null.");
        }
        #endregion

        #region--Detail--
        // GET: UserModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }
        #endregion

        #region--Create--
        // GET: UserModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,UserName,Password,Sex,Address,CreateDate,UpdateDate")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(_passwordKey))
                {
                    throw new ArgumentNullException("PasswordKey is null");
                }
                
                if (!string.IsNullOrEmpty(userModel.Password))
                {
                    userModel.Password = Security.PasswordEncrypt(userModel.Password, _passwordKey);
                }
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }
        #endregion

        #region--Edit--
        // GET: UserModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: UserModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,UserName,Password,Sex,CreateDate,UpdateDate")] UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: UserModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }
        #endregion

        #region--Delete--
        // POST: UserModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserModel == null)
            {
                return Problem("Entity set 'MariaDBContext.UserModel'  is null.");
            }
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel != null)
            {
                _context.UserModel.Remove(userModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool UserModelExists(int id)
        {
          return (_context.UserModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region--Login--
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult Login(string Email, string Password)
        {
            if (string.IsNullOrEmpty(_passwordKey))
            {
                throw new ArgumentNullException("PasswordKey is null");
            }

            Password = Security.PasswordEncrypt(Password, _passwordKey);
            IEnumerable<UserModel> users = _context.UserModel.Where(x => x.Email == Email && x.Password == Password);
            
            if(users.Count() > 0)
            {
                
                Dictionary<string, string> token = Security.GetLoginToken(Email);
                Response.Cookies.Append("token", token["account"] + "|" + token["token"]);
                HttpContext.Session.SetString(token["account"] , token["token"]);

                ViewData["LoginResult"] = "Login Success";
                return View("Index", users);
            }
            else
            {
                ViewData["LoginResult"] = "Login Failed , Please check your email or password";
                return View();
            }
            
        }

        #endregion

    }
}
