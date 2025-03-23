#region Old

// using System;
// using System.Collections.Generic;
// using System.Linq;

// /// <summary>
// /// Simple structure to hold (UnixTime, MACD Value).
// /// </summary>
// public class MacdPoint
// {
//     public long UnixTime { get; set; }
//     public double MacdValue { get; set; }
// }
// public static class MacdRegressionCalculator
// {
//     /// <summary>
//     /// Computes the linear regression line from a list of MACD points (time vs. MACD).
//     /// </summary>
//     /// <param name="macdPoints">A list of MacdPoint objects, each with UnixTime and MacdValue.</param>
//     /// <returns>A LinearRegressionLine based on MACD over time.</returns>
//     public static LinearRegressionLine ComputeLinearRegression(List<MacdPoint> macdPoints)
//     {
//         if (macdPoints == null || macdPoints.Count < 2)
//             throw new ArgumentException("At least two MACD points are required.");

//         var xVals = macdPoints.Select(mp => (double)mp.UnixTime).ToList();
//         var yVals = macdPoints.Select(mp => mp.MacdValue).ToList();

//         return LinearRegressionLine.Create(xVals, yVals);
//     }
// }

// public static class MacdCalculator
// {
//     /// <summary>
//     /// Calculates the MACD line for each candle based on the closes.
//     /// shortPeriod = 12, longPeriod = 26 by default, but you can override.
//     /// 
//     /// Returns one MacdPoint for each candle:
//     ///   MacdPoint.UnixTime = candle.UnixTime
//     ///   MacdPoint.MacdValue = EMA_short - EMA_long
//     /// </summary>
//     public static List<MacdPoint> CalculateMacd(
//         List<Candle> candles, 
//         int shortPeriod = 12, 
//         int longPeriod  = 26)
//     {
//         if (candles == null || candles.Count == 0)
//             throw new ArgumentException("No candle data for MACD calculation.");

//         if (shortPeriod <= 0 || longPeriod <= 0)
//             throw new ArgumentException("Periods must be positive.");

//         // Extract close prices and times
//         double[] closePrices = candles.Select(c => c.Close).ToArray();
//         long[]   times       = candles.Select(c => c.UnixTime).ToArray();

//         // Calculate the two EMAs
//         double[] shortEma = CalculateEma(closePrices, shortPeriod);
//         double[] longEma  = CalculateEma(closePrices, longPeriod);

//         // Build MACD = shortEma[i] - longEma[i]
//         var macdPoints = new List<MacdPoint>(candles.Count);

//         for (int i = 0; i < candles.Count; i++)
//         {
//             double macdValue = shortEma[i] - longEma[i];
//             macdPoints.Add(new MacdPoint
//             {
//                 UnixTime  = times[i],
//                 MacdValue = macdValue
//             });
//         }

//         return macdPoints;
//     }

//     /// <summary>
//     /// Calculates an EMA for the given prices with the specified period.
//     /// Returns an array of the same length as 'prices'.
//     /// 
//     /// Standard formula:
//     ///    EMA[i] = alpha * price[i] + (1 - alpha) * EMA[i-1]
//     ///    alpha = 2 / (period + 1)
//     /// 
//     /// The first EMA value is set to prices[0] (simple initialization).
//     /// </summary>
//     private static double[] CalculateEma(double[] prices, int period)
//     {
//         double[] ema = new double[prices.Length];
//         if (prices.Length == 0)
//             return ema; // empty

//         double alpha = 2.0 / (period + 1);

//         // Initialize first EMA value:
//         ema[0] = prices[0];

//         // Compute subsequent EMA values
//         for (int i = 1; i < prices.Length; i++)
//         {
//             ema[i] = alpha * prices[i] + (1 - alpha) * ema[i - 1];
//         }

//         return ema;
//     }
// }

// public enum PriceSelector
// {
//     Open,
//     High,
//     Low,
//     Close,
//     Average // (Open+High+Low+Close)/4
// }

// public class Candle
// {
//     public long UnixTime { get; set; }
//     public double Open { get; set; }
//     public double High { get; set; }
//     public double Low { get; set; }
//     public double Close { get; set; }
// }

// public class LinearRegressionLine
// {
//     public double Slope { get; }
//     public double Intercept { get; }

//     private LinearRegressionLine(double slope, double intercept)
//     {
//         Slope = slope;
//         Intercept = intercept;
//     }

//     /// <summary>
//     /// Returns the line value at the specified Unix time.
//     /// </summary>
//     public double GetLineValueAtTime(long unixTime)
//     {
//         // Convert the long to double for the math
//         double x = (double)unixTime;
//         return (Slope * x) + Intercept;
//     }

//     /// <summary>
//     /// Creates a LinearRegressionLine from arrays of x-values and y-values.
//     /// </summary>
//     public static LinearRegressionLine Create(IEnumerable<double> xVals, IEnumerable<double> yVals)
//     {
//         double[] xArray = xVals.ToArray();
//         double[] yArray = yVals.ToArray();

//         if (xArray.Length != yArray.Length)
//             throw new ArgumentException("xVals and yVals must have the same length.");

//         if (xArray.Length < 2)
//             throw new ArgumentException("Need at least two points to form a line.");

//         double meanX = xArray.Average();
//         double meanY = yArray.Average();

//         double numerator = 0.0;
//         double denominator = 0.0;

//         for (int i = 0; i < xArray.Length; i++)
//         {
//             double xDiff = xArray[i] - meanX;
//             double yDiff = yArray[i] - meanY;

//             numerator   += xDiff * yDiff;
//             denominator += xDiff * xDiff;
//         }

//         double slope = numerator / denominator;
//         double intercept = meanY - slope * meanX;

//         return new LinearRegressionLine(slope, intercept);
//     }
// }

// public static class LinearRegressionCalculator
// {
//     /// <summary>
//     /// Computes the linear regression line from a list of Candle objects,
//     /// using either open, high, low, close, or the average of these values.
//     /// </summary>
//     public static LinearRegressionLine ComputeLinearRegression(
//         List<Candle> candles, 
//         PriceSelector priceSelector)
//     {
//         if (candles == null || candles.Count == 0)
//             throw new ArgumentException("No candle data provided.");

//         List<double> xVals = new List<double>();
//         List<double> yVals = new List<double>();

//         foreach (var candle in candles)
//         {
//             double x = (double)candle.UnixTime;  // might want to offset if very large
//             double y = priceSelector switch
//             {
//                 PriceSelector.Open    => candle.Open,
//                 PriceSelector.High    => candle.High,
//                 PriceSelector.Low     => candle.Low,
//                 PriceSelector.Close   => candle.Close,
//                 // PriceSelector.Average => (candle.Open + candle.High + candle.Low + candle.Close) / 4.0,
//                 PriceSelector.Average => (candle.High + candle.Low) / 2.0,
//                 _ => throw new NotImplementedException()
//             };

//             xVals.Add(x);
//             yVals.Add(y);
//         }

//         return LinearRegressionLine.Create(xVals, yVals);
//     }
// }

// // ------------------------------------
// // Example usage
// // ------------------------------------
// // public class Program
// // {
// //     public static void Main()
// //     {
// //         // Sample candle data
// //         var candles = new List<Candle>
// //         {
// //             new Candle { UnixTime = 1677638400, Open = 101.5, High = 102.0, Low = 100.2, Close = 101.9 },
// //             new Candle { UnixTime = 1677638460, Open = 101.9, High = 103.2, Low = 101.1, Close = 102.7 },
// //             new Candle { UnixTime = 1677638520, Open = 102.7, High = 103.5, Low = 102.4, Close = 103.1 },
// //             // ...
// //         };

// //         // 1) Compute linear regression using "Close" prices
// //         var regressionLineClose = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Close);

// //         Console.WriteLine("Linear Regression (Close Price):");
// //         Console.WriteLine($"  Slope: {regressionLineClose.Slope}");
// //         Console.WriteLine($"  Intercept: {regressionLineClose.Intercept}");

// //         // 2) Get the line value at some specific Unix time
// //         long specificTime = 1677638600;
// //         double lineValueAtTime = regressionLineClose.GetLineValueAtTime(specificTime);
// //         Console.WriteLine($"  Line value at time {specificTime}: {lineValueAtTime}");

// //         // 3) Compute regression using the average (O+H+L+C)/4
// //         var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Average);
// //         Console.WriteLine("\nLinear Regression (Average Price):");
// //         Console.WriteLine($"  Slope: {regressionLineAvg.Slope}");
// //         Console.WriteLine($"  Intercept: {regressionLineAvg.Intercept}");
// //         Console.WriteLine($"  Line value at time {specificTime}: {regressionLineAvg.GetLineValueAtTime(specificTime)}");
// //     }
// // }

#endregion

#region New with Signal

using System;
using System.Collections.Generic;
using System.Linq;

#region Data Structures

/// <summary>
/// Represents one OHLC candle, plus UnixTime.
/// </summary>
public class Candle
{
    public long UnixTime { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low  { get; set; }
    public double Close { get; set; }
}

/// <summary>
/// Enumeration to pick which price to use in linear regression.
/// </summary>
public enum PriceSelector
{
    Open,
    High,
    Low,
    Close,
    Average // Could be (High+Low)/2 or (O+H+L+C)/4, etc.
}

/// <summary>
/// Simple structure to hold (UnixTime, MACD Value).
/// </summary>
public class MacdPoint
{
    public long UnixTime { get; set; }
    public double MacdValue { get; set; }
}

#endregion

#region MACD Calculator

/// <summary>
/// Utility class to calculate MACD and its signal line, as well as the underlying EMAs.
/// </summary>
public static class MacdCalculator
{
    /// <summary>
    /// Calculates the MACD line for each candle based on the closes.
    /// Default fastPeriod=12, slowPeriod=26, but can be overridden.
    /// 
    /// Returns one MacdPoint for each candle:
    ///   MacdPoint.UnixTime = candle.UnixTime
    ///   MacdPoint.MacdValue = EMA(fastPeriod) - EMA(slowPeriod)
    /// </summary>
    public static List<MacdPoint> CalculateMacd(
        List<Candle> candles, 
        int fastPeriod = 12, 
        int slowPeriod = 26)
    {
        if (candles == null || candles.Count == 0)
            throw new ArgumentException("No candle data for MACD calculation.");
        if (fastPeriod <= 0 || slowPeriod <= 0)
            throw new ArgumentException("Periods must be positive.");

        // Extract close prices and times
        double[] closePrices = candles.Select(c => c.Close).ToArray();
        long[]   times       = candles.Select(c => c.UnixTime).ToArray();

        // Calculate the two EMAs
        double[] fastEma = CalculateEma(closePrices, fastPeriod);
        double[] slowEma = CalculateEma(closePrices, slowPeriod);

        // Build MACD = fastEma[i] - slowEma[i]
        var macdPoints = new List<MacdPoint>(candles.Count);

        for (int i = 0; i < candles.Count; i++)
        {
            double macdValue = fastEma[i] - slowEma[i];
            macdPoints.Add(new MacdPoint
            {
                UnixTime  = times[i],
                MacdValue = macdValue
            });
        }

        return macdPoints;
    }

    /// <summary>
    /// NEW METHOD:
    /// Calculates the "signal line" from a list of MacdPoint objects.
    /// The signal line is typically the 9-period EMA of the MACD line.
    /// 
    /// Returns a new list of MacdPoint with:
    ///   MacdPoint.UnixTime = same as original
    ///   MacdPoint.MacdValue = signal line EMA
    /// </summary>
    public static List<MacdPoint> CalculateSignal(
        List<MacdPoint> macdPoints, 
        int signalPeriod = 9)
    {
        if (macdPoints == null || macdPoints.Count == 0)
            throw new ArgumentException("No MACD data provided.");
        if (signalPeriod <= 0)
            throw new ArgumentException("Signal period must be positive.");

        double[] macdValues = macdPoints.Select(mp => mp.MacdValue).ToArray();
        long[]   times      = macdPoints.Select(mp => mp.UnixTime).ToArray();

        // Compute EMA of the MACD line
        double[] signalEma = CalculateEma(macdValues, signalPeriod);

        // Build MacdPoint list for the signal line
        var signalPoints = new List<MacdPoint>(macdPoints.Count);
        for (int i = 0; i < macdPoints.Count; i++)
        {
            signalPoints.Add(new MacdPoint
            {
                UnixTime  = times[i],
                MacdValue = signalEma[i]
            });
        }

        return signalPoints;
    }

    /// <summary>
    /// Helper to calculate an EMA array for given data and period.
    /// Standard formula:
    ///    EMA[i] = alpha*price[i] + (1 - alpha)*EMA[i-1]
    ///    alpha = 2/(period+1)
    /// 
    /// The first EMA is seeded with prices[0].
    /// </summary>
    private static double[] CalculateEma(double[] prices, int period)
    {
        double[] ema = new double[prices.Length];
        if (prices.Length == 0)
            return ema; // empty set

        double alpha = 2.0 / (period + 1);

        // Initialize
        ema[0] = prices[0];

        // Fill
        for (int i = 1; i < prices.Length; i++)
        {
            ema[i] = alpha * prices[i] + (1 - alpha) * ema[i - 1];
        }

        return ema;
    }
}

#endregion

#region MACD Regression

/// <summary>
/// Utility for performing a linear regression (time vs. MACD).
/// </summary>
public static class MacdRegressionCalculator
{
    /// <summary>
    /// Computes the linear regression line from a list of MacdPoint (time vs. MACD).
    /// e.g. to see if the MACD line is trending up or down over time.
    /// </summary>
    public static LinearRegressionLine ComputeLinearRegression(List<MacdPoint> macdPoints)
    {
        if (macdPoints == null || macdPoints.Count < 2)
            throw new ArgumentException("At least two MACD points are required.");

        var xVals = macdPoints.Select(mp => (double)mp.UnixTime).ToList();
        var yVals = macdPoints.Select(mp => mp.MacdValue).ToList();

        return LinearRegressionLine.Create(xVals, yVals);
    }
}

#endregion

#region Linear Regression (Price-based)

/// <summary>
/// A line described by: y = (Slope * x) + Intercept
/// Includes a method to get the line value at a given Unix time.
/// </summary>
public class LinearRegressionLine
{
    public double Slope { get; }
    public double Intercept { get; }

    private LinearRegressionLine(double slope, double intercept)
    {
        Slope = slope;
        Intercept = intercept;
    }

    /// <summary>
    /// Returns the line value at the specified Unix time (x).
    /// </summary>
    public double GetLineValueAtTime(long unixTime)
    {
        double x = (double)unixTime;
        return Slope * x + Intercept;
    }

    /// <summary>
    /// Factory that creates a LinearRegressionLine from x-values and y-values.
    /// Minimum of 2 points required.
    /// </summary>
    public static LinearRegressionLine Create(IEnumerable<double> xVals, IEnumerable<double> yVals)
    {
        double[] xArray = xVals.ToArray();
        double[] yArray = yVals.ToArray();

        if (xArray.Length != yArray.Length)
            throw new ArgumentException("xVals and yVals must have the same length.");
        if (xArray.Length < 2)
            throw new ArgumentException("Need at least two points to form a line.");

        double meanX = xArray.Average();
        double meanY = yArray.Average();

        double numerator = 0.0;
        double denominator = 0.0;

        for (int i = 0; i < xArray.Length; i++)
        {
            double xDiff = xArray[i] - meanX;
            double yDiff = yArray[i] - meanY;

            numerator   += xDiff * yDiff;
            denominator += xDiff * xDiff;
        }

        double slope = numerator / denominator;
        double intercept = meanY - (slope * meanX);

        return new LinearRegressionLine(slope, intercept);
    }
}

/// <summary>
/// Performs a linear regression on Candle data (time vs. chosen price).
/// </summary>
public static class LinearRegressionCalculator
{
    /// <summary>
    /// Computes the linear regression line from a list of Candle objects,
    /// using either open, high, low, close, or an average (like (High+Low)/2).
    /// </summary>
    public static LinearRegressionLine ComputeLinearRegression(
        List<Candle> candles, 
        PriceSelector priceSelector)
    {
        if (candles == null || candles.Count == 0)
            throw new ArgumentException("No candle data provided.");

        var xVals = new List<double>();
        var yVals = new List<double>();

        foreach (var candle in candles)
        {
            double x = (double)candle.UnixTime;
            double y = priceSelector switch
            {
                PriceSelector.Open    => candle.Open,
                PriceSelector.High    => candle.High,
                PriceSelector.Low     => candle.Low,
                PriceSelector.Close   => candle.Close,
                PriceSelector.Average => (candle.High + candle.Low) / 2.0,
                _ => throw new NotImplementedException()
            };

            xVals.Add(x);
            yVals.Add(y);
        }

        return LinearRegressionLine.Create(xVals, yVals);
    }
}

#endregion

// ---------------------------------------------------------------------------
// EXAMPLE USAGE (commented out for convenience)
// ---------------------------------------------------------------------------
//
// public class Program
// {
//     public static void Main()
//     {
//         // 1) Candle data
//         var candles = new List<Candle>
//         {
//             new Candle { UnixTime = 1677638400, Open = 101.5, High = 102.0, Low = 100.2, Close = 101.9 },
//             new Candle { UnixTime = 1677638460, Open = 101.9, High = 103.2, Low = 101.1, Close = 102.7 },
//             new Candle { UnixTime = 1677638520, Open = 102.7, High = 103.5, Low = 102.4, Close = 103.1 },
//         };

//         // 2) Calculate the MACD line (12, 26):
//         var macdPoints = MacdCalculator.CalculateMacd(candles, 12, 26);

//         // 3) Calculate the signal line (9) from the MACD line
//         var signalPoints = MacdCalculator.CalculateSignal(macdPoints, 9);

//         // 4) Do a linear regression on the MACD line
//         var macdRegLine = MacdRegressionCalculator.ComputeLinearRegression(macdPoints);
//         Console.WriteLine($"MACD Regression slope={macdRegLine.Slope}, intercept={macdRegLine.Intercept}");

//         // 5) Do a linear regression on the signal line
//         var signalRegLine = MacdRegressionCalculator.ComputeLinearRegression(signalPoints);
//         Console.WriteLine($"Signal Regression slope={signalRegLine.Slope}, intercept={signalRegLine.Intercept}");

//         // 6) Also do a price-based linear regression on the "Close"
//         var priceRegLine = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Close);
//         Console.WriteLine($"Price Regression slope={priceRegLine.Slope}, intercept={priceRegLine.Intercept}");
//     }
// }

#endregion