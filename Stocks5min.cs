using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Multi
{
    #region db data class
    class Stocks5min : IComparable<Stocks5min>
    {
        #region Parameters
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public string Type { get; set; }
        public int Speed { get; set; }
        public int Accupoint_1 { get; set; }
        public int Accupoint_2 { get; set; }
        public double TotalAccuPnt { get; set; }
        #endregion
        public Stocks5min(string name, DateTime date, double open, double high, double low, double close)
        {
            Name = name; Date = date; Open = open; High = high; Low = low; Close = close;
        }
        public static List<Stocks5min> GetStockData(string conn, string symbol, int year, int month)
        {
            List<Stocks5min> stockList = new List<Stocks5min>(); // and month([_datetime]) >= '" + month + "' // year([_datetime]) >= '" + year + "' and // OandaTbl4Hours
            using (SqlConnection connection = new SqlConnection(conn)) // datain1dayallSTK // datain1hourallSTK // datain5minallSTK // datain1minallSTK // datain1hour1month // datain15min1month
            // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain1day1year] WHERE LEFT([unixdatetime], 4) >= '" + year + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
            using (SqlCommand command = new SqlCommand("SELECT TOP 1000 * FROM [datain1hour1month] WHERE year([_datetime]) = '" + year + "' and month([_datetime]) = '" + month + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
            // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain1dayallSTK] WHERE year([_datetime]) >= '" + year + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
            // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain1hourallSTK] WHERE year([_datetime]) >= '" + year + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
            // // Oanda Tbl
            // using (SqlCommand command = new SqlCommand("SELECT TOP 100000000000000 * FROM [OandaTbl] WHERE year([_datetime]) >= '" + year + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        #region  daily bars
                        // int date = reader.GetInt32(2);
                        // int d = date % 100;
                        // int m = (date / 100) % 100;
                        // int y = date / 10000;
                        // var result = new DateTime(y, m, d);
                        // stockList.Add(new Stocks5min(symbol, result, Math.Round(reader.GetDouble(3), 5), Math.Round(reader.GetDouble(4), 5), Math.Round(reader.GetDouble(5), 5), Math.Round(reader.GetDouble(6), 5)));
                        #endregion
                        // ibkr
                        stockList.Add(new Stocks5min(symbol, DateTime.Parse(unixTime(reader.GetInt32(2))), Math.Round(reader.GetDouble(3), 5), Math.Round(reader.GetDouble(4), 5), Math.Round(reader.GetDouble(5), 5), Math.Round(reader.GetDouble(6), 5)));
                        // // polygon.io
                        // stockList.Add(new Stocks5min(symbol, DateTime.Parse(unixTime(reader.GetDouble(2))), Math.Round(reader.GetDouble(3), 5), Math.Round(reader.GetDouble(4), 5), Math.Round(reader.GetDouble(5), 5), Math.Round(reader.GetDouble(6), 5)));
                    }
                }
            }
            int LastIndex = stockList.Count;
            stockList.Reverse();
            foreach (Stocks5min s in stockList)
            {
                DateTime currentTime = s.Date;
                s.Speed = (int)((DateTimeOffset)currentTime).ToUnixTimeSeconds();
                int index = stockList.IndexOf(s);
                #region High / Low // bottoms
                try
                {
                    if (index != 0 && index != LastIndex - 1)
                    {
                        if (s.Low <= stockList[index - 1].Low && s.Low <= stockList[index + 1].Low)
                        {
                            s.Type = "b";
                        }
                        else
                        {
                            s.Type = "n";
                        }
                    }
                }
                catch (Exception e)
                {

                }
                #endregion
                #region accu by LowestHigh & HighestLow
                // foreach (Stocks5min o1 in stockList) // first loooooooop
                // {
                //     double lowestHigh = 10000; double highestLow = 0; double o2Total = 0;
                //     double accuScore = 0; double n = 1; // 1 = new accu
                //     int o1_index = stockList.IndexOf(o1);
                //     foreach (Stocks5min o2 in stockList) // second loooooooop
                //     {
                //         int o2_index = stockList.IndexOf(o2);
                //         if (o2.Date >= o1.Date)
                //         {
                //             o2Total = o2Total + (o2.High - o2.Low);
                //             if (o2.High <= lowestHigh)
                //                 lowestHigh = o2.High;
                //             if (o2.Low >= highestLow)
                //                 highestLow = o2.Low;
                //             if (o2_index - o1_index >= 2)// && lowestHigh > highestLow) // more than 2 candles
                //             // if (1 == 1)
                //             {
                //                 if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total > 0.50)// && n == 1)
                //                 {
                //                     // o1.Accupoint_1 = 1;
                //                     o2.Accupoint_2 = 1;
                //                     o1.TotalAccuPnt = (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total;
                //                     o2.TotalAccuPnt = (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total;
                //                 }
                //                 else
                //                 {
                //                     o2.Accupoint_2 = 0;
                //                 }
                //                 // if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total >= accuScore && n == 0)
                //                 // {
                //                 //     accuScore = (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total;
                //                 // }
                //                 // else if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total < accuScore && n == 0)
                //                 // {
                //                 //     o1.Accupoint_1 = 1;
                //                 //     stockList[o2_index - 1].Accupoint_2 = 1;
                //                 //     // Console.WriteLine(o2.Date);
                //                 //     // Console.WriteLine(o1.Date);
                //                 //     continue;
                //                 // }
                //             }
                //         }
                //     }
                // }
                #endregion
            }
            return stockList;
        }
        private static string unixTime(int timestamp)
        {
            // First make a System.DateTime equivalent to the UNIX Epoch.
            DateTime dateTime = new System.DateTime(1970, 1, 1, 3, 0, 0, 0);
            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds(Convert.ToInt32(timestamp));
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }
        private static string unixTime(double timestamp)
        {
            string t = timestamp.ToString();
            int tim = Convert.ToInt32(t.Substring(0, t.Length-3));
            // int t = Convert.ToInt32(timestamp);
            // First make a System.DateTime equivalent to the UNIX Epoch.
            DateTime dateTime = new System.DateTime(1970, 1, 1, 3, 0, 0, 0);
            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds(Convert.ToInt32(tim));
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }
        public int CompareTo(Stocks5min that)
        {
            return this.Date.CompareTo(that.Date);
        }
    }
    #endregion

}
