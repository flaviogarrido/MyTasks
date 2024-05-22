namespace MyTasks.Tree;

internal class MyTreeItem
{
    public MyTreeItem()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Classification = MyTreeClassInfo.None;
        Items = new();
        File = null;
        Properties = new();
    }

    public MyTreeItem(string name) : this()
    {
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public MyTreeClassInfo Classification { get; set; }
    public FileInfo? File { get; set; }
    public List<MyTreeItem> Items { get; set; }
    public Dictionary<string, string> Properties { get; set; }

    public static MyTreeItem Create(MyTreeItem treeItem, List<MyTreeItem> treeItems)
    {
        var name = treeItem.Name;

        var result = treeItems
            .Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        if (result != null)
            return result;

        treeItems.Add(treeItem);
        return treeItem;
    }

    public static MyTreeItem CreatePackage(string packageText)
        => new MyTreeItem(packageText)
        {
            Classification = MyTreeClassInfo.Package
        };

    public static MyTreeItem CreateTask(string taskText)
    {
        var newTask = new MyTreeItem(taskText)
        {
            Classification = MyTreeClassInfo.Task
        };

        var filename = Path.Combine(
            Config.Default.WorkingFolder,
            $"task_{newTask.Id}.txt");

        newTask.File = new FileInfo(filename);

        if (!newTask.File.Exists)
        {
            if (!newTask.File.Directory!.Exists)
                Directory.CreateDirectory(newTask.File.DirectoryName!);

            System.IO.File.WriteAllText(newTask.File.FullName, $"");
        }

        return newTask;
    }
}
