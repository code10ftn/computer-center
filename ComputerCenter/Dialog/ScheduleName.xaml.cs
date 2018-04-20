using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Model;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for ScheduleName.xaml
    /// </summary>
    public partial class ScheduleName : IDemoState
    {
        private ScheduleNameViewModel _viewModel;
        public string Name { get; set; }

        public ScheduleName()
        {
            _viewModel = new ScheduleNameViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            Name = _viewModel.ScheduleName;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(
                    () => { _viewModel.ScheduleName = _viewModel.ScheduleDao.FindAvailableDemoId("DemoSchedule"); });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Save();
                    DemoStateHandler.Instance.DemoScheduleId = _viewModel.ScheduleName;
                    DemoStateHandler.Instance.DemoState = Application.Current.MainWindow as MainWindow;
                });
            });
        }

        public bool DemoStopped()
        {
            for (var i = 0; i < 10; i++)
            {
                if (DemoStateHandler.Instance.Stopped)
                {
                    Application.Current.Dispatcher.Invoke(Close);
                    return true;
                }
                Thread.Sleep(100);
            }
            return false;
        }

        private void OpenContextHelp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenContextHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.ScheduleAdd };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}