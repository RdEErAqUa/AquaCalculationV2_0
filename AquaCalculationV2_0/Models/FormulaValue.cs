using AquaCalculationV2_0.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models
{
    class FormulaValue : ModelBase
    {
        #region A : double - формула от A ДО B
        private double _A;
        public double A { get => _A; set => Set(ref _A, value); }
        #endregion

        #region B : double - формула от A ДО B
        private double _B;
        public double B { get => _B; set => Set(ref _B, value); }
        #endregion

        #region String : Formula - формула

        private String _Formula;

        public String Formula { get => _Formula; set => Set(ref _Formula, value); }

        #endregion
    }
}
