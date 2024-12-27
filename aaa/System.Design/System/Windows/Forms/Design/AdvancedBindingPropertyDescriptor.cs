using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000174 RID: 372
	internal class AdvancedBindingPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000D9C RID: 3484 RVA: 0x00037C17 File Offset: 0x00036C17
		internal AdvancedBindingPropertyDescriptor()
			: base(SR.GetString("AdvancedBindingPropertyDescName"), null)
		{
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00037C2A File Offset: 0x00036C2A
		public override Type ComponentType
		{
			get
			{
				return typeof(ControlBindingsCollection);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x00037C38 File Offset: 0x00036C38
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

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00037C70 File Offset: 0x00036C70
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00037C73 File Offset: 0x00036C73
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00037C7F File Offset: 0x00036C7F
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

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00037C97 File Offset: 0x00036C97
		public override object GetEditor(Type type)
		{
			if (type == typeof(UITypeEditor))
			{
				return AdvancedBindingPropertyDescriptor.advancedBindingEditor;
			}
			return base.GetEditor(type);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00037CB3 File Offset: 0x00036CB3
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00037CB6 File Offset: 0x00036CB6
		protected override void FillAttributes(IList attributeList)
		{
			attributeList.Add(RefreshPropertiesAttribute.All);
			base.FillAttributes(attributeList);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00037CCB File Offset: 0x00036CCB
		public override object GetValue(object component)
		{
			return component;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00037CCE File Offset: 0x00036CCE
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00037CD0 File Offset: 0x00036CD0
		public override void SetValue(object component, object value)
		{
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00037CD2 File Offset: 0x00036CD2
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x04000F25 RID: 3877
		internal static AdvancedBindingEditor advancedBindingEditor = new AdvancedBindingEditor();

		// Token: 0x04000F26 RID: 3878
		internal static AdvancedBindingPropertyDescriptor.AdvancedBindingTypeConverter advancedBindingTypeConverter = new AdvancedBindingPropertyDescriptor.AdvancedBindingTypeConverter();

		// Token: 0x02000175 RID: 373
		internal class AdvancedBindingTypeConverter : TypeConverter
		{
			// Token: 0x06000DAA RID: 3498 RVA: 0x00037CEB File Offset: 0x00036CEB
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
