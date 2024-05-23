namespace MyTasks.Tree;

internal class MyTreeItem
{
    public MyTreeItem()
    {
        Id = Guid.NewGuid();
        ParentId = Guid.Empty;
        Name = string.Empty;
        Classification = MyTreeClassInfo.None;
        Items = new();
        File = null;
        Properties = new();
        Style = MyTreeItemStyleInfo.Document;
    }

    public MyTreeItem(string name, Guid parentId) : this()
    {
        Name = name;
        ParentId = parentId;
    }

    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string Name { get; set; }
    public MyTreeClassInfo Classification { get; set; }
    public FileInfo? File { get; set; }
    public List<MyTreeItem> Items { get; set; }
    public Dictionary<string, string> Properties { get; set; }
    public MyTreeItemStyleInfo Style { get; set; }

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

    public static MyTreeItem CreatePackage(string packageText, Guid parentId)
        => new MyTreeItem(packageText, parentId)
        {
            Classification = MyTreeClassInfo.Package
        };

    public static MyTreeItem CreateTask(string taskText, Guid parentId)
    {
        var newTask = new MyTreeItem(taskText, parentId)
        {
            Classification = MyTreeClassInfo.Task,
            Style = MyTreeItemStyleInfo.DocumentAdd
        };

        DefineFile(newTask);

        if (newTask.File != null && !newTask.File.Exists)
        {
            if (!newTask.File.Directory!.Exists)
                Directory.CreateDirectory(newTask.File.DirectoryName!);

            System.IO.File.WriteAllText(newTask.File.FullName, $"");
        }

        return newTask;
    }

    private static void DefineFile(MyTreeItem treeItem)
    {
        if (treeItem.Classification != MyTreeClassInfo.Task) 
            return;

        var filename = Path.Combine(
            Config.Default.WorkingFolder,
            $"task_{treeItem.Id}.txt");

        treeItem.File = new FileInfo(filename);
    }

    internal static MyTreeItem CreateFromString(string strTreeItem)
    {
        var arrStringTreeItem = strTreeItem.Split(';');

        var newTreeItem = new MyTreeItem()
        {
            Id = new Guid(arrStringTreeItem[0]),
            Name = arrStringTreeItem[1],
            Classification = (MyTreeClassInfo)int.Parse(arrStringTreeItem[2]),
            Style = (MyTreeItemStyleInfo)int.Parse(arrStringTreeItem[3]),
            ParentId = new Guid(arrStringTreeItem[4]),
        };
        DefineFile(newTreeItem);
        return newTreeItem;
    }

    public override string ToString()
    {
        return 
            $"{Id};" +
            $"{Name};" +
            $"{(int)Classification};" +
            $"{(int)Style};" +
            $"{ParentId}";
    }
}
