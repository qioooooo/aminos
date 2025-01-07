using System;
using System.Collections;
using System.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel.Design
{
	internal class InheritedPropertyDescriptor : PropertyDescriptor
	{
		public InheritedPropertyDescriptor(PropertyDescriptor propertyDescriptor, object component, bool rootComponent)
			: base(propertyDescriptor, new Attribute[0])
		{
			this.propertyDescriptor = propertyDescriptor;
			this.InitInheritedDefaultValue(component, rootComponent);
			bool flag = false;
			if (typeof(ICollection).IsAssignableFrom(propertyDescriptor.PropertyType) && propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content))
			{
				ICollection collection = propertyDescriptor.GetValue(component) as ICollection;
				if (collection != null && collection.Count > 0)
				{
					bool flag2 = false;
					bool flag3 = false;
					foreach (MethodInfo methodInfo in TypeDescriptor.GetReflectionType(collection).GetMethods(BindingFlags.Instance | BindingFlags.Public))
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == 1)
						{
							string name = methodInfo.Name;
							Type type = null;
							if (name.Equals("AddRange") && parameters[0].ParameterType.IsArray)
							{
								type = parameters[0].ParameterType.GetElementType();
							}
							else if (name.Equals("Add"))
							{
								type = parameters[0].ParameterType;
							}
							if (type != null)
							{
								if (typeof(IComponent).IsAssignableFrom(type))
								{
									flag2 = true;
									break;
								}
								flag3 = true;
							}
						}
					}
					if (flag3 && !flag2)
					{
						Attribute[] array = (Attribute[])new ArrayList(this.AttributeArray)
						{
							DesignerSerializationVisibilityAttribute.Hidden,
							ReadOnlyAttribute.Yes,
							new EditorAttribute(typeof(UITypeEditor), typeof(UITypeEditor)),
							new TypeConverterAttribute(typeof(InheritedPropertyDescriptor.ReadOnlyCollectionConverter))
						}.ToArray(typeof(Attribute));
						this.AttributeArray = array;
						flag = true;
					}
				}
			}
			if (!flag && this.defaultValue != InheritedPropertyDescriptor.noDefault)
			{
				ArrayList arrayList = new ArrayList(this.AttributeArray);
				arrayList.Add(new DefaultValueAttribute(this.defaultValue));
				Attribute[] array2 = new Attribute[arrayList.Count];
				arrayList.CopyTo(array2, 0);
				this.AttributeArray = array2;
			}
		}

		public override Type ComponentType
		{
			get
			{
				return this.propertyDescriptor.ComponentType;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return this.propertyDescriptor.IsReadOnly || this.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
			}
		}

		internal object OriginalValue
		{
			get
			{
				return this.originalValue;
			}
		}

		internal PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propertyDescriptor;
			}
			set
			{
				this.propertyDescriptor = value;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return this.propertyDescriptor.PropertyType;
			}
		}

		public override bool CanResetValue(object component)
		{
			if (this.defaultValue == InheritedPropertyDescriptor.noDefault)
			{
				return this.propertyDescriptor.CanResetValue(component);
			}
			return !object.Equals(this.GetValue(component), this.defaultValue);
		}

		private object ClonedDefaultValue(object value)
		{
			DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = (DesignerSerializationVisibilityAttribute)this.propertyDescriptor.Attributes[typeof(DesignerSerializationVisibilityAttribute)];
			DesignerSerializationVisibility designerSerializationVisibility;
			if (designerSerializationVisibilityAttribute == null)
			{
				designerSerializationVisibility = DesignerSerializationVisibility.Visible;
			}
			else
			{
				designerSerializationVisibility = designerSerializationVisibilityAttribute.Visibility;
			}
			if (value != null && designerSerializationVisibility == DesignerSerializationVisibility.Content)
			{
				if (value is ICloneable)
				{
					value = ((ICloneable)value).Clone();
				}
				else
				{
					value = InheritedPropertyDescriptor.noDefault;
				}
			}
			return value;
		}

		protected override void FillAttributes(IList attributeList)
		{
			base.FillAttributes(attributeList);
			foreach (object obj in this.propertyDescriptor.Attributes)
			{
				Attribute attribute = (Attribute)obj;
				attributeList.Add(attribute);
			}
		}

		public override object GetValue(object component)
		{
			return this.propertyDescriptor.GetValue(component);
		}

		private void InitInheritedDefaultValue(object component, bool rootComponent)
		{
			try
			{
				object value;
				if (!this.propertyDescriptor.ShouldSerializeValue(component))
				{
					DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)this.propertyDescriptor.Attributes[typeof(DefaultValueAttribute)];
					if (defaultValueAttribute != null)
					{
						this.defaultValue = defaultValueAttribute.Value;
						value = this.defaultValue;
					}
					else
					{
						this.defaultValue = InheritedPropertyDescriptor.noDefault;
						value = this.propertyDescriptor.GetValue(component);
					}
				}
				else
				{
					this.defaultValue = this.propertyDescriptor.GetValue(component);
					value = this.defaultValue;
					this.defaultValue = this.ClonedDefaultValue(this.defaultValue);
				}
				this.SaveOriginalValue(value);
			}
			catch
			{
				this.defaultValue = InheritedPropertyDescriptor.noDefault;
			}
			this.initShouldSerialize = this.ShouldSerializeValue(component);
		}

		public override void ResetValue(object component)
		{
			if (this.defaultValue == InheritedPropertyDescriptor.noDefault)
			{
				this.propertyDescriptor.ResetValue(component);
				return;
			}
			this.SetValue(component, this.defaultValue);
		}

		private void SaveOriginalValue(object value)
		{
			if (value is ICollection)
			{
				this.originalValue = new object[((ICollection)value).Count];
				((ICollection)value).CopyTo((Array)this.originalValue, 0);
				return;
			}
			this.originalValue = value;
		}

		public override void SetValue(object component, object value)
		{
			this.propertyDescriptor.SetValue(component, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			if (this.IsReadOnly)
			{
				return this.propertyDescriptor.ShouldSerializeValue(component) && this.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content);
			}
			if (this.defaultValue == InheritedPropertyDescriptor.noDefault)
			{
				return this.propertyDescriptor.ShouldSerializeValue(component);
			}
			return !object.Equals(this.GetValue(component), this.defaultValue);
		}

		private PropertyDescriptor propertyDescriptor;

		private object defaultValue;

		private static object noDefault = new object();

		private bool initShouldSerialize;

		private object originalValue;

		private class ReadOnlyCollectionConverter : TypeConverter
		{
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return SR.GetString("InheritanceServiceReadOnlyCollection");
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
