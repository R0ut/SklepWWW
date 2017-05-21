using SklepWWW.DAL;
using SklepWWW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepWWW.Infrastructure
{
    public class KoszykMenager
    {
        private KursyContext db;
        private ISessionMenager session;

        public KoszykMenager(ISessionMenager session, KursyContext db)
        {
            this.session = session;
            this.db = db;
        }

        public List<PozycjaKoszyka> PobierzKoszyk()
        {
            List<PozycjaKoszyka> koszyk;

            if (session.Get<List<PozycjaKoszyka>>(Consts.KoszykSessionKey) == null)
            {
                koszyk = new List<PozycjaKoszyka>();
            }
            else
            {
                koszyk = session.Get<List<PozycjaKoszyka>>(Consts.KoszykSessionKey) as List<PozycjaKoszyka>;
            }

            return koszyk;
        }

        public void DodajDoKoszyka(int kursId)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(x => x.Kurs.KursId == kursId);

            if (pozycjaKoszyka != null)
            {
                pozycjaKoszyka.Ilosc++;
            }
            else
            {
                var kursDoDodania = db.Kursy.Where(x => x.KursId == kursId).SingleOrDefault();

                if(kursDoDodania != null)
                {
                    var nowaPozycjaKoszyka = new PozycjaKoszyka()
                    {
                        Kurs = kursDoDodania,
                        Ilosc = 1,
                        Wartosc = kursDoDodania.CenaKursu
                    };
                    koszyk.Add(nowaPozycjaKoszyka);
                }
            }

            session.Set(Consts.KoszykSessionKey, koszyk);
        }

        public int UsunZKoszyka(int kursId)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(x => x.Kurs.KursId == kursId);

            if(pozycjaKoszyka != null)
            {
                if(pozycjaKoszyka.Ilosc > 1)
                {
                    pozycjaKoszyka.Ilosc--;
                    return pozycjaKoszyka.Ilosc;
                }
                else
                {
                    koszyk.Remove(pozycjaKoszyka);
                }
            }
            return 0;
        }

        public decimal PobierzWartoscKoszyka()
        {
            var koszyk = PobierzKoszyk();
            return koszyk.Sum(x => (x.Ilosc * x.Kurs.CenaKursu));
        }

        public int PobierzIloscPozycjiKoszyka()
        {
            var koszyk = PobierzKoszyk();
            int ilosc = koszyk.Sum(x => x.Ilosc);
            return ilosc;
        }

        public Zamowienie UtworzZamowienie(Zamowienie noweZamowienie, string userId)
        {
            var koszyk = PobierzKoszyk();
            noweZamowienie.DataDodania = DateTime.Now;
            //noweZamowienie.userId = userId;
            db.Zamowienia.Add(noweZamowienie);
        
            if(noweZamowienie.PozycjaZamowienia == null)
            {
                noweZamowienie.PozycjaZamowienia = new List<PozycjaZamowienia>();
            }
            decimal koszykWartosc = 0;

            foreach (var koszykElement in koszyk)
            {
                var nowaPozycjaZamowienia = new PozycjaZamowienia()
                {
                    KursId = koszykElement.Kurs.KursId,
                    Ilosc = koszykElement.Ilosc,
                    CenaZakupu = koszykElement.Kurs.CenaKursu
                };

                koszykWartosc += (koszykElement.Ilosc * koszykElement.Kurs.CenaKursu);
                noweZamowienie.PozycjaZamowienia.Add(nowaPozycjaZamowienia);
            }

            noweZamowienie.WartoscZamowienia = koszykWartosc;

            db.SaveChanges();

            return noweZamowienie;
        }

        public void PustyKoszyk()
        {
            session.Set<List<PozycjaKoszyka>>(Consts.KoszykSessionKey, null);
        }
    }
}