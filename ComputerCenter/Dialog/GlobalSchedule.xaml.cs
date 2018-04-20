using ComputerCenter.Custom;
using ComputerCenter.ViewModel;
using Data.Model;
using Syncfusion.UI.Xaml.Schedule;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for GlobalSchedule.xaml
    /// </summary>
    public partial class GlobalSchedule
    {
        private readonly Schedule _schedule;
        private readonly GlobalScheduleViewModel _viewModel;

        public GlobalSchedule(Schedule schedule)
        {
            _schedule = schedule;
            _viewModel = new GlobalScheduleViewModel();
            DataContext = _viewModel;
            InitializeComponent();

            _viewModel.ScheduleAppointments = Schedule.Appointments;
            _viewModel.Schedule = _schedule;
            _viewModel.InitSessions();
        }

        private void Schedule_OnAppointmentStartDragging(object sender, AppointmentStartDraggingEventArgs e)
        {
            e.Cancel = true;
        }

        private void Schedule_OnAppointmentDragging(object sender, AppointmentDraggingEventArgs e)
        {
            e.Cancel = true;
        }

        private void Schedule_OnAppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            var sessionAppointment = e.Appointment as SessionAppointment;
            if (sessionAppointment != null)
            {
                var session = sessionAppointment.Session;
                new GlobalSessionDetails(session).ShowDialog();
            }
            e.Cancel = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true;
            }
        }

        private void Schedule_ContextMenuOpening(object sender,
            Syncfusion.UI.Xaml.Schedule.ContextMenuOpeningEventArgs e)
        {
            e.Cancel = true;
        }

        private void OpenContextHelp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenContextHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel
            {
                State = Constants.ApplicationState.ScheduleGlobal
            };

            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }
    }
}