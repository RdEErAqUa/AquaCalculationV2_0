using AquaCalculationV2_0.Models.Base;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models.Integral
{
    class Fredgolma : ModelBase
    {
        #region A : double -  интегрирования от {A; B} - A значение
        private double _A;
        public double A { get => _A; set => Set(ref _A, value); }
        #endregion

        #region B : double -  интегрирования от {A; B} - B значение
        private double _B;
        public double B { get => _B; set => Set(ref _B, value); }
        #endregion

        #region IntegralFunctionLine : String - нижняя граница формулою.
        private String _IntegralFunctionLine;
        public String IntegralFunctionLine { get => _IntegralFunctionLine; set => Set(ref _IntegralFunctionLine, value); }
        #endregion
        #region FunctionLine : String - нижняя граница формулою.
        private String _FunctionLine;
        public String FunctionLine { get => _FunctionLine; set => Set(ref _FunctionLine, value); }
        #endregion

        #region AnswerValue : String - нижняя граница формулою.
        private String _AnswerValue;
        public String AnswerValue { get => _AnswerValue; set => Set(ref _AnswerValue, value); }
        #endregion

        #region ErrorNValue : int -  интегрирования от {A; B} - B значение
        private int _ErrorNValue;
        public int ErrorNValue { get => _ErrorNValue; set => Set(ref _ErrorNValue, value); }
        #endregion

        #region
        private DataModel _DataModel;

        public DataModel DataModel { get => _DataModel; set => Set(ref _DataModel, value); }

        #endregion

        public double IntegralValueSimpson()
        {
            Argument X = new Argument($"x = 0");
            Argument S = new Argument($"s = 0");
            Expression KXS = new Expression(IntegralFunctionLine, X, S);
            Expression FX = new Expression(FunctionLine, X);

            List<double> XValue = new List<double> { };
            List<double> AValue = new List<double> { };

            double step = (B - A) / ErrorNValue;

            for(int i = 0; i <= ErrorNValue; i++)
            {
                double needToAddA = i == 0 || i == ErrorNValue ? step/3.0 : (i % 2 == 0 ? 2 * step / 3.0 : 4 * step / 3.0);
                AValue.Add(needToAddA);
                XValue.Add(step * i);
            }
            List<List<double>> xMatrix = new List<List<double>> { };
            List<double> FreeValue = new List<double> { };
            for (int i1 = 0; i1 < XValue.Count; i1++)
            {
                double el = (double)XValue[i1];
                var xValue = new List<double> { };
                X.setArgumentValue(el);
                for (int i = 0; i < XValue.Count; i++)
                {
                    S.setArgumentValue(XValue[i]);
                    xValue.Add(AValue[i] * KXS.calculate());
                }
                xMatrix.Add(xValue);

                xMatrix[i1][i1] += 1;

                FreeValue.Add(FX.calculate());
            }
            List<double> xMatrixOneDimentional = new List<double> { };
            foreach(var el in xMatrix)
            {
                foreach(var el2 in el)
                {
                    xMatrixOneDimentional.Add(el2);
                }
            }

            var yValue = GaussienInvoke(xMatrixOneDimentional, FreeValue, xMatrix.Count, xMatrix.First().Count);

            AnswerValue = FunctionLine + " - ("; ;
            for (int i = 0; i <= ErrorNValue; i++)
            {
                AnswerValue += " + "  + $"{Math.Round(AValue[i], 4)}"  + " * "  + IntegralFunctionLine.Replace("s", Math.Round(XValue[i], 4).ToString()) + " * " + Math.Round(yValue[i], 4);
            }
            AnswerValue += " )";

            double stepValue = (B - A) / 50;
            var z  = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
            for (int i = 0; i < 50; i++)
            {
                double xValue = XValue.First() + stepValue * i;
                X.setArgumentValue(xValue);

                double yvalue = FX.calculate();
                for (int j = 0; j <= ErrorNValue; j++)
                {
                    S.setArgumentValue(XValue[j]);
                    yvalue += (AValue[j] * KXS.calculate() * yValue[j]);
                }

                z.XYValue.Add(new XYDataModel { X = xValue, Y = yvalue });
            }
            DataModel = z;
            return 0;
        }

        public double IntegralValueTrapezoid()
        {
            Argument X = new Argument($"x = 0");
            Argument S = new Argument($"s = 0");
            Expression KXS = new Expression(IntegralFunctionLine, X, S);
            Expression FX = new Expression(FunctionLine, X);

            List<double> XValue = new List<double> { };
            List<double> AValue = new List<double> { };

            double step = (B - A) / ErrorNValue;

            for (int i = 0; i <= ErrorNValue; i++)
            {
                double needToAddA = i == 0 || i == ErrorNValue ? step / 2.0 : step;
                AValue.Add(needToAddA);
                XValue.Add(step * i);
            }
            List<List<double>> xMatrix = new List<List<double>> { };
            List<double> FreeValue = new List<double> { };
            for (int i1 = 0; i1 < XValue.Count; i1++)
            {
                double el = (double)XValue[i1];
                var xValue = new List<double> { };
                X.setArgumentValue(el);
                for (int i = 0; i < XValue.Count; i++)
                {
                    S.setArgumentValue(XValue[i]);
                    xValue.Add(AValue[i] * KXS.calculate());
                }
                xMatrix.Add(xValue);

                xMatrix[i1][i1] += 1;

                FreeValue.Add(FX.calculate());
            }
            List<double> xMatrixOneDimentional = new List<double> { };
            foreach (var el in xMatrix)
            {
                foreach (var el2 in el)
                {
                    xMatrixOneDimentional.Add(el2);
                }
            }

            var yValue = GaussienInvoke(xMatrixOneDimentional, FreeValue, xMatrix.Count, xMatrix.First().Count);

            AnswerValue = FunctionLine + " - ("; ;
            for (int i = 0; i <= ErrorNValue; i++)
            {
                AnswerValue += " + " + $"{Math.Round(AValue[i], 4)}" + " * " + IntegralFunctionLine.Replace("s", Math.Round(XValue[i], 4).ToString()) + " * " + Math.Round(yValue[i], 4);
            }
            AnswerValue += " )";

            double stepValue = (B - A) / 50;
            var z = new DataModel { XYValue = new ObservableCollection<XYDataModel> { } };
            for (int i = 0; i < 50; i++)
            {
                double xValue = XValue.First() + stepValue * i;
                X.setArgumentValue(xValue);

                double yvalue = FX.calculate();
                for (int j = 0; j <= ErrorNValue; j++)
                {
                    S.setArgumentValue(XValue[j]);
                    yvalue += (AValue[j] * KXS.calculate() * yValue[j]);
                }

                z.XYValue.Add(new XYDataModel { X = xValue, Y = yvalue });
            }
            DataModel = z;
            return 0;
        }

        public List<double> GaussienInvoke(List<double> ElementValue, List<double> FreeValue, int n = 0, int m = 0)
        {
            if (n <= 0 || m <= 0)
            {
                m = n = (int)Math.Sqrt((double)ElementValue.Count);
            }
            else if (n * m != ElementValue.Count)
            {
                return null;
            }
            double FirstElement = ElementValue[0];
            for (int i = 0; i < m; i++)
                ElementValue[i] /= FirstElement;
            FreeValue[0] /= FirstElement;
            for (int step = 0; step < 2; step++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    for (int i = j + 1; i < n; i++)
                    {
                        double ValueExpression = ElementValue[i * n + j] / ElementValue[j * n + j];
                        for (int k = j; k < m; k++)
                        {
                            ElementValue[i * n + k] -= (ValueExpression * ElementValue[j * n + k]);
                        }
                        FreeValue[i] -= (ValueExpression * FreeValue[j]);

                        if (step == 0)
                        {
                            double Element = ElementValue[i * n + i];
                            for (int k = j; k < m; k++)
                            {
                                ElementValue[i * n + k] /= Element;
                            }
                            FreeValue[i] /= Element;
                        }
                    }
                }
                FreeValue = Reverse(FreeValue);
                ElementValue = Reverse(ElementValue);
            }

            return FreeValue;
        }

        public List<double> Reverse(List<double> valueX)
        {
            List<double> returnValue = new List<double> { };

            for (int i = valueX.Count - 1; i >= 0; i--)
            {
                returnValue.Add(valueX[i]);
            }
            return returnValue;
        }
    }
}
