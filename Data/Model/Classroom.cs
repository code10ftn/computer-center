using Data.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Data.Model
{
    public class Classroom : BaseViewModel
    {
        private string _id;
        private string _description;
        private int _seats;
        private bool _hasProjector;
        private bool _hasBlackboard;
        private bool _hasSmartboard;
        private Constants.Platform _platform;
        private List<Software> _installedSoftware;

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

        public int Seats
        {
            get { return _seats; }
            set
            {
                _seats = value;
                OnPropertyChanged();
            }
        }

        public bool HasProjector
        {
            get { return _hasProjector; }
            set
            {
                _hasProjector = value;
                OnPropertyChanged();
            }
        }

        public bool HasBlackboard
        {
            get { return _hasBlackboard; }
            set
            {
                _hasBlackboard = value;
                OnPropertyChanged();
            }
        }

        public bool HasSmartboard
        {
            get { return _hasSmartboard; }
            set
            {
                _hasSmartboard = value;
                OnPropertyChanged();
            }
        }

        public Constants.Platform Platform
        {
            get { return _platform; }
            set
            {
                _platform = value;
                OnPropertyChanged();
            }
        }

        public List<Software> InstalledSoftware
        {
            get { return _installedSoftware; }
            set
            {
                _installedSoftware = value;
                OnPropertyChanged();
            }
        }

        public void Update(Classroom classroom)
        {
            Id = classroom.Id;
            Description = classroom.Description;
            Seats = classroom.Seats;
            HasProjector = classroom.HasProjector;
            HasBlackboard = classroom.HasBlackboard;
            HasSmartboard = classroom.HasSmartboard;
            Platform = classroom.Platform;
            InstalledSoftware = classroom.InstalledSoftware;
        }

        public void UpdateWithoutSoftware(Classroom classroom)
        {
            Id = classroom.Id;
            Description = classroom.Description;
            Seats = classroom.Seats;
            HasProjector = classroom.HasProjector;
            HasBlackboard = classroom.HasBlackboard;
            HasSmartboard = classroom.HasSmartboard;
            Platform = classroom.Platform;
        }

        public bool CanHostSubject(Subject subject)
        {
            return (subject.RequiredPlatform == this.Platform || subject.RequiredPlatform == Constants.Platform.Any ||
                    this.Platform == Constants.Platform.Both) &&
                   (subject.RequiresProjector == this.HasProjector || subject.RequiresProjector == false) &&
                   (subject.RequiresBlackboard == this.HasBlackboard || subject.RequiresBlackboard == false) &&
                   (subject.RequiresSmartboard == this.HasSmartboard || subject.RequiresSmartboard == false) &&
                   subject.RequiredSoftware.TrueForAll(
                       s => this.InstalledSoftware.FirstOrDefault(ss => ss.Id == s.Id) != null) &&
                   subject.GroupSize <= this.Seats;
        }
    }
}