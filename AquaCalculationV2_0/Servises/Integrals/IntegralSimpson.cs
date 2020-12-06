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
            int m = data.Count / 2;

            for(int i = 0; i < m; i++)
            {

            }
            return null;
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
    }
}
