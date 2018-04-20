using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for CourseList.xaml
    /// </summary>
    public partial class CourseList : IDemoState
    {
        private readonly CourseListViewModel _viewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public CourseList(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _viewModel = new CourseListViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new CourseDetails(_viewModel.Courses, _mainWindowViewModel).ShowDialog();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;

            var dialog = new ConfirmDelete(new List<Session>(),
                "Are you sure you want to delete course with id " + tag +
                "? All the subjects (with their respective sessions) that belong to this course will be deleted as well.")
            {
                Owner = this,
                HelpState = Constants.ApplicationState.CoursesView
            };

            dialog.ShowDialog();

            if (dialog.Confirm)
            {
                _viewModel.RemoveCourse((string)tag);
                _mainWindowViewModel.InitRemainigSessions();
                _mainWindowViewModel.InitSessions();
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;
            new CourseDetails((string)tag, _viewModel.Courses, _mainWindowViewModel).ShowDialog();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var query = textBox.Text;
            var collectionView = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);

            collectionView.Filter = o =>
            {
                var c = o as Course;

                return c != null &&
                       (c.Id.ToLower().Contains(query.ToLower()) ||
                        c.Name.ToLower().Contains(query.ToLower()) ||
                        c.Description != null && c.Description.ToLower().Contains(query.ToLower()));
            };
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var dialog = new CourseDetails(_viewModel.Courses, _mainWindowViewModel);
                    DemoStateHandler.Instance.DemoState = dialog;
                    dialog.ShowDialog();
                });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Close();
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
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.CoursesView };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}