using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations
{
    class TrigonometricInterpolation : IInterpolation
    {
        public double InterpolationPolynom(ICollection<XYDataModel> data, double X)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();
            double AZeroValue = 0;

            int NValue = (data.Count - 1) / 2;

            var ABValue = new List<XYDataModel> { };

            foreach (var el in y)
                AZeroValue += el;
            AZeroValue *= (1.0 / (2.0 * NValue + 1.0));

            for (double i = 1; i <= NValue; i++)
            {
                double a = 0;
                double b = 0;

                foreach (var el in data)
                {
                    a += (el.Y * Math.Cos(el.X * (i)));
                    b += (el.Y * Math.Sin(el.X * (i)));
                }
                a *= (2.0 / (NValue * 2.0 + 1.0));
                b *= (2.0 / (NValue * 2.0 + 1.0));
                ABValue.Add(new XYDataModel { X = a, Y = b });
            }

            double answer = AZeroValue;

            for (int i = 1; i <= NValue; i++)
            {
                answer += (ABValue.ElementAt(i - 1).X * Math.Cos(i * X) + ABValue.ElementAt(i - 1).Y * Math.Sin(i * X));
            }


            return answer;
        }
    }
}
