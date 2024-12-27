using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	// Token: 0x020000C1 RID: 193
	public class ImageFormatConverter : TypeConverter
	{
		// Token: 0x06000BCB RID: 3019 RVA: 0x00023215 File Offset: 0x00022215
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002322E File Offset: 0x0002222E
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00023248 File Offset: 0x00022248
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				string text2 = text.Trim();
				foreach (PropertyInfo propertyInfo in this.GetProperties())
				{
					if (string.Equals(propertyInfo.Name, text2, StringComparison.OrdinalIgnoreCase))
					{
						object[] array = null;
						return propertyInfo.GetValue(null, array);
					}
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000232A8 File Offset: 0x000222A8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is ImageFormat)
			{
				PropertyInfo propertyInfo = null;
				PropertyInfo[] properties = this.GetProperties();
				foreach (PropertyInfo propertyInfo2 in properties)
				{
					if (propertyInfo2.GetValue(null, null).Equals(value))
					{
						propertyInfo = propertyInfo2;
						break;
					}
				}
				if (propertyInfo != null)
				{
					if (destinationType == typeof(string))
					{
						return propertyInfo.Name;
					}
					if (destinationType == typeof(InstanceDescriptor))
					{
						return new InstanceDescriptor(propertyInfo, null);
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0002333B File Offset: 0x0002233B
		private PropertyInfo[] GetProperties()
		{
			return typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00023350 File Offset: 0x00022350
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				ArrayList arrayList = new ArrayList();
				foreach (PropertyInfo propertyInfo in this.GetProperties())
				{
					object[] array = null;
					arrayList.Add(propertyInfo.GetValue(null, array));
				}
				this.values = new TypeConverter.StandardValuesCollection(arrayList.ToArray());
			}
			return this.values;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000233AE File Offset: 0x000223AE
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000A60 RID: 2656
		private TypeConverter.StandardValuesCollection values;
	}
}
