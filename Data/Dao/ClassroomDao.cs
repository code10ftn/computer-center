using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Dao
{
    public class ClassroomDao
    {
        public List<Classroom> FindAll()
        {
            using (var db = new DatabaseContext())
            {
                return db.Classrooms.Include(c => c.InstalledSoftware).ToList();
            }
        }

        public Classroom FindById(string id)
        {
            using (var db = new DatabaseContext())
            {
                var classroom = db.Classrooms.Include(c => c.InstalledSoftware).FirstOrDefault(c => c.Id == id);
                return classroom;
            }
        }

        public bool IdTaken(string id)
        {
            return FindById(id) != null;
        }

        public void Add(Classroom classroom)
        {
            using (var db = new DatabaseContext())
            {
                foreach (var software in classroom.InstalledSoftware)
                {
                    db.Entry(software).State = EntityState.Unchanged;
                }

                db.Classrooms.Add(classroom);
                db.SaveChanges();
            }
        }

        public void Remove(string classroomId)
        {
            using (var db = new DatabaseContext())
            {
                var classroom = db.Classrooms.FirstOrDefault(c => c.Id == classroomId);
                if (classroom == null) return;
                db.Classrooms.Remove(classroom);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Update(Classroom classroom)
        {
            using (var db = new DatabaseContext())
            {
                var original = db.Classrooms.Include(c => c.InstalledSoftware)
                    .FirstOrDefault(c => c.Id == classroom.Id);
                if (original != null)
                {
                    original.InstalledSoftware.Clear();

                    foreach (var software in classroom.InstalledSoftware)
                    {
                        var originalSoftware = db.Softwares.FirstOrDefault(s => s.Id == software.Id);
                        db.Entry(originalSoftware).State = EntityState.Unchanged;
                        original.InstalledSoftware.Add(originalSoftware);
                    }

                    original.UpdateWithoutSoftware(classroom);
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
                while (db.Classrooms.FirstOrDefault(c => c.Id == availableId) != null)
                {
                    availableId = id + num++;
                }
                return availableId;
            }
        }
    }
}