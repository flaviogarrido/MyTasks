using MyTasks.Models;
using MyTasks.Tree;

namespace MyTasks.Forms;

internal class TreeViewHandler
{
    TreeView _treeview;
    MyTreeContainer _treeContainer;

    static TreeView _instanceTreeView;

    public event EventHandler<FileInfo>? OnRequestFileOpen;


    public TreeViewHandler(Control control)
    {
        _treeContainer = new();

        _treeview = new()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
            TabIndex = 0,
        };
        _treeview.NodeMouseClick += TreeView_NodeMouseClick;
        _treeview.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
        control.Controls.Add(_treeview);

        _instanceTreeView = _treeview;
    }

    internal void Initialize()
    {
        _treeview.SuspendLayout();
        _treeview.Nodes.Clear();
        BuildTreeView(_treeview.Nodes, _treeContainer.GetRoot());
        _treeview.Nodes[0].Expand();
        _treeview.ResumeLayout();
    }

    private static void BuildTreeView(TreeNodeCollection nodes, MyTreeItem treeItem)
    {
        var treeNode = CreateTreeNode(treeItem);
        nodes.Add(treeNode);

        foreach (var subTreeItem in treeItem.Items)
            BuildTreeView(treeNode.Nodes, subTreeItem);
    }

    private static TreeNode CreateTreeNode(MyTreeItem treeItem, bool includeSubItems = false)
    {
        var node = new TreeNode(treeItem.Name);
        node.Tag = treeItem;

        if (includeSubItems)
            foreach (var subItems in treeItem.Items)
                node.Nodes.Add(CreateTreeNode(subItems, includeSubItems));

        return node;
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

    internal static bool AddTask(string text, MyTaskType myTaskInfo)
    {
        var findText = string.Empty;
        switch (myTaskInfo)
        {
            case MyTaskType.Urgent:
                findText = Properties.Resources.PackageUrgentText;
                break;
            case MyTaskType.Important:
                findText = Properties.Resources.PackageImportantText;
                break;
        }

        var packageTreeNode = FindNodeByName(_instanceTreeView.Nodes[0].Nodes, findText);
        if (packageTreeNode == null)
            return false;

        var packageTreeItem = packageTreeNode.Tag as MyTreeItem;
        if (packageTreeItem == null)
            return false;

        var newTask = MyTreeItem.CreateTask(text);
        packageTreeItem.Items.Add(newTask);

        BuildTreeView(packageTreeNode.Nodes, newTask);
        packageTreeNode.Expand();

        return true;
    }


    private static void Sort(TreeNode node, SortOrder order = SortOrder.Ascending)
    {
        _instanceTreeView.SuspendLayout();

        TreeNode[] arrSorted = new TreeNode[node.Nodes.Count];
        node.Nodes.CopyTo(arrSorted, 0);

        var sorted = new List<TreeNode>();
        sorted.AddRange(arrSorted);

        if (order == SortOrder.Ascending)
            sorted.Sort((x, y) => string.Compare(x.Text, y.Text, true));
        else
            sorted.Sort((x, y) => string.Compare(y.Text, x.Text, true));

        node.Nodes.Clear();
        node.Nodes.AddRange(sorted.ToArray());

        _instanceTreeView.ResumeLayout();
    }

    private static TreeNode? FindNodeByName(TreeNodeCollection nodes, string name, bool recursive = false)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Text == name)
                return node;

            if (recursive)
            {
                TreeNode? found = FindNodeByName(node.Nodes, name, recursive);
                if (found != null)
                    return found;
            }

        }
        return null;
    }

}
