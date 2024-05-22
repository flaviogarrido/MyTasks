using System.Windows.Forms;

namespace MyTasks;

internal class AppConstants
{
    public const int VERSION = 1;

    public const string APP_NAME = "MyTasks";

    public const string CONFIG_FOLDER = "Configs";
    public const string CONFIG_DEFAULT_FILENAME = "MyTasks.cfg";
    public const string CONFIG_DEFAULT_WORKINGFOLDER = "Working";

    public const char CONFIG_COMMENT_CHAR = '#';
    public const char CONFIG_SEPARATOR_CHAR = '=';

    public const string CONFIG_KEY_AUTOSTART = "auto-start";
    public const string CONFIG_KEY_WORKINGFOLDER = "working-folder";
}
