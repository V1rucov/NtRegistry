namespace NtRegEdit
{
	partial class EditKey
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
			this.T_Name = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.B_OK = new System.Windows.Forms.Button();
			this.B_Cancel = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// T_Name
			// 
			this.T_Name.Location = new System.Drawing.Point(53, 9);
			this.T_Name.Name = "T_Name";
			this.T_Name.Size = new System.Drawing.Size(273, 20);
			this.T_Name.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name:";
			// 
			// B_OK
			// 
			this.B_OK.Location = new System.Drawing.Point(91, 57);
			this.B_OK.Name = "B_OK";
			this.B_OK.Size = new System.Drawing.Size(75, 23);
			this.B_OK.TabIndex = 2;
			this.B_OK.Text = "OK";
			this.B_OK.UseVisualStyleBackColor = true;
			this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
			// 
			// B_Cancel
			// 
			this.B_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.B_Cancel.Location = new System.Drawing.Point(172, 57);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Size = new System.Drawing.Size(75, 23);
			this.B_Cancel.TabIndex = 3;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.UseVisualStyleBackColor = true;
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(240, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "All \"\\\" (backslash) will be replaced with \"\\0\" (null)";
			// 
			// EditKey
			// 
			this.AcceptButton = this.B_OK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.B_Cancel;
			this.ClientSize = new System.Drawing.Size(334, 85);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.B_Cancel);
			this.Controls.Add(this.B_OK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.T_Name);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "EditKey";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Key";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox T_Name;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button B_OK;
		private System.Windows.Forms.Button B_Cancel;
		private System.Windows.Forms.Label label2;
	}
}