using SklepWWW.DAL;
using SklepWWW.Infrastructure;
using SklepWWW.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SklepWWW.Controllers
{
    public class KoszykController : Controller
    {
        private KoszykMenager koszykMenager;
        private ISessionMenager sessionMenager { get; set; }
        private KursyContext db;

        public KoszykController()
        {
            db = new KursyContext();
            sessionMenager = new SessionMenager();
            koszykMenager = new KoszykMenager(sessionMenager, db);
        }

        // GET: Koszyk
        public ActionResult Index()
        {
            var pozycjaKoszyka = koszykMenager.PobierzKoszyk();
            var cenaCalkowita = koszykMenager.PobierzWartoscKoszyka();


            KoszykViewModel koszykVM = new KoszykViewModel
            {
                PozycjeKoszyka = pozycjaKoszyka,
                CenaCalkowita = cenaCalkowita
            };
            return View(koszykVM);
        }

        public ActionResult DodajDoKoszyka(int id)
        {
            koszykMenager.DodajDoKoszyka(id);
            return RedirectToAction("Index");
        }

        public int PobierzIloscElementowKoszyka()
        {
            return koszykMenager.PobierzIloscPozycjiKoszyka();
        }

        public ActionResult UsunZKoszyka(int kursId)
        {
            int iloscPozycji = koszykMenager.UsunZKoszyka(kursId);
            int iloscPozycjiKoszyka = koszykMenager.PobierzIloscPozycjiKoszyka();
            decimal wartoscKoszyka = koszykMenager.PobierzWartoscKoszyka();

            var wynik = new KoszykUsuwanieViewModel
            {
                IdPozycjiUsuwanej = kursId,
                IloscPozycjiUsuwanej = iloscPozycji,
                KoszykCenaCalkowita = wartoscKoszyka,
                KoszykIloscPozycji = iloscPozycjiKoszyka,
            };
            return Json(wynik);
        }
    }
}