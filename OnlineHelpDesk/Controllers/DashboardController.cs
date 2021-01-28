using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Controllers
{
    [Authorize(Roles = "Administrator,Support,Employee")]
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        private OnlineHelpDeskEntities db;
        public DashboardController(OnlineHelpDeskEntities _db)
        {
            this.db = _db;
        }
        [Route("Index")]
        [Route("")]
        
        public IActionResult Index()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            ViewBag.tickets = db.Tickets.Where(t => t.EmployeeId == account.Id).ToList();
            return View();
        }
    }
}
