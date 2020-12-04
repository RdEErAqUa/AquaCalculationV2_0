using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCalculationV2_0.Servises.Interpolation.Interfaces
{
    interface ILagrangeInterpolation
    {
        double InterpolationPolynom(List<double> x, List<double> y, double X, double Size);
    }
}
