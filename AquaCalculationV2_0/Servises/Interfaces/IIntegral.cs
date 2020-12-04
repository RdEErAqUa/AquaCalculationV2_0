using AquaCalculationV2_0.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCalculationV2_0.Servises.Interfaces
{
    interface IIntegral
    {
        double IntegralRunWithStep(IntegralModel integralModels, double a, double b, double step, string Formula);

        double IntegralRun(IntegralModel integralModels, double a, double b, double E, string Formula);

        double Integral(ICollection<XYDataModel> xYDatas, double step);

        ICollection<XYDataModel> FullfillWithStep(ICollection<XYDataModel> DataValue, double step, double a, double b);
    }
}
