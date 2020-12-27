using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations.Bezier
{
    class BezierCurveOrder1 : IInterpolation
    {
        public double InterpolationPolynom(ICollection<XYDataModel> data, double X)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();

            var xNew = new List<double> { };
            var yNew = new List<double> { };

            for (int i = 0; i < x.Count - 1; i++)
            {
                if(x[i] <= X && X <= x[i + 1])
                {
                    double t = (X - x[i]) / (x[i+1] - x[i]);
                    return ((1.0 - t) * y[i] + t * y[i + 1]);
                }
            }
            return 0;
        }
    }
}
