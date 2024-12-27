using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200013F RID: 319
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class TimeSpanConverter : TypeConverter
	{
		// Token: 0x06000A5C RID: 2652 RVA: 0x00023F7F File Offset: 0x00022F7F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00023F98 File Offset: 0x00022F98
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00023FB4 File Offset: 0x00022FB4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				try
				{
					return TimeSpan.Parse(text);
				}
				catch (FormatException ex)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						"TimeSpan"
					}), ex);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0002402C File Offset: 0x0002302C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is TimeSpan)
			{
				MethodInfo method = typeof(TimeSpan).GetMethod("Parse", new Type[] { typeof(string) });
				if (method != null)
				{
					return new InstanceDescriptor(method, new object[] { value.ToString() });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
