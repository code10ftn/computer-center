using Data.Model;
using Data.ViewModel;
using System.Windows;

namespace ComputerCenter.ViewModel
{
    public class ContextHelpViewModel : BaseViewModel
    {
        private Constants.ApplicationState _state;

        public Constants.ApplicationState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged("IsApplicationHelpVisible");
                OnPropertyChanged("IsScheduleHelpVisible");
                OnPropertyChanged("IsScheduleAddHelpVisible");
                OnPropertyChanged("IsScheduleOpenHelpVisible");
                OnPropertyChanged("IsScheduleGlobalHelpVisible");
                OnPropertyChanged("IsSessionDetailsHelpVisible");
                OnPropertyChanged("IsCoursesHelpVisible");
                OnPropertyChanged("IsCoursesViewHelpVisible");
                OnPropertyChanged("IsCoursesAddHelpVisible");
                OnPropertyChanged("IsSoftwareHelpVisible");
                OnPropertyChanged("IsSoftwareViewHelpVisible");
                OnPropertyChanged("IsSoftwareAddHelpVisible");
                OnPropertyChanged("IsSubjectHelpVisible");
                OnPropertyChanged("IsSubjectViewHelpVisible");
                OnPropertyChanged("IsSubjectAddHelpVisible");
                OnPropertyChanged("IsClassroomHelpVisible");
                OnPropertyChanged("IsClassroomViewHelpVisible");
                OnPropertyChanged("IsClassroomAddHelpVisible");
            }
        }

        public bool IsApplicationSelected => State.Equals(Constants.ApplicationState.Application);

        public bool IsCoursesExpanded => State.Equals(Constants.ApplicationState.Courses) ||
                                         State.Equals(Constants.ApplicationState.CoursesAdd) ||
                                         State.Equals(Constants.ApplicationState.CoursesView);

        public bool IsSoftwareExpanded => State.Equals(Constants.ApplicationState.Softwares) ||
                                          State.Equals(Constants.ApplicationState.SoftwaresAdd) ||
                                          State.Equals(Constants.ApplicationState.SoftwaresView);

        public bool IsSubjectsExpanded => State.Equals(Constants.ApplicationState.Subjects) ||
                                          State.Equals(Constants.ApplicationState.SubjectsAdd) ||
                                          State.Equals(Constants.ApplicationState.SubjectsView);

        public bool IsClassroomsExpanded => State.Equals(Constants.ApplicationState.Classrooms) ||
                                            State.Equals(Constants.ApplicationState.ClassroomsAdd) ||
                                            State.Equals(Constants.ApplicationState.ClassroomsView);

        public bool IsScheduleExpanded => State.Equals(Constants.ApplicationState.Schedule) ||
                                          State.Equals(Constants.ApplicationState.ScheduleAdd) ||
                                          State.Equals(Constants.ApplicationState.ScheduleOpen) ||
                                          State.Equals(Constants.ApplicationState.ScheduleGlobal) ||
                                          State.Equals(Constants.ApplicationState.SessionDetails);

        public bool IsCoursesAddSelected => State.Equals(Constants.ApplicationState.CoursesAdd);
        public bool IsCoursesViewSelected => State.Equals(Constants.ApplicationState.CoursesView);

        public bool IsSoftwareAddSelected => State.Equals(Constants.ApplicationState.SoftwaresAdd);
        public bool IsSoftwareViewSelected => State.Equals(Constants.ApplicationState.SoftwaresView);

        public bool IsSubjectsAddSelected => State.Equals(Constants.ApplicationState.SubjectsAdd);
        public bool IsSubjectsViewSelected => State.Equals(Constants.ApplicationState.SubjectsView);

        public bool IsClassroomsAddSelected => State.Equals(Constants.ApplicationState.ClassroomsAdd);
        public bool IsClassroomsViewSelected => State.Equals(Constants.ApplicationState.ClassroomsView);

        public bool IsScheduleSelected => State.Equals(Constants.ApplicationState.Schedule);
        public bool IsScheduleAddSelected => State.Equals(Constants.ApplicationState.ScheduleAdd);
        public bool IsScheduleOpenSelected => State.Equals(Constants.ApplicationState.ScheduleOpen);
        public bool IsScheduleGlobalSelected => State.Equals(Constants.ApplicationState.ScheduleGlobal);
        public bool IsSessionDetailsSelected => State.Equals(Constants.ApplicationState.SessionDetails);

        public Visibility IsApplicationHelpVisible => State.Equals(Constants.ApplicationState.Application)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsScheduleHelpVisible => State.Equals(Constants.ApplicationState.Schedule)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsScheduleAddHelpVisible => State.Equals(Constants.ApplicationState.ScheduleAdd)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsScheduleOpenHelpVisible => State.Equals(Constants.ApplicationState.ScheduleOpen)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsScheduleGlobalHelpVisible => State.Equals(Constants.ApplicationState.ScheduleGlobal)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsSessionDetailsHelpVisible => State.Equals(Constants.ApplicationState.SessionDetails)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsCoursesHelpVisible => State.Equals(Constants.ApplicationState.Courses)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsCoursesViewHelpVisible => State.Equals(Constants.ApplicationState.CoursesView)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsCoursesAddHelpVisible => State.Equals(Constants.ApplicationState.CoursesAdd)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsSoftwareHelpVisible => State.Equals(Constants.ApplicationState.Softwares)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsSoftwareViewHelpVisible => State.Equals(Constants.ApplicationState.SoftwaresView)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsSoftwareAddHelpVisible => State.Equals(Constants.ApplicationState.SoftwaresAdd)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsSubjectHelpVisible => State.Equals(Constants.ApplicationState.Subjects)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsSubjectViewHelpVisible => State.Equals(Constants.ApplicationState.SubjectsView)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsSubjectAddHelpVisible => State.Equals(Constants.ApplicationState.SubjectsAdd)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsClassroomHelpVisible => State.Equals(Constants.ApplicationState.Classrooms)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsClassroomViewHelpVisible => State.Equals(Constants.ApplicationState.ClassroomsView)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility IsClassroomAddHelpVisible => State.Equals(Constants.ApplicationState.ClassroomsAdd)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}