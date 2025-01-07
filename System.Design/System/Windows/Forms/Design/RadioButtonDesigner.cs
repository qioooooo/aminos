using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class RadioButtonDesigner : ButtonBaseDesigner
	{
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
