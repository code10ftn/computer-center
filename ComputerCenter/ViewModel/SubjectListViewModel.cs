using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComputerCenter.ViewModel
{
    public class SubjectListViewModel : BaseViewModel
    {
        public SubjectDao SubjectDao { get; } = new SubjectDao();

        public ObservableCollection<Subject> Subjects { get; set; } = new ObservableCollection<Subject>();

        private string _query = "";

        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                OnPropertyChanged();
            }
        }

        private string _requiresProjector = "";

        public string RequiresProjector
        {
            get { return _requiresProjector; }
            set
            {
                _requiresProjector = value;
                OnPropertyChanged();
            }
        }

        private string _requiresBlackboard = "";

        public string RequiresBlackboard
        {
            get { return _requiresBlackboard; }
            set
            {
                _requiresBlackboard = value;
                OnPropertyChanged();
            }
        }

        private string _requiresSmartboard = "";

        public string RequiresSmartboard
        {
            get { return _requiresSmartboard; }
            set
            {
                _requiresSmartboard = value;
                OnPropertyChanged();
            }
        }

        private string _platform = "";

        public string Platform
        {
            get { return _platform; }
            set
            {
                _platform = value;
                OnPropertyChanged();
            }
        }

        public SubjectListViewModel()
        {
            var subjects = SubjectDao.FindAll();
            subjects.ForEach(s => Subjects.Add(s));
        }

        public void RemoveSubject(string id)
        {
            Subjects.Remove(Subjects.First(s => s.Id == id));
            SubjectDao.Remove(id);
        }
    }
}