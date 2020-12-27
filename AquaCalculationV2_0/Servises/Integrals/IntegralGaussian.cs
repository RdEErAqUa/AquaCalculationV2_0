using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.Servises.Interpolations;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Integrals
{
    class IntegralGaussian : IIntegral
    {
        public double Error(ICollection<XYDataModel> data, double step, double a, double b) => 0;

        public ICollection<XYDataModel> Function(ICollection<XYDataModel> data)
        {
            return null;
        }

        public double Integral(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            int n = (int)step >= 1 && (int)step <= 7 ? (int)step : 2;

            IntegralGaussian.MakeQuadratureCoefficients(n);

            IInterpolation interpolation = new CubicSplineInterpolation();

            double f = 0;

            for (int i = 0; i < n; i++)
            {
                (double, double) el = i <= QuadratureElementValue.Count - 1 ? QuadratureElementValue[i] : (QuadratureElementValue[n - i - 1].Item1, -QuadratureElementValue[n - i - 1].Item2);


                double Ei = (b + a) / 2.0 + ((b - a) / 2.0) * el.Item2;

                double Yi = interpolation.InterpolationPolynom(xYDatas, Ei);

                f += el.Item1 * Yi;
            }

            f *= ((b - a) / 2.0);

            return f;
        }

        private static List<(double, double)> QuadratureElementValue;
        private static int n;
        private static void MakeQuadratureCoefficients(int n)
        {
            IntegralGaussian.n = n;
            QuadratureElementValue = new List<(double, double)> { };
            switch (n)
            {
                case 1:
                    QuadratureElementValue = new List<(double, double)> {(2, 0.5) };
                    break;
                case 2:
                    QuadratureElementValue = new List<(double, double)> { (1, -0.577350269) };
                    break;
                case 3:
                    QuadratureElementValue = new List<(double, double)> { (0.555555556, -0.774596669), (0.888888889, 0) };
                    break;
                case 4:
                    QuadratureElementValue = new List<(double, double)> { (0.347854845, -0.861136312), (0.652145155, -0.339981044) };
                    break;
                case 5:
                    QuadratureElementValue = new List<(double, double)> { (0.236926885, -0.906179846), (0.478628670, -0.538469310), (0.568888889, 0) };
                    break;
                case 6:
                    QuadratureElementValue = new List<(double, double)> { (0.171324492, -0.932469514), (0.360761573, -0.661209386), (0.467913935, -0.238619186) };
                    break;
                case 7:
                    QuadratureElementValue = new List<(double, double)> { (0.129485, -0.949108), (0.279705, -0.741531), (0.38183, -0.405845), (0.41796, 0) };
                    break;
            }
        }

        public string ProtocolBuild(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            string Protocol = $"При n = {step}\nwi      |      xi      |      Ei      |      f(Ei)      |      wi * f(Ei)      |      Сума";

            //
            int n = (int)step >= 1 && (int)step <= 7 ? (int)step : 2;

            IntegralGaussian.MakeQuadratureCoefficients(n);

            IInterpolation interpolation = new CubicSplineInterpolation();

            double f = 0;

            for (int i = 0; i < n; i++)
            {
                (double, double) el = i <= QuadratureElementValue.Count - 1 ? QuadratureElementValue[i] : (QuadratureElementValue[n - i - 1].Item1, -QuadratureElementValue[n - i - 1].Item2);

                double Ei = (b + a) / 2.0 + ((b - a) / 2.0) * el.Item2;

                double Yi = interpolation.InterpolationPolynom(xYDatas, Ei);

                f += el.Item1 * Yi;

                Protocol += $"\n{el.Item1}      |      {el.Item2}      |      {Ei}      |      {Yi}      |      {el.Item1 * Yi}      |      {f}";
            }

            f *= ((b - a) / 2.0);

            Protocol += $"\nИнтеграл = {f} при " + "{a, b} " + $"{{{a},{b}}}";
            //
            return Protocol;

        }
    }
}
