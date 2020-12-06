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
using AquaCalculationV2_0.Models.RepresentedData;

namespace AquaCalculationV2_0.ViewModels.Lab3
{
    class Lab3ViewModel : ViewModel
    {

        #region DataValue : DataValue - задані дані функції

        private DataModel _DataValue;

        public DataModel DataValue { get => _DataValue; set => Set(ref _DataValue, value); }

        #endregion

        #region NeedInterpolationValue : double - в какой точке нужно найти интерполяцию

        private double _NeedInterpolationValue;

        public double NeedInterpolationValue { get => _NeedInterpolationValue; set => Set(ref _NeedInterpolationValue, value); }

        #endregion

        #region NeedInterpolationStep : double - с каким шагом нужно найти интерполяцию

        private double _NeedInterpolationStep;

        public double NeedInterpolationStep { get => _NeedInterpolationStep; set => Set(ref _NeedInterpolationStep, value); }

        #endregion

        #region InterpolationValue : double - значення інтерполяції в точці

        private double _InterpolationValue;

        public double InterpolationValue { get => _InterpolationValue; set => Set(ref _InterpolationValue, value); }

        #endregion

        #region selectedDifferentiationData : DifferentiationData - выбранный метод дифференцирования

        private DifferentiationData _selectedDifferentiationData;

        public DifferentiationData selectedDifferentiationData
        {
            get => _selectedDifferentiationData; set
            {
                Set(ref _selectedDifferentiationData, value); 
                DifferentiationMath.SetNumerecialDifferentiation(value.dataValue);
            }
        }

        #endregion

        #region selectedDifferentiationData : DifferentiationData - выбранный метод интегрирования

        private IntegralData _selectedIntegralData;

        public IntegralData selectedIntegralData
        {
            get => _selectedIntegralData; set
            {
                Set(ref _selectedIntegralData, value);
                IntegralMath.SetIntegral(value.dataValue);
            }
        }

        #endregion

        #region selectedInterpolationData : InterpolationData - выбранный метод интерполирования

        private InterpolationData _selectedInterpolationData;

        public InterpolationData selectedInterpolationData { get => _selectedInterpolationData; set
            {
                Set(ref _selectedInterpolationData, value);
                DifferentiationMath.SetInterpolation(value.dataValue);
                IntegralMath.SetInterpolation(value.dataValue);
                InterpolationMath.SetInterpolation(value.dataValue);
            }
        }

        #endregion

        #region differentiationDatas : ObservableCollection<DifferentiationData> - методы дифференцирования, загруженные в базу

        private ObservableCollection<DifferentiationData> _differentiationDatas;

        public ObservableCollection<DifferentiationData> differentiationDatas { get => _differentiationDatas; set => Set(ref _differentiationDatas, value); }

        #endregion

        #region interpolationDatas : ObservableCollection<InterpolationData> - методы интерполирования, загруженные в базу

        private ObservableCollection<InterpolationData> _interpolationDatas;

        public ObservableCollection<InterpolationData> interpolationDatas { get => _interpolationDatas; set => Set(ref _interpolationDatas, value); }

        #endregion

        #region integralDatas : ObservableCollection<IntegralData> - методы интегрирования, загруженные в базу

        private ObservableCollection<IntegralData> _integralDatas;

        public ObservableCollection<IntegralData> integralDatas { get => _integralDatas; set => Set(ref _integralDatas, value); }

        #endregion

        #region InterpolationRun -  интерполирование функции

        private bool _isBusyInterpolation;
        public bool isBusyInterpolation
        {
            get => _isBusyInterpolation;
            private set => Set(ref _isBusyInterpolation, value);
        }

        public ICommand InterpolationRun { get; }
        private async Task OnInterpolationRunExecuted(object p)
        {
            if (selectedInterpolationData == null)
                return;
            try
            {
                isBusyInterpolation = true;
                await Task.Run(() => {
                    switch ((int)p)
                    {
                        //Случай интерполирования в точке
                        case 0:
                            selectedInterpolationData.dataValue.InterpolationPolynom(DataValue.XYValue, NeedInterpolationValue);
                            break;
                        //Случай интерполирования с шагом
                        case 1:
                            for(double a = DataValue.XYValue.First().X; a < DataValue.XYValue.Last().X; a += NeedInterpolationStep)
                                selectedInterpolationData.dataValue.InterpolationPolynom(DataValue.XYValue, a);
                            break;
                    }
                });

            }
            finally
            {
                isBusyInterpolation = false;
            }
        }

        private bool CanInterpolationRunExecute(object p)
        {
            return !isBusyInterpolation;
        }
        #endregion

        #region IntegralRun -  посчитать интеграл, ассинхронная команда

        private bool _isBusyIntegral;
        public bool isBusyIntegral
        {
            get => _isBusyIntegral;
            private set => Set(ref _isBusyIntegral, value);
        }

        public ICommand IntegralRun { get; }
        private async Task OnIntegralRunExecuted(object p)
        {
            try
            {
                isBusyIntegral = true;
                await Task.Run(() => {
                });
                
            }
            finally
            {
                isBusyIntegral = false;
            }
        }

        private bool CanIntegralRunExecute(object p)
        {
            return !isBusyIntegral;
        }
        #endregion

        private void ConfigureData()
        {
            differentiationDatas = new ObservableCollection<DifferentiationData> { };
            interpolationDatas = new ObservableCollection<InterpolationData> { };
            integralDatas = new ObservableCollection<IntegralData> { };

            differentiationDatas.Add(new DifferentiationData { dataValue = new NewtonFirstDifferentiation(), methodName = "Перший многочлен Ньютона" });
            differentiationDatas.Add(new DifferentiationData { dataValue = new NewtonSecondDifferentiation(), methodName = "Другий многочлен Ньютона" });

            interpolationDatas.Add(new InterpolationData { dataValue = new LagrangeInterpolation(), methodName = "Метод Лагранжа" });

            integralDatas.Add(new IntegralData { dataValue = new IntegralRectangles(), methodName = "Метод прямокутника" });
            integralDatas.Add(new IntegralData { dataValue = new IntegralTrapezoid(), methodName = "Метод трапецій" });
            integralDatas.Add(new IntegralData { dataValue = new IntegralSimpson(), methodName = "Метод Сімпсона" });
        }

        public Lab3ViewModel()
        {
            ConfigureData();
            InterpolationRun = new AsyncLambdaCommand(OnInterpolationRunExecuted, CanInterpolationRunExecute);
            IntegralRun = new AsyncLambdaCommand(OnIntegralRunExecuted, CanIntegralRunExecute);
            DataValue = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
        }
    }
}
