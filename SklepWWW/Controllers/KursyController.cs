using SklepWWW.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SklepWWW.Controllers
{
    public class KursyController : Controller
    {
        private KursyContext db = new KursyContext();
        // GET: Kursy
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string nazwaKategori, string searchQuery = null)
        {
            var kategorie = db.Kategorie.Include("Kursy").Where(x => x.NazwaKategorii.ToUpper() == nazwaKategori.ToUpper()).Single();

            var kursy = kategorie.Kursy.Where(x => (searchQuery == null || x.TytulKursu.ToLower().Contains(searchQuery.ToLower()) ||
                                                x.AutorKursu.ToLower().Contains(searchQuery.ToLower())) &&
                                                !x.Ukryty);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_KursyList", kursy);
            }

            return View(kursy);
        }
        public ActionResult Szczegoly(string id)
        {

            var kurs = db.Kursy.Find(Convert.ToInt16(id));
            return View(kurs);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 60000)] // odpytamy baze tylko raz a potem bedzie brane z cache(odciarzenie bazy danych).Cache bedzie trzymał dane przez jeden dzien(60000)
        public ActionResult KategorieMenu()
        {
            var kategorie = db.Kategorie.ToList();

            return PartialView("_KategorieMenu", kategorie);
        }
        public ActionResult KursyPodpowiedzi(string term)
        {
            var kursy = db.Kursy.Where(x => !x.Ukryty && x.TytulKursu.ToLower().Contains(term.ToLower()))
                        .Take(5).Select(x => new { label = x.TytulKursu });


            return Json(kursy, JsonRequestBehavior.AllowGet);
        }

    }
}