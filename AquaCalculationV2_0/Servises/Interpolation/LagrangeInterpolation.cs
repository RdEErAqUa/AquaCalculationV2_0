using AquaCalculationV2_0.Servises.Interpolation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCalculationV2_0.Servises.Interpolation
{
    class LagrangeInterpolation : ILagrangeInterpolation
    {
        public double InterpolationPolynom(List<double> x, List<double> y, double X, double Size)
        {
            double answer = 0;

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
