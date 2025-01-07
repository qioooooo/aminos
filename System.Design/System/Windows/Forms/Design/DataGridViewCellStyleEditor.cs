using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewCellStyleEditor : UITypeEditor
	{
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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private DataGridViewCellStyleBuilder builderDialog;

		private object value;
	}
}
