using MvcSiteMapProvider;
using SklepWWW.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepWWW.Infrastructure
{
    public class KursySzczegolyDynamicNodeProvider : DynamicNodeProviderBase
    {
        private KursyContext db = new KursyContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodee)
        {
            var returnValue = new List<DynamicNode>();

            foreach (var kurs in db.Kursy)
            {
                DynamicNode node = new DynamicNode();
                node.Title = kurs.TytulKursu;
                node.Key = "Kurs_" + kurs.KursId;
                node.ParentKey = "Kategoria_" + kurs.KategoriaId;
                node.RouteValues.Add("id", kurs.KursId);
                returnValue.Add(node);
            }

            return returnValue;
        }
    }
}