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
using AquaCalculationV2_0.Servises.Interpolations.Bezier;

namespace AquaCalculationV2_0.ViewModels.Lab3
{
    class Lab3ViewModel : ViewModel
    {

        #region DataValue : DataModel - задані дані функції

        private DataModel _DataValue;

        public DataModel DataValue { get => _DataValue; set => Set(ref _DataValue, value); }

        #endregion

        //Интерполирования
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

        #region InterpolationValueData : DataModel - значение интерполяции с шагом

        private DataModel _InterpolationValueData;

        public DataModel InterpolationValueData { get => _InterpolationValueData; set => Set(ref _InterpolationValueData, value); }

        #endregion
        //Дифференцирования

        #region NeedDifferentiationValue : double - в какой точке нужно найти дифференциал

        private double _NeedDifferentiationValue;

        public double NeedDifferentiationValue { get => _NeedDifferentiationValue; set => Set(ref _NeedDifferentiationValue, value); }

        #endregion

        #region NeedDifferentiationStep : double - с каким шагом нужно дифференцировать

        private double _NeedDifferentiationStep;

        public double NeedDifferentiationStep { get => _NeedDifferentiationStep; set => Set(ref _NeedDifferentiationStep, value); }

        #endregion

        #region NeedDifferentiationError : double - оишбка дифференцирования

        private double _NeedDifferentiationError;

        public double NeedDifferentiationError { get => _NeedDifferentiationError; set => Set(ref _NeedDifferentiationError, value); }

        #endregion

        #region DifferentiationValue : double - значення диференциала у точці

        private double _DifferentiationValue;

        public double DifferentiationValue { get => _DifferentiationValue; set => Set(ref _DifferentiationValue, value); }

        #endregion

        #region DifferentiationValueData : DataModel - значение дифференциации с шагом

        private DataModel _DifferentiationValueData;

        public DataModel DifferentiationValueData { get => _DifferentiationValueData; set => Set(ref _DifferentiationValueData, value); }

        #endregion

        //Интегрирования
        #region NeedIntegralAValue : double - интервал от A до B на котором происходит интегрирование. Значение A

        private double _NeedIntegralAValue;

        public double NeedIntegralAValue { get => _NeedIntegralAValue; set => Set(ref _NeedIntegralAValue, value); }

        #endregion

        #region NeedIntegralBValue : double - интервал от A до B на котором происходит интегрирование. Значение B

        private double _NeedIntegralBValue;

        public double NeedIntegralBValue { get => _NeedIntegralBValue; set => Set(ref _NeedIntegralBValue, value); }

        #endregion

        #region NeedIntegralInterval : double - количество интервало на промежутке от A до B

        private double _NeedIntegralInterval;

        public double NeedIntegralInterval { get => _NeedIntegralInterval; set => Set(ref _NeedIntegralInterval, value); }

        #endregion

        #region NeedIntegralError : double - значение ошибки, которое не должен превысить интеграл на интервале от A до B(для Рунге и остаточного числа)

        private double _NeedIntegralError = 0.01;

        public double NeedIntegralError { get => _NeedIntegralError; set {
                if (value < 1 && value > 0)
                    Set(ref _NeedIntegralError, value);
                else
                    Set(ref _NeedInterpolationValue, 0.1);
            }
        }

        #endregion

        #region IntegralValue : double - значение интеграла от A  до B

        private double _IntegralValue;

        public double IntegralValue { get => _IntegralValue; set => Set(ref _IntegralValue, value); }

        #endregion

        #region IntegralValueData : DataModel - значение интерполированого(для определения количества узлов в конце интегрирования)

        private DataModel _IntegralValueData;

        public DataModel IntegralValueData { get => _IntegralValueData; set => Set(ref _IntegralValueData, value); }

        #endregion
        //Функция для заполнения
        #region FunctionValue : string - функция, для заполнения от A до B

        private string _FunctionValue;

        public string FunctionValue { get => _FunctionValue; set => Set(ref _FunctionValue, value); }

        #endregion

        #region FunctionAValue : double - A значение

        private double _FunctionAValue;

        public double FunctionAValue { get => _FunctionAValue; set => Set(ref _FunctionAValue, value); }

        #endregion

        #region FunctionBValue : double - B значение

        private double _FunctionBValue;

        public double FunctionBValue { get => _FunctionBValue; set => Set(ref _FunctionBValue, value); }

        #endregion

        #region FunctionCountValue : double - количество елементов для генерации

        private double _FunctionCountValue;

        public double FunctionCountValue { get => _FunctionCountValue; set => Set(ref _FunctionCountValue, value); }

        #endregion

        //Выбор методов
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

        #region TransferData -  трансфер данных

        private bool _isBusyTransferDat;
        public bool isBusyTransferDat
        {
            get => _isBusyTransferDat;
            private set => Set(ref _isBusyTransferDat, value);
        }

        public ICommand TransferData { get; }
        private async Task OnTransferDataExecuted(object p)
        {
            try
            {
                isBusyTransferDat = true;
                await Task.Run(() => {
                    switch (Int32.Parse((string)p))
                    {
                        //Случай трансфера с интерполирования в данные
                        case 0:
                            DataValue.XYValue = InterpolationValueData?.XYValue != null ? new ObservableCollection<XYDataModel>(InterpolationValueData.XYValue) : DataValue.XYValue;
                            break;
                        //Случай трансфера с дифференцировани в данные
                        case 1:
                            DataValue.XYValue = DifferentiationValueData?.XYValue != null ? new ObservableCollection<XYDataModel>(DifferentiationValueData.XYValue) : DataValue.XYValue;
                            break;
                        case 2:
                            DataValue.XYValue = IntegralValueData?.XYValue != null ? new ObservableCollection<XYDataModel>(IntegralValueData.XYValue) : DataValue.XYValue;
                            break;
                    }
                });

            }
            finally
            {
                isBusyTransferDat = false;
            }
        }

        private bool CanTransferDataExecute(object p)
        {
            return !isBusyTransferDat;
        }
        #endregion
        //
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
                    switch (Int32.Parse((string)p))
                    {
                        //Случай интерполирования в точке
                        case 0:
                            InterpolationValue = Math.Round(InterpolationMath.Interpolation(DataValue.XYValue, NeedInterpolationValue), 15);
                            break;
                        //Случай интерполирования с шагом
                        case 1:
                            var temp = InterpolationMath.FullFill(DataValue.XYValue, NeedInterpolationStep);
                            InterpolationValueData = new DataModel { XYValue = new ObservableCollection<XYDataModel>(temp) };
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

        #region DifferentiationRun -  дифференцирование функции

        private bool _isBusyDifferentiation;
        public bool isBusyDifferentiation
        {
            get => _isBusyDifferentiation;
            private set => Set(ref _isBusyDifferentiation, value);
        }

        public ICommand DifferentiationRun { get; }
        private async Task OnDifferentiationRunExecuted(object p)
        {
            if (selectedDifferentiationData == null || selectedInterpolationData == null)
                return;
            try
            {
                isBusyDifferentiation = true;
                await Task.Run(() => {
                    switch (Int32.Parse((string)p))
                    {
                        //Случай дифференцирования в точке
                        case 0:
                            DifferentiationValue = Math.Round(DifferentiationMath.Differentiation(DataValue.XYValue, NeedDifferentiationValue).Value, 15);
                            break;
                        //Случай дифференцирования с шагом
                        case 1:
                            var temp = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
                            for (double a = DataValue.XYValue.First().X; a < DataValue.XYValue.Last().X; a = Math.Round(a + NeedDifferentiationStep, 15))
                                temp.XYValue.Add(new XYDataModel { X = a, Y = Math.Round(InterpolationMath.Interpolation(DataValue.XYValue, a), 15) });
                            var temp2 = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
                            foreach (var el in temp.XYValue)
                            {
                                double? z = DifferentiationMath.Differentiation(temp.XYValue, el.X);
                                if (z != null)
                                    temp2.XYValue.Add(new XYDataModel { X = el.X, Y = Math.Round(z.Value, 15) });
                            }
                            DifferentiationValueData = temp2;
                            break;
                        //Случай дифференцирования с шагом Рунге
                        case 2:
                            DifferentiationValue = DifferentiationMath.DifferentiationWithRunge(DataValue.XYValue, NeedDifferentiationValue, NeedDifferentiationError) != null ?
                            DifferentiationMath.DifferentiationWithRunge(DataValue.XYValue, NeedDifferentiationValue, NeedDifferentiationError).Value : 0;
                            break;
                    }
                });

            }
            finally
            {
                isBusyDifferentiation = false;
            }
        }

        private bool CanDifferentiationRunExecute(object p)
        {
            return !isBusyDifferentiation;
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
            if (selectedInterpolationData == null || selectedDifferentiationData == null || selectedIntegralData == null)
                return;
            try
            {
                if (NeedIntegralAValue >= NeedIntegralBValue) { NeedIntegralAValue = DataValue.XYValue.Select(X => X.X).ToList().Min(); NeedIntegralBValue = DataValue.XYValue.Select(X => X.X).ToList().Max(); }

                isBusyIntegral = true;
                await Task.Run(() =>
                {
                    switch (Int32.Parse((string)p))
                    {
                        //Случай интегрирования от A до B с количеством интервалов NeedIntegralInterval
                        case 0:
                            if (NeedIntegralInterval <= 0) return;
                            double step = (NeedIntegralBValue - NeedIntegralAValue) / NeedIntegralInterval;

                            var XYValueData = new ObservableCollection<XYDataModel>(InterpolationMath.FullFill(DataValue.XYValue, step));

                            IntegralValue = IntegralMath.Integral(XYValueData, step, NeedIntegralAValue, NeedIntegralBValue);
                            IntegralValueData = new DataModel { XYValue = new ObservableCollection<XYDataModel>(IntegralMath.Function(XYValueData)) };
                            break;
                        //Случай интегрирование с шагом Рунге
                        case 1:
                            IntegralValue = IntegralMath.IntegralWithRunge(DataValue.XYValue, NeedIntegralAValue, NeedIntegralBValue, NeedIntegralError).Value;
                            break;
                        //Случай интегрирование с шагом остаточного числа
                        case 2:
                            IntegralValue = IntegralMath.IntegralWithError(DataValue.XYValue, NeedIntegralAValue, NeedIntegralBValue, NeedIntegralError).Value;
                            break;
                    }
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

        #region FullFillWithFunctionRun -  посчитать интеграл, ассинхронная команда

        private bool _isBusyFullFillFunction;
        public bool isBusyFullFillFunction
        {
            get => _isBusyFullFillFunction;
            private set => Set(ref _isBusyFullFillFunction, value);
        }

        public ICommand FullFillWithFunctionRun { get; }
        private async Task OnFullFillWithFunctionRunExecuted(object p)
        {
            try
            {
                isBusyFullFillFunction = true;
                await Task.Run(() =>
                {
                    Argument x = new Argument($"x = 0");

                    Expression ex = new Expression(FunctionValue, x);

                    var tempData = new ObservableCollection<XYDataModel> { };
                    double step = Math.Round(Math.Round((FunctionBValue - FunctionAValue), 15) / Math.Round((FunctionCountValue - 1), 15), 15);
                    double a = 0;
                    for (a = FunctionAValue; a < FunctionBValue; a = Math.Round(a + step, 15))
                    {
                        x.setArgumentValue(a);

                        tempData.Add(new XYDataModel { X = a, Y = ex.calculate() });
                    }
                    x.setArgumentValue(a);

                    tempData.Add(new XYDataModel { X = a, Y = ex.calculate() });

                    DataValue.XYValue = tempData;
                });
            }
            finally
            {
                isBusyFullFillFunction = false;
            }
        }

        private bool CanFullFillWithFunctionRunExecute(object p)
        {
            return !isBusyFullFillFunction;
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
            interpolationDatas.Add(new InterpolationData { dataValue = new NewtonFirstInterpolation(), methodName = "Перший многочлен Ньютона " });
            interpolationDatas.Add(new InterpolationData { dataValue = new NewtonSecondInterpolation(), methodName = "Другий многочлен Ньютона" });
            interpolationDatas.Add(new InterpolationData { dataValue = new CubicSplineInterpolation(), methodName = "Кубический  сплайн" });
            interpolationDatas.Add(new InterpolationData { dataValue = new LinearAproximation(), methodName = "МНК: Линейная" });
            interpolationDatas.Add(new InterpolationData { dataValue = new BezierCurveOrder1(), methodName = "Кривая Безье: Степень 1" });
            interpolationDatas.Add(new InterpolationData { dataValue = new BezierCurveOrder2(), methodName = "Кривая Безье: Степень 2" });
            interpolationDatas.Add(new InterpolationData { dataValue = new BezierCurveOrder3(), methodName = "Кривая Безье: Степень 3" });

            integralDatas.Add(new IntegralData { dataValue = new IntegralRectangles(), methodName = "Метод прямокутника" });
            integralDatas.Add(new IntegralData { dataValue = new IntegralTrapezoid(), methodName = "Метод трапецій" });
            integralDatas.Add(new IntegralData { dataValue = new IntegralSimpson(), methodName = "Метод Сімпсона" });
        }

        public Lab3ViewModel()
        {
            ConfigureData();
            TransferData = new AsyncLambdaCommand(OnTransferDataExecuted, CanTransferDataExecute);
            FullFillWithFunctionRun = new AsyncLambdaCommand(OnFullFillWithFunctionRunExecuted, CanFullFillWithFunctionRunExecute);
            DifferentiationRun = new AsyncLambdaCommand(OnDifferentiationRunExecuted, CanDifferentiationRunExecute);
            InterpolationRun = new AsyncLambdaCommand(OnInterpolationRunExecuted, CanInterpolationRunExecute);
            IntegralRun = new AsyncLambdaCommand(OnIntegralRunExecuted, CanIntegralRunExecute);
            DataValue = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
        }
    }
}
