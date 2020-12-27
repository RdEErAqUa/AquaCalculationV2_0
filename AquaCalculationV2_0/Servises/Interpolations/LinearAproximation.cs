using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations
{
    class LinearAproximation : IInterpolation
    {
        public double InterpolationPolynom(ICollection<XYDataModel> data, double X)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();

            double SX = 0, SXX = 0, SY = 0, SXY = 0;

            for (int i = 0; i < x.Count; i++)
            {
                SX += x[i];
                SXX += Math.Pow(x[i], 2);
                SY += y[i];
                SXY += (y[i] * x[i]);
            }
            double delta = (x.Count) * SXX - Math.Pow(SX, 2);
            double deltaA = (x.Count) * SXY - SX * SY;
            double deltaB = SXX * SY - SX * SXY;

            double a = deltaA / delta;
            double b = deltaB / delta;

            return a * X + b;
        }
    }
}
