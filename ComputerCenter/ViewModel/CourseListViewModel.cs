using Data.Dao;
using Data.Model;
using Data.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComputerCenter.ViewModel
{
    public class CourseListViewModel : BaseViewModel
    {
        public CourseDao CourseDao { get; } = new CourseDao();

        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();

        public CourseListViewModel()
        {
            var courses = CourseDao.FindAll();
            courses.ForEach(c => Courses.Add(c));
        }

        public void RemoveCourse(string id)
        {
            Courses.Remove(Courses.First(c => c.Id == id));
            CourseDao.Remove(id);
        }
    }
}