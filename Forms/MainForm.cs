using System.Runtime.InteropServices;

namespace MyTasks.Forms;

public partial class MainForm : Form
{
    [DllImport("user32.dll")]
    static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    
    [DllImport("user32.dll")]
    static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    const int HOTKEY_ID = 1;

    ExplorerForm? _taskForm;
    CancellationTokenSource _cancellationTokenSource = new();

    public MainForm()
    {
        InitializeComponent();
        InitializeForm();
        InitializeMenu();
        InitializeHotkey();
        InitializeNotifyIcon();
    }

    private void InitializeForm()
    {
        Text = $"My tasks - v{AppConstants.VERSION}";
        WindowState = FormWindowState.Maximized;

        MenuExplorer_Click(this, EventArgs.Empty);
    }

    private void InitializeMenu()
    {
        MenuExplorer.Click += MenuExplorer_Click;
        MenuForceRefresh.Click += MenuForceRefresh_Click;
        MenuSettings.Click += MenuSettings_Click;
        MenuDarkScreen.Click += MenuDarkScreen_Click;
        MenuExit.Click += MenuExit_Click;
        MenuAbout.Click += MenuAbout_Click;
    }

    private void MenuExplorer_Click(object? sender, EventArgs e)
    {
        if (_taskForm != null)
            return;

        _taskForm = new(this);
        _taskForm.FormClosed += TaskForm_FormClosed;
        _taskForm.Show();
    }

    private void TaskForm_FormClosed(object? sender, FormClosedEventArgs e)
    {
        _taskForm?.Dispose();
        _taskForm = null;
    }

    private void MenuForceRefresh_Click(object? sender, EventArgs e)
    {
        _taskForm?.Close();
        
        Config.Default.Load();
        MenuExplorer_Click(this, EventArgs.Empty);
    }

    private void MenuDarkScreen_Click(object? sender, EventArgs e)
    {
        var form = new DarkForm();
        form.ShowDialog();
    }

    private void MenuSettings_Click(object? sender, EventArgs e)
    {
        var form = new SettingsForm();
        form.ShowDialog();
    }
    private void MenuExit_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    private void MenuAbout_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("Software de apoio as tarefas do dia a dia");
    }

    private void InitializeHotkey()
    {
        // 0x0001 = MOD_ALT
        // 0x0002 = MOD_CONTROL
        // 0x31 = 1
        RegisterHotKey(this.Handle, HOTKEY_ID, 0x0001 | 0x0002, 0x31);
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        
        // 0x0312 é o código de mensagem para WM_HOTKEY
        if (m.Msg == 0x0312 && m.WParam.ToInt32() == HOTKEY_ID) 
            MessageBox.Show("Hotkey CTRL + ALT + 1 pressionada!");
    }

    private void InitializeNotifyIcon()
    {
        MyTasksContextMenuStrip = new ContextMenuStrip();
        MyTasksContextMenuStrip.Items.Add("&Show", null, ShowApplication);
        MyTasksContextMenuStrip.Items.Add("Dark screen", null, MenuDarkScreen_Click);
        MyTasksContextMenuStrip.Items.Add("-");
        MyTasksContextMenuStrip.Items.Add("E&xit", null, MenuExit_Click);

        MyTasksNotifyIcon = new NotifyIcon();
        MyTasksNotifyIcon.Text = "Minha Aplicação";
        MyTasksNotifyIcon.Icon = Properties.Resources.TrayIcon;
        MyTasksNotifyIcon.ContextMenuStrip = MyTasksContextMenuStrip;
        MyTasksNotifyIcon.DoubleClick += MyTasksNotifyIcon_DoubleClick;
        MyTasksNotifyIcon.Visible = true;
    }

    private void MyTasksNotifyIcon_DoubleClick(object? sender, EventArgs e)
    {
        ShowApplication(sender, e);
    }

    private void ShowApplication(object? sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Maximized;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (WindowState == FormWindowState.Minimized)
            Hide();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        UnregisterHotKey(this.Handle, HOTKEY_ID);
        MyTasksNotifyIcon?.Dispose();
        base.OnFormClosing(e);
    }
}
