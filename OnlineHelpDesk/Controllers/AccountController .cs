using Microsoft.AspNetCore.Mvc;
using OnlineHelpDesk.Models;
using OnlineHelpDesk.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using OnlineHelpDesk.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace OnlineHelpDesk.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private OnlineHelpDeskEntities db;
        public AccountController(OnlineHelpDeskEntities _db)
        {
            this.db = _db;
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            
            ViewBag.accounts = db.Accounts.Where(a => a.Roleld != 1).ToList();

            return View("Index");
        }
        //--Add
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            /*.Where(a => a.Roleld != 1)*/
            var accountViewModel = new AccountViewModel();
            accountViewModel.Account = new Account();
            //đăng ký account không có admin
            var roles = db.Roles.Where(r => r.Id != 1).ToList();
            //đăng ký accout có admin
            //var roles = db.Roles.ToList();
            accountViewModel.Roles = new SelectList(roles, "Id", "Name");

            return View("Add",accountViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("add")]
        public IActionResult Add(AccountViewModel accountViewModel )
        {
            try
            {
                accountViewModel.Account.Password = BCrypt.Net.BCrypt.HashPassword(accountViewModel.Account.Password, BCrypt.Net.BCrypt.GenerateSalt());
                db.Accounts.Add(accountViewModel.Account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                return View("Add", accountViewModel);
            }
        }
        //--Add
        //--Delete
        [Authorize(Roles = "Administrator")]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var account = db.Accounts.SingleOrDefault(a=>a.Id==id && a.Roleld !=1);
                db.Accounts.Remove(account);
                db.SaveChanges();
                ViewBag.msg = "Done";
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
            }
            ViewBag.accounts = db.Accounts.Where(a => a.Roleld != 1).ToList();
            return View("Index");
        }
        //--Delete
        //--Edit
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            /*.Where(a => a.Roleld != 1)*/
            var accountViewModel = new AccountViewModel();
            accountViewModel.Account = db.Accounts.Find(id);
            var roles = db.Roles.Where(r => r.Id != 1).ToList();
            accountViewModel.Roles = new SelectList(roles, "Id", "Name");

            return View("Edit", accountViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id,AccountViewModel accountViewModel)
        {
            try
            {
                var password = db.Accounts.AsNoTracking().SingleOrDefault(a=>a.Id==id).Password;
                if (!string.IsNullOrEmpty(accountViewModel.Account.Password))
                {
                    password = BCrypt.Net.BCrypt.HashPassword(accountViewModel.Account.Password, BCrypt.Net.BCrypt.GenerateSalt());
                }
                accountViewModel.Account.Password = password;
                db.Entry(accountViewModel.Account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                return View("Edit", accountViewModel);
            }
        }
        //--Edit
        //-Profile
        [Authorize(Roles = "Administrator,Support,Employee")]
        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            return View("Profile", account);
        }




        [Authorize(Roles = "Administrator,Support,Employee")]
        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var currentAccount = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            try
            {
                
                //currentAccount.UserName = account.UserName;
                if (!string.IsNullOrEmpty(account.Password))
                {
                    currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password, BCrypt.Net.BCrypt.GenerateSalt());
                }
                currentAccount.Phone = account.Phone;
                currentAccount.FullName = account.FullName;
                currentAccount.Email = account.Email;
                db.SaveChanges();
                ViewBag.msg = "Done";
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";


            }
            return View("Profile", currentAccount);

        }
        //-Profile
    }
}
