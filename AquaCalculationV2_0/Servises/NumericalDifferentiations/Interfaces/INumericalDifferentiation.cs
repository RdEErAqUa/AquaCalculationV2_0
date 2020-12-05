using AquaCalculationV2_0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces
{
    interface INumericalDifferentiation
    {
        //Only 2-d differtiation
        double? NumericalDiffertiationRun(ICollection<XYDataModel> DataValue, double X); 
    }
}
