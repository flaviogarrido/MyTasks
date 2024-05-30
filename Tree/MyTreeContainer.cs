
namespace MyTasks.Tree;

internal class MyTreeContainer
{
    MyTreeItem _root;
    MyTreeItem _pkgUrgent;
    MyTreeItem _pkgImportant;

    public MyTreeContainer()
    {
        _pkgUrgent = new();
        _pkgImportant = new();

        LoadRepository();

        if (_root == null)
        {

            _root = new MyTreeItem(Properties.Resources.TreeRootText);
            _root.Style = MyTreeItemStyleInfo.Root;

            _pkgUrgent = MyTreeItem.CreatePackage(Properties.Resources.TreePackageUrgentText, _root);
            _pkgUrgent.Style = MyTreeItemStyleInfo.DocumentWarning;

            _pkgImportant = MyTreeItem.CreatePackage(Properties.Resources.TreePackageImportantText, _root);
            _pkgImportant.Style = MyTreeItemStyleInfo.DocumentPinned;

            _root.Items.Add(_pkgUrgent);
            _root.Items.Add(_pkgImportant);
        }
    }


    private void LoadRepository()
    {
        FileInfo fileinfo = new FileInfo(Config.Default.WorkingFileFullName);
        if (!fileinfo.Exists)
            return;

        foreach (var line in File.ReadLines(fileinfo.FullName))
        {
            var arrStringTreeItem = line.Split(';');
            var parentId = Guid.Empty;
            try
            {
                parentId = Guid.Parse(arrStringTreeItem[4]);
            } catch { }

            MyTreeItem? parent = null;
            if (!parentId.Equals(Guid.Empty) && _root != null)
                parent = FindByParentId(parentId, _root);

            var treeItem = MyTreeItem.CreateFromString(line, parent);
            if (treeItem == null)
                continue;

            if (treeItem.Parent == null || treeItem.Parent.Id.Equals(Guid.Empty))
            {
                _root = treeItem;
                continue;
            }

            else if (treeItem.Parent.Id.Equals(_root?.Id))

                if (treeItem.Style == MyTreeItemStyleInfo.DocumentWarning)
                    _pkgUrgent = treeItem;

                else if (treeItem.Style == MyTreeItemStyleInfo.DocumentPinned)
                    _pkgImportant = treeItem;


            if (_root == null)
                continue;

            if (parent != null)
                parent.Items.Add(treeItem);
        }
    }

    private static MyTreeItem? FindByParentId(Guid parentId, MyTreeItem? treeItem)
    {
        if (treeItem == null)
            return null;

        if (treeItem.Id.Equals(parentId))
            return treeItem;

        MyTreeItem? result = null;
        foreach (var treeSubItem in treeItem.Items)
            result = FindByParentId(parentId, treeSubItem);

        return result;
    }

    public MyTreeItem GetRoot()
        => _root;

}
