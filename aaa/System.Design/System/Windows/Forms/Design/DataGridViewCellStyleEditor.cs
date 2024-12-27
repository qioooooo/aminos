using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001E2 RID: 482
	internal class DataGridViewCellStyleEditor : UITypeEditor
	{
		// Token: 0x06001284 RID: 4740 RVA: 0x0005D790 File Offset: 0x0005C790
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			this.value = value;
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				IUIService iuiservice = (IUIService)provider.GetService(typeof(IUIService));
				IComponent component = context.Instance as IComponent;
				if (windowsFormsEditorService != null)
				{
					if (this.builderDialog == null)
					{
						this.builderDialog = new DataGridViewCellStyleBuilder(provider, component);
					}
					if (iuiservice != null)
					{
						this.builderDialog.Font = (Font)iuiservice.Styles["DialogFont"];
					}
					DataGridViewCellStyle dataGridViewCellStyle = value as DataGridViewCellStyle;
					if (dataGridViewCellStyle != null)
					{
						this.builderDialog.CellStyle = dataGridViewCellStyle;
					}
					this.builderDialog.Context = context;
					if (this.builderDialog.ShowDialog() == DialogResult.OK)
					{
						this.value = this.builderDialog.CellStyle;
					}
				}
			}
			value = this.value;
			this.value = null;
			return value;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0005D86E File Offset: 0x0005C86E
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x04001150 RID: 4432
		private DataGridViewCellStyleBuilder builderDialog;

		// Token: 0x04001151 RID: 4433
		private object value;
	}
}
