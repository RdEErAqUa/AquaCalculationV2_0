using AquaCalculationV2_0.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCalculationV2_0.Models
{
    class XYDataModel : ModelBase
    {
        #region X : double - значение координаты по X
        private double _X;
        public double X { get => _X; set => Set(ref _X, value); }
        #endregion
        #region Y : double - значение координаты по Y
        private double _Y;
        public double Y { get => _Y; set => Set(ref _Y, value); }
        #endregion
    }
}
