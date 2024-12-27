using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	// Token: 0x0200005D RID: 93
	public class RectangleConverter : TypeConverter
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x00018331 File Offset: 0x00017331
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001834A File Offset: 0x0001734A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00018364 File Offset: 0x00017364
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
			if (array2.Length == 4)
			{
				return new Rectangle(array2[0], array2[1], array2[2], array2[3]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[] { "text", text2, "x, y, width, height" }));
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00018464 File Offset: 0x00017464
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Rectangle)
			{
				if (destinationType == typeof(string))
				{
					Rectangle rectangle = (Rectangle)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string text = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[4];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, rectangle.X);
					array[num++] = converter.ConvertToString(context, culture, rectangle.Y);
					array[num++] = converter.ConvertToString(context, culture, rectangle.Width);
					array[num++] = converter.ConvertToString(context, culture, rectangle.Height);
					return string.Join(text, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					Rectangle rectangle2 = (Rectangle)value;
					ConstructorInfo constructor = typeof(Rectangle).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[] { rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height });
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00018628 File Offset: 0x00017628
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			object obj = propertyValues["X"];
			object obj2 = propertyValues["Y"];
			object obj3 = propertyValues["Width"];
			object obj4 = propertyValues["Height"];
			if (obj == null || obj2 == null || obj3 == null || obj4 == null || !(obj is int) || !(obj2 is int) || !(obj3 is int) || !(obj4 is int))
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
			}
			return new Rectangle((int)obj, (int)obj2, (int)obj3, (int)obj4);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000186D1 File Offset: 0x000176D1
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000186D4 File Offset: 0x000176D4
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Rectangle), attributes);
			return properties.Sort(new string[] { "X", "Y", "Width", "Height" });
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00018720 File Offset: 0x00017720
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
