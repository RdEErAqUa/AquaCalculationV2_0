using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models.RepresentedData.Interfaces
{
    interface IRepresentedData<T>
    {
        T dataValue { get; set; }
        string methodName { get; set; }
    }
}
