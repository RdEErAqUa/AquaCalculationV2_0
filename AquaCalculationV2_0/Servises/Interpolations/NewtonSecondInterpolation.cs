using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations
{
    class NewtonSecondInterpolation : IInterpolation
    {
        public List<List<double>> FindDelY(List<double> y, int power)
        {
            List<List<double>> DelY = new List<List<double>> { };

            for (int i = 0; i < power; i++)
            {
                List<double> delY = new List<double> { };

                for (int z = 0; z < y.Count - i - 1; z++)
                {
                    if (i == 0)
                        delY.Add(y[z + 1] - y[z]);
                    else
                        delY.Add(DelY[i - 1][z + 1] - DelY[i - 1][z]);
                }

                DelY.Add(delY);

            }

            return DelY;

        }

        public double InterpolationPolynom(ICollection<XYDataModel> data, double X)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();
            int power = y.Count > 6 ? 5 : y.Count - 1;
            double h = 0;
            if (x.Count > 1)
                h = x[1] - x[0];
            else
                return 0;

            int tempt = 0;

            foreach (var el in x)
            {
                if (el <= X) tempt++;
                else break;
            }

            if ((x.Count == tempt)) tempt--;
            double t = 1;

            List<List<double>> delY = FindDelY(y, power);


            int n = delY.Count;

            double P = y[tempt];

            for (int i = 0; i < delY.Count; i++)
            {
                t *= (((X - x[tempt]) / h) + i);
                if (delY[i].Count > tempt - i - 1 && tempt - i - 1 >= 0)
                {
                    P += t * (delY[i][tempt - i - 1] / (FindFactorial(i)));
                }
            }

            return P;
        }

        public double FindFactorial(int value)
        {
            if (value == 0)
                return 1;
            else
                return value * FindFactorial(value - 1);
        }
    }
}
