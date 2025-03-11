    #region linear period starts first
    // using System;
    // using System.Collections.Generic;
    // using System.IO;
    // using MathNet.Numerics.LinearAlgebra;
    // using MathNet.Numerics.LinearAlgebra.Double;

    // public class PolynomialRegression
    // {
    //     private readonly Vector<double> coefficients;

    //     public PolynomialRegression(double[] xData, double[] yData, int degree)
    //     {
    //         // Create the Vandermonde matrix
    //         var matrix = Matrix<double>.Build.Dense(xData.Length, degree + 1);
    //         var yVector = Vector<double>.Build.Dense(yData);

    //         for (int i = 0; i < xData.Length; i++)
    //         {
    //             for (int j = 0; j <= degree; j++)
    //             {
    //                 matrix[i, j] = Math.Pow(xData[i], j);
    //             }
    //         }

    //         // Solve for the coefficients using Least Squares
    //         var qr = matrix.QR();
    //         coefficients = qr.Solve(yVector);
    //     }
        

    //     // Method to get the y value (curve value) at any given x value
    //     public double GetCurveValue(double x)
    //     {
    //         double y = 0.0;
    //         for (int i = 0; i < coefficients.Count; i++)
    //         {
    //             y += coefficients[i] * Math.Pow(x, i);
    //         }
    //         return y;
    //     }

    //     // Method to calculate BIC for this model
    //     public static double CalculateBIC(double[] xData, double[] yData, int degree, out PolynomialRegression model)
    //     {
    //         model = new PolynomialRegression(xData, yData, degree);
    //         double n = xData.Length; // Number of data points
    //         double k = degree + 1;  // Number of parameters (degree + intercept)

    //         // Calculate Residual Sum of Squares (RSS)
    //         double rss = 0.0;
    //         for (int i = 0; i < xData.Length; i++)
    //         {
    //             double yPred = model.GetCurveValue(xData[i]);
    //             rss += Math.Pow(yData[i] - yPred, 2);
    //         }

    //         // BIC formula: BIC = n * log(RSS / n) + k * log(n)
    //         return n * Math.Log(rss / n) + k * Math.Log(n);
    //     }

    //     // Method to determine trend and concavity for quadratic polynomial (degree 2)
    //     public double IdentifyQuadraticTrend()
    //     {
    //         if (coefficients.Count < 3)
    //         {
    //             // Console.WriteLine("This is not a quadratic polynomial.");
    //             return 0;
    //         }

    //         double a = coefficients[2]; // Coefficient of x^2
    //         double b = coefficients[1]; // Coefficient of x
    //         double c = coefficients[0]; // Coefficient of constant term

    //         // Console.WriteLine($"Quadratic Equation: y = {a:0.###}x^2 + {b:0.###}x + {c:0.###}");

    //         if (a > 0 && b < 0)
    //         {
    //             // go
    //             // Console.WriteLine("The curve is U-shaped (Concave Up).");
    //             return 1;
    //         }
    //         else if (a < 0)
    //         {
    //             // -go
    //             // Console.WriteLine("The curve is inverted U-shaped (Concave Down).");
    //             return 0;
    //         }
    //         else
    //         {
    //             // Console.WriteLine("The curve is linear or undefined as a quadratic.");
    //             return 0;
    //         }
    //     }
    // }
    #endregion

    #region Linear period more than concaving up period
    using System;
    using System.Collections.Generic;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    

    public class PolynomialRegression
    {
        private Vector<double> coefficients;

        public PolynomialRegression(double[] xData, double[] yData, int degree, double linearWeight = 2.0)
        {
            // Create the weighted Vandermonde matrix
            var matrix = Matrix<double>.Build.Dense(xData.Length, degree + 1);
            var yVector = Vector<double>.Build.Dense(xData.Length);

            for (int i = 0; i < xData.Length; i++)
            {
                double weight = i < xData.Length / 2 ? linearWeight : 1.0; // Higher weight for earlier points
                yVector[i] = yData[i] * weight;

                for (int j = 0; j <= degree; j++)
                {
                    matrix[i, j] = weight * Math.Pow(xData[i], j);
                }
            }

            // Solve for the coefficients using Least Squares
            var qr = matrix.QR();
            coefficients = qr.Solve(yVector);
        }

        // Method to get the y value (curve value) at any given x value
        public double GetCurveValue(double x)
        {
            double y = 0.0;
            for (int i = 0; i < coefficients.Count; i++)
            {
                y += coefficients[i] * Math.Pow(x, i);
            }
            return y;
        }

        // Method to calculate BIC for this model
        public static double CalculateBIC(double[] xData, double[] yData, int degree, out PolynomialRegression model, double linearWeight = 2.0)
        {
            model = new PolynomialRegression(xData, yData, degree, linearWeight);
            double n = xData.Length; // Number of data points
            double k = degree + 1;  // Number of parameters (degree + intercept)

            // Calculate Residual Sum of Squares (RSS)
            double rss = 0.0;
            for (int i = 0; i < xData.Length; i++)
            {
                double yPred = model.GetCurveValue(xData[i]);
                rss += Math.Pow(yData[i] - yPred, 2);
            }

            // BIC formula: BIC = n * log(RSS / n) + k * log(n)
            return n * Math.Log(rss / n) + k * Math.Log(n);
        }

        // The following method modifies the x2 coefficient to control the concavity of the curve
        public void AdjustConcavity(double concavityFactor)
        {
            if (coefficients.Count < 3)
            {
                throw new InvalidOperationException("The polynomial must be at least degree 2 to adjust concavity.");
            }

            // Multiply the x^2 coefficient (a) by the concavity factor
            coefficients[2] *= concavityFactor;
        }

        // Method to determine trend and concavity for quadratic polynomial (degree 2)
        public double IdentifyQuadraticTrend()
        {
            if (coefficients.Count < 3)
            {
                return 0; // Not a quadratic polynomial
            }

            double a = coefficients[2]; // Coefficient of x^2
            double b = coefficients[1]; // Coefficient of x

            if (a > 0)// && b < 0)
            {
                return 1; // U-shaped (Concave Up)
            }
            else if (a < 0)
            {
                return -1; // Inverted U-shaped (Concave Down)
            }
            else
            {
                return 0; // Linear or undefined
            }
        }
        // Method to get the concavity degree
        public double GetConcavityDegree()
        {
            double a = coefficients[2]; // Coefficient of x^2
            return Math.Abs(a); // The magnitude of the x^2 coefficient
        }

        // Method to calculate curvature at a specific x value
        public double GetCurvature(double x)
        {
            // Calculate the first derivative at x
            double firstDerivative = 0.0;
            for (int i = 1; i < coefficients.Count; i++)
            {
                firstDerivative += i * coefficients[i] * Math.Pow(x, i - 1);
            }

            // Calculate the second derivative at x
            double secondDerivative = 0.0;
            for (int i = 2; i < coefficients.Count; i++)
            {
                secondDerivative += i * (i - 1) * coefficients[i] * Math.Pow(x, i - 2);
            }

            // Calculate curvature using the formula
            double curvature = Math.Abs(secondDerivative) / Math.Pow(1 + Math.Pow(firstDerivative, 2), 1.5);

            return curvature;
        }
    }

    // class Program
    // {
    //     static void Main()
    //     {
    //         // Example data points
    //         double[] xData = { 0, 1, 2, 3, 4, 5 };
    //         double[] yData = { 10, 8, 6, 4, 5, 10 }; // Downward trend, then curve up

    //         // Fit the polynomial regression with a longer linear period
    //         int degree = 2;
    //         double linearWeight = 3.0; // Emphasize the linear trend
    //         var regression = new PolynomialRegression(xData, yData, degree, linearWeight);

    //         // Print the polynomial equation
    //         Console.WriteLine("Fitted Polynomial:");
    //         for (int i = 0; i < regression.coefficients.Count; i++)
    //         {
    //             Console.WriteLine($"Coefficient for x^{i}: {regression.coefficients[i]:0.###}");
    //         }

    //         // Identify the trend and concavity
    //         double trend = regression.IdentifyQuadraticTrend();
    //         Console.WriteLine(trend == 1
    //             ? "The curve starts linear and transitions to a concave upward trend."
    //             : "The curve does not follow the desired trend.");

    //         // Evaluate the curve at various points
    //         Console.WriteLine("\nEvaluating the curve:");
    //         for (double x = 0; x <= 6; x += 0.5)
    //         {
    //             Console.WriteLine($"At x = {x:0.0}, y = {regression.GetCurveValue(x):0.00}");
    //         }
    //     }
    // }
    #endregion