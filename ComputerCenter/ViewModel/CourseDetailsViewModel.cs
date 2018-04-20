using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ComputerCenter.ViewModel
{
    public class CourseDetailsViewModel : BaseViewModel
    {
        private bool _adding;
        private bool _saveEnabled = true;
        public CourseDao CourseDao { get; } = new CourseDao();
        public Course Course { get; set; }
        private readonly HashSet<string> _takenIds = new HashSet<string>();

        public CourseDetailsViewModel()
        {
            _adding = true;
            Course = new Course { DateOpened = DateTime.Now };

            foreach (var course in CourseDao.FindAll())
            {
                _takenIds.Add(course.Id);
            }
        }

        public CourseDetailsViewModel(string id)
        {
            _adding = false;
            Course = CourseDao.FindById(id);

            foreach (var course in CourseDao.FindAll())
            {
                _takenIds.Add(course.Id);
            }
        }

        private double _maxFieldWidth;

        public double MaxFieldWidth
        {
            get { return _maxFieldWidth; }
            set
            {
                _maxFieldWidth = value;
                OnPropertyChanged();
            }
        }

        public string Id
        {
            get { return Course.Id; }
            set
            {
                Course.Id = value;
                OnPropertyChanged();
                OnPropertyChanged("IdExistsMessageVisible");
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Name
        {
            get { return Course.Name; }
            set
            {
                Course.Name = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public DateTime DateOpened
        {
            get { return Course.DateOpened; }
            set
            {
                Course.DateOpened = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Description
        {
            get { return Course.Description; }
            set
            {
                Course.Description = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public bool EditIdEnabled
        {
            get { return _adding; }
            set { OnPropertyChanged(); }
        }

        public bool SaveEnabled
        {
            get { return _saveEnabled && !FieldEmpty; }
            set
            {
                _saveEnabled = value;
                OnPropertyChanged();
            }
        }

        public Visibility IdExistsMessageVisible
        {
            get
            {
                if (_adding && !string.IsNullOrEmpty(Id) && _takenIds.Contains(Id))
                {
                    SaveEnabled = false;
                    return Visibility.Visible;
                }

                SaveEnabled = true;
                return Visibility.Collapsed;
            }
        }

        public bool FieldEmpty => string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Name);

        public Visibility AllFieldsRequiredVisible => FieldEmpty ? Visibility.Visible : Visibility.Collapsed;

        public Course SaveCourse()
        {
            if (_adding)
            {
                CourseDao.Add(Course);
            }
            else
            {
                CourseDao.Update(Course);
            }

            return Course;
        }
    }
}