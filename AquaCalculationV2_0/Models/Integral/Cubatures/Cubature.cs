using AquaCalculationV2_0.Models.Base;
using AquaCalculationV2_0.Servises.Interpolations;
using AquaCalculationV2_0.Servises.Interpolations.Interfaces;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models.Integral.Cubatures
{
    class Cubature : ModelBase
    {
        #region A : double -  интегрирования от {A; B} - A значение
        private double _A;
        public double A { get => _A; set => Set(ref _A, value); }
        #endregion

        #region B : double -  интегрирования от {A; B} - B значение
        private double _B;
        public double B { get => _B; set => Set(ref _B, value); }
        #endregion

        #region AFormula : String - нижняя граница формулою.
        private String _AFormula;
        public String AFormula { get => _AFormula; set => Set(ref _AFormula, value); }
        #endregion

        #region BFormula : String - верхняя граница формулою.
        private String _BFormula;
        public String BFormula { get => _BFormula; set => Set(ref _BFormula, value); }
        #endregion

        #region FXY : String - формула.
        private String _FXY;
        public String FXY { get => _FXY; set => Set(ref _FXY, value); }
        #endregion

        #region AccuracyValue : int - значение точности от 1 до 7
        public int _AccuracyValue;
        public int AccuracyValue { get => _AccuracyValue; set { Set(ref _AccuracyValue, value); MakeQuadratureCoefficients(value); } }
        #endregion

        #region NX : int - значение количества узлов по X
        public int _NX;
        public int NX { get => _NX; set => Set(ref _NX, value); }
        #endregion

        #region NY : int - значение количества узлов по Y
        public int _NY;
        public int NY { get => _NY; set => Set(ref _NY, value);}
        #endregion

        #region IntegralValueRight : double - правильное значение интеграла
        public double _IntegralValueRight;
        public double IntegralValueRight { get => _IntegralValueRight; set => Set(ref _IntegralValueRight, value); }
        #endregion


        #region
        private ObservableCollection<ThirdDimensionalData> _ElementValue;

        public ObservableCollection<ThirdDimensionalData> ElementValue { get => _ElementValue; set => Set(ref _ElementValue, value); }

        #endregion

        #region
        private List<(double, double)> _QuadratureElementValue;

        public List<(double, double)> QuadratureElementValue { get => _QuadratureElementValue; set => Set(ref _QuadratureElementValue, value); }

        #endregion

        #region

        #region DataModel : UpperFunction - функция, которая ограничивает сверху

        private DataModel _UpperFunction;

        public DataModel UpperFunction { get => _UpperFunction; set => Set(ref _UpperFunction, value); }

        #endregion

        #region DataModel : FunctionErrorX - функция, которая ограничивает сверху

        private DataModel _FunctionErrorX;

        public DataModel FunctionErrorX { get => _FunctionErrorX; set => Set(ref _FunctionErrorX, value); }

        #endregion

        #region DataModel : FunctionErrorY - функция, которая ограничивает сверху

        private DataModel _FunctionErrorY;

        public DataModel FunctionErrorY { get => _FunctionErrorY; set => Set(ref _FunctionErrorY, value); }

        #endregion

        #region DataModel : LowerFunction - функция, которая ограничивает снизу

        private DataModel _LowerFunction;

        public DataModel LowerFunction { get => _LowerFunction; set => Set(ref _LowerFunction, value); }

        #endregion

        #region DataModel : Function - функция

        private DataModel _Function;

        public DataModel Function { get => _Function; set => Set(ref _Function, value); }

        #endregion

        #endregion
        public void SetElement(int nx = 0, int ny = 0)
        {
            if (nx <= 1 || ny <= 1) { nx = NX; ny = NY; }
            Argument a = new Argument($"x = 0");
            Expression aex = new Expression(AFormula, a);
            Argument b = new Argument($"x = 0");
            Expression bex = new Expression(BFormula, b);

            Argument fx = new Argument($"x = 0");
            Argument fy = new Argument($"y = 0");
            Expression fxyex = new Expression(FXY, fx, fy);

            var u = new DataModel { XYValue = new System.Collections.ObjectModel.ObservableCollection<XYDataModel> { } };
            var l = new DataModel { XYValue = new System.Collections.ObjectModel.ObservableCollection<XYDataModel> { } };
            var fz = new ObservableCollection<ThirdDimensionalData> { };

            double fxr = A;
            for (int j = 0; j < nx; j++)
            {
                a.setArgumentValue(fxr);
                b.setArgumentValue(fxr);

                u.XYValue.Add(new XYDataModel { X = fxr, Y = bex.calculate() });
                l.XYValue.Add(new XYDataModel { X = fxr, Y = aex.calculate() });
                fxr += (B - A) / (double)(nx - 1);
            }

            for (int i = 0; i < u.XYValue.Count; i++)
            {
                fx.setArgumentValue(u.XYValue[i].X);

                double step = (u.XYValue[i].Y - l.XYValue[i].Y) / (ny - 1);
                if (step > 0)
                {
                    double j = l.XYValue[i].Y;
                    for (int temp1 = 0; temp1 < ny; temp1++)
                    {
                        fy.setArgumentValue(j);
                        fz.Add(new ThirdDimensionalData { X = l.XYValue[i].X, Y = j, Z = fxyex.calculate() });
                        j += step;
                    }
                }
                else
                {
                    fy.setArgumentValue(l.XYValue[i].Y);
                    fz.Add(new ThirdDimensionalData { X = l.XYValue[i].X, Y = l.XYValue[i].Y, Z = fxyex.calculate() });
                }
            }
            UpperFunction = u;
            LowerFunction = l;
            ElementValue = fz;
        }
        public double Integral()
        {
            if (AccuracyValue <= 0 || AccuracyValue > 8) return 0;
            if (AFormula == null || BFormula == null || FXY == null) return 0;

            SetElement();

            Argument a = new Argument($"x = 0");
            Expression aex = new Expression(AFormula, a);
            Argument b = new Argument($"x = 0");
            Expression bex = new Expression(BFormula, b);

            Argument fx = new Argument($"x = 0");
            Argument fy = new Argument($"y = 0");
            Expression fxyex = new Expression(FXY, fx, fy);

            double f = 0;

            for (int i = 0; i < AccuracyValue; i++)
            {
                (double, double) el = i <= QuadratureElementValue.Count - 1 ? QuadratureElementValue[i] : (QuadratureElementValue[AccuracyValue - i - 1].Item1, -QuadratureElementValue[AccuracyValue - i - 1].Item2);

                double Ei = (B + A) / 2.0 + ((B - A) / 2.0) * el.Item2;

                a.setArgumentValue(Ei);
                b.setArgumentValue(Ei);

                double MaxX = bex.calculate();
                double MinX = aex.calculate();

                double f1 = 0;

                fx.setArgumentValue(Ei);

                for (int j = 0; j < AccuracyValue; j++)
                {
                    (double, double) el2 = j <= QuadratureElementValue.Count - 1 ? QuadratureElementValue[j] : (QuadratureElementValue[AccuracyValue - j - 1].Item1, -QuadratureElementValue[AccuracyValue - j - 1].Item2);

                    double EiY = (MaxX + MinX) / 2.0 + ((MaxX - MinX) / 2.0) * el2.Item2;

                    fy.setArgumentValue(EiY);

                    f1 += el2.Item1 * fxyex.calculate();
                }

                f += el.Item1 * f1 * ((MaxX - MinX) / 2);
            }

            f *= ((B - A) / 2.0);

            return f;
        }
        public void BuildFunctionError()
        {
            var FunctionErrorX = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
            var FunctionErrorY = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
            for (int i = 0; i < 100; i++)
            {
                FunctionErrorX.XYValue.Add(new XYDataModel { X = i, Y = Math.Abs(IntegralValueRight - CubatureSimpson(30, i)) });
            }
            this.FunctionErrorX = FunctionErrorX;
            for (int i = 0; i < 100; i++)
            {
                FunctionErrorY.XYValue.Add(new XYDataModel { X = i, Y = Math.Abs(IntegralValueRight - CubatureSimpson(i, 30)) });
            }
            this.FunctionErrorY = FunctionErrorY;
        }
        public double CubatureSimpson(int nx = 0, int ny = 0)
        {
            if (nx <= 1 || ny <= 1) { nx = NX; ny = NY; }
            if (AccuracyValue <= 0 || AccuracyValue > 8) return 0;
            if (AFormula == null || BFormula == null || FXY == null) return 0;

            SetElement(nx, ny);

            Argument a = new Argument($"x = 0");
            Expression aex = new Expression(AFormula, a);
            Argument b = new Argument($"x = 0");
            Expression bex = new Expression(BFormula, b);

            Argument fx = new Argument($"x = 0");
            Argument fy = new Argument($"y = 0");
            Expression fxyex = new Expression(FXY, fx, fy);

            double f = 0;

            double MaxY = ElementValue.Select(X => X.Y).Max();
            double MinY = ElementValue.Select(X => X.Y).Min();

            double HX = (B - A) / (nx - 1);
            double HY = (MaxY - MinY) / (ny - 1);

            var NewElementValue = new ObservableCollection<ThirdDimensionalData> { };
            double x = A;
            //Заполним функцию, которая ограничивает наше значение
            for (int i = 0; i < nx; i++)
            {
                fx.setArgumentValue(x);
                a.setArgumentValue(x);
                b.setArgumentValue(x);
                double y = MinY;
                for (int j = 0; j < ny; j++)
                {
                    fy.setArgumentValue(y);
                    double value = fxyex.calculate();
                    if (!Double.IsNaN(value) && !Double.IsInfinity(value) && (bex.calculate() >= y && aex.calculate() <= y))
                        NewElementValue.Add(new ThirdDimensionalData { X = x, Y = y, Z = value });
                    else
                        NewElementValue.Add(new ThirdDimensionalData { X = x, Y = y, Z = 0 });
                    y += HY;
                }
                x += HX;
            }
            if (nx * ny != NewElementValue.Count) return -1;
            //
            List<double> Q0J = new List<double> { };
            List<double> Q0I = new List<double> { };

            for(int i = 0; i < nx; i++)
            {
                double value = (i == 0 || i == nx - 1 ? 1 :
                    (i % 2 == 0 ? (2) : (4)));
                Q0J.Add(value);
            }
            for(int i = 0; i < ny; i++)
            {
                double value = (i == 0 || i == ny - 1 ? 1 :
                    (i % 2 == 0 ? (2) : (4)));
                Q0I.Add(value);
            }
 
            for (int i = 0; i < nx; i++)
            {
                double Ei = NewElementValue.First().X + HX * i; 

                fx.setArgumentValue(Ei);

                for (int j = 0; j < ny; j++)
                {
                    f += (Q0J[i] * Q0I[j]) * NewElementValue[i * ny + j].Z;
                }
            }

            f *= ((HX * HY) / 9.0);

            return f;
        }

        private void MakeQuadratureCoefficients(int n)
        {
            this.QuadratureElementValue = new List<(double, double)> { };
            switch (AccuracyValue)
            {
                case 1:
                    QuadratureElementValue = new List<(double, double)> { (2, 0.5) };
                    break;
                case 2:
                    QuadratureElementValue = new List<(double, double)> { (1, -0.577350269) };
                    break;
                case 3:
                    QuadratureElementValue = new List<(double, double)> { (0.555555556, -0.774596669), (0.888888889, 0) };
                    break;
                case 4:
                    QuadratureElementValue = new List<(double, double)> { (0.347854845, -0.861136312), (0.652145155, -0.339981044) };
                    break;
                case 5:
                    QuadratureElementValue = new List<(double, double)> { (0.236926885, -0.906179846), (0.478628670, -0.538469310), (0.568888889, 0) };
                    break;
                case 6:
                    QuadratureElementValue = new List<(double, double)> { (0.171324492, -0.932469514), (0.360761573, -0.661209386), (0.467913935, -0.238619186) };
                    break;
                case 7:
                    QuadratureElementValue = new List<(double, double)> { (0.129485, -0.949108), (0.279705, -0.741531), (0.38183, -0.405845), (0.41796, 0) };
                    break;
                case 8:
                    QuadratureElementValue = new List<(double, double)> { (0.101228, -0.96029), (0.222381, -0.796666), (0.313707, -0.525532), (0.362684, 0.183434) };
                    break;
            }
        }
    }
}
