using AquaCalculationV2_0.Models;
using AquaCalculationV2_0.Servises.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AquaCalculationV2_0.Servises.Interpolation.Interfaces;
using AquaCalculationV2_0.Data.Differentiation;
using org.mariuszgromada.math.mxparser;

namespace AquaCalculationV2_0.Servises
{
    class IntegralRectangles : IIntegral
    {
        private ILagrangeInterpolation lagrangeInterpolation { get; set; }
        public IntegralRectangles(ILagrangeInterpolation lagrangeInterpolation)
        {
            this.lagrangeInterpolation = lagrangeInterpolation;   
        }

        public ICollection<XYDataModel> FullfillWithStep(ICollection<XYDataModel> DataValue, double step, double a, double b)
        {
            List<double> x1 = DataValue.Select(x => x.X).ToList();
            List<double> y1 = DataValue.Select(x => x.Y).ToList();
            List<double> x = new List<double> { };
            List<double> y = new List<double> { };
            for (double i = a; i <= b; i += step)
            {
                    y.Add(lagrangeInterpolation.InterpolationPolynom(x1, y1, i, x1.Count));
                    x.Add(i);
            }
            ICollection<XYDataModel> newValue = new List<XYDataModel> { };
            for(int i = 0; i < x.Count; i++)
            {
                newValue.Add(new XYDataModel { X = x[i], Y = y[i]});
            }

            newValue = newValue.OrderBy(x => x.X).ToList();

            return newValue;
        }
        public ICollection<XYDataModel> FullfillWithStep(double step, double a, double b, string Formula)
        {
            Argument x = new Argument($"x = {0}");

            Expression eh = new Expression(Formula, x);

            ICollection<XYDataModel> xYDatas = new List<XYDataModel> { };

            for (double i = a; Math.Round(i, 6) <= b; i += step)
            {
                x.setArgumentValue(Math.Round(i, 6));

                xYDatas.Add(new XYDataModel { X = Math.Round(i, 6), Y = eh.calculate() });

            }
            return xYDatas;
        }

        public double IntegralRunWithStep(IntegralModel integralModels, double a, double b, double step, string Formula = null)
        {
            Argument x = new Argument($"x = {0}");

            Expression eh = new Expression(Formula, x);

            double IntegralValueH;
            step = integralModels.H = (b - a) / (step);

            ICollection<XYDataModel> xYDatas = String.IsNullOrEmpty(Formula) ? FullfillWithStep(integralModels.XYValue,step, a, b) : FullfillWithStep(step, a, b, Formula);

            IntegralValueH = Integral(xYDatas, integralModels.H);

            integralModels.XYValue = new System.Collections.ObjectModel.ObservableCollection<XYDataModel>(xYDatas);
            integralModels.XYBuild = new System.Collections.ObjectModel.ObservableCollection<XYDataModel>(xYDatas);

            return IntegralValueH;
        }

        public double IntegralRun(IntegralModel integralModels, double a, double b, double E, string Formula = null)
        {
            double n = 1;

            double IntegralValueH = -10000;
            double IntegralValueH2 = -10000;
            while (true)
            {
                
                double step = integralModels.H = (b - a) / (n);
                ICollection<XYDataModel> xYDatas = String.IsNullOrEmpty(Formula) ? FullfillWithStep(integralModels.XYValue, step, a, b) : FullfillWithStep(step, a, b, Formula);

                IntegralValueH = Integral(xYDatas, integralModels.H);

                double errorValue = (Math.Abs(IntegralValueH - IntegralValueH2));
                if (errorValue > E)
                {
                    n *= 2;
                    IntegralValueH2 = IntegralValueH;
                }
                else
                {
                    integralModels.XYValue = new System.Collections.ObjectModel.ObservableCollection<XYDataModel>(xYDatas);
                    integralModels.XYBuild = new System.Collections.ObjectModel.ObservableCollection<XYDataModel>(xYDatas);
                    break;
                }
            }

            return IntegralValueH2;
        }

        public double Integral(ICollection<XYDataModel> xYDatas, double step)
        {
            double integralValue = 0;

            for (int i = 0; i < xYDatas.Count - 1; i++)
                integralValue += xYDatas.ElementAt(i).Y;

            return (integralValue * step);
        }
    }
}
