using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing.Printing
{
	// Token: 0x0200010C RID: 268
	public class MarginsConverter : ExpandableObjectConverter
	{
		// Token: 0x06000E45 RID: 3653 RVA: 0x00029F70 File Offset: 0x00028F70
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00029F89 File Offset: 0x00028F89
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00029FA4 File Offset: 0x00028FA4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text2 = text.Trim();
			if (text2.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char c = culture.TextInfo.ListSeparator[0];
			string[] array = text2.Split(new char[] { c });
			int[] array2 = new int[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length != 4)
			{
				throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[] { text2, "left, right, top, bottom" }));
			}
			return new Margins(array2[0], array2[1], array2[2], array2[3]);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0002A094 File Offset: 0x00029094
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Margins)
			{
				if (destinationType == typeof(string))
				{
					Margins margins = (Margins)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string text = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[4];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, margins.Left);
					array[num++] = converter.ConvertToString(context, culture, margins.Right);
					array[num++] = converter.ConvertToString(context, culture, margins.Top);
					array[num++] = converter.ConvertToString(context, culture, margins.Bottom);
					return string.Join(text, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					Margins margins2 = (Margins)value;
					ConstructorInfo constructor = typeof(Margins).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[] { margins2.Left, margins2.Right, margins2.Top, margins2.Bottom });
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0002A254 File Offset: 0x00029254
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			object obj = propertyValues["Left"];
			object obj2 = propertyValues["Right"];
			object obj3 = propertyValues["Top"];
			object obj4 = propertyValues["Bottom"];
			if (obj == null || obj2 == null || obj4 == null || obj3 == null || !(obj is int) || !(obj2 is int) || !(obj4 is int) || !(obj3 is int))
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
			}
			return new Margins((int)obj, (int)obj2, (int)obj3, (int)obj4);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0002A2F8 File Offset: 0x000292F8
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
