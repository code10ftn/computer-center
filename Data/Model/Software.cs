using Data.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Software : BaseViewModel
    {
        private string _id;
        private string _name;
        private Constants.Platform _platform;
        private string _maker;
        private string _makerWebsite;
        private string _yearReleased;
        private double _price;
        private string _description;
        private List<Classroom> _classrooms;
        private List<Subject> _subjects;

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

        public Constants.Platform Platform
        {
            get { return _platform; }
            set
            {
                _platform = value;
                OnPropertyChanged();
            }
        }

        public string Maker
        {
            get { return _maker; }
            set
            {
                _maker = value;
                OnPropertyChanged();
            }
        }

        public string MakerWebsite
        {
            get { return _makerWebsite; }
            set
            {
                _makerWebsite = value;
                OnPropertyChanged();
            }
        }

        public string YearReleased
        {
            get { return _yearReleased; }
            set
            {
                _yearReleased = value;
                OnPropertyChanged();
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
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

        public List<Classroom> Classrooms
        {
            get { return _classrooms; }
            set
            {
                _classrooms = value;
                OnPropertyChanged();
            }
        }

        public List<Subject> Subjects
        {
            get { return _subjects; }
            set
            {
                _subjects = value;
                OnPropertyChanged();
            }
        }

        public void Update(Software software)
        {
            Id = software.Id;
            Platform = software.Platform;
            Maker = software.Maker;
            MakerWebsite = software.MakerWebsite;
            YearReleased = software.YearReleased;
            Price = software.Price;
            Description = software.Description;
            Classrooms = software.Classrooms;
            Subjects = software.Subjects;
        }
    }
}