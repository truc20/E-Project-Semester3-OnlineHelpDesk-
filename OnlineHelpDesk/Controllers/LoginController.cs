using Microsoft.AspNetCore.Mvc;
using OnlineHelpDesk.Models;
using OnlineHelpDesk.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
namespace OnlineHelpDesk.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private OnlineHelpDeskEntities db;
        public LoginController(OnlineHelpDeskEntities _db)
        {
            this.db = _db;
        }
        [Route("Index")]
        [Route("")]
        [Route("/")]

        public IActionResult Index()
        {
            

            return View();
        }
        
        [HttpPost]
        [Route("process")]
        public IActionResult Process(string username, string password)
        {
            var account = check(username, password);
            if (account!=null)
            {
                SecurityManager securityManager = new SecurityManager();
                securityManager.SignIn(HttpContext, account);
                return RedirectToAction("Index", "Dashboard");

            }
            else
            {
                ViewBag.error = "Invalid";
                return View("Index");
            }
            
        }
        [Route("SignOut")]

        public IActionResult SignOut()
        {

            SecurityManager securityManager = new SecurityManager();
            securityManager.SignOut(HttpContext);
            return RedirectToAction("Index");
        }
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        { 
            return View("AccessDenied");
        }
        
        private Account check(string username,string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username) && a.Status==true);
            if (account !=null)
            {
                try
                {
                    //đăng nhập có mã hoá password
                    if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                    {
                        return account;
                    }
                    //đăng nhập không mã hoá password
                    /*if (account.Password.Equals(password))
                    {
                        return account;
                    }*/
                }
                catch (Exception)
                {

                    NotFound();
                }
                
            }
            return null;
        }
    }
}
