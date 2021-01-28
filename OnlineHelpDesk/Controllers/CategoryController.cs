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
    [Route("category")]
    public class CategoryController : Controller
    {
        private OnlineHelpDeskEntities db;
        public CategoryController(OnlineHelpDeskEntities _db)
        {
            this.db = _db;
        }
        
        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {

            ViewBag.categories = db.Categories.ToList();
            return View("Index");
        }
        //--Add
        [HttpGet]
        [Route("add")]
        
        public IActionResult Add()
        {
            return View("Add",new Category());
        }

        [HttpPost]
        [Route("add")]

        public IActionResult Add(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                return View("Add", new Category());
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
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.msg = "Failed";
                ViewBag.categories = db.Categories.ToList();
                return View("Index");
            }
            
        }
        //--Delete
        //--Edit
        [HttpGet]
        [Route("edit/{id}")]

        public IActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return View("Edit",category);
        }
        [HttpPost]
        [Route("edit/{id}")]

        public IActionResult Edit(int id,Category category)
        {
            try
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            { 
                ViewBag.msg = "Failed";
                return View("Edit",category);
            }
        }
        //--Edit


    }
}
