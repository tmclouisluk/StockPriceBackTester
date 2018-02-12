using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceBackTest
{
    public class stock
    {
        public string date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double adjClose { get; set; }
        public int volume { get; set; }
        public int lot { get; set; }
    }
}
