using System.Windows.Input;

namespace ComputerCenter.Command
{
    public static class RoutedCommands
    {
        public static readonly RoutedUICommand NewSchedule = new RoutedUICommand(
            "NewSchedule",
            "NewSchedule",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand OpenSchedule = new RoutedUICommand(
            "OpenSchedule",
            "OpenSchedule",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ToggleMenu = new RoutedUICommand(
            "OpenMenu",
            "OpenMenu",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Escape)
            }
        );

        public static readonly RoutedUICommand OpenContextHelp = new RoutedUICommand(
            "OpenContextHelp",
            "OpenContextHelp",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1)
            }
        );

        public static readonly RoutedUICommand ShowGlobalSchedule = new RoutedUICommand(
            "ShowGlobalSchedule",
            "ShowGlobalSchedule",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.G, ModifierKeys.Control)
            }
        );
    }
}