using CacheByAttribute.Core;
using Glimpse.Core.Extensibility;

namespace CacheByAttribute.Glipmse
{
    public class CacheByAttributeTab : TabBase, IKey /*, ITabLayout*/
    {
        /*
        private static readonly object layout = TabLayout.Create()
          .Row(r =>
          {
              r.Cell(0).WidthInPixels(80);
              r.Cell(1).WidthInPixels(80);
              r.Cell(2);
              r.Cell(3);
              r.Cell(4).WidthInPercent(15).Suffix(" ms").AlignRight().Prefix("T+ ").Class("mono");
              r.Cell(5).WidthInPercent(15).Suffix(" ms").AlignRight().Class("mono");
          }).Build();
        */

        public override string Name
        {
            get { return "Cache By Attribute"; }
        }

        public string Key
        {
            get { return "glimpse_CacheByAttribute"; }
        }


        public override object GetData(ITabContext context)
        {
            return CacheManager.StatisticsSnapshots;
        }
    }
}