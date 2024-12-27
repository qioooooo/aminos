using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x020002BD RID: 701
	public class CursorConverter : TypeConverter
	{
		// Token: 0x060026D1 RID: 9937 RVA: 0x0005F77F File Offset: 0x0005E77F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(byte[]) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x0005F7A5 File Offset: 0x0005E7A5
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0005F7CC File Offset: 0x0005E7CC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				foreach (PropertyInfo propertyInfo in this.GetProperties())
				{
					if (string.Equals(propertyInfo.Name, text, StringComparison.OrdinalIgnoreCase))
					{
						object[] array = null;
						return propertyInfo.GetValue(null, array);
					}
				}
			}
			if (value is byte[])
			{
				MemoryStream memoryStream = new MemoryStream((byte[])value);
				return new Cursor(memoryStream);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x0005F848 File Offset: 0x0005E848
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null)
			{
				PropertyInfo[] properties = this.GetProperties();
				int num = -1;
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo propertyInfo = properties[i];
					object[] array = null;
					Cursor cursor = (Cursor)propertyInfo.GetValue(null, array);
					if (cursor == (Cursor)value)
					{
						if (object.ReferenceEquals(cursor, value))
						{
							return propertyInfo.Name;
						}
						num = i;
					}
				}
				if (num != -1)
				{
					return properties[num].Name;
				}
				throw new FormatException(SR.GetString("CursorCannotCovertToString"));
			}
			else
			{
				if (destinationType == typeof(InstanceDescriptor) && value is Cursor)
				{
					PropertyInfo[] properties2 = this.GetProperties();
					foreach (PropertyInfo propertyInfo2 in properties2)
					{
						if (propertyInfo2.GetValue(null, null) == value)
						{
							return new InstanceDescriptor(propertyInfo2, null);
						}
					}
				}
				if (destinationType != typeof(byte[]))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				if (value != null)
				{
					MemoryStream memoryStream = new MemoryStream();
					Cursor cursor2 = (Cursor)value;
					cursor2.SavePicture(memoryStream);
					memoryStream.Close();
					return memoryStream.ToArray();
				}
				return new byte[0];
			}
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x0005F983 File Offset: 0x0005E983
		private PropertyInfo[] GetProperties()
		{
			return typeof(Cursors).GetProperties(BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x0005F998 File Offset: 0x0005E998
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

		// Token: 0x060026D7 RID: 9943 RVA: 0x0005F9F6 File Offset: 0x0005E9F6
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0400165E RID: 5726
		private TypeConverter.StandardValuesCollection values;
	}
}
