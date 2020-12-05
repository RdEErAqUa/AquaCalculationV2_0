using AquaCalculationV2_0.Models;
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

        static public double? Differentiation(ICollection<XYDataModel> dataValue, double xValue)
        {
            return _differentiation.NumericalDiffertiationRun(dataValue, xValue);
        }
    }
}
