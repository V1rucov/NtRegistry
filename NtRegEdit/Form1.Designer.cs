namespace NtRegEdit
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.LV_Values = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.IL = new System.Windows.Forms.ImageList(this.components);
			this.TV_Keys = new System.Windows.Forms.TreeView();
			this.status_strip = new System.Windows.Forms.StatusStrip();
			this.TSLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ts_exit = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MI_About = new System.Windows.Forms.ToolStripMenuItem();
			this.CTX_Key = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MI_New = new System.Windows.Forms.ToolStripMenuItem();
			this.MI_DeleteKey = new System.Windows.Forms.ToolStripMenuItem();
			this.newValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MI_DWORD = new System.Windows.Forms.ToolStripMenuItem();
			this.MI_String = new System.Windows.Forms.ToolStripMenuItem();
			this.CTX_Value = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MI_EditValue = new System.Windows.Forms.ToolStripMenuItem();
			this.MI_DeleteValue = new System.Windows.Forms.ToolStripMenuItem();
			this.status_strip.SuspendLayout();
			this.panel1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.CTX_Key.SuspendLayout();
			this.CTX_Value.SuspendLayout();
			this.SuspendLayout();
			// 
			// LV_Values
			// 
			this.LV_Values.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.LV_Values.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LV_Values.HideSelection = false;
			this.LV_Values.Location = new System.Drawing.Point(237, 24);
			this.LV_Values.Name = "LV_Values";
			this.LV_Values.Size = new System.Drawing.Size(662, 629);
			this.LV_Values.SmallImageList = this.IL;
			this.LV_Values.TabIndex = 0;
			this.LV_Values.UseCompatibleStateImageBehavior = false;
			this.LV_Values.View = System.Windows.Forms.View.Details;
			this.LV_Values.SelectedIndexChanged += new System.EventHandler(this.LV_Values_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 169;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			this.columnHeader2.Width = 90;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Data";
			this.columnHeader3.Width = 370;
			// 
			// IL
			// 
			this.IL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL.ImageStream")));
			this.IL.TransparentColor = System.Drawing.Color.Transparent;
			this.IL.Images.SetKeyName(0, "folder");
			this.IL.Images.SetKeyName(1, "binary");
			this.IL.Images.SetKeyName(2, "text");
			this.IL.Images.SetKeyName(3, "folder_open");
			// 
			// TV_Keys
			// 
			this.TV_Keys.Dock = System.Windows.Forms.DockStyle.Left;
			this.TV_Keys.HideSelection = false;
			this.TV_Keys.ImageIndex = 0;
			this.TV_Keys.ImageList = this.IL;
			this.TV_Keys.Location = new System.Drawing.Point(0, 24);
			this.TV_Keys.Name = "TV_Keys";
			this.TV_Keys.SelectedImageIndex = 0;
			this.TV_Keys.Size = new System.Drawing.Size(233, 629);
			this.TV_Keys.TabIndex = 1;
			this.TV_Keys.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TV_Keys_AfterCollapse);
			this.TV_Keys.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TV_Keys_BeforeExpand);
			this.TV_Keys.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TV_Keys_AfterExpand);
			this.TV_Keys.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TV_Keys_AfterSelect);
			// 
			// status_strip
			// 
			this.status_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSLabel});
			this.status_strip.Location = new System.Drawing.Point(0, 653);
			this.status_strip.Name = "status_strip";
			this.status_strip.Size = new System.Drawing.Size(899, 22);
			this.status_strip.TabIndex = 2;
			this.status_strip.Text = "statusStrip1";
			// 
			// TSLabel
			// 
			this.TSLabel.Name = "TSLabel";
			this.TSLabel.Size = new System.Drawing.Size(39, 17);
			this.TSLabel.Text = "Ready";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.LV_Values);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.TV_Keys);
			this.panel1.Controls.Add(this.menuStrip1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(899, 653);
			this.panel1.TabIndex = 3;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(233, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(4, 629);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(899, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts_exit});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// ts_exit
			// 
			this.ts_exit.Name = "ts_exit";
			this.ts_exit.Size = new System.Drawing.Size(92, 22);
			this.ts_exit.Text = "E&xit";
			this.ts_exit.Click += new System.EventHandler(this.ts_exit_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_About});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// MI_About
			// 
			this.MI_About.Name = "MI_About";
			this.MI_About.Size = new System.Drawing.Size(152, 22);
			this.MI_About.Text = "About";
			this.MI_About.Click += new System.EventHandler(this.MI_About_Click);
			// 
			// CTX_Key
			// 
			this.CTX_Key.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_New,
            this.MI_DeleteKey,
            this.newValueToolStripMenuItem});
			this.CTX_Key.Name = "CTX_Key";
			this.CTX_Key.Size = new System.Drawing.Size(133, 70);
			// 
			// MI_New
			// 
			this.MI_New.Name = "MI_New";
			this.MI_New.Size = new System.Drawing.Size(132, 22);
			this.MI_New.Text = "New Key ...";
			this.MI_New.Click += new System.EventHandler(this.MI_NewKey_Click);
			// 
			// MI_DeleteKey
			// 
			this.MI_DeleteKey.Name = "MI_DeleteKey";
			this.MI_DeleteKey.Size = new System.Drawing.Size(132, 22);
			this.MI_DeleteKey.Text = "Delete...";
			this.MI_DeleteKey.Click += new System.EventHandler(this.MI_DeleteKey_Click);
			// 
			// newValueToolStripMenuItem
			// 
			this.newValueToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_DWORD,
            this.MI_String});
			this.newValueToolStripMenuItem.Name = "newValueToolStripMenuItem";
			this.newValueToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
			this.newValueToolStripMenuItem.Text = "New Value";
			// 
			// MI_DWORD
			// 
			this.MI_DWORD.Name = "MI_DWORD";
			this.MI_DWORD.Size = new System.Drawing.Size(117, 22);
			this.MI_DWORD.Text = "DWORD";
			this.MI_DWORD.Click += new System.EventHandler(this.MI_DWORD_Click);
			// 
			// MI_String
			// 
			this.MI_String.Name = "MI_String";
			this.MI_String.Size = new System.Drawing.Size(117, 22);
			this.MI_String.Text = "String";
			this.MI_String.Click += new System.EventHandler(this.MI_String_Click);
			// 
			// CTX_Value
			// 
			this.CTX_Value.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_EditValue,
            this.MI_DeleteValue});
			this.CTX_Value.Name = "CTX_Value";
			this.CTX_Value.Size = new System.Drawing.Size(117, 48);
			// 
			// MI_EditValue
			// 
			this.MI_EditValue.Name = "MI_EditValue";
			this.MI_EditValue.Size = new System.Drawing.Size(116, 22);
			this.MI_EditValue.Text = "Edit...";
			this.MI_EditValue.Click += new System.EventHandler(this.MI_EditValue_Click);
			// 
			// MI_DeleteValue
			// 
			this.MI_DeleteValue.Name = "MI_DeleteValue";
			this.MI_DeleteValue.Size = new System.Drawing.Size(116, 22);
			this.MI_DeleteValue.Text = "Delete...";
			this.MI_DeleteValue.Click += new System.EventHandler(this.MI_DeleteValue_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(899, 675);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.status_strip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "NT Registry Editor";
			this.status_strip.ResumeLayout(false);
			this.status_strip.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.CTX_Key.ResumeLayout(false);
			this.CTX_Value.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView LV_Values;
		private System.Windows.Forms.TreeView TV_Keys;
		private System.Windows.Forms.StatusStrip status_strip;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ts_exit;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MI_About;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ContextMenuStrip CTX_Key;
		private System.Windows.Forms.ToolStripMenuItem MI_New;
		private System.Windows.Forms.ToolStripMenuItem MI_DeleteKey;
		private System.Windows.Forms.ToolStripMenuItem newValueToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MI_DWORD;
		private System.Windows.Forms.ToolStripMenuItem MI_String;
		private System.Windows.Forms.ContextMenuStrip CTX_Value;
		private System.Windows.Forms.ToolStripMenuItem MI_EditValue;
		private System.Windows.Forms.ToolStripMenuItem MI_DeleteValue;
		private System.Windows.Forms.ImageList IL;
		private System.Windows.Forms.ToolStripStatusLabel TSLabel;
	}
}

