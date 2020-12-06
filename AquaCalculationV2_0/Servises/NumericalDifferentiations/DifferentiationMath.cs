using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.NumericalDifferentiations
{
    static class DifferentiationMath
    {
        static private INumericalDifferentiation _differentiation;
        static private IInterpolation _interpolation;
        static public bool SetInterpolation(IInterpolation interpolation)
        {
            try
            {
                _interpolation = interpolation;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static public bool SetNumerecialDifferentiation(INumericalDifferentiation differentiation)
        {
            try
            {
                _differentiation = differentiation;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static public double DifferentiationMaxInNodes(ICollection<XYDataModel> dataValue, int degree = 1) => DifferentiationForEveryNode(dataValue, degree).Select(X => X.Y).Max();
        static public double DifferentiationMinInNodes(ICollection<XYDataModel> dataValue, int degree = 1) => DifferentiationForEveryNode(dataValue, degree).Select(X => X.Y).Min();
        static public ICollection<XYDataModel> DifferentiationForEveryNode(ICollection<XYDataModel> dataValue, int degree = 1)
        {
            ICollection<XYDataModel> DifferentiationValue = new List<XYDataModel>(dataValue);

            for (int i = 0; i < degree; i++)
            {
                ICollection<XYDataModel> tempValue = new List<XYDataModel>();
                foreach (var el in DifferentiationValue)
                {
                    double? yValue = Differentiation(DifferentiationValue, el.X);
                    if (yValue != null)
                        tempValue.Add(new XYDataModel { X = el.X, Y = yValue.Value });
                }
                DifferentiationValue = tempValue;
            }
            return DifferentiationValue;
        }

        static public double? DifferentiationWithRunge(ICollection<XYDataModel> dataValue, double xValue, double E = 0.01)
        {
            double ErrorValue = 1;

            double valueH = 0, valueH2 = 0;

            double step = (dataValue.Last().X - dataValue.First().X) / dataValue.Count;

            while(ErrorValue > E)
            {
                ICollection<XYDataModel> tempValue = new List<XYDataModel> { };

                for (double a = dataValue.First().X; a <= dataValue.Last().X; a += step)
                    tempValue.Add(new XYDataModel { X = a, Y = Math.Round(_interpolation.InterpolationPolynom(dataValue, a), 15) });

                valueH = valueH2;

                valueH2 = Differentiation(tempValue, xValue).Value;

                ErrorValue = Math.Abs(valueH2 - valueH);

                step /= 2.0;
            }

            return valueH2;
        }
        static public double? Differentiation(ICollection<XYDataModel> dataValue, double xValue)
        {
            return _differentiation.NumericalDiffertiationRun(dataValue, xValue);
        }
    }
}
