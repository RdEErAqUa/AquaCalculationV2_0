using AquaCalculationV2_0.ViewModels.Base;
using AquaCalculationV2_0.ViewModels.Lab1;
using AquaCalculationV2_0.ViewModels.Lab2;
using AquaCalculationV2_0.ViewModels.Lab3;
using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCalculationV2_0.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public Lab1ViewModel Lab1Model { get; }
        public Lab2ViewModel Lab2Model { get; }
        public Lab3ViewModel Lab3Model { get; }
        public MainWindowViewModel(Lab1ViewModel lab1ViewModel, Lab2ViewModel lab2ViewModel, Lab3ViewModel lab3ViewModel)
        {
            this.Lab1Model = lab1ViewModel;
            this.Lab2Model = lab2ViewModel;
            this.Lab3Model = lab3ViewModel;
        }
    }
}
