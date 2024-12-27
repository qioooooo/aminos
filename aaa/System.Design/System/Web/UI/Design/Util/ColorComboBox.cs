using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003C7 RID: 967
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class ColorComboBox : ComboBox
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x000BEE5C File Offset: 0x000BDE5C
		// (set) Token: 0x0600236C RID: 9068 RVA: 0x000BEE88 File Offset: 0x000BDE88
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

		// Token: 0x0600236D RID: 9069 RVA: 0x000BEEED File Offset: 0x000BDEED
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (!base.DesignMode && !base.RecreatingHandle)
			{
				base.Items.Clear();
				base.Items.AddRange(ColorComboBox.COLOR_VALUES);
			}
		}

		// Token: 0x040018A0 RID: 6304
		private static readonly string[] COLOR_VALUES = new string[]
		{
			"Aqua", "Black", "Blue", "Fuchsia", "Gray", "Green", "Lime", "Maroon", "Navy", "Olive",
			"Purple", "Red", "Silver", "Teal", "White", "Yellow"
		};
	}
}
