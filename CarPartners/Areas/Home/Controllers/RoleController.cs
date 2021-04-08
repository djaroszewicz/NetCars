using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCars.Areas.Home.Models.Db.Account;
using NetCars.Areas.Home.Models.View.Role;

namespace NetCars.Areas.Home.Controllers
{
    [Area("home")]
    [Route("home/{controller}/{action=list}/{id?}")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleView role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = role.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Role");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult List()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction("List", "Role");
            }

            var model = new EditRoleView
            {
                Id = role.Id,
                RoleName = role.Name
            };

            List<User> users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var userChecker = await _userManager.IsInRoleAsync(user, role.Name);
                if (userChecker)
                {
                    if (model.Users == null)
                    {
                        model.Users = new List<string>();
                    }
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleView view)
        {
            var role = await _roleManager.FindByIdAsync(view.Id);

            if (role == null)
            {
                return RedirectToAction("List", "Role");
            }
            else
            {
                role.Name = view.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Role");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(view);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RedirectToAction("List", "Role");
            }

            List<User> users = _userManager.Users.ToList();
            var model = new List<UserRoleView>();

            foreach (var user in users)
            {
                var userRoleView = new UserRoleView
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleView.IsSelected = true;
                }
                else
                {
                    userRoleView.IsSelected = false;
                }
                model.Add(userRoleView);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(List<UserRoleView> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return RedirectToAction("List", "Role");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                { continue; }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("List", "Role");
                }
            }

            return RedirectToAction("List", "Role");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var result = await _roleManager.DeleteAsync(role);

            return RedirectToAction("List", "Role");
        }
    }
}