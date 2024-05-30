using FastColoredTextBoxNS;
using MyTasks.Tree;
using System.ComponentModel;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;

namespace MyTasks.Forms;

internal class NotepadHandler : IDisposable
{
    IContainer _components;

    Label _titleLabel;
    FastColoredTextBox _notepad;
    FileInfo? _file;
    TextStyle _hyperlinkStyle;

    MenuStrip _mainMenu;
    ToolStripMenuItem _editMenuItem = new();
    ToolStripMenuItem _findMenuItem = new();
    ToolStripMenuItem _replaceMenuItem = new();
    ToolStripMenuItem _setSelectedAsReadonlyMenuItem = new();
    ToolStripMenuItem _setSelectedAsWritableMenuItem = new();
    ToolStripMenuItem _collapseSelectedBlockMenuItem = new();
    ToolStripMenuItem _increaseIndentMenuItem = new();
    ToolStripMenuItem _decreaseIndentMenuItem = new();
    ToolStripMenuItem _commentSelectedLinesMenuItem = new();
    ToolStripMenuItem _uncommentSelectedLinesMenuItem = new();
    ToolStripMenuItem _goBackwardCtrlMenuItem = new();
    ToolStripMenuItem _goForwardCtrlShiftMenuItem = new();
    ToolStripMenuItem _autoIndentMenuItem = new();
    ToolStripMenuItem _textToSpeech = new();
    ToolStripMenuItem _print = new();
    ToolStripMenuItem _startStopMacroRecordingMenuItem = new();
    ToolStripMenuItem _executeMacroMenuItem = new();
    ToolStripMenuItem _exportMenuItem = new();
    ToolStripMenuItem _exportHTMLMenuItem = new();
    ToolStripMenuItem _exportRTFMenuItem = new();

    SpeechSynthesizer _synthesizer = new();
    string _remainingText = string.Empty;


    public NotepadHandler(Control control, MenuStrip mainMenu)
    {
        _components = new Container();
        var resources = new ComponentResourceManager(typeof(NotepadHandler));

        _mainMenu = mainMenu;
        ConfigureMenu();
        ConfigureTextToSpeech();

        _hyperlinkStyle = new TextStyle(Config.Default.ColorNotepadHyperlink(), null, FontStyle.Underline | FontStyle.Bold);

        _titleLabel = CreateTitle(control);
        _notepad = CreateNotepad();
        control.Controls.Add(_titleLabel);
        control.Controls.Add(_notepad);
        _notepad.BringToFront();
    }

    #region Notepad
    private FastColoredTextBox CreateNotepad()
    {
        var notepad = new FastColoredTextBox()
        {
            Dock = DockStyle.Fill,
            Location = new Point(0, 0),
            ForeColor = Config.Default.ColorNotepadForeground(),
            BackColor = Config.Default.ColorNotepadBackground(),
            LineNumberColor = Config.Default.ColorNotepadLineNumber(),
            BookmarkColor = Config.Default.ColorNotepadBookmark(),
            PaddingBackColor = Config.Default.ColorNotepadPadding(),
            IndentBackColor = Config.Default.ColorNotepadIndent(),

            WordWrap = true,
        };
        notepad.TextChanged += Notepad_TextChanged;
        notepad.TextChangedDelayed += Notepad_TextChangedDelayed;
        notepad.MouseMove += Notepad_MouseMove;
        notepad.MouseDown += Notepad_MouseDown;
        
        return notepad;
    }
    #endregion
    #region Title of notepad
    private Label CreateTitle(Control control)
    {
        return new Label()
        {
            Dock = DockStyle.Top,
            Size = new Size(100, control.Font.Height * 2 + 4),
            Text = string.Empty,
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Config.Default.ColorNotepadTitleBackground(),
            ForeColor = Config.Default.ColorNotepadTitleForeground(),
        };
    }
    #endregion
    #region Text to speech
    private void ConfigureTextToSpeech()
    {
        ConfigurePortugueseVoice();
        _synthesizer.SpeakProgress += new EventHandler<SpeakProgressEventArgs>(Synthesizer_SpeakProgress);
        _synthesizer.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(Synthesizer_SpeakCompleted);
    }

    private void Synthesizer_SpeakProgress(object? sender, SpeakProgressEventArgs e)
    {
        _remainingText = _notepad.Text.Substring(e.CharacterPosition + e.CharacterCount);
    }

    private void Synthesizer_SpeakCompleted(object? sender, SpeakCompletedEventArgs e)
    {
        _remainingText = string.Empty;
    }
    private void RestartSpeech(int rateIncrease = 0)
    {
        if (!string.IsNullOrEmpty(_remainingText))
        {
            if (_synthesizer.Rate > -10 && _synthesizer.Rate < 10)
            {
                _synthesizer.Rate += rateIncrease;
                _synthesizer.SpeakAsyncCancelAll();
                _synthesizer.SpeakAsync(_remainingText);
            }
        }
    }

    private void ShowTextToSpeechControl()
    {
        var panel = new FlowLayoutPanel()
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlowDirection = FlowDirection.TopDown,
        };
        var closeButton = CreateToolButton("Close", (sender, e) => { });
        var playButton = CreateToolButton("Play", (sender, e) => { _synthesizer?.SpeakAsync(_notepad.Text); });
        var stopButton = CreateToolButton("Stop", (sender, e) => { _synthesizer?.SpeakAsyncCancelAll(); });
        var increaseSpeedButton = CreateToolButton("Increase Speed", (sender, e) => { RestartSpeech(1); });
        var decreaseSpeedButton = CreateToolButton("Decrease Speed", (sender, e) => { RestartSpeech(-1); });

        panel.Controls.Add(closeButton);
        panel.Controls.Add(playButton);
        panel.Controls.Add(stopButton);
        panel.Controls.Add(increaseSpeedButton);
        panel.Controls.Add(decreaseSpeedButton);
        panel.Refresh();

        var form = new Form()
        {
            Text = "TTS",
            FormBorderStyle = FormBorderStyle.SizableToolWindow,
            StartPosition = FormStartPosition.CenterParent,
            Size = new Size(panel.PreferredSize.Width + 24, panel.PreferredSize.Height + 64),
            CancelButton = closeButton,
        };
        form.Controls.Add(panel);
        form.ShowDialog();
    }

    private static Button CreateToolButton(string text, EventHandler eventHandler)
    {
        var label = new Label()
        {
            AutoSize = true,
            Text = text,
        };

        var button = new Button()
        {
            Text = text,
            Height = label.PreferredSize.Height + 20,
            Width = label.PreferredSize.Width + 40,
        };
        button.Click += eventHandler;
        return button;
    }

    private void ConfigurePortugueseVoice()
    {
        foreach (var voice in _synthesizer.GetInstalledVoices())
        {
            var info = voice.VoiceInfo;
            if (info.Culture.Name.Equals("pt-br", StringComparison.OrdinalIgnoreCase))
            {
                _synthesizer.SelectVoice(info.Name);
                break;
            }
        }
    }
    #endregion
    #region Menu
    private void ConfigureMenu()
    {
        _editMenuItem.Text = Properties.Resources.MenuEditText;
        _findMenuItem.Text = Properties.Resources.MenuFindText;
        _findMenuItem.Click += (sender, e) => { _notepad.ShowFindDialog(); };

        _replaceMenuItem.Text = Properties.Resources.MenuReplaceText;
        _replaceMenuItem.Click += (sender, e) => { _notepad.ShowReplaceDialog(); };

        _setSelectedAsReadonlyMenuItem.Text = Properties.Resources.MenuSetSelectedAsReadonlyText;
        _setSelectedAsReadonlyMenuItem.Click += (sender, e) => { _notepad.Selection.ReadOnly = true; };

        _setSelectedAsWritableMenuItem.Text = Properties.Resources.MenuSetSelectedAsWritableText;
        _setSelectedAsWritableMenuItem.Click += (sender, e) => { _notepad.Selection.ReadOnly = false; };

        _collapseSelectedBlockMenuItem.Text = Properties.Resources.MenuCollapseSelectedBlockText;
        _collapseSelectedBlockMenuItem.Click += (sender, e) => { _notepad.CollapseBlock(_notepad.Selection.Start.iLine, _notepad.Selection.End.iLine); };

        _increaseIndentMenuItem.Text = Properties.Resources.MenuIncreaseIndentShiftTabText;
        _increaseIndentMenuItem.Click += (sender, e) => { _notepad.IncreaseIndent(); };

        _decreaseIndentMenuItem.Text = Properties.Resources.MenuDecreaseIndentTabText;
        _decreaseIndentMenuItem.Click += (sender, e) => { _notepad.DecreaseIndent(); };

        _commentSelectedLinesMenuItem.Text = Properties.Resources.MenuCommentSelectedLinesText;
        _commentSelectedLinesMenuItem.Click += (sender, e) => { _notepad.InsertLinePrefix(_notepad.CommentPrefix); };

        _uncommentSelectedLinesMenuItem.Text = Properties.Resources.MenuUncommentSelectedLinesText;
        _uncommentSelectedLinesMenuItem.Click += (sender, e) => { _notepad.RemoveLinePrefix(_notepad.CommentPrefix); };

        _goBackwardCtrlMenuItem.Text = Properties.Resources.MenuGoBackwardCtrlText;
        _goBackwardCtrlMenuItem.Click += (sender, e) => { _notepad.NavigateBackward(); };

        _goForwardCtrlShiftMenuItem.Text = Properties.Resources.MenuGoForwardCtrlShiftText;
        _goForwardCtrlShiftMenuItem.Click += (sender, e) => { _notepad.NavigateForward(); };

        _autoIndentMenuItem.Text = Properties.Resources.MenuAutoIndentText;
        _autoIndentMenuItem.Click += (sender, e) => { _notepad.DoAutoIndent(); };

        _textToSpeech.Text = Properties.Resources.MenuTextToSpeechText;
        _textToSpeech.Click += (sender, e) => { ShowTextToSpeechControl(); };

        _print.Text = Properties.Resources.MenuPrintText;
        _print.Click += (sender, e) => { _notepad.Print(new PrintDialogSettings() { ShowPrintPreviewDialog = true }); };

        _startStopMacroRecordingMenuItem.Text = Properties.Resources.MenuStartStopMacroRecordingText;
        _startStopMacroRecordingMenuItem.Click += (sender, e) => { _notepad.MacrosManager.IsRecording = !_notepad.MacrosManager.IsRecording; };

        _executeMacroMenuItem.Text = Properties.Resources.MenuExecuteMacroText;
        _executeMacroMenuItem.Click += (sender, e) => { _notepad.MacrosManager.ExecuteMacros(); };


        _exportMenuItem.Text = Properties.Resources.MenuExportText;
        _exportHTMLMenuItem.Text = Properties.Resources.MenuExportHTMLText;
        _exportHTMLMenuItem.Click += (sender, e) => { ExportToHTML(); };

        _exportRTFMenuItem.Text = Properties.Resources.MenuExportRTFText;
        _exportRTFMenuItem.Click += (sender, e) => { ExportToRTF(); };


        _mainMenu.Items.Insert(1, _editMenuItem);
        _mainMenu.Items.Insert(2, _exportMenuItem);

        _editMenuItem.DropDownItems.AddRange(new ToolStripItem[]
        {
            _findMenuItem,
            _replaceMenuItem,
            new ToolStripSeparator(),
            _setSelectedAsReadonlyMenuItem,
            _setSelectedAsWritableMenuItem,
            new ToolStripSeparator(),
            _collapseSelectedBlockMenuItem,
            new ToolStripSeparator(),
            _increaseIndentMenuItem,
            _decreaseIndentMenuItem,
            new ToolStripSeparator(),
            _commentSelectedLinesMenuItem,
            _uncommentSelectedLinesMenuItem,
            new ToolStripSeparator(),
            _goBackwardCtrlMenuItem,
            _goForwardCtrlShiftMenuItem,
            new ToolStripSeparator(),
            _autoIndentMenuItem,
            new ToolStripSeparator(),
            _print,
            new ToolStripSeparator(),
            _textToSpeech,
            new ToolStripSeparator(),
            _startStopMacroRecordingMenuItem,
            _executeMacroMenuItem
        });

        _exportMenuItem.DropDownItems.AddRange(new ToolStripItem[]
        {
            _exportHTMLMenuItem,
            _exportRTFMenuItem
        });

    }
    #endregion
    #region Export functions

    private void ExportToRTF()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "RTF|*.rtf";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            string rtf = _notepad.Rtf;
            File.WriteAllText(sfd.FileName, rtf);
        }
    }

    private void ExportToHTML()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "HTML with <PRE> tag|*.html|HTML without <PRE> tag|*.html";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            string html = "";

            if (sfd.FilterIndex == 1)
            {
                html = _notepad.Html;
            }
            if (sfd.FilterIndex == 2)
            {

                ExportToHTML exporter = new ExportToHTML();
                exporter.UseBr = true;
                exporter.UseNbsp = false;
                exporter.UseForwardNbsp = true;
                exporter.UseStyleTag = true;
                html = exporter.GetHtml(_notepad);
            }
            File.WriteAllText(sfd.FileName, html);
        }
    }

    #endregion
    #region Hyperlink

    bool CharIsHyperlink(Place place)
    {
        var mask = _notepad.GetStyleIndexMask(new Style[] { _hyperlinkStyle });
        if (place.iChar < _notepad.GetLineLength(place.iLine))
            if ((_notepad[place].style & mask) != 0)
                return true;

        return false;
    }

    private void Notepad_MouseDown(object? sender, MouseEventArgs e)
    {
        var p = _notepad.PointToPlace(e.Location);
        if (CharIsHyperlink(p))
        {
            var url = _notepad.GetRange(p, p).GetFragment(@"[\S]").Text;
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    private void Notepad_MouseMove(object? sender, MouseEventArgs e)
    {
        var p = _notepad.PointToPlace(e.Location);
        if (CharIsHyperlink(p))
            _notepad.Cursor = Cursors.Hand;
        else
            _notepad.Cursor = Cursors.IBeam;
    }

    private void Notepad_TextChangedDelayed(object? sender, TextChangedEventArgs e)
    {
        e.ChangedRange.ClearStyle(_hyperlinkStyle);
        e.ChangedRange.SetStyle(_hyperlinkStyle, @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
    }

    #endregion
    #region Load file
    internal void Load(MyTreeItem treeItem, int linha = 0)
    {
        _file = treeItem.File;

        if (_file == null)
            return;

        _file.Refresh();
        if (!_file.Exists)
            return;

        _titleLabel.Text = treeItem.Name + "\n" + _file.FullName;
        _notepad.Text = File.ReadAllText(_file.FullName);

        if (linha > 0)
            _notepad.SetSelectedLine(linha);

        ShowHints();
    }

    private void ShowHints()
    {
        foreach (var hint in Config.Default.Hints)
            foreach (var range in _notepad.GetRanges(hint, RegexOptions.IgnoreCase))
                _notepad.AddHint(range, $"{range.Text}", scrollToHint: true, inline: true, dock: false);
    }

    #endregion
    #region Save
    Object _lockObject = new Object();
    private void Notepad_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_file == null || !_file.Exists)
            return;

        lock (_lockObject)
        {
            File.WriteAllTextAsync(_file.FullName, _notepad.Text);
        }
    }
    #endregion

    internal void Initialize()
    {
    }

    public void Dispose()
    {
        _synthesizer?.SpeakAsyncCancelAll();
        _mainMenu.Items.Remove(_editMenuItem);
        _mainMenu.Items.Remove(_exportMenuItem);
        _components?.Dispose();
        _textToSpeech?.Dispose();
    }
}
