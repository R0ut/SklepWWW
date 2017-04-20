using SklepWWW.DAL;
using SklepWWW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SklepWWW.Controllers
{
    public class HomeController : Controller
    {
        private KursyContext db = new KursyContext();

        public ActionResult Index()
        {
            var listaKategorii = db.Kategorie.ToList();            

            return View();
        }
    }
}