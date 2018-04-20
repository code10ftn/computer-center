using Data.Dao;
using Data.ViewModel;
using System.Collections.Generic;
using System.Windows;

namespace ComputerCenter.ViewModel
{
    public class ScheduleNameViewModel : BaseViewModel
    {
        public ScheduleDao ScheduleDao { get; } = new ScheduleDao();
        private bool _saveEnabled;
        private string _scheduleName;
        private readonly HashSet<string> _takenIds = new HashSet<string>();

        public ScheduleNameViewModel()
        {
            foreach (var schedule in ScheduleDao.FindAll())
            {
                _takenIds.Add(schedule.Id);
            }
        }

        public string ScheduleName
        {
            get { return _scheduleName; }
            set
            {
                _scheduleName = value;
                OnPropertyChanged();
                OnPropertyChanged("SaveNameEnabled");
                OnPropertyChanged("NameExistsMessageVisible");
            }
        }

        public bool SaveNameEnabled
        {
            get { return _saveEnabled && !string.IsNullOrEmpty(ScheduleName); }
            set
            {
                _saveEnabled = value;
                OnPropertyChanged();
            }
        }

        public Visibility NameExistsMessageVisible
        {
            get
            {
                if (!string.IsNullOrEmpty(ScheduleName) && ScheduleDao.IdTaken(ScheduleName))
                {
                    SaveNameEnabled = false;
                    return Visibility.Visible;
                }

                SaveNameEnabled = true;
                return Visibility.Collapsed;
            }
        }
    }
}