using System.Text;

namespace MyTasks.Forms;

internal class TreeViewContextMenuHandler
{
    internal static void Show(TreeViewHandler handler, TreeView treeview, TreeNode node, Point location)
    {
        ContextMenuStrip menu = new();

        AddMenuItem(Properties.Resources.CTMenuCreateTreeItem, menu, () => { CreateItem(handler, node); });
        AddMenuItem(Properties.Resources.CTMenuRemoveTreeItem, menu, () => { RemoveItem(handler, node); });
        AddMenuItem(Properties.Resources.CTMenuRenameTreeItem, menu, () => { RenameItem(node); });

        menu.Items.Add("-");
        AddMenuItem(Properties.Resources.CTMenuFind, menu, () => { Find(); });
        AddMenuItem(Properties.Resources.CTMenuFindNext, menu, () => { FindNext(); });
        AddMenuItem(Properties.Resources.CTMenuFindPrev, menu, () => { FindPrev(); });

        menu.Items.Add("-");
        AddMenuItem(Properties.Resources.CTSort, menu, () => { Sort(node); });
        AddMenuItem(Properties.Resources.CTExportTreeNode, menu, () => { ExportTreeNode(node); });
        AddMenuItem(Properties.Resources.CTExpandAll, menu, () => { ExpandAll(node); });
        AddMenuItem(Properties.Resources.CTExpandCollapse, menu, () => { ExpandCollapse(node); });
        

        if (menu.Items.Count > 0)
            menu.Show(treeview, location);
    }

    private static void CreateItem(TreeViewHandler handler, TreeNode node)
    {
        handler.CreateItem(node);
    }

    private static void RemoveItem(TreeViewHandler handler, TreeNode node)
    {
        handler.RemoveTreeNode(node);
    }

    private static void RenameItem(TreeNode node)
    {
        node.BeginEdit();
    }

    private static void Find()
    {
        MessageBox.Show("falta implementar");
    }

    private static void FindNext()
    {
        MessageBox.Show("falta implementar");
    }

    private static void FindPrev()
    {
        MessageBox.Show("falta implementar");
    }

    private static void Sort(TreeNode node)
    {
        TreeViewHandler.Sort(node);
    }

    private static void ExportTreeNode(TreeNode node)
    {
        CopyToClipboard(node);
    }

    private static void ExpandAll(TreeNode node)
    {
        node.ExpandAll();
    }

    private static void ExpandCollapse(TreeNode node)
    {
        node.Collapse();
    }

    private static void CopyToClipboard(TreeNode node)
    {
        var sb = new StringBuilder();
        GetTextFromNodeAndSubNodes(sb, node, 0);
        Clipboard.SetText(sb.ToString());
    }

    private static void GetTextFromNodeAndSubNodes(StringBuilder sb, TreeNode node, int level)
    {
        var ident = string.Concat(Enumerable.Repeat("\t", level));
        sb.Append(ident);
        sb.AppendLine(node.Text);

        if (node.Nodes.Count > 0)
        {
            int newLevel = level + 1;
            foreach (var subNode in node.Nodes)
                GetTextFromNodeAndSubNodes(sb, (subNode as TreeNode)!, newLevel);
        }
    }

    static void AddMenuItem(string menuText, ContextMenuStrip menu, Action action)
    {
        var item = menu.Items.Add(menuText);
        item.Click += (sender, e) => action.Invoke();
    }
}