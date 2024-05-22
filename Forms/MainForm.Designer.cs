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
        MainMenu.SuspendLayout();
        SuspendLayout();
        // 
        // MainMenu
        // 
        MainMenu.Items.AddRange(new ToolStripItem[] { MenuTools, MenuAbout });
        MainMenu.Location = new Point(0, 0);
        MainMenu.Name = "MainMenu";
        MainMenu.Size = new Size(722, 24);
        MainMenu.TabIndex = 1;
        MainMenu.Text = "menuStrip1";
        // 
        // MenuTools
        // 
        MenuTools.DropDownItems.AddRange(new ToolStripItem[] { MenuExplorer, MenuForceRefresh, MenuDarkScreen, toolStripMenuItem1, MenuSettings, toolStripMenuItem2, MenuExit });
        MenuTools.Name = "MenuTools";
        MenuTools.Size = new Size(46, 20);
        MenuTools.Text = "&Tools";
        // 
        // MenuExplorer
        // 
        MenuExplorer.Name = "MenuExplorer";
        MenuExplorer.Size = new Size(180, 22);
        MenuExplorer.Text = "&Explorer";
        // 
        // MenuForceRefresh
        // 
        MenuForceRefresh.Name = "MenuForceRefresh";
        MenuForceRefresh.Size = new Size(180, 22);
        MenuForceRefresh.Text = "&Force refresh";
        // 
        // MenuDarkScreen
        // 
        MenuDarkScreen.Name = "MenuDarkScreen";
        MenuDarkScreen.Size = new Size(180, 22);
        MenuDarkScreen.Text = "Dark screen";
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(177, 6);
        // 
        // MenuSettings
        // 
        MenuSettings.Name = "MenuSettings";
        MenuSettings.Size = new Size(180, 22);
        MenuSettings.Text = "&Settings";
        // 
        // toolStripMenuItem2
        // 
        toolStripMenuItem2.Name = "toolStripMenuItem2";
        toolStripMenuItem2.Size = new Size(177, 6);
        // 
        // MenuExit
        // 
        MenuExit.Name = "MenuExit";
        MenuExit.Size = new Size(180, 22);
        MenuExit.Text = "E&xit";
        // 
        // MenuAbout
        // 
        MenuAbout.Name = "MenuAbout";
        MenuAbout.Size = new Size(52, 20);
        MenuAbout.Text = "&About";
        // 
        // MyTasksNotifyIcon
        // 
        MyTasksNotifyIcon.Text = "notifyIcon1";
        MyTasksNotifyIcon.Visible = true;
        // 
        // MyTasksContextMenuStrip
        // 
        MyTasksContextMenuStrip.Name = "MyTasksContextMenuStrip";
        MyTasksContextMenuStrip.Size = new Size(61, 4);
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(722, 539);
        Controls.Add(MainMenu);
        IsMdiContainer = true;
        MainMenuStrip = MainMenu;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "My Tasks";
        WindowState = FormWindowState.Maximized;
        MainMenu.ResumeLayout(false);
        MainMenu.PerformLayout();
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
}