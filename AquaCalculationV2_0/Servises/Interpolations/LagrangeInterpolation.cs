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
                double value = y[i];

                double upperValue = 1;
                double lowerValue = 1;

                for(int j = 0; j < Size; j++)
                {
                    if(i != j) 
                    { 
                        upperValue *= (X - x[j]);
                        lowerValue *= (x[i] - x[j]);
                    }
                }
                answer += (value * upperValue / lowerValue);
            }

            return answer;
        }
    }
}
