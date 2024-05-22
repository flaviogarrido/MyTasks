namespace MyTasks.Forms;

internal class Prompt
{
    public static string ShowDialog(string title, string label, string text = "")
    {
        var form = new PromptForm(title, label, text);
        return form.ShowDialog() == DialogResult.OK ? form.PromptText : text;
    }
}
