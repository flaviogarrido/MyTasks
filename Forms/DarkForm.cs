using System.Runtime.InteropServices;

namespace MyTasks.Forms;
public partial class DarkForm : Form
{

    //[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //static extern uint SetThreadExecutionState(uint esFlags);

    //const uint ES_CONTINUOUS = 0x80000000;
    //const uint ES_SYSTEM_REQUIRED = 0x00000001;
    //const uint ES_DISPLAY_REQUIRED = 0x00000002;


    public DarkForm(bool showClose = false)
    {
        StartPosition = FormStartPosition.Manual;
        InitializeComponent();

        if (!showClose)
            CloseButton.Visible = false;
        else
            CloseButton.Click += (sender, e) => { Close(); };

        DoubleClick += (sender, e) => { Close(); };
    }

    //protected override void OnLoad(EventArgs e)
    //{
    //    base.OnLoad(e);
    //    SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
    //}

    //protected override void OnFormClosed(FormClosedEventArgs e)
    //{
    //    base.OnFormClosed(e);
    //    SetThreadExecutionState(ES_CONTINUOUS);
    //}
}
