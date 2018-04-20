using Data.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Subject : BaseViewModel
    {
        public static int TermLenght = 45;

        private string _id;
        private string _name;
        private Course _course;
        private string _description;
        private int _groupSize;
        private int _minimumTermsPerSession = 1;
        private int _requiredTerms = 1;
        private bool _requiresProjector;
        private bool _requiresBlackboard;
        private bool _requiresSmartboard;
        private Constants.Platform _requiredPlatform;
        private List<Software> _requiredSoftware;

        [Key]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        [Required]
        public Course Course
        {
            get { return _course; }
            set
            {
                _course = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(250)]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public int GroupSize
        {
            get { return _groupSize; }
            set
            {
                _groupSize = value;
                OnPropertyChanged();
            }
        }

        public int MinimumTermsPerSession
        {
            get { return _minimumTermsPerSession; }
            set
            {
                _minimumTermsPerSession = value;
                OnPropertyChanged();
            }
        }

        public int RequiredTerms
        {
            get { return _requiredTerms; }
            set
            {
                _requiredTerms = value;
                OnPropertyChanged();
            }
        }

        public bool RequiresProjector
        {
            get { return _requiresProjector; }
            set
            {
                _requiresProjector = value;
                OnPropertyChanged();
            }
        }

        public bool RequiresBlackboard
        {
            get { return _requiresBlackboard; }
            set
            {
                _requiresBlackboard = value;
                OnPropertyChanged();
            }
        }

        public bool RequiresSmartboard
        {
            get { return _requiresSmartboard; }
            set
            {
                _requiresSmartboard = value;
                OnPropertyChanged();
            }
        }

        public Constants.Platform RequiredPlatform
        {
            get { return _requiredPlatform; }
            set
            {
                _requiredPlatform = value;
                OnPropertyChanged();
            }
        }

        public List<Software> RequiredSoftware
        {
            get { return _requiredSoftware; }
            set
            {
                _requiredSoftware = value;
                OnPropertyChanged();
            }
        }

        public void Update(Subject subject)
        {
            Name = subject.Name;
            Course = subject.Course;
            Description = subject.Description;
            GroupSize = subject.GroupSize;
            MinimumTermsPerSession = subject.MinimumTermsPerSession;
            RequiredTerms = subject.RequiredTerms;
            RequiresProjector = subject.RequiresProjector;
            RequiresBlackboard = subject.RequiresBlackboard;
            RequiresSmartboard = subject.RequiresSmartboard;
            RequiredPlatform = subject.RequiredPlatform;
            RequiredSoftware = subject.RequiredSoftware;
        }

        public void UpdateWithoutCourse(Subject subject)
        {
            Name = subject.Name;
            Description = subject.Description;
            GroupSize = subject.GroupSize;
            MinimumTermsPerSession = subject.MinimumTermsPerSession;
            RequiredTerms = subject.RequiredTerms;
            RequiresProjector = subject.RequiresProjector;
            RequiresBlackboard = subject.RequiresBlackboard;
            RequiresSmartboard = subject.RequiresSmartboard;
            RequiredPlatform = subject.RequiredPlatform;
        }

        public bool IsSessionCompatible(Session session)
        {
            return MinimumTermsPerSession <= session.Terms && session.Classroom.CanHostSubject(this);
        }
    }
}