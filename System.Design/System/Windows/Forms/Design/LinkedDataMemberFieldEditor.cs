using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class LinkedDataMemberFieldEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null && context.Instance != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(context.Instance)["LinkedDataSource"];
				if (propertyDescriptor != null)
				{
					object value2 = propertyDescriptor.GetValue(context.Instance);
					if (value2 != null)
					{
						if (this.designBindingPicker == null)
						{
							this.designBindingPicker = new DesignBindingPicker();
						}
						DesignBinding designBinding = new DesignBinding(null, (string)value);
						DesignBinding designBinding2 = this.designBindingPicker.Pick(context, provider, false, true, false, value2, string.Empty, designBinding);
						if (designBinding2 != null)
						{
							value = designBinding2.DataMember;
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
