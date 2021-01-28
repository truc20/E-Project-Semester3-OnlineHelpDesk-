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
using OnlineHelpDesk.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace OnlineHelpDesk.Controllers
{
    [Route("ticket")]
    public class TicketController : Controller
    {
        private OnlineHelpDeskEntities db;
        [Obsolete]
        private IHostingEnvironment ihostingEnvironment;

        [Obsolete]
        public TicketController(OnlineHelpDeskEntities _db,IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }
        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("send")]
        public IActionResult Send()
        {
            var ticketViewModel = new TicketViewModel();
            ticketViewModel.Ticket = new Ticket();

            var categories = db.Categories.Where(r => r.Status ).ToList();
            ticketViewModel.Categories = new SelectList(categories, "Id", "Name");

            var statues = db.Statuses.Where(r => r.Display ).ToList();
            ticketViewModel.Statuses = new SelectList(statues, "Id", "Name");

            var periods = db.Periods.Where(r => r.Status).ToList();
            ticketViewModel.Periods = new SelectList(periods, "Id", "Name");


            return View("Send", ticketViewModel);
        }

        [HttpPost]
        [Route("send")]
        [Obsolete]
        public IActionResult Send(TicketViewModel ticketViewModel, IFormFile[] files)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name).Value;
                var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
                ticketViewModel.Ticket.CreateDate = DateTime.Now;
                ticketViewModel.Ticket.EmployeeId = account.Id;
                db.Tickets.Add(ticketViewModel.Ticket);
                db.SaveChanges();
                //upload photos for ticket(optional)
                if (files !=null && files.Length>0)
                {
                    foreach (var file in files)
                    {
                        var fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + file.FileName;
                        var path = Path.Combine(ihostingEnvironment.WebRootPath, "uploads", fileName);
                        var stream = new FileStream(path, FileMode.Create);
                        file.CopyToAsync(stream);
                        //save photo to database
                        var photo = new Photo();
                        photo.Name = fileName;
                        photo.TicketId = ticketViewModel.Ticket.Id;
                        db.Photos.Add(photo);
                        db.SaveChanges();
                    }
                }
                TempData["msg"] = "Done";
              
            }
            catch (Exception)
            {
                TempData["msg"] = "Failed";

                ViewBag.msg = "Failed"; 
            }
            
            return RedirectToAction("Send");
        }
        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("history")]
        public IActionResult History()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            ViewBag.tickets = db.Tickets.OrderByDescending(t => t.Id).Where(t => t.EmployeeId == account.Id).ToList();
            return View("History");
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
           
            ViewBag.tickets = db.Tickets.OrderByDescending(t => t.Id).ToList();
            return View("List");
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("Assign")]
        public IActionResult Assign()
        {

            ViewBag.tickets = db.Tickets.Where(t=>t.Supporter==null).OrderByDescending(t => t.Id).ToList();
            return View("Assign");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("Assign")]
        public IActionResult Assign(int id,int supporterId)
        {
            var ticket = db.Tickets.Find(id);
            ticket.SupporterId = supporterId;
            db.SaveChanges();
            ViewBag.tickets = db.Tickets.Where(t => t.Supporter == null).OrderByDescending(t => t.Id).ToList();
            return RedirectToAction("List");
        }
        //load ticket is assigned to supporter
        [Authorize(Roles = "Support")]
        [HttpGet]
        [Route("Assigned")]
        public IActionResult Assigned()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            ViewBag.tickets = db.Tickets.Where(t => t.SupporterId ==account.Id).OrderByDescending(t => t.Id).ToList();
            return View("Assigned");
        }

        [Authorize(Roles = "Administrator,Support,Employee")]
        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {

            ViewBag.ticket = db.Tickets.Find(id);
            ViewBag.supporters = db.Accounts.Where(a => a.Roleld == 2 && a.Status == true).ToList();
            ViewBag.dicussions = db.Discussions.Where(d => d.TicketId == id).OrderBy(d => d.Id).ToList();
            return View("Details");
        }
        

        [Authorize(Roles = "Support,Employee")]
        [HttpPost]
        [Route("send_discussion")]
        public IActionResult SendDiscussion(int ticketId,string message)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            var discussion = new Discussion();
            discussion.CreateDate = DateTime.Now;
            discussion.Content = message;
            discussion.TicketId = ticketId;
            discussion.AccountId = account.Id;
            db.Discussions.Add(discussion);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = ticketId });

        }
    }
}
