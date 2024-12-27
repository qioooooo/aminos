using System;
using System.Collections;
using System.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel.Design
{
	// Token: 0x02000130 RID: 304
	internal class InheritedPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000BD8 RID: 3032 RVA: 0x0002E4DC File Offset: 0x0002D4DC
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

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0002E6E9 File Offset: 0x0002D6E9
		public override Type ComponentType
		{
			get
			{
				return this.propertyDescriptor.ComponentType;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0002E6F6 File Offset: 0x0002D6F6
		public override bool IsReadOnly
		{
			get
			{
				return this.propertyDescriptor.IsReadOnly || this.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0002E726 File Offset: 0x0002D726
		internal object OriginalValue
		{
			get
			{
				return this.originalValue;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0002E72E File Offset: 0x0002D72E
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0002E736 File Offset: 0x0002D736
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

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0002E73F File Offset: 0x0002D73F
		public override Type PropertyType
		{
			get
			{
				return this.propertyDescriptor.PropertyType;
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x0002E74C File Offset: 0x0002D74C
		public override bool CanResetValue(object component)
		{
			if (this.defaultValue == InheritedPropertyDescriptor.noDefault)
			{
				return this.propertyDescriptor.CanResetValue(component);
			}
			return !object.Equals(this.GetValue(component), this.defaultValue);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0002E780 File Offset: 0x0002D780
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

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002E7E4 File Offset: 0x0002D7E4
		protected override void FillAttributes(IList attributeList)
		{
			base.FillAttributes(attributeList);
			foreach (object obj in this.propertyDescriptor.Attributes)
			{
				Attribute attribute = (Attribute)obj;
				attributeList.Add(attribute);
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0002E84C File Offset: 0x0002D84C
		public override object GetValue(object component)
		{
			return this.propertyDescriptor.GetValue(component);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002E85C File Offset: 0x0002D85C
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

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0002E928 File Offset: 0x0002D928
		public override void ResetValue(object component)
		{
			if (this.defaultValue == InheritedPropertyDescriptor.noDefault)
			{
				this.propertyDescriptor.ResetValue(component);
				return;
			}
			this.SetValue(component, this.defaultValue);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002E951 File Offset: 0x0002D951
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

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002E990 File Offset: 0x0002D990
		public override void SetValue(object component, object value)
		{
			this.propertyDescriptor.SetValue(component, value);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0002E9A0 File Offset: 0x0002D9A0
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

		// Token: 0x04000E65 RID: 3685
		private PropertyDescriptor propertyDescriptor;

		// Token: 0x04000E66 RID: 3686
		private object defaultValue;

		// Token: 0x04000E67 RID: 3687
		private static object noDefault = new object();

		// Token: 0x04000E68 RID: 3688
		private bool initShouldSerialize;

		// Token: 0x04000E69 RID: 3689
		private object originalValue;

		// Token: 0x02000131 RID: 305
		private class ReadOnlyCollectionConverter : TypeConverter
		{
			// Token: 0x06000BE9 RID: 3049 RVA: 0x0002EA11 File Offset: 0x0002DA11
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
