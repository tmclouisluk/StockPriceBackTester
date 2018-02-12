using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceBackTest
{
    public class movingAverage
    {
        public int period { get; set; }
        public Queue<double> prices { get; set; }        

        public movingAverage(int period)
        {
            this.period = period;
            this.prices = new Queue<double>(period);
            //this.value = 0;
        }

        public void add(double price){
            if (this.prices.Count >= period)
                this.prices.Dequeue();
            this.prices.Enqueue(price);
        }

        public double getValue()
        {
            return this.prices.Average();
        }

    }
}
