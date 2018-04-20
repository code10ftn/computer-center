using ComputerCenter.Custom;
using Data.Dao;
using Data.Model;
using Data.ViewModel;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComputerCenter.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ClassroomDao ClassroomDao { get; } = new ClassroomDao();
        public ScheduleDao ScheduleDao { get; } = new ScheduleDao();
        public SubjectDao SubjectDao { get; } = new SubjectDao();
        public SessionDao SessionDao { get; } = new SessionDao();

        private Classroom _classroom;

        public Classroom Classroom
        {
            get { return _classroom; }
            set
            {
                _classroom = value;
                OnPropertyChanged();
                InitRemainigSessions();
                InitSessions();
            }
        }

        public Schedule LastOpenedSchedule { get; set; }
        public ObservableCollection<Classroom> Classrooms { get; set; } = new ObservableCollection<Classroom>();

        public ObservableCollection<RemainingSession> RemainingSessions { get; set; } =
            new ObservableCollection<RemainingSession>();

        public ScheduleAppointmentCollection ScheduleAppointments { get; set; }

        public MainWindowViewModel()
        {
            var classrooms = ClassroomDao.FindAll();
            classrooms.ForEach(classroom => Classrooms.Add(classroom));

            FindLastSchedule();
        }

        public void FindLastSchedule()
        {
            var schedules = ScheduleDao.FindAll();
            foreach (var schedule in schedules)
            {
                if (schedule.LastOpened)
                {
                    LastOpenedSchedule = schedule;
                    InitRemainigSessions();
                    return;
                }
            }
            LastOpenedSchedule = null;
        }

        public void RemoveSession(Session session)
        {
            SessionDao.Remove(session.Id);
        }

        public void UpdateSession(Session session)
        {
            SessionDao.Update(session);
        }

        public void InitRemainigSessions()
        {
            RemainingSessions.Clear();
            if (_classroom == null)
                return;

            var classroom = ClassroomDao.FindById(_classroom.Id);

            var schedule = LastOpenedSchedule;
            var subjects = SubjectDao.FindAll();
            var sessionsDictionary = subjects.ToDictionary(subject => subject.Id, subject => new RemainingSession()
            {
                RemainingTerms = subject.RequiredTerms,
                Subject = subject
            });

            var sessions = SessionDao.FindByScheduleId(schedule.Id);
            foreach (var session in sessions)
            {
                var currSession = sessionsDictionary[session.Subject.Id];
                currSession.RemainingTerms -= session.Terms;
            }

            RemainingSessions.Clear();
            var sessionList = sessionsDictionary.ToList();
            sessionList.Sort(
                (pair1, pair2) =>
                    String.Compare(pair1.Value.Subject.Name, pair2.Value.Subject.Name, StringComparison.Ordinal));
            foreach (var remainingSession in sessionList)
            {
                var session = remainingSession.Value;
                session.Compatible = classroom.CanHostSubject(session.Subject);
                RemainingSessions.Add(session);
            }
        }

        public void InitSessions()
        {
            ScheduleAppointments.Clear();
            if (_classroom == null)
                return;

            var sessions = SessionDao.FindByScheduleIdAndClassroomId(LastOpenedSchedule.Id, _classroom.Id);
            foreach (var session in sessions)
            {
                var sessionAppoinment = new SessionAppointment()
                {
                    Session = session,
                    Subject = session.Subject.Name,
                    StartTime = session.Time,
                    EndTime = session.Time + TimeSpan.FromMinutes(session.Terms * Subject.TermLenght),
                    RecurrenceRule = "week"
                };
                ScheduleAppointments.Add(sessionAppoinment);
            }
        }

        public int GetAvailableTerms(Session openedSession, DateTime date, RemainingSession remainingSession)
        {
            var day = date.DayOfWeek;
            var startTime = date.TimeOfDay;

            Session nextSession = null;

            var sessions = SessionDao.FindByScheduleId(LastOpenedSchedule.Id);

            foreach (var session in sessions)
            {
                // sesion is on the same day or in same classroom
                if (session.Day == day && session.Classroom.Id == Classroom.Id)
                {
                    var sessionStart = session.Time.TimeOfDay;
                    var sessionEnd = sessionStart.Add(new TimeSpan(0, Subject.TermLenght * session.Terms, 0));

                    if (openedSession != null && openedSession.Time.TimeOfDay.CompareTo(sessionStart) == 0)
                    {
                        continue;
                    }

                    if (startTime.CompareTo(sessionStart) >= 0 && startTime.CompareTo(sessionEnd) < 0)
                    {
                        return -1;
                    }
                    if (startTime.CompareTo(sessionStart) < 0)
                    {
                        if (nextSession == null || sessionStart.CompareTo(nextSession.Time.TimeOfDay) < 0)
                        {
                            nextSession = session;
                        }
                    }
                }
            }

            TimeSpan diff;
            if (nextSession != null)
            {
                var nextSessionStart = nextSession.Time.TimeOfDay;
                diff = nextSessionStart - startTime;
            }
            else
            {
                var maxTimeSpan = new TimeSpan(22, 0, 0);
                diff = maxTimeSpan - startTime;
            }

            var diffMinutes = diff.TotalMinutes;
            var maxTerms = (int)(diffMinutes / Subject.TermLenght);

            if (openedSession != null)
            {
                return maxTerms > remainingSession.RemainingTerms + openedSession.Terms
                    ? remainingSession.RemainingTerms + +openedSession.Terms
                    : maxTerms;
            }
            return maxTerms > remainingSession.RemainingTerms ? remainingSession.RemainingTerms : maxTerms;
        }

        public Schedule FindSchedule(string id)
        {
            return ScheduleDao.FindById(id);
        }

        public Schedule SaveCurrentSchedule()
        {
            ScheduleDao.Add(LastOpenedSchedule);
            return LastOpenedSchedule;
        }

        public Schedule UpdateCurrentSchedule()
        {
            ScheduleDao.Update(LastOpenedSchedule);
            return LastOpenedSchedule;
        }

        public void RemoveClassroom(string id)
        {
            if (id == null) return;
            var classroom = Classrooms.FirstOrDefault(c => c.Id == id);
            if (classroom == null) return;
            Classrooms.Remove(classroom);
        }
    }
}