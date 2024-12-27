using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001D0 RID: 464
	internal class ControlBindingsConverter : TypeConverter
	{
		// Token: 0x060011FD RID: 4605 RVA: 0x0005746D File Offset: 0x0005646D
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00057490 File Offset: 0x00056490
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (value is ControlBindingsCollection)
			{
				ControlBindingsCollection controlBindingsCollection = (ControlBindingsCollection)value;
				IBindableComponent bindableComponent = controlBindingsCollection.BindableComponent;
				bindableComponent.GetType();
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(bindableComponent, null);
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < properties.Count; i++)
				{
					Binding binding = controlBindingsCollection[properties[i].Name];
					bool flag = binding != null && !(binding.DataSource is IListSource) && !(binding.DataSource is IList) && !(binding.DataSource is Array);
					DesignBindingPropertyDescriptor designBindingPropertyDescriptor = new DesignBindingPropertyDescriptor(properties[i], null, flag);
					if (((BindableAttribute)properties[i].Attributes[typeof(BindableAttribute)]).Bindable || !((DesignBinding)designBindingPropertyDescriptor.GetValue(controlBindingsCollection)).IsNull)
					{
						arrayList.Add(designBindingPropertyDescriptor);
					}
				}
				arrayList.Add(new AdvancedBindingPropertyDescriptor());
				PropertyDescriptor[] array = new PropertyDescriptor[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return new PropertyDescriptorCollection(array);
			}
			return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x000575C2 File Offset: 0x000565C2
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
