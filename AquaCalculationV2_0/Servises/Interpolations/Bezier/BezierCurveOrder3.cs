using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations.Bezier
{
    class BezierCurveOrder3 : IInterpolation
    {
        public double InterpolationPolynom(ICollection<XYDataModel> data, double X)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();

            for (int i = 0; i < x.Count - 3; i += 3)
            {
                if (x[i] <= X && X <= x[i + 3])
                {
                    double t = (X - x[i]) / (x[i + 3] - x[i]);
                    return (Math.Pow((1.0 - t), 3.0) * y[i] + 3.0 * t * Math.Pow((1.0 - t), 2.0) * y[i + 1] + 2 * Math.Pow(t, 2.0) * (1.0 - t) * y[i + 2]) + Math.Pow(t, 3.0) * y[i + 3];
                }
            }
            return 0;
        }
    }
}
