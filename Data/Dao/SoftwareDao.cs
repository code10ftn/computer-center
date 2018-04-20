using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Dao
{
    public class SoftwareDao
    {
        public List<Software> FindAll()
        {
            using (var db = new DatabaseContext())
            {
                return db.Softwares.ToList();
            }
        }

        public Software FindById(string id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Softwares.Find(id);
            }
        }

        public bool IdTaken(string id)
        {
            return FindById(id) != null;
        }

        public void Add(Software software)
        {
            using (var db = new DatabaseContext())
            {
                db.Softwares.Add(software);
                db.SaveChanges();
            }
        }

        public void Remove(Software software)
        {
            using (var db = new DatabaseContext())
            {
                db.Softwares.Remove(software);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Remove(string softwareId)
        {
            using (var db = new DatabaseContext())
            {
                var software = db.Softwares.FirstOrDefault(c => c.Id == softwareId);
                if (software == null) return;
                db.Softwares.Remove(software);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void Update(Software software)
        {
            using (var db = new DatabaseContext())
            {
                var original = db.Softwares.FirstOrDefault(c => c.Id == software.Id);
                db.Entry(original).CurrentValues.SetValues(software);
                db.SaveChanges();
            }
        }

        public string FindAvailableDemoId(string id)
        {
            using (var db = new DatabaseContext())
            {
                var availableId = id;
                var num = 1;
                while (db.Softwares.FirstOrDefault(s => s.Id == availableId) != null)
                {
                    availableId = id + num++;
                }
                return availableId;
            }
        }
    }
}