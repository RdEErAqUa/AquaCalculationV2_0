using AquaCalculationV2_0.Data.Differentiation;
using AquaCalculationV2_0.Infrastructure.Commands;
using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.ViewModels;
using AquaCalculationV2_0.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AquaCalculationV2_0.ViewModels.Lab1
{
    internal class Lab1ViewModel : ViewModel
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
            double f1 = 0, f2 = 0;
            bool isFirst = true, isSecond = true;
            (f1, f2, isFirst, isSecond) = NumericalDifferentiation.FindDifferentiationFirstNewtonFormula(XYDataValue, XValue);

            ProtocolValue = (isFirst && isSecond) ? $"Похідна за першим многочленом Ньютона - f` = {Math.Round(f1, 3)}; f`` = {Math.Round(f2, 3)}" :
                isFirst ? $"Похідна за першим многочленом Ньютона - f` - {Math.Round(f1, 3)} f`` - нехватає даних" :
                isSecond ? $"Похідна за першим многочленом Ньютона - f` - нехватає даних; f`` = {Math.Round(f2, 3)}" :
                $"\nПохідна за першим многочленом Ньютона неможливо обчислити";

            (f1, f2, isFirst, isSecond) = NumericalDifferentiation.FindDifferentiationSecondNewtonFormula(XYDataValue, XValue);


            ProtocolValue += (isFirst && isSecond) ? $"\nПохідна за другим многочленом Ньютона - f` = {Math.Round(f1, 3)}; f`` = {Math.Round(f2, 3)}" :
                isFirst ? $"\nПохідна за другим многочленом Ньютона - f` = {Math.Round(f1, 3)}; f`` - нехватає даних" :
                isSecond ? $"\nПохідна за другим многочленом Ньютона - f` - нехватає даних; f`` = {Math.Round(f2, 3)}" :
                $"\nПохідна за другим многочленом Ньютона неможливо обчислити";
            //Перенести это в сервис
        }
        #endregion

        #endregion

        public Lab1ViewModel()
        {
            this.MainModel = MainModel;

            CalculateDifferential = new LambdaCommand(OnCalculateDifferentialExecuted, CanCalculateDifferentialExecute);
        }
    }
}
