
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

            _root = new MyTreeItem(Properties.Resources.TreeRootText, Guid.Empty);
            _root.Style = MyTreeItemStyleInfo.Root;

            _pkgUrgent = MyTreeItem.CreatePackage(Properties.Resources.TreePackageUrgentText, _root.Id);
            _pkgUrgent.Style = MyTreeItemStyleInfo.DocumentWarning;

            _pkgImportant = MyTreeItem.CreatePackage(Properties.Resources.TreePackageImportantText, _root.Id);
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

        foreach(var line in File.ReadLines(fileinfo.FullName))
        {
            var treeItem = MyTreeItem.CreateFromString(line);
            if (treeItem == null) 
                continue;

            if (treeItem.ParentId.Equals(Guid.Empty))
            {
                _root = treeItem;
                continue;
            }

            else if (treeItem.ParentId.Equals(_root?.Id))

                if (treeItem.Style == MyTreeItemStyleInfo.DocumentWarning)
                    _pkgUrgent = treeItem;

                else if (treeItem.Style == MyTreeItemStyleInfo.DocumentPinned)
                    _pkgImportant = treeItem;


            if (_root == null)
                continue;

            var parent = FindByParentId(treeItem.ParentId, _root);
            if (parent != null)
                parent.Items.Add(treeItem);
        }
    }

    private static MyTreeItem? FindByParentId(Guid parentId, MyTreeItem treeItem)
    {
        if (treeItem.Id.Equals(parentId)) 
            return treeItem;

        MyTreeItem? result = null;
        foreach(var treeSubItem in treeItem.Items)
            result = FindByParentId(parentId, treeSubItem);

        return result;
    }

    public MyTreeItem GetRoot()
        => _root;

}
