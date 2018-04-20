using ComputerCenter.Custom;
using ComputerCenter.Demo;
using ComputerCenter.ViewModel;
using Data.Model;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComputerCenter.Dialog
{
    /// <summary>
    /// Interaction logic for SessionDetails.xaml
    /// </summary>
    public partial class SessionDetails : IDemoState
    {
        private readonly bool _adding;
        private ScheduleAppointmentCollection _appointments;
        private SessionDetailsViewModel _viewModel;
        private MainWindowViewModel _mainWindowViewModel;
        private RemainingSession _remainingSession;

        public SessionDetails()
        {
            InitializeComponent();
        }

        public SessionDetails(MainWindowViewModel mainWindowViewModel,
            ScheduleAppointmentCollection scheduleAppointments, int avaibleTerms, DateTime startTime,
            RemainingSession dragData)
        {
            _adding = true;
            _appointments = scheduleAppointments;
            _viewModel = new SessionDetailsViewModel(dragData.Subject.MinimumTermsPerSession, avaibleTerms);
            DataContext = _viewModel;
            _remainingSession = dragData;
            _mainWindowViewModel = mainWindowViewModel;
            _viewModel.Session.Subject = dragData.Subject;
            _viewModel.Session.Classroom = mainWindowViewModel.Classroom;
            _viewModel.Session.Day = startTime.DayOfWeek;
            _viewModel.Session.Time = startTime;
            _viewModel.Session.Schedule = mainWindowViewModel.LastOpenedSchedule;
            InitializeComponent();
            DeleteButton.Visibility = Visibility.Collapsed;
        }

        public SessionDetails(Session session, MainWindowViewModel mainWindowViewModel,
            ScheduleAppointmentCollection scheduleAppointments)
        {
            _adding = false;
            _appointments = scheduleAppointments;
            var avaibleTerms = mainWindowViewModel.GetAvailableTerms(session, session.Time,
                mainWindowViewModel.RemainingSessions.FirstOrDefault(s => s.Subject.Id == session.Subject.Id));
            _viewModel = new SessionDetailsViewModel(session.Subject.MinimumTermsPerSession, avaibleTerms);
            DataContext = _viewModel;
            _mainWindowViewModel = mainWindowViewModel;
            _viewModel.Session = session;
            InitializeComponent();
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (_adding)
            {
                var session = _viewModel.SaveSession();
                var sessionAppoinment = new SessionAppointment()
                {
                    Session = session,
                    Subject = session.Subject.Name,
                    StartTime = session.Time,
                    EndTime = session.Time + TimeSpan.FromMinutes(session.Terms * Subject.TermLenght),
                    RecurrenceRule = "week"
                };
                _appointments.Add(sessionAppoinment);
                _remainingSession.RemainingTerms -= session.Terms;
            }
            else
            {
                _viewModel.UpdateSession();
                var session = _viewModel.Session;
                var app =
                    (SessionAppointment)
                    _appointments.FirstOrDefault(a => (a as SessionAppointment).Session.Id == session.Id);
                app.Session = _viewModel.Session;
                app.EndTime = session.Time + TimeSpan.FromMinutes(session.Terms * Subject.TermLenght);

                _mainWindowViewModel.InitRemainigSessions();
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
                    () => { _viewModel.Term = 1; });
                if (DemoStopped()) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Save();
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
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.SessionDetails };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var dialog = new ConfirmDelete(new List<Session>(),
                "Are you sure you want to delete this session?")
            {
                HelpState = Constants.ApplicationState.SessionDetails
            };

            dialog.ShowDialog();

            if (dialog.Confirm)
            {
                _viewModel.DeleteSession();
                _mainWindowViewModel.InitSessions();
                _mainWindowViewModel.InitRemainigSessions();
                Close();
            }
        }
    }
}