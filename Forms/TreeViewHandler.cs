namespace MyTasks.Forms;

internal class TreeViewHandler
{
    TreeView _treeview;

    public event EventHandler<FileInfo>? OnRequestFileOpen;


    public TreeViewHandler(Control control)
    {
        _treeview = new()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
            TabIndex = 0,
        };
        _treeview.NodeMouseClick += TreeView_NodeMouseClick;
        _treeview.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
        control.Controls.Add(_treeview);
    }

    internal void Initialize()
    {
    }

    private void TreeView_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            _treeview.SelectedNode = e.Node;
            //exibir menu de contexto
        }
    }

    private void TreeView_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (!e.Button.Equals(MouseButtons.Left))
            return;

        var treeItem = e.Node.Tag as FileInfo;
        if (treeItem == null) 
            return;

        OnRequestFileOpen?.Invoke(null, treeItem);

    }

}
