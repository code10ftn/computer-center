using ComputerCenter.ViewModel;
using Data.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for ConfirmDelete.xaml
    /// </summary>
    public partial class ConfirmDelete : Window
    {
        private readonly ConfirmDeleteViewModel _viewModel;
        public bool Confirm { get; set; }
        public Constants.ApplicationState HelpState { get; set; }

        public ConfirmDelete(List<Session> sessions, string message)
        {
            _viewModel = new ConfirmDeleteViewModel(sessions);
            DataContext = _viewModel;
            InitializeComponent();

            MessageTextBlock.Text = message;
            if (sessions.Count == 0)
            {
                this.Height = 230;
                this.Width = 420;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void ConfirmedDelete(object sender, RoutedEventArgs e)
        {
            Confirm = true;
            Close();
        }

        private void NotConfirmedDelete(object sender, RoutedEventArgs e)
        {
            Confirm = false;
            Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _viewModel.MaxFieldWidth = (sender as Window).ActualWidth - 50;
        }

        private void OpenContextHelp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenContextHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel { State = HelpState };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}