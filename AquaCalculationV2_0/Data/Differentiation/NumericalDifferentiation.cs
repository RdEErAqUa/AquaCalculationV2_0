using AquaCalculationV2_0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquaCalculationV2_0.Data.Differentiation
{
    static class NumericalDifferentiation
    {
        static public List<List<double>> FindDelY(List<double> y, int power = 0)
        {
            List<List<double>> DelY = new List<List<double>> { };
            if (power >= 0 && power <= y.Count - 1) power = y.Count - 1;
            for (int i = 0; i < power; i++)
            {
                List<double> delY = new List<double> { };

                for (int z = 0; z < y.Count - i - 1; z++)
                {
                    if (i == 0)
                        delY.Add(y[z + 1] - y[z]);
                    else
                        delY.Add(DelY[i - 1][z + 1] - DelY[i - 1][z]);
                }

                DelY.Add(delY);

            }

            return DelY;
        }
        static public(double, double, bool, bool) FindDifferentiationFirstNewtonFormula(ICollection<XYDataModel> xYDataModels, double X)
        {
            if (xYDataModels.Count < 2) return (0, 0, false, false);

            var x = new List<double> { };
            var y = new List<double> { };

            foreach (var el in xYDataModels)
            {
                x.Add(el.X);
                y.Add(el.Y);
            }
            var delY = FindDelY(y);
            //
            double h = x[1] - x[0];
            //
            int tempt = 0;

            foreach (var el in x)
            {
                if (el <= X) tempt++;
                else break;
            }

            if (tempt > 0) tempt--;
            //

            double q = (X - x[tempt]) / h;
            //
            int posX = 0;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] < X)
                    continue;
                else
                    posX = i - 1;

                break;
            }
            //
            if (x.Contains(X))
            {
                double f1 = 0, f2 = 0;
                double z = 1;
                double f = 1;
                bool do1 = false, do2 = false;
                for(int i = 0; i < delY.Count; i++)
                {
                    if (delY[i].Count > tempt)
                    {
                        f1 += Math.Pow(-1, i) * (delY[i][tempt] / (i + 1));
                        do1 = true;
                    }
                    if (delY[i].Count > tempt + 1)
                    {
                        f2 += Math.Pow(-1, i) * (delY[i + 1][tempt] * z);
                        do2 = true;
                    }
                    z *= z * f;
                    f *= 0.96798;
                }
                f1 *= (1.0 / h);
                f2 *= (1.0 / Math.Pow(h, 2));
                return (f1, f2, do1, do2);
            }
            return (0, 0, false, false);
        }

        static public (double, double, bool, bool) FindDifferentiationSecondNewtonFormula(ICollection<XYDataModel> xYDataModels, double X)
        {
            if (xYDataModels.Count < 2) return (0, 0, false, false);

            var x = new List<double> { };
            var y = new List<double> { };

            foreach (var el in xYDataModels)
            {
                x.Add(el.X);
                y.Add(el.Y);
            }
            var delY = FindDelY(y);
            //
            double h = x[1] - x[0];
            //
            int tempt = 0;

            foreach (var el in x)
            {
                if (el <= X) tempt++;
                else break;
            }

            if (tempt > 0) tempt--;
            //

            double q = (X - x[tempt]) / h;
            //
            int posX = 0;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] < X)
                    continue;
                else
                    posX = i - 1;

                break;
            }
            //
            if (x.Contains(X))
            {
                double f1 = 0, f2 = 0;
                double z = 1;
                double f = 1;
                bool do1 = false, do2 = false;
                for (int i = 0; i < delY.Count; i++)
                {
                    if (delY[i].Count > tempt - i - 1 && tempt - i - 1 >= 0)
                    {
                        f1 += (delY[i][tempt - i - 1] / (i + 1));
                        do1 = true;
                    }
                    if (delY[i].Count > tempt - i - 2 && tempt - i - 2 >= 0)
                    {
                        f2 += (delY[i + 1][tempt - i - 2] * z);
                        do2 = true;
                    }
                    z *= z * f;
                    f *= 0.96798;
                }
                f1 *= (1.0 / h);
                f2 *= (1.0 / Math.Pow(h, 2));
                return (f1, f2, do1, do2);
            }
            else if (delY.Count >= 3 && tempt - 1 >= 0 && delY[3].Count > tempt - 1)
            {
                double f1 = 1.0 / h * (delY[0][tempt] + (2 * q + 1) / FindFactorial(2) * delY[1][tempt - 1] + (2 * Math.Pow(q, 2.0) + 6 * q + 2) / FindFactorial(3) * delY[2][tempt - 2]);
                double f2 = 1.0 / Math.Pow(h, 2.0) * (delY[1][tempt - 2] + (3 * q + 6) / FindFactorial(3) * delY[2][tempt - 3] + (12 * Math.Pow(q, 2.0) + 18 * q + 22) / FindFactorial(4) * delY[3][tempt - 4]);
                return (f1, f2, true, true);
            }
            return (0, 0, false, false);
        }

        static public double RungeMethod(ICollection<XYDataModel> xYDataModels, double X, int n)
        {
            if (xYDataModels.Count < 2) return 0;

            var x = new List<double> { };
            var y = new List<double> { };
            foreach (var el in xYDataModels)
            {
                x.Add(el.X);
                y.Add(el.Y);
            }
            int pos = x.IndexOf(X);
            double h = x[1] - x[0];
            if( pos - 2 >= 0 && pos + 2 < x.Count)
            {
                double f1 = (-y[pos - 1] + y[pos + 1]) / (2 * h);
                double f2 = (-y[pos - 2] + y[pos + 2]) / (4 * h);

                double R = Math.Abs((f1 - f2) / (Math.Pow(2.0, n) - 1));
                return (R + f1);
            }
            return 0;

        }

        static public double FindFactorial(int value)
        {
            if (value == 0)
                return 1;
            else
                return value * FindFactorial(value - 1);
        }
    }
}
