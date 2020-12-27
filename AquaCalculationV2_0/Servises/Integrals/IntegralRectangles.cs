using AquaCalculationV2_0.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using AquaCalculationV2_0.Servises.NumericalDifferentiations;

namespace AquaCalculationV2_0.Servises.Integrals
{
    class IntegralRectangles : IIntegral
    {
        public double Error(ICollection<XYDataModel> data, double step, double a, double b) => (step * (b - a) / 2.0) * DifferentiationMath.DifferentiationMaxInNodes(data, 1);

        public ICollection<XYDataModel> Function(ICollection<XYDataModel> data)
        {
            ICollection<XYDataModel> returnValue = new List<XYDataModel> { };

            for(int i = 0; i < data.Count; i++)
            {
                if (i != 0 && i != data.Count - 1)
                {
                    returnValue.Add(new XYDataModel { X = data.ElementAt(i).X, Y = data.ElementAt(i - 1).Y });
                    returnValue.Add(new XYDataModel { X = data.ElementAt(i).X, Y = data.ElementAt(i).Y });
                }
                else if(i == 0)
                {
                    returnValue.Add(new XYDataModel { X = data.ElementAt(i).X, Y = data.ElementAt(i).Y });
                }
                else
                {
                    returnValue.Add(new XYDataModel { X = data.ElementAt(i).X, Y = data.ElementAt(i - 1).Y });
                }
            }

            return returnValue;
        }

        public double Integral(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            if(a == b) { a = xYDatas.Select(X => X.X).ToList().Min(); b = xYDatas.Select(X => X.X).ToList().Max(); }
            double integralValue = 0;

            ICollection<XYDataModel> value = xYDatas.Where(x => a <= x.X && b >= x.X).ToList();

            for (int i = 0; i < value.Count - 1; i++)
                integralValue += value.ElementAt(i).Y;

            return (integralValue * step);
        }

        public string ProtocolBuild(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            String Protocol = $"Метод прямоугольников: При шаге интегрирования - {step}, та интервале интегрирования  - {{{a},{b}}} интеграл - ";

            if (a == b) { a = xYDatas.Select(X => X.X).ToList().Min(); b = xYDatas.Select(X => X.X).ToList().Max(); }
            double integralValue = 0;

            ICollection<XYDataModel> value = xYDatas.Where(x => a <= x.X && b >= x.X).ToList();

            for (int i = 0; i < value.Count - 1; i++)
                integralValue += value.ElementAt(i).Y;
            integralValue *= step;

            Protocol += $"{integralValue}\n";

            return Protocol;
        }
    }
}
