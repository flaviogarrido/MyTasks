using Microsoft.Win32;
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

        foreach(var line in File.ReadAllLines(GetFullPathAndFileName()))
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


}
