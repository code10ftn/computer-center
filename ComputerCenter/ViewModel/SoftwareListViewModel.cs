using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComputerCenter.ViewModel
{
    public class SoftwareListViewModel : BaseViewModel
    {
        public SoftwareDao SoftwareDao { get; } = new SoftwareDao();

        public ObservableCollection<Software> SoftwareList { get; set; } = new ObservableCollection<Software>();

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

        public SoftwareListViewModel()
        {
            var softwareList = SoftwareDao.FindAll();
            softwareList.ForEach(s => SoftwareList.Add(s));
        }

        public void RemoveSoftware(string id)
        {
            SoftwareList.Remove(SoftwareList.First(s => s.Id == id));
            SoftwareDao.Remove(id);
        }
    }
}