using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200027E RID: 638
	internal class RadioButtonDesigner : ButtonBaseDesigner
	{
		// Token: 0x060017C4 RID: 6084 RVA: 0x0007BADC File Offset: 0x0007AADC
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["TabStop"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
			{
				propertyDescriptor.SetValue(base.Component, true);
			}
		}
	}
}
