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
            Table t = new Table();
            return RedirectToAction("Index", new { id = t.ID });
        }

        public ActionResult Index(int id)
        {
            if (!Table.AllTables.ContainsKey(id))
                return RedirectToAction("Index", "Home");

            ViewBag.TableID = id;
            return View();
        }
    }
}
