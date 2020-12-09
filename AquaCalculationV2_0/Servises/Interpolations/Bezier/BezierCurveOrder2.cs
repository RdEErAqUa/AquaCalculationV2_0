using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations.Bezier
{
    class BezierCurveOrder2 : IInterpolation
    {
        public double InterpolationPolynom(ICollection<XYDataModel> data, double X)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();

            for (int i = 0; i < x.Count - 2; i += 2)
            {
                if (x[i] <= X && X <= x[i + 2])
                {
                    double t = (X - x[i]) / (x[i + 2] - x[i]);
                    return (Math.Pow((1.0 - t), 2.0) * y[i] + 2.0 * t * (1.0 - t) * y[i + 1] + Math.Pow(t, 2.0) * y[i + 2]);
                }
            }
            return 0;
        }
    }
}
