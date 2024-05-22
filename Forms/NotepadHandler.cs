using FastColoredTextBoxNS;

namespace MyTasks.Forms;

internal class NotepadHandler
{
    FastColoredTextBox _notepad;
    FileInfo? _file;

    public NotepadHandler(Control control)
    {
        _notepad = new()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
        };
        _notepad.TextChanged += Notepad_TextChanged;
        control.Controls.Add(_notepad);
    }

    internal void Initialize()
    {
    }

    Object _lockObject = new Object(); 
    private void Notepad_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_file == null || !_file.Exists)
            return;

        lock (_lockObject)
        {
            File.WriteAllTextAsync(_file.FullName, _notepad.Text);
        }
    }

    internal void Load(FileInfo file, int linha = 0)
    {
        _file = file;

        if (!_file.Exists)
            return;

        _notepad.Text = File.ReadAllText(_file.FullName);

        if (linha > 0)
            _notepad.SetSelectedLine(linha);

    }
}
