using AquaCalculationV2_0.Models.Base;
using AquaCalculationV2_0.Models.RepresentedData.Interfaces;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models.RepresentedData
{
    class DifferentiationData : ModelBase, IRepresentedData<INumericalDifferentiation>
    {
        #region dataValue : INumericalDifferentiation - метод числового дифференцеирования
        private INumericalDifferentiation _dataValue;
        public INumericalDifferentiation dataValue { get => _dataValue; set => Set(ref _dataValue, value); }
        #endregion

        #region methodName : string - название метода
        private string _methodName;
        public string methodName { get => _methodName; set => Set(ref _methodName, value); }
        #endregion
    }
}
