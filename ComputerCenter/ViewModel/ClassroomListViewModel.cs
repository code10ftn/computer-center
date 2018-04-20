using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComputerCenter.ViewModel
{
    public class ClassroomListViewModel : BaseViewModel
    {
        public ClassroomDao ClassroomDao { get; } = new ClassroomDao();

        public ObservableCollection<Classroom> Classrooms { get; set; } = new ObservableCollection<Classroom>();

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

        private string _hasProjector = "";

        public string HasProjector
        {
            get { return _hasProjector; }
            set
            {
                _hasProjector = value;
                OnPropertyChanged();
            }
        }

        private string _hasBlackboard = "";

        public string HasBlackboard
        {
            get { return _hasBlackboard; }
            set
            {
                _hasBlackboard = value;
                OnPropertyChanged();
            }
        }

        private string _hasSmartboard = "";

        public string HasSmartboard
        {
            get { return _hasSmartboard; }
            set
            {
                _hasSmartboard = value;
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

        public ClassroomListViewModel()
        {
            var classrooms = ClassroomDao.FindAll();
            classrooms.ForEach(c => Classrooms.Add(c));
        }

        public void RemoveClassroom(string id)
        {
            Classrooms.Remove(Classrooms.First(c => c.Id == id));
            ClassroomDao.Remove(id);
        }
    }
}