using Data.Model;
using Syncfusion.UI.Xaml.Schedule;
using System.ComponentModel;

namespace ComputerCenter.Custom
{
    public class SessionAppointment : ScheduleAppointment, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private long _id;

        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private Session _session;

        public Session Session
        {
            get { return _session; }
            set
            {
                _session = value;
                OnPropertyChanged("Session");
            }
        }
    }
}