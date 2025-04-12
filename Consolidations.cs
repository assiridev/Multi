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
                #region Vars
                double support = 0;
                double qud = 0; double qudup = 0;
                double qud1 = 0; int mid = 0;
                double qud2 = 0; 
                double qudLow = 0;
                double qudHigh = 0;
                double signal = 0;
                double reg = 0; double regLow = 0; double regHigh = 0;
                double highestP = 0; double lowestP = 0;
                int o1_index = stockList.IndexOf(o1);
                #endregion
                #region Loops
                foreach (Stock o2 in stockList) // second loooooooop
                {
                    highestP = HighestPoint(o1, o2, stockList);
                    lowestP = LowestPoint(o1, o2, stockList);
                    int o2_index = stockList.IndexOf(o2);
                    if (o2.Date >= o1.Date)
                    {
                        // if (o2_index - o1_index > 10)
                        //     break;
                        // if (o2_index - o1_index < 6)
                        //     continue;
                        if (o2_index - o1_index > 5)// && o2_index - o1_index <= 10) // 10 30
                        {
                #endregion
                            #region Quad
                            // quad close
                            // signal = SignalLine(o1, o2, stockList);
                            // reg = regLine(o1, o2, stockList);
                            // if (accuCounter(o1, o2, stockList) < (o2_index - o1_index + 1) / 2)
                            //     continue;
                            // mid = o1_index + (o2_index - o1_index) / 2;
                            // qud1 = QuadLow(o1, stockList[(int)mid], o1_index, mid, -1, stockList);
                            // qud2 = QuadLow(stockList[(int)mid], o2, mid, o2_index, -1, stockList);
                            if (accuCounter(o1, o2, stockList) < 5)
                                continue;
                            if (CndlPassCounter(o1, o2, stockList) < (o2_index - o1_index + 1) / 2)
                                continue;
                            support = Support(o1, o2, o1_index, o2_index, stockList);
                            // qud = Quad(o1, o2, o1_index, o2_index, -1, stockList); // -1
                            qudup = QuadUp(o1, o2, o1_index, o2_index, 1, stockList); // -1
                            // qudLow = QuadLow(o1, o2, o1_index, o2_index, -1, stockList); // 1
                            // qudHigh = QuadHigh(o1, o2, o1_index, o2_index, -1, stockList); // 1
                            // reg = regLine(o1, o2, stockList); // && support >= 1 
                            // if (qud == 1 && (qudLow == 1 || qudHigh == 1))// && o2.Accupoint_2 == 1)// && qud2 == 1)// && support >= 1)//
                            if (support >= 1 && /*qud == 1 &&*/ qudup == 1)// && o2.Accupoint_2 == 1)// && regLine(o1, stockList[(int)mid], stockList) < regLine(stockList[(int)mid], o2, stockList))
                            // if (o2.Accupoint_2 == 1 && reg > 0)// && support >= 1)
                            // if (qud1 == 1 && qud2 == 1 && o1.Low < o2.Low)
                                // if (GetConsolidationsBigPic(o1, o2, o1_index, o2_index, lowestP, highestP, o2_index - o1_index, regLow, stockList) == 1)
                                consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date, lowestP));
                            else
                                continue;
                            #endregion
                            #region quadLow and High
                            // // quad high && low
                            // qudLow = QuadLow(o1, o2, o1_index, o2_index, 1, stockList);
                            // qudHigh = QuadHigh(o1, o2, o1_index, o2_index, 1, stockList);
                            // // support = Support(o1, o2, o1_index, o2_index, stockList);
                            // if (o2.Accupoint_2 == 1 && qudLow > qudHigh && qudHigh > 1)
                            // // if (qudLow < 0 && qudHigh < 0 && support > 0)// && qudLow > qudHigh)
                            //     // if (GetConsolidationsBigPic(o1, o2, o1_index, o2_index, lowestP, highestP, o2_index - o1_index, reg, stockList) == 1)
                            //         consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date, qudHigh));
                            // else
                            //     continue;
                            #endregion
                            #region regLine
                            // // regLine
                            // // reg = regLine(o1, o2, stockList);
                            // // signal = SignalLine(o1, o2, stockList);
                            // regLow = regLineLow(o1, o2, stockList);
                            // regHigh = regLineHigh(o1, o2, stockList);
                            // support = Support(o1, o2, o1_index, o2_index, stockList);
                            // // if (reg < 0)// && support > 0)
                            // if (regLow < 0 && regHigh < 0 && regLow < regHigh && support > 0)// && regLow > regHigh) // good
                            // // if (reg < 0 && signal > 0)// && regLow < 0 && regHigh < 0 && regLow < regHigh) // good
                            // // if (regLow < 0 && regHigh < 0)// && regLow < regHigh) // good
                            //     if (GetConsolidationsBigPic(o1, o2, o1_index, o2_index, lowestP, highestP, o2_index - o1_index, regLow, stockList) == 1)
                            //         consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date, lowestP));
                            // else
                            //     continue;
                            #endregion
                            #region Quad with regLine
                            // // Quad with regLine
                            // qud = Quad(o1, o2, o1_index, o2_index, 1, stockList);
                            // // reg = regLine(o1, o2, stockList);
                            // // signal = SignalLine(o1, o2, stockList);
                            // regLow = regLineLow(o1, o2, stockList);
                            // regHigh = regLineHigh(o1, o2, stockList);
                            // // if (regLow > 0 && regHigh > 0 && regLow < regHigh) // good
                            // if (regLow < 0 && regHigh < 0 && regLow < regHigh && qud == 1) // good
                            // // if (qud == 1)// && reg < 0)// signal > 0
                            //     if (GetConsolidationsBigPic(o1, o2, highestP, lowestP, o2_index - o1_index, stockList) == 1)
                            //         consolidations.Add(new Consolidations(o1.Name, o1.Date, o2.Date, lowestP));
                            // else
                            //     continue;
                            #endregion
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

        public static double GetConsolidationsBigPic(Stock start, Stock end, int start_index, int end_index, double curvLowest, double curvHighest, int conPeriod, double biggerReg, List<Stock> stockList)
        {
            double conPer = 0.5;
            int daysCnt = 2; // regline we use (conPeriod / 2) // quads we use (3)
            double consolidations = 0;
            #region accu by LowestHigh & HighestLow
            foreach (Stock o1 in stockList) // first loooooooop
            {
                if (o1.Date < start.Date)
                        continue;
                if (o1.Date > end.Date)
                    break;
                double qud = 0;
                double highestP = 0; double lowestP = 0;
                double lowestHigh = 10000; double highestLow = 0; double o2Total = 0;
                double reg = 0; double regLow = 0; double regHigh = 0; double signal = 0;
                double accuScore = 0; double n = 1; // 1 = new accu
                int o1_index = stockList.IndexOf(o1);
                foreach (Stock o2 in stockList) // second loooooooop
                {
                    if (o2.Date < start.Date)
                        continue;
                    if (o2.Date > end.Date)
                        break;
                    int o2_index = stockList.IndexOf(o2);
                    if (o2.Date >= o1.Date)
                    {
                        o2Total = o2Total + (o2.High - o2.Low);
                        if (o2.High <= lowestHigh)
                            lowestHigh = o2.High;
                        if (o2.Low >= highestLow)
                            highestLow = o2.Low;
                        if (o2_index - o1_index >= daysCnt)// && o2_index - o1_index <= conPeriod / 2)
                        {
                            #region new code
                            // // the new code
                            // if (o1.Date == start.Date)// && (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total > conPer)
                            // {
                            //     // Console.WriteLine(" 1: " + (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total + " " + o1.Date + " " + o2.Date);
                            //     reg = regLine(o1, o2, stockList);
                            //     if (reg < 0)
                            //     {
                            //         if (o2.Date == end.Date)// && (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total > conPer)
                            //         {
                            //             // Console.WriteLine(" 2: " + (lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total + " " + o1.Date + " " + o2.Date);
                            //             reg = regLine(o1, o2, stockList);
                            //             if (reg > 0)
                            //             {
                            //                 consolidations = 1;
                            //                 return consolidations;
                            //             }
                            //         }
                            //     }
                            // }
                            #endregion

                            // // the old code
                            // lowestP = LowestPoint(o1, o2, stockList);
                            if ((lowestHigh - highestLow) * (o2_index - o1_index + 1) / o2Total > conPer && o2.Date == end.Date)// && curvLowest > lowestP)//curvLowest + (curvHighest - curvLowest) * 0.50)// && o2.Close > highestLow)
                            {
                                // qud = Quad(o1, o2, o1_index, o2_index, 1, stockList);
                                // if (qud == 1)
                                    return 1;
                            //     highestP = HighestPoint(o1, o2, stockList);
                            //     lowestP = LowestPoint(o1, o2, stockList);
                            //     regLow = regLineLow(o1, o2, stockList);
                            //     regHigh = regLineHigh(o1, o2, stockList);
                                // consolidations = consolidations + 1;
                                // if (consolidations / (double)(end_index - start_index + 1) > 0.20)
                                // if (reg < 0 && reg < biggerReg)
                                // if (regLow < 0 && regHigh < 0)// /*&& regLow < regHigh*/ && o2.Close > lowestP + (highestP + lowestP) * 0.25)
                                // {
                                    // return 1;
                                // }
                            }
                            //     // lowestP = LowestPoint(o1, stockList[o2_index - 1], stockList);
                            //     // highestP = HighestPoint(o1, o2, stockList);
                            //     // lowestP = LowestPoint(o1, o2, stockList);
                            //     // if (WhereEmpty(o1, o2, lowestHigh, highestLow, highestP, lowestP, stockList) == true
                            //     // // // && o2.Close < LowestPoint(o1, stockList[o2_index - 1], stockList)
                            //     // // // && o2.Close < HighestPoint(o1, stockList[o2_index - 1], stockList)
                            //     // )
                            //     // {
                            //     //     consolidations = 1;
                            //     //     return consolidations;
                            //     // }
                            //     // signal = SignalLine(o1, o2, stockList);
                            //     // qud = Quad(o1, o2, o1_index, o2_index, 1, stockList);
                            //     // if (qud == 1 && o2.Close > lowestP)
                            //     // regLow = regLineLow(o1, o2, stockList);
                            //     // regHigh = regLineHigh(o1, o2, stockList);
                            //     // if (regLow < 0 && regHigh < 0 && o2.Close > lowestP)// && regLow < regHigh)// && signal > 0)
                            //     // reg = regLine(o1, o2, stockList);
                            //     // if (reg < 0 && o2.Close > lowestP)
                            //     // // if (regLow < 0 && regHigh < 0 && regLow > curvLowest && regHigh > curvHighest && o2.Close > lowestP)
                            //     // if (o2.Close > lowestP)
                            //     // if (signal > 0)
                            //     // {
                            //     //     return 1;
                            //     // }
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
            return 0;
        }

        public static bool WhereEmpty(Stock start, Stock end, double accuTop, double accuBot, double highest, double lowest, List<Stock> stocks)
        {
            double fillTop = 0; double fillBot = 0;
            double emptyTop = 0; double emptyBot = 0;
            foreach (Stock o in stocks)
            {
                if (o.Date < start.Date)
                    continue;
                if (o.Date > end.Date)
                    break;
                if (o.High > accuTop)
                {
                    fillTop = fillTop + (o.High - accuTop);
                    emptyTop = emptyTop + (highest - o.High);
                }
                if (o.Low < accuBot)
                {
                    fillBot = fillBot + (accuBot - o.Low);
                    emptyBot = emptyBot + (o.Low - lowest);
                }
            }
            // return fillTop > fillBot; // good
            // return fillTop / (fillTop + emptyTop) > fillBot / (fillBot + emptyBot); // neutral
            return fillTop / (fillTop + emptyTop) < fillBot / (fillBot + emptyBot); // bad
            // return emptyTop / (fillTop + emptyTop) > emptyBot / (fillBot + emptyBot); // bad
            // return emptyTop / (fillTop + emptyTop) < emptyBot / (fillBot + emptyBot); // neutral
            // return fillTop + emptyTop > fillBot + emptyBot; // bad
            // return fillTop + emptyTop < fillBot + emptyBot; // neutral
            // return fillBot / (fillBot + emptyBot) > 0.50;
            // return fillTop / (fillTop + emptyTop) > 0.50 && fillBot / (fillBot + emptyBot) > 0.50
            // && fillTop / (fillTop + emptyTop) > fillBot / (fillBot + emptyBot);
        }
        #region Regs

        public static double regLine(Stock p0, Stock x, List<Stock> stocks)
        {
            double all = 0; double touching = 0;
            double closeAbove = 0; double closeBelow = 0;
            double totalAbove = 0; double totalBelow = 0;
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
            List<MacdPoint> macdPoints = MacdCalculator.CalculateMacd(candles, 12, 26);
            
            var macdRegressionLine = MacdRegressionCalculator.ComputeLinearRegression(macdPoints);
            #endregion

            var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Close);
            var stats = RegressionMetrics.ComputeStats(candles, regressionLineAvg);
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
                if (q.Close > lineValueAtTime)
                    closeAbove = closeAbove + 1;
                if (q.Close <= lineValueAtTime)
                    closeBelow = closeBelow + 1;
                totalAbove = totalAbove + (q.High - lineValueAtTime);
                totalBelow = totalBelow + (lineValueAtTime - q.Low);
            }
            if (
            // 1 == 1
            touching / all >= 1
            && totalBelow / (totalAbove + totalBelow) > 0.50
            // && touching / all >= 0.50
            // stats.R2 < 0.50
            // stats.MSE < 0.01
            // && closeBelow / all > 0.60// <= closeAbove
            // && macdRegressionLine.Slope < 0 && touching / all > 0.60
            // && macdRegressionLine.Slope - regressionLineAvg.Slope > 0
            // if (regressionLineAvg.Slope < 0 && macdRegressionLine.Slope < 0 //&& touching / all < 0.10
            // && macdRegressionLine.Slope < regressionLineAvg.Slope
            )
            return regressionLineAvg.Slope;
            else
                return 0;
        }
        public static double regLineLow(Stock p0, Stock x, List<Stock> stocks)
        {
            double all = 0; double touching = 0;
            double fill = 0; double empty = 0;
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
            List<MacdPoint> macdPoints = MacdCalculator.CalculateMacd(candles, 12, 26);
            
            var macdRegressionLine = MacdRegressionCalculator.ComputeLinearRegression(macdPoints);
            #endregion

            var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Low);
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
            if (
            1 == 1
            // regressionLineAvg.Slope < 0 &&
            // touching / all >= 0.70
            // && macdRegressionLine.Slope < 0 && touching / all > 0.60
            // && macdRegressionLine.Slope - regressionLineAvg.Slope > 0
            // if (regressionLineAvg.Slope < 0 && macdRegressionLine.Slope < 0 //&& touching / all < 0.10
            // && macdRegressionLine.Slope < regressionLineAvg.Slope
            )
                return regressionLineAvg.Slope;
            else
                return 0;
        }
        public static double regLineHigh(Stock p0, Stock x, List<Stock> stocks)
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
            List<MacdPoint> macdPoints = MacdCalculator.CalculateMacd(candles, 12, 26);
            
            var macdRegressionLine = MacdRegressionCalculator.ComputeLinearRegression(macdPoints);
            #endregion

            var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.High);
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
            if (
            1 == 1
            // regressionLineAvg.Slope < 0 &&
            // touching / all >= 0.50
            // && macdRegressionLine.Slope < 0 && touching / all > 0.60
            // && macdRegressionLine.Slope - regressionLineAvg.Slope > 0
            // if (regressionLineAvg.Slope < 0 && macdRegressionLine.Slope < 0 //&& touching / all < 0.10
            // && macdRegressionLine.Slope < regressionLineAvg.Slope
            )
                return regressionLineAvg.Slope;
            else
                return 0;
        }
        #endregion
        
        
        #region Quads

        public static double Quad(Stock p0, Stock x, int p0_index, int x_index, double concav, List<Stock> stocks)
        {
            #region Vars
            double all = 0; double touching = 0;;
            double above = 0; double below = 0;
            double above1 = 0; double below1 = 0;
            double above2 = 0; double below2 = 0;
            double belowPer = 0;  double lowAg= 0;
            double lowestCurv = 10000; int lowestCurv_index = 0;
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_25 = (((x_index - p0_index) / 4) * 1) + p0_index;
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
                if (q.Date <= p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                // if (q.Accupoint_2 == 1)
                // {
                    xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                    // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                    // yDataList.Add((q.High - q.Low));
                    yDataList.Add(q.Close);
                // }
                // xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                // // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                // yDataList.Add((q.High - q.Low) / 2);
                // yDataList.Add(q.Close);
                // yDataList.Add(Convert.ToDouble(q.Low + (lowAg + q.Low) / (index - p0_index)));
                // Console.WriteLine((lowAg + q.Low) / (index - p0_index));
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
                    // touching / all >= 0.50 &&
                    // above > below
                    // // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // 1 == 1
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) < bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) < bestModel.GetCurveValue(Convert.ToDouble(x.Speed))

                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) < bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed))
                    && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))

                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))

                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) < bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) < bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
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
                    // && lowestCurv_index < stocks.IndexOf(stocks[(int)quart_80])
                    // lowestCurv_index == x_index
                    )
                    {
                        return 1;//bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed));
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
        public static double QuadUp(Stock p0, Stock x, int p0_index, int x_index, double concav, List<Stock> stocks)
        {
            #region Vars
            double all = 0; double touching = 0;;
            double above = 0; double below = 0;
            double above1 = 0; double below1 = 0;
            double above2 = 0; double below2 = 0;
            double belowPer = 0;  double lowAg= 0;
            double lowestCurv = 10000; int lowestCurv_index = 0;
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_25 = (((x_index - p0_index) / 4) * 1) + p0_index;
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
                if (q.Date <= p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                if (q.Accupoint_2 == 1)
                {
                    xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                    // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                    // yDataList.Add((q.High - q.Low));
                    yDataList.Add(q.Close);
                }
                // xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                // // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                // yDataList.Add((q.High - q.Low) / 2);
                // yDataList.Add(q.Close);
                // yDataList.Add(Convert.ToDouble(q.Low + (lowAg + q.Low) / (index - p0_index)));
                // Console.WriteLine((lowAg + q.Low) / (index - p0_index));
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
                    // touching / all >= 0.50 &&
                    // above > below
                    // // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // 1 == 1
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) < bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) < bestModel.GetCurveValue(Convert.ToDouble(x.Speed))

                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) < bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed))
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))

                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))

                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) < bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) < bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
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
                    // && lowestCurv_index < stocks.IndexOf(stocks[(int)quart_80])
                    // lowestCurv_index == x_index
                    )
                    {
                        return 1;//bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed));
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
        public static double QuadLow(Stock p0, Stock x, int p0_index, int x_index, double concav, List<Stock> stocks)
        {
            #region Vars
            double all = 0; double touching = 0;;
            double above = 0; double below = 0;
            double above1 = 0; double below1 = 0;
            double above2 = 0; double below2 = 0;
            double emptyBelow = 0; double emptyAbove = 0;
            double belowPer = 0;
            double torque = 0; double square = 0; double torqueTOT = 0;
            double torque1 = 0; double square1 = 0; double torqueTOT1 = 0;
            double torque2 = 0; double square2 = 0; double torqueTOT2 = 0;
            double highest = HighestPoint(p0, x, stocks);
            double lowest = LowestPoint(p0, x, stocks);
            double lowestCurv = 10000; int lowestCurv_index = 0;
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_80 = (((x_index - p0_index) / 5) * 4) + p0_index;
            double highest1 = HighestPoint(p0, stocks[(int)mid], stocks); double lowest1 = LowestPoint(stocks[(int)mid], x, stocks);
            double highest2 = HighestPoint(p0, stocks[(int)mid], stocks); double lowest2 = LowestPoint(stocks[(int)mid], x, stocks);
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
                // if (q.Accupoint_2 == 1)
                // {
                    // Console.WriteLine(p1.Date + " : " + x.Date);// + " : " + )
                    xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                    // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                    yDataList.Add(q.Low);
                // }
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
                        // torque implementation
                        torque = torque + (q.High - q.Low);
                        square = square + (highest - lowest);

                        if (index <= mid)
                        {
                            torque1 = torque1 + (q.High - q.Low);
                            square1 = square1 + (highest1 - lowest1);
                        }
                        if (index >= mid)
                        {
                            torque2 = torque2 + (q.High - q.Low);
                            square2 = square2 + (highest2 - lowest2);
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
                    torqueTOT = torque / square;
                    torqueTOT1 = torque1 / square;
                    torqueTOT2 = torque2 / square;
                    #endregion

                    if (
                    // above < below * 1.382 && // goooooood
                    // above1 > below1 && //  >
                    // above2 > below2 && //  >
                    // above1 < above2 && //  <
                    // below1 < below2 && //  <
                    // x.High < LowestPoint(p0, x, stocks) + (HighestPoint(p0, x, stocks) - LowestPoint(p0, x, stocks)) * 0.50 &&
                    // touching / all >= 1 &&
                    // torqueTOT > 0.30 &&
                    // torqueTOT1 < torqueTOT2 * 1
                    // torqueTOT1 > 0.30 &&
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) < bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
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
                    )
                    {
                        return 1;//bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed));
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
                // if (q.Accupoint_2 == 1)
                // {
                    // Console.WriteLine(p1.Date + " : " + x.Date);// + " : " + )
                    xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                    // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                    yDataList.Add(q.High);
                // }
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
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) // mid || quart_80
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
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
                    // && lowestCurv_index < x_index
                    )
                    {
                        return 1;//bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed));
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
        #endregion


        public static double SignalLine(Stock p0, Stock x, List<Stock> stocks)
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
            List<MacdPoint> macdPoints = MacdCalculator.CalculateMacd(candles, 12, 26);
            var signalPoints = MacdCalculator.CalculateSignal(macdPoints, 9);

            var macdRegressionLine = MacdRegressionCalculator.ComputeLinearRegression(macdPoints);
            var signalRegLine = MacdRegressionCalculator.ComputeLinearRegression(signalPoints);

            #endregion

            var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Average);
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
            // if (
            // signalRegLine.Slope > 0 //&& touching / all <= 0.50
            // // macdRegressionLine.Slope > 0// && touching / all == 1
            // // && macdRegressionLine.Slope < 0 && touching / all > 0.60
            // // && macdRegressionLine.Slope - regressionLineAvg.Slope > 0
            // // if (regressionLineAvg.Slope < 0 && macdRegressionLine.Slope < 0 //&& touching / all < 0.10
            // // && macdRegressionLine.Slope < regressionLineAvg.Slope
            // )
            return signalRegLine.Slope;
            // else
            //     return 0;
        }
        public static double Support(Stock p1, Stock x, int p1_index, int x_index, List<Stock> stocks)
        {
            int supCntr = 0; int cntr = 0;
            int mid = p1_index + (x_index - p1_index) / 2;
            int lowestIndx = LowestPoint_index(p1, stocks[x_index - 1], stocks);
            foreach (Stock outlier in stocks)
            {
                int index = stocks.IndexOf(outlier);
                if (index < p1_index)// - 30)
                    continue;
                // if (index > mid)
                //     break;
                if (index >= x_index) // p1_index for supports before the pattern
                    break;
                {
                    cntr = cntr + 1;
                    if (
                    (outlier.Type == "b" &&
                    x.Low < outlier.Low && x.Close > outlier.Low
                    && findOutliersX(cntr, outlier, p1, x, x_index, stocks) == true) // check empty before X
                    // || 
                    // (outlier.Type == "b" && stocks[lowestIndx].Type == "b"
                    // && stocks[lowestIndx].Low < outlier.Low && stocks[lowestIndx].Close > outlier.Low
                    // && findOutliersX(outlier, x, x_index, stocks) == true)
                    )
                    {
                        supCntr = supCntr + 1;
                        // to find specific supports
                        // if (p1.Date == p1d && p2.Date == p2d && x.Date == xd)
                        // {
                        //     Console.WriteLine(outlier.Date);
                        //     Console.WriteLine((x_index - index) / (x_index - p1_index));
                        // }
                    }
                }
                
            }
            return supCntr;
        }
        private static bool findOutliersX(int cntr, Stock outlier, Stock p1, Stock x, int x_index, List<Stock> stocks)
        {
            int beforeX = 0;
            foreach (Stock before in Enumerable.Reverse(stocks))
            {
                int index = stocks.IndexOf(before);
                if (before.Date < p1.Date)
                    break;
                if (before.Date <= x.Date)
                {
                    // Console.WriteLine(cntr);
                    // Console.WriteLine(before.Date);
                    // Task.Delay(1000).Wait();
                    if (before.Low > outlier.Low// && before.Close > stocks[index - 1].Close && before.Close > stocks[index + 1].Close
                    && beforeX == 0
                    )
                        // beforeX = 1;
                        return true;
                    if (before.Close < outlier.Low
                    && beforeX == 0
                    )
                        return false;
                    // if (before.Low <= (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index))
                    // && beforeX == 1
                    // )
                    // {
                    //     return true;
                    // }
                }
            }
            return false;
        }

        private static double accuCounter(Stock from, Stock to, List<Stock> stocks)
        {
            int cntr = 0; double accuLow = 10000;
            foreach (Stock q in stocks)
            {
                if (q.Date < from.Date)
                    continue;
                if (q.Date > to.Date)
                    break;
                if (q.Accupoint_2 == 1 && q.Low < accuLow)
                {
                    cntr = cntr + 1;
                    accuLow = q.Low;
                }
            }
            return cntr;
        }
        private static double CndlPassCounter(Stock from, Stock to, List<Stock> stocks)
        {
            int cntr = 0;
            foreach (Stock q in stocks)
            {
                if (q.Date < from.Date)
                    continue;
                if (q.Date > to.Date)
                    break;
                if (q.Low < to.Close && q.High > to.Close)
                {
                    cntr = cntr + 1;
                }
            }
            return cntr;
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
        private static int LowestPoint_index(Stock from, Stock to, List<Stock> stocks)
        {
            double lowest = 10000;
            int index = 0;
            foreach (Stock c in stocks)
            {
                if (c.Date >= from.Date && c.Date <= to.Date)
                {
                    if (c.Low < lowest)
                    {
                        lowest = c.Low;
                        index = stocks.IndexOf(c);
                    }
                }
            }
            return index;
        }
        public int CompareTo(Consolidations that)
        {
            return this.Name.CompareTo(that.Name);
        }

        
    }
}