using MvcSiteMapProvider.Caching;
using SklepWWW.DAL;
using SklepWWW.Infrastructure;
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

            ICacheProvider cashe = new DefaultCacheProvider();
            List<Kurs> nowosci;

            if(cashe.IsSet(Consts.NowosciCasheKey)) //jak w cashe jest juz pobrane z bazy to ustawia z cashe w przeciwnym wypadku pobiera z bazy i ustawia wartość cashe na ta z bazy
            {
                nowosci = cashe.Get(Consts.NowosciCasheKey) as List<Kurs>;
            }
            else
            {
                nowosci = db.Kursy.Where(x => !x.Ukryty).OrderByDescending(x => x.DataDodania).Take(3).ToList();
                cashe.Set(Consts.NowosciCasheKey, nowosci, 60);
            }
            List<Kurs> bestseller;

            if (cashe.IsSet(Consts.BestsellerCasheKey))
            {
                bestseller = cashe.Get(Consts.BestsellerCasheKey) as List<Kurs>;
            }
            else
            {
                bestseller = db.Kursy.Where(x => !x.Ukryty && x.Bestseller).OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                cashe.Set(Consts.BestsellerCasheKey, bestseller, 60);
            }

            List<Kategoria> kategorie;

            if (cashe.IsSet(Consts.KategorieCasheKey))
            {
                kategorie = cashe.Get(Consts.KategorieCasheKey) as List<Kategoria>;
            }
            else
            {
                kategorie = db.Kategorie.ToList();
                cashe.Set(Consts.KategorieCasheKey, kategorie, 60);
            }

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