using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Data
{
	// Token: 0x020000B3 RID: 179
	internal sealed class DefaultValueTypeConverter : StringConverter
	{
		// Token: 0x06000C24 RID: 3108 RVA: 0x001F9A58 File Offset: 0x001F8E58
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				if (value == null)
				{
					return DefaultValueTypeConverter.nullString;
				}
				if (value == DBNull.Value)
				{
					return DefaultValueTypeConverter.dbNullString;
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x001F9AA4 File Offset: 0x001F8EA4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null && value.GetType() == typeof(string))
			{
				string text = (string)value;
				if (string.Compare(text, DefaultValueTypeConverter.nullString, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return null;
				}
				if (string.Compare(text, DefaultValueTypeConverter.dbNullString, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return DBNull.Value;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x04000888 RID: 2184
		private static string nullString = "<null>";

		// Token: 0x04000889 RID: 2185
		private static string dbNullString = "<DBNull>";
	}
}
