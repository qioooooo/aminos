using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class DesignBindingPropertyDescriptor : PropertyDescriptor
	{
		internal DesignBindingPropertyDescriptor(PropertyDescriptor property, Attribute[] attrs, bool readOnly)
			: base(property.Name, attrs)
		{
			this.property = property;
			this.readOnly = readOnly;
			if (base.AttributeArray != null && base.AttributeArray.Length > 0)
			{
				Attribute[] array = new Attribute[this.AttributeArray.Length + 2];
				this.AttributeArray.CopyTo(array, 0);
				array[this.AttributeArray.Length - 1] = NotifyParentPropertyAttribute.Yes;
				array[this.AttributeArray.Length] = RefreshPropertiesAttribute.Repaint;
				base.AttributeArray = array;
				return;
			}
			base.AttributeArray = new Attribute[]
			{
				NotifyParentPropertyAttribute.Yes,
				RefreshPropertiesAttribute.Repaint
			};
		}

		public override Type ComponentType
		{
			get
			{
				return typeof(ControlBindingsCollection);
			}
		}

		public override TypeConverter Converter
		{
			get
			{
				return DesignBindingPropertyDescriptor.designBindingConverter;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return typeof(DesignBinding);
			}
		}

		public override bool CanResetValue(object component)
		{
			return !DesignBindingPropertyDescriptor.GetBinding((ControlBindingsCollection)component, this.property).IsNull;
		}

		public override object GetValue(object component)
		{
			return DesignBindingPropertyDescriptor.GetBinding((ControlBindingsCollection)component, this.property);
		}

		public override void ResetValue(object component)
		{
			DesignBindingPropertyDescriptor.SetBinding((ControlBindingsCollection)component, this.property, DesignBinding.Null);
		}

		public override void SetValue(object component, object value)
		{
			DesignBindingPropertyDescriptor.SetBinding((ControlBindingsCollection)component, this.property, (DesignBinding)value);
			this.OnValueChanged(component, EventArgs.Empty);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		private static void SetBinding(ControlBindingsCollection bindings, PropertyDescriptor property, DesignBinding designBinding)
		{
			if (designBinding == null)
			{
				return;
			}
			Binding binding = bindings[property.Name];
			if (binding != null)
			{
				bindings.Remove(binding);
			}
			if (!designBinding.IsNull)
			{
				bindings.Add(property.Name, designBinding.DataSource, designBinding.DataMember);
			}
		}

		private static DesignBinding GetBinding(ControlBindingsCollection bindings, PropertyDescriptor property)
		{
			Binding binding = bindings[property.Name];
			if (binding == null)
			{
				return DesignBinding.Null;
			}
			return new DesignBinding(binding.DataSource, binding.BindingMemberInfo.BindingMember);
		}

		private static TypeConverter designBindingConverter = new DesignBindingConverter();

		private PropertyDescriptor property;

		private bool readOnly;
	}
}
