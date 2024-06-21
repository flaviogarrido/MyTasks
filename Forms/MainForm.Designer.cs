namespace MyTasks.Forms;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        MainMenu = new MenuStrip();
        MenuTools = new ToolStripMenuItem();
        MenuExplorer = new ToolStripMenuItem();
        MenuForceRefresh = new ToolStripMenuItem();
        MenuDarkScreen = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripSeparator();
        MenuSettings = new ToolStripMenuItem();
        toolStripMenuItem2 = new ToolStripSeparator();
        MenuExit = new ToolStripMenuItem();
        MenuAbout = new ToolStripMenuItem();
        MyTasksNotifyIcon = new NotifyIcon(components);
        MyTasksContextMenuStrip = new ContextMenuStrip(components);
        MainStatusBar = new StatusStrip();
        ClockLabel = new ToolStripStatusLabel();
        CountdownLabel = new ToolStripStatusLabel();
        MainTimer = new System.Windows.Forms.Timer(components);
        MainMenu.SuspendLayout();
        MainStatusBar.SuspendLayout();
        SuspendLayout();
        // 
        // MainMenu
        // 
        MainMenu.ImageScalingSize = new Size(24, 24);
        MainMenu.Items.AddRange(new ToolStripItem[] { MenuTools, MenuAbout });
        MainMenu.Location = new Point(0, 0);
        MainMenu.Name = "MainMenu";
        MainMenu.Padding = new Padding(9, 3, 0, 3);
        MainMenu.Size = new Size(1031, 35);
        MainMenu.TabIndex = 1;
        MainMenu.Text = "menuStrip1";
        // 
        // MenuTools
        // 
        MenuTools.DropDownItems.AddRange(new ToolStripItem[] { MenuExplorer, MenuForceRefresh, MenuDarkScreen, toolStripMenuItem1, MenuSettings, toolStripMenuItem2, MenuExit });
        MenuTools.Name = "MenuTools";
        MenuTools.Size = new Size(69, 29);
        MenuTools.Text = "&Tools";
        // 
        // MenuExplorer
        // 
        MenuExplorer.Name = "MenuExplorer";
        MenuExplorer.Size = new Size(324, 34);
        MenuExplorer.Text = "&Explorer";
        // 
        // MenuForceRefresh
        // 
        MenuForceRefresh.Name = "MenuForceRefresh";
        MenuForceRefresh.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F;
        MenuForceRefresh.Size = new Size(324, 34);
        MenuForceRefresh.Text = "&Force refresh";
        // 
        // MenuDarkScreen
        // 
        MenuDarkScreen.Name = "MenuDarkScreen";
        MenuDarkScreen.ShortcutKeys = Keys.Control | Keys.Shift | Keys.D;
        MenuDarkScreen.Size = new Size(324, 34);
        MenuDarkScreen.Text = "&Dark screen";
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(321, 6);
        // 
        // MenuSettings
        // 
        MenuSettings.Name = "MenuSettings";
        MenuSettings.Size = new Size(324, 34);
        MenuSettings.Text = "&Settings";
        // 
        // toolStripMenuItem2
        // 
        toolStripMenuItem2.Name = "toolStripMenuItem2";
        toolStripMenuItem2.Size = new Size(321, 6);
        // 
        // MenuExit
        // 
        MenuExit.Name = "MenuExit";
        MenuExit.Size = new Size(324, 34);
        MenuExit.Text = "E&xit";
        // 
        // MenuAbout
        // 
        MenuAbout.Name = "MenuAbout";
        MenuAbout.Size = new Size(78, 29);
        MenuAbout.Text = "&About";
        // 
        // MyTasksNotifyIcon
        // 
        MyTasksNotifyIcon.Text = "notifyIcon1";
        MyTasksNotifyIcon.Visible = true;
        // 
        // MyTasksContextMenuStrip
        // 
        MyTasksContextMenuStrip.ImageScalingSize = new Size(24, 24);
        MyTasksContextMenuStrip.Name = "MyTasksContextMenuStrip";
        MyTasksContextMenuStrip.Size = new Size(61, 4);
        // 
        // MainStatusBar
        // 
        MainStatusBar.ImageScalingSize = new Size(24, 24);
        MainStatusBar.Items.AddRange(new ToolStripItem[] { ClockLabel, CountdownLabel });
        MainStatusBar.Location = new Point(0, 866);
        MainStatusBar.Name = "MainStatusBar";
        MainStatusBar.Size = new Size(1031, 32);
        MainStatusBar.TabIndex = 3;
        MainStatusBar.Text = "statusStrip1";
        // 
        // ClockLabel
        // 
        ClockLabel.Font = new Font("Arial", 9F);
        ClockLabel.Name = "ClockLabel";
        ClockLabel.Size = new Size(56, 25);
        ClockLabel.Text = "--:--:--";
        // 
        // CountdownLabel
        // 
        CountdownLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
        CountdownLabel.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        CountdownLabel.Name = "CountdownLabel";
        CountdownLabel.Size = new Size(127, 25);
        CountdownLabel.Text = "Countdown: --";
        // 
        // MainTimer
        // 
        MainTimer.Enabled = true;
        MainTimer.Interval = 1000;
        MainTimer.Tick += MainTimer_Tick;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1031, 898);
        Controls.Add(MainStatusBar);
        Controls.Add(MainMenu);
        IsMdiContainer = true;
        MainMenuStrip = MainMenu;
        Margin = new Padding(4, 5, 4, 5);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "My Tasks";
        WindowState = FormWindowState.Maximized;
        MainMenu.ResumeLayout(false);
        MainMenu.PerformLayout();
        MainStatusBar.ResumeLayout(false);
        MainStatusBar.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip MainMenu;
    private ToolStripMenuItem MenuTools;
    private ToolStripMenuItem MenuExplorer;
    private ToolStripMenuItem MenuForceRefresh;
    private ToolStripMenuItem MenuDarkScreen;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem MenuSettings;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem MenuExit;
    private ToolStripMenuItem MenuAbout;
    private NotifyIcon MyTasksNotifyIcon;
    private ContextMenuStrip MyTasksContextMenuStrip;
    private StatusStrip MainStatusBar;
    private System.Windows.Forms.Timer MainTimer;
    private ToolStripStatusLabel ClockLabel;
    private ToolStripStatusLabel CountdownLabel;
}