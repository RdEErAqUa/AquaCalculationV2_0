using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.Servises.Interpolations;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using AquaCalculationV2_0.Servises.NumericalDifferentiations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Integrals
{
    class IntegralChebishev : IIntegral
    {
        public double Error(ICollection<XYDataModel> data, double step, double a, double b) => 0;

        public ICollection<XYDataModel> Function(ICollection<XYDataModel> data)
        {
            return null;
        }

        public double Integral(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            int n = (int)step >= 1 && (int)step <= 7 ? (int)step : 2;

            IntegralChebishev.MakeQuadratureCoefficients(n);

            IInterpolation interpolation = new CubicSplineInterpolation();

            double f = 0;

            for (int i = 0; i < n; i++)
            {
                double el = i <= QuadratureElementValue.Count - 1 ? QuadratureElementValue[i] : -QuadratureElementValue[n - i - 1];


                double Ei = (b + a) / 2.0 + ((b - a) / 2.0) * el;

                double Yi = interpolation.InterpolationPolynom(xYDatas, Ei);

                f += Yi;
            }

            f *= ((b - a) / n);

            return f;
        }

        private static List<double> QuadratureElementValue;
        private static int n;
        private static void MakeQuadratureCoefficients(int n)
        {
            IntegralChebishev.n = n;
            QuadratureElementValue = new List<double> { };
            switch (n)
            {
                case 2:
                    QuadratureElementValue = new List<double> { -0.57735 };
                    break;
                case 3:
                    QuadratureElementValue = new List<double> { -0.707107, 0 };
                    break;
                case 4:
                    QuadratureElementValue = new List<double> { -0.794654, -0.187592 };
                    break;
                case 5:
                    QuadratureElementValue = new List<double> { -0.832498, -0.374541, 0 };
                    break;
                case 6:
                    QuadratureElementValue = new List<double> { -0.866247, -0.422519, -0.266635 };
                    break;
                case 7:
                    QuadratureElementValue = new List<double> { -0.883862, -0.529657, -0.323912, 0 };
                    break;
            }
        }

        public string ProtocolBuild(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            string Protocol = $"При n = {step}\nwi      |      xi      |      Ei      |      f(Ei)      |      wi * f(Ei)      |      Сума";

            //
            int n = (int)step >= 1 && (int)step <= 7 ? (int)step : 2;

            IntegralChebishev.MakeQuadratureCoefficients(n);

            IInterpolation interpolation = new CubicSplineInterpolation();

            double f = 0;

            for (int i = 0; i < n; i++)
            {
                double el = i <= QuadratureElementValue.Count - 1 ? QuadratureElementValue[i] : -QuadratureElementValue[n - i - 1];

                double Ei = (b + a) / 2.0 + ((b - a) / 2.0) * el;

                double Yi = interpolation.InterpolationPolynom(xYDatas, Ei);

                f += Yi;

                Protocol += $"\n{el}     |      {Ei}      |      {Yi}      |         {f}";
            }

            f *= ((b - a) / n);

            Protocol += $"\nИнтеграл = {f} при " + "{a, b} " + $"{{{a},{b}}}";
            //
            return Protocol;

        }
    }
}
