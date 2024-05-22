namespace MyTasks.Forms;
public partial class PromptForm : Form
{
    public PromptForm(string title, string promptLabel, string promptText = "")
    {
        InitializeComponent();
        
        Text = title;
        PromptLabel.Text = promptLabel;
        PromptTextBox.Text = promptText;
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        Close();
    }

    public string PromptText => PromptTextBox.Text;
}
