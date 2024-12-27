using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001DB RID: 475
	internal class DataGridTableStyleMappingNameEditor : UITypeEditor
	{
		// Token: 0x06001249 RID: 4681 RVA: 0x0005AF58 File Offset: 0x00059F58
		private DataGridTableStyleMappingNameEditor()
		{
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x0005AF60 File Offset: 0x00059F60
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0005AF64 File Offset: 0x00059F64
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null && context.Instance != null)
			{
				object instance = context.Instance;
				DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)context.Instance;
				if (dataGridTableStyle.DataGrid == null)
				{
					return value;
				}
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(dataGridTableStyle.DataGrid)["DataSource"];
				if (propertyDescriptor != null)
				{
					object value2 = propertyDescriptor.GetValue(dataGridTableStyle.DataGrid);
					if (this.designBindingPicker == null)
					{
						this.designBindingPicker = new DesignBindingPicker();
					}
					DesignBinding designBinding = new DesignBinding(value2, (string)value);
					DesignBinding designBinding2 = this.designBindingPicker.Pick(context, provider, false, true, true, value2, string.Empty, designBinding);
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

		// Token: 0x0600124C RID: 4684 RVA: 0x0005B02F File Offset: 0x0005A02F
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x04001113 RID: 4371
		private DesignBindingPicker designBindingPicker;
	}
}
