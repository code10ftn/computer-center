using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.Generic;
using System.Windows;

namespace ComputerCenter.ViewModel
{
    public class SoftwareDetailsViewModel : BaseViewModel
    {
        private bool _adding;
        private bool _saveEnabled = true;
        public SoftwareDao SoftwareDao { get; } = new SoftwareDao();
        public Software Software { get; set; }
        private readonly HashSet<string> _takenIds = new HashSet<string>();

        public SoftwareDetailsViewModel()
        {
            _adding = true;
            Software = new Software();

            foreach (var software in SoftwareDao.FindAll())
            {
                _takenIds.Add(software.Id);
            }
        }

        public SoftwareDetailsViewModel(string id)
        {
            _adding = false;
            Software = SoftwareDao.FindById(id);

            foreach (var software in SoftwareDao.FindAll())
            {
                _takenIds.Add(software.Id);
            }
        }

        public string Id
        {
            get { return Software.Id; }
            set
            {
                Software.Id = value;
                OnPropertyChanged();
                OnPropertyChanged("IdExistsMessageVisible");
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Name
        {
            get { return Software.Name; }
            set
            {
                Software.Name = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public Constants.Platform Platform
        {
            get { return Software.Platform; }
            set
            {
                Software.Platform = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Maker
        {
            get { return Software.Maker; }
            set
            {
                Software.Maker = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string MakerWebsite
        {
            get { return Software.MakerWebsite; }
            set
            {
                Software.MakerWebsite = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string YearReleased
        {
            get { return Software.YearReleased; }
            set
            {
                Software.YearReleased = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public double Price
        {
            get { return Software.Price; }
            set
            {
                Software.Price = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Description
        {
            get { return Software.Description; }
            set
            {
                Software.Description = value;
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

        public Software SaveSoftware()
        {
            if (_adding)
            {
                SoftwareDao.Add(Software);
            }
            else
            {
                SoftwareDao.Update(Software);
            }

            return Software;
        }
    }
}