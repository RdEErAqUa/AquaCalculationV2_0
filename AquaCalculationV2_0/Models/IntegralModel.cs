using AquaCalculationV2_0.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AquaCalculationV2_0.Models
{
    class IntegralModel : ModelBase
    {
        #region H : double - значение шага интегрирование

        private double _H;

        public double H { get => _H; set => Set(ref _H, value); }

        #endregion

        #region XYValue : ObservableCollection<XYDataModel> - координаты точек, в котором происходит интегрирование

        private ObservableCollection<XYDataModel> _XYValue;

        public ObservableCollection<XYDataModel> XYValue { get => _XYValue; set => Set(ref _XYValue, value); }

        #endregion

        #region XYBuild : ObservableCollection<XYDataModel> - дополнительные координаты точок, с шагом H

        private ObservableCollection<XYDataModel> _XYBuild;

        public ObservableCollection<XYDataModel> XYBuild { get => _XYBuild; set => Set(ref _XYBuild, value); }

        #endregion
    }
}
