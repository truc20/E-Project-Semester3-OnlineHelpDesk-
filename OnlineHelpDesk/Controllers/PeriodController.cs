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
    [Route("period")]
    public class PeriodController : Controller
    {
        private OnlineHelpDeskEntities db;
        public PeriodController(OnlineHelpDeskEntities _db)
        {
            this.db = _db;
        }

        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {

            ViewBag.periods = db.Periods.ToList();
            return View("Index");
        }
        //--Add
        [HttpGet]
        [Route("add")]

        public IActionResult Add()
        {
            return View("Add", new Period());
        }

        [HttpPost]
        [Route("add")]

        public IActionResult Add(Period period)
        {
            try
            {
                db.Periods.Add(period);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                return View("Add", new Period());
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
                var period = db.Periods.Find(id);
                db.Periods.Remove(period);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                ViewBag.periods = db.Periods.ToList();
                return View("Index");
            }

        }
        //--Delete
        //--Edit
        [HttpGet]
        [Route("edit/{id}")]

        public IActionResult Edit(int id)
        {
            var period = db.Periods.Find(id);
            return View("Edit", period);
        }
        [HttpPost]
        [Route("edit/{id}")]

        public IActionResult Edit(int id, Period period )
        {
            try
            {
                db.Entry(period).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Edit", period);
            }
        }
        //--Edit


    }
}
