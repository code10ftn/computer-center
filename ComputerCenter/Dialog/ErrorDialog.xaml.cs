using ComputerCenter.ViewModel;
using Data.Model;
using System.Windows;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog
    {
        public Constants.ApplicationState HelpState { get; set; }

        public ErrorDialog(string errorMessage)
        {
            InitializeComponent();
            MessageTextBlock.Text = errorMessage;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenContextHelp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenContextHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel {State = HelpState};
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}