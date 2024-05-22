namespace MyTasks.Forms;

partial class PromptForm
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
        PromptLabel = new Label();
        PromptTextBox = new TextBox();
        ConfirmButton = new Button();
        SuspendLayout();
        // 
        // PromptLabel
        // 
        PromptLabel.AutoSize = true;
        PromptLabel.Location = new Point(24, 24);
        PromptLabel.Name = "PromptLabel";
        PromptLabel.Size = new Size(38, 15);
        PromptLabel.TabIndex = 0;
        PromptLabel.Text = "label1";
        // 
        // PromptTextBox
        // 
        PromptTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        PromptTextBox.Location = new Point(24, 47);
        PromptTextBox.Name = "PromptTextBox";
        PromptTextBox.Size = new Size(342, 23);
        PromptTextBox.TabIndex = 1;
        // 
        // ConfirmButton
        // 
        ConfirmButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        ConfirmButton.DialogResult = DialogResult.OK;
        ConfirmButton.Location = new Point(266, 76);
        ConfirmButton.Name = "ConfirmButton";
        ConfirmButton.Size = new Size(100, 23);
        ConfirmButton.TabIndex = 2;
        ConfirmButton.Text = "OK";
        ConfirmButton.UseVisualStyleBackColor = true;
        ConfirmButton.Click += ConfirmButton_Click;
        // 
        // PromptForm
        // 
        AcceptButton = ConfirmButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(378, 111);
        Controls.Add(ConfirmButton);
        Controls.Add(PromptTextBox);
        Controls.Add(PromptLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Name = "PromptForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PromptForm";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label PromptLabel;
    private TextBox PromptTextBox;
    private Button ConfirmButton;
}