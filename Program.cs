using MyTasks.Forms;

namespace MyTasks;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Config.Default.Load();

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}