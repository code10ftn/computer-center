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
    public class SubjectDetailsViewModel : BaseViewModel
    {
        private bool _adding;
        private bool _saveEnabled = true;
        private MainWindowViewModel _mainWindowViewModel;
        public SubjectDao SubjectDao { get; } = new SubjectDao();
        public CourseDao CourseDao { get; } = new CourseDao();
        public SoftwareDao SoftwareDao { get; } = new SoftwareDao();
        public SessionDao SessionDao { get; } = new SessionDao();

        public Subject Subject { get; set; }

        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();

        public ObservableCollection<SoftwareViewModel> SoftwareList { get; set; } =
            new ObservableCollection<SoftwareViewModel>();

        private readonly HashSet<string> _takenIds = new HashSet<string>();

        public SubjectDetailsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = true;
            Subject = new Subject();

            var softwareList = SoftwareDao.FindAll();
            softwareList.ForEach(s => SoftwareList.Add(new SoftwareViewModel(s)));

            var courses = CourseDao.FindAll();
            courses.ForEach(c => Courses.Add(c));

            foreach (var subject in SubjectDao.FindAll())
            {
                _takenIds.Add(subject.Id);
            }
        }

        public SubjectDetailsViewModel(string id, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _adding = false;
            Subject = SubjectDao.FindById(id);

            var softwareList = SoftwareDao.FindAll();
            foreach (var software in softwareList)
            {
                var softwareViewModel = new SoftwareViewModel(software)
                {
                    IsChecked = Subject.RequiredSoftware.FirstOrDefault(s => s.Id == software.Id) != null
                };
                SoftwareList.Add(softwareViewModel);
            }

            var courses = CourseDao.FindAll();
            courses.ForEach(c => Courses.Add(c));

            foreach (var subject in SubjectDao.FindAll())
            {
                _takenIds.Add(subject.Id);
            }
        }

        public string Id
        {
            get { return Subject.Id; }
            set
            {
                Subject.Id = value;
                OnPropertyChanged();
                OnPropertyChanged("IdExistsMessageVisible");
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Name
        {
            get { return Subject.Name; }
            set
            {
                Subject.Name = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public Course Course
        {
            get { return Subject.Course; }
            set
            {
                Subject.Course = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public string Description
        {
            get { return Subject.Description; }
            set
            {
                Subject.Description = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public int GroupSize
        {
            get { return Subject.GroupSize; }
            set
            {
                Subject.GroupSize = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public int MinimumTermsPerSession
        {
            get { return Subject.MinimumTermsPerSession; }
            set
            {
                Subject.MinimumTermsPerSession = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
                if (RequiredTerms < MinimumTermsPerSession)
                {
                    RequiredTerms = MinimumTermsPerSession;
                }
            }
        }

        public int RequiredTerms
        {
            get { return Subject.RequiredTerms; }
            set
            {
                Subject.RequiredTerms = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public bool RequiresProjector
        {
            get { return Subject.RequiresProjector; }
            set
            {
                Subject.RequiresProjector = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public bool RequiresBlackboard
        {
            get { return Subject.RequiresBlackboard; }
            set
            {
                Subject.RequiresBlackboard = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public bool RequiresSmartboard
        {
            get { return Subject.RequiresSmartboard; }
            set
            {
                Subject.RequiresSmartboard = value;
                OnPropertyChanged();
                OnPropertyChanged("AllFieldsRequiredVisible");
                OnPropertyChanged("SaveEnabled");
            }
        }

        public Constants.Platform RequiredPlatform
        {
            get { return Subject.RequiredPlatform; }
            set
            {
                Subject.RequiredPlatform = value;
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

        public bool FieldEmpty => string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Name) || Subject.Course == null;

        public Visibility AllFieldsRequiredVisible => FieldEmpty ? Visibility.Visible : Visibility.Collapsed;

        public Subject SaveSubject()
        {
            Subject.RequiredSoftware = new List<Software>();
            foreach (var softwareViewModel in SoftwareList)
            {
                if (softwareViewModel.IsChecked)
                {
                    Subject.RequiredSoftware.Add(softwareViewModel.Software);
                }
            }

            if (_adding)
            {
                SubjectDao.Add(Subject);
            }
            else
            {
                var subject = SubjectDao.FindById(Subject.Id);
                subject.Update(Subject);

                var sessions = SessionDao.GetIncompatibleSessions(subject);
                if (sessions.Count > 0)
                {
                    var dialog = new ConfirmDelete(sessions,
                        "Are you sure you want to update this subject? Some of the sessions have already been placed in the schedule. Following is the list of all sessions that will be deleted if you proceed with this update.")
                    {
                        HelpState = Constants.ApplicationState.SubjectsAdd
                    };
                    dialog.ShowDialog();

                    if (dialog.Confirm)
                    {
                        SubjectDao.Update(subject);
                        foreach (var session in sessions)
                        {
                            SessionDao.Remove(session.Id);
                        }

                        _mainWindowViewModel.InitRemainigSessions();
                        _mainWindowViewModel.InitSessions();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    SubjectDao.Update(subject);
                    _mainWindowViewModel.InitRemainigSessions();
                    _mainWindowViewModel.InitSessions();
                }
            }

            return Subject;
        }
    }
}