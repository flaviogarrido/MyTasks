using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace MyTasks;

internal class Config
{
    readonly Dictionary<string, List<string>> _keyValues = new();

    string _fileName;

    static Config? _defaultConfig;

    public Config(string fileName = AppConstants.CONFIG_DEFAULT_FILENAME)
    {
        _fileName = fileName;
    }

    public static Config Default
    {
        get
        {
            if (_defaultConfig == null)
                _defaultConfig = new Config();
            return _defaultConfig;
        }
    }

    public void Load()
    {
        if (Exists())
        {
            LoadKeyValuePairs();
            ConfigureEnviroment();
        }
    }

    private bool Exists()
    {
        return File.Exists(GetFullPathAndFileName());
    }

    private void LoadKeyValuePairs()
    {
        _keyValues.Clear();

        foreach (var line in File.ReadAllLines(GetFullPathAndFileName()))
        {
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith(AppConstants.CONFIG_COMMENT_CHAR))
                continue;

            var separatorIndex = line.IndexOf(AppConstants.CONFIG_SEPARATOR_CHAR);
            if (separatorIndex == -1)
                continue;

            var key = line.Substring(0, separatorIndex).Trim();
            var value = line.Substring(separatorIndex + 1).Trim();

            if (!_keyValues.ContainsKey(key))
                _keyValues[key] = new List<string>();

            _keyValues[key].Add(value);
        }
    }

    public string GetFullPathAndFileName()
    {
        return Path.Combine(
            Directory.GetCurrentDirectory(),
            AppConstants.CONFIG_FOLDER,
            _fileName);
    }

    public string WorkingFolder
    {
        get
        {
            var key = AppConstants.CONFIG_KEY_WORKINGFOLDER;
            if (_keyValues.ContainsKey(key))
                return _keyValues[key][0];

            return Path.Combine(
                Directory.GetCurrentDirectory(),
                AppConstants.CONFIG_DEFAULT_WORKINGFOLDER);
        }
    }

    public string WorkingFileFullName
    {
        get
        {
            var key = AppConstants.CONFIG_KEY_WORKINGFILENAME;
            if (_keyValues.ContainsKey(key))
                return _keyValues[key][0];

            return Path.Combine(
                WorkingFolder,
                AppConstants.CONFIG_DEFAULT_WORKINGFILENAME);
        }
    }

    private void ConfigureEnviroment()
    {
        ConfigureStartupWithWindows();
    }

    private void ConfigureStartupWithWindows()
    {
        var key = AppConstants.CONFIG_KEY_AUTOSTART;
        if (_keyValues.ContainsKey(key))
            SetStartup(_keyValues[key][0].Equals("true", StringComparison.OrdinalIgnoreCase));
    }

    private void SetStartup(bool add)
    {
        string appPath = Assembly.GetExecutingAssembly().Location;
        RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

        if (add)
            key?.SetValue(AppConstants.APP_NAME, appPath);
        else
            key?.DeleteValue(AppConstants.APP_NAME, false);
    }

    public string[] Hints
    {
        get
        {
            var key = AppConstants.CONFIG_KEY_NOTEPAD_HINTS;
            
            return _keyValues.ContainsKey(key) ?
                _keyValues[key].ToArray() :
                Array.Empty<string>();
        }
    }

    public bool DarkScreenShowButton
    {
        get
        {
            var key = AppConstants.CONFIG_KEY_DARKSCREEN_SHOWBUTTON;
            if (_keyValues.ContainsKey(key))
                return _keyValues[key][0].Equals("true", StringComparison.OrdinalIgnoreCase);
            else
                return true;
        }
    }

    public bool IsDarkTheme
    {
        get
        {
            var key = AppConstants.CONFIG_KEY_DARKTHEME;
            if (_keyValues.ContainsKey(key))
                return _keyValues[key][0].Equals("true", StringComparison.OrdinalIgnoreCase);
            else
                return true;
        }
    }

    private Color GetColorByKey(string key, string defaultColor)
    {
        string colorString;
        if (_keyValues.ContainsKey(key))
            colorString = _keyValues[key][0];
        else
            colorString = defaultColor;

        try
        {
            var knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorString);
            return Color.FromKnownColor(knownColor);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Color.Transparent;
        }
    }

    internal Color ColorTreeviewForeground()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_TREEVIEW_FOREGROUND :
            AppConstants.CONFIG_KEY_LIGHTTHEME_TREEVIEW_FOREGROUND;

        return GetColorByKey(key, IsDarkTheme ? "White" : "Black");
    }

    internal Color ColorTreeviewBackground()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_TREEVIEW_BACKGROUND :
            AppConstants.CONFIG_KEY_LIGHTTHEME_TREEVIEW_BACKGROUND;

        return GetColorByKey(key, IsDarkTheme ? "DarkGray" : "LightGray");
    }

    internal Color ColorTreeviewRootForeground()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_TREEVIEW_FOREGROUND_ROOT :
            AppConstants.CONFIG_KEY_LIGHTTHEME_TREEVIEW_FOREGROUND_ROOT;

        return GetColorByKey(key, IsDarkTheme ? "LightBlue" : "DarkBlue");
    }

    internal Color ColorNotepadTitleBackground()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_TITLE_BACKGROUND :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_TITLE_BACKGROUND;

        return GetColorByKey(key, IsDarkTheme ? "DarkGray" : "ControlLight");
    }

    internal Color ColorNotepadTitleForeground()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_TITLE_FOREGROUND :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_TITLE_FOREGROUND;

        return GetColorByKey(key, IsDarkTheme ? "Black" : "Black");
    }

    internal Color ColorNotepadBackground()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_BACKGROUND :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_BACKGROUND;

        return GetColorByKey(key, IsDarkTheme ? "Black" : "White");
    }

    internal Color ColorNotepadForeground()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_FOREGROUND :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_FOREGROUND;

        return GetColorByKey(key, IsDarkTheme ? "LightGreen" : "Black");
    }

    internal Color ColorNotepadLineNumber()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_LINENUMBER:
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_LINENUMBER;

        return GetColorByKey(key, IsDarkTheme ? "LightSeaGreen" : "LightSeaGreen");
    }

    internal Color ColorNotepadBookmark()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_BOOKMARK :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_BOOKMARK;

        return GetColorByKey(key, IsDarkTheme ? "DarkGreen" : "DarkGreen");
    }

    internal Color ColorNotepadPadding()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_PADDING :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_PADDING;

        return GetColorByKey(key, IsDarkTheme ? "Transparent" : "Transparent");
    }

    internal Color ColorNotepadIndent()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_INDENT :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_INDENT;

        return GetColorByKey(key, IsDarkTheme ? "Black" : "ControlLight");
    }

    internal Brush ColorNotepadHyperlink()
    {
        var key = IsDarkTheme ?
            AppConstants.CONFIG_KEY_DARKTHEME_NOTEPAD_HYPERLINK :
            AppConstants.CONFIG_KEY_LIGHTTHEME_NOTEPAD_HYPERLINK;

        var color = GetColorByKey(key, IsDarkTheme ? "LightBlue" : "Blue");
        
        return new SolidBrush(color);
    }
}
