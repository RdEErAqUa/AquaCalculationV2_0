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

        #region NeedIntegralIntervalSecond : double - для метода Гаусса

        private double _NeedIntegralIntervalSecond;

        public double NeedIntegralIntervalSecond { get => _NeedIntegralIntervalSecond; set => Set(ref _NeedIntegralIntervalSecond, value); }

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

        #region IntegralProtocol : String - протокол, для интегрирования
        private String _IntegralProtocol;

        public String IntegralProtocol { get => _IntegralProtocol; set => Set(ref _IntegralProtocol, value); }

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
        //Определение площади фигур
        #region FormulaDatas : ObservableCollection<String> - формулы, в пределах которых нужно найти площадь

        private ObservableCollection<FormulaValue> _FormulaDatas;

        public ObservableCollection<FormulaValue> FormulaDatas { get => _FormulaDatas; set => Set(ref _FormulaDatas, value); }

        #endregion

        #region String : SelectedFormula - выбранная формула

        private FormulaValue _SelectedFormula;

        public FormulaValue SelectedFormula { get => _SelectedFormula; set => Set(ref _SelectedFormula, value); }

        #endregion

        #region String : FormulaForAdd - выбранная формула

        private FormulaValue _FormulaForAdd;

        public FormulaValue FormulaForAdd { get => _FormulaForAdd; set => Set(ref _FormulaForAdd, value); }

        #endregion

        #region int : RandomPointCount - количество случайных точек

        private int _RandomPointCount;

        public int RandomPointCount { get => _RandomPointCount; set => Set(ref _RandomPointCount, value); }

        #endregion

        #region DataModel : FormulaData - точки фигуры

        private DataModel _FormulaData;

        public DataModel FormulaData { get => _FormulaData; set => Set(ref _FormulaData, value); }

        #endregion

        #region DataModelPoint : RandomPointValue - случайные точки

        private DataModelChecked _RandomPointValue;

        public DataModelChecked RandomPointValue { get => _RandomPointValue; set => Set(ref _RandomPointValue, value); }

        #endregion

        #region DataModelPoint : RandomPointApproved - случайные точки

        private DataModelChecked _RandomPointApproved;

        public DataModelChecked RandomPointApproved { get => _RandomPointApproved; set => Set(ref _RandomPointApproved, value); }

        #endregion

        #region DataModel : RandomPointPerCount - количество Y елементов в зоне на всего количество елементов X

        private DataModel _RandomPointPerCount;

        public DataModel RandomPointPerCount { get => _RandomPointPerCount; set => Set(ref _RandomPointPerCount, value); }

        #endregion

        #region DataModel : RandomPointSPerCount - количество Y елементов в зоне на всего количество елементов X

        private DataModel _RandomPointSPerCount;

        public DataModel RandomPointSPerCount { get => _RandomPointSPerCount; set => Set(ref _RandomPointSPerCount, value); }

        #endregion

        #region DataModel : _AAveragePlus - Середньоквадратичне відхилення додатнье

        private DataModel _AAveragePlus;

        public DataModel AAveragePlus { get => _AAveragePlus; set => Set(ref _AAveragePlus, value); }

        #endregion

        #region DataModel : AAverage - Середньоквадратичне відхилення відємне

        private DataModel _AAverageMinus;

        public DataModel AAverageMinus { get => _AAverageMinus; set => Set(ref _AAverageMinus, value); }

        #endregion

        #region DataModel : FormulaDataMaxMin - точки ограничивающей фигуры

        private DataModel _FormulaDataMaxMin;

        public DataModel FormulaDataMaxMin { get => _FormulaDataMaxMin; set => Set(ref _FormulaDataMaxMin, value); }

        #endregion

        #region FiguraDataRun -  интерполирование функции

        private bool _isBusyFigureRun;
        public bool isBusyFigureRun
        {
            get => _isBusyFigureRun;
            private set => Set(ref _isBusyFigureRun, value);
        }

        public ICommand FiguraDataRun { get; }
        private async Task OnFiguraDataRunExecuted(object p)
        {
            if (FormulaForAdd.Formula == null) return;
            try
            {
                isBusyFigureRun = true;
                await Task.Run(() =>
                {
                    var tempValue = FormulaDatas != null ? new ObservableCollection<FormulaValue>(FormulaDatas) : new ObservableCollection<FormulaValue> { };
                    switch (Int32.Parse((string)p))
                    {
                        case 0:
                            tempValue.Add(new FormulaValue { Formula = FormulaForAdd.Formula, A = FormulaForAdd.A, B = FormulaForAdd.B });
                            FormulaDatas = tempValue;
                            break;
                        case 1:
                            tempValue.Remove(SelectedFormula);
                            FormulaDatas = tempValue;
                            break;
                    }
                });
            }
            finally
            {
                isBusyFigureRun = false;
            }
        }

        private bool CanFiguraDataRunExecute(object p)
        {
            return !isBusyFigureRun;
        }
        #endregion

        #region BuildFigureRun -  построить фигуры

        private bool _isBusyBuildFigureRun;
        public bool isBusyBuildFigureRun
        {
            get => _isBusyBuildFigureRun;
            private set => Set(ref _isBusyBuildFigureRun, value);
        }

        public ICommand BuildFigureRun { get; }
        private async Task OnBuildFigureRunExecuted(object p)
        {
            if (FormulaDatas == null) return;
            try
            {
                isBusyBuildFigureRun = true;
                await Task.Run(() =>
                {
                    var tempValue = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
                    double step = 0;
                    double a = FormulaDatas.Select(X => X.A).ToList().Min(), b = FormulaDatas.Select(X => X.B).ToList().Max();
                    step = (b - a) / FunctionCountValue;
                    foreach (var el in FormulaDatas)
                    {
                        Argument x = new Argument($"x = 0");

                        Expression ex = new Expression(el.Formula, x);

                        for (double i = a; i <= b; i = Math.Round(i + step, 15))
                        {
                            x.setArgumentValue(i);

                            if(!Double.IsNaN(ex.calculate()) && (el.A <= i && el.B >= i))
                                tempValue.XYValue.Add(new XYDataModel { X = i, Y = ex.calculate() });
                        }
                    }
                    var tempValueMaxMin = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };

                    double maxY = tempValue.XYValue.Select(Y => Y.Y).ToList().Max();
                    double minY = tempValue.XYValue.Select(Y => Y.Y).ToList().Min();

                    double maxX = tempValue.XYValue.Select(X => X.X).ToList().Max();
                    double minX = tempValue.XYValue.Select(X => X.X).ToList().Min();

                    tempValueMaxMin.XYValue.Add( new XYDataModel { X = minX, Y = minY});
                    tempValueMaxMin.XYValue.Add(new XYDataModel { X = minX, Y = maxY });
                    tempValueMaxMin.XYValue.Add(new XYDataModel { X = maxX, Y = maxY });
                    tempValueMaxMin.XYValue.Add(new XYDataModel { X = maxX, Y = minY });
                    tempValueMaxMin.XYValue.Add(new XYDataModel { X = minX, Y = minY });

                    FormulaDataMaxMin = tempValueMaxMin;

                    FormulaData = tempValue;
                });
            }
            finally
            {
                isBusyBuildFigureRun = false;
            }
        }

        private bool CanBuildFigureRunExecute(object p)
        {
            return !isBusyBuildFigureRun;
        }
        #endregion

        #region S : double - площа фигуры

        private double _SValue;
        public double SValue
        {
            get => _SValue;
            set => Set(ref _SValue, value);
        }

        #endregion

        #region SValueApproved : double - точная площадь фигуры

        private double _SValueApproved;
        public double SValueApproved
        {
            get => _SValueApproved;
            set => Set(ref _SValueApproved, value);
        }

        #endregion

        #region FindSRun -  найти площадь

        private bool _isBusyBFindSRun;
        public bool isBusyBFindSRun
        {
            get => _isBusyBFindSRun;
            private set => Set(ref _isBusyBFindSRun, value);
        }

        public ICommand FindSRun { get; }
        private async Task OnFindSRunExecuted(object p)
        {
            if (RandomPointCount <= 0 || FormulaDataMaxMin == null || FormulaDataMaxMin.XYValue.Count < 0) return;
            try
            {
                isBusyBFindSRun = true;
                await Task.Run(() =>
                {
                    double maxY = FormulaDataMaxMin.XYValue.Select(Y => Y.Y).ToList().Max();
                    double minY = FormulaDataMaxMin.XYValue.Select(Y => Y.Y).ToList().Min();

                    double maxX = FormulaDataMaxMin.XYValue.Select(X => X.X).ToList().Max();
                    double minX = FormulaDataMaxMin.XYValue.Select(X => X.X).ToList().Min();

                    var temp = new DataModelChecked { XYValue = new ObservableCollection<XYDataModelIsChecked> { } };

                    Random rand = new Random();
                    double a = FormulaDatas.Select(X => X.A).ToList().Min(), b = FormulaDatas.Select(X => X.B).ToList().Max();
                    double ErrorValue = (b - a) / FunctionCountValue * 0.5;
                    var tempDataPerCount = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
                    for (int j = 0; j < RandomPointCount; j++)
                    {
                        var x = rand.NextDouble() * (maxX - minX) + minX;
                        var y = rand.NextDouble() * (maxY - minY) + minY;

                        temp.XYValue.Add(new XYDataModelIsChecked { X = x, Y = y, IsChecked = false });

                        var allY = FormulaData.XYValue.Where(X => X.X < temp.XYValue.Last().X + ErrorValue && X.X > temp.XYValue.Last().X - ErrorValue).Select(X => X.Y).ToList();

                        allY.Sort();
                        var allY2 = FormulaData.XYValue.Where(X => X.X < temp.XYValue.Last().X + ErrorValue && X.X > temp.XYValue.Last().X - ErrorValue).ToList();

                        for (int i = 0; i < allY.Count - 1; i++)
                        {
                            if (i % 2 == 0 && allY[i] < temp.XYValue.Last().Y && allY[i + 1] > temp.XYValue.Last().Y)
                            {
                                temp.XYValue.Last().IsChecked = true;
                                break;
                            }
                            else if (i % 2 == 1 && allY[i] < temp.XYValue.Last().Y && allY[i + 1] > temp.XYValue.Last().Y)
                            {
                                temp.XYValue.Last().IsChecked = false;
                                break;
                            }
                        }
                        tempDataPerCount.XYValue.Add(new XYDataModel { X = j, Y = temp.XYValue.Where(X => X.IsChecked).ToList().Count });
                    }
                    RandomPointPerCount = tempDataPerCount;
                    RandomPointValue = new DataModelChecked { XYValue = new ObservableCollection<XYDataModelIsChecked>(temp.XYValue.Where(X => !X.IsChecked).ToList()) };
                    RandomPointApproved = new DataModelChecked { XYValue = new ObservableCollection<XYDataModelIsChecked>(temp.XYValue.Where(X => X.IsChecked).ToList()) };

                    double XValueData = (FormulaDataMaxMin.XYValue.Select(X => X.X).ToList().Max() - FormulaDataMaxMin.XYValue.Select(X => X.X).ToList().Min());
                    double YValueData = FormulaDataMaxMin.XYValue.Select(X => X.Y).ToList().Max() - FormulaDataMaxMin.XYValue.Select(X => X.Y).ToList().Min();

                    double S = XValueData * YValueData;

                    var tempValueZR = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
                    foreach (var el in RandomPointPerCount.XYValue)
                    {
                        double AValue = (el.Y / el.X) * S;

                        double deltaValue = (AValue - SValueApproved) / SValueApproved;

                        tempValueZR.XYValue.Add(new XYDataModel { X = el.X, Y = deltaValue });
                    }
                    RandomPointSPerCount = tempValueZR;
                    SValue = (RandomPointPerCount.XYValue.Select(X => X.Y).ToList().Max() / RandomPointPerCount.XYValue.Select(X => X.X).ToList().Max()) * S;

                    var temp2 = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
                    var temp3 = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
                    foreach (var el in RandomPointSPerCount.XYValue)
                    {
                        temp2.XYValue.Add(new XYDataModel { X = el.X, Y = (3 / Math.Sqrt(el.X)) });
                        temp3.XYValue.Add(new XYDataModel { X = el.X, Y = -(3 / Math.Sqrt(el.X)) });
                    }
                    AAveragePlus = temp2;
                    AAverageMinus = temp3;
                });
            }
            finally
            {
                isBusyBFindSRun = false;
            }
        }

        private bool CanFindSRunExecute(object p)
        {
            return !isBusyBFindSRun;
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
                            IntegralProtocol = $"При количестве узлов - {NeedIntegralInterval}: \n" + IntegralMath.ProtocolBuild(XYValueData, step, NeedIntegralAValue, NeedIntegralBValue);
                            break;
                        //Случай интегрирование с шагом Рунге
                        case 1:
                            IntegralValue = IntegralMath.IntegralWithRunge(DataValue.XYValue, NeedIntegralAValue, NeedIntegralBValue, NeedIntegralError).Value;
                            IntegralProtocol = $"При E - {NeedIntegralError}: \n" + IntegralMath.IntegralWithRungeProtocol(DataValue.XYValue, NeedIntegralAValue, NeedIntegralBValue, NeedIntegralError);
                            break;
                        //Случай интегрирование с шагом остаточного числа
                        case 2:
                            IntegralValue = IntegralMath.IntegralWithError(DataValue.XYValue, NeedIntegralAValue, NeedIntegralBValue, NeedIntegralError).Value;
                            break;
                        //Интегрирование Гаусса
                        case 3:
                            double QuadriticValue = NeedIntegralInterval;
                            IntegralValue = IntegralMath.Integral(DataValue.XYValue, QuadriticValue, NeedIntegralAValue, NeedIntegralBValue);
                            IntegralProtocol = IntegralMath.ProtocolBuild(DataValue.XYValue, QuadriticValue, NeedIntegralAValue, NeedIntegralBValue);
                            QuadriticValue = NeedIntegralIntervalSecond;
                            IntegralValue = IntegralMath.Integral(DataValue.XYValue, QuadriticValue, NeedIntegralAValue, NeedIntegralBValue);
                            IntegralProtocol +=  "\n" + IntegralMath.ProtocolBuild(DataValue.XYValue, QuadriticValue, NeedIntegralAValue, NeedIntegralBValue);
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
            integralDatas.Add(new IntegralData { dataValue = new IntegralGaussian(), methodName = "Метод Гаусса" });
        }

        public Lab3ViewModel()
        {
            ConfigureData();
            FormulaForAdd = new FormulaValue();
            FindSRun = new AsyncLambdaCommand(OnFindSRunExecuted, CanFindSRunExecute);
            BuildFigureRun = new AsyncLambdaCommand(OnBuildFigureRunExecuted, CanBuildFigureRunExecute);
            FiguraDataRun = new AsyncLambdaCommand(OnFiguraDataRunExecuted, CanFiguraDataRunExecute);
            TransferData = new AsyncLambdaCommand(OnTransferDataExecuted, CanTransferDataExecute);
            FullFillWithFunctionRun = new AsyncLambdaCommand(OnFullFillWithFunctionRunExecuted, CanFullFillWithFunctionRunExecute);
            DifferentiationRun = new AsyncLambdaCommand(OnDifferentiationRunExecuted, CanDifferentiationRunExecute);
            InterpolationRun = new AsyncLambdaCommand(OnInterpolationRunExecuted, CanInterpolationRunExecute);
            IntegralRun = new AsyncLambdaCommand(OnIntegralRunExecuted, CanIntegralRunExecute);
            DataValue = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
        }
    }
}
