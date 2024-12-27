using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000232 RID: 562
	public class FileNameEditor : UITypeEditor
	{
		// Token: 0x0600156B RID: 5483 RVA: 0x0006F53C File Offset: 0x0006E53C
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

		// Token: 0x0600156C RID: 5484 RVA: 0x0006F5B9 File Offset: 0x0006E5B9
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0006F5BC File Offset: 0x0006E5BC
		protected virtual void InitializeDialog(OpenFileDialog openFileDialog)
		{
			openFileDialog.Filter = SR.GetString("GenericFileFilter");
			openFileDialog.Title = SR.GetString("GenericOpenFile");
		}

		// Token: 0x04001288 RID: 4744
		private OpenFileDialog openFileDialog;
	}
}
