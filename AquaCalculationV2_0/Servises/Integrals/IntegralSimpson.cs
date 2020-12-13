using AquaCalculationV2_0.Models;
using System.Collections.Generic;
using System.Linq;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using System;
using AquaCalculationV2_0.Servises.NumericalDifferentiations;

namespace AquaCalculationV2_0.Servises.Integrals
{
    class IntegralSimpson : IIntegral
    {
        public double Error(ICollection<XYDataModel> data, double step, double a, double b) => (Math.Pow(step, 4.0) * (b - a) / 180.0) * DifferentiationMath.DifferentiationMaxInNodes(data, 4);

        public ICollection<XYDataModel> Function(ICollection<XYDataModel> data)
        {
            int m = data.Count % 2 == 0 ? data.Count / 2 : data.Count / 2 + 1;

            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();

            double step = (data.Last().X - data.First().X) / data.Count;

            var answerValue = new List<XYDataModel> { };

            for(int i = 1; i < m; i++)
            {
                double A = (y[2 * i - 2] - 2 * y[2 * i - 1] + y[2 * i]) / (2.0 * Math.Pow(step, 2.0));
                double B = (y[2 * i] - y[2 * i - 2])/(2.0 * step);
                double C = y[2 * i - 1];

                foreach (var el in x) {
                    if (x[2 * i - 2] <= el && x[2 * i] >= el) {
                        double yValue = A * Math.Pow(el - x[2 * i - 1], 2.0) + B * (el - x[2 * i - 1]) + C;
                        answerValue.Add(new XYDataModel { X = el, Y = yValue});
                    }
                }

            }
            return answerValue;
        }

        public double Integral(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            if (a == b) { a = xYDatas.Select(X => X.X).ToList().Min(); b = xYDatas.Select(X => X.X).ToList().Max(); }
            double integralValue = 0;

            ICollection<XYDataModel> value = xYDatas.Where(x => a <= x.X && b >= x.X).ToList();

            for (int i = 0; i < value.Count; i++)
                integralValue = (i == 0 || i == value.Count - 1 ? integralValue + value.ElementAt(i).Y: 
                    ( i % 2 == 0  ? (integralValue + 2 * xYDatas.ElementAt(i).Y) : (integralValue + 4 * value.ElementAt(i).Y)));

            return (integralValue * (step / 3.0));
        }

        public string ProtocolBuild(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            String Protocol = $"Метод Симпсона: При шаге интегрирования - {step}, та интервале интегрирования  - {{{a},{b}}} интеграл - ";

            if (a == b) { a = xYDatas.Select(X => X.X).ToList().Min(); b = xYDatas.Select(X => X.X).ToList().Max(); }
            double integralValue = 0;

            ICollection<XYDataModel> value = xYDatas.Where(x => a <= x.X && b >= x.X).ToList();

            for (int i = 0; i < value.Count; i++)
                integralValue = (i == 0 || i == value.Count - 1 ? integralValue + value.ElementAt(i).Y :
                    (i % 2 == 0 ? (integralValue + 2 * xYDatas.ElementAt(i).Y) : (integralValue + 4 * value.ElementAt(i).Y)));
            integralValue *= (step / 3.0);

            Protocol += $"{integralValue}\n";

            return Protocol;
        }
    }
}
