using FastColoredTextBoxNS;
using MyTasks.Tree;

namespace MyTasks.Forms;

internal class NotepadHandler
{
    Label _titleLabel;
    FastColoredTextBox _notepad;
    FileInfo? _file;

    public NotepadHandler(Control control)
    {
        _titleLabel = new Label()
        {
            Dock = DockStyle.Top,
            Size = new Size(100, control.Font.Height * 2 + 4),
            Text = string.Empty,
            TextAlign = ContentAlignment.MiddleCenter,
        };

        _notepad = new()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
            ForeColor = Color.LightGreen,
            BackColor = Color.Black,
            LineNumberColor = Color.LightSeaGreen,
            BookmarkColor = Color.DarkGreen,
            PaddingBackColor = Color.DarkGray,
        };
        _notepad.TextChanged += Notepad_TextChanged;

        control.Controls.Add(_titleLabel);
        control.Controls.Add(_notepad);
        _notepad.BringToFront();
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

    internal void Load(MyTreeItem treeItem, int linha = 0)
    {
        _file = treeItem.File;

        if (_file == null) 
            return;

        _file.Refresh();
        if (!_file.Exists)
            return;

        _titleLabel.Text = treeItem.Name + "\n" + _file.FullName;
        _notepad.Text = File.ReadAllText(_file.FullName);

        if (linha > 0)
            _notepad.SetSelectedLine(linha);

    }
}
