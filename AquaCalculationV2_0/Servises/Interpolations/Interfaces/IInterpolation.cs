using AquaCalculationV2_0.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCalculationV2_0.Servises.Interpolations.Interfaces
{
    interface IInterpolation
    {
        double InterpolationPolynom(ICollection<XYDataModel> data, double X);
    }
}
