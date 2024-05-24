using System.Text;

namespace MyTasks.Forms;

internal class TreeViewContextMenuHandler
{
    internal static void Show(TreeView treeview, TreeNode node, Point location)
    {
        ContextMenuStrip menu = new();

        AddMenuItem(Properties.Resources.CTMenuCreateTreeItem, menu, () => { MessageBox.Show("falta implementar"); });
        AddMenuItem(Properties.Resources.CTMenuRemoveTreeItem, menu, () => { MessageBox.Show("falta implementar"); });
        AddMenuItem(Properties.Resources.CTMenuRenameTreeItem, menu, () => { MessageBox.Show("falta implementar"); });

        menu.Items.Add("-");
        AddMenuItem(Properties.Resources.CTMenuFind, menu, () => { MessageBox.Show("falta implementar"); });
        AddMenuItem(Properties.Resources.CTMenuFindNext, menu, () => { MessageBox.Show("falta implementar"); });
        AddMenuItem(Properties.Resources.CTMenuFindPrev, menu, () => { MessageBox.Show("falta implementar"); });

        menu.Items.Add("-");
        AddMenuItem(Properties.Resources.CTSort, menu, () => { TreeViewHandler.Sort(node); });
        AddMenuItem(Properties.Resources.CTExportTreeNode, menu, () => { CopyToClipboard(node); });
        AddMenuItem(Properties.Resources.CTExpandAll, menu, () => { node.ExpandAll(); });
        AddMenuItem(Properties.Resources.CTExpandCollapse, menu, () => { node.Collapse(); });
        

        if (menu.Items.Count > 0)
            menu.Show(treeview, location);
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