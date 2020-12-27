using AquaCalculationV2_0.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCalculationV2_0.Servises.Integrals.Interfaces
{
    interface IIntegral
    {
        double Integral(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0);

        double Error(ICollection<XYDataModel> data, double step, double a, double b);

        public ICollection<XYDataModel> Function(ICollection<XYDataModel> data);

        public String ProtocolBuild(ICollection<XYDataModel> xYDatas, double step, double a = 0, double b = 0);
    }
}
