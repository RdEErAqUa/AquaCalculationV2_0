using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.Servises.Interpolations;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.Integrals
{
    static class IntegralMath
    {
        static private IIntegral _integral;
        static private IInterpolation _interpolation;
        static public bool SetIntegral(IIntegral integral)
        {
            try
            {
                _integral = integral;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
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
        static public double Error(ICollection<XYDataModel> data, double step, double a = 0, double b = 0) => _integral.Error(data, step, a, b);
        static public double Integral(ICollection<XYDataModel> data, double step, double a = 0, double b = 0) => _integral.Integral(data, step, a, b);
        static public double IntegralFullFillStep(ICollection<XYDataModel> data, double step, double a = 0, double b = 0) 
            => Integral(InterpolationMath.FullFill(_interpolation, data, step), step, a, b);
    }
}
