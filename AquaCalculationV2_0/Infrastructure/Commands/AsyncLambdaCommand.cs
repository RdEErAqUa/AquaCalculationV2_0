using AquaCalculationV2_0.Infrastructure.Async;
using AquaCalculationV2_0.Infrastructure.Commands.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AquaCalculationV2_0.Infrastructure.Commands
{
    internal class AsyncLambdaCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public AsyncLambdaCommand(
            Func<object, Task> execute,
            Func<object, bool> canExecute = null,
            IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute(object p)
        {
            return !_isExecuting && (_canExecute?.Invoke(p) ?? true);
        }

        public async Task ExecuteAsync(object p)
        {
            if (CanExecute(p))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(p);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync(parameter).FireAndForgetSafeAsync(_errorHandler);
        }
    }
}
