namespace MyTasks.Forms;

partial class DarkForm
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
        CloseButton = new Button();
        SuspendLayout();
        // 
        // CloseButton
        // 
        CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        CloseButton.BackColor = Color.DimGray;
        CloseButton.FlatStyle = FlatStyle.Popup;
        CloseButton.Location = new Point(302, 180);
        CloseButton.Name = "CloseButton";
        CloseButton.Size = new Size(75, 23);
        CloseButton.TabIndex = 0;
        CloseButton.Text = "Fechar";
        CloseButton.UseVisualStyleBackColor = false;
        // 
        // DarkForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Black;
        CancelButton = CloseButton;
        ClientSize = new Size(389, 215);
        Controls.Add(CloseButton);
        FormBorderStyle = FormBorderStyle.None;
        Name = "DarkForm";
        ShowInTaskbar = false;
        Text = "DarkForm";
        WindowState = FormWindowState.Maximized;
        ResumeLayout(false);
    }

    #endregion

    private Button CloseButton;
}