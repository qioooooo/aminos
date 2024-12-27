using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Management
{
	// Token: 0x0200003F RID: 63
	internal class ManagementQueryConverter : ExpandableObjectConverter
	{
		// Token: 0x0600024E RID: 590 RVA: 0x0000C1FF File Offset: 0x0000B1FF
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(ManagementQuery) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C218 File Offset: 0x0000B218
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C234 File Offset: 0x0000B234
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is EventQuery && destinationType == typeof(InstanceDescriptor))
			{
				EventQuery eventQuery = (EventQuery)value;
				ConstructorInfo constructor = typeof(EventQuery).GetConstructor(new Type[] { typeof(string) });
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[] { eventQuery.QueryString });
				}
			}
			if (value is ObjectQuery && destinationType == typeof(InstanceDescriptor))
			{
				ObjectQuery objectQuery = (ObjectQuery)value;
				ConstructorInfo constructor2 = typeof(ObjectQuery).GetConstructor(new Type[] { typeof(string) });
				if (constructor2 != null)
				{
					return new InstanceDescriptor(constructor2, new object[] { objectQuery.QueryString });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
