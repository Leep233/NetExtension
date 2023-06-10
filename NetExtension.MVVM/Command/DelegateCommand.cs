using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            bool canExe = true;

            if (canExecute is not null)
                canExe = canExecute.Invoke();

            return canExe;
        }

        public  void Execute(object? parameter)
        {
            if (execute is not null)
                execute.Invoke();

            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute() {
            return command.CanExecute(null);
        }
  
        public void Execute() 
        {
            command.Execute(null) ;
        }

        public DelegateCommand(Func<bool> canExecute, Action execute)
        {
            this.canExecute = new Func<bool>(canExecute);

            this.execute = execute;

            this.command = this;

            _synchronizationContext = SynchronizationContext.Current;

        }

        public void RaiseCanExecuteChanged() {
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

    public class DelegateCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;


        private Func<T,bool> canExecute;

        private Action<T> execute;

        private readonly ICommand command;
       public bool CanExecute(object? parameter)
        {
            bool canExe = true;

            if (canExecute is not null) 
            { 
                if(parameter is T args)
                    canExe = canExecute.Invoke(args);
            }
               

            return canExe;
        }

        public void Execute(object? parameter)
        {
            if (execute is not null)
            {
                if (parameter is T args)
                    execute.Invoke(args);
            }                
        }

    

        public DelegateCommand(Func<T,bool> canExecute, Action<T> execute)
        {
            this.canExecute = canExecute;

            this.execute = execute;

            this.command = this;
        }
    }

}
