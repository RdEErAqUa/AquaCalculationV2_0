using AquaCalculationV2_0.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCalculationV2_0.Models
{
    class ThirdDimensionalData : ModelBase
    {
        private double _X;
        public double X { get => _X; set => Set(ref _X, value); }

        private double _Y;
        public double Y { get => _Y; set => Set(ref _Y, value); }

        private double _Z;
        public double Z { get => _Z; set => Set(ref _Z, value); }
    }
}
