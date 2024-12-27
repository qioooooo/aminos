using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000246 RID: 582
	internal class FormatStringEditor : UITypeEditor
	{
		// Token: 0x0600161F RID: 5663 RVA: 0x0007344C File Offset: 0x0007244C
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					DataGridViewCellStyle dataGridViewCellStyle = context.Instance as DataGridViewCellStyle;
					ListControl listControl = context.Instance as ListControl;
					if (this.formatStringDialog == null)
					{
						this.formatStringDialog = new FormatStringDialog(context);
					}
					if (listControl != null)
					{
						this.formatStringDialog.ListControl = listControl;
					}
					else
					{
						this.formatStringDialog.DataGridViewCellStyle = dataGridViewCellStyle;
					}
					IComponentChangeService componentChangeService = (IComponentChangeService)provider.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						if (dataGridViewCellStyle != null)
						{
							componentChangeService.OnComponentChanging(dataGridViewCellStyle, TypeDescriptor.GetProperties(dataGridViewCellStyle)["Format"]);
							componentChangeService.OnComponentChanging(dataGridViewCellStyle, TypeDescriptor.GetProperties(dataGridViewCellStyle)["NullValue"]);
							componentChangeService.OnComponentChanging(dataGridViewCellStyle, TypeDescriptor.GetProperties(dataGridViewCellStyle)["FormatProvider"]);
						}
						else
						{
							componentChangeService.OnComponentChanging(listControl, TypeDescriptor.GetProperties(listControl)["FormatString"]);
							componentChangeService.OnComponentChanging(listControl, TypeDescriptor.GetProperties(listControl)["FormatInfo"]);
						}
					}
					windowsFormsEditorService.ShowDialog(this.formatStringDialog);
					this.formatStringDialog.End();
					if (this.formatStringDialog.Dirty)
					{
						TypeDescriptor.Refresh(context.Instance);
						if (componentChangeService != null)
						{
							if (dataGridViewCellStyle != null)
							{
								componentChangeService.OnComponentChanged(dataGridViewCellStyle, TypeDescriptor.GetProperties(dataGridViewCellStyle)["Format"], null, null);
								componentChangeService.OnComponentChanged(dataGridViewCellStyle, TypeDescriptor.GetProperties(dataGridViewCellStyle)["NullValue"], null, null);
								componentChangeService.OnComponentChanged(dataGridViewCellStyle, TypeDescriptor.GetProperties(dataGridViewCellStyle)["FormatProvider"], null, null);
							}
							else
							{
								componentChangeService.OnComponentChanged(listControl, TypeDescriptor.GetProperties(listControl)["FormatString"], null, null);
								componentChangeService.OnComponentChanged(listControl, TypeDescriptor.GetProperties(listControl)["FormatInfo"], null, null);
							}
						}
					}
				}
			}
			return value;
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00073611 File Offset: 0x00072611
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x040012DE RID: 4830
		private FormatStringDialog formatStringDialog;
	}
}
