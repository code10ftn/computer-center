using Data.ViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Session : BaseViewModel
    {
        private long _id;
        private Subject _subject;
        private Classroom _classroom;
        private DayOfWeek _day;
        private DateTime _time;
        private int _terms;
        private Schedule _schedule;

        [Key]
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string SubjectId { get; set; }

        [Required]
        public Subject Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        public string ClassroomId { get; set; }

        [Required]
        public Classroom Classroom
        {
            get { return _classroom; }
            set
            {
                _classroom = value;
                OnPropertyChanged();
            }
        }

        public DayOfWeek Day
        {
            get { return _day; }
            set
            {
                _day = value;
                OnPropertyChanged();
            }
        }

        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public int Terms
        {
            get { return _terms; }
            set
            {
                _terms = value;
                OnPropertyChanged();
            }
        }

        public string ScheduleId { get; set; }

        public Schedule Schedule
        {
            get { return _schedule; }
            set
            {
                _schedule = value;
                OnPropertyChanged();
            }
        }
    }
}