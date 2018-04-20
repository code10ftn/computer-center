using Data.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Schedule : BaseViewModel
    {
        private string _id;
        private bool _lastOpened;
        private List<Session> _sessions;

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

        public bool LastOpened
        {
            get { return _lastOpened; }
            set
            {
                _lastOpened = value;
                OnPropertyChanged();
            }
        }

        public List<Session> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                OnPropertyChanged();
            }
        }
    }
}