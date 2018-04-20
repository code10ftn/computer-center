using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Dao
{
    public class CourseDao
    {
        public List<Course> FindAll()
        {
            using (var db = new DatabaseContext())
            {
                return db.Courses.ToList();
            }
        }

        public Course FindById(string id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Courses.Find(id);
            }
        }

        public bool IdTaken(string id)
        {
            return FindById(id) != null;
        }

        public void Add(Course course)
        {
            using (var db = new DatabaseContext())
            {
                db.Courses.Add(course);
                db.SaveChanges();
            }
        }

        public void Remove(string courseId)
        {
            using (var db = new DatabaseContext())
            {
                var course = db.Courses.FirstOrDefault(c => c.Id == courseId);
                if (course == null) return;
                db.Courses.Remove(course);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Update(Course course)
        {
            using (var db = new DatabaseContext())
            {
                var original = db.Courses.FirstOrDefault(c => c.Id == course.Id);
                db.Entry(original).CurrentValues.SetValues(course);
                db.SaveChanges();
            }
        }

        public string FindAvailableDemoId(string id)
        {
            using (var db = new DatabaseContext())
            {
                var availableId = id;
                var num = 1;
                while (db.Courses.FirstOrDefault(c => c.Id == availableId) != null)
                {
                    availableId = id + num++;
                }
                return availableId;
            }
        }
    }
}