using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.CSharp
{
	// Token: 0x020002BA RID: 698
	internal abstract class CSharpModifierAttributeConverter : TypeConverter
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001771 RID: 6001
		protected abstract object[] Values { get; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001772 RID: 6002
		protected abstract string[] Names { get; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001773 RID: 6003
		protected abstract object DefaultValue { get; }

		// Token: 0x06001774 RID: 6004 RVA: 0x0004E3DD File Offset: 0x0004D3DD
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0004E3F8 File Offset: 0x0004D3F8
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = (string)value;
				string[] names = this.Names;
				for (int i = 0; i < names.Length; i++)
				{
					if (names[i].Equals(text))
					{
						return this.Values[i];
					}
				}
			}
			return this.DefaultValue;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0004E444 File Offset: 0x0004D444
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				object[] values = this.Values;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i].Equals(value))
					{
						return this.Names[i];
					}
				}
				return SR.GetString("toStringUnknown");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0004E4AD File Offset: 0x0004D4AD
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0004E4B0 File Offset: 0x0004D4B0
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x0004E4B3 File Offset: 0x0004D4B3
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new TypeConverter.StandardValuesCollection(this.Values);
		}
	}
}
