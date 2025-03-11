using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Simple structure to hold (UnixTime, MACD Value).
/// </summary>
public class MacdPoint
{
    public long UnixTime { get; set; }
    public double MacdValue { get; set; }
}
public static class MacdRegressionCalculator
{
    /// <summary>
    /// Computes the linear regression line from a list of MACD points (time vs. MACD).
    /// </summary>
    /// <param name="macdPoints">A list of MacdPoint objects, each with UnixTime and MacdValue.</param>
    /// <returns>A LinearRegressionLine based on MACD over time.</returns>
    public static LinearRegressionLine ComputeLinearRegression(List<MacdPoint> macdPoints)
    {
        if (macdPoints == null || macdPoints.Count < 2)
            throw new ArgumentException("At least two MACD points are required.");

        var xVals = macdPoints.Select(mp => (double)mp.UnixTime).ToList();
        var yVals = macdPoints.Select(mp => mp.MacdValue).ToList();

        return LinearRegressionLine.Create(xVals, yVals);
    }
}

public static class MacdCalculator
{
    /// <summary>
    /// Calculates the MACD line for each candle based on the closes.
    /// shortPeriod = 12, longPeriod = 26 by default, but you can override.
    /// 
    /// Returns one MacdPoint for each candle:
    ///   MacdPoint.UnixTime = candle.UnixTime
    ///   MacdPoint.MacdValue = EMA_short - EMA_long
    /// </summary>
    public static List<MacdPoint> CalculateMacd(
        List<Candle> candles, 
        int shortPeriod = 12, 
        int longPeriod  = 26)
    {
        if (candles == null || candles.Count == 0)
            throw new ArgumentException("No candle data for MACD calculation.");

        if (shortPeriod <= 0 || longPeriod <= 0)
            throw new ArgumentException("Periods must be positive.");

        // Extract close prices and times
        double[] closePrices = candles.Select(c => c.Close).ToArray();
        long[]   times       = candles.Select(c => c.UnixTime).ToArray();

        // Calculate the two EMAs
        double[] shortEma = CalculateEma(closePrices, shortPeriod);
        double[] longEma  = CalculateEma(closePrices, longPeriod);

        // Build MACD = shortEma[i] - longEma[i]
        var macdPoints = new List<MacdPoint>(candles.Count);

        for (int i = 0; i < candles.Count; i++)
        {
            double macdValue = shortEma[i] - longEma[i];
            macdPoints.Add(new MacdPoint
            {
                UnixTime  = times[i],
                MacdValue = macdValue
            });
        }

        return macdPoints;
    }

    /// <summary>
    /// Calculates an EMA for the given prices with the specified period.
    /// Returns an array of the same length as 'prices'.
    /// 
    /// Standard formula:
    ///    EMA[i] = alpha * price[i] + (1 - alpha) * EMA[i-1]
    ///    alpha = 2 / (period + 1)
    /// 
    /// The first EMA value is set to prices[0] (simple initialization).
    /// </summary>
    private static double[] CalculateEma(double[] prices, int period)
    {
        double[] ema = new double[prices.Length];
        if (prices.Length == 0)
            return ema; // empty

        double alpha = 2.0 / (period + 1);

        // Initialize first EMA value:
        ema[0] = prices[0];

        // Compute subsequent EMA values
        for (int i = 1; i < prices.Length; i++)
        {
            ema[i] = alpha * prices[i] + (1 - alpha) * ema[i - 1];
        }

        return ema;
    }
}

public enum PriceSelector
{
    Open,
    High,
    Low,
    Close,
    Average // (Open+High+Low+Close)/4
}

public class Candle
{
    public long UnixTime { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
}

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
    /// Returns the line value at the specified Unix time.
    /// </summary>
    public double GetLineValueAtTime(long unixTime)
    {
        // Convert the long to double for the math
        double x = (double)unixTime;
        return (Slope * x) + Intercept;
    }

    /// <summary>
    /// Creates a LinearRegressionLine from arrays of x-values and y-values.
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
        double intercept = meanY - slope * meanX;

        return new LinearRegressionLine(slope, intercept);
    }
}

public static class LinearRegressionCalculator
{
    /// <summary>
    /// Computes the linear regression line from a list of Candle objects,
    /// using either open, high, low, close, or the average of these values.
    /// </summary>
    public static LinearRegressionLine ComputeLinearRegression(
        List<Candle> candles, 
        PriceSelector priceSelector)
    {
        if (candles == null || candles.Count == 0)
            throw new ArgumentException("No candle data provided.");

        List<double> xVals = new List<double>();
        List<double> yVals = new List<double>();

        foreach (var candle in candles)
        {
            double x = (double)candle.UnixTime;  // might want to offset if very large
            double y = priceSelector switch
            {
                PriceSelector.Open    => candle.Open,
                PriceSelector.High    => candle.High,
                PriceSelector.Low     => candle.Low,
                PriceSelector.Close   => candle.Close,
                PriceSelector.Average => (candle.Open + candle.High + candle.Low + candle.Close) / 4.0,
                _ => throw new NotImplementedException()
            };

            xVals.Add(x);
            yVals.Add(y);
        }

        return LinearRegressionLine.Create(xVals, yVals);
    }
}

// ------------------------------------
// Example usage
// ------------------------------------
// public class Program
// {
//     public static void Main()
//     {
//         // Sample candle data
//         var candles = new List<Candle>
//         {
//             new Candle { UnixTime = 1677638400, Open = 101.5, High = 102.0, Low = 100.2, Close = 101.9 },
//             new Candle { UnixTime = 1677638460, Open = 101.9, High = 103.2, Low = 101.1, Close = 102.7 },
//             new Candle { UnixTime = 1677638520, Open = 102.7, High = 103.5, Low = 102.4, Close = 103.1 },
//             // ...
//         };

//         // 1) Compute linear regression using "Close" prices
//         var regressionLineClose = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Close);

//         Console.WriteLine("Linear Regression (Close Price):");
//         Console.WriteLine($"  Slope: {regressionLineClose.Slope}");
//         Console.WriteLine($"  Intercept: {regressionLineClose.Intercept}");

//         // 2) Get the line value at some specific Unix time
//         long specificTime = 1677638600;
//         double lineValueAtTime = regressionLineClose.GetLineValueAtTime(specificTime);
//         Console.WriteLine($"  Line value at time {specificTime}: {lineValueAtTime}");

//         // 3) Compute regression using the average (O+H+L+C)/4
//         var regressionLineAvg = LinearRegressionCalculator.ComputeLinearRegression(candles, PriceSelector.Average);
//         Console.WriteLine("\nLinear Regression (Average Price):");
//         Console.WriteLine($"  Slope: {regressionLineAvg.Slope}");
//         Console.WriteLine($"  Intercept: {regressionLineAvg.Intercept}");
//         Console.WriteLine($"  Line value at time {specificTime}: {regressionLineAvg.GetLineValueAtTime(specificTime)}");
//     }
// }