using DMTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DMTools.Controllers
{
    public class TableController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            Table t = new Table(name);
            return RedirectToAction("Index", new { id = t.ID });
        }

        public ActionResult Index(int id)
        {
            ViewBag.TableID = id;
            return View();
        }
    }
}
