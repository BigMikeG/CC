/*
 * Created by SharpDevelop.
 * User: GZNDQ9
 * Date: 3/23/2012
 * Time: 12:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace CalCompare
{
    partial class MainForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asCommaDelimitedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asTabDelimitedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearTheGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoResizeColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideEmptyColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calSupportToolsWebSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.filterStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.showAllLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.diffCheckBox = new System.Windows.Forms.CheckBox();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.xmlSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.csvSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.importOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.filterTimer = new System.Windows.Forms.Timer(this.components);
            this.hexRadioButton = new System.Windows.Forms.RadioButton();
            this.engRadioButton = new System.Windows.Forms.RadioButton();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.fileToolStripMenuItem,
                                    this.toolsToolStripMenuItem,
                                    this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(749, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.openToolStripMenuItem,
                                    this.toolStripSeparator1,
                                    this.exportToolStripMenuItem,
                                    this.importToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(116, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.asCommaDelimitedToolStripMenuItem,
                                    this.asTabDelimitedToolStripMenuItem,
                                    this.asXMLToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // asCommaDelimitedToolStripMenuItem
            // 
            this.asCommaDelimitedToolStripMenuItem.Name = "asCommaDelimitedToolStripMenuItem";
            this.asCommaDelimitedToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.asCommaDelimitedToolStripMenuItem.Text = "as Comma Delimited (CSV)";
            this.asCommaDelimitedToolStripMenuItem.Click += new System.EventHandler(this.ExportAsCsvClick);
            // 
            // asTabDelimitedToolStripMenuItem
            // 
            this.asTabDelimitedToolStripMenuItem.Name = "asTabDelimitedToolStripMenuItem";
            this.asTabDelimitedToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.asTabDelimitedToolStripMenuItem.Text = "as Tab Delimited";
            this.asTabDelimitedToolStripMenuItem.Click += new System.EventHandler(this.ExportAsTabDelimitedClick);
            // 
            // asXMLToolStripMenuItem
            // 
            this.asXMLToolStripMenuItem.Name = "asXMLToolStripMenuItem";
            this.asXMLToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.asXMLToolStripMenuItem.Text = "as XML";
            this.asXMLToolStripMenuItem.Click += new System.EventHandler(this.ExportAsXmlClick);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.importToolStripMenuItem.Text = "Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.ImportClick);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.clearTheGridToolStripMenuItem,
                                    this.autoResizeColumnsToolStripMenuItem,
                                    this.hideEmptyColumnsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // clearTheGridToolStripMenuItem
            // 
            this.clearTheGridToolStripMenuItem.Name = "clearTheGridToolStripMenuItem";
            this.clearTheGridToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.clearTheGridToolStripMenuItem.Text = "Clear The Grid";
            this.clearTheGridToolStripMenuItem.Click += new System.EventHandler(this.ClearTheGridToolStripMenuItemClick);
            // 
            // autoResizeColumnsToolStripMenuItem
            // 
            this.autoResizeColumnsToolStripMenuItem.Name = "autoResizeColumnsToolStripMenuItem";
            this.autoResizeColumnsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.autoResizeColumnsToolStripMenuItem.Text = "Auto Resize Columns";
            this.autoResizeColumnsToolStripMenuItem.Click += new System.EventHandler(this.AutoResizeColumnsToolStripMenuItemClick);
            // 
            // hideEmptyColumnsToolStripMenuItem
            // 
            this.hideEmptyColumnsToolStripMenuItem.CheckOnClick = true;
            this.hideEmptyColumnsToolStripMenuItem.Name = "hideEmptyColumnsToolStripMenuItem";
            this.hideEmptyColumnsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.hideEmptyColumnsToolStripMenuItem.Text = "Hide Empty Columns";
            this.hideEmptyColumnsToolStripMenuItem.ToolTipText = "Warning: This can take a ridiculously long time!";
            this.hideEmptyColumnsToolStripMenuItem.Click += new System.EventHandler(this.HideEmptyColumnsToolStripMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.usersGuideToolStripMenuItem,
                                    this.calSupportToolsWebSiteToolStripMenuItem,
                                    this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // usersGuideToolStripMenuItem
            // 
            this.usersGuideToolStripMenuItem.Name = "usersGuideToolStripMenuItem";
            this.usersGuideToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.usersGuideToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.usersGuideToolStripMenuItem.Text = "User\'s Guide";
            this.usersGuideToolStripMenuItem.Click += new System.EventHandler(this.UsersGuideToolStripMenuItemClick);
            // 
            // calSupportToolsWebSiteToolStripMenuItem
            // 
            this.calSupportToolsWebSiteToolStripMenuItem.Name = "calSupportToolsWebSiteToolStripMenuItem";
            this.calSupportToolsWebSiteToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.calSupportToolsWebSiteToolStripMenuItem.Text = "Cal Support Tools Site";
            this.calSupportToolsWebSiteToolStripMenuItem.Click += new System.EventHandler(this.CalSupportToolsWebSiteToolStripMenuItemClick);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(749, 299);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView1ColumnHeaderMouseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.toolStripProgressBar1,
                                    this.filterStatusLabel,
                                    this.showAllLabel,
                                    this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 323);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(749, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Step = 1;
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // filterStatusLabel
            // 
            this.filterStatusLabel.Name = "filterStatusLabel";
            this.filterStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.filterStatusLabel.Visible = false;
            // 
            // showAllLabel
            // 
            this.showAllLabel.IsLink = true;
            this.showAllLabel.Name = "showAllLabel";
            this.showAllLabel.Size = new System.Drawing.Size(53, 17);
            this.showAllLabel.Text = "Show &All";
            this.showAllLabel.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(298, 17);
            this.toolStripStatusLabel1.Text = "Click \"File -> Open\" and select calplot files to compare.";
            // 
            // diffCheckBox
            // 
            this.diffCheckBox.Location = new System.Drawing.Point(151, 0);
            this.diffCheckBox.Name = "diffCheckBox";
            this.diffCheckBox.Size = new System.Drawing.Size(44, 24);
            this.diffCheckBox.TabIndex = 5;
            this.diffCheckBox.Text = "Diff";
            this.diffCheckBox.UseVisualStyleBackColor = true;
            this.diffCheckBox.CheckedChanged += new System.EventHandler(this.DiffCheckBoxCheckedChanged);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(201, 2);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(172, 20);
            this.filterTextBox.TabIndex = 6;
            this.filterTextBox.TextChanged += new System.EventHandler(this.FilterTextBoxTextChanged);
            // 
            // xmlSaveFileDialog
            // 
            this.xmlSaveFileDialog.DefaultExt = "xml";
            this.xmlSaveFileDialog.FileName = "CalCompare.xml";
            this.xmlSaveFileDialog.Filter = "XML files|*.xml";
            this.xmlSaveFileDialog.Title = "Export as XML";
            this.xmlSaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.ExportAsXmlFileOk);
            // 
            // csvSaveFileDialog
            // 
            this.csvSaveFileDialog.DefaultExt = "csv";
            this.csvSaveFileDialog.FileName = "CalCompare.csv";
            this.csvSaveFileDialog.Filter = "CSV files|*.csv";
            this.csvSaveFileDialog.Title = "Export as CSV";
            this.csvSaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.ExportAsCsvFileOk);
            // 
            // tabSaveFileDialog
            // 
            this.tabSaveFileDialog.DefaultExt = "txt";
            this.tabSaveFileDialog.FileName = "CalCompare.txt";
            this.tabSaveFileDialog.Filter = "Text files|*.txt";
            this.tabSaveFileDialog.Title = "Export as Tab Delimited";
            this.tabSaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.ExportAsTabFileOk);
            // 
            // importOpenFileDialog
            // 
            this.importOpenFileDialog.AddExtension = false;
            this.importOpenFileDialog.Filter = "CSV files|*.csv|Tab Delimited files|*.txt|XML files|*.xml";
            this.importOpenFileDialog.Title = "Import An Exported Cal Compare File";
            // 
            // filterTimer
            // 
            this.filterTimer.Interval = 1000;
            this.filterTimer.Tick += new System.EventHandler(this.FilterTimerTick);
            // 
            // hexRadioButton
            // 
            this.hexRadioButton.Checked = true;
            this.hexRadioButton.Location = new System.Drawing.Point(379, 0);
            this.hexRadioButton.Name = "hexRadioButton";
            this.hexRadioButton.Size = new System.Drawing.Size(46, 24);
            this.hexRadioButton.TabIndex = 7;
            this.hexRadioButton.TabStop = true;
            this.hexRadioButton.Text = "Hex";
            this.hexRadioButton.UseVisualStyleBackColor = true;
            this.hexRadioButton.CheckedChanged += new System.EventHandler(this.UnitTypeRadioButtonCheckedChanged);
            // 
            // engRadioButton
            // 
            this.engRadioButton.Enabled = false;
            this.engRadioButton.Location = new System.Drawing.Point(432, 0);
            this.engRadioButton.Name = "engRadioButton";
            this.engRadioButton.Size = new System.Drawing.Size(46, 24);
            this.engRadioButton.TabIndex = 8;
            this.engRadioButton.Text = "Eng";
            this.engRadioButton.UseVisualStyleBackColor = true;
            this.engRadioButton.CheckedChanged += new System.EventHandler(this.UnitTypeRadioButtonCheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 345);
            this.Controls.Add(this.engRadioButton);
            this.Controls.Add(this.hexRadioButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.diffCheckBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Cal Compare Pro";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.RadioButton engRadioButton;
        private System.Windows.Forms.RadioButton hexRadioButton;
        private System.Windows.Forms.Timer filterTimer;
        private System.Windows.Forms.ToolStripMenuItem hideEmptyColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoResizeColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearTheGridToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog importOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog tabSaveFileDialog;
        private System.Windows.Forms.SaveFileDialog csvSaveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem asXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asTabDelimitedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asCommaDelimitedToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog xmlSaveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel filterStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel showAllLabel;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.CheckBox diffCheckBox;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calSupportToolsWebSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}
