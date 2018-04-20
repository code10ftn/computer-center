using Data.ViewModel;

namespace Data.Model
{
    public class RemainingSession : BaseViewModel
    {
        private int _remainingTerms;
        private Subject _subject;
        private bool _enabled;
        private bool _empty;
        private bool _compatible;

        public Subject Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        public int RemainingTerms
        {
            get { return _remainingTerms; }
            set
            {
                _remainingTerms = value;
                Empty = value == 0;
                OnPropertyChanged("Enabled");
                OnPropertyChanged();
            }
        }

        public bool Enabled
        {
            get { return !_empty && _compatible; }

            set
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public bool Empty
        {
            get { return _empty; }

            set
            {
                _empty = value;
                OnPropertyChanged("Enabled");
                OnPropertyChanged();
            }
        }

        public bool Compatible
        {
            get { return _compatible; }

            set
            {
                _compatible = value;
                OnPropertyChanged("Enabled");
                OnPropertyChanged();
            }
        }
    }
}