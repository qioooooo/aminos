using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	// Token: 0x0200005A RID: 90
	public class PointConverter : TypeConverter
	{
		// Token: 0x0600058E RID: 1422 RVA: 0x0001773C File Offset: 0x0001673C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00017755 File Offset: 0x00016755
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00017770 File Offset: 0x00016770
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
			if (array2.Length == 2)
			{
				return new Point(array2[0], array2[1]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[] { text2, "x, y" }));
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00017860 File Offset: 0x00016860
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Point)
			{
				if (destinationType == typeof(string))
				{
					Point point = (Point)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string text = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[2];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, point.X);
					array[num++] = converter.ConvertToString(context, culture, point.Y);
					return string.Join(text, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					Point point2 = (Point)value;
					ConstructorInfo constructor = typeof(Point).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[] { point2.X, point2.Y });
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000179AC File Offset: 0x000169AC
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			object obj = propertyValues["X"];
			object obj2 = propertyValues["Y"];
			if (obj == null || obj2 == null || !(obj is int) || !(obj2 is int))
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
			}
			return new Point((int)obj, (int)obj2);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00017A1B File Offset: 0x00016A1B
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00017A20 File Offset: 0x00016A20
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Point), attributes);
			return properties.Sort(new string[] { "X", "Y" });
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00017A5C File Offset: 0x00016A5C
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
