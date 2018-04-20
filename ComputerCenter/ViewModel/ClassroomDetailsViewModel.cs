using ComputerCenter.Dialog;
using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ComputerCenter.ViewModel
{
    public class ClassroomDetailsViewModel : BaseViewModel
    {
        private bool _adding;
        private bool _saveEnabled = true;
        private MainWindowViewModel _mainWindowViewModel;
        public ClassroomDao ClassroomDao { get; } = new ClassroomDao();
        public SoftwareDao SoftwareDao { get; } = new SoftwareDao();
        public SessionDao SessionDao { get; } = new SessionDao();

        public Classroom Classroom { get; set; }

        public ObservableCollection<SoftwareViewModel> SoftwareList { get; set; } =
            new ObservableCollection<SoftwareViewModel>();

        private readonly HashSet<string> _takenIds = new HashSet<string>();

        public ClassroomDetailsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = true;
            Classroom = new Classroom();
            var softwareList = SoftwareDao.FindAll();
            softwareList.ForEach(s => SoftwareList.Add(new SoftwareViewModel(s)));

            foreach (var classroom in ClassroomDao.FindAll())
            {
                _takenIds.Add(classroom.Id);
            }
        }

        public ClassroomDetailsViewModel(string id, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = false;
            Classroom = ClassroomDao.FindById(id);
            var softwareList = SoftwareDao.FindAll();
            foreach (var software in softwareList)
            {
                var softwareViewModel = new SoftwareViewModel(software)
                {
                    IsChecked = Classroom.InstalledSoftware.FirstOrDefault(s => s.Id == software.Id) != null
                };
                SoftwareList.Add(softwareViewModel);
            }

            foreach (var classroom in ClassroomDao.FindAll())
            {
                _takenIds.Add(classroom.Id);
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

        public string Id
        {
            get { return Classroom.Id; }
            set
            {
                Classroom.Id = value;
                OnPropertyChanged();
                OnPropertyChanged("IdExistsMessageVisible");
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Description
        {
            get { return Classroom.Description; }
            set
            {
                Classroom.Description = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public int Seats
        {
            get { return Classroom.Seats; }
            set
            {
                Classroom.Seats = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public bool HasProjector
        {
            get { return Classroom.HasProjector; }
            set
            {
                Classroom.HasProjector = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public bool HasBlackboard
        {
            get { return Classroom.HasBlackboard; }
            set
            {
                Classroom.HasBlackboard = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public bool HasSmartboard
        {
            get { return Classroom.HasSmartboard; }
            set
            {
                Classroom.HasSmartboard = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public Constants.Platform Platform
        {
            get { return Classroom.Platform; }
            set
            {
                Classroom.Platform = value;
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

        public bool FieldEmpty => string.IsNullOrEmpty(Id);

        public Visibility AllFieldsRequiredVisible => FieldEmpty ? Visibility.Visible : Visibility.Collapsed;

        public Classroom SaveClassroom()
        {
            Classroom.InstalledSoftware = new List<Software>();
            foreach (var softwareViewModel in SoftwareList)
            {
                if (softwareViewModel.IsChecked)
                {
                    Classroom.InstalledSoftware.Add(softwareViewModel.Software);
                }
            }

            if (_adding)
            {
                ClassroomDao.Add(Classroom);
            }
            else
            {
                var sessions = SessionDao.GetIncompatibleSessions(Classroom);

                if (sessions.Count > 0)
                {
                    var dialog = new ConfirmDelete(sessions,
                        "Are you sure you want to update this classroom? Conflicting sessions have been found in the schedule. Following is the list of all sessions that will be deleted if you proceed with this update.")
                    {
                        HelpState = Constants.ApplicationState.ClassroomsAdd
                    };
                    dialog.ShowDialog();

                    if (dialog.Confirm)
                    {
                        ClassroomDao.Update(Classroom);
                        foreach (var session in sessions)
                        {
                            SessionDao.Remove(session.Id);
                        }

                        _mainWindowViewModel.InitRemainigSessions();
                        _mainWindowViewModel.InitSessions();
                    }
                }
                else
                {
                    ClassroomDao.Update(Classroom);
                    _mainWindowViewModel.InitRemainigSessions();
                    _mainWindowViewModel.InitSessions();
                }
            }

            return Classroom;
        }
    }
}