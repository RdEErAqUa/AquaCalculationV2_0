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

        public double Integral(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0)
        {
            if(a == b) { a = xYDatas.Select(X => X.X).ToList().Min(); b = xYDatas.Select(X => X.X).ToList().Max(); }
            double integralValue = 0;

            ICollection<XYDataModel> value = xYDatas.Where(x => a <= x.X && b >= x.X).ToList();

            for (int i = 0; i < value.Count - 1; i++)
                integralValue += value.ElementAt(i).Y;

            return (integralValue * step);
        }
    }
}
