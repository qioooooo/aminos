using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	// Token: 0x020000E3 RID: 227
	public class SizeFConverter : TypeConverter
	{
		// Token: 0x06000D1A RID: 3354 RVA: 0x00026F45 File Offset: 0x00025F45
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00026F5E File Offset: 0x00025F5E
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00026F78 File Offset: 0x00025F78
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
			float[] array2 = new float[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (float)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 2)
			{
				return new SizeF(array2[0], array2[1]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[] { text2, "Width,Height" }));
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00027068 File Offset: 0x00026068
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is SizeF)
			{
				SizeF sizeF = (SizeF)value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string text = culture.TextInfo.ListSeparator + " ";
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
				string[] array = new string[2];
				int num = 0;
				array[num++] = converter.ConvertToString(context, culture, sizeF.Width);
				array[num++] = converter.ConvertToString(context, culture, sizeF.Height);
				return string.Join(text, array);
			}
			if (destinationType == typeof(InstanceDescriptor) && value is SizeF)
			{
				SizeF sizeF2 = (SizeF)value;
				ConstructorInfo constructor = typeof(SizeF).GetConstructor(new Type[]
				{
					typeof(float),
					typeof(float)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[] { sizeF2.Width, sizeF2.Height });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x000271B9 File Offset: 0x000261B9
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new SizeF((float)propertyValues["Width"], (float)propertyValues["Height"]);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x000271E5 File Offset: 0x000261E5
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000271E8 File Offset: 0x000261E8
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SizeF), attributes);
			return properties.Sort(new string[] { "Width", "Height" });
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00027224 File Offset: 0x00026224
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
