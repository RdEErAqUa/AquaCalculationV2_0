using AquaCalculationV2_0.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models
{
    class DataModelChecked : ModelBase
    {
        #region XYValue : ObservableCollection<XYDataModel> - координаты точек

        private ObservableCollection<XYDataModelIsChecked> _XYValue;

        public ObservableCollection<XYDataModelIsChecked> XYValue { get => _XYValue; set => Set(ref _XYValue, value); }

        #endregion
    }
}
