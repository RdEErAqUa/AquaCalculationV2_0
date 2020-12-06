﻿using AquaCalculationV2_0.Models;
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
        static public double? IntegralWithError(ICollection<XYDataModel> dataValue, double a = 0, double b = 0, double E = 0.01)
        {
            double ErrorValue = 1;

            if (a >= b) { a = dataValue.Select(X => X.X).ToList().Min(); b = dataValue.Select(X => X.X).ToList().Max(); }

            double step = (b - a) / dataValue.Count;

            ICollection<XYDataModel> tempValue = new List<XYDataModel> { };
            
            while (ErrorValue > E)
            {
                step /= 2.0;
                tempValue = new List<XYDataModel> { };

                for (double i = a; i <= b; i += step)
                    tempValue.Add(new XYDataModel { X = i, Y = Math.Round(_interpolation.InterpolationPolynom(dataValue, i), 15) });

                ErrorValue = Error(tempValue, step, a, b);
            }

            return Integral(tempValue, step, a, b);
        }
        static public double? IntegralWithRunge(ICollection<XYDataModel> dataValue,  double a = 0, double b = 0 ,double E = 0.01)
        {
            double ErrorValue = 1;

            double valueH = 0, valueH2 = 0;

            if(a >= b) { a = dataValue.Select(X => X.X).ToList().Min(); b = dataValue.Select(X => X.X).ToList().Max() ; }

            double step = (b - a) / dataValue.Count;

            while (ErrorValue > E)
            {
                ICollection<XYDataModel> tempValue = new List<XYDataModel> { };

                for (double i = a; i <= b; i += step)
                    tempValue.Add(new XYDataModel { X = i, Y = Math.Round(_interpolation.InterpolationPolynom(dataValue, i), 15) });

                valueH = valueH2;

                valueH2 = Integral(tempValue, step, a, b);

                ErrorValue = Math.Abs(valueH2 - valueH);

                step /= 2.0;
            }

            return valueH2;
        }
        static public double Error(ICollection<XYDataModel> data, double step, double a = 0, double b = 0) => _integral.Error(data, step, a, b);
        static public double Integral(ICollection<XYDataModel> data, double step, double a = 0, double b = 0) => _integral.Integral(data, step, a, b);
        static public double IntegralFullFillStep(ICollection<XYDataModel> data, double step, double a = 0, double b = 0) 
            => Integral(InterpolationMath.FullFill(_interpolation, data, step), step, a, b);
    }
}