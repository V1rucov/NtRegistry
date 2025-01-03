namespace NtRegEdit
{
	partial class EditValue_String
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
			this.label1 = new System.Windows.Forms.Label();
			this.T_Name = new System.Windows.Forms.TextBox();
			this.T_Value = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.B_OK = new System.Windows.Forms.Button();
			this.B_Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// T_Name
			// 
			this.T_Name.Location = new System.Drawing.Point(56, 6);
			this.T_Name.Name = "T_Name";
			this.T_Name.Size = new System.Drawing.Size(128, 20);
			this.T_Name.TabIndex = 1;
			// 
			// T_Value
			// 
			this.T_Value.Location = new System.Drawing.Point(56, 31);
			this.T_Value.Name = "T_Value";
			this.T_Value.Size = new System.Drawing.Size(237, 20);
			this.T_Value.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(37, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Value:";
			// 
			// B_OK
			// 
			this.B_OK.Location = new System.Drawing.Point(78, 58);
			this.B_OK.Name = "B_OK";
			this.B_OK.Size = new System.Drawing.Size(75, 23);
			this.B_OK.TabIndex = 6;
			this.B_OK.Text = "OK";
			this.B_OK.UseVisualStyleBackColor = true;
			this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
			// 
			// B_Cancel
			// 
			this.B_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.B_Cancel.Location = new System.Drawing.Point(159, 58);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Size = new System.Drawing.Size(75, 23);
			this.B_Cancel.TabIndex = 7;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.UseVisualStyleBackColor = true;
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// EditValue_String
			// 
			this.AcceptButton = this.B_OK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.B_Cancel;
			this.ClientSize = new System.Drawing.Size(312, 89);
			this.ControlBox = false;
			this.Controls.Add(this.B_Cancel);
			this.Controls.Add(this.B_OK);
			this.Controls.Add(this.T_Value);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.T_Name);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "EditValue_String";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Value";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox T_Name;
		private System.Windows.Forms.TextBox T_Value;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button B_OK;
		private System.Windows.Forms.Button B_Cancel;
	}
}