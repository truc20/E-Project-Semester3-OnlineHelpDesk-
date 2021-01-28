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
    [Authorize(Roles = "Administrator")]
    [Route("status")]
    public class StatusController : Controller
    {
        private OnlineHelpDeskEntities db;
        public StatusController(OnlineHelpDeskEntities _db)
        {
            this.db = _db;
        }

        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {

            ViewBag.statuses = db.Statuses.ToList();
            return View("Index");
        }
        //--Add
        [HttpGet]
        [Route("add")]

        public IActionResult Add()
        {
            return View("Add", new Status());
        }

        [HttpPost]
        [Route("add")]

        public IActionResult Add(Status status )
        {
            try
            {
                db.Statuses.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                return View("Add", new Status());
            }

        }
        //--Add
        //--Delete
        [HttpGet]
        [Route("delete/{id}")]

        public IActionResult Delete(int id)
        {
            try
            {
                var status = db.Statuses.Find(id);
                db.Statuses.Remove(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                ViewBag.statuses = db.Statuses.ToList();
                return View("Index");
            }

        }
        //--Delete
        //--Edit
        [HttpGet]
        [Route("edit/{id}")]

        public IActionResult Edit(int id)
        {
            var status = db.Statuses.Find(id);
            return View("Edit", status);
        }
        [HttpPost]
        [Route("edit/{id}")]

        public IActionResult Edit(int id, Status status)
        {
            try
            {
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Edit", status);
            }
        }
        //--Edit


    }
}
