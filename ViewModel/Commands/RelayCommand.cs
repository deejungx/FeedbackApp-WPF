using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace RestaurantFeedbackApp.ViewModel.Commands
{
    public class RelayCommand<T> : ICommand
    {
        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;

        /* 
         * <summary>
         * Creates a new command.
         * </summary>
         * <param name="execute">The execution logic.</param>
         * <param name="canExecute">The execution status logic.</param>
        */
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action<T> execute)
        : this(execute, null)
        {
        }

        /* 
         * <summary>
         * Defines the method that determines whether the command can execute in its 
         * current state.
         * </summary>
         * <param name="parameter">Data used by the command.  If the command 
         * does not require data to be passed, this object can be set to null.</param>
         * <returns>
         * true if this command can be executed; otherwise, false.
         * </returns> 
        */
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        /*
         * <summary>
         * Defines the method to be called when the command is invoked.
         * </summary>
         * <param name="parameter">Data used by the command. If the command does not 
         * require data to be passed, this object can be set to <see langword="null" />.
         * </param>
        */
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        /*
         * <summary>
         * Occurs when changes occur that affect whether or not the command 
         * should execute.
         * </summary>
        */
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
