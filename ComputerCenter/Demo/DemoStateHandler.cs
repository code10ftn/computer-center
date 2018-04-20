using Data.Dao;
using Gma.System.MouseKeyHook;
using System;

namespace ComputerCenter.Demo
{
    public class DemoStateHandler
    {
        private static DemoStateHandler _instance;

        private IDemoState _demoState;

        private IKeyboardMouseEvents _events;

        public bool Stopped { get; private set; }

        public string DemoScheduleId { get; set; }
        public string DemoCourseId { get; set; }
        public string DemoSoftwareId { get; set; }
        public string DemoSubjectId { get; set; }
        public string DemoClassroomId { get; set; }
        public long DemoSessionId { get; set; }

        private readonly ScheduleDao _scheduleDao = new ScheduleDao();
        private readonly CourseDao _courseDao = new CourseDao();
        private readonly SoftwareDao _softwareDao = new SoftwareDao();
        private readonly SubjectDao _subjectDao = new SubjectDao();
        private readonly ClassroomDao _classroomDao = new ClassroomDao();
        private readonly SessionDao _sessionDao = new SessionDao();

        private DemoStateHandler()
        {
        }

        public static DemoStateHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DemoStateHandler();
                }
                return _instance;
            }
        }

        public void InitDemo()
        {
            Stopped = false;
            _events = Hook.GlobalEvents();
            _events.KeyPress += StopDemo;
            _events.MouseClick += StopDemo;
            ResetDemoIds();
        }

        public IDemoState DemoState
        {
            get { return _demoState; }
            set
            {
                _demoState = value;
                _demoState.DoDemo();
            }
        }

        private void StopDemo(object sender, EventArgs e)
        {
            Stopped = true;
        }

        public void StopDemo()
        {
            _events.KeyPress -= StopDemo;
            _events.MouseClick -= StopDemo;
            _events.Dispose();
            RemoveDemoEntities();
        }

        public void RemoveDemoEntities()
        {
            if (DemoSessionId != -1)
            {
                _sessionDao.Remove(DemoSessionId);
            }

            if (!string.IsNullOrEmpty(DemoClassroomId))
            {
                _classroomDao.Remove(DemoClassroomId);
            }

            if (!string.IsNullOrEmpty(DemoSubjectId))
            {
                _subjectDao.Remove(DemoSubjectId);
            }

            if (!string.IsNullOrEmpty(DemoSoftwareId))
            {
                _softwareDao.Remove(DemoSoftwareId);
            }

            if (!string.IsNullOrEmpty(DemoCourseId))
            {
                _courseDao.Remove(DemoCourseId);
            }

            if (!string.IsNullOrEmpty(DemoScheduleId))
            {
                _scheduleDao.Remove(DemoScheduleId);
            }

            ResetDemoIds();
        }

        private void ResetDemoIds()
        {
            DemoScheduleId = null;
            DemoCourseId = null;
            DemoSoftwareId = null;
            DemoSubjectId = null;
            DemoClassroomId = null;
            DemoSessionId = -1;
        }
    }
}