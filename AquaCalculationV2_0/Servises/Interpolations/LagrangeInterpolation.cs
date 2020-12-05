using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquaCalculationV2_0.Servises.Interpolations
{
    class LagrangeInterpolation : IInterpolation
    {
        public double InterpolationPolynom(ICollection<XYDataModel> data, double X)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();

            double answer = 0;

            double Size = x.Count;

            for (int i = 0; i < Size; i++)
            {
                double temp1 = 1;
                for (int z = 0; z < Size; z++)
                {
                    if (z == i)
                        continue;
                    else
                        temp1 *= (X - x[z]);
                }
                double temp2 = 1;
                for (int z = 0; z < Size; z++)
                {
                    if (z == i)
                        continue;
                    else
                        temp2 *= (x[i] - x[z]);
                }

                double temp3 = temp1 / temp2;
                temp3 *= y[i];

                answer += temp3;
            }

            return answer;
        }
    }
}
