using System;
using System.Windows;
using System.Windows.Input;

namespace ComputerCenter.Command
{
    public class CloseThisWindowCommand : ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return (parameter is Window);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                ((Window)parameter).Close();
            }
        }

        #endregion ICommand Members

        private CloseThisWindowCommand()
        {
        }

        public static readonly ICommand Instance = new CloseThisWindowCommand();
    }
}