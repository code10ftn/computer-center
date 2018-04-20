using Data.Model;
using Data.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ComputerCenter.ViewModel
{
    public class ConfirmDeleteViewModel : BaseViewModel
    {
        public ObservableCollection<Session> Sessions { get; set; }

        public ConfirmDeleteViewModel(List<Session> sessions)
        {
            Sessions = new ObservableCollection<Session>();
            foreach (var session in sessions)
            {
                Sessions.Add(session);
            }
        }

        private double _maxFieldWidth;

        public double MaxFieldWidth
        {
            get { return _maxFieldWidth; }
            set
            {
                _maxFieldWidth = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsDataGridVisible => Sessions.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
    }
}