using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001F5 RID: 501
	internal class DataMemberFieldEditor : UITypeEditor
	{
		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x00062568 File Offset: 0x00061568
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0006256C File Offset: 0x0006156C
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null && context.Instance != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(context.Instance)["DataSource"];
				if (propertyDescriptor != null)
				{
					object value2 = propertyDescriptor.GetValue(context.Instance);
					if (this.designBindingPicker == null)
					{
						this.designBindingPicker = new DesignBindingPicker();
					}
					DesignBinding designBinding = new DesignBinding(value2, (string)value);
					DesignBinding designBinding2 = this.designBindingPicker.Pick(context, provider, false, true, false, value2, string.Empty, designBinding);
					if (value2 != null && designBinding2 != null)
					{
						value = designBinding2.DataMember;
					}
				}
			}
			return value;
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000625F1 File Offset: 0x000615F1
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x0400119A RID: 4506
		private DesignBindingPicker designBindingPicker;
	}
}
