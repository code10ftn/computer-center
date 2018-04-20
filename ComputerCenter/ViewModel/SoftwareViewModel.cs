using Data.Model;
using Data.ViewModel;

namespace ComputerCenter.ViewModel
{
    public class SoftwareViewModel : BaseViewModel
    {
        private Software _software;

        public Software Software
        {
            get { return _software; }
            set { _software = value; }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public SoftwareViewModel(Software software)
        {
            _software = software;
        }
    }
}