namespace MyTasks.Forms;

public partial class ExplorerForm : Form
{

    TreeViewHandler? _treeviewHandler;
    NotepadHandler? _notepadHandler;

    public ExplorerForm(Form parentForm)
    {
        InitializeComponent();
        InitializeForm(parentForm);
    }

    private void InitializeForm(Form parentForm)
    {
        Text = $"explorer @ {Config.Default.WorkingFolder}";
        MdiParent = parentForm;
        WindowState = FormWindowState.Maximized;

        _treeviewHandler = new(MySplitContainer.Panel1);
        _treeviewHandler.Initialize();
        _treeviewHandler.OnRequestFileOpen += TreeviewHandler_OnRequestFileOpen;

        _notepadHandler = new(MySplitContainer.Panel2);
        _notepadHandler.Initialize();
    }

    private void TreeviewHandler_OnRequestFileOpen(object? sender, FileInfo e)
    {
        _notepadHandler?.Load(e);
    }

}
