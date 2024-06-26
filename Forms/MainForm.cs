﻿using MyTasks.Models;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MyTasks.Forms;

public partial class MainForm : Form
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern uint SetThreadExecutionState(uint esFlags);
    [DllImport("user32.dll")]
    static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    [DllImport("user32.dll")]
    static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    const uint ES_CONTINUOUS = 0x80000000;
    const uint ES_SYSTEM_REQUIRED = 0x00000001;
    const uint ES_DISPLAY_REQUIRED = 0x00000002;

    const int HOTKEY_0 = 0;
    const int HOTKEY_1 = 1;
    const int HOTKEY_2 = 2;

    ExplorerForm? _taskForm;
    CancellationTokenSource _cancellationTokenSource = new();

    static MainForm _thisForm;

    public MainForm()
    {
        InitializeComponent();
        InitializeForm();
        InitializeMenu();
        InitializeHotkey();
        InitializeNotifyIcon();
        InitializeStatusBar();

        _thisForm = this;
    }

    private void InitializeForm()
    {
        SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);

        Text = $"My tasks - v{AppConstants.VERSION}";
        Icon = Properties.Resources.Alfred;
        WindowState = FormWindowState.Maximized;

        MenuExplorer_Click(this, EventArgs.Empty);
    }

    private void InitializeStatusBar()
    {
        MainStatusBar.BackColor = Config.Default.ColorTreeviewBackground();
        MainStatusBar.ForeColor = Config.Default.ColorTreeviewForeground();
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

        _taskForm = new(this, MainMenu);
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
        foreach (var screen in Screen.AllScreens)
        {
            var form = new DarkForm()
            {
                Bounds = screen.Bounds,
            };
            form.Show();
        }
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
        RegisterHotKey(this.Handle, HOTKEY_0, 0x0001 | 0x0002, 0x30); // 0x30 = 0
        RegisterHotKey(this.Handle, HOTKEY_1, 0x0001 | 0x0002, 0x31); // 0x31 = 1
        RegisterHotKey(this.Handle, HOTKEY_2, 0x0001 | 0x0002, 0x32); // 0x32 = 2
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        // 0x0312 é o código de mensagem para WM_HOTKEY
        if (m.Msg != 0x0312)
            return;

        switch (m.WParam.ToInt32())
        {
            case HOTKEY_0:
                Hotkey0();
                break;
            case HOTKEY_1:
                Hotkey1();
                break;
            case HOTKEY_2:
                Hotkey2();
                break;
        }
    }

    private static void Hotkey0() //Hotkey CTRL + ALT + 0
        => AlternateWindowState();

    private static void Hotkey1() //Hotkey CTRL + ALT + 1
        => AddUrgentTask();

    private static void Hotkey2() //Hotkey CTRL + ALT + 2
        => AddImportantTask();

    private static void AlternateWindowState()
    {
        if (_thisForm.WindowState == FormWindowState.Minimized)
        {
            _thisForm.Show();
            _thisForm.WindowState = FormWindowState.Maximized;
        }
        else
            _thisForm.WindowState = FormWindowState.Minimized;
    }

    private static void AddUrgentTask()
    {
        var text = Prompt.ShowDialog(Properties.Resources.AddTaskUrgentDialogTitle, Properties.Resources.AddTaskDialogLabel);
        text = text.Replace(';', '.');
        TreeViewHandler.AddTask(text, MyTaskType.Urgent);
    }

    private static void AddImportantTask()
    {
        var text = Prompt.ShowDialog(Properties.Resources.AddTaskImportantDialogTitle, Properties.Resources.AddTaskDialogLabel);
        text = text.Replace(';', '.');
        TreeViewHandler.AddTask(text, MyTaskType.Important);
    }

    private void InitializeNotifyIcon()
    {
        MyTasksContextMenuStrip = new ContextMenuStrip();
        MyTasksContextMenuStrip.Items.Add("&Show", null, ShowApplication);
        MyTasksContextMenuStrip.Items.Add("Dark screen", null, MenuDarkScreen_Click);
        MyTasksContextMenuStrip.Items.Add("-");
        MyTasksContextMenuStrip.Items.Add("E&xit", null, MenuExit_Click);

        MyTasksNotifyIcon = new NotifyIcon();
        MyTasksNotifyIcon.Text = "My Tasks";
        MyTasksNotifyIcon.Icon = Properties.Resources.Alfred;
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
        SetThreadExecutionState(ES_CONTINUOUS);

        UnregisterHotKey(this.Handle, HOTKEY_0);
        UnregisterHotKey(this.Handle, HOTKEY_1);
        UnregisterHotKey(this.Handle, HOTKEY_2);
        MyTasksNotifyIcon?.Dispose();
        base.OnFormClosing(e);
    }

    private void MainTimer_Tick(object sender, EventArgs e)
    {
        //Clock
        ClockLabel.Text = DateTime.Now.ToString("HH:mm:ss");

        //Countdown
        UpdateCountdown();
    }

    private async void UpdateCountdown()
    {
        var countdownDateTime = Config.Default.Countdown;
        if (countdownDateTime == null)
        {
            CountdownLabel.Visible = false;
        }
        else
        {
            CountdownLabel.Visible = true;
            string result = await Task.Run(() => GetTimeUntil(countdownDateTime.Value));
            CountdownLabel.Text = result;
        }
    }

    public static string GetTimeUntil(DateTime futureDate)
    {
        TimeSpan timeSpan = futureDate.Subtract(DateTime.Now);

        int weeks = -1;
        if (timeSpan.Days >= 7)
            weeks = timeSpan.Days / 7;

        if (weeks == -1)
            return Properties.Resources.StatusBarCountdownTextDays
                .Replace("{0}", $"{timeSpan.Days}");
        else
            return Properties.Resources.StatusBarCountdownTextWeeks
                .Replace("{0}", $"{timeSpan.Days}")
                .Replace("{1}", $"{weeks}");
    }

}
