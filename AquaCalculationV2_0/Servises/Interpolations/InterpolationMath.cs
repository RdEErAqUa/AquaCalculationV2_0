using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Interpolations
{
    static class InterpolationMath
    {
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
        static public double Interpolation(ICollection<XYDataModel> data, double X) => _interpolation.InterpolationPolynom(data, X);
        static public ICollection<XYDataModel> FullFill(IInterpolation interpolation, ICollection<XYDataModel> data, double step, double a = 0, double b = 0)
        {
            var x = data.Select(X => X.X).ToList();
            var y = data.Select(Y => Y.Y).ToList();
            if (a == b) { a = x.First(); b = x.Last(); }
            ICollection<XYDataModel> value = new List<XYDataModel>();
            for (; a <= b; a += step)
                value.Add(new XYDataModel { X = a, Y = interpolation.InterpolationPolynom(data, a) });
            return value;
        }
    }
}
