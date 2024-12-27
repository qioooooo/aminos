using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	// Token: 0x020003C1 RID: 961
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XmlFileEditor : UITypeEditor
	{
		// Token: 0x06002354 RID: 9044 RVA: 0x000BEBF8 File Offset: 0x000BDBF8
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.fileDialog == null)
					{
						this.fileDialog = new OpenFileDialog();
						this.fileDialog.Title = SR.GetString("XMLFilePicker_Caption");
						this.fileDialog.Filter = SR.GetString("XMLFilePicker_Filter");
					}
					if (value != null)
					{
						this.fileDialog.FileName = value.ToString();
					}
					if (this.fileDialog.ShowDialog() == DialogResult.OK)
					{
						value = this.fileDialog.FileName;
					}
				}
			}
			return value;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000BEC91 File Offset: 0x000BDC91
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x0400189D RID: 6301
		internal FileDialog fileDialog;
	}
}
