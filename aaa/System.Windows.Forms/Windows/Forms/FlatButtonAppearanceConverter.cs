using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020003FE RID: 1022
	internal class FlatButtonAppearanceConverter : ExpandableObjectConverter
	{
		// Token: 0x06003C4D RID: 15437 RVA: 0x000D9548 File Offset: 0x000D8548
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x000D956C File Offset: 0x000D856C
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (context != null && context.Instance is Button)
			{
				Attribute[] array = new Attribute[attributes.Length + 1];
				attributes.CopyTo(array, 0);
				array[attributes.Length] = new ApplicableToButtonAttribute();
				attributes = array;
			}
			return TypeDescriptor.GetProperties(value, attributes);
		}
	}
}
