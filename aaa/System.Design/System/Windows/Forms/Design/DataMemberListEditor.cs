using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001F6 RID: 502
	internal class DataMemberListEditor : UITypeEditor
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x000625FC File Offset: 0x000615FC
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00062600 File Offset: 0x00061600
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
					DesignBinding designBinding2 = this.designBindingPicker.Pick(context, provider, false, true, true, value2, string.Empty, designBinding);
					if (value2 != null && designBinding2 != null)
					{
						value = designBinding2.DataMember;
					}
				}
			}
			return value;
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00062685 File Offset: 0x00061685
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x0400119B RID: 4507
		private DesignBindingPicker designBindingPicker;
	}
}
