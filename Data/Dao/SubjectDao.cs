using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Dao
{
    public class SubjectDao
    {
        public List<Subject> FindAll()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return databaseContext.Subjects.Include(s => s.RequiredSoftware)
                    .Include(s => s.Course)
                    .ToList();
            }
        }

        public Subject FindById(string id)
        {
            using (var db = new DatabaseContext())
            {
                var subject = db.Subjects
                    .Include(s => s.RequiredSoftware)
                    .Include(s => s.Course)
                    .FirstOrDefault(s => s.Id == id);
                return subject;
            }
        }

        public bool IdTaken(string id)
        {
            return FindById(id) != null;
        }

        public void Add(Subject subject)
        {
            using (var db = new DatabaseContext())
            {
                var course = db.Courses.Find(subject.Course.Id);

                foreach (var software in subject.RequiredSoftware)
                {
                    db.Entry(software).State = EntityState.Unchanged;
                }

                subject.Course = course;
                db.Entry(subject).State = EntityState.Added;
                db.Entry(course).State = EntityState.Unchanged;

                db.SaveChanges();
            }
        }

        public void Remove(Subject subject)
        {
            using (var db = new DatabaseContext())
            {
                db.Subjects.Remove(subject);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Remove(string subjectId)
        {
            using (var db = new DatabaseContext())
            {
                var subject = db.Subjects.FirstOrDefault(c => c.Id == subjectId);
                if (subject == null) return;
                db.Subjects.Remove(subject);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Update(Subject subject)
        {
            using (var db = new DatabaseContext())
            {
                var original = db.Subjects.Include(s => s.RequiredSoftware).FirstOrDefault(c => c.Id == subject.Id);
                if (original != null)
                {
                    var course = db.Courses.Find(subject.Course.Id);

                    original.RequiredSoftware.Clear();

                    foreach (var software in subject.RequiredSoftware)
                    {
                        var originalSoftware = db.Softwares.FirstOrDefault(s => s.Id == software.Id);
                        db.Entry(originalSoftware).State = EntityState.Unchanged;
                        original.RequiredSoftware.Add(originalSoftware);
                    }

                    original.Course = course;
                    original.UpdateWithoutCourse(subject);
                    db.Entry(course).State = EntityState.Unchanged;
                    db.Entry(original).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public string FindAvailableDemoId(string id)
        {
            using (var db = new DatabaseContext())
            {
                var availableId = id;
                var num = 1;
                while (db.Subjects.FirstOrDefault(s => s.Id == availableId) != null)
                {
                    availableId = id + num++;
                }
                return availableId;
            }
        }
    }
}