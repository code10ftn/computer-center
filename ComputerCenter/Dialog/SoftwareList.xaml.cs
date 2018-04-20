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
    /// Interaction logic for SoftwareList.xaml
    /// </summary>
    public partial class SoftwareList : IDemoState
    {
        private readonly SoftwareListViewModel _viewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public SoftwareList(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _viewModel = new SoftwareListViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new SoftwareDetails(_viewModel.SoftwareList, _mainWindowViewModel).ShowDialog();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;

            var dialog = new ConfirmDelete(new List<Session>(),
                "Are you sure you want to delete software with id " + tag + "?")
            {
                Owner = this,
                HelpState = Constants.ApplicationState.SoftwaresView
            };

            dialog.ShowDialog();

            if (dialog.Confirm)
            {
                _viewModel.RemoveSoftware((string)tag);
                _mainWindowViewModel.InitRemainigSessions();
                _mainWindowViewModel.InitSessions();
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;
            new SoftwareDetails((string)tag, _viewModel.SoftwareList, _mainWindowViewModel).ShowDialog();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
                var s = o as Software;
                if (s == null) return false;

                var query = _viewModel.Query.ToLower();

                bool platform;
                switch (_viewModel.Platform)
                {
                    case "Windows":
                        platform = s.Platform == Constants.Platform.Windows ||
                                   s.Platform == Constants.Platform.Both;
                        break;

                    case "Linux":
                        platform = s.Platform == Constants.Platform.Linux ||
                                   s.Platform == Constants.Platform.Both;
                        break;

                    case "Both":
                        platform = s.Platform == Constants.Platform.Both;
                        break;

                    default:
                        platform = true;
                        break;
                }

                return (s.Id.ToLower().Contains(query) ||
                        s.Description != null && s.Description.ToLower().Contains(query)) &&
                       platform;
            };
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var dialog = new SoftwareDetails(_viewModel.SoftwareList, _mainWindowViewModel);
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
            var viewModel = new ContextHelpViewModel {State = Constants.ApplicationState.SoftwaresView};
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}