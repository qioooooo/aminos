using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XmlFileEditor : UITypeEditor
	{
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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		internal FileDialog fileDialog;
	}
}
