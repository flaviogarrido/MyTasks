using FastColoredTextBoxNS;

namespace MyTasks.Forms;

internal class NotepadHandler
{
    FastColoredTextBox _notepad;

    public NotepadHandler(Control control)
    {
        _notepad = new()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
        };
        control.Controls.Add(_notepad);
    }

    internal void Initialize()
    {
    }

    internal void Load(FileInfo file, int linha = 0)
    {
        if (!file.Exists)
            return;

        _notepad.Text = File.ReadAllText(file.FullName);

        if (linha > 0)
            _notepad.SetSelectedLine(linha);

    }
}
