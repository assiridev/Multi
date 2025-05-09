// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Data.SqlClient;

// namespace Curvy
// {
//     #region db class
//     class Stock : IComparable<Stock>
//     {
//         #region Parameters
//         public string Name { get; set; }
//         public DateTime Date { get; set; }
//         public double Open { get; set; }
//         public double High { get; set; }
//         public double Low { get; set; }
//         public double Close { get; set; }
//         public string Type { get; set; }
//         public int Speed { get; set; }
//         public double Mid { get; set; }
//         public int Accupoint_1 { get; set; }
//         public int Accupoint_2 { get; set; }
//         public double TotalAccuPnt { get; set; }
//         #endregion
//         public Stock(string name, DateTime date, double open, double high, double low, double close, int speed)
//         {
//             Name = name; Date = date; Open = open; High = high; Low = low; Close = close;
//         }
//         public static List<Stock> GetStockData(string conn, string symbol, int year, int month)
//         {
//             List<Stock> stockList = new List<Stock>();
//             using (SqlConnection connection = new SqlConnection(conn)) // month([_datetime]) = '" + month + "' and // year([_datetime]) = '" + year + "' and
//             // CHANGE TO TOP 600  month([_datetime]) = '" + year + "' and //// datain1hour1month /// datain15min1month // 
//             // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain1day1year] WHERE LEFT([unixdatetime], 4) = '" + year + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
//             // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain1hour1month] WHERE year([_datetime]) = '" + year + "' and month([_datetime]) = '" + month + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
//             // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain1dayallSTK] WHERE year([_datetime]) = '" + year + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
//             // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain1hourallSTK] WHERE year([_datetime]) = '" + year + "' and month([_datetime]) = '" + month + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
//             // using (SqlCommand command = new SqlCommand("SELECT TOP 10000000000 * FROM [datain5minallSTK] WHERE year([_datetime]) = '" + year + "' and month([_datetime]) = '" + month + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
//             // Oanda Tbl
//             using (SqlCommand command = new SqlCommand("SELECT TOP 100000000000000 * FROM [OandaTbl] WHERE year([_datetime]) = '" + year + "' and symbol = '" + symbol + "' ORDER BY 3 DESC", connection))
//             {
//                 connection.Open();
//                 using (SqlDataReader reader = command.ExecuteReader())
//                 {
//                     while (reader.Read())
//                     {
//                         #region  daily bars
//                         // int date = reader.GetInt32(2);
//                         // int d = date % 100;
//                         // int m = (date / 100) % 100;
//                         // int y = date / 10000;
//                         // var result = new DateTime(y, m, d);
//                         // stockList.Add(new Stock(symbol, result, Math.Round(reader.GetDouble(3), 5), Math.Round(reader.GetDouble(4), 5), Math.Round(reader.GetDouble(5), 5), Math.Round(reader.GetDouble(6), 5), 0));
//                         #endregion
//                         // ibkr
//                         stockList.Add(new Stock(symbol, DateTime.Parse(unixTime(reader.GetInt32(2))), Math.Round(reader.GetDouble(3), 5), Math.Round(reader.GetDouble(4), 5), Math.Round(reader.GetDouble(5), 5), Math.Round(reader.GetDouble(6), 5), 0));
//                         // // polygon.io
//                         // stockList.Add(new Stock(symbol, DateTime.Parse(unixTime(reader.GetDouble(2))), Math.Round(reader.GetDouble(3), 5), Math.Round(reader.GetDouble(4), 5), Math.Round(reader.GetDouble(5), 5), Math.Round(reader.GetDouble(6), 5), 0));
//                     }
//                 }
//             }
//             int LastIndex = stockList.Count;
//             stockList.Reverse();
//             foreach (Stock s in stockList)
//             {
//                 DateTime currentTime = s.Date;
//                 s.Speed = (int)((DateTimeOffset)currentTime).ToUnixTimeSeconds();
//                 int index = stockList.IndexOf(s);
//                 s.Mid = 0;
//                 #region High / Low // bottoms
//                 // s.Mid = s.Low + (s.High - s.Low) * 0.50; // s.Close
//                 try
//                 {
//                     if (index != 0 && index != LastIndex - 1)
//                     {
//                         if (s.Low <= stockList[index - 1].Low && s.Low <= stockList[index + 1].Low)
//                         {
//                             s.Type = "b";
//                         }
//                         else
//                         {
//                             s.Type = "n";
//                         }
//                     }
//                 }
//                 catch (Exception e)
//                 {

//                 }
//                 #endregion
//                 // stockList.Reverse();
//             }
//             #region accu by LowestHigh & HighestLow
//             foreach (Stock o1 in stockList) // first loooooooop
//             {
//                 double lowestHigh = 10000; double highestLow = 0; double o2Total = 0;
//                 double accuScore = 0; double n = 1; // 1 = new accu
//                 int o1_index = stockList.IndexOf(o1);
//                 foreach (Stock o2 in stockList) // second loooooooop
//                 {
//                     int o2_index = stockList.IndexOf(o2);
//                     if (o2.Date >= o1.Date)
//                     {
//                         o2Total = o2Total + (o2.High - o2.Low);
//                         if (o2.High <= lowestHigh)
//                             lowestHigh = o2.High;
//                         if (o2.Low >= highestLow)
//                             highestLow = o2.Low;
//                         // if (o2_index - o1_index > 1)
//                         //     break;
//                         if (o2_index - o1_index >= 2)// && lowestHigh > highestLow) // more than 2 candles
//                         // if (1 == 1)
//                         {
//                             if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total > 0.50
//                             // && o2.Close > LowestPoint(o1, stockList[o2_index - 1], stockList)
//                             // && o2.Close < HighestPoint(o1, stockList[o2_index - 1], stockList)
//                             )
//                             {
//                                 // if (B1(o1, o2, stockList) == false)
//                                 //     continue;
//                                 // o1.Accupoint_1 = 1;
//                                 o2.Accupoint_2 = 1;
//                                 o2.Mid = Mdl(o1, o2, stockList);
//                                 o2.Type = "Mdl";
//                                 o1.TotalAccuPnt = (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total;
//                                 o2.TotalAccuPnt = (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total;
//                             }
//                             else
//                             {
//                                 o2.Accupoint_2 = 0;
//                             }
//                         }
//                     }
//                 }
//             }
//             #endregion
//             #region accu by highest(Open,Close) & Lowest(Open,Close)
//             // foreach (Stock o1 in stockList) // first loooooooop
//             // {
//             //     double highest = 0; double lowest = 10000; double o2Total = 0;
//             //     double accuScore = 0; double n = 1; // 1 = new accu
//             //     int o1_index = stockList.IndexOf(o1);
//             //     foreach (Stock o2 in stockList) // second loooooooop
//             //     {
//             //         int o2_index = stockList.IndexOf(o2);
//             //         if (o2.Date >= o1.Date)
//             //         {
//             //             o2Total = o2Total + (o2.High - o2.Low);
//             //             #region Highest(Open, Close) & Lowest(Open, Close)
//             //             // need to implement summing the traded areas
//             //             if (Math.Max(o2.Open, o2.Close) >= highest)
//             //                 highest = Math.Max(o2.Open, o2.Close);
//             //             if (Math.Min(o2.Open, o2.Close) <= lowest)
//             //                 lowest = Math.Min(o2.Open, o2.Close);
//             //             #endregion
//             //             if (o2_index - o1_index > 1)// && highest > lowest) // more than 2 candles
//             //             // if (1 == 1)
//             //             {
//             //                 if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total > 0.50 && n == 1)
//             //                 {
//             //                     accuScore = (highest - lowest) * (o2_index - o1_index + 1) / o2Total;
//             //                     n = 0;
//             //                 }
//             //                 if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total >= accuScore && n == 0)
//             //                 {
//             //                     accuScore = (highest - lowest) * (o2_index - o1_index + 1) / o2Total;
//             //                 }
//             //                 else if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total < accuScore && n == 0)
//             //                 {
//             //                     o1.Accupoint_1 = 2;
//             //                     stockList[o2_index - 1].Accupoint_2 = 2;
//             //                     // Console.WriteLine(o2.Date);
//             //                     // Console.WriteLine(o1.Date);
//             //                     break;
//             //                 }
//             //             }
//             //         }
//             //     }
//             // }
//             #endregion
//             return stockList;
//         }
//         private static double Mdl(Stock o1, Stock o2, List<Stock> stocks)
//         {
//             double highest = 0; double lowest = 10000;
//             foreach (Stock s in stocks)
//             {
//                 if (s.Date < o1.Date)
//                     continue;
//                 if (s.Date > o2.Date)
//                     break;
                
//                 if (s.High > highest)
//                     highest = s.High;
//                 if (s.Low < lowest)
//                     lowest = s.Low;
//             }
//             return lowest + (highest - lowest) * 0.50;
//         }
//         private static double HighestPoint(Stock from, Stock to, List<Stock> stocks)
//         {
//             double highest = 0;
//             foreach (Stock c in stocks)
//             {
//                 if (c.Date >= from.Date && c.Date <= to.Date)
//                 {
//                     if (c.High > highest)
//                         highest = c.High;
//                 }
//             }
//             return highest;
//         }
//         private static double LowestPoint(Stock from, Stock to, List<Stock> stocks)
//         {
//             double lowest = 10000;
//             foreach (Stock c in stocks)
//             {
//                 if (c.Date >= from.Date && c.Date <= to.Date)
//                 {
//                     if (c.Low < lowest)
//                         lowest = c.Low;
//                 }
//             }
//             return lowest;
//         }
//         private static bool B1(Stock from, Stock to, List<Stock> stocks)
//         {
//             double b1 = 0; double b2 = 0;
//             foreach (Stock s in stocks)
//             {
//                 int index = stocks.IndexOf(s);
//                 if (s.Date <= from.Date)
//                     continue;
//                 if (s.Date > to.Date)
//                     break;
//                 if (s.Low <= stocks[index - 1].Low)
//                     b1 = b1 + 1;
//                 if (s.Low > stocks[index - 1].Low)
//                     b2 = b2 + 1;
//             }
//             return b1 > b2;
//         }
//         private static string unixTime(int timestamp)
//         {
//             // First make a System.DateTime equivalent to the UNIX Epoch.
//             DateTime dateTime = new System.DateTime(1970, 1, 1, 3, 0, 0, 0);
//             // Add the number of seconds in UNIX timestamp to be converted.
//             dateTime = dateTime.AddSeconds(Convert.ToInt32(timestamp));
//             return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
//         }
//         private static string unixTime(double timestamp)
//         {
//             string t = timestamp.ToString();
//             int tim = Convert.ToInt32(t.Substring(0, t.Length-3));
//             // int t = Convert.ToInt32(timestamp);
//             // First make a System.DateTime equivalent to the UNIX Epoch.
//             DateTime dateTime = new System.DateTime(1970, 1, 1, 3, 0, 0, 0);
//             // Add the number of seconds in UNIX timestamp to be converted.
//             dateTime = dateTime.AddSeconds(Convert.ToInt32(tim));
//             return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
//         }
//         public int CompareTo(Stock that)
//         {
//             return this.Date.CompareTo(that.Date);
//         }
//     }
//     #endregion
    
//     #region excel class
//     // class Stock
//     // {
//     //     #region Parameters
//     //     public string Name { get; set; }
//     //     public DateTime Date { get; set; }
//     //     public double Open { get; set; }
//     //     public double High { get; set; }
//     //     public double Low { get; set; }
//     //     public double Close { get; set; }
//     //     public string Type { get; set; }
//     //     #endregion
//     //     public Stock(string name, DateTime date, double open, double high, double low, double close)
//     //     {
//     //         Name = name; Date = date; Open = open; High = high; Low = low; Close = close;
//     //     }

//     //     public static List<Stock> GetStockData(string file, string name)
//     //     {
//     //         List<Stock> stockList = new List<Stock>();
//     //         StreamReader reader = File.OpenText(file);
//     //         string line;
//     //         while ((line = reader.ReadLine()) != null)
//     //         {
//     //             string[] items = line.Split(',');
//     //             if (items[0] == "time" || items[0] == "unixdatetime") continue; //
//     //             //stockList.Add(new Stock(name, DateTime.Parse(items[0]), double.Parse(items[1]), double.Parse(items[2]), double.Parse(items[3]), double.Parse(items[4])));
//     //             stockList.Add(new Stock(name, DateTime.Parse(unixTime(items[0])), double.Parse(items[1]), double.Parse(items[2]), double.Parse(items[3]), double.Parse(items[4])));
//     //         }

//     //         int LastIndex = stockList.Count;
//     //         foreach (Stock s in stockList)
//     //         {
//     //             int index = stockList.IndexOf(s);
//     //             #region Close
//     //             // if (index != 0 && index != LastIndex - 1)
//     //             // {
//     //             //     if (s.Close > stockList[index - 1].Close && s.Close >= stockList[index + 1].Close)
//     //             //     {
//     //             //         // top
//     //             //         s.Type = "t";
//     //             //     }
//     //             //     else if (s.Close <= stockList[index - 1].Close && s.Close < stockList[index + 1].Close)
//     //             //     {
//     //             //         //bottom
//     //             //         s.Type = "b";
//     //             //     }
//     //             //     else
//     //             //     {
//     //             //         s.Type = "n";
//     //             //     }
//     //             // }
//     //             #endregion
//     //             #region High / Low
//     //             if (index != 0 && index != LastIndex - 1)
//     //             {
//     //                 if (s.High > stockList[index - 1].High && s.High >= stockList[index + 1].High)
//     //                 {
//     //                     // top
//     //                     s.Type = "t";
//     //                 }
//     //                 else if (s.Low < stockList[index - 1].Low && s.Low <= stockList[index + 1].Low)
//     //                 {
//     //                     //bottom
//     //                     s.Type = "b";
//     //                 }
//     //                 else
//     //                 {
//     //                     s.Type = "n";
//     //                 }
//     //             }
//     //             #endregion
//     //         }
//     //         return stockList;
//     //     }

//     //     private static string unixTime(string timestamp)
//     //     {
//     //         // First make a System.DateTime equivalent to the UNIX Epoch.
//     //         DateTime dateTime = new System.DateTime(1970, 1, 1, 3, 0, 0, 0);
//     //         // Add the number of seconds in UNIX timestamp to be converted.
//     //         dateTime = dateTime.AddSeconds(Convert.ToInt32(timestamp));
//     //         return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
//     //     }
//     // }
//     #endregion
// }