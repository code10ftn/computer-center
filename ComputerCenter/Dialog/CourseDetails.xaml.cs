using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for CourseDetails.xaml
    /// </summary>
    public partial class CourseDetails : IDemoState
    {
        private readonly bool _adding;
        private readonly CourseDetailsViewModel _viewModel;
        private readonly ObservableCollection<Course> _courses;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public CourseDetails(ObservableCollection<Course> viewModelCourses, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = true;
            _courses = viewModelCourses;
            _viewModel = new CourseDetailsViewModel();

            DataContext = _viewModel;
            InitializeComponent();
        }

        public CourseDetails(string id, ObservableCollection<Course> viewModelCourses,
            MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = false;
            _courses = viewModelCourses;
            _viewModel = new CourseDetailsViewModel(id);

            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var course = _viewModel.SaveCourse();
            if (_adding)
            {
                _courses.Add(course);
            }
            else
            {
                var item = _courses.First(c => c.Id == course.Id);
                item.Update(course);
            }
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;

            if (picker == null) return;

            if (picker.SelectedDate == null)
            {
                picker.SelectedDate = _viewModel.Course.DateOpened;
            }
            else if (DateTime.Compare(picker.SelectedDate.Value, SqlDateTime.MinValue.Value) < 0)
            {
                picker.SelectedDate = DateTime.Now;
            }
        }

        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Util.Util.IsNumberOrDot(e.Text);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _viewModel.MaxFieldWidth = (e.Source as Window).ActualWidth - 56;
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(
                    () => { _viewModel.Id = _viewModel.CourseDao.FindAvailableDemoId("DemoCourse"); });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Name = "DemoCourseName"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Description = "DemoCourseDescription"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Save();
                    DemoStateHandler.Instance.DemoCourseId = _viewModel.Id;
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
            var viewModel = new ContextHelpViewModel {State = Constants.ApplicationState.CoursesAdd};
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}