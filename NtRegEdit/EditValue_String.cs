using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NTRegistry;

namespace NtRegEdit
{
	public partial class EditValue_String : Form
	{
		public EditValue_String()
		{
			InitializeComponent();
		}

		public string ValueName
		{
			get
			{
				return T_Name.Text;
			}
			set
			{
				T_Name.Text = value;
			}
		}

		public string ValueData
		{
			get
			{
				return T_Value.Text;
			}
			set
			{
				T_Value.Text = value;
			}
		}

		private void B_OK_Click(object sender, EventArgs e)
		{
			if (T_Name.Text.Trim() == "")
			{
				MessageBox.Show(this, "Please enter a valid name!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (T_Value.Text.Trim() == "")
			{
				MessageBox.Show(this, "Please enter a value!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void B_Cancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
	}
}
