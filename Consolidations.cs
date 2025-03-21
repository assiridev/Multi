using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace Multi
{
    class Consolidations : IComparable<Consolidations>
    {
        #region Parameters
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double LowestPnt { get; set; }
        #endregion
        public Consolidations(string name, DateTime start, DateTime end, double lowestPnt)
        {
            Name = name; Start = start; End = end; LowestPnt = lowestPnt;
        }

        public static List<Consolidations> GetConsolidations(List<Stock> stockList)
        {
            List<Consolidations> consolidations = new List<Consolidations>();
            #region accu by LowestHigh & HighestLow
            foreach (Stock o1 in stockList) // first loooooooop
            {
                // Console.WriteLine(stockList.Count);
                double qud = 0;
                double qudLow = 0;
                double qudHigh = 0;
                double highestP = 0; double lowestP = 0;
                double lowestHigh = 10000; double highestLow = 0; double o2Total = 0;
                double accuScore = 0; double n = 1; // 1 = new accu
                int o1_index = stockList.IndexOf(o1);
                foreach (Stock o2 in stockList) // second loooooooop
                {
                    highestP = HighestPoint(o1, o2, stockList);
                    lowestP = LowestPoint(o1, o2, stockList);
                    if (o2.High > highestP)
                        highestP = o2.High;
                    if (o2.Low < lowestP)
                        lowestP = o2.Low;
                    int o2_index = stockList.IndexOf(o2);
                    if (o2.Date >= o1.Date)
                    {
                        o2Total = o2Total + (o2.High - o2.Low);
                        if (o2.High <= lowestHigh)
                            lowestHigh = o2.High;
                        if (o2.Low >= highestLow)
                            highestLow = o2.Low;
                        if (o2_index - o1_index >= 5)// && o2_index - o1_index <= 6)// && lowestHigh > highestLow) // more than 2 candles
                        // if (1 == 1)
                        {
                            // quad high && low
                            qudLow = QuadLow(o1, o2, o1_index, o2_index, 1, stockList);
                            qudHigh = QuadHigh(o1, o2, o1_index, o2_index, 1, stockList);
                            // Console.WriteLine((decimal)qudLow);
                            if (qudLow == 0 && qudHigh == 0)
                            // quad close
                            // qud = Quad(o1, o2, o1_index, o2_index, 1, stockList);
                            // if (qud == 1)
                                if (GetConsolidationsBigPic(o1, o2, highestP, lowestP, o2_index - o1_index, stockList) == 1)
                                    consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date, lowestP));
                            else
                                continue;
                            // if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total > 0.20 && n == 1)
                            // {
                            //     accuScore = (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total;
                            //     n = 0;
                            //     // if (o2.Close <= lowestP + (highestP - lowestP) * 0.25 && o2.Low > lowestP)
                            //     // if (o2.Low > lowestP)
                            //     // if (regLine(o1, o2, stockList) == 1)// && o2.Close <= lowestP + (highestP - lowestP) * 0.75)
                                // if (qud == 1 && o2.Low > lowestP && o2.Close > stockList[o2_index - 1].Low)
                                
                            // }
                            // if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total >= accuScore && n == 0)
                            // {
                            //     accuScore = (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total;
                            //     // if (o2.Close <= lowestP + (highestP - lowestP) * 0.25 && o2.Low > lowestP)
                            //     // if (o2.Low > lowestP)
                            //     // if (regLine(o1, o2, stockList) == 1)// && o2.Close <= lowestP + (highestP - lowestP) * 0.75)
                            //     if (Quad(o1, o2, 1, stockList) == 1)
                            //         consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date));
                            // }
                            // else if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total < accuScore && n == 0)
                            // {
                            //     continue;
                            // }
                        }
                    }
                }
            }
            #endregion
            #region accu by highest(Open,Close) & Lowest(Open,Close)
            // foreach (Stock o1 in stockList) // first loooooooop
            // {
            //     double highest = 0; double lowest = 10000; double o2Total = 0;
            //     double accuScore = 0; double n = 1; // 1 = new accu
            //     int o1_index = stockList.IndexOf(o1);
            //     foreach (Stock o2 in stockList) // second loooooooop
            //     {
            //         int o2_index = stockList.IndexOf(o2);
            //         if (o2.Date >= o1.Date)
            //         {
            //             o2Total = o2Total + (o2.High - o2.Low);
            //             #region Highest(Open, Close) & Lowest(Open, Close)
            //             // need to implement summing the traded areas
            //             if (Math.Max(o2.Open, o2.Close) >= highest)
            //                 highest = Math.Max(o2.Open, o2.Close);
            //             if (Math.Min(o2.Open, o2.Close) <= lowest)
            //                 lowest = Math.Min(o2.Open, o2.Close);
            //             #endregion
            //             if (o2_index - o1_index > 2)// && highest > lowest) // more than 2 candles
            //             {
            //                 if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total > 0.90 && n == 1) // was 90
            //                 {
            //                     accuScore = (highest - lowest) * (o2_index - o1_index + 1) / o2Total;
            //                     n = 0;
            //                     consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date)); // stockList[o2_index - 1].Date
            //                 }
            //                 if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total >= accuScore && n == 0)
            //                 {
            //                     accuScore = (highest - lowest) * (o2_index - o1_index + 1) / o2Total;
            //                     consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date)); // stockList[o2_index - 1].Date
            //                 }
            //                 else if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total < accuScore && n == 0)
            //                     break;
            //             }
            //         }
            //     }
            // }
            #endregion
            return consolidations;
        }

        public static double regLine(Stock p0, Stock x, List<Stock> stocks)
        {
            double all = 0; double touching = 0;
            List<Candle> candles = new List<Candle>();
            #region Linear Reg by Price
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                candles.Add(new Candle { UnixTime = q.Speed, Open = q.Open, High = q.High, Low = q.Low, Close = q.Close });
            }
            #endregion

            #region Macd calculation
            List<MacdPoint> macdPoints = MacdCalculator.CalculateMacd(candles, shortPeriod: 12, longPeriod: 26);
            
            var macdRegressionLine = MacdRegressionCalculator.ComputeLinearRegression(macdPoints);
            #endregion

            var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Close);
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                all = all + 1;
                double lineValueAtTime = regressionLineAvg.GetLineValueAtTime(q.Speed);
                if (q.High > lineValueAtTime && q.Low < lineValueAtTime)
                    touching = touching + 1;
            }
            if (regressionLineAvg.Slope < 0 // && touching / all == 1
            // && macdRegressionLine.Slope < 0 && touching / all > 0.60
            // && macdRegressionLine.Slope - regressionLineAvg.Slope > 0
            // if (regressionLineAvg.Slope < 0 && macdRegressionLine.Slope < 0 //&& touching / all < 0.10
            // && macdRegressionLine.Slope < regressionLineAvg.Slope
            )
                return 1;
            else
                return 0;
        }
        public static double Quad(Stock p0, Stock x, int p0_index, int x_index, double concav, List<Stock> stocks)
        {
            #region Vars
            double all = 0; double touching = 0;;
            double above = 0; double below = 0;
            double above1 = 0; double below1 = 0;
            double above2 = 0; double below2 = 0;
            double belowPer = 0; 
            double lowestCurv = 10000; int lowestCurv_index = 0;
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_80 = (((x_index - p0_index) / 5) * 4) + p0_index;
            List<double> xDataList = new List<double>();
            List<double> yDataList = new List<double>();
            #endregion
            
            #region PolyNomial by Macd
            // MACDCalculator macdCalculator = new MACDCalculator();
            // List<Tuple<DateTime, double>> prices = new List<Tuple<DateTime, double>>();
            // int quad75 = ((x_index - p0_index) / 4) * 3 + p0_index;
            // foreach (Stock q in stocks)
            // {
            //     int index = stocks.IndexOf(q);
            //     if (index < p0_index)
            //         continue;
            //     if (index > x_index)
            //         break;
            //     prices.Add(new Tuple<DateTime, double>(q.Date, q.Close));
            // }

            // foreach (var price in prices)
            // {
            //     macdCalculator.AddPrice(price.Item1, price.Item2);
            // }
            // foreach (Stock q in stocks)
            // {
            //     int index = stocks.IndexOf(q);
            //     if (q.Date < p0.Date)
            //         continue;
            //     // if (index < highestPntIndex)
            //     //     continue;
            //     if (index > x_index)
            //         break;
            //     xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
            //     yDataList.Add(macdCalculator.GetMACDAt(q.Date));
            // }
            #endregion

            #region PolyNomial by Price
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                // Console.WriteLine(p1.Date + " : " + x.Date);// + " : " + )
                xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                yDataList.Add(q.Close);
            }
             // Convert lists to arrays
            double[] xData = xDataList.ToArray();
            double[] yData = yDataList.ToArray();
            
            #endregion
            
            #region Kitchen
            // Determine the best polynomial degree using BIC
            // Adjust the concavity (e.g., double the curvature)
            int maxDegree = 3; // Limit to a reasonable degree
            int bestDegree = 0;
            double bestBIC = double.MaxValue;
            double linearWeight = 1.0; // Emphasize the linear trend
            PolynomialRegression bestModel = null;

            // for (int degree = 1; degree <= maxDegree; degree++)
            // {
            //     // double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out var model);
            //     double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out PolynomialRegression model, linearWeight: 1.0);
            //     // Fit the polynomial regression with a longer linear period

            //     // var regression = new PolynomialRegression(xData, yData, degree, linearWeight);

            //     if (bic < bestBIC)
            //     {
            //         bestBIC = bic;
            //         bestDegree = degree;
            //         bestModel = model;
            //     }
            // }
            for (int degree = 2; degree <= maxDegree; degree++)
            {
                double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out PolynomialRegression model, linearWeight: 1.0); // : 1.0
                // Console.WriteLine($"Degree {degree}: BIC = {bic:0.###}");

                if (bic < bestBIC)
                {
                    bestBIC = bic;
                    bestDegree = degree;
                    bestModel = model;
                }
            }
            double concavityFactor = 1; // Higher values increase concavity
            // bestModel.AdjustConcavity(concavityFactor);
            // Console.WriteLine($"\nBest Polynomial Degree: {bestDegree} (BIC = {bestBIC:0.###})");
            // Identify trend and concavity if the best model is quadratic
            // Console.WriteLine(bestDegree);
            if (bestDegree == 2 && bestModel != null)
            {
                if (bestModel.IdentifyQuadraticTrend() == concav
                // && (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) < Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5) * 0.05)
                // && (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) < Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5) * 1)
                )
                {
                    // if (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) > Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5))
                    // {
                    //     Console.Write(Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5));
                    //     Console.Write(" : ");
                    //     Console.WriteLine(Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed)), 5));
                    // }
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        if (q.Date < p0.Date)
                            continue;
                        if (q.Date > x.Date)
                            break;
                        all = all + 1;

                        double yValue = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                        if (bestModel.GetCurveValue(Convert.ToDouble(q.Speed)) < lowestCurv)
                        {
                            lowestCurv = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                            lowestCurv_index = stocks.IndexOf(q);
                        }
                        if (q.High > yValue && q.Low < yValue)
                            touching = touching + 1;
                        above = above + (q.High - yValue);
                        below = below + (yValue - q.Low);
                        if (index <= mid)
                        {
                            above1 = above1 + (q.High - yValue);
                            below1 = below1 + (yValue - q.Low);
                        }
                        if (index >= mid)
                        {
                            above2 = above2 + (q.High - yValue);
                            below2 = below2 + (yValue - q.Low);
                        }
                    }
                    // check accu points
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        if (q.Date < p0.Date)
                            continue;
                        if (q.Date > x.Date)
                            break;
                    }
                    belowPer = below / (below + above);
                    #endregion

                    if (
                    // above < below * 1.382 && // goooooood
                    // above1 > below1 && //  >
                    // above2 > below2 && //  >
                    // above1 < above2 && //  <
                    // below1 < below2 && //  <
                    // x.High < LowestPoint(p0, x, stocks) + (HighestPoint(p0, x, stocks) - LowestPoint(p0, x, stocks)) * 0.50 &&
                    // touching / all >= 1 &&
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) // mid || quart_80
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed)) >
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) >
                    // (bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))) * 1
                    // && x.Close < stocks[x_index - 1].Close
                    // && x.Close > stocks[x_index - 1].Low
                    // && x.Low < stocks[x_index - 1].Low
                    // && stocks[x_index - 1].Close < stocks[x_index - 2].Close
                    // && x.Close < LowestPoint(p0, x, stocks) + (HighestPoint(p0, x, stocks) - LowestPoint(p0, x, stocks)) * 0.50

                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))) * 1
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed))) * 1
                    // && lowestCurv_index - p0_index >= x_index - lowestCurv_index
                    // lowestCurv_index > stocks.IndexOf(stocks[(int)quart_80])
                    )
                    {
                        return 1;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
            {
                // Console.WriteLine("The best model is not quadratic.");
                return 0;
            }
        }
        public static int GetConsolidationsBigPic(Stock start, Stock end, double curvHighest, double curvLowest, int conPeriod, List<Stock> stockList)
        {
            double conPer = 0.50;
            int daysCnt = 3;// conPeriod / 2;
            int consolidations = 0;
            #region accu by LowestHigh & HighestLow
            foreach (Stock o1 in stockList) // first loooooooop
            {
                if (o1.Date < start.Date)
                        continue;
                if (o1.Date > end.Date)
                    break;
                // Console.WriteLine(stockList.Count);
                double qud = 0;
                double highestP = 0; double lowestP = 0;
                double lowestHigh = 10000; double highestLow = 0; double o2Total = 0;
                double accuScore = 0; double n = 1; // 1 = new accu
                int o1_index = stockList.IndexOf(o1);
                foreach (Stock o2 in stockList) // second loooooooop
                {
                    if (o2.Date < start.Date)
                        continue;
                    if (o2.Date > end.Date)
                        break;
                    int o2_index = stockList.IndexOf(o2);
                    highestP = HighestPoint(o1, o2, stockList);
                    lowestP = LowestPoint(o1, o2, stockList);
                    if (o2.High > highestP)
                        highestP = o2.High;
                    if (o2.Low < lowestP)
                        lowestP = o2.Low;
                
                    if (o2.Date >= o1.Date)
                    {
                        o2Total = o2Total + (o2.High - o2.Low);
                        if (o2.High <= lowestHigh)
                            lowestHigh = o2.High;
                        if (o2.Low >= highestLow)
                            highestLow = o2.Low;
                        if (o2_index - o1_index >= daysCnt)// && o2_index - o1_index <= 6)// && lowestHigh > highestLow) // more than 2 candles
                        {
                            if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total > conPer && o2.Date == end.Date && lowestHigh < curvLowest + (curvHighest - curvLowest) * 0.50)// && o2.Close > highestLow)
                            {
                                consolidations = 1;
                                return consolidations;
                            }
                        }
                    }
                }
            }
            #endregion
            #region accu by highest(Open,Close) & Lowest(Open,Close)
            // foreach (Stock o1 in stockList) // first loooooooop
            // {
            //     double highest = 0; double lowest = 10000; double o2Total = 0;
            //     double accuScore = 0; double n = 1; // 1 = new accu
            //     int o1_index = stockList.IndexOf(o1);
            //     foreach (Stock o2 in stockList) // second loooooooop
            //     {
            //         int o2_index = stockList.IndexOf(o2);
            //         if (o2.Date >= o1.Date)
            //         {
            //             o2Total = o2Total + (o2.High - o2.Low);
            //             #region Highest(Open, Close) & Lowest(Open, Close)
            //             // need to implement summing the traded areas
            //             if (Math.Max(o2.Open, o2.Close) >= highest)
            //                 highest = Math.Max(o2.Open, o2.Close);
            //             if (Math.Min(o2.Open, o2.Close) <= lowest)
            //                 lowest = Math.Min(o2.Open, o2.Close);
            //             #endregion
            //             if (o2_index - o1_index > 2)// && highest > lowest) // more than 2 candles
            //             {
            //                 if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total > 0.90 && n == 1) // was 90
            //                 {
            //                     accuScore = (highest - lowest) * (o2_index - o1_index + 1) / o2Total;
            //                     n = 0;
            //                     consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date)); // stockList[o2_index - 1].Date
            //                 }
            //                 if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total >= accuScore && n == 0)
            //                 {
            //                     accuScore = (highest - lowest) * (o2_index - o1_index + 1) / o2Total;
            //                     consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date)); // stockList[o2_index - 1].Date
            //                 }
            //                 else if ((highest - lowest) * (o2_index - o1_index + 1) / o2Total < accuScore && n == 0)
            //                     break;
            //             }
            //         }
            //     }
            // }
            #endregion
            return consolidations;
        }
        private static double HighestPoint(Stock from, Stock to, List<Stock> stocks)
        {
            double highest = 0;
            foreach (Stock c in stocks)
            {
                if (c.Date >= from.Date && c.Date <= to.Date)
                {
                    if (c.High > highest)
                        highest = c.High;
                }
            }
            return highest;
        }
        private static double LowestPoint(Stock from, Stock to, List<Stock> stocks)
        {
            double lowest = 10000;
            foreach (Stock c in stocks)
            {
                if (c.Date >= from.Date && c.Date <= to.Date)
                {
                    if (c.Low < lowest)
                        lowest = c.Low;
                }
            }
            return lowest;
        }
        public int CompareTo(Consolidations that)
        {
            return this.Name.CompareTo(that.Name);
        }

        public static double QuadLow(Stock p0, Stock x, int p0_index, int x_index, double concav, List<Stock> stocks)
        {
            #region Vars
            double all = 0; double touching = 0;;
            double above = 0; double below = 0;
            double above1 = 0; double below1 = 0;
            double above2 = 0; double below2 = 0;
            double emptyBelow = 0; double emptyAbove = 0;
            double belowPer = 0; 
            double lowestCurv = 10000; int lowestCurv_index = 0;
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_80 = (((x_index - p0_index) / 5) * 4) + p0_index;
            List<double> xDataList = new List<double>();
            List<double> yDataList = new List<double>();
            #endregion
            
            #region PolyNomial by Macd
            // MACDCalculator macdCalculator = new MACDCalculator();
            // List<Tuple<DateTime, double>> prices = new List<Tuple<DateTime, double>>();
            // int quad75 = ((x_index - p0_index) / 4) * 3 + p0_index;
            // foreach (Stock q in stocks)
            // {
            //     int index = stocks.IndexOf(q);
            //     if (index < p0_index)
            //         continue;
            //     if (index > x_index)
            //         break;
            //     prices.Add(new Tuple<DateTime, double>(q.Date, q.Close));
            // }

            // foreach (var price in prices)
            // {
            //     macdCalculator.AddPrice(price.Item1, price.Item2);
            // }
            // foreach (Stock q in stocks)
            // {
            //     int index = stocks.IndexOf(q);
            //     if (q.Date < p0.Date)
            //         continue;
            //     // if (index < highestPntIndex)
            //     //     continue;
            //     if (index > x_index)
            //         break;
            //     xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
            //     yDataList.Add(macdCalculator.GetMACDAt(q.Date));
            // }
            #endregion

            #region PolyNomial by Price
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                // Console.WriteLine(p1.Date + " : " + x.Date);// + " : " + )
                xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                yDataList.Add(q.Low);
            }
             // Convert lists to arrays
            double[] xData = xDataList.ToArray();
            double[] yData = yDataList.ToArray();
            
            #endregion
            
            #region Kitchen
            // Determine the best polynomial degree using BIC
            // Adjust the concavity (e.g., double the curvature)
            int maxDegree = 3; // Limit to a reasonable degree
            int bestDegree = 0;
            double bestBIC = double.MaxValue;
            double linearWeight = 1.0; // Emphasize the linear trend
            PolynomialRegression bestModel = null;

            // for (int degree = 1; degree <= maxDegree; degree++)
            // {
            //     // double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out var model);
            //     double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out PolynomialRegression model, linearWeight: 1.0);
            //     // Fit the polynomial regression with a longer linear period

            //     // var regression = new PolynomialRegression(xData, yData, degree, linearWeight);

            //     if (bic < bestBIC)
            //     {
            //         bestBIC = bic;
            //         bestDegree = degree;
            //         bestModel = model;
            //     }
            // }
            for (int degree = 2; degree <= maxDegree; degree++)
            {
                double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out PolynomialRegression model, linearWeight: 1.0); // : 1.0
                // Console.WriteLine($"Degree {degree}: BIC = {bic:0.###}");

                if (bic < bestBIC)
                {
                    bestBIC = bic;
                    bestDegree = degree;
                    bestModel = model;
                }
            }
            double concavityFactor = 1; // Higher values increase concavity
            // bestModel.AdjustConcavity(concavityFactor);
            // Console.WriteLine($"\nBest Polynomial Degree: {bestDegree} (BIC = {bestBIC:0.###})");
            // Identify trend and concavity if the best model is quadratic
            // Console.WriteLine(bestDegree);
            if (bestDegree == 2 && bestModel != null)
            {
                if (bestModel.IdentifyQuadraticTrend() == concav
                // && (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) < Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5) * 0.05)
                // && (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) < Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5) * 1)
                )
                {
                    // if (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) > Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5))
                    // {
                    //     Console.Write(Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5));
                    //     Console.Write(" : ");
                    //     Console.WriteLine(Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed)), 5));
                    // }
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        if (q.Date < p0.Date)
                            continue;
                        if (q.Date > x.Date)
                            break;
                        all = all + 1;

                        double yValue = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                        if (bestModel.GetCurveValue(Convert.ToDouble(q.Speed)) < lowestCurv)
                        {
                            lowestCurv = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                            lowestCurv_index = stocks.IndexOf(q);
                        }
                        // touching
                        if (q.High > yValue && q.Low < yValue)
                            touching = touching + 1;
                        // Filling above and below
                        if (q.High >= yValue && q.Low <= yValue)
                        {
                            above = above + (q.High - yValue);
                            below = below + (yValue - q.Low);
                        }
                        else if (q.High < yValue)
                            below = below + (q.High - q.Low);
                        else if (q.Low > yValue)
                            above = above + (q.High - q.Low);
                        // Empty above and below
                        if (q.High < yValue)
                            emptyBelow = emptyBelow + (yValue - q.High);
                        if (q.Low > yValue)
                            emptyAbove = emptyAbove + (q.Low - yValue);
                        
                    }
                    // check accu points
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        if (q.Date < p0.Date)
                            continue;
                        if (q.Date > x.Date)
                            break;
                    }
                    belowPer = below / (below + above);
                    #endregion

                    if (
                    // above < below * 1.382 && // goooooood
                    // above1 > below1 && //  >
                    // above2 > below2 && //  >
                    // above1 < above2 && //  <
                    // below1 < below2 && //  <
                    // x.High < LowestPoint(p0, x, stocks) + (HighestPoint(p0, x, stocks) - LowestPoint(p0, x, stocks)) * 0.50 &&
                    // touching / all >= 1 &&
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) // mid || quart_80
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed)) >
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) >
                    // (bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))) * 1
                    // && x.Close < stocks[x_index - 1].Close
                    // && x.Close > stocks[x_index - 1].Low
                    // && x.Low < stocks[x_index - 1].Low
                    // && stocks[x_index - 1].Close < stocks[x_index - 2].Close
                    // && x.Close < LowestPoint(p0, x, stocks) + (HighestPoint(p0, x, stocks) - LowestPoint(p0, x, stocks)) * 0.50

                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))) * 1
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed))) * 1
                    // && lowestCurv_index - p0_index >= x_index - lowestCurv_index
                    // && lowestCurv_index > stocks.IndexOf(stocks[(int)quart_80])
                    // && lowestCurv_index < x_index
                    // && lowestCurv_index > mid
                    )
                    {
                        return 1;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
            {
                // Console.WriteLine("The best model is not quadratic.");
                return 0;
            }
        }
        public static double QuadHigh(Stock p0, Stock x, int p0_index, int x_index, double concav, List<Stock> stocks)
        {
            #region Vars
            double all = 0; double touching = 0;;
            double above = 0; double below = 0;
            double above1 = 0; double below1 = 0;
            double above2 = 0; double below2 = 0;
            double emptyBelow = 0; double emptyAbove = 0;
            double belowPer = 0; 
            double lowestCurv = 10000; int lowestCurv_index = 0;
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_80 = (((x_index - p0_index) / 5) * 4) + p0_index;
            List<double> xDataList = new List<double>();
            List<double> yDataList = new List<double>();
            #endregion
            
            #region PolyNomial by Macd
            // MACDCalculator macdCalculator = new MACDCalculator();
            // List<Tuple<DateTime, double>> prices = new List<Tuple<DateTime, double>>();
            // int quad75 = ((x_index - p0_index) / 4) * 3 + p0_index;
            // foreach (Stock q in stocks)
            // {
            //     int index = stocks.IndexOf(q);
            //     if (index < p0_index)
            //         continue;
            //     if (index > x_index)
            //         break;
            //     prices.Add(new Tuple<DateTime, double>(q.Date, q.Close));
            // }

            // foreach (var price in prices)
            // {
            //     macdCalculator.AddPrice(price.Item1, price.Item2);
            // }
            // foreach (Stock q in stocks)
            // {
            //     int index = stocks.IndexOf(q);
            //     if (q.Date < p0.Date)
            //         continue;
            //     // if (index < highestPntIndex)
            //     //     continue;
            //     if (index > x_index)
            //         break;
            //     xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
            //     yDataList.Add(macdCalculator.GetMACDAt(q.Date));
            // }
            #endregion

            #region PolyNomial by Price
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                // Console.WriteLine(p1.Date + " : " + x.Date);// + " : " + )
                xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                yDataList.Add(q.High);
            }
             // Convert lists to arrays
            double[] xData = xDataList.ToArray();
            double[] yData = yDataList.ToArray();
            
            #endregion
            
            #region Kitchen
            // Determine the best polynomial degree using BIC
            // Adjust the concavity (e.g., double the curvature)
            int maxDegree = 3; // Limit to a reasonable degree
            int bestDegree = 0;
            double bestBIC = double.MaxValue;
            double linearWeight = 1.0; // Emphasize the linear trend
            PolynomialRegression bestModel = null;

            // for (int degree = 1; degree <= maxDegree; degree++)
            // {
            //     // double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out var model);
            //     double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out PolynomialRegression model, linearWeight: 1.0);
            //     // Fit the polynomial regression with a longer linear period

            //     // var regression = new PolynomialRegression(xData, yData, degree, linearWeight);

            //     if (bic < bestBIC)
            //     {
            //         bestBIC = bic;
            //         bestDegree = degree;
            //         bestModel = model;
            //     }
            // }
            for (int degree = 2; degree <= maxDegree; degree++)
            {
                double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out PolynomialRegression model, linearWeight: 1.0); // : 1.0
                // Console.WriteLine($"Degree {degree}: BIC = {bic:0.###}");

                if (bic < bestBIC)
                {
                    bestBIC = bic;
                    bestDegree = degree;
                    bestModel = model;
                }
            }
            double concavityFactor = 1; // Higher values increase concavity
            // bestModel.AdjustConcavity(concavityFactor);
            // Console.WriteLine($"\nBest Polynomial Degree: {bestDegree} (BIC = {bestBIC:0.###})");
            // Identify trend and concavity if the best model is quadratic
            // Console.WriteLine(bestDegree);
            if (bestDegree == 2 && bestModel != null)
            {
                if (bestModel.IdentifyQuadraticTrend() == concav
                // && (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) < Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5) * 0.05)
                // && (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) < Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5) * 1)
                )
                {
                    // if (Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5) > Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed) * 0.25), 5))
                    // {
                    //     Console.Write(Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index].Speed)), 5));
                    //     Console.Write(" : ");
                    //     Console.WriteLine(Math.Round(bestModel.GetCurveValue(Convert.ToDouble(stocks[x_index - 1].Speed)), 5));
                    // }
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        if (q.Date < p0.Date)
                            continue;
                        if (q.Date > x.Date)
                            break;
                        all = all + 1;

                        double yValue = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                        if (bestModel.GetCurveValue(Convert.ToDouble(q.Speed)) < lowestCurv)
                        {
                            lowestCurv = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                            lowestCurv_index = stocks.IndexOf(q);
                        }
                        // touching
                        if (q.High > yValue && q.Low < yValue)
                            touching = touching + 1;
                        // Filling above and below
                        if (q.High >= yValue && q.Low <= yValue)
                        {
                            above = above + (q.High - yValue);
                            below = below + (yValue - q.Low);
                        }
                        else if (q.High < yValue)
                            below = below + (q.High - q.Low);
                        else if (q.Low > yValue)
                            above = above + (q.High - q.Low);
                        // Empty above and below
                        if (q.High < yValue)
                            emptyBelow = emptyBelow + (yValue - q.High);
                        if (q.Low > yValue)
                            emptyAbove = emptyAbove + (q.Low - yValue);

                        if (index <= mid)
                        {
                            above1 = above1 + (q.High - yValue);
                            below1 = below1 + (yValue - q.Low);
                        }
                        if (index >= mid)
                        {
                            above2 = above2 + (q.High - yValue);
                            below2 = below2 + (yValue - q.Low);
                        }
                    }
                    // check accu points
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        if (q.Date < p0.Date)
                            continue;
                        if (q.Date > x.Date)
                            break;
                    }
                    belowPer = below / (below + above);
                    #endregion

                    if (
                    // above < below * 1.382 && // goooooood
                    // above1 > below1 && //  >
                    // above2 > below2 && //  >
                    // above1 < above2 && //  <
                    // below1 < below2 && //  <
                    // x.High < LowestPoint(p0, x, stocks) + (HighestPoint(p0, x, stocks) - LowestPoint(p0, x, stocks)) * 0.50 &&
                    // touching / all >= 1 &&
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) // mid || quart_80
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed)) >
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) >
                    // (bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))) * 1
                    // && x.Close < stocks[x_index - 1].Close
                    // && x.Close > stocks[x_index - 1].Low
                    // && x.Low < stocks[x_index - 1].Low
                    // && stocks[x_index - 1].Close < stocks[x_index - 2].Close
                    // && x.Close < LowestPoint(p0, x, stocks) + (HighestPoint(p0, x, stocks) - LowestPoint(p0, x, stocks)) * 0.50

                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))) * 1
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed))) * 1
                    // && lowestCurv_index - p0_index >= x_index - lowestCurv_index
                    // lowestCurv_index > stocks.IndexOf(stocks[(int)quart_80])
                    )
                    {
                        return 1;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
            {
                // Console.WriteLine("The best model is not quadratic.");
                return 0;
            }
        }
    }
}