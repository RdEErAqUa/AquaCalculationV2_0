using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.NumericalDifferentiations
{
    class NewtonSecondDifferentiation : INumericalDifferentiation
    {
        private List<List<double>> FindDelY(List<double> y, int power = 0)
        {
            List<List<double>> DelY = new List<List<double>> { };
            if (power < 1 && power > y.Count - 1) power = y.Count - 1;
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
        public double? NumericalDiffertiationRun(ICollection<XYDataModel> DataValue, double X)
        {
            if (DataValue.Count < 2) return 0;
            var x = DataValue.Select(X => X.X).ToList();
            var y = DataValue.Select(Y => Y.Y).ToList();
            var delY = FindDelY(y, 5);
            double h = x[1] - x[0];
            int tempt = 0;
            foreach (var el in x)
            {
                if (el <= X) tempt++;
                else break;
            }

            if (tempt > 0) tempt--;

            double q = (X - x[tempt]) / h;
            int posX = 0;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] < X)
                    continue;
                else
                    posX = i - 1;

                break;
            }
            if (x.First() != X && x[1] != X)
            {
                double f1 = 0;
                double z = 1;
                double f = 1;
                for (int i = 0; i < delY.Count; i++)
                {
                    if (delY[i].Count > tempt - i - 1 && tempt - i - 1 >= 0)
                    {
                        f1 += (delY[i][tempt - i - 1] / (i + 1));
                    }
                    z *= z * f;
                    f *= 0.96798;
                }
                f1 *= (1.0 / h);
                return f1;
            }
            return null;
        }

        public double Error(ICollection<XYDataModel> DataValue, double step)
        {
            throw new NotImplementedException();
        }
    }
}
