using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000138 RID: 312
	internal class TypeTypeConverter : TypeAndNameConverter
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x00045CA0 File Offset: 0x00044CA0
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00045CAC File Offset: 0x00044CAC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				TypeAndName typeAndName = (TypeAndName)base.ConvertFrom(context, culture, value);
				return typeAndName.type;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00045CE0 File Offset: 0x00044CE0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				TypeAndName typeAndName = new TypeAndName((Type)value);
				return base.ConvertTo(context, culture, typeAndName, destinationType);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
