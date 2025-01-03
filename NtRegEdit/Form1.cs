using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using NTRegistry;

namespace NtRegEdit
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			string[] root_names = { "HKEY_CLASSES_ROOT", "HKEY_LOCAL_MACHINE", "HKEY_USERS", "HKEY_CURRENT_USER", "HKEY_CURRENT_CONFIG" };
			NtRegistryHive[] root_keys = { NtRegistryHive.ClassesRoot, NtRegistryHive.LocalMachine, NtRegistryHive.Users, NtRegistryHive.CurrentUser, NtRegistryHive.CurrentConfig };


			for (int i = 0; i < root_names.Length; i++)
			{
				try
				{
					var root_name = root_names[i];
					var root_key = root_keys[i];

					var node = new TreeNode(root_name) { Tag = NtRegistryKey.OpenBaseKey(root_key, true), ImageKey = "folder" };

					TV_Keys.Nodes.Add(node);
					RefreshNode_SubKeys(node);
				}
				catch (Exception)
				{
				}
			}
		}

		private void ts_exit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void TV_Keys_AfterSelect(object sender, TreeViewEventArgs e)
		{
			try
			{
				if (e.Node == null)
				{
					TV_Keys.ContextMenuStrip = null;
					return;
				}

				NtRegistryKey key = e.Node.Tag as NtRegistryKey;

				//
				RefreshNode_Values(e.Node);

				// Update Context Menu
				TV_Keys.ContextMenuStrip = CTX_Key;
				MI_DeleteKey.Enabled = !key.IsRoot;

				// Path
				TSLabel.Text = key.Name;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void TV_Keys_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			try
			{
				// Actually load the subkeys
				RefreshNode_SubKeys(e.Node, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		private void TV_Keys_AfterExpand(object sender, TreeViewEventArgs e)
		{
			try
			{
				// e.Node.ImageKey = "folder_open";

				TV_Keys.Invalidate();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void TV_Keys_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			try
			{
				// e.Node.ImageKey = "folder";

				TV_Keys.Invalidate();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private string DataToString(object data, NtRegistryValueKind kind)
		{
			switch (kind)
			{
				case NtRegistryValueKind.Binary:
					return data.ToString();

				case NtRegistryValueKind.DWord:
					return String.Format("0x{0:X8} ({0})", data);

				case NtRegistryValueKind.QWord:
					return String.Format("0x{0:X16} ({0})", data);

				case NtRegistryValueKind.String:
				case NtRegistryValueKind.ExpandedString:
					return data.ToString();

				case NtRegistryValueKind.MultiString:
					var strings = (string[])data;
					return String.Join(" | ", strings);

				default:
					return "(???)";
			}
		}

		private string DataIcon(NtRegistryValueKind kind)
		{
			switch (kind)
			{
				case NtRegistryValueKind.Binary:
				case NtRegistryValueKind.DWord:
				case NtRegistryValueKind.QWord:
					return "binary";

				case NtRegistryValueKind.String:
				case NtRegistryValueKind.ExpandedString:
				case NtRegistryValueKind.MultiString:
					return "text";

				default:
					return null;
			}
		}

		private void RefreshNode_SubKeys(TreeNode node, bool forced = false)
		{
			// Load the subkeys for this node

			NtRegistryKey key = node.Tag as NtRegistryKey;

			if (key == null)
			{
				// Needs to open the key / assign tag...
				var parentNode = node.Parent;
				var parentKey = parentNode.Tag as NtRegistryKey;
				key = parentKey.OpenSubKey(node.Text.Replace("\\","\0"));
				node.Tag = key;
			}

			// Add dummy node if it is not expanded
			if (!node.IsExpanded && !forced)
			{
				if (key.SubKeyCount > 0)
					node.Nodes.Add(new TreeNode("----------------------- dummy -----------"));

				return;
			}

			// Matching existing nodes with refreshed subkeys
			var subKeyNames = key.GetSubKeyNames();
			var subNodes = new List<TreeNode>();

			foreach (TreeNode subNode in node.Nodes)
				subNodes.Add(subNode);

			var subNodes_deleted = subNodes.Where(x => !subKeyNames.Contains(x.Text.Replace("\\","\0"))).ToArray();

			TreeNode prevSubNode = null;
			foreach (var subKeyName in subKeyNames)
			{
				var subNode = subNodes.Where(x => x.Text == subKeyName.Replace("\0", "\\")).FirstOrDefault();

				if (subNode == null)
				{
					subNode = new TreeNode(subKeyName.Replace("\0", "\\")) { ImageKey = "folder" };
					node.Nodes.Insert(prevSubNode != null ? prevSubNode.Index + 1 : 0, subNode);
				}

				bool refreshed = true;
				try
				{
					RefreshNode_SubKeys(subNode);
				}
				catch (Exception)
				{
					refreshed = false;
					// Failed to access the node... just quietly remove it
					subNode.Remove();
				}

				if (refreshed)
					prevSubNode = subNode;
			}

			foreach (var subNode in subNodes_deleted)
				subNode.Remove();

		}

		private void RefreshNode_Values(TreeNode node)
		{
			var key = node.Tag as NtRegistryKey;

			// Load values
			var valueNames = key.GetValueNames();

			LV_Values.Items.Clear();

			// No Default ?
			if (!valueNames.Contains(""))
			{
				LV_Values.Items.Add(new ListViewItem(new string[] { "(Default)", NtRegistryValueKind.String.CodeName(), "(value not set)" }) { ImageKey = "text" });
			}

			// Other values
			foreach (var valueName in valueNames)
			{
				try
				{
					var name = valueName;
					var kind = key.GetValueKind(valueName);
					var data = key.GetValue(valueName);
					var data_text = DataToString(data, kind);
					var icon = DataIcon(kind);

					if (valueName == "")
					{
						// Default
						name = "(Default)";
					}

					var lvi = new ListViewItem(new string[] { name, kind.CodeName(), data_text }) { ImageKey = icon };

					LV_Values.Items.Add(lvi);
				}
				catch (Exception)
				{
					// Just skip this item...
				}
			}
		}

		private void MI_NewKey_Click(object sender, EventArgs e)
		{
			try
			{
				var node = TV_Keys.SelectedNode;
				var key = node.Tag as NtRegistryKey;

				var dlgEdit = new EditKey();

				dlgEdit.KeyName = "(New Key)";

				if (dlgEdit.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return;

				var subKey = key.CreateSubKey(dlgEdit.KeyName.Replace("\\", "\0"));
				subKey.Flush();
				key.Flush();

				RefreshNode_SubKeys(node);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MI_DeleteKey_Click(object sender, EventArgs e)
		{
			try
			{
				var node = TV_Keys.SelectedNode;
				var key = node.Tag as NtRegistryKey;

				if (key.IsRoot)
				{
					MessageBox.Show(this, "This key cannot be deleted", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				if (MessageBox.Show(this, "Deleting registry entries may cause unstabilities to your system. Sure?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
				{
					var parentKey = node.Parent.Tag as NtRegistryKey;
					// parentKey.DeleteSubKey(node.Text);

					parentKey.DeleteSubKeyTree(node.Text.Replace("\\", "\0"));

					// Can't delete directly because of inappropriate KeyAccess
					// key.Delete();

					// Refresh parent
					RefreshNode_SubKeys(node.Parent);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void MI_DWORD_Click(object sender, EventArgs e)
		{
			try
			{
				var dlg = new EditValue_DWord();

				dlg.ValueName = "(New Value)";
				dlg.ValueData = 0;

				if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
					return;

				var node = TV_Keys.SelectedNode;
				var key = node.Tag as NtRegistryKey;

				if (key.HasValue(dlg.ValueName))
				{
					MessageBox.Show(this, "Another value with the same name already exists!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				key.SetValue(dlg.ValueName, dlg.ValueData);
				key.Flush();

				RefreshNode_Values(node);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MI_String_Click(object sender, EventArgs e)
		{
			try
			{
				var dlg = new EditValue_String();

				dlg.ValueName = "(New Value)";
				dlg.ValueData = "";

				if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
					return;

				var node = TV_Keys.SelectedNode;
				var key = node.Tag as NtRegistryKey;

				if (key.HasValue(dlg.ValueName))
				{
					MessageBox.Show(this, "Another value with the same name already exists!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				key.SetValue(dlg.ValueName, dlg.ValueData);
				key.Flush();

				RefreshNode_Values(node);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MI_EditValue_Click(object sender, EventArgs e)
		{
			try
			{
				var node = TV_Keys.SelectedNode;
				var key = node.Tag as NtRegistryKey;

				var lvi = LV_Values.SelectedItems[0];
				var valueName = lvi.Text;

				var valueKind = key.GetValueKind(valueName);
				switch (valueKind)
				{
					case NtRegistryValueKind.DWord:
						var dlgDWORD = new EditValue_DWord();
						dlgDWORD.ValueName = valueName;
						dlgDWORD.ValueData = (uint)key.GetValue(valueName);

						if (dlgDWORD.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
							return;

						key.SetValue(valueName, dlgDWORD.ValueData);
						key.Flush();

						RefreshNode_Values(node);

						break;

					case NtRegistryValueKind.String:
						var dlgString = new EditValue_String();
						dlgString.ValueName = valueName;
						dlgString.ValueData = (string)key.GetValue(valueName);

						if (dlgString.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
							return;

						key.SetValue(valueName, dlgString.ValueData);
						key.Flush();

						RefreshNode_Values(node);

						break;

					default:
						MessageBox.Show(this, "Value of type " + valueKind.CodeName() + " is not supported!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

						break;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MI_DeleteValue_Click(object sender, EventArgs e)
		{
			try
			{
				var node = TV_Keys.SelectedNode;
				var key = node.Tag as NtRegistryKey;

				var lvi = LV_Values.SelectedItems[0];
				var valueName = lvi.Text;

				if (MessageBox.Show(this, "Deleting values in Registry may cause system unstabilities. Sure?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes)
					return;

				key.DeleteValue(valueName);
				key.Flush();

				RefreshNode_Values(node);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LV_Values_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (LV_Values.SelectedItems.Count > 0)
					LV_Values.ContextMenuStrip = CTX_Value;
				else
					LV_Values.ContextMenu = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "An error has occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MI_About_Click(object sender, EventArgs e)
		{
			var dlg = new AboutBox();

			dlg.ShowDialog(this);
		}

	}
}
