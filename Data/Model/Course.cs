using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Course : BaseViewModel
    {
        private string _id;
        private string _name;
        private DateTime _dateOpened;
        private string _description;
        private List<Subject> _subjects;

        [Key]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateOpened
        {
            get { return _dateOpened; }
            set
            {
                _dateOpened = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(250)]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public List<Subject> Subjects
        {
            get { return _subjects; }
            set
            {
                _subjects = value;
                OnPropertyChanged();
            }
        }

        public void Update(Course course)
        {
            Id = course.Id;
            Name = course.Name;
            DateOpened = course.DateOpened;
            Description = course.Description;
            Subjects = course.Subjects;
        }
    }
}