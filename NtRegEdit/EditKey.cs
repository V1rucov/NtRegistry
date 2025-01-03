using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NtRegEdit
{
	public partial class EditKey : Form
	{
		public EditKey()
		{
			InitializeComponent();
		}

		public string KeyName
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

		private void B_OK_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void B_Cancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void CK_ReplaceNull_CheckedChanged(object sender, EventArgs e)
		{

		}
	}
}
