using AquaCalculationV2_0.Models.Base;
using AquaCalculationV2_0.Models.RepresentedData.Interfaces;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models.RepresentedData
{
    class IntegralData : ModelBase, IRepresentedData<IIntegral>
    {
        #region dataValue : INumericalDifferentiation - метод интегрирования
        private IIntegral _dataValue;
        public IIntegral dataValue { get => _dataValue; set => Set(ref _dataValue, value); }
        #endregion

        #region methodName : string - название метода
        private string _methodName;
        public string methodName { get => _methodName; set => Set(ref _methodName, value); }
        #endregion
    }
}
