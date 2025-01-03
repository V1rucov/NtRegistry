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
	public partial class EditValue_DWord : Form
	{
		public EditValue_DWord()
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

		public uint ValueData
		{
			get
			{
				if (!CK_Hex.Checked)
					return Convert.ToUInt32(T_Value.Text.Trim(), 10);
				else
					return Convert.ToUInt32(T_Value.Text.Trim(), 16);
			}
			set
			{
				T_Value.Text = value.ToString();
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

			try
			{
				if (!CK_Hex.Checked)
					Convert.ToUInt32(T_Value.Text.Trim(), 10);
				else
					Convert.ToUInt32(T_Value.Text.Trim(), 16);
			}
			catch (FormatException)
			{
				MessageBox.Show(this, "Please enter a valid integer!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void B_Cancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void CK_Hex_CheckedChanged(object sender, EventArgs e)
		{
			// Note that this is after it has been checked / unchecked
			var hex = !CK_Hex.Checked;

			uint value = 0;

			try
			{
				if (!hex)
					value = Convert.ToUInt32(T_Value.Text.Trim(), 10);
				else
					value = Convert.ToUInt32(T_Value.Text.Trim(), 16);
			}
			catch (FormatException)
			{
				MessageBox.Show(this, "Please enter a valid integer!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				
				// Uncheck & focus
				T_Value.Focus();
				T_Value.SelectAll();


				CK_Hex.Checked = !CK_Hex.Checked;
			}

			// Convert
			if (CK_Hex.Checked)
				T_Value.Text = String.Format("{0:X}", value);
			else
				T_Value.Text = String.Format("{0}", value);
		}
	}
}
