namespace MyTasks.Forms;
public partial class DarkForm : Form
{
    public DarkForm()
    {
        InitializeComponent();

        TopMost = true;

        StartPosition = FormStartPosition.Manual;

        if (!Config.Default.DarkScreenShowButton)
            CloseButton.Visible = false;
        else
            CloseButton.Click += (sender, e) => { Close(); };

        DoubleClick += (sender, e) => { Close(); };
    }
}
