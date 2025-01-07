using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class ColorComboBox : ComboBox
	{
		public string Color
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex != -1)
				{
					return ColorComboBox.COLOR_VALUES[selectedIndex];
				}
				return this.Text.Trim();
			}
			set
			{
				this.SelectedIndex = -1;
				this.Text = string.Empty;
				if (value == null)
				{
					return;
				}
				string text = value.Trim();
				if (text.Length != 0)
				{
					for (int i = 0; i < ColorComboBox.COLOR_VALUES.Length; i++)
					{
						if (string.Compare(ColorComboBox.COLOR_VALUES[i], text, StringComparison.OrdinalIgnoreCase) == 0)
						{
							text = ColorComboBox.COLOR_VALUES[i];
							break;
						}
					}
					this.Text = text;
				}
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (!base.DesignMode && !base.RecreatingHandle)
			{
				base.Items.Clear();
				base.Items.AddRange(ColorComboBox.COLOR_VALUES);
			}
		}

		private static readonly string[] COLOR_VALUES = new string[]
		{
			"Aqua", "Black", "Blue", "Fuchsia", "Gray", "Green", "Lime", "Maroon", "Navy", "Olive",
			"Purple", "Red", "Silver", "Teal", "White", "Yellow"
		};
	}
}
