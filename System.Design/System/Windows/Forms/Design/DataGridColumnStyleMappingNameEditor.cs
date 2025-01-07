using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DataGridColumnStyleMappingNameEditor : UITypeEditor
	{
		private DataGridColumnStyleMappingNameEditor()
		{
		}

		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private DesignBindingPicker designBindingPicker;
	}
}
