using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.NumericalDifferentiations
{
    class LagrangeDifferentiation : INumericalDifferentiation
    {
        public double Error(ICollection<XYDataModel> DataValue, double step)
        {
            throw new NotImplementedException();
        }

        public double? NumericalDiffertiationRun(ICollection<XYDataModel> DataValue, double X)
        {
            throw new NotImplementedException();
        }
    }
}
