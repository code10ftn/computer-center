using Data;
using System.Windows;

namespace ComputerCenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DatabaseContext DatabaseContext { get; private set; }

        public App()
        {
            DatabaseContext = new DatabaseContext();
            DatabaseContext.Database.Initialize(false);
        }
    }
}