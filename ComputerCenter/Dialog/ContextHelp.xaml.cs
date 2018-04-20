using ComputerCenter.ViewModel;
using Data.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for ContextHelp.xaml
    /// </summary>
    public partial class ContextHelp
    {
        private readonly ContextHelpViewModel _viewModel;

        public ContextHelp()
        {
            InitializeComponent();
        }

        public ContextHelp(ContextHelpViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void HelpItem_Selected(object sender, RoutedEventArgs e)
        {
            var treeViewItem = e.OriginalSource as TreeViewItem;
            if (treeViewItem != null)
            {
                var name = treeViewItem.Name;
                _viewModel.State = (Constants.ApplicationState)Enum.Parse(typeof(Constants.ApplicationState), name);
            }
        }
    }
}