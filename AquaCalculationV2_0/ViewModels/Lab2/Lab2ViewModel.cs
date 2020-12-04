using AquaCalculationV2_0.Data.Differentiation;
using AquaCalculationV2_0.Infrastructure.Commands;
using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.ViewModels;
using AquaCalculationV2_0.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AquaCalculationV2_0.ViewModels.Lab2
{
    class Lab2ViewModel : ViewModel
    {
        public MainWindowViewModel MainModel { get; }
        #region XYDataValue : ObservableCollection<XYDataModel> - Узлы интерполяции

        private ObservableCollection<XYDataModel> _XYDataValue = new ObservableCollection<XYDataModel> { };

        public ObservableCollection<XYDataModel> XYDataValue { get => _XYDataValue; set => Set(ref _XYDataValue, value); }

        #endregion

        #region ProtocolValue : string - протокол ответа

        private string _ProtocolValue;

        public string ProtocolValue { get => _ProtocolValue; set => Set(ref _ProtocolValue, value); }

        #endregion

        #region XValue : double - протокол ответа

        private double _XValue;

        public double XValue { get => _XValue; set => Set(ref _XValue, value); }

        #endregion

        #region Command

        #region CalculateDifferential

        public ICommand CalculateDifferential { get; }

        private bool CanCalculateDifferentialExecute(object p)
        {
            return true;
        }
        private void OnCalculateDifferentialExecuted(object p)
        {
            double f1 = 0;
            int n = 2;
            f1 = NumericalDifferentiation.RungeMethod(XYDataValue, XValue, n);

            ProtocolValue = $"Похідна за методом Рунге(при r - {n}) - f` = {Math.Round(f1, n)};";

            //Перенести это в сервис
        }
        #endregion

        #endregion
        public Lab2ViewModel()
        {
            this.MainModel = MainModel;

            CalculateDifferential = new LambdaCommand(OnCalculateDifferentialExecuted, CanCalculateDifferentialExecute);
        }
    }
}
