using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for ClassroomDetails.xaml
    /// </summary>
    public partial class ClassroomDetails : IDemoState
    {
        private readonly bool _adding;
        private readonly ClassroomDetailsViewModel _viewModel;
        private ObservableCollection<Classroom> _classrooms;
        private ObservableCollection<Classroom> _mainWindowClassrooms;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ClassroomDetails(ObservableCollection<Classroom> viewModelClassrooms,
            ObservableCollection<Classroom> mainWindowViewModelClassrooms, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = true;
            _viewModel = new ClassroomDetailsViewModel(_mainWindowViewModel);
            Init(viewModelClassrooms, mainWindowViewModelClassrooms);
        }

        public ClassroomDetails(string id, ObservableCollection<Classroom> viewModelClassrooms,
            ObservableCollection<Classroom> mainWindowViewModelClassrooms, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = false;
            _viewModel = new ClassroomDetailsViewModel(id, _mainWindowViewModel);
            Init(viewModelClassrooms, mainWindowViewModelClassrooms);
        }

        private void Init(ObservableCollection<Classroom> viewModelClassrooms,
            ObservableCollection<Classroom> mainWindowViewModelClassrooms)
        {
            _classrooms = viewModelClassrooms;
            _mainWindowClassrooms = mainWindowViewModelClassrooms;
            DataContext = _viewModel;

            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var classroom = _viewModel.SaveClassroom();
            if (_adding)
            {
                _classrooms.Add(classroom);
                _mainWindowClassrooms.Add(classroom);
            }
            else
            {
                var item = _classrooms.First(c => c.Id == classroom.Id);
                item.Update(classroom);
            }
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _viewModel.MaxFieldWidth = (e.Source as Window).ActualWidth;
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(
                    () => { _viewModel.Id = _viewModel.ClassroomDao.FindAvailableDemoId("DemoClassroom"); });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Description = "DemoClassroomDescription"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.HasProjector = true; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.HasBlackboard = true; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.HasSmartboard = true; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Seats = 20; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Platform = Constants.Platform.Both; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Save();
                    DemoStateHandler.Instance.DemoClassroomId = _viewModel.Id;
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
            var viewModel = new ContextHelpViewModel {State = Constants.ApplicationState.ClassroomsAdd};
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}