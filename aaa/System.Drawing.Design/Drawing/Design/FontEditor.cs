using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Drawing.Design
{
	// Token: 0x02000017 RID: 23
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class FontEditor : UITypeEditor
	{
		// Token: 0x0600009A RID: 154 RVA: 0x000055E8 File Offset: 0x000045E8
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			this.value = value;
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.fontDialog == null)
					{
						this.fontDialog = new FontDialog();
						this.fontDialog.ShowApply = false;
						this.fontDialog.ShowColor = false;
						this.fontDialog.AllowVerticalFonts = false;
					}
					Font font = value as Font;
					if (font != null)
					{
						this.fontDialog.Font = font;
					}
					IntPtr focus = UnsafeNativeMethods.GetFocus();
					try
					{
						if (this.fontDialog.ShowDialog() == DialogResult.OK)
						{
							this.value = this.fontDialog.Font;
						}
					}
					finally
					{
						if (focus != IntPtr.Zero)
						{
							UnsafeNativeMethods.SetFocus(new HandleRef(null, focus));
						}
					}
				}
			}
			value = this.value;
			this.value = null;
			return value;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000056D0 File Offset: 0x000046D0
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x04000083 RID: 131
		private FontDialog fontDialog;

		// Token: 0x04000084 RID: 132
		private object value;
	}
}
