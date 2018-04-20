using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace ComputerCenter.ViewModel
{
    public class SessionDetailsViewModel : BaseViewModel
    {
        public SessionDao SessionDao { get; } = new SessionDao();

        private bool _saveEnabled = true;

        public int AvailableTerms { get; set; }

        public Session Session { get; set; }

        public ObservableCollection<int> Terms { get; set; } = new ObservableCollection<int>();

        public SessionDetailsViewModel(int minTerm, int termNumber)
        {
            Session = new Session();
            for (var i = minTerm; i <= termNumber; i++)
            {
                Terms.Add(i);
            }
        }

        public int Term
        {
            get { return Session.Terms; }
            set
            {
                Session.Terms = value;
                OnPropertyChanged("SaveEnabled");
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged();
            }
        }

        public Session SaveSession()
        {
            return SessionDao.Add(Session);
        }

        public void UpdateSession()
        {
            SessionDao.Update(Session);
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

        public void DeleteSession()
        {
            SessionDao.Remove(Session.Id);
        }

        public bool FieldEmpty => Session.Terms == 0;

        public Visibility AllFieldsRequiredVisible => FieldEmpty ? Visibility.Visible : Visibility.Collapsed;
    }
}