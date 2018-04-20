using ComputerCenter.ViewModel;
using Data.Model;
using System.Windows;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for GlobalSessionDetails.xaml
    /// </summary>
    public partial class GlobalSessionDetails
    {
        private readonly GlobalSessionDetailsViewModel _viewModel;

        public GlobalSessionDetails(Session session)
        {
            _viewModel = new GlobalSessionDetailsViewModel(session);
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void OpenContextHelp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenContextHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.SessionDetails };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}