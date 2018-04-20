using ComputerCenter.Custom;
using ComputerCenter.Demo;
using ComputerCenter.Dialog;
using ComputerCenter.Util;
using ComputerCenter.ViewModel;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ComputerCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDemoState
    {
        private readonly MainWindowViewModel _viewModel;

        private string _appTitle = "Computer Center Scheduler";

        private Point _dragStartPosition;
        private bool _dropped;
        private bool _madeChanges = true; // this should be changed when schedule logic is implemented

        private RemainingSession _dragData;

        private bool _dragging;

        private Constants.ApplicationState _nextDemoState = Constants.ApplicationState.ScheduleAdd;

        public MainWindow()
        {
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
            InitializeComponent();

            _viewModel.ScheduleAppointments = Schedule.Appointments;
            OpenLastSchedule();
        }

        private void OpenCourseList(object sender, RoutedEventArgs e)
        {
            new CourseList(_viewModel).ShowDialog();
        }

        private void OpenSoftwareList(object sender, RoutedEventArgs e)
        {
            new SoftwareList(_viewModel).ShowDialog();
        }

        private void OpenSubjectList(object sender, RoutedEventArgs e)
        {
            new SubjectList(_viewModel).ShowDialog();
        }

        private void OpenClassroomList(object sender, RoutedEventArgs e)
        {
            new ClassroomList(_viewModel).ShowDialog();
        }

        private void StartDemo(object sender, RoutedEventArgs e)
        {
            DemoStateHandler.Instance.InitDemo();
            DemoStateHandler.Instance.DemoState = this;
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPosition = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            var currentMousePosition = e.GetPosition(null);
            var diff = _dragStartPosition - currentMousePosition;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null) return;
                var data = (RemainingSession)listViewItem.DataContext;

                var dragData = new DataObject("myFormat", data);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T) return (T)current;
                current = VisualTreeHelper.GetParent(current);
            } while (current != null);
            return null;
        }

        private void Schedule_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Schedule_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                _dragData = e.Data.GetData("myFormat") as RemainingSession;

                var t = new Thread(MimicMouseClick);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
        }

        private void MimicMouseClick()
        {
            _dropped = true;
            for (var i = 0; i < 30; i++)
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Right)
                {
                    RoutedEvent = Mouse.PreviewMouseDownEvent,
                    Source = Schedule
                };
                Dispatcher.Invoke(() => { InputManager.Current.ProcessInput(e); });
            }
        }

        private void Schedule_AppointmentEditorOpening(object sender,
            Syncfusion.UI.Xaml.Schedule.AppointmentEditorOpeningEventArgs e)
        {
            if (_dropped)
            {
                var avaibleTerms = _viewModel.GetAvailableTerms(null, e.StartTime, _dragData);
                if (avaibleTerms == -1)
                {
                    var dialog = new ErrorDialog("Sessions are overlaping.")
                    {
                        Owner = this,
                        HelpState = Constants.ApplicationState.Schedule
                    };
                    dialog.ShowDialog();
                }
                else if (avaibleTerms < _dragData.Subject.MinimumTermsPerSession)
                {
                    var dialog =
                        new ErrorDialog($"Minimum session lenght is {_dragData.Subject.MinimumTermsPerSession} terms.")
                        {
                            Owner = this,
                            HelpState = Constants.ApplicationState.Schedule
                        };
                    dialog.ShowDialog();
                }
                else
                {
                    new SessionDetails(_viewModel, Schedule.Appointments, avaibleTerms, e.StartTime, _dragData)
                        .ShowDialog();
                }
                _dropped = false;
            }
            else
            {
                if (e.Appointment != null)
                    new SessionDetails((e.Appointment as SessionAppointment).Session, _viewModel, Schedule.Appointments)
                        .ShowDialog();
            }
            e.Cancel = true;
        }

        private void Schedule_ContextMenuOpening(object sender,
            Syncfusion.UI.Xaml.Schedule.ContextMenuOpeningEventArgs e)
        {
            e.Cancel = true;
        }

        private void NewSchedule_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewSchedule_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var scheduleDialog = new ScheduleName { Owner = this };
            scheduleDialog.ShowDialog();

            if (!string.IsNullOrEmpty(scheduleDialog.Name))
            {
                if (_viewModel.LastOpenedSchedule != null)
                {
                    _viewModel.LastOpenedSchedule.LastOpened = false;
                    _viewModel.UpdateCurrentSchedule();
                }

                _viewModel.LastOpenedSchedule = new Schedule
                {
                    Id = scheduleDialog.Name,
                    LastOpened = true
                };
                //_viewModel.ScheduleAppointments = Schedule.Appointments;
                _viewModel.SaveCurrentSchedule();
                _viewModel.InitRemainigSessions();

                // in case this is the first time running
                ScheduleView.Visibility = Visibility.Visible;
                ClassromsPanel.Visibility = Visibility.Visible;
                ScheduleLabel.Visibility = Visibility.Hidden;
                GlobalButton.IsEnabled = true;

                Schedule.Appointments.Clear(); // reset schedule

                Title = $"{scheduleDialog.Name} - {_appTitle}";
            }
        }

        private void OpenSchedule_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenSchedule_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var openScheduleDialog = new OpenSchedule() { Owner = this };
            openScheduleDialog.ShowDialog();

            if (openScheduleDialog.ScheduleToOpen != null)
            {
                var schedule = _viewModel.FindSchedule(openScheduleDialog.ScheduleToOpen.Id);
                if (_viewModel.LastOpenedSchedule != null && _viewModel.LastOpenedSchedule.Id != schedule.Id)
                {
                    _viewModel.LastOpenedSchedule.LastOpened = false;
                    _viewModel.UpdateCurrentSchedule();
                }

                _viewModel.LastOpenedSchedule = schedule;
                _viewModel.LastOpenedSchedule.LastOpened = true;
                _viewModel.UpdateCurrentSchedule();
                _viewModel.InitRemainigSessions();

                // in case this is the first time running
                ScheduleView.Visibility = Visibility.Visible;
                ClassromsPanel.Visibility = Visibility.Visible;
                ScheduleLabel.Visibility = Visibility.Hidden;
                GlobalButton.IsEnabled = true;
                // clear schedule from screen
                // repopulate schedule
                //_viewModel.ScheduleAppointments = Schedule.Appointments;
                _viewModel.InitSessions();

                Title = $"{schedule.Id} - {_appTitle}";
            }
        }

        private void ToggleMenu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dragging)
            {
                e.CanExecute = false;
                _dragging = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void ToggleMenu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DrawerHost.IsLeftDrawerOpen = !DrawerHost.IsLeftDrawerOpen;
        }

        private void Schedule_AppointmentEndDragging(object sender,
            Syncfusion.UI.Xaml.Schedule.AppointmentEndDraggingEventArgs e)
        {
            _dragging = false;
            var app = e.Appointment as SessionAppointment;
            var appSpan = new TimeSpan(0, app.Session.Terms * Subject.TermLenght, 0);
            var appEnd = e.To.TimeOfDay + appSpan;
            if (e.To.TimeOfDay.Hours < 7 || (appEnd >= new TimeSpan(0, 22, 15, 0)))
            {
                app.StartTime = app.Session.Time;
                app.EndTime = app.StartTime + appSpan;
                e.Cancel = true;
                return;
            }

            if (e.To.TimeOfDay.Minutes % 15 != 0)
            {
                var diff = e.To.TimeOfDay.Minutes % 15;
                e.To = new DateTime(e.To.Year, e.To.Month, e.To.Day, e.To.Hour, e.To.Minute - diff, 0);
            }

            foreach (var scheduleAppointment in Schedule.Appointments)
            {
                if (scheduleAppointment != app &&
                    SessionOverlap(scheduleAppointment.StartTime, scheduleAppointment.EndTime, e.To,
                        e.To.Add(appSpan)))
                {
                    e.Cancel = true;
                    return;
                }
            }

            app.StartTime = e.To;
            app.EndTime = app.StartTime.Add(appSpan);
            app.Session.Day = app.StartTime.DayOfWeek;
            app.Session.Time = app.StartTime;

            _viewModel.UpdateSession(app.Session);

            e.Cancel = true;
        }

        private bool SessionOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (end1.CompareTo(start2) <= 0 || start1.CompareTo(end2) >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Schedule_AppointmentStartResizing(object sender,
            Syncfusion.UI.Xaml.Schedule.AppointmentStartResizingEventArgs e)
        {
            e.Cancel = true;
        }

        private void Schedule_AppointmentEndResizing(object sender,
            Syncfusion.UI.Xaml.Schedule.AppointmentEndResizingEventArgs e)
        {
            e.Cancel = true;
        }

        public void DoDemo()
        {
            Task.Factory.StartNew(() =>
            {
                if (DemoStopped()) return;
                switch (_nextDemoState)
                {
                    case Constants.ApplicationState.ScheduleAdd:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            DrawerHost.IsLeftDrawerOpen = false;
                            var dialog = new ScheduleName { Owner = this };
                            DemoStateHandler.Instance.DemoState = dialog;
                            dialog.ShowDialog();

                            if (!string.IsNullOrEmpty(dialog.Name))
                            {
                                if (_viewModel.LastOpenedSchedule != null)
                                {
                                    _viewModel.UpdateCurrentSchedule();
                                }

                                _viewModel.LastOpenedSchedule = new Schedule
                                {
                                    Id = dialog.Name,
                                };

                                _viewModel.SaveCurrentSchedule();
                                _viewModel.InitRemainigSessions();

                                // in case this is the first time running
                                ScheduleView.Visibility = Visibility.Visible;
                                ClassromsPanel.Visibility = Visibility.Visible;
                                ScheduleLabel.Visibility = Visibility.Hidden;
                                GlobalButton.IsEnabled = true;

                                Schedule.Appointments.Clear();

                                Title = $"{dialog.Name} - {_appTitle}";
                            }

                            _nextDemoState = Constants.ApplicationState.Courses;
                            DrawerHost.IsLeftDrawerOpen = true;
                        });
                        DemoStopped();
                        break;

                    case Constants.ApplicationState.Courses:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var dialog = new CourseList(_viewModel);
                            DemoStateHandler.Instance.DemoState = dialog;
                            dialog.ShowDialog();
                            _nextDemoState = Constants.ApplicationState.Softwares;
                        });
                        DemoStopped();
                        break;

                    case Constants.ApplicationState.Softwares:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var dialog = new SoftwareList(_viewModel);
                            DemoStateHandler.Instance.DemoState = dialog;
                            dialog.ShowDialog();
                            _nextDemoState = Constants.ApplicationState.Subjects;
                        });
                        DemoStopped();
                        break;

                    case Constants.ApplicationState.Subjects:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var dialog = new SubjectList(_viewModel);
                            DemoStateHandler.Instance.DemoState = dialog;
                            dialog.ShowDialog();
                            _nextDemoState = Constants.ApplicationState.Classrooms;
                        });
                        DemoStopped();
                        break;

                    case Constants.ApplicationState.Classrooms:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var dialog = new ClassroomList(_viewModel);
                            DemoStateHandler.Instance.DemoState = dialog;
                            dialog.ShowDialog();
                            _nextDemoState = Constants.ApplicationState.SessionDetails;
                            DrawerHost.IsLeftDrawerOpen = false;
                        });
                        DemoStopped();
                        break;

                    case Constants.ApplicationState.SessionDetails:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _viewModel.Classroom = _viewModel.Classrooms[0];
                            var startTime = DateTime.Now.StartOfWeek().Date + new TimeSpan(7, 15, 0);
                            var session = _viewModel.RemainingSessions[0];
                            var avaibleTerms = _viewModel.GetAvailableTerms(null, startTime, session);
                            var dialog = new SessionDetails(_viewModel, Schedule.Appointments, avaibleTerms, startTime,
                                session);
                            DemoStateHandler.Instance.DemoState = dialog;
                            dialog.ShowDialog();
                            _nextDemoState = Constants.ApplicationState.ScheduleAdd;
                        });
                        if (DemoStopped()) return;
                        //if (DemoStopped()) return;
                        Application.Current.Dispatcher.Invoke(
                            () => { _viewModel.RemoveClassroom(DemoStateHandler.Instance.DemoClassroomId); });
                        DemoStateHandler.Instance.RemoveDemoEntities();
                        _viewModel.LastOpenedSchedule = null;
                        break;
                }
            });
        }

        public bool DemoStopped()
        {
            for (var i = 0; i < 10; i++)
            {
                if (DemoStateHandler.Instance.Stopped)
                {
                    _nextDemoState = Constants.ApplicationState.ScheduleAdd;
                    Application.Current.Dispatcher.Invoke(
                        () => { _viewModel.RemoveClassroom(DemoStateHandler.Instance.DemoClassroomId); });
                    DemoStateHandler.Instance.StopDemo();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DrawerHost.IsLeftDrawerOpen = false;
                        _viewModel.FindLastSchedule();
                        OpenLastSchedule();
                        _viewModel.RemoveClassroom(DemoStateHandler.Instance.DemoClassroomId);
                    });

                    return true;
                }
                Thread.Sleep(100);
            }
            return false;
        }

        private void OpenLastSchedule()
        {
            if (_viewModel.LastOpenedSchedule != null)
            {
                _viewModel.InitRemainigSessions();
                ScheduleView.Visibility = Visibility.Visible;
                ClassromsPanel.Visibility = Visibility.Visible;
                ScheduleLabel.Visibility = Visibility.Hidden;
                GlobalButton.IsEnabled = true;
                Title = $"{_viewModel.LastOpenedSchedule.Id} - {_appTitle}";
                _viewModel.InitSessions();
            }
            else
            {
                ScheduleView.Visibility = Visibility.Hidden;
                ClassromsPanel.Visibility = Visibility.Hidden;
                ScheduleLabel.Visibility = Visibility.Visible;
                GlobalButton.IsEnabled = false;
                Title = _appTitle;
            }
        }

        private void OpenContextHelp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenContextHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel
            {
                State = _viewModel.LastOpenedSchedule != null
                    ? Constants.ApplicationState.Schedule
                    : Constants.ApplicationState.Application
            };

            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }

        private void OpenHelp(object sender, RoutedEventArgs e)
        {
            var viewModel = new ContextHelpViewModel { State = Constants.ApplicationState.Application };
            var contextHelp = new ContextHelp(viewModel);
            contextHelp.ShowDialog();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var selected = Schedule.SelectedAppointment as SessionAppointment;
                if (selected != null)
                {
                    var dialog = new ConfirmDelete(new List<Session>(),
                        "Are you sure you want to delete this session?")
                    {
                        HelpState = Constants.ApplicationState.Schedule
                    };

                    dialog.ShowDialog();

                    if (dialog.Confirm)
                    {
                        _viewModel.RemoveSession(selected.Session);
                        _viewModel.InitRemainigSessions();
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true;
            }
        }

        private void ShowGlobalSchedule_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _viewModel.LastOpenedSchedule != null;
        }

        private void ShowGlobalSchedule_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var globalScheduleDialog = new GlobalSchedule(_viewModel.LastOpenedSchedule) { Owner = this };
            globalScheduleDialog.ShowDialog();
        }

        private void Schedule_AppointmentDragging(object sender,
            Syncfusion.UI.Xaml.Schedule.AppointmentDraggingEventArgs e)
        {
            _dragging = true;
        }
    }
}