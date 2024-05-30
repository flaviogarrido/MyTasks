using MyTasks.Tree;

namespace MyTasks.Forms;

public partial class ExplorerForm : Form
{

    TreeViewHandler? _treeviewHandler;
    NotepadHandler? _notepadHandler;

    MenuStrip _menu;

    public ExplorerForm(Form parentForm, MenuStrip menu)
    {
        _menu = menu;
        InitializeComponent();
        InitializeForm(parentForm);
    }

    private void InitializeForm(Form parentForm)
    {
        Text = $"explorer @ {Config.Default.WorkingFolder}";
        MdiParent = parentForm;
        WindowState = FormWindowState.Maximized;
        Icon = Properties.Resources.DatabaseProject;

        _treeviewHandler = new(MySplitContainer.Panel1);
        _treeviewHandler.Initialize();
        _treeviewHandler.OnRequestFileOpen += TreeviewHandler_OnRequestFileOpen;

        _notepadHandler = new(MySplitContainer.Panel2, _menu);
        _notepadHandler.Initialize();
    }

    private void TreeviewHandler_OnRequestFileOpen(object? sender, MyTreeItem e)
    {
        _notepadHandler?.Load(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _treeviewHandler?.Dispose();
        _notepadHandler?.Dispose();
    }

}
