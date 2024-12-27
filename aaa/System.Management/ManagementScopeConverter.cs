using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Management
{
	// Token: 0x0200007A RID: 122
	internal class ManagementScopeConverter : ExpandableObjectConverter
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0000DEF9 File Offset: 0x0000CEF9
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(ManagementScope) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000DF12 File Offset: 0x0000CF12
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000DF2C File Offset: 0x0000CF2C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is ManagementScope && destinationType == typeof(InstanceDescriptor))
			{
				ManagementScope managementScope = (ManagementScope)value;
				ConstructorInfo constructor = typeof(ManagementScope).GetConstructor(new Type[] { typeof(string) });
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[] { managementScope.Path.Path });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
