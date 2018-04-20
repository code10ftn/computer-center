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
    /// Interaction logic for SubjectList.xaml
    /// </summary>
    public partial class SubjectList : IDemoState
    {
        private readonly SubjectListViewModel _viewModel;
        private MainWindowViewModel _mainWindowViewModel;

        public SubjectList(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _viewModel = new SubjectListViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new SubjectDetails(_viewModel.Subjects, _mainWindowViewModel).ShowDialog();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var tag = (string)((Button)sender).Tag;

            var dialog = new ConfirmDelete(new List<Session>(),
                "Are you sure you want to delete subject with id " + tag +
                "? All the sessions of this subject will be removed from schedule.")
            {
                Owner = this,
                HelpState = Constants.ApplicationState.SubjectsView
            };

            dialog.ShowDialog();

            if (dialog.Confirm)
            {
                _viewModel.RemoveSubject((string)tag);
                _mainWindowViewModel.InitRemainigSessions();
                _mainWindowViewModel.InitSessions();
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;
            new SubjectDetails((string)tag, _viewModel.Subjects, _mainWindowViewModel).ShowDialog();
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
                var s = o as Subject;
                if (s == null) return false;

                var query = _viewModel.Query.ToLower();

                bool projector;
                switch (_viewModel.RequiresProjector)
                {
                    case "Yes":
                        projector = s.RequiresProjector;
                        break;

                    case "No":
                        projector = !s.RequiresProjector;
                        break;

                    default:
                        projector = true;
                        break;
                }

                bool blackboard;
                switch (_viewModel.RequiresBlackboard)
                {
                    case "Yes":
                        blackboard = s.RequiresBlackboard;
                        break;

                    case "No":
                        blackboard = !s.RequiresBlackboard;
                        break;

                    default:
                        blackboard = true;
                        break;
                }

                bool smartboard;
                switch (_viewModel.RequiresSmartboard)
                {
                    case "Yes":
                        smartboard = s.RequiresSmartboard;
                        break;

                    case "No":
                        smartboard = !s.RequiresSmartboard;
                        break;

                    default:
                        smartboard = true;
                        break;
                }

                bool platform;
                switch (_viewModel.Platform)
                {
                    case "Windows":
                        platform = s.RequiredPlatform == Constants.Platform.Windows;
                        break;

                    case "Linux":
                        platform = s.RequiredPlatform == Constants.Platform.Linux;
                        break;

                    case "Any":
                        platform = s.RequiredPlatform == Constants.Platform.Any;
                        break;

                    default:
                        platform = true;
                        break;
                }

                return (s.Id.ToLower().Contains(query) ||
                        s.Description != null && s.Description.ToLower().Contains(query)) &&
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
                    var dialog = new SubjectDetails(_viewModel.Subjects, _mainWindowViewModel);
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
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.SubjectsView };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}