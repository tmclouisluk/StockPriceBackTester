using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceBackTest
{
    class Program
    {

        static void Main(string[] args)
        {
            //double starting = 1000;
            Console.WriteLine("Please enter the location of stock csv (default: C:\\stock.csv): ");

            string csvFilePath = Console.ReadLine(); //@"C:\MNST.csv";
            if (String.IsNullOrEmpty(csvFilePath))
            {
                csvFilePath = @"C:\stock.csv";
            }

            Console.WriteLine("Please enter the starting buying power (default: 1000): ");
            string buyingPower = Console.ReadLine();
            double balance = 1000;
            if (!String.IsNullOrEmpty(buyingPower))
            {
                balance = Convert.ToDouble(buyingPower);
            }
            
            int row = 0;
            movingAverage ma10 = new movingAverage(10);
            movingAverage ma30 = new movingAverage(30);
            //double lastprice = 0;
            //List<stock> data = new List<stock>();
            //double boughtPrice = 0;
            //int lot = 0;
            bool isBought = false;
            bool isSold = false;
            //bool isReady = false;
            List<stock> pofo = new List<stock>();

            using (var reader = new StreamReader(csvFilePath))
            {               
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (row > 0)
                    {
                        try
                        {
                            stock stock = new stock();
                            stock.date = values[0];
                            stock.open = Convert.ToDouble(values[1]);
                            stock.high = Convert.ToDouble(values[2]);
                            stock.low = Convert.ToDouble(values[3]);
                            stock.close = Convert.ToDouble(values[4]);
                            stock.adjClose = Convert.ToDouble(values[5]);
                            stock.volume = Convert.ToInt32(values[6]);
                            //data.Add(stock);

                            ma10.add(stock.close);
                            ma30.add(stock.close);

                            if (row > 30)
                            {
                                bool isMA10GTMA30 = ma10.getValue() > ma30.getValue();
                                bool isPriceGTMA10 = stock.close > ma10.getValue();


                                if (isBought)
                                {
                                    isBought = false;
                                    stock.lot = (int)(balance / stock.open);
                                    pofo.Add(stock);
                                    Console.WriteLine("Buy - " + stock.date + " balance: " + balance);
                                    //Console.WriteLine("Buy - " + stock.date + " ma10: " + ma10.getValue() + " ma30: " + ma30.getValue() + " dayhigh: " + stock.high +
                                    //" daylow: " + stock.low + " balance: " + balance);
                                }
                                else if (isSold)
                                {

                                    isSold = false;
                                    balance += (stock.open - pofo[0].open) * pofo[0].lot;
                                    pofo.Clear();
                                    Console.WriteLine("Sell - " + stock.date + " balance: " + balance);
                                    //Console.WriteLine("Sell - " + stock.date + " ma10: " + ma10.getValue() + " ma30: " + ma30.getValue() + " dayhigh: " + stock.high +
                                    //" daylow: " + stock.low + " balance: " + balance);

                                }

                                if (isMA10GTMA30 && isPriceGTMA10)
                                {
                                    if (pofo.Count() == 0 && !isBought)
                                    {
                                        isBought = true;
                                    }
                                    //lot = (int)(balance / ma10.getValue());
                                    //boughtPrice = ma10.getValue();
                                    //balance = 
                                    //balance -= ma10.getValue();
                                    //Console.WriteLine("Buy - " + stock.date + " ma10: " + ma10.getValue() + " ma30: " + ma30.getValue() + " dayhigh: " + stock.high +
                                    //    " daylow: " + stock.low + " balance: " + balance);
                                }
                                else
                                {
                                    if (pofo.Count() > 0 && !isSold)
                                    {
                                        isSold = true;
                                    }
                                }

                                /*
                                if (isMA10GTMA30 && isPriceGTMA10 && !isBought)
                                {
                                    isBought = true;
                                    lot = (int)  (balance / ma10.getValue());
                                    boughtPrice = ma10.getValue();
                                    //balance = 
                                    //balance -= ma10.getValue();
                                    Console.WriteLine("Buy - " + stock.date + " balance: " + balance);
                                }
                                else
                                {
                                    if (isBought)
                                    {
                                        isBought = false;
                                        balance += (ma10.getValue() - boughtPrice) * lot;
                                        boughtPrice = 0;
                                        lot = 0;
                                        Console.WriteLine("Sell - " + stock.date +" balance: " + balance);
                                    }
                                }
                                */

                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        //Console.WriteLine("Row " + row + " - " + stock.date + " ma10: " + ma10.getValue() + " ma30: " + ma30.getValue() + "");

                    }
                    row++;
                }
            }

            Console.ReadLine();
        }
    }
}
