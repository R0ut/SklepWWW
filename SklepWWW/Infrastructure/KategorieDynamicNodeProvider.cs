using MvcSiteMapProvider;
using SklepWWW.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepWWW.Infrastructure
{
    public class KategorieDynamicNodeProvider : DynamicNodeProviderBase
    {
        private KursyContext db = new KursyContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodee)
        {
            var returnValue = new List<DynamicNode>();

            foreach (var kategorie in db.Kategorie)
            {
                DynamicNode node = new DynamicNode();
                node.Title = kategorie.NazwaKategorii;
                node.Key = "Kategoria_" + kategorie.KategoriaId;
                node.RouteValues.Add("nazwaKategori", kategorie.NazwaKategorii);
                returnValue.Add(node);
            }

            return returnValue;
        }
    }
}