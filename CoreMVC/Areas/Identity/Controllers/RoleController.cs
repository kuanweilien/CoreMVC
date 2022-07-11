using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CoreMVC.Areas.Identity.Models;

namespace CoreMVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AccountRoleModel> _role;
        private readonly IRoleStore<AccountRoleModel> _roleStore;
        public RoleController(RoleManager<AccountRoleModel> role,
                            IRoleStore<AccountRoleModel> roleStore
            )
        {
            _role = role;
            _roleStore = roleStore;
        }

        public IActionResult Index()
        {
            return View(_role.Roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AccountRoleModel role)
        {
            if (ModelState.IsValid)
            {
                role.CreationDate = DateTime.Now;
                role.UpdateDate = DateTime.Now;
                await _role.CreateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AccountRoleModel role)
        {
            AccountRoleModel newRole = _role.Roles.Where(x => x.Id == role.Id).First();
            if(newRole != null)
            {
                newRole.UpdateDate = DateTime.Now;
                newRole.Name = role.Name;
                await _role.UpdateAsync(newRole);
            }
                

            return RedirectToAction("Index");
        }
        public IActionResult AssignUser()
        {
            return View();
        }
    }
}
