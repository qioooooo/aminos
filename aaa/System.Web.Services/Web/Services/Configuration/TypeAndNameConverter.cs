using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000137 RID: 311
	internal class TypeAndNameConverter : TypeConverter
	{
		// Token: 0x06000998 RID: 2456 RVA: 0x00045C15 File Offset: 0x00044C15
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00045C2E File Offset: 0x00044C2E
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return new TypeAndName((string)value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00045C50 File Offset: 0x00044C50
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			TypeAndName typeAndName = (TypeAndName)value;
			if (typeAndName.name != null)
			{
				return typeAndName.name;
			}
			return typeAndName.type.AssemblyQualifiedName;
		}
	}
}
