using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Dao;
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
    /// Interaction logic for SubjectDetails.xaml
    /// </summary>
    public partial class SubjectDetails : IDemoState
    {
        private readonly bool _adding;
        private readonly SubjectDetailsViewModel _viewModel;
        private ObservableCollection<Subject> _subjects;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public SubjectDetails(ObservableCollection<Subject> viewModelSubjects, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = true;
            _viewModel = new SubjectDetailsViewModel(_mainWindowViewModel);
            Init(viewModelSubjects);
        }

        public SubjectDetails(string id, ObservableCollection<Subject> viewModelSubjects,
            MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = false;
            _viewModel = new SubjectDetailsViewModel(id, _mainWindowViewModel);
            Init(viewModelSubjects);

            var course = _viewModel.Courses.FirstOrDefault(c => c.Id == _viewModel.Course.Id);
            CourseComboBox.SelectedIndex = _viewModel.Courses.IndexOf(course);
        }

        private void Init(ObservableCollection<Subject> viewModelsubjects)
        {
            _subjects = viewModelsubjects;
            DataContext = _viewModel;

            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var subject = _viewModel.SaveSubject();
            if (_adding)
            {
                _subjects.Add(subject);
                _mainWindowViewModel.InitRemainigSessions();
                _mainWindowViewModel.InitSessions();
            }
            else
            {
                if (subject != null)
                {
                    var item = _subjects.First(s => s.Id == subject.Id);
                    item.Update(subject);
                }
            }
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
                    () => { _viewModel.Id = _viewModel.SubjectDao.FindAvailableDemoId("DemoSubject"); });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Name = "DemoSubjectName"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Description = "DemoSubjectDescription"; });
                if (DemoStopped()) return;
                var course = new CourseDao().FindById(DemoStateHandler.Instance.DemoCourseId);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _viewModel.Course = course;
                    var selectedCourse = _viewModel.Courses.FirstOrDefault(c => c.Id == _viewModel.Course.Id);
                    CourseComboBox.SelectedIndex = _viewModel.Courses.IndexOf(selectedCourse);
                });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.RequiredPlatform = Constants.Platform.Any; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.RequiredTerms = 2; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Save();
                    DemoStateHandler.Instance.DemoSubjectId = _viewModel.Id;
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
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.SubjectsAdd };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}