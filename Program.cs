using System;
using System.Collections.Generic;
// using MathNet.Numerics.LinearAlgebra;
// using MathNet.Numerics.LinearAlgebra.Double;

namespace Multi
{
    class Program
    {
        static void Main(string[] args)
        {
            #region  IB forex data
            string[] Shares = { //"NZD.CAD" };
            "EUR.AUD", "AUD.CAD", "EUR.CAD", "GBP.CAD", "NZD.CAD", "USD.CAD", "AUD.CHF", "CAD.CHF", "EUR.CHF",
                "GBP.CHF", "NZD.CHF", "USD.CHF", "EUR.GBP", /*"AUD.NZD",*/ "AUD.USD", "EUR.USD", "GBP.USD", "NZD.USD" };

            // "EUR.AUD", "AUD.CAD", "EUR.CAD", "NZD.CAD", "USD.CAD", "AUD.CHF", "CAD.CHF", "EUR.CHF",
            //     "NZD.CHF", "USD.CHF", "AUD.NZD", "AUD.USD", "EUR.USD", "NZD.USD" };
                // "EUR.AUD", "AUD.CAD", "EUR.CAD", "NZD.CAD", "USD.CAD", "AUD.CHF", "CAD.CHF", "EUR.CHF",
                // "NZD.CHF", "USD.CHF", "AUD.NZD", "AUD.USD", "EUR.USD", "NZD.USD" };
                // "OANDA_EURCHF, 60testt", "OANDA_NZDCHF, 60nov-2021" };
                // "OANDA_EURAUD, 60", "OANDA_AUDCAD, 60", "OANDA_EURCAD, 60", "OANDA_GBPCAD, 60", "OANDA_NZDCAD, 60", "OANDA_USDCAD, 60", "OANDA_AUDCHF, 60", "OANDA_CADCHF, 60", "OANDA_EURCHF, 60",
                // "OANDA_GBPCHF, 60", "OANDA_NZDCHF, 60", "OANDA_USDCHF, 60", "OANDA_EURGBP, 60", "OANDA_AUDNZD, 60", "OANDA_AUDUSD, 60", "OANDA_EURUSD, 60", "OANDA_GBPUSD, 60", "OANDA_NZDUSD, 60" };
                // "EUR.AUD, IB", "AUD.CAD, IB", "EUR.CAD, IB", "GBP.CAD, IB", "NZD.CAD, IB", "USD.CAD, IB", "AUD.CHF, IB", "CAD.CHF, IB", "EUR.CHF, IB",
                // "GBP.CHF, IB", "NZD.CHF, IB", "USD.CHF, IB", "EUR.GBP, IB", "AUD.NZD, IB", "AUD.USD, IB", "EUR.USD, IB", "GBP.USD, IB", "NZD.USD, IB" };//, "HKD.JPY, IB", "ZAR.JPY, IB" };
            #endregion

            #region IB Stocks
            // string[] Shares = { //"CRM" };
            // // comparison data
            // "JPM","IBKR","PFE","MA","MU","MSFT","EBAY","TECH","MRVL",
            // "GOOG","CRM","SAIA","AAPL","ADSK","GE","ALK","VZ","AVGO","WMT","F","DIS","ADI","LLY","BIDU","QCOM","ALGN","C","ABT","CMCSA","KO","TXN",
            // "GS","BAC","BBBY","GRMN","AMT","TUP","BBY","BA","AMD","V","HD","AMAT","ADBE","PLUG","NVDA","CTSH","DAL","JNJ","AMZN","ASML","AVT","T","PEP"
            // }; // 1hour // 1D

            // "MU","MSFT","EBAY","GOOG", "AAPL", "ADSK", "GE", "VZ","AVGO", "F","QCOM","CMCSA","KO","TXN","BAC","AMD","ADBE","NVDA","AMZN","ASML","T","PEP" }; // 5min
            // // // "aapl", "avgo", "goog", "msft", "cmcsa" }; // 1min

            // "MSFT","GOOG","AAPL","TMUS","AVGO","PYPL","GOOGL","TSLA","CMCSA","TXN","ADBE","NVDA","AMZN","ASML","PEP","FB","ROKU","MRNA","QCOM","MU","EBAY","AMD","AAL","ADSK","TWTR","UBER",
            //     "SNAP","PINS","T","PSX","VZ","BABA","KO","GM","GE","F","BAC","DAL","BA","AMT","ALK","TECH" ,"PTON" ,"BILL" ,"AMC"  ,"SAVA" ,"CRM"  ,"WDAY" ,"SNOW" ,"DIS"  ,"NURO" ,"PFE"  ,"ADI",
            //     "SAIA","V","JPM","PLTR", "MA" ,"BBIG","SQ"  ,"JNJ" ,"CRWD","AMAT","WMT" ,"ZM" ,"CVX" ,"C","JD","BRK B","IBKR" ,"HD"   ,"SE"   ,"MIME" ,"GS"   ,"MRVL" ,"LLY"  ,"ALGN" ,"FVRR" ,"GRMN",
            //     "TUP"  ,"PLUG" ,"FCEL" ,"NET"  ,"MNDT","SHOP" ,"SPOT" ,"LCID" ,"HOOD" ,"CTSH" ,"ABT"  ,"AVT"  ,"BBBY" ,"BBY"  ,"BIDU" ,"BKNG"
                // // };
                //  ,
                //  "CCL" ,"CL"   ,"CLF"  ,"COP"  ,"CPB"  ,"DDD"  ,
                // "DXCM" ,"BB"   ,"FCX"  ,"FDX"  ,"FSLR" ,"HP"   ,"HPE"  ,"HPQ"  ,"LYFT" ,"MCD"  ,"NIO"  ,"NKE"  ,"NOK"  ,"NOV"  ,"NTAP" ,"OXY"  ,"PCG"  ,"SNA" ,"TEVA","VRNA","TXT" ,"SBUX","X","REGN"
            // };
            #endregion
            #region  stocks data
            // string[] Shares = { //"NYSE_TWTR, 60" };
            // "NASDAQ_GOOG, 60", "NASDAQ_AAPL, 60", "NASDAQ_FB, 60", "NYSE_TWTR, 60", "NYSE_UBER, 60", "NASDAQ_MSFT, 60", "NYSE_SNAP, 60", "NASDAQ_TSLA, 60", "NASDAQ_NFLX, 60",
            // "NYSE_PINS, 60", "NASDAQ_ROKU, 60", "NASDAQ_MRNA, 60", "NASDAQ_QCOM, 60", "NYSE_T, 60", "NASDAQ_PYPL, 60", "NYSE_PSX, 60", "NYSE_VZ, 60", "NASDAQ_NVDA, 60",
            // "NYSE_BABA, 60", "NASDAQ_MU, 60", "NYSE_KO, 60", "NYSE_GM, 60", "NYSE_GE, 60", "NYSE_F, 60", "NYSE_BAC, 60", "NASDAQ_EBAY, 60", "NYSE_DAL, 60", "NYSE_C, 60",
            // "NYSE_BA, 60", "NASDAQ_AMZN, 60", "NASDAQ_AMD, 60", "NYSE_AMT, 60", "NASDAQ_AAL, 60", "NASDAQ_ADSK, 60", "NYSE_ALK, 60" };
            // 5 min
            //    "NASDAQ_GOOG, 5", "NASDAQ_AAPL, 5", "NASDAQ_BKNG, 5", "NASDAQ_MSFT, 5", "NYSE_TWTR, 5", "NYSE_SNAP, 5", "NASDAQ_FB, 5", "NYSE_UBER, 5", "NASDAQ_AMZN, 5",
            //    "NYSE_BABA, 5", "NASDAQ_EBAY, 5", "NASDAQ_NFLX, 5", "NASDAQ_BIDU, 5", "NASDAQ_TSLA, 5", "NASDAQ_INTC, 5", "NYSE_KO, 5", "NYSE_BAC, 5", "NYSE_T, 5", "NYSE_F, 5", "NYSE_ABT, 5",
            //    "NYSE_BB, 5", "NASDAQ_AMAT, 5", "NASDAQ_AMD, 5", "NASDAQ_FSLR, 5", "NYSE_HPQ, 5", "NASDAQ_ADSK, 5", "NYSE_CVX, 5", "NYSE_BBY, 5", "NASDAQ_REGN, 5", "NYSE_GE, 5", "NYSE_PSX, 5",
            //    "NYSE_FCX, 5", "NYSE_C, 5", "NASDAQ_CSCO, 5", "NASDAQ_CMCSA, 5", "NASDAQ_QCOM, 5", "NASDAQ_NVDA, 5", "NYSE_DAL, 5", "NYSE_BA, 5", "NYSE_GM, 5", "NYSE_NIO, 5", "NASDAQ_MU, 5",
            //    "NASDAQ_SBUX, 5", "NYSE_X, 5", "NASDAQ_DNKN, 5", "NYSE_DDD, 5", "NYSE_NOK, 5", "NYSE_MCD, 5", "NASDAQ_AAL, 5", "NYSE_VZ, 5", "NYSE_MA, 5", "NASDAQ_PYPL, 5", "NYSE_COP, 5",
            //    "NASDAQ_DXCM, 5", "NASDAQ_JD, 5", "NASDAQ_ROKU, 5", "NASDAQ_LVGO, 5" };
            #endregion
            
            #region other data
            //string[] Shares = { "OANDA_EURUSD, 5", "OANDA_GBPUSD, 5", "OANDA_AUDUSD, 5", "OANDA_NZDUSD, 5" };
            //string[] Shares = { "gbpusd" };
            //string[] Shares = { /*"OANDA_EURUSD, 3",*/ "OANDA_EURUSD, 5" /*"OANDA_EURUSD, 15",*//* "OANDA_EURUSD, 30",*//* "OANDA_EURUSD, 60"*/ };
            //string[] Shares = { "FOREXCOM_EURUSD, 5", "FOREXCOM_GBPUSD, 5", "FOREXCOM_EURCAD, 5", "FOREXCOM_USDJPY, 5" };
            //string[] Shares = { "CURRENCYCOM_EURUSD, 5", "CURRENCYCOM_GBPUSD, 5", "CURRENCYCOM_USDCHF, 5", "CURRENCYCOM_USDJPY, 5", "CURRENCYCOM_USDCAD, 5", "CURRENCYCOM_USDMXN, 5" };
            //string[] Shares = { "CURRENCYCOM_USDHKD, 5", "CURRENCYCOM_USDHKD, 30" };
            //string[] Shares = { "CURRENCYCOM_EURUSD, 5", "CURRENCYCOM_GBPUSD, 5" };
            //string[] Shares = { /*"SAXO_EURUSD, 5", "OANDA_EURUSD, 5", "OANDA_GBPUSD, 5",*/ "CURRENCYCOM_OIL_CRUDE, 5", "CURRENCYCOM_OIL_BRENT, 5", "OANDA_SUGARUSD, 5", "OANDA_XAUUSD, 30" };
            //string[] Shares = { "CURRENCYCOM_OIL_BRENT, 30" }; // important (lesson learned: I have to limit stop order to be less than a specific percentage!)
            //string[] Shares = { "ICEUS_SB1!, D" };
            //string[] Shares = { "CBOT_DL_ZO1!, 5" };
            //string[] Shares = { "NASDAQ_GOOG, 30", "NASDAQ_AAPL, 30", "NASDAQ_BKNG, 30", "NASDAQ_MSFT, 30", "NYSE_TWTR, 30", "NYSE_SNAP, 30", "NASDAQ_FB, 30", "NYSE_UBER, 30", "NASDAQ_AMZN, 30", "NYSE_BABA, 30", "NASDAQ_EBAY, 30", "NASDAQ_NFLX, 30", "NASDAQ_BIDU, 30", "NASDAQ_TSLA, 30", "NASDAQ_INTC, 30", "NYSE_KO, 30", "NYSE_BAC, 30", "NYSE_T, 30", "NYSE_F, 30", "NYSE_ABT, 30", "NYSE_BB, 30", "NASDAQ_AMAT, 30", "NASDAQ_AMD, 30", "NASDAQ_FSLR, 30", "NYSE_HPQ, 30",
            //"NASDAQ_ADSK, 30", "NYSE_CVX, 30", "NYSE_BBY, 30", "NASDAQ_REGN, 30", "NYSE_GE, 30", "NYSE_PSX, 30", "NYSE_FCX, 30", "NYSE_C, 30", "NASDAQ_CSCO, 30", "NASDAQ_CMCSA, 30", "NASDAQ_QCOM, 30", "NASDAQ_NVDA, 30", "NYSE_DAL, 30", "NYSE_BA, 30", "NYSE_GM, 30", "NYSE_NIO, 30" };
            //string[] Shares = { "CURRENCYCOM_EURUSD, 5", "SAXO_EURUSD, 5", "FX_IDC_EURUSD, 5", "FOREXCOM_EURUSD, 5", "OANDA_EURUSD, 5", "FX_EURUSD, 5" };
            //string[] Shares = { "CURRENCYCOM_EURUSD, 30", "SAXO_EURUSD, 30", "FX_IDC_EURUSD, 30", "FOREXCOM_EURUSD, 30", "OANDA_EURUSD, 30", "FX_EURUSD, 30" };
            //string[] Shares = { "CURRENCYCOM_EURUSD, 30", "CURRENCYCOM_GBPUSD, 30" };
            //string[] Shares = { "FX_GBPUSD, 5" };
            #endregion

            #region my calculations
            double success = 0; double fail = 0; double total_profit = 0; double total_loss = 0; double target_period = 0;
            for (int i = 0; i < Shares.Length; i++)
            {
                List<Stock> stocks;
                List<Stocks5min> targetstocks;
                List<Stocks5minTar> targetstocksTar;
                List<Consolidations> consolidations;
                int year = 2019;
                int month = 5;
                // ###############################################################################################################
                List<BuyPattern> patterns;

                // for db data
                stocks = Stock.GetStockData(@"Server=MSI\SQLEXPRESS; Database=market_data; Integrated Security=True;", Shares[i], year, month);
                consolidations = Consolidations.GetConsolidations(stocks);
                // stocks.Reverse();
                targetstocks = Stocks5min.GetStockData(@"Server=MSI\SQLEXPRESS; Database=market_data; Integrated Security=True;", Shares[i], year, month);
                targetstocksTar = Stocks5minTar.GetStockData(@"Server=MSI\SQLEXPRESS; Database=market_data; Integrated Security=True;", Shares[i], year, month);
                // targetstocks.Reverse();
                // // for excel data
                // stocks = Stock.GetStockData(@"C:\stocks\" + Shares[i] + ".csv", Shares[i]);

                patterns = BuyPattern.ThePattern(stocks, targetstocks, consolidations, targetstocksTar, year, month);
                Console.WriteLine("-------" + Shares[i] + "-------");
                double share_success = 0; double share_fail = 0;
                foreach (BuyPattern p in patterns)
                // // ###############################################################################################################
                {
                    // success // for every share // for all shares // total profit %
                    if (p.Status == 1) { share_success++; success++; total_profit = total_profit + Math.Round(p.Profit, 5); }
                    // fail // for every share // for all shares // total loss %
                    if (p.Status == 0) { share_fail++; fail++; total_loss = total_loss + Math.Round(p.Loss, 5); }
                    // printing the data points
                    target_period = target_period + p.Period;
                    Console.WriteLine(
                        " " + p.Status + " " + 
                        p.StartCon.ToString("MM/dd/yyyy HH:mm") + " " +
                        p.EndCon.ToString("MM/dd/yyyy HH:mm") + " " +
                        p.P1.ToString("MM/dd/yyyy HH:mm") + " " +
                        p.P2.ToString("MM/dd/yyyy HH:mm") + " " +
                        p.XX.ToString("MM/dd/yyyy HH:mm") + " " + 
                        p.Cancel + " " +
                        "( " +
                        "stoplimit: " +
                        Math.Round(p.Tradeopen, 5) + " " +
                        "target: " +
                        Math.Round(p.Target, 5) + " " + " " + 
                        "stoploss: " +
                        Math.Round(p.Stoploss, 5) + " " + ") " + 
                        Math.Round(p.Target, 5) + " " + " " + Math.Round(p.Profit, 5) + "%" + " " + Math.Round(p.Loss, 5) + "%" + " " +  Math.Round(p.Period, 5));
                }
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine(" " + Shares[i] + " Accuracy ==> {0} %", Math.Round(share_success / (share_success + share_fail) * 100, 2));
                Console.WriteLine("---------------------------------------------");
            }
            Console.WriteLine("Succeeded= {0}, Failed= {1}, Total= {2}, Accuracy= {3}% , Profit= {4}, Loss= {5}, avg Profit = {6}%, avg period= {7}", success, fail, success + fail, Math.Round(success / (success + fail) * 100, 2), total_profit, total_loss, Math.Round((1 - (total_loss / total_profit)) * 100, 1), Math.Round(target_period / (success), 1));
            Console.WriteLine("Moreef Analysis has finished!");
            #endregion
        }
    }
}

#region from tradingview export unix timestamp then add it to OandaTbl
// INSERT INTO [market_data].[dbo].[OandaTbl] ([symbol]
// --      ,[unixdatetime]
// --      ,[_open]
// --      ,[_high]
// --      ,[_low]
// --      ,[_close]
// --      ,[_datetime])

// --select 'nzd.usd'
// --,[time]
// --,[open]
// --,[high]
// --,[low]
// --,[close]
// --,cast(dateadd(S, [time], '1970-01-02') as date)
// --from [market_data].[dbo].[NZDUSD_1D]
#endregion