using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000234 RID: 564
	internal class Com2EnumConverter : TypeConverter
	{
		// Token: 0x06001AE9 RID: 6889 RVA: 0x00033B2B File Offset: 0x00032B2B
		public Com2EnumConverter(Com2Enum enumObj)
		{
			this.com2Enum = enumObj;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00033B3A File Offset: 0x00032B3A
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00033B53 File Offset: 0x00032B53
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return base.CanConvertTo(context, destType) || destType.IsEnum;
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00033B67 File Offset: 0x00032B67
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return this.com2Enum.FromString((string)value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00033B8C File Offset: 0x00032B8C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null)
			{
				string text = this.com2Enum.ToString(value);
				if (text != null)
				{
					return text;
				}
				return "";
			}
			else
			{
				if (destinationType.IsEnum)
				{
					return Enum.ToObject(destinationType, value);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00033BF0 File Offset: 0x00032BF0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				object[] array = this.com2Enum.Values;
				if (array != null)
				{
					this.values = new TypeConverter.StandardValuesCollection(array);
				}
			}
			return this.values;
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00033C26 File Offset: 0x00032C26
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return this.com2Enum.IsStrictEnum;
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00033C33 File Offset: 0x00032C33
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00033C38 File Offset: 0x00032C38
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string text = this.com2Enum.ToString(value);
			return text != null && text.Length > 0;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x00033C60 File Offset: 0x00032C60
		public void RefreshValues()
		{
			this.values = null;
		}

		// Token: 0x040012EB RID: 4843
		internal readonly Com2Enum com2Enum;

		// Token: 0x040012EC RID: 4844
		private TypeConverter.StandardValuesCollection values;
	}
}
