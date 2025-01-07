using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	public class FileNameEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.openFileDialog == null)
					{
						this.openFileDialog = new OpenFileDialog();
						this.InitializeDialog(this.openFileDialog);
					}
					if (value is string)
					{
						this.openFileDialog.FileName = (string)value;
					}
					if (this.openFileDialog.ShowDialog() == DialogResult.OK)
					{
						value = this.openFileDialog.FileName;
					}
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		protected virtual void InitializeDialog(OpenFileDialog openFileDialog)
		{
			openFileDialog.Filter = SR.GetString("GenericFileFilter");
			openFileDialog.Title = SR.GetString("GenericOpenFile");
		}

		private OpenFileDialog openFileDialog;
	}
}
