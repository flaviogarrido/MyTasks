using MyTasks.Models;
using MyTasks.Tree;
using System.Text;

namespace MyTasks.Forms;

internal class TreeViewHandler : IDisposable
{
    MyTreeContainer _treeContainer;
    TreeView _treeview;

    static FileInfo _treeFile;
    static TreeView _instanceTreeView = new();

    public event EventHandler<MyTreeItem>? OnRequestFileOpen;


    public TreeViewHandler(Control control)
    {
        _treeFile = new FileInfo(Config.Default.WorkingFileFullName);
        _treeContainer = new();

        _treeview = CreateTreeView();
        _instanceTreeView = _treeview;
        control.Controls.Add(_treeview);

        ConfigureImageList();
    }

    TreeView CreateTreeView()
    {
        var treeview = new TreeView()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
            TabIndex = 0,
            ForeColor = Config.Default.ColorTreeviewForeground(),
            BackColor = Config.Default.ColorTreeviewBackground(),
            LabelEdit = true,
            AllowDrop = true,
        };
        treeview.KeyDown += Treeview_KeyDown;
        treeview.NodeMouseClick += TreeView_NodeMouseClick;
        treeview.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
        treeview.AfterLabelEdit += Treeview_AfterLabelEdit;
        treeview.ItemDrag += new ItemDragEventHandler(Treeview_ItemDrag);
        treeview.DragEnter += new DragEventHandler(Treeview_DragEnter);
        treeview.DragOver += new DragEventHandler(Treeview_DragOver);
        treeview.DragDrop += new DragEventHandler(Treeview_DragDrop);
        return treeview;
    }

    private void Treeview_ItemDrag(object? sender, ItemDragEventArgs e)
    {
        if (e.Item != null)
            _treeview.DoDragDrop(e.Item, DragDropEffects.Move);
    }

    private void Treeview_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data != null && e.Data.GetDataPresent(typeof(TreeNode)))
            e.Effect = DragDropEffects.Move;
        else
            e.Effect = DragDropEffects.None;
    }

    private void Treeview_DragOver(object? sender, DragEventArgs e)
    {
        Point targetPoint = _treeview.PointToClient(new Point(e.X, e.Y));

        // Seleciona o nó no local do ponteiro do mouse
        _treeview.SelectedNode = _treeview.GetNodeAt(targetPoint);
    }

    private void Treeview_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data == null)
            return;

        if (!e.Data.GetDataPresent(typeof(TreeNode)))
            return;

        TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode))!;
        // Obtem a posição do ponteiro do mouse sobre o TreeView
        Point targetPoint = _treeview.PointToClient(new Point(e.X, e.Y));
        TreeNode targetNode = _treeview.GetNodeAt(targetPoint);

        if (targetNode != null && draggedNode != targetNode)
        {
            // Remove o nó arrastado de sua posição original
            draggedNode.Remove();
            // Insere o nó arrastado abaixo do nó alvo
            targetNode.Nodes.Add(draggedNode);
            targetNode.Expand();

            // Atualiza o nó selecionado
            _treeview.SelectedNode = draggedNode;
        }
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
            treeNode.ForeColor = Config.Default.ColorTreeviewRootForeground();
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
            ResizeFont(e);

        else if (e.KeyCode == Keys.Delete && _treeview.SelectedNode != null)
            DeleteTreeItem();

        else if (e.KeyCode == Keys.F2 && _treeview.SelectedNode != null)
            RenameTreeItem();

    }

    private void ResizeFont(KeyEventArgs e)
    {
        var currentFont = _treeview.Font;
        var newSize = currentFont.Size + (e.KeyCode == Keys.Up ? 1 : -1);
        if (newSize < 1) newSize = 1;
        _treeview.Font = new Font(currentFont.FontFamily, newSize, currentFont.Style);
        e.Handled = true;
    }

    private void DeleteTreeItem()
    {
    }

    private void RenameTreeItem()
    {
        _treeview.SelectedNode.BeginEdit();
    }

    private void TreeView_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            _treeview.SelectedNode = e.Node;
            TreeViewContextMenuHandler.Show(_treeview, e.Node, e.Location);
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

    private void Treeview_AfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
    {
        if (e.Label == null)
        {
            e.CancelEdit = true;
            return;
        }

        if (e.Label.Trim().Length == 0)
        {
            MessageBox.Show(
                Properties.Resources.ErrorMsgEmptyName,
                Properties.Resources.TreeErrorValidation,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            e.CancelEdit = true;
            e.Node?.BeginEdit();
        }
    }

    internal static bool AddTask(string text, MyTaskType myTaskInfo)
    {
        var findText = string.Empty;
        switch (myTaskInfo)
        {
            case MyTaskType.Urgent:
                findText = Properties.Resources.TreePackageUrgentText;
                break;
            case MyTaskType.Important:
                findText = Properties.Resources.TreePackageImportantText;
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

    internal static void Sort(TreeNode node, SortOrder order = SortOrder.Ascending)
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

    internal static TreeNode? FindNodeByName(TreeNodeCollection nodes, string name, bool recursive = false)
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

    public void Dispose()
    {
    }
}
