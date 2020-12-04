using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AquaCalculationV2_0.ViewModels.Base
{
    class ViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Шаблон события изменение елемента
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;

        }
        #endregion

        #region Деструктор
        //~ViewModel()
        //{
        //    Dispose(false);
        //}

        public void Dispose()
        {
        }

        private bool _Disposed;

        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _Disposed) return;
            _Disposed = true;
            //Освобождение памяти 
        }
        #endregion
    }
}
