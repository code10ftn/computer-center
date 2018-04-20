using ComputerCenter.Custom;
using Data.Dao;
using Data.Model;
using Data.ViewModel;
using Syncfusion.UI.Xaml.Schedule;
using System;

namespace ComputerCenter.ViewModel
{
    public class GlobalScheduleViewModel : BaseViewModel
    {
        public SessionDao SessionDao { get; } = new SessionDao();

        public Schedule Schedule { get; set; }

        public ScheduleAppointmentCollection ScheduleAppointments { get; set; }

        public void InitSessions()
        {
            ScheduleAppointments.Clear();

            var sessions = SessionDao.FindByScheduleId(Schedule.Id);
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
    }
}