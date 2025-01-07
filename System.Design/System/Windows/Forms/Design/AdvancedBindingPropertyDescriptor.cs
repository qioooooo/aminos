using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class AdvancedBindingPropertyDescriptor : PropertyDescriptor
	{
		internal AdvancedBindingPropertyDescriptor()
			: base(SR.GetString("AdvancedBindingPropertyDescName"), null)
		{
		}

		public override Type ComponentType
		{
			get
			{
				return typeof(ControlBindingsCollection);
			}
		}

		public override AttributeCollection Attributes
		{
			get
			{
				return new AttributeCollection(new Attribute[]
				{
					new SRDescriptionAttribute("AdvancedBindingPropertyDescriptorDesc"),
					NotifyParentPropertyAttribute.Yes,
					new MergablePropertyAttribute(false)
				});
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		public override TypeConverter Converter
		{
			get
			{
				if (AdvancedBindingPropertyDescriptor.advancedBindingTypeConverter == null)
				{
					AdvancedBindingPropertyDescriptor.advancedBindingTypeConverter = new AdvancedBindingPropertyDescriptor.AdvancedBindingTypeConverter();
				}
				return AdvancedBindingPropertyDescriptor.advancedBindingTypeConverter;
			}
		}

		public override object GetEditor(Type type)
		{
			if (type == typeof(UITypeEditor))
			{
				return AdvancedBindingPropertyDescriptor.advancedBindingEditor;
			}
			return base.GetEditor(type);
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		protected override void FillAttributes(IList attributeList)
		{
			attributeList.Add(RefreshPropertiesAttribute.All);
			base.FillAttributes(attributeList);
		}

		public override object GetValue(object component)
		{
			return component;
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		internal static AdvancedBindingEditor advancedBindingEditor = new AdvancedBindingEditor();

		internal static AdvancedBindingPropertyDescriptor.AdvancedBindingTypeConverter advancedBindingTypeConverter = new AdvancedBindingPropertyDescriptor.AdvancedBindingTypeConverter();

		internal class AdvancedBindingTypeConverter : TypeConverter
		{
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return string.Empty;
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
