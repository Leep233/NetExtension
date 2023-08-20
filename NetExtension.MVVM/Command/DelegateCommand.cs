using System;
using System.Windows.Input;
using System.Threading;

namespace NetExtension.Command
{

    public class DelegateCommand : ICommand
    {

        private readonly SynchronizationContext? _synchronizationContext;

        public event EventHandler? CanExecuteChanged;


        private Func<bool> canExecute;

        private Action execute;

        private readonly ICommand command;


        bool ICommand.CanExecute(object? parameter)
        {
            return (canExecute is null) ? true : canExecute.Invoke();
        }

        void ICommand.Execute(object? parameter)
        {
            execute?.Invoke();
        }

        public bool CanExecute()
        {
            return command.CanExecute(null);
        }

        public void Execute()
        {
            command.Execute(null);
        }

        public DelegateCommand(Func<bool> canExecute, Action execute)
        {
            this.canExecute = new Func<bool>(canExecute);

            this.execute = execute;

            this.command = this;

            _synchronizationContext = SynchronizationContext.Current;
        }

        public void RaiseCanExecuteChanged()
        {
            if (_synchronizationContext != null)
            {
                _synchronizationContext.Post(o => { CanExecuteChanged?.Invoke(o, EventArgs.Empty); }, EventArgs.Empty);
            }
            else
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

    }

    public class DelegateCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly SynchronizationContext? _synchronizationContext;

        private Func<T, bool> canExecute;

        private Action<T> execute;

        private readonly ICommand command;

        public DelegateCommand(Func<T, bool> canExecute, Action<T> execute)
        {
            this.canExecute = canExecute;

            this.execute = execute;

            this.command = this;

            _synchronizationContext = SynchronizationContext.Current;
        }


         bool ICommand.CanExecute(object? parameter)
        {

           return (parameter is T param)?CanExecute(param):true;
        }

         void ICommand.Execute(object? parameter)
        {
            if (parameter is T args)
            {
                Execute(args);
            }
        }


        public bool CanExecute(T parameter)
        {
            return canExecute is null ? true : canExecute.Invoke(parameter);
        }

        public void Execute(T parameter)
        {
             execute?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (_synchronizationContext != null)
            {
                _synchronizationContext.Post(o => { CanExecuteChanged?.Invoke(o, EventArgs.Empty); }, null);
            }
            else
            {
                CanExecuteChanged?.Invoke(null, EventArgs.Empty);
            }
        }


    }
}
