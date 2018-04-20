using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Model;
using System;
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
    /// Interaction logic for ClassroomList.xaml
    /// </summary>
    public partial class ClassroomList : IDemoState
    {
        private readonly ClassroomListViewModel _viewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ClassroomList(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _viewModel = new ClassroomListViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new ClassroomDetails(_viewModel.Classrooms, _mainWindowViewModel.Classrooms, _mainWindowViewModel)
                .ShowDialog();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var tag = (string)((Button)sender).Tag;

            var dialog = new ConfirmDelete(new List<Session>(),
                "Are you sure you want to delete classroom with id " + tag +
                "? All the sessions that are being held in this classroom will be deleted from schedule.")
            {
                Owner = this,
                HelpState = Constants.ApplicationState.ClassroomsView
            };

            dialog.ShowDialog();

            if (dialog.Confirm)
            {
                _viewModel.RemoveClassroom((string)tag);
                _mainWindowViewModel.RemoveClassroom(tag);

                _mainWindowViewModel.InitRemainigSessions();
                _mainWindowViewModel.InitSessions();
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;
            new ClassroomDetails((string)tag, _viewModel.Classrooms, _mainWindowViewModel.Classrooms,
                _mainWindowViewModel).ShowDialog();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void FilterList(object sender, EventArgs e)
        {
            if (DataGrid?.ItemsSource == null) return;

            var collectionView = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);
            collectionView.Filter = o =>
            {
                var c = o as Classroom;
                if (c == null) return false;

                var query = _viewModel.Query.ToLower();

                bool projector;
                switch (_viewModel.HasProjector)
                {
                    case "Yes":
                        projector = c.HasProjector;
                        break;

                    case "No":
                        projector = !c.HasProjector;
                        break;

                    default:
                        projector = true;
                        break;
                }

                bool blackboard;
                switch (_viewModel.HasBlackboard)
                {
                    case "Yes":
                        blackboard = c.HasBlackboard;
                        break;

                    case "No":
                        blackboard = !c.HasBlackboard;
                        break;

                    default:
                        blackboard = true;
                        break;
                }

                bool smartboard;
                switch (_viewModel.HasSmartboard)
                {
                    case "Yes":
                        smartboard = c.HasSmartboard;
                        break;

                    case "No":
                        smartboard = !c.HasSmartboard;
                        break;

                    default:
                        smartboard = true;
                        break;
                }

                bool platform;
                switch (_viewModel.Platform)
                {
                    case "Windows":
                        platform = c.Platform == Constants.Platform.Windows ||
                                   c.Platform == Constants.Platform.Both;
                        break;

                    case "Linux":
                        platform = c.Platform == Constants.Platform.Linux ||
                                   c.Platform == Constants.Platform.Both;
                        break;

                    case "Both":
                        platform = c.Platform == Constants.Platform.Both;
                        break;

                    default:
                        platform = true;
                        break;
                }

                return (c.Id.ToLower().Contains(query) ||
                        c.Description != null && c.Description.ToLower().Contains(query)) &&
                       projector && blackboard && smartboard && platform;
            };
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var dialog = new ClassroomDetails(_viewModel.Classrooms, _mainWindowViewModel.Classrooms,
                        _mainWindowViewModel);
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
            var viewModel = new ContextHelpViewModel {State = Constants.ApplicationState.ClassroomsView};
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}