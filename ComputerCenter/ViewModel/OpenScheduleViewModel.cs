using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.ObjectModel;

namespace ComputerCenter.ViewModel
{
    public class OpenScheduleViewModel : BaseViewModel
    {
        public ScheduleDao ScheduleDao { get; } = new ScheduleDao();
        private Schedule _selectedSchedule;

        public ObservableCollection<Schedule> Schedules { get; set; } = new ObservableCollection<Schedule>();

        public OpenScheduleViewModel()
        {
            ScheduleDao.FindAll().ForEach(schedule => Schedules.Add(schedule));
        }

        public Schedule SelectedSchedule
        {
            get { return _selectedSchedule; }
            set
            {
                _selectedSchedule = value;
                OnPropertyChanged();
                OnPropertyChanged("OpenEnabled");
            }
        }

        public bool OpenEnabled => SelectedSchedule != null;
    }
}