using AquaCalculationV2_0.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models
{
    class XYDataModelIsChecked : ModelBase
    {
        #region X : double - значение координаты по X
        private double _X;
        public double X { get => _X; set => Set(ref _X, value); }
        #endregion
        #region Y : double - значение координаты по Y
        private double _Y;
        public double Y { get => _Y; set => Set(ref _Y, value); }
        #endregion
        #region IsChecked : bool

        private bool _IsChecked;

        public bool IsChecked { get => _IsChecked; set => Set(ref _IsChecked, value); }

        #endregion

    }
}
