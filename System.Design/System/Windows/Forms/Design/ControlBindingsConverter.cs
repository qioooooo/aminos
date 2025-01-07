using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class ControlBindingsConverter : TypeConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

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

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
