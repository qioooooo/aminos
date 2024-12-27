using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.VisualBasic
{
	// Token: 0x020002BF RID: 703
	internal abstract class VBModifierAttributeConverter : TypeConverter
	{
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001802 RID: 6146
		protected abstract object[] Values { get; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001803 RID: 6147
		protected abstract string[] Names { get; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001804 RID: 6148
		protected abstract object DefaultValue { get; }

		// Token: 0x06001805 RID: 6149 RVA: 0x00052F0A File Offset: 0x00051F0A
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00052F24 File Offset: 0x00051F24
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = (string)value;
				string[] names = this.Names;
				for (int i = 0; i < names.Length; i++)
				{
					if (names[i].Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						return this.Values[i];
					}
				}
			}
			return this.DefaultValue;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00052F70 File Offset: 0x00051F70
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

		// Token: 0x06001808 RID: 6152 RVA: 0x00052FD9 File Offset: 0x00051FD9
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00052FDC File Offset: 0x00051FDC
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00052FDF File Offset: 0x00051FDF
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new TypeConverter.StandardValuesCollection(this.Values);
		}
	}
}
