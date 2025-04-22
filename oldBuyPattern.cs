#region High / Low
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.SqlClient;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Curvy
{
    class BuyPattern
    {
        #region Properties
        public DateTime P1 { get; set; }
        public DateTime P2 { get; set; }
        public DateTime XX { get; set; }
        public double Target { get; set; }
        public int Status { get; set; }
        public int Duplicate { get; set; }
        public double Profit { get; set; }
        public double Loss { get; set; }
        public double Tradeopen { get; set; }
        public double Stoploss { get; set; }
        public double Period { get; set; }
        public double Cross { get; set; }
        public double Cancel { get; set; }
        public static DateTime BeforeXTopDateTime { get; set; }
        public static double HighestBefX { get; set; }
        public static int HighestBeFXindex { get; set; }
        #endregion

        public BuyPattern(DateTime p1, DateTime p2, DateTime xx, double target, double stoploss, int status, int duplicate, double profit, double loss, double period, double cross, double tradeopen, double cancel)
        {
            P1 = p1; P2 = p2; XX = xx; Target = target; Stoploss = stoploss; Status = status; Duplicate = duplicate; Profit = profit; Loss = loss; Period = period; Cross = cross; Tradeopen = tradeopen; Cancel = cancel;
        }
        public static List<BuyPattern> ThePattern(List<Stock> stocks, List<Stocks5min> stocks5min, int year)
        {
            Dictionary<string, double> values = new Dictionary<string, double>();
            double cancel= 0; 
            int p1_index; int p2_index; int x_index; double crossing = 0; 
            int p11_index; int p22_index; int xx_index; double innerCrossing = 0;
            
            List<BuyPattern> patterns = new List<BuyPattern>();
            // Find p1
            foreach (Stock p1 in stocks)
            {
            // if (p1.Mid == 0)
            //     continue;
            if (p1.Accupoint_2 == 0)
                continue;
            if (p1.Type == "Mdl") { p1_index = stocks.IndexOf(p1); } else { continue; }
            p1.Accupoint_1 = 4;
            // if (p1.Accupoint_2 != 1) // to make sure before x there is an accu
            //     continue;
            // if (p1.Accupoint_2 == 1) { p1_index = stocks.IndexOf(p1); } else { continue; }
                // Find p2
                foreach (Stock p2 in stocks)
                {
                    if (p2.Date > p1.Date)
                    {
                        // if (p2.Mid == 0)
                        //     continue;
                        if (p2.Accupoint_2 == 0)
                            continue;
                        if (p2.Mid > p1.Mid) continue; // Howwa daa (<) up Trend ... (>) down Trend
                        if (p2.Type == "Mdl") { p2_index = stocks.IndexOf(p2); } else { continue; }
                        p2.Accupoint_1 = 4;
                        // if (p2.Accupoint_2 != 1) // to make sure before x there is an accu
                        //     continue;
                        // if (p2.Low < p1.Low) continue; // Howwa daa (<) up Trend ... (>) down Trend
                        // if (p2.Accupoint_2 == 1) { p2_index = stocks.IndexOf(p2); } else { continue; }
                        // // Find x
                        foreach (Stock x in stocks)
                        {
                            if (x.Date > p2.Date)// && x.Date.Year >= year)
                            {
                                // if (x.Mid == 0)
                                //     continue;
                                if (x.Accupoint_2 == 0)
                                    continue;
                                if (x.Mid > p2.Mid) continue; // Howwa daa (<) up Trend ... (>) down Trend
                                if (x.Type == "Mdl") { x_index = stocks.IndexOf(x); } else { continue; }
                                x.Accupoint_1 = 4;
                                // if (stocks[x_index - 1].Accupoint_2 != 1) // to make sure before x there is an accu
                                //     continue;
                                // if (x.Accupoint_2 == 1) { x_index = stocks.IndexOf(x); } else { continue; }
                                // check the trend
                                if (x_index - p1_index < 5) // 5 daily
                                    continue;
                                if (x_index - p1_index > 90)// 90 daily
                                    break;
                                if (p2_index - p1_index < 3)
                                    continue;
                                if (x_index - p2_index < 3)
                                    continue;
                                // if (accuCounter(p1, p2, stocks) < 4)
                                //     continue;
                                // if (accuCounter(p1, x, stocks) < 3)
                                //     continue;
                                crossing = Crossing(p1, p2, x, p1_index, p2_index, x_index);
                                // crossing = HighestLow(p1.Low, p2.Low); // support
                                // double lowestpnt = LowestPoint(p1, stocks[x_index - 1], p1_index, x_index, stocks);
                                // double highestpnt = HighestPoint(p1, stocks[x_index - 1], p1_index, x_index, stocks);
                                // int lowestpnt_index = LowestPoint_index(p1, x, p1_index, x_index, stocks);
                                // int highestpnt_index = HighestPoint_index(p1, x, p1_index, x_index, stocks);
                                // if (x_index - highestpnt_index < 3)
                                //     continue;
                                if (1 == 1//x.Low <= crossing && x.Close > crossing // lowestpnt x.Low <= crossing &&
                                )
                                {
                                    // TradeOpen
                                    double tradeopen = crossing;// LowestPoint(stocks[x_index - 4], x, x_index, x_index, stocks);
                                    int mid = ((x_index - p1_index) / 2) + p1_index;
                                    int quad = ((x_index - p1_index) / 4) + p1_index;
                                    int quart_80 = (((x_index - p1_index) / 5) * 4) + p1_index;
                                    // buy
                                    if ((HighestPoint(p1, x, x_index, x_index, stocks) - tradeopen) * 0.25 / tradeopen < 0.005) // mid
                                        continue;

                                    // if (HighestPoint(stocks[(int)mid], x, x_index, x_index, stocks) > HighestPoint(p1, stocks[(int)mid], x_index, x_index, stocks))
                                    //     continue;
                                    // if(findOutliersImp(p1, p2, x, p1_index, p2_index, x_index, crossing, stocks) == false)
                                    //     continue;
                                    // if(findOutliersBeforeP2(p1, p2, x, p1_index, p2_index, x_index, crossing, stocks) == false)
                                    //     continue;
                                    // if(findOutliersAfterP2(p1, p2, x, p1_index, p2_index, x_index, crossing, stocks) == false)
                                    //     continue;
                                    // if(findOutliersX(p1, p2, x, p1_index, p2_index, x_index, crossing, stocks) == false)
                                    //     continue;
                                    // if (x_index - HighestBeFXindex < 3)
                                    //     continue;
                                    // if (accuCounter(p1, x, stocks) < 5)
                                    //     continue;
                                    try
                                    {
                                        if (conditions(p1, p2, x, p1_index, p2_index, x_index, crossing, tradeopen, stocks, stocks5min, year))
                                        {
                                            // Console.WriteLine("here");
                                            // if ((x.Date - BeforeXTopDateTime).Days < 5) // needed for quad only to prevent errors
                                            //     continue;
                                            cancel = tradeopen + (HighestPoint(p1, x, x_index, x_index, stocks) - tradeopen) * 0.50;
                                            // cancel = tradeopen + (HighestBefX - tradeopen) * 5;
                                            int target_index = 0;
                                            values = setValues(p1, p2, x, p1_index, p2_index, x_index, crossing, tradeopen, stocks);
                                            patterns.Add(new BuyPattern(p1.Date, p2.Date, x.Date, values["target"], values["stoploss"], 1, 0, values["profit_per"], values["loss_per"], 0, crossing, tradeopen, cancel));
                                            continue;
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            // to find duplicates in my list of patterns
            List<BuyPattern> temp = removeDuplicates(patterns);
            // return temp;
            List<BuyPattern> afterTargets = findTargets(temp, stocks5min);
            // List<BuyPattern> afterTargets = findMultipleTargets(stocks, temp, stocks5min);
            return afterTargets;
        }

        private static List<BuyPattern> removeDuplicates(List<BuyPattern> patterns)
        {
            #region remove duplicates my m pat
            patterns.Reverse();
            List<BuyPattern> TempList = new List<BuyPattern>();
            List<BuyPattern> TempList1 = new List<BuyPattern>();
            List<BuyPattern> TempList2 = new List<BuyPattern>();
            foreach (BuyPattern u1 in patterns)
            {
                bool duplicatefound = false;
                foreach (BuyPattern u2 in TempList)
                    if (u1.P1 == u2.P1 && u1.P2 == u2.P2 && u1.XX == u2.XX)
                        duplicatefound = true;

                if (!duplicatefound)
                    TempList.Add(u1);
            }
            TempList1 = TempList.OrderByDescending(x => x.P1).OrderByDescending(x => x.P2).OrderBy(z => z.XX).ToList();
            foreach (BuyPattern u1 in TempList1)
            {
                bool duplicatefound = false;
                foreach (BuyPattern u2 in TempList2)
                   if (u1.P1 == u2.P1 && u1.P2 == u2.P2 && u1.XX == u2.XX)
                    // if (u1.P1 == u2.P1 && u1.P2 == u2.P2)
                    // if (u1.XX == u2.XX)
                        duplicatefound = true;

                if (!duplicatefound)
                    TempList2.Add(u1);
            }
            return TempList2;
            #endregion
        }
        private static bool conditions(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, double crossing, double tradeopen, List<Stock> stocks, List<Stocks5min> stocks5min, int year)
        {
            if (
            // x.Close < LowestPointByIndex(p1_index, x_index, stocks) + (HighestPointByIndex(p1_index, x_index, stocks) - LowestPointByIndex(p1_index, x_index, stocks)) * 0.25 &&
            // HighestPointByIndex(p2_index, x_index, stocks) > p1.Low &&// HighestPointByIndex(p1_index, p2_index, stocks) &&
            // crossing > LowestPoint(p1, x, x_index, x_index, stocks) + (HighestPoint(p1, x, x_index, x_index, stocks) - LowestPoint(p1, x, x_index, x_index, stocks)) * 0.25 &&
            // (HighestPoint(p1, p2, x_index, x_index, stocks) - p2.Low) / (HighestPoint(p2, x, x_index, x_index, stocks)  - p2.Low) > 0.618 &&
            // (HighestPoint(p2, x, x_index, x_index, stocks) -  p2.Low) / (HighestPoint(p1, p2, x_index, x_index, stocks) - p2.Low) > 0.382 &&
            // x_index - p2_index < (p2_index - p1_index) * 1 &&
            // (HighestPoint(p2, x, x_index, x_index, stocks) -  p2.Low) / (HighestPoint(p1, p2, x_index, x_index, stocks) - p2.Low) < 0.618 &&
            // HighestPoint(p2, x, x_index, x_index, stocks) < HighestPoint(p1, p2, x_index, x_index, stocks) &&
            // (HighestPoint(p2, x, x_index, x_index, stocks) - tradeopen) * 1 / ((tradeopen - x.Low) - 0.0005) < 5 &&
            // (HighestPoint(p2, x, x_index, x_index, stocks) - tradeopen) * 0.25 / tradeopen >= 0.001 &&
            // ((tradeopen - x.Low) - 0.0005) / tradeopen <= 0.0005 &&
            // p1.High > p2.High &&
            // Math.Abs(p1.Low - p2.Low) / p2.Low > 0.003 &&
            // p1.Speed > 0.50 &&
            // p2.Speed > 0.50 &&
            // x.Speed  > 0.50 &&
            // x.Speed > (p1.Speed + p2.Speed) * 4 &&
            // StDev(p1, x, p1_index, x_index, stocks) == true &&
            // x_index - HighestPoint_index(stocks[((x_index - p1_index) / 2) + p1_index], x, p1_index, x_index, stocks) > 3 &&
            // x_index - HighestPoint_index(stocks[((x_index - p1_index) / 2) + p1_index], x, p1_index, x_index, stocks) > 3 &&
            // quadMacdThis(p1, x, p1_index, x_index, crossing, 0, stocks) > 0 &&
            // quadMacdThis(p1, x, p1_index, x_index, crossing, 1, stocks) < quad(p1, x, p1_index, x_index, crossing, 1, stocks) &&
            // Supports(p1, x, p1_index, x_index, stocks) == true &&
            // findSupportCntr(p1, p2, x, p1_index, p2_index, x_index, stocks) > 0 &&
            // Support(p1, x, p1_index, p2_index, x_index, stocks) >= 1 &&
            // slope
            // (p2_index - p1_index) / (p2.Mid - p1.Mid) < (x_index - p2_index) / (x.Mid - p2.Mid) &&
            // (p2.Mid - p1.Mid) / (p2_index - p1_index) < (x.Mid - p2.Mid) / (x_index - p2_index) &&
            // CndlPassCounter(p1, p2, p1_index, p2_index, stocks) > 0.50 &&
            // CndlPassCounter(p2, x, p2_index, x_index, stocks) > 0.50 &&
            // Quad(p1, p2, p1_index, p2_index, 1, stocks) == 1 &&
            // Quad(p2, x, p2_index, x_index, 1, stocks) == 1 &&
            // QuadAccu(p1, x, p1_index, x_index, 1, stocks) == 1 &&
            Quad(p1, x, p1_index, x_index, 1, stocks) == 1 &&

            // CndlPassCounter(p1, x, p1_index, x_index, stocks) >= 0.80 &&
            // Quad(p1, x, p1_index, x_index, -1, stocks) == 1 &&
            // Quad(p1, x, p1_index, x_index, 1, stocks) == 1 &&
            // x_index - p2_index > (p2_index - p1_index) * 0.70 &&
            // p2_index - p1_index > (x_index - p2_index) * 0.70 &&
            // (HighestPoint(p1, p2, x_index, x_index, stocks) - p1.Mid) / (HighestPoint(p1, p2, x_index, x_index, stocks) - p2.Mid) > 0.50 && // ok
            // // (HighestPoint(p1, p2, x_index, x_index, stocks) - p2.Mid) / (HighestPoint(p1, p2, x_index, x_index, stocks) - p1.Mid) > 0.50 &&
            // (HighestPoint(p2, x, x_index, x_index, stocks) - p2.Mid) / (HighestPoint(p2, x, x_index, x_index, stocks) - x.Mid) > 0.50 && // ok
            // // (HighestPoint(p2, x, x_index, x_index, stocks) - x.Mid) / (HighestPoint(p2, x, x_index, x_index, stocks) - p2.Mid) > 0.50 &&
            // regLine(p1, x, p1_index, x_index, crossing, stocks) > 0 &&
            // accumulation(p1, p2, x, p1_index, p2_index, x_index, stocks) == true &&
            // sidewayAccumulation(p1, p2, x, p1_index, p2_index, x_index, stocks) == true &&
            // Quad(p1, p2, x, p1_index, p2_index, x_index, crossing, 1, stocks) > 0 &&
            // DynSupport(p1, x, p1_index, x_index, stocks, year) == true &&
            // Macd(p1, x, p1, x, p1_index, x_index, crossing, stocks) == true &&
            // Curvature(p1, p2, x, p1_index, p2_index, x_index, crossing, stocks) == true &&
            // quad5min(p1.Date, x.Date, 1, stocks5min) > 0 &&
            // quadMacdThis5min(p1.Date, x.Date, 1, stocks5min) > 0 &&
            // findSupports(stocks[(((x_index - p1_index) / 5) * 3) + p1_index], x, p2_index, x_index, crossing, stocks) == true &&
            // findSupportss(stocks[x_index - 3], stocks[x_index + 3], p2_index, x_index, crossing, stocks5min) == true &&
            x_index - p1_index < 200
            )
                return true;
            else return false;
        }
        private static double CndlPassCounter(Stock from, Stock to, int from_index, int to_index, List<Stock> stocks)
        {
            int cntr = 0;
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < from.Date)
                    continue;
                if (q.Date > to.Date)
                    break;
                if (q.High > (from.Mid - ((from.Mid - to.Mid) / (to_index - from_index) * (index - from_index)))
                // && q.High > (from.Mid - ((from.Mid - to.Mid) / (to_index - from_index) * (index - from_index)))
                )
                {
                    cntr = cntr + 1;
                }
            }
            return cntr / (to_index - from_index + 1);
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
        private static bool findOutliersBeforeP2(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, double Crossing, List<Stock> stocks)
        {
            double highestBeforeP2 = 0; int beforeP2 = 0;
            double highestpoint = HighestPoint(p1, x, p1_index, x_index, stocks);
            foreach (Stock before in Enumerable.Reverse(stocks))
            {
                double crossOutlier = (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index));
                if (before.Date <= p2.Date)
                {
                    if (before.High > highestBeforeP2)
                        highestBeforeP2 = before.High;
                    if (before.Low > (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index))
                    && beforeP2 == 0
                    )
                        beforeP2 = 1;
                    if (before.Close < (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index))
                    && beforeP2 == 0
                    )
                        return false;
                    if (before.Low <= (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index))
                    // && highestBeforeP2 < crossOutlier + (highestpoint - crossOutlier) * 0.50
                    && beforeP2 == 1
                    )
                        return true;
                }
            }
            return false;
        }
        private static bool findOutliersAfterP2(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, double Crossing, List<Stock> stocks)
        {
            double highestAfterP2 = 0; int afterP2 = 0;
            double highestpoint = HighestPoint(p1, x, p1_index, x_index, stocks);
            foreach (Stock after in stocks)
            {
                double crossOutlier = (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(after) - p1_index));
                if (after.Date >= p2.Date)
                {
                    if (after.High > highestAfterP2)
                        highestAfterP2 = after.High;
                    if (after.Low > (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(after) - p1_index))
                    && afterP2 == 0
                    )
                        afterP2 = 1;
                    if (after.Close < (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(after) - p1_index))
                    && afterP2 == 0
                    )
                        return false;
                    if (after.Low <= (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(after) - p1_index))
                    // && highestAfterP2 < crossOutlier + (highestpoint - crossOutlier) * 0.50
                    && afterP2 == 1
                    )
                        return true;
                }
            }
            return false;
        }
        private static bool findOutliersX(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, double Crossing, List<Stock> stocks)
        {
            double highestBeforeX = 0; int beforeX = 0;
            double highestpoint = HighestPoint(p1, x, p1_index, x_index, stocks);
            DateTime HighestDateTime = new DateTime();
            foreach (Stock before in Enumerable.Reverse(stocks))
            {
                double crossOutlier = (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index));
                if (before.Date <= x.Date)
                {
                    if (before.High > highestBeforeX)
                    {
                        highestBeforeX = before.Close;
                        HighestBefX = before.Close;
                        HighestBeFXindex = stocks.IndexOf(before);
                        HighestDateTime = before.Date; // date of highest
                        BeforeXTopDateTime = HighestDateTime;
                    }
                    if (before.Low > (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index))
                    && beforeX == 0
                    )
                        beforeX = 1;
                    if (before.Close < (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index))
                    && beforeX == 0
                    )
                        return false;
                    if (before.Low <= (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index))
                    // && highestBeforeX < crossOutlier + (highestpoint - crossOutlier) * 0.382
                    && beforeX == 1
                    )
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private static bool findOutliersImp(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, double Crossing, List<Stock> stocks)
        {
            #region vars
            // Stock LastIndex;
            bool quading = false;
            double bo1 = 0; double bo2 = 0;
            double totalBelow = 0;
            double totalAbove = 0;
            double abovePer = 0;
            double belowPer = 0;
            double BtmsCntr = 0;
            double BtmsCntrX = 0;
            double highestBeforeX = 0;
            double BtmsCntrAfter = 0;
            double empty1Per = 0; double empty2Per = 0;
            double empty1 = 0; double empty2 = 0;
            double fill = 0;
            int xing = 0;
            int Lows = 0;
            int Rindexes = 0;
            int Lindexes = 0;
            int lastIndex = 0;
            int checkP2 = 0;
            int afterMid = 0; int beforeMid = 0;
            int mid = ((x_index - p1_index) / 2) + p1_index;
            int quart_75 = (((x_index - p1_index) / 4) * 3) + p1_index;
            int quart_80 = (((x_index - p1_index) / 5) * 4) + p1_index;
            double highestpoint = HighestPoint(p1, x, p1_index, x_index, stocks);
            #endregion
            List<BuyPattern> patternsWithOutliers = new List<BuyPattern>();
            // Find outliers
            foreach (Stock outlier in stocks)
            {
                int index = stocks.IndexOf(outlier);
                double crossOutlier = (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index));
                int countedAsBottom = 0;
                if (outlier.Date > p1.Date && outlier.Date < x.Date) // all
                {
                    #region Size Above and Below
                    if (outlier.Low <= (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index))
                    && outlier.High >= (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)))
                    {
                        if (outlier.High - (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) >
                         (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) - outlier.Low)
                            totalAbove = totalAbove + 1;
                        else
                            totalBelow = totalBelow + 1;
                        // totalAbove = totalAbove + (outlier.High - (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)));
                        // totalBelow = totalBelow + ((p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) - outlier.Low);
                    }
                    else if (outlier.Low > (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index))
                    // && outlier.High > (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index))
                    )
                    {
                        // empty1 = empty1 + (outlier.Low - (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)));
                        // // totalAbove = totalAbove + (outlier.Low - (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)));
                        // totalAbove = totalAbove + (outlier.High - outlier.Low);
                        // // totalAbove = totalAbove + (outlier.High - (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)));
                        totalAbove = totalAbove + 1;
                    }
                    else if (outlier.High < (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index))
                    // && outlier.Low < (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index))
                    )
                    {
                        // empty2 = empty2 + ((p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) - outlier.High);
                        // // totalBelow = totalBelow + ((p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) - outlier.High);
                        // totalBelow = totalBelow + (outlier.High - outlier.Low);
                        // // totalBelow = totalBelow + ((p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) - outlier.Low);
                        totalBelow = totalBelow + 1;
                    }
                    // if (outlier.Low < (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)))
                    // {
                    //     // empty = empty + (outlier.Low - (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)));
                    //     fill = fill + (outlier.High - outlier.Low);
                    // }
                    #endregion

                    if (outlier.Type == "b")
                    {
                        // Check if close below the 13 trend
                        if (
                        outlier.Low < (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index))
                        && outlier.Close > (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index))
                        // // tale < 20%
                        // // ((p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) - outlier.Low) / (outlier.High - outlier.Low) < 0.20
                        // && ((p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(outlier) - p1_index)) - outlier.Low) <= 0.0010
                        )
                        {         
                            double highestAfterOutlier = 0;
                            int AfterOutlier = 0;
                            double BtmsCntrA05 = 0;  
                            foreach (Stock after in stocks)
                            {
                                int a_index = stocks.IndexOf(after);
                                double crossing = (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(after) - p1_index));
                                if (after.Date > outlier.Date && after.Date <= x.Date)
                                {
                                    if (after.High > highestAfterOutlier)
                                        highestAfterOutlier = after.High;
                                    if (after.Low > crossing
                                    && AfterOutlier == 0
                                    )
                                        AfterOutlier = 1;
                                    if (after.Close < crossing
                                    && AfterOutlier == 0
                                    )
                                        break;
                                    if (after.Low <= crossing
                                    // && highestAfterOutlier > crossOutlier + (highestpoint - crossOutlier) * 0.618
                                    && AfterOutlier == 1
                                    )
                                        BtmsCntrA05 = 0.50;
                                }
                            }
                            double highestBeforeOutlier = 0;
                            int beforeOutlier = 0;
                            int beforeX = 0;
                            double BtmsCntrB05 = 0;
                            foreach (Stock before in Enumerable.Reverse(stocks))
                            {
                                int b_index = stocks.IndexOf(before);
                                double crossing = (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks.IndexOf(before) - p1_index));
                                if (before.Date < outlier.Date && before.Date >= p1.Date)
                                {
                                    if (before.High > highestBeforeOutlier)
                                        highestBeforeOutlier = before.High;
                                    if (before.Low > crossing
                                    && beforeOutlier == 0
                                    )
                                        beforeOutlier = 1;
                                    if (before.Close < crossing
                                    && beforeOutlier == 0
                                    )
                                        break;
                                    if (before.Low <= crossing
                                    // && highestBeforeOutlier > crossOutlier + (highestpoint - crossOutlier) * 0.618
                                    && beforeOutlier == 1
                                    )
                                        BtmsCntrB05 = 0.50;
                                }
                            }
                            if (BtmsCntrA05 + BtmsCntrB05 == 1)
                                BtmsCntr = BtmsCntr + 1;
                        }
                    }
                }
            }
            abovePer = totalAbove / (totalAbove + totalBelow);
            empty1Per = empty1 / (empty1 + totalAbove);
            belowPer = totalBelow / (totalAbove + totalBelow);
            empty2Per = empty2 / (empty2 + totalBelow);
            if (
            BtmsCntr >= 2
            && abovePer > 0.95
            // && empty1Per > 0.60


            // && abovePer > 0.80
            // && abovePer > 0.382
            // && bo1 < bo2 * 0.20
            

            // && quading == true
            // && BtmsCntrAfter >= 2
            // && BtmsCntrAfter >= 2//(x_index - p1_index) * 0.50
            // && 
            // BtmsCntrAfter < BtmsCntr * 1
            // BtmsCntrAfter / (BtmsCntrAfter + BtmsCntr) > 0.50
            // && empty1Per > 0.50
            // && IndexToX(p1, p2, x, p1_index, p2_index, x_index, lastIndex, stocks) == true
            // && BtmsCntrAfter >= 1
            // && afterP2 >= 1
            // && BtmsCntr <= 5
            // && empty1Per < 0.70
            // && empty1Per > 0.70
            // && belowPer < empty2Per
            // && abovePer  > belowPer
            // && empty1Per > empty2Per
            )
                return true;
            else
                return false;
        }
        public static double Support(Stock p1, Stock x, int p1_index, int p2_index, int x_index, List<Stock> stocks)
        {
            int supCntr = 0; int cntr = 0;
            int mid = p1_index + (x_index - p1_index) / 2;
            // int lowestIndx = LowestPoint_index(p1, stocks[x_index - 1], stocks);
            foreach (Stock outlier in stocks)
            {
                int index = stocks.IndexOf(outlier);
                if (index < p1_index - (x_index - p1_index) * 4)
                    continue;
                // if (index > mid)
                //     break;
                if (index > x_index) // p1_index for supports before the pattern
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
        private static double findSupportCntr(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, List<Stock> stocks)
        {
            // DateTime p1d = new DateTime(2023, 6, 29, 0, 0, 0);
            // DateTime p2d = new DateTime(2023, 7, 28, 0, 0, 0);
            // DateTime xd = new DateTime(2023, 12, 11, 1, 0, 0);
            int supCntr = 0; int cntr = 0;
            foreach (Stock outlier in stocks)
            {
                int index = stocks.IndexOf(outlier);
                if (outlier.Date > p1.Date && outlier.Date < x.Date)
                // if (outlier.Date < x.Date && x_index - index < 15)
                // if (outlier.Date < x.Date && (double)(x_index - index) / (double)(x_index - p1_index) < 0.50)
                // // && (double)(x_index - index) / (double)(x_index - p1_index) < 1)
                {
                    cntr = cntr + 1;
                    if (
                    outlier.Type == "b" && 
                    x.Low < outlier.Low && x.Close > outlier.Low // support
                    && supportHelper(outlier, p1, x, x_index, stocks) == true
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
            // if ((double)supCntr / (double)cntr > 0.3) // && supCntr != 0)
            return supCntr;
        }
        private static bool supportHelper(Stock outlier, Stock p1, Stock x, int x_index, List<Stock> stocks5min)
        {
            int beforeX = 0;
            foreach (Stock before in Enumerable.Reverse(stocks5min))
            {
                if (before.Date < p1.Date)
                    break;
                if (before.Date <= x.Date)
                {
                    if (before.Low > outlier.Low
                    && beforeX == 0
                    )
                    {
                        // Console.WriteLine(outlier.Date);
                        // beforeX = 1;
                        return true;
                    }
                    if (before.Close < outlier.Low
                    && beforeX == 0
                    )
                        return false;
                    // if (before.Low <= (p1.Low - (p1.Low - p2.Low) / (p2_index - p1_index) * (stocks5min.IndexOf(before) - p1_index))
                    // && beforeX == 1
                    // )
                    // {
                    //     return true;
                    // }
                }
            }
            return false;
        }
        public static double regLine(Stock p0, Stock x, int p0_index, int x_index, double Crossing, List<Stock> stocks)
        {
            double all = 0; double touching = 0;
            List<Candle> candles = new List<Candle>();
            #region Linear Reg by Price
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < p0.Date)
                    continue;
                // if (q.Date < BeforeXTopDateTime)
                //     continue;
                // if (x_index - 20 < index)
                //     continue;
                if (index > x_index)
                    break;
                candles.Add(new Candle { UnixTime = q.Speed, Open = q.Open, High = q.High, Low = q.Low, Close = q.Close });
            }
            #endregion

            #region Macd calculation
            List<MacdPoint> macdPoints = MacdCalculator.CalculateMacd(candles, shortPeriod: 12, longPeriod: 26);
            
            var macdRegressionLine = MacdRegressionCalculator.ComputeLinearRegression(macdPoints);
            #endregion

            var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Average);
            foreach (Stock q in stocks)
            {
                int index = stocks.IndexOf(q);
                if (q.Date < p0.Date)
                    continue;
                // if (q.Date < BeforeXTopDateTime)
                //     continue;
                // if (x_index - 20 < index)
                //     continue;
                if (index > x_index)
                    break;
                all = all + 1;
                double lineValueAtTime = regressionLineAvg.GetLineValueAtTime(q.Speed);
                if (q.High > lineValueAtTime && q.Low < lineValueAtTime)
                    touching = touching + 1;
            }
            if (
            // regressionLineAvg.Slope < 0 && macdRegressionLine.Slope > 0 //&& touching / all < 0.10
            // macdRegressionLine.Slope - regressionLineAvg.Slope > 0
            // if (regressionLineAvg.Slope < 0 && macdRegressionLine.Slope < 0 //&& touching / all < 0.10
            macdRegressionLine.Slope > regressionLineAvg.Slope
            // macdRegressionLine.Slope > 0
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
            double belowPer = 0;  double lowAg= 0;
            double highest = 0; double lowest = 10000;
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
                if (q.Date < p0.Date)
                    continue;
                if (q.Date > x.Date)
                    break;
                // if (q.Accupoint_2 == 1)
                // {
                    xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                //     // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                //     // yDataList.Add((q.High - q.Low));
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
                        {
                            touching = touching + 1;
                            if (q.Low <= lowest)
                                lowest = q.Low;
                            if (q.High >= highest)
                                highest = q.High;
                        }
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
                    touching / all >= 0.80 && // >= 0.70
                    // touching / all <= 0.80 &&
                    x.Close < lowest + (highest - lowest) * 0.25 && // 0.20
                    // above > below
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // 1 == 1
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed))
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && (bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))) >
                    // (bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed))) * 3
                    // && lowestCurv_index != x_index
                    // lowestCurv_index == x_index

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
                    && lowestCurv_index == x_index
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
        public static double QuadAccu(Stock p0, Stock x, int p0_index, int x_index, double concav, List<Stock> stocks)
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
                if (q.Date < p0.Date)
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
                    touching / all >= 0.70 && // 0.70
                    // above > below
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // 1 == 1
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_25].Speed))
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && (bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))) >
                    // (bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)lowestCurv_index].Speed))) * 3
                    // && lowestCurv_index != x_index
                    // lowestCurv_index == x_index

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
                    // && lowestCurv_index == x_index
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
            double highest = HighestPoint(p0, x, x_index, x_index, stocks);
            double lowest = LowestPoint(p0, x, x_index, x_index, stocks);
            double lowestCurv = 10000; int lowestCurv_index = 0;
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_80 = (((x_index - p0_index) / 5) * 4) + p0_index;
            double highest1 = HighestPoint(p0, stocks[(int)mid], x_index, x_index, stocks); double lowest1 = LowestPoint(stocks[(int)mid], x, x_index, x_index, stocks);
            double highest2 = HighestPoint(p0, stocks[(int)mid], x_index, x_index, stocks); double lowest2 = LowestPoint(stocks[(int)mid], x, x_index, x_index, stocks);
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
                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
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
        private static bool sidewayAccumulation(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, List<Stock> stocks)
        {
            int vrible = 4;
            double UpperWings = 0; double LowerWings = 0; double accu = 0;
            double highestLow = HighestLowByIndex(x_index - vrible, x_index, stocks);
            double lowestHigh = LowestHighByIndex(x_index - vrible, x_index, stocks);
            double highestOpenClose = HighestOpenCloseByIndex(x_index - vrible, x_index, stocks);
            double lowestOpenClose = LowestOpenCloseByIndex(x_index - vrible, x_index, stocks);
            double totalEmpty = 0;
            foreach (Stock outlier in stocks)
            {
                int index = stocks.IndexOf(outlier);
                if (outlier.Date < x.Date && x_index - index < vrible)
                {
                    UpperWings = UpperWings + (outlier.High - lowestHigh);
                    LowerWings = LowerWings + (highestLow - outlier.Low);
                    accu = accu + (lowestHigh - highestLow);
                    
                    // if (outlier.High >= highestOpenClose && outlier.Low <= lowestOpenClose)
                    //     accu = accu + (highestOpenClose - lowestOpenClose);
                    // else if (outlier.High <= highestOpenClose && outlier.Low >= lowestOpenClose)
                    //     accu = accu + (outlier.High - outlier.Low);
                    // else if (outlier.High >= highestOpenClose && outlier.Low >= lowestOpenClose)
                    //     accu = accu + (highestOpenClose - outlier.Low);
                    // else if (outlier.High <= highestOpenClose && outlier.Low <= lowestOpenClose)
                    //     accu = accu + (outlier.High - lowestOpenClose);
                    // if (outlier.High < highestOpenClose)
                    //     totalEmpty = totalEmpty + (highestOpenClose - outlier.High);
                    // if (outlier.Low > lowestOpenClose)
                    //     totalEmpty = totalEmpty + (outlier.Low - lowestOpenClose);
                }
            }
            if (
            // accu > LowerWings && UpperWings > LowerWings &&
            // UpperWings > LowerWings * 1 &&
            accu / (accu + UpperWings + LowerWings) > 0.30 // 80 and only this line of code
            // accu / (accu + totalEmpty) > 0.80
            )
                return true;
            else
                return false;
        }

        private static bool accumulation(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, List<Stock> stocks)
        {
            int vrible = 5;
            double UpperEmpty = 0; double LowerEmpty = 0; double accu = 0;
            double highest = HighestPointByIndex(x_index - vrible, x_index, stocks);
            double lowest = LowestPointByIndex(x_index - vrible, x_index, stocks);
            foreach (Stock outlier in stocks)
            {
                int index = stocks.IndexOf(outlier);
                // if (outlier.Date > p1.Date && outlier.Date < x.Date)
                if (outlier.Date < x.Date && x_index - index < vrible)
                // if (outlier.Date < x.Date && (double)(x_index - index) / (double)(x_index - p1_index) < 0.50)
                // // && (double)(x_index - index) / (double)(x_index - p1_index) < 1)
                {
                    UpperEmpty = UpperEmpty + (highest - outlier.High);
                    LowerEmpty = LowerEmpty + (outlier.Low - lowest);
                    accu = accu + (outlier.High - outlier.Low);
                }
            }
            if (
            accu > LowerEmpty && UpperEmpty > LowerEmpty && 
            accu / (accu + UpperEmpty + LowerEmpty) > 0.30)
                return true;
            else
                return false;
        }
        private static bool findSupports(Stock p1, Stock x, int p1_index, int x_index, double crossing, List<Stock> stocks)
        {
            double above = 0; double below = 0; double abovePer = 0;
            int trigger = 0; // trigger start finding a support
            double BtmsCntr = 0; double highestpoint = HighestPoint(p1, x, p1_index, x_index, stocks);
            // Find outliers
            foreach (Stock outlier in stocks)
            {
                int index = stocks.IndexOf(outlier);
                if (outlier.Date > p1.Date && outlier.Date < x.Date)
                {
                    if (outlier.Low <= crossing && outlier.Close >= crossing && trigger == 0)
                        trigger = 1;
                    if (trigger == 1)
                    {
                        if (outlier.Close >= crossing)
                            above = above + 1;
                        else if (outlier.Close < crossing)
                            below = below + 1;
                    }
                }
                if (outlier.Date > p1.Date && outlier.Date < x.Date) // or only before X
                {
                    if (outlier.Low <= crossing && outlier.Close >= crossing
                    && outlier.Type == "b"
                    )
                    {
                        double highestAfterOutlier = 0;
                        int AfterOutlier = 0;
                        double BtmsCntrA05 = 0;  
                        foreach (Stock after in stocks)
                        {
                            int a_index = stocks.IndexOf(after);
                            if (after.Date > outlier.Date && after.Date <= x.Date)
                            {
                                if (after.High > highestAfterOutlier)
                                    highestAfterOutlier = after.High;
                                if (after.Low > crossing
                                && AfterOutlier == 0
                                )
                                    AfterOutlier = 1;
                                if (after.Close < crossing
                                && AfterOutlier == 0
                                )
                                    break;
                                if (after.Low <= crossing
                                // && highestAfterOutlier > crossing + (highestpoint - crossing) * 0.50
                                && AfterOutlier == 1
                                )
                                    BtmsCntrA05 = 0.50;
                            }
                        }
                        double highestBeforeOutlier = 0;
                        int beforeOutlier = 0;
                        double BtmsCntrB05 = 0;
                        foreach (Stock before in Enumerable.Reverse(stocks))
                        {
                            int b_index = stocks.IndexOf(before);
                            if (before.Date < outlier.Date && before.Date >= p1.Date)
                            {
                                if (before.High > highestBeforeOutlier)
                                    highestBeforeOutlier = before.High;
                                if (before.Low > crossing
                                && beforeOutlier == 0
                                )
                                    beforeOutlier = 1;
                                if (before.Close < crossing
                                && beforeOutlier == 0
                                )
                                    break;
                                if (before.Low <= crossing
                                // && highestBeforeOutlier > crossing + (highestpoint - crossing) * 0.50
                                && beforeOutlier == 1
                                )
                                    BtmsCntrB05 = 0.50;
                            }
                        }
                        if (BtmsCntrA05 + BtmsCntrB05 == 1)
                            BtmsCntr = BtmsCntr + 1;
                    }
                }
            }
            abovePer = above / (above + below);
            if (BtmsCntr >= 2
            //&& abovePer > 0.50
            )
                return true;
            else
                return false;
        }
        private static List<BuyPattern> findMultipleTargets(List<Stock> stocks, List<BuyPattern> patterns, List<Stocks5min> targetstocks)
        {
            List<BuyPattern> patternsWithTargets = new List<BuyPattern>();
            // Find target
            foreach (BuyPattern pattern in patterns)
            {
                #region Buy with BreakEven
                int cntr = 50;  // 5 daily // while applied conditions, make this number of orders for every candle
                int cVar = 1; // 12 4Hours // cancel order after this number of candles if not executed
                // int firstIndex = 0;
                foreach (Stock stk in stocks)
                {
                    int Frame4HoursCntr = 6;
                    int buyIndex = 0; // opening index
                    int closeIndex = 0; // closing index
                    int catching = 1;
                    int bought = 0;
                    int breakEven = 1;
                    int counter = 0;
                    int checkLossFirst = 0; // 0 check loss first // 1 normal
                    if (stk.Date > pattern.XX)
                    {
                        if (cntr > 0)
                        {
                            foreach (Stocks5min target in targetstocks)
                            {
                                // if (Frame4HoursCntr > 0)
                                // {
                                //     Frame4HoursCntr = Frame4HoursCntr - 1;
                                //     continue;
                                // }
                                if (target.Date >= stk.Date) // start find target from crossing date // same time frame // Yes >= --> sure
                                // if (DateTime.Parse(target.Date.ToShortDateString()) >= stk.Date)  // different time frame // Yes >= --> sure
                                {
                                    // if (target.Date.Day == stk.Date.Day) // (DAY, HOUR....)
                                    //     continue;
                                    // if (target.Date.Hour == stk.Date.Hour) // (HOUR, MIN....)
                                    //     continue;

                                    // if (target.Date == pattern.X)
                                    //     buyIndex = targetstocks.IndexOf(target);
                                    if (target.Close > pattern.Tradeopen && catching == 0)// && target.Date.Day > pattern.X.Day)
                                    {
                                        catching = 1;
                                        continue;
                                    }
                                    // else if (target.Close < pattern.Cross && catching == 1)
                                    // {
                                    //     break;
                                    // }
                                    if (catching == 1)
                                    {
                                        // if (target.High >= Math.Round(pattern.Cncl, 5) && bought == 1)
                                        // {
                                        //     break;
                                        // }
                                        if (target.Low < pattern.Tradeopen && bought == 0 && counter <= cVar) // last part this (1)
                                        {
                                            // Console.WriteLine(target.Low);
                                            bought = 1;
                                            buyIndex = targetstocks.IndexOf(target);
                                            // continue;
                                        }
                                        else if (counter > cVar && bought != 1) // this (2)
                                        {
                                            // counter = 1;
                                            break;
                                        }
                                        counter = counter + 1;
                                        // if (counter < 4)
                                        //     continue;
                                        if (bought == 1)
                                        {
                                            if (target.Low <= Math.Round(pattern.Stoploss, 5) && breakEven == 1) // failed patterns (0)
                                            {
                                                counter = 0;
                                                closeIndex = targetstocks.IndexOf(target); // closing index
                                                patternsWithTargets.Add(new BuyPattern(pattern.P1, pattern.P2, pattern.XX, pattern.Target, pattern.Stoploss, 0, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                                                break;
                                            }
                                            #region BreakEven
                                            // // Time Based BreakEven
                                            // else if (targetstocks.IndexOf(target) - buyIndex > 1 && target.High > Math.Round(pattern.Tradeopen, 5))
                                            //     breakEven = 0;
                                            // else if (target.Low <= Math.Round(pattern.Tradeopen, 5) && breakEven == 0) // failed patterns (0)
                                            // {
                                            //     break;
                                            // }
                                            // // Target Based BreakEven
                                            // else if (target.High >= Math.Round(pattern.Cancel, 5) && breakEven == 1) // successful patterns (1)
                                            // {
                                            //     breakEven = 0;
                                            // }
                                            // else if (target.Low <= Math.Round(pattern.Tradeopen, 5) && breakEven == 0) // failed patterns (0)
                                            // {
                                            //     break;
                                            // }
                                            #endregion
                                            else if (target.High >= Math.Round(pattern.Target, 5) && checkLossFirst == 1) // successful patterns (1)
                                            {
                                                counter = 0;
                                                closeIndex = targetstocks.IndexOf(target); // closing index
                                                patternsWithTargets.Add(new BuyPattern(pattern.P1, pattern.P2, pattern.XX, pattern.Target, pattern.Stoploss, 1, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                                                break;
                                            }
                                            checkLossFirst = 1;
                                        }
                                    }
                                    else
                                        continue;
                                }
                            }
                        }
                        cntr = cntr - 1;
                        if (stk.Close < pattern.Tradeopen || stk.Low > pattern.Tradeopen || stk.High > pattern.Cancel)
                        // if (stk.Close < pattern.Tradeopen)// || stk.Low < pattern.Stoploss)
                            break;
                    }
                }
                #endregion
                
                #region Sell with BreakEven
                // int cntr = 50;  // 5 daily
                // int cVar = 7; // 12 4Hours
                // // int firstIndex = 0;
                // foreach (Stock stk in stocks)
                // {
                //     int Frame4HoursCntr = 6;
                //     int sellIndex = 0; // opening index
                //     int closeIndex = 0; // closing index
                //     int catching = 1;
                //     int bought = 0;
                //     int breakEven = 1;
                //     int counter = 0;
                //     int checkLossFirst = 0; // 0 check loss first // 1 normal
                //     if (stk.Date > pattern.XX)
                //     {
                //         if (cntr > 0)
                //         {
                //             foreach (Stocks5min target in targetstocks)
                //             {
                //                 // if (Frame4HoursCntr > 0)
                //                 // {
                //                 //     Frame4HoursCntr = Frame4HoursCntr - 1;
                //                 //     continue;
                //                 // }
                //                 if (target.Date >= stk.Date) // start find target from crossing date // same time frame
                //                 // if (DateTime.Parse(target.Date.ToShortDateString()) >= stk.Date)  // different time frame
                //                 {
                //                     // if (target.Date.Day == stk.Date.Day) // (DAY, HOUR....)
                //                     //     continue;
                //                     // if (target.Date.Hour == stk.Date.Hour) // (HOUR, MIN....)
                //                     //     continue;

                //                     // if (target.Date == pattern.X)
                //                     //     buyIndex = targetstocks.IndexOf(target);
                //                     if (target.Close > pattern.Tradeopen && catching == 0)// && target.Date.Day > pattern.X.Day)
                //                     {
                //                         catching = 1;
                //                         continue;
                //                     }
                //                     // else if (target.Close < pattern.Cross && catching == 1)
                //                     // {
                //                     //     break;
                //                     // }
                //                     if (catching == 1)
                //                     {
                //                         // if (target.High >= Math.Round(pattern.Cncl, 5) && bought == 1)
                //                         // {
                //                         //     break;
                //                         // }
                //                         if (target.High > pattern.Tradeopen && bought == 0 && counter <= cVar) // last part this (1)
                //                         {
                //                             // Console.WriteLine(target.Low);
                //                             bought = 1;
                //                             sellIndex = targetstocks.IndexOf(target);
                //                             // continue;
                //                         }
                //                         else if (counter > cVar && bought != 1) // this (2)
                //                         {
                //                             // counter = 1;
                //                             break;
                //                         }
                //                         counter = counter + 1;
                //                         // if (counter < 4)
                //                         //     continue;
                //                         if (bought == 1)
                //                         {
                //                             if (target.High >= Math.Round(pattern.Stoploss, 5) && breakEven == 1) // failed patterns (0)
                //                             {
                //                                 counter = 0;
                //                                 closeIndex = targetstocks.IndexOf(target); // closing index
                //                                 patternsWithTargets.Add(new BuyPattern(pattern.P1, pattern.P2, pattern.XX, pattern.Target, pattern.Stoploss, 0, 0, pattern.Profit, pattern.Loss, closeIndex - sellIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                //                                 break;
                //                             }
                //                             #region BreakEven
                //                             // else if (target.Low <= Math.Round(pattern.Cancel, 5) && breakEven == 1) // successful patterns (1)
                //                             // {
                //                             //     breakEven = 0;
                //                             // }
                //                             // else if (target.High >= Math.Round(pattern.Tradeopen, 5) && breakEven == 0) // failed patterns (0)
                //                             // {
                //                             //     break;
                //                             // }
                //                             #endregion
                //                             else if (target.Low <= Math.Round(pattern.Target, 5) && checkLossFirst == 1) // successful patterns (1)
                //                             {
                //                                 counter = 0;
                //                                 closeIndex = targetstocks.IndexOf(target); // closing index
                //                                 patternsWithTargets.Add(new BuyPattern(pattern.P1, pattern.P2, pattern.XX, pattern.Target, pattern.Stoploss, 1, 0, pattern.Profit, pattern.Loss, closeIndex - sellIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                //                                 break;
                //                             }
                //                             checkLossFirst = 1;
                //                         }
                //                     }
                //                     else
                //                         continue;
                //                 }
                //             }
                //         }
                //         cntr = cntr - 1;
                //         if (stk.Close > pattern.Tradeopen || stk.High < pattern.Tradeopen)// || stk.Low < pattern.Cancel)
                //             break;
                //     }
                // }
                #endregion
            }
            return patternsWithTargets;
        }
        private static List<BuyPattern> findTargets(List<BuyPattern> patterns, List<Stocks5min> targetstocks)
        {
            List<BuyPattern> patternsWithTargets = new List<BuyPattern>();
            // Find target
            foreach (BuyPattern pattern in patterns)
            {
                #region Buy with Penetration
                int buyIndex = 0; // opening index
                int closeIndex = 0; // closing index
                int catching = 0;
                int bought = 0;
                int breakEven = 1;
                double counter = 1;
                int checkLossFirst = 0;
                int firstIndex = 0;
                string xDate = "";
                foreach (Stocks5min target in targetstocks)
                {
                    if (target.Date >= pattern.XX) // For the same timeframe
                    // if (DateTime.Parse(target.Date.ToShortDateString()) >= DateTime.Parse(pattern.X.ToShortDateString())) // For different frames
                    {
                        // if (firstIndex == 0)
                        //     firstIndex = targetstocks.IndexOf(target);
                        // if (counter == 0)
                        // {
                        //     counter = counter + 1;
                        //     continue;
                        // }
                        if (target.Date == pattern.XX) // For the same frames
                        // if (DateTime.Parse(target.Date.ToShortDateString()) == DateTime.Parse(pattern.X.ToShortDateString())) // For different frames
                        {
                            buyIndex = targetstocks.IndexOf(target);
                            // xDate = target.Date.ToString("MM/dd/yyyy HH:mm");
                        }
                        // if (target.Close > pattern.Tradeopen && catching == 1 && target.Date > pattern.X)
                        // {
                        //     catching = 0;
                        //     continue;
                        // }
                        // else if (target.Close < pattern.Cross && catching == 1)
                        // {
                        //     break;
                        // }
                        if (catching == 0)
                        {
                            // // if (target.High >= Math.Round(pattern.Cancel, 5) && bought == 1 && target.Date > pattern.X)
                            // // {
                            // //     // counter = 0;
                            // //     break;
                            // // }
                            // if (target.Low < pattern.Cross && bought == 1 && target.Date > pattern.X)
                            // // if (target.High >= pattern.Cross && bought == 1 && target.Date > pattern.X)
                            // {
                            //     // if (targetstocks.IndexOf(target) - firstIndex > pattern.Tradeopen)
                            //     //     break;
                            //     bought = 0;
                            //     buyIndex = targetstocks.IndexOf(target);
                            //     continue;
                            // }
                            if (bought == 0)
                            {
                                if (target.Low <= Math.Round(pattern.Stoploss, 5) && target.Date > pattern.XX && breakEven == 1) // failed patterns (0)
                                {
                                    closeIndex = targetstocks.IndexOf(target); // closing index
                                    patternsWithTargets.Add(new BuyPattern(pattern.P1, pattern.P2, pattern.XX, pattern.Target, pattern.Stoploss, 0, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                                    break;
                                }
                                #region BreakEven
                                // else if (target.High >= Math.Round(pattern.Cancel, 5) && target.Date > pattern.X && breakEven == 1) // successful patterns (1)
                                // {
                                //     breakEven = 0;
                                // }
                                // else if (target.Low <= Math.Round(pattern.Cross, 5) && target.Date > pattern.X && breakEven == 0) // failed patterns (0)
                                // {
                                //     break;
                                // }
                                #endregion
                                else if (target.High >= Math.Round(pattern.Target, 5) && target.Date > pattern.XX && checkLossFirst == 1) // successful patterns (1)
                                {
                                    closeIndex = targetstocks.IndexOf(target); // closing index
                                    patternsWithTargets.Add(new BuyPattern(pattern.P1, pattern.P2, pattern.XX, pattern.Target, pattern.Stoploss, 1, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                                    break;
                                }
                                checkLossFirst = 1;
                            }

                        }
                        else
                            continue;
                    }
                }
                #endregion
                #region Buy with Penetration
                // int buyIndex = 0; // opening index
                // int closeIndex = 0; // closing index
                // int bought = 0;
                // foreach (Stocks5min target in targetstocks)
                // {
                //     if (target.Date >= pattern.X) // start find target from crossing date
                //     {
                //         if (target.Date == pattern.X)
                //             buyIndex = targetstocks.IndexOf(target);
                //         // if (target.High >= Math.Round(pattern.Target, 5) && bought == 1 && target.Date > pattern.X)
                //         //     break;
                //         // if (target.Low <= pattern.Cross && bought == 1 && target.Date > pattern.X)
                //         if (target.High > pattern.Cancel && bought == 1 && target.Date > pattern.X)
                //         {
                //             // counter = 0;
                //             bought = 0;
                //             buyIndex = targetstocks.IndexOf(target);
                //             // continue;
                //         }
                //         // {
                //         //     bought = 0;
                //         //     buyIndex = targetstocks.IndexOf(target);
                //         //     continue;
                //         // }
                //         if (bought == 0)
                //         {
                //             if (target.Low <= Math.Round(pattern.Stoploss, 5) && target.Date > pattern.X) // failed patterns (0)
                //             {
                //                 closeIndex = targetstocks.IndexOf(target); // closing index
                //                 patternsWithTargets.Add(new BuyPattern(pattern.P0, pattern.X, pattern.Target, pattern.Stoploss, 0, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                //                 break;
                //             }
                //             else if (target.High >= Math.Round(pattern.Target, 5) && target.Date > pattern.X) // successful patterns (1)
                //             {
                //                 closeIndex = targetstocks.IndexOf(target); // closing index
                //                 patternsWithTargets.Add(new BuyPattern(pattern.P0, pattern.X, pattern.Target, pattern.Stoploss, 1, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                //                 break;
                //             }
                //         }
                //     }
                // }
                #endregion
                #region Sell
                // int buyIndex = 0; // opening index
                // int closeIndex = 0; // closing index
                // int catching = 0;
                // int bought = 0;
                // int breakEven = 1;
                // int checkLossFirst = 0;
                // int firstIndex = 0;
                // foreach (Stocks5min target in targetstocks)
                // {
                //     if (target.Date >= pattern.X) // start find target from crossing date
                //     {
                //         if (firstIndex == 0)
                //             firstIndex = targetstocks.IndexOf(target);
                //         if (target.Date == pattern.X)
                //             buyIndex = targetstocks.IndexOf(target);
                //         if (target.Close > pattern.Tradeopen && catching == 1 && target.Date > pattern.X)
                //         {
                //             catching = 0;
                //             continue;
                //         }
                //         // else if (target.Close < pattern.Cross && catching == 1)
                //         // {
                //         //     break;
                //         // }
                //         if (catching == 0)
                //         {
                //             if (target.High > pattern.Cross && bought == 1 && target.Date > pattern.X)
                //             {
                //                 if (targetstocks.IndexOf(target) - firstIndex > pattern.Tradeopen)
                //                     break;
                //                 bought = 0;
                //                 buyIndex = targetstocks.IndexOf(target);
                //                 continue;
                //             }
                //             // if (target.Low <= Math.Round(pattern.Target, 5) && bought == 1 && target.Date > pattern.X)
                //             //     break;
                //             if (bought == 0)
                //             {
                //                 if (target.High >= Math.Round(pattern.Stoploss, 5) && target.Date > pattern.X && breakEven == 1) // failed patterns (0)
                //                 {
                //                     closeIndex = targetstocks.IndexOf(target); // closing index
                //                     patternsWithTargets.Add(new BuyPattern(pattern.P0, pattern.P1, pattern.X, pattern.Target, pattern.Stoploss, 0, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                //                     break;
                //                 }
                //                 #region BreakEven
                //                 // else if (target.Low <= Math.Round(pattern.Cancel, 5) && target.Date > pattern.X && breakEven == 1) // successful patterns (1)
                //                 // {
                //                 //     breakEven = 0;
                //                 // }
                //                 // else if (target.High >= Math.Round(pattern.Cross, 5) && target.Date > pattern.X && breakEven == 0) // failed patterns (0)
                //                 // {
                //                 //     break;
                //                 // }
                //                 #endregion
                //                 else if (target.Low <= Math.Round(pattern.Target, 5) && target.Date > pattern.X && checkLossFirst == 1) // successful patterns (1)
                //                 {
                //                     closeIndex = targetstocks.IndexOf(target); // closing index
                //                     patternsWithTargets.Add(new BuyPattern(pattern.P0, pattern.P1, pattern.X, pattern.Target, pattern.Stoploss, 1, 0, pattern.Profit, pattern.Loss, closeIndex - buyIndex, pattern.Cross, pattern.Tradeopen, pattern.Cancel));
                //                     break;
                //                 }
                //                 checkLossFirst = 1;
                //             }

                //         }
                //         else
                //             continue;
                //     }
                // }
                #endregion
            }
            return patternsWithTargets;
        }
        #region Supportive methods
        
        public static double Quad(Stock p0, Stock p2, Stock x, int p0_index, int p2_index, int x_index, double Crossing, double concav, List<Stock> stocks)
        {
            #region Quad
            double aboveMdl = 0; double belowMdl = 0; double vn = 0; 
            double MdlLine = LowestPoint(p0, x, p0_index, x_index, stocks) + (HighestPoint(p0, x, p0_index, x_index, stocks) - LowestPoint(p0, x, p0_index, x_index, stocks)) * 0.50;
            double top25 = LowestPoint(p0, x, p0_index, x_index, stocks) + (HighestPoint(p0, x, p0_index, x_index, stocks) - LowestPoint(p0, x, p0_index, x_index, stocks)) * 0.75;
            double down25 = LowestPoint(p0, x, p0_index, x_index, stocks) + (HighestPoint(p0, x, p0_index, x_index, stocks) - LowestPoint(p0, x, p0_index, x_index, stocks)) * 0.25;
            double all = 0; double touching = 0;
            double all1 = 0; double touching1 = 0; double touching2 = 0;
            double upove = 0; double below = 0;
            double belowPer = 0; 
            double lowestCurv = 1000; int lowestCurv_index = 0;
            double accuCntr = 0; double accuCn = 0;
            List<double> xDataList = new List<double>();
            List<double> yDataList = new List<double>();
            int mid = ((x_index - p0_index) / 2) + p0_index;
            int quart_20 = (((x_index - p0_index) / 5) * 1) + p0_index;
            int quart_80 = (((x_index - p0_index) / 5) * 4) + p0_index;
            int highestPntIndex = HighestPoint_index(p0, x, p0_index, x_index, stocks);
            int lowestPntIndex = LowestPoint_index(p0, x, p0_index, x_index, stocks);
            
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
                // if (q.Date < p0.Date)
                //     continue;
                if (index < highestPntIndex)
                    continue;
                // if (q.Date < BeforeXTopDateTime)
                //     continue;
                // if (index < HighestBeFXindex)
                //     continue;
                if (index > x_index)
                    break;
                // Console.WriteLine(p1.Date + " : " + x.Date);// + " : " + )
                xDataList.Add(Convert.ToDouble(q.Speed)); // First column (unixdatetime)
                // yDataList.Add(q.High - ((q.High - q.Low) * 0.50)); // Second column (_close)
                yDataList.Add(q.Close);
            }
            #endregion
             // Convert lists to arrays
            double[] xData = xDataList.ToArray();
            double[] yData = yDataList.ToArray();

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
                double bic = PolynomialRegression.CalculateBIC(xData, yData, degree, out PolynomialRegression model, linearWeight: 1.0);
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
                        // if (q.Date < p0.Date)
                        //     continue;
                        if (index < highestPntIndex)
                            continue;
                        // if (q.Date < BeforeXTopDateTime)
                        //     continue;
                        // if (index < HighestBeFXindex)
                        //     continue;
                        if (index > x_index)
                            break;
                        all = all + 1;
                        if (bestModel.GetCurveValue(Convert.ToDouble(q.Speed)) < lowestCurv)
                        {
                            lowestCurv = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                            lowestCurv_index = stocks.IndexOf(q);
                        }
                        if (bestModel.GetCurveValue(Convert.ToDouble(q.Speed)) < (p0.Low - ((p0.Low - p2.Low) / (p2_index - p0_index) * (index - p0_index))))
                            vn = vn + 1;
                        if (q.High > MdlLine && q.Low < MdlLine)
                        {
                            aboveMdl = aboveMdl + (q.High - MdlLine);
                            belowMdl = belowMdl + (MdlLine - q.Low);
                        }
                        else if (q.Low > MdlLine)
                            aboveMdl = aboveMdl + (q.High - q.Low);
                        else
                            belowMdl = belowMdl + (q.High - q.Low);
                        double yValue = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                        // double curvature = regression.GetCurvature(Convert.ToDouble(stocks[(int)mid].Speed));
                        // if (q.High > yValue && q.Low < yValue && index < mid)
                        //     touching = touching + 1;
                        // if (q.High > yValue && q.Low < yValue && index > mid)
                        //     touching1 = touching1 + 1;
                        if (q.High > yValue && q.Low < yValue)
                            touching = touching + 1;
                            
                        if (q.Low > yValue)
                            upove = upove + 1;
                        if (q.High < yValue)
                            below = below + 1;
                    }
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        if (index > highestPntIndex && index <= mid)
                        {
                            all1 = all1 + 1;
                            double yValue1 = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                            if (q.High > yValue1 && q.Low < yValue1)
                                touching1 = touching1 + 1;
                        }
                        else if (index > mid && index <= x_index)
                        {
                            double yValue2 = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                            if (q.High > yValue2 && q.Low < yValue2)
                                touching2 = touching2 + 1;
                        }
                    }
                    // check accu points
                    foreach (Stock q in stocks)
                    {
                        int index = stocks.IndexOf(q);
                        // if (q.Date < p0.Date)
                        //     continue;
                        if (index < highestPntIndex)
                            continue;
                        // if (q.Date < BeforeXTopDateTime)
                        //     continue;
                        // if (index < HighestBeFXindex)
                        //     continue;
                        if (index > x_index)
                            break;
                        // Console.WriteLine(q.Accupoint_1);
                        if (q.Accupoint_2 == 1)
                        {
                            accuCn = accuCn + 1;
                            double yValue2 = bestModel.GetCurveValue(Convert.ToDouble(q.Speed));
                            if (q.High > yValue2 && q.Low < yValue2)
                                accuCntr = accuCntr + 1;
                        }
                    }
                    belowPer = below / (below + upove);
                    #endregion
                    if (
                    // aboveMdl < belowMdl * 1
                    // touching / all < touching1 / all * 0.50
                    // && (touching + touching1) / all > 0.50
                    // touching / all >= 0.50
                    // below < upove * 1
                    // 1 == 1
                    // && belowPer > 0.20
                    // && bestModel.GetCurvature(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurvature(Convert.ToDouble(x.Speed))
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > 0
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) > 0
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) >
                    // bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // upove > below * 1
                    // 1 == 1
                    // vn >= 0.50
                    // touching / all >= 0.80 &&
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) // mid || quart_80
                    // // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_80].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))

                    bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed)))
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))) * 1
                    // accuCntr / accuCn > 0.70
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed))
                    // // // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_80].Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // // && x.Low < bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && x.High < bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) < down25
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > top25
                    // && x.Low < stocks[x_index - 1].Low
                    
                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_20].Speed)) > bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)quart_80].Speed))
                    // && bestModel.GetConcavityDegree() > 0.00000000000001
                    // bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed))
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) - bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) >
                    // (bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) - bestModel.GetCurveValue(Convert.ToDouble(x.Speed))) * 2

                    // lowestCurv_index < stocks.IndexOf(stocks[(int)quart_80])

                    // && bestModel.GetCurveValue(Convert.ToDouble(stocks[(int)mid].Speed)) <
                    // LowestPoint(p0, x, x_index, x_index, stocks) + (HighestPoint(p0, x, x_index, x_index, stocks) - LowestPoint(p0, x, x_index, x_index, stocks)) * 0.25
                    // && 
                    // bestModel.GetCurvature(Convert.ToDouble(x.Speed)) < 1.0000000000000000E-14
                    // bestModel.GetConcavityDegree() > 0.0001
                    // if (below > upove * 2)
                    // if (belowPer < 0.30 && belowPer < 0.80)
                    // && bestModel.GetCurveValue(Convert.ToDouble(p0.Speed)) > bestModel.GetCurveValue(Convert.ToDouble(x.Speed)) * 1
                    )
                    {
                        // Console.WriteLine(bestModel.GetConcavityDegree());
                        // Console.WriteLine(p0.Date + " " + x.Date);
                        return 1;//bestModel.GetConcavityDegree();
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
        private static double HighestPoint(Stock from, Stock to, int from_index, int to_index, List<Stock> stocks)
        {
            double highest = 0;
            foreach (Stock c in stocks)
            {
                if (c.Date > from.Date && c.Date <= to.Date)
                {
                    if (c.High > highest)
                        highest = c.High;
                }
            }
            return highest;
        }
        private static double LowestPoint(Stock from, Stock to, int from_index, int to_index, List<Stock> stocks)
        {
            double lowest = 10000;
            foreach (Stock c in stocks)
            {
                if (c.Date > from.Date && c.Date <= to.Date)
                {
                    if (c.Low < lowest)
                        lowest = c.Low;
                }
            }
            return lowest;
        }
        private static double HighestPointByIndex(int from_index, int to_index, List<Stock> stocks)
        {
            double highest = 0;
            foreach (Stock c in stocks)
            {
                int index = stocks.IndexOf(c);
                if (index >= from_index && index <= to_index)
                {
                    if (c.High > highest)
                        highest = c.High;
                }
            }
            return highest;
        }
        private static double LowestPointByIndex(int from_index, int to_index, List<Stock> stocks)
        {
            double lowest = 10000;
            foreach (Stock c in stocks)
            {
                int index = stocks.IndexOf(c);
                if (index >= from_index && index <= to_index)
                {
                    if (c.Low < lowest)
                        lowest = c.Low;
                }
            }
            return lowest;
        }
        private static double HighestLowByIndex(int from_index, int to_index, List<Stock> stocks)
        {
            double highest = 0;
            foreach (Stock c in stocks)
            {
                int index = stocks.IndexOf(c);
                if (index >= from_index && index <= to_index)
                {
                    if (c.Low > highest)
                        highest = c.Low;
                }
            }
            return highest;
        }
        private static double LowestHighByIndex(int from_index, int to_index, List<Stock> stocks)
        {
            double lowest = 10000;
            foreach (Stock c in stocks)
            {
                int index = stocks.IndexOf(c);
                if (index >= from_index && index <= to_index)
                {
                    if (c.High < lowest)
                        lowest = c.High;
                }
            }
            return lowest;
        }
        private static double HighestOpenCloseByIndex(int from_index, int to_index, List<Stock> stocks)
        {
            double highest = 0;
            foreach (Stock c in stocks)
            {
                int index = stocks.IndexOf(c);
                if (index >= from_index && index <= to_index)
                {
                    if (c.Open > highest)
                        highest = c.Open;
                    if (c.Close > highest)
                        highest = c.Close;
                }
            }
            return highest;
        }
        private static double LowestOpenCloseByIndex(int from_index, int to_index, List<Stock> stocks)
        {
            double lowest = 10000;
            foreach (Stock c in stocks)
            {
                int index = stocks.IndexOf(c);
                if (index >= from_index && index <= to_index)
                {
                    if (c.Open < lowest)
                        lowest = c.Open;
                    if (c.Close < lowest)
                        lowest = c.Close;
                }
            }
            return lowest;
        }
        private static double HighestPoint(Stocks5min from, Stocks5min to, int from_index, int to_index, List<Stocks5min> stocks)
        {
            double highest = 0;
            foreach (Stocks5min c in stocks)
            {
                if (c.Date > from.Date && c.Date <= to.Date)
                {
                    if (c.High > highest)
                        highest = c.High;
                }
            }
            return highest;
        }
        private static double LowestPoint(Stocks5min from, Stocks5min to, int from_index, int to_index, List<Stocks5min> stocks)
        {
            double lowest = 10000;
            foreach (Stocks5min c in stocks)
            {
                if (c.Date > from.Date && c.Date <= to.Date)
                {
                    if (c.Low < lowest)
                        lowest = c.Low;
                }
            }
            return lowest;
        }
        private static int HighestPoint_index(Stock from, Stock to, int from_index, int to_index, List<Stock> stocks)
        {
            double highest = 0;
            int index = 0;
            foreach (Stock c in stocks)
            {
                if (c.Date > from.Date && c.Date <= to.Date)
                {
                    if (c.High > highest)
                    {
                        highest = c.High;
                        index = stocks.IndexOf(c);
                    }
                }
            }
            return index;
        }
        private static int LowestPoint_index(Stock from, Stock to, int from_index, int to_index, List<Stock> stocks)
        {
            double lowest = 10000;
            int index = 0;
            foreach (Stock c in stocks)
            {
                if (c.Date > from.Date && c.Date <= to.Date)
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
        #endregion

        private static double Crossing(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index) =>
        // (p1.Low - ((p1.Low - p2.Low) / (p2_index - p1_index) * (x_index - p1_index)));
        x.Close;

        private static Dictionary<string, double> setValues(Stock p1, Stock p2, Stock x, int p1_index, int p2_index, int x_index, double crossing, double tradeopen, List<Stock> stocks)
        {
            // crossing value inside thePattern method
            // market type inside thePattern method
            // set target, stoploss here..
            Dictionary<string, double> values = new Dictionary<string, double>();

            double xclose = tradeopen;
            double penetration = tradeopen;

            #region Buy
            int spd = Convert.ToInt32(x.Speed);
            int mid = ((x_index - p1_index) / 2) + p1_index;
            int quart_80 = (((x_index - p1_index) / 5) * 4) + p1_index;
            
            // double target   = penetration + (HighestPoint(stocks[(int)quart_80], x, x_index, x_index, stocks) - penetration) * 1;
            // double stoploss = penetration - (HighestPoint(stocks[(int)quart_80], x, x_index, x_index, stocks) - penetration) * 0.25;
            // double target = penetration + (HighestPointByIndex(x_index - totalAccuPnt, x_index, stocks) - LowestPointByIndex(x_index - totalAccuPnt, x_index, stocks)) * 3;
            // double stoploss = penetration - (HighestPointByIndex(x_index - totalAccuPnt, x_index, stocks) - LowestPointByIndex(x_index - totalAccuPnt, x_index, stocks)) * 3;
            // double target   = penetration + (HighestBefX - penetration) * 0.50;
            // double stoploss = penetration - (HighestBefX - penetration) * 1;
            // double stoploss = x.Low - 0.0025;
            // double stoploss = penetration - (x.High - x.Low);
            double target   = penetration + (HighestPoint(p1, x, x_index, x_index, stocks) - penetration) * 0.25;
            double stoploss = penetration - (HighestPoint(p1, x, x_index, x_index, stocks) - penetration) * 0.50;
            // double target   = penetration + (HighestPoint(p1, x, x_index, x_index, stocks) - LowestPoint(p1, x, x_index, x_index, stocks)) * 0.25;
            // double stoploss = penetration - (HighestPoint(p1, x, x_index, x_index, stocks) - LowestPoint(p1, x, x_index, x_index, stocks)) * 0.25;
            // double stoploss = support - 0.0020;
            // double target   = penetration * 1.005;
            // double stoploss = LowestPoint(stocks[(int)mid], x, x_index, x_index, stocks) - 0.0005;
            // double target   = HighestPoint(stocks[x_index - 2], x, x_index, x_index, stocks) + 0.0005;
            // double stoploss = penetration - (HighestPoint(p2, x, x_index, x_index, stocks) - penetration) * 1;
            // double stoploss = penetration - (HighestPoint(stocks[x_index - spd], x, x_index, x_index, stocks) - penetration) * 1;
            // double target = x.High + 0.0005;
            // double stoploss = penetration - (HighestPoint(p1, x, x_index, x_index, stocks) - penetration) * 0.25;
            // double target   = penetration + (HighestPoint(p1, x, x_index, x_index, stocks) - penetration) * 0.25;
            // double stoploss = LowestPoint(stocks[x_index - 4], x, x_index, x_index, stocks) - 0.0010;
            // double stoploss = tradeopen - 0.0010; // daily 0.0050
            #endregion
            #region Sell 
            // double target   = penetration - (penetration - LowestPoint(stocks[(int)mid], x, x_index, x_index, stocks)) * 0.50;
            // double stoploss = penetration + (penetration - LowestPoint(stocks[(int)mid], x, x_index, x_index, stocks)) * 0.20;

            // double target   = penetration - (HighestPoint(stocks[(int)mid], x, x_index, x_index, stocks) - penetration) * 0.50;
            // double stoploss = penetration + (HighestPoint(stocks[(int)mid], x, x_index, x_index, stocks) - penetration) * 0.20;
            #endregion

            double profit_per = Math.Abs(target - xclose) / xclose * 100; // profit percentage
            double loss_per = Math.Abs(xclose - stoploss) / xclose * 100; // loss percentage
            // target period
            // added as the last variable in the object
            double pip_diff = Math.Abs(Math.Round(target - penetration, 5));

            values.Add("target", target);
            values.Add("profit_per", profit_per);
            values.Add("stoploss", stoploss);
            values.Add("loss_per", loss_per);
            values.Add("pip_diff", pip_diff);

            return values;
        }
    }
}
#endregion
