using AquaCalculationV2_0.Infrastructure.Commands;
using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using org.mariuszgromada.math.mxparser;
using System.Threading.Tasks;
using AquaCalculationV2_0.Servises;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.Servises.Integrals;
using Microsoft.Extensions.DependencyInjection;
using AquaCalculationV2_0.Servises.NumericalDifferentiations;
using AquaCalculationV2_0.Servises.NumericalDifferentiations.Interfaces;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using AquaCalculationV2_0.Servises.Interpolations;

namespace AquaCalculationV2_0.ViewModels.Lab3
{
    class Lab3ViewModel : ViewModel
    {

        #region IntegralValue : IntegralModel - функція, та деякі параметри інтеграла, який знаходиться.

        private IntegralModel _IntegralValue;

        public IntegralModel IntegralValue { get => _IntegralValue; set => Set(ref _IntegralValue, value); }

        #endregion

        #region IntegralCalculated : IntegralCalculated - значение интеграла

        private double _IntegralCalculated;

        public double IntegralCalculated { get => _IntegralCalculated; set => Set(ref _IntegralCalculated, value); }

        #endregion

        #region A : A - интеграл от A  до B  - это значение A

        private double _A;

        public double A { get => _A; set => Set(ref _A, value); }

        #endregion

        #region B : B - A : A - интеграл от A  до B  - это значение B.

        private double _B;

        public double B { get => _B; set => Set(ref _B, value); }

        #endregion

        #region E : E - если E >= 1 - количество вузлов для нахождения интеграла, 0 < E < 1 - точность интегрирования.

        private double _E = 0.1; 

        public double E { get => _E; set => Set(ref _E, value); }

        #endregion

        #region IntegralRun -  посчитать интеграл, ассинхронная команда

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set => Set(ref _isBusy, value);
        }

        public ICommand IntegralRun { get; }
        private async Task OnIntegralRunExecuted(object p)
        {
            try
            {
                IsBusy = true;
                var x = IntegralValue.XYValue.Select(X => X.X).ToList();
                var y = IntegralValue.XYValue.Select(Y => Y.Y).ToList();
                await Task.Run(() => {
                });
                
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanIntegralRunExecute(object p)
        {
            return !IsBusy;
        }
        #endregion

        #region FormulaValue : String - производная функции для подсеча ошибки
        private string _FormulaValue;

        public string FormulaValue { get => _FormulaValue; set => Set(ref _FormulaValue, value); }

        #endregion
        public Lab3ViewModel()
        {
            IntegralRun = new AsyncLambdaCommand(OnIntegralRunExecuted, CanIntegralRunExecute);
            IntegralValue = new IntegralModel { XYValue = new ObservableCollection<XYDataModel> { } };
        }
    }
}
