using MyTasks.Models;
using MyTasks.Tree;
using System.Text;

namespace MyTasks.Forms;

internal class TreeViewHandler
{
    TreeView _treeview;
    MyTreeContainer _treeContainer;

    static FileInfo _treeFile;

    static TreeView _instanceTreeView;

    public event EventHandler<MyTreeItem>? OnRequestFileOpen;


    public TreeViewHandler(Control control)
    {
        _treeFile = new FileInfo(Config.Default.WorkingFileFullName);
        _treeContainer = new();


        _treeview = new()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
            TabIndex = 0,
            ForeColor = Color.LightGreen,
            BackColor = Color.Black,
        };
        _treeview.KeyDown += Treeview_KeyDown;
        _treeview.NodeMouseClick += TreeView_NodeMouseClick;
        _treeview.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
        control.Controls.Add(_treeview);

        ConfigureImageList();

        _instanceTreeView = _treeview;
    }

    void ConfigureImageList()
    {
        var imageList = new ImageList();
        imageList.ColorDepth = ColorDepth.Depth32Bit;
        imageList.ImageSize = new Size(24, 24);

        imageList.Images.Add("Root", Properties.Resources.folder_document);
        imageList.Images.Add("Document", Properties.Resources.document);
        imageList.Images.Add("DocumentAdd", Properties.Resources.document_add);
        imageList.Images.Add("DocumentCheck", Properties.Resources.document_check);
        imageList.Images.Add("DocumentEdit", Properties.Resources.document_edit);
        imageList.Images.Add("DocumentDelete", Properties.Resources.document_delete);
        imageList.Images.Add("DocumentError", Properties.Resources.document_error);
        imageList.Images.Add("DocumentGear", Properties.Resources.document_gear);
        imageList.Images.Add("DocumentHeart", Properties.Resources.document_heart);
        imageList.Images.Add("DocumentInfo", Properties.Resources.document_info);
        imageList.Images.Add("DocumentLock", Properties.Resources.document_lock);
        imageList.Images.Add("DocumentNew", Properties.Resources.document_new);
        imageList.Images.Add("DocumentOk", Properties.Resources.document_ok);
        imageList.Images.Add("DocumentOut", Properties.Resources.document_out);
        imageList.Images.Add("DocumentPinned", Properties.Resources.document_pinned);
        imageList.Images.Add("DocumentPlain", Properties.Resources.document_plain);
        imageList.Images.Add("DocumentWarning", Properties.Resources.document_warning);

        _treeview.ImageList = imageList;
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
        ConfigureStyle(treeNode, treeItem);
        nodes.Add(treeNode);

        foreach (var subTreeItem in treeItem.Items)
            BuildTreeView(treeNode.Nodes, subTreeItem);
    }

    private static void ConfigureStyle(TreeNode treeNode, MyTreeItem treeItem)
    {
        if (treeItem.Style == MyTreeItemStyleInfo.Root)
        {
            //treeNode.NodeFont = new Font("Arial", 8, FontStyle.Bold);
            treeNode.ForeColor = Color.LightBlue;
        }
    }

    private static TreeNode CreateTreeNode(MyTreeItem treeItem, bool includeSubItems = false)
    {
        var node = new TreeNode(treeItem.Name, (int)treeItem.Style, (int)treeItem.Style);
        node.Tag = treeItem;

        if (includeSubItems)
            foreach (var subItems in treeItem.Items)
                node.Nodes.Add(CreateTreeNode(subItems, includeSubItems));

        return node;
    }

    private void Treeview_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)) 
        {
            var currentFont = _treeview.Font;
            var newSize = currentFont.Size + (e.KeyCode == Keys.Up ? 1 : -1);
            if (newSize < 1) newSize = 1;
            _treeview.Font = new Font(currentFont.FontFamily, newSize, currentFont.Style);
            e.Handled = true;
        }
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

        var treeItem = e.Node.Tag as MyTreeItem;
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

        var newTask = MyTreeItem.CreateTask(text, packageTreeItem.Id);
        packageTreeItem.Items.Add(newTask);

        BuildTreeView(packageTreeNode.Nodes, newTask);
        packageTreeNode.Expand();

        StringBuilder sb = new();
        DumpTreeView(_instanceTreeView.Nodes[0].Tag as MyTreeItem, sb);
        WriteTreeFile(sb);
        return true;
    }

    private static void DumpTreeView(MyTreeItem? myTreeItem, StringBuilder sb)
    {
        if (myTreeItem == null) 
            return;

        sb.AppendLine(myTreeItem.ToString());

        foreach (var item in myTreeItem.Items)
            DumpTreeView(item, sb);
    }

    private static void WriteTreeFile(StringBuilder sb)
    {
        if (!_treeFile.Directory!.Exists)
            Directory.CreateDirectory(_treeFile.DirectoryName!);

        File.WriteAllText(_treeFile.FullName, sb.ToString());
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
