using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for SoftwareDetails.xaml
    /// </summary>
    public partial class SoftwareDetails : IDemoState
    {
        private readonly bool _adding;
        private readonly SoftwareDetailsViewModel _viewModel;
        private readonly ObservableCollection<Software> _softwareList;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public SoftwareDetails(ObservableCollection<Software> viewModelSoftwareList,
            MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = true;
            _softwareList = viewModelSoftwareList;
            _viewModel = new SoftwareDetailsViewModel();

            DataContext = _viewModel;
            InitializeComponent();
        }

        public SoftwareDetails(string id, ObservableCollection<Software> viewModelSoftwareList,
            MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = false;
            _softwareList = viewModelSoftwareList;
            _viewModel = new SoftwareDetailsViewModel(id);

            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var software = _viewModel.SaveSoftware();
            if (_adding)
            {
                _softwareList.Add(software);
            }
            else
            {
                var item = _softwareList.First(s => s.Id == software.Id);
                item.Update(software);
            }
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void YearReleased_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Util.Util.IsNumber(e.Text);
        }

        private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Util.Util.IsNumber(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!Util.Util.IsNumber(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void Price_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "$0.00";
            }
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(
                    () => { _viewModel.Id = _viewModel.SoftwareDao.FindAvailableDemoId("DemoSoftware"); });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Name = "DemoSoftwareName"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Maker = "DemoSoftwareMaker"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.MakerWebsite = "www.demosoftware.com"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Description = "DemoSoftwareDescription"; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() => { _viewModel.Platform = Constants.Platform.Both; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Save();
                    DemoStateHandler.Instance.DemoSoftwareId = _viewModel.Id;
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
            var viewModel = new ContextHelpViewModel {State = Constants.ApplicationState.SoftwaresAdd};
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}