using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200025D RID: 605
	internal class LinkedDataMemberFieldEditor : UITypeEditor
	{
		// Token: 0x060016FA RID: 5882 RVA: 0x000767CC File Offset: 0x000757CC
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

		// Token: 0x060016FB RID: 5883 RVA: 0x00076851 File Offset: 0x00075851
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x04001311 RID: 4881
		private DesignBindingPicker designBindingPicker;
	}
}
