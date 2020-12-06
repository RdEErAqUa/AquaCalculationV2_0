using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.Servises.NumericalDifferentiations;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AquaCalculationV2_0.Servises.Integrals
{
    class IntegralTrapezoid : IIntegral
    {
        public double Error(ICollection<XYDataModel> data, double step, double a, double b) => (Math.Pow(step, 2.0) * (b - a) / 12.0) * DifferentiationMath.DifferentiationMaxInNodes(data, 2);

        public double Integral(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            if (a == b) { a = xYDatas.Select(X => X.X).ToList().Min(); b = xYDatas.Select(X => X.X).ToList().Max(); }
            double integralValue = 0;

            ICollection<XYDataModel> value = xYDatas.Where(x => a <= x.X && b >= x.X).ToList();

            for (int i = 0; i < value.Count; i++)
                integralValue = i == 0 || i == value.Count - 1 ? integralValue + value.ElementAt(i).Y / 2.0 : integralValue + value.ElementAt(i).Y;

            return (integralValue * step);
        }
    }
}
