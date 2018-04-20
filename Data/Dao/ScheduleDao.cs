using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Dao
{
    public class ScheduleDao
    {
        public List<Schedule> FindAll()
        {
            using (var db = new DatabaseContext())
            {
                return db.Schedules.ToList();
            }
        }

        public Schedule FindById(string id)
        {
            using (var db = new DatabaseContext())
            {
                var schedule = db.Schedules
                    .Include(s => s.Sessions)
                    .FirstOrDefault(s => s.Id == id);
                return schedule;
            }
        }

        public bool IdTaken(string id)
        {
            return FindById(id) != null;
        }

        public void Add(Schedule schedule)
        {
            using (var db = new DatabaseContext())
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
            }
        }

        public void Remove(string scheduleId)
        {
            using (var db = new DatabaseContext())
            {
                var schedule = db.Schedules.Include(s => s.Sessions).FirstOrDefault(c => c.Id == scheduleId);
                if (schedule == null) return;
                db.Schedules.Remove(schedule);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Update(Schedule schedule)
        {
            using (var db = new DatabaseContext())
            {
                var original = db.Schedules.FirstOrDefault(c => c.Id == schedule.Id);
                db.Entry(original).CurrentValues.SetValues(schedule);
                db.SaveChanges();
            }
        }

        public string FindAvailableDemoId(string id)
        {
            using (var db = new DatabaseContext())
            {
                var availableId = id;
                var num = 1;
                while (db.Schedules.FirstOrDefault(s => s.Id == availableId) != null)
                {
                    availableId = id + num++;
                }
                return availableId;
            }
        }
    }
}