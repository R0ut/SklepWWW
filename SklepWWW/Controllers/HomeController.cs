using SklepWWW.DAL;
using SklepWWW.Models;
using SklepWWW.ViewModels;
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
            var kategorie = db.Kategorie.ToList();
            var nowosci = db.Kursy.Where(x => !x.Ukryty).OrderByDescending(x => x.DataDodania).Take(3).ToList();
            var bestseller = db.Kursy.Where(x => !x.Ukryty && x.Bestseller).OrderBy(x => Guid.NewGuid()).Take(3).ToList();

            var vm = new HomeViewModel()
            {
                Kategorie = kategorie,
                Nowosci = nowosci,
                Bestsellery = bestseller
            };

            return View(vm);
        }

        public ActionResult StronyStatyczne(string nazwa)
        {
            return View(nazwa);
        }

        
    }
}