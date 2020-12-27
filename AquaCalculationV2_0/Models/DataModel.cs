using AquaCalculationV2_0.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AquaCalculationV2_0.Models
{
    class DataModel : ModelBase
    {
        #region XYValue : ObservableCollection<XYDataModel> - координаты точек

        private ObservableCollection<XYDataModel> _XYValue;

        public ObservableCollection<XYDataModel> XYValue { get => _XYValue; set => Set(ref _XYValue, value); }

        #endregion
    }
}
