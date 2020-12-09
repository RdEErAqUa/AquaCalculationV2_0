using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations
{
    class NewtonFirstInterpolation : IInterpolation
    {
        private List<List<double>> FindDelY(List<double> y, int power)
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
            double h = 0;
            int power = y.Count > 6 ? 5 : y.Count - 1;
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

            if (tempt > 0) tempt--;


            double t = 1;

            List<List<double>> delY = FindDelY(y, power);

            double P = y[tempt];

            for (int i = 0; i < delY.Count; i++)
            {
                t *= (((X - x[tempt]) / h) - i);
                if (delY[i].Count > tempt)
                {
                    P += ((t * delY[i][tempt]) / FindFactorial(i + 1));
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
