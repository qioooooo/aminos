using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001D9 RID: 473
	internal class DataGridColumnStyleMappingNameEditor : UITypeEditor
	{
		// Token: 0x0600123F RID: 4671 RVA: 0x0005ABD3 File Offset: 0x00059BD3
		private DataGridColumnStyleMappingNameEditor()
		{
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x0005ABDB File Offset: 0x00059BDB
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0005ABE0 File Offset: 0x00059BE0
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null && context.Instance != null)
			{
				object instance = context.Instance;
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)context.Instance;
				if (dataGridColumnStyle.DataGridTableStyle == null || dataGridColumnStyle.DataGridTableStyle.DataGrid == null)
				{
					return value;
				}
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(dataGridColumnStyle.DataGridTableStyle.DataGrid)["DataSource"];
				if (propertyDescriptor != null)
				{
					object value2 = propertyDescriptor.GetValue(dataGridColumnStyle.DataGridTableStyle.DataGrid);
					if (this.designBindingPicker == null)
					{
						this.designBindingPicker = new DesignBindingPicker();
					}
					DesignBinding designBinding = new DesignBinding(null, (string)value);
					DesignBinding designBinding2 = this.designBindingPicker.Pick(context, provider, false, true, false, value2, string.Empty, designBinding);
					if (value2 != null && designBinding2 != null)
					{
						if (string.IsNullOrEmpty(designBinding2.DataMember) || designBinding2.DataMember == null)
						{
							value = "";
						}
						else
						{
							value = designBinding2.DataField;
						}
					}
				}
			}
			return value;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0005ACC2 File Offset: 0x00059CC2
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x04001110 RID: 4368
		private DesignBindingPicker designBindingPicker;
	}
}
