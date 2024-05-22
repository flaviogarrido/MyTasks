
namespace MyTasks.Tree;

internal class MyTreeContainer
{
    MyTreeItem _root;
    MyTreeItem _pkgUrgent;
    MyTreeItem _pkgImportant;

    public MyTreeContainer()
    {
        _root = new MyTreeItem(Properties.Resources.RootText);
        _pkgUrgent = MyTreeItem.CreatePackage(Properties.Resources.PackageUrgentText);
        _pkgImportant = MyTreeItem.CreatePackage(Properties.Resources.PackageImportantText);
        ConfigureRoot();
        LoadRepository();
    }

    private void ConfigureRoot()
    {
        _root.Items.Add(_pkgUrgent);
        _root.Items.Add(_pkgImportant);
    }

    private void LoadRepository()
    {
    }

    public MyTreeItem GetRoot()
        => _root;

}
