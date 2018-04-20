using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Dao
{
    public class SessionDao
    {
        public SubjectDao SubjectDao { get; set; } = new SubjectDao();

        public List<Session> FindAll()
        {
            using (var db = new DatabaseContext())
            {
                return db.Sessions.ToList();
            }
        }

        public Session FindById(long id)
        {
            using (var db = new DatabaseContext())
            {
                var session = db.Sessions
                    .Include(s => s.Classroom)
                    .Include(s => s.Subject)
                    .FirstOrDefault(s => s.Id == id);
                if (session != null)
                {
                    session.Classroom = db.Classrooms.Include(c => c.InstalledSoftware)
                        .FirstOrDefault(c => c.Id == session.Classroom.Id);
                }
                return session;
            }
        }

        public List<Session> FindByScheduleId(string id)
        {
            using (var db = new DatabaseContext())
            {
                var sessions = db.Sessions
                    .Include(s => s.Classroom)
                    .Include(s => s.Subject)
                    .Where(s => s.ScheduleId == id);
                return sessions.ToList();
            }
        }

        public List<Session> FindByClassroomId(string id)
        {
            using (var db = new DatabaseContext())
            {
                var sessions = db.Sessions
                    .Include(s => s.Classroom)
                    .Include(s => s.Subject)
                    .Where(s => s.ClassroomId == id);
                return sessions.ToList();
            }
        }

        public List<Session> FindByScheduleIdAndClassroomId(string scheduleId, string classroomId)
        {
            using (var db = new DatabaseContext())
            {
                var sessions = db.Sessions
                    .Include(s => s.Classroom)
                    .Include(s => s.Subject)
                    .Where(s => s.ClassroomId == classroomId && s.ScheduleId == scheduleId);
                return sessions.ToList();
            }
        }

        public bool IdTaken(long id)
        {
            return FindById(id) != null;
        }

        public Session Add(Session session)
        {
            using (var db = new DatabaseContext())
            {
                session.SubjectId = session.Subject.Id;
                var subject = db.Subjects.FirstOrDefault(s => s.Id == session.SubjectId);
                session.Subject = subject;
                db.Entry(subject).State = EntityState.Unchanged;

                session.ClassroomId = session.Classroom.Id;
                var classroom = db.Classrooms.FirstOrDefault(c => c.Id == session.ClassroomId);
                session.Classroom = classroom;
                db.Entry(classroom).State = EntityState.Unchanged;

                session.ScheduleId = session.Schedule.Id;
                session.Schedule = null;
                var newSession = db.Sessions.Add(session);
                db.SaveChanges();
                return FindById(newSession.Id);
            }
        }

        public void Remove(long sessionId)
        {
            using (var db = new DatabaseContext())
            {
                var session = db.Sessions.FirstOrDefault(s => s.Id == sessionId);
                if (session == null) return;
                db.Sessions.Remove(session);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Update(Session session)
        {
            using (var db = new DatabaseContext())
            {
                var original = db.Sessions.Include(s => s.Classroom)
                    .Include(s => s.Subject)
                    .FirstOrDefault(c => c.Id == session.Id);
                db.Entry(original).CurrentValues.SetValues(session);
                db.SaveChanges();
            }
        }

        public List<Session> FindBySubjectId(string id)
        {
            using (var db = new DatabaseContext())
            {
                var sessions = db.Sessions
                    .Include(s => s.Classroom)
                    .Include(s => s.Subject)
                    .Where(s => s.SubjectId == id);
                return sessions.ToList();
            }
        }

        public List<Session> GetIncompatibleSessions(Subject subject)
        {
            var sessions = new List<Session>();

            var subjectSessions = FindBySubjectId(subject.Id);

            var termCount = 0;
            foreach (var session in subjectSessions)
            {
                var original = FindById(session.Id);
                if (!subject.IsSessionCompatible(original))
                    sessions.Add(original);
                termCount += original.Terms;
            }

            return termCount > subject.RequiredTerms ? subjectSessions : sessions;
        }

        public List<Session> GetIncompatibleSessions(Classroom classroom)
        {
            var sessions = new List<Session>();

            var classroomSessions = FindByClassroomId(classroom.Id);

            foreach (var session in classroomSessions)
            {
                var originalSubject = SubjectDao.FindById(session.Subject.Id);
                if (!classroom.CanHostSubject(originalSubject))
                    sessions.Add(session);
            }

            return sessions;
        }
    }
}