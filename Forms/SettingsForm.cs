using FastColoredTextBoxNS;

namespace MyTasks.Forms;

public partial class SettingsForm : Form
{
    FileInfo? _configFile = null;
    FastColoredTextBox? _textbox = null;

    public SettingsForm(FileInfo? configFile = null)
    {
        _configFile = configFile ?? new FileInfo(Config.Default.GetFullPathAndFileName());
        
        InitializeComponent();
        InitializeForm();
    }

    private void InitializeForm()
    {
        CheckConfigFile();

        Text = $"settings @ {Config.Default.GetFullPathAndFileName()}";

        _textbox = new()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
        };
        _textbox.Text = File.ReadAllText(_configFile!.FullName);
        _textbox.TextChanged += Textbox_TextChanged;
        
        Controls.Add(_textbox);
    }

    private void CheckConfigFile()
    {
        if (!_configFile!.Exists)
        {
            if (!_configFile.Directory!.Exists)
                Directory.CreateDirectory(_configFile.DirectoryName!);

            File.WriteAllText(_configFile.FullName, "# empty config file");
        }
    }

    Object _lockObject = new Object();
    private void Textbox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        lock (_lockObject)
        {
            File.WriteAllTextAsync(_configFile!.FullName, _textbox!.Text);
        }
    }
}
