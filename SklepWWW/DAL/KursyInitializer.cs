using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SklepWWW.Models;
using SklepWWW.DAL;
using SklepWWW.Migrations;
using System.Data.Entity.Migrations;

namespace SklepWWW.DAL
{
    public class KursyInitializer : MigrateDatabaseToLatestVersion<KursyContext,Configuration>
    {
        //protected override void Seed(KursyContext context)
        //{
        //    SeedKursyData(context);
        //    base.Seed(context);
        //}

        public static void SeedKursyData(KursyContext context)
        {
            var kategorie = new List<Kategoria>
           {
               new Kategoria() {KategoriaId = 1,NazwaKategorii = "Asp.Net", NazwaPlikuIkony="aspnet.png",OpisKategorii = "Opis asp net mvc" },
               new Kategoria() {KategoriaId = 2,NazwaKategorii = "JavaScript", NazwaPlikuIkony="javascript.png",OpisKategorii = "Opis java" },
               new Kategoria() {KategoriaId = 3,NazwaKategorii = "jQuery", NazwaPlikuIkony="jquery.png",OpisKategorii = "Opis php" },
               new Kategoria() {KategoriaId = 4,NazwaKategorii = "Html5", NazwaPlikuIkony="html.png",OpisKategorii = "Opis html" },
               new Kategoria() {KategoriaId = 5,NazwaKategorii = "Css3", NazwaPlikuIkony="css.png",OpisKategorii = "Opis css" },
               new Kategoria() {KategoriaId = 6,NazwaKategorii = "Xml", NazwaPlikuIkony="xml.png",OpisKategorii = "Opis xml" },
               new Kategoria() {KategoriaId = 7,NazwaKategorii = "C#", NazwaPlikuIkony="csharp.png",OpisKategorii = "Opis c#" },
            };

            kategorie.ForEach(k => context.Kategorie.AddOrUpdate(k));
            context.SaveChanges();



            var kursy = new List<Kurs>
         {
             new Kurs() {AutorKursu = "Tomek", TytulKursu="asp.net mvc", KategoriaId = 1, CenaKursu = 99, Bestseller = true, NazwaPlikuObrazka="obrazekaspnet.png",
             DataDodania = DateTime.Now, OpisKursu = "opis kursu" },
             new Kurs() {AutorKursu = "Jacek", TytulKursu="asp.net mvc 3", KategoriaId = 1, CenaKursu = 120, Bestseller = true, NazwaPlikuObrazka="obrazekmvc.png",
             DataDodania = DateTime.Now, OpisKursu = "opis kursu mvc3" },
             new Kurs() {AutorKursu = "Irek", TytulKursu="asp.net mvc 4", KategoriaId = 1, CenaKursu = 120, Bestseller = true, NazwaPlikuObrazka="obrazekmvc2.png",
             DataDodania = DateTime.Now, OpisKursu = "opis kursu mvc4" },
             new Kurs() {AutorKursu = "Romek", TytulKursu="HTML5 wstęp", KategoriaId = 4, CenaKursu = 50, Bestseller = true, NazwaPlikuObrazka="obrazekhtml.png",
             DataDodania = DateTime.Now, OpisKursu = "opis kursu mvc 5" },
             new Kurs() {AutorKursu = "Arek", TytulKursu="jQuery-podstawy", KategoriaId = 3, CenaKursu = 100, Bestseller = true, NazwaPlikuObrazka="obrazekjquery.png",
             DataDodania = DateTime.Now, OpisKursu = "opis kursu mvc 5" },
         };

            kursy.ForEach(k => context.Kursy.AddOrUpdate(k));
            context.SaveChanges();
        }
    }
}