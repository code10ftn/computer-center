using Data.Model;
using Data.ViewModel;

namespace ComputerCenter.ViewModel
{
    public class GlobalSessionDetailsViewModel : BaseViewModel
    {
        public Session Session { get; set; }

        public GlobalSessionDetailsViewModel(Session session)
        {
            Session = session;
        }
    }
}