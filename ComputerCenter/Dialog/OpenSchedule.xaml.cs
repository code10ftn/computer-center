using ComputerCenter.ViewModel;
using Data.Model;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for OpenSchedule.xaml
    /// </summary>
    public partial class OpenSchedule
    {
        private readonly OpenScheduleViewModel _viewModel;
        public Schedule ScheduleToOpen { get; set; }

        public OpenSchedule()
        {
            _viewModel = new OpenScheduleViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            ScheduleToOpen = _viewModel.SelectedSchedule;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            ScheduleToOpen = null;
            Close();
        }

        public void DoDemo()
        {
            Thread.Sleep(2000);
            Close();
        }

        private void OpenContextHelp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenContextHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.ScheduleOpen };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}