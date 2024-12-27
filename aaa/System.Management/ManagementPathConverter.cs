using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Management
{
	// Token: 0x02000036 RID: 54
	internal class ManagementPathConverter : ExpandableObjectConverter
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x00009C3A File Offset: 0x00008C3A
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(ManagementPath) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009C53 File Offset: 0x00008C53
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009C6C File Offset: 0x00008C6C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is ManagementPath && destinationType == typeof(InstanceDescriptor))
			{
				ManagementPath managementPath = (ManagementPath)value;
				ConstructorInfo constructor = typeof(ManagementPath).GetConstructor(new Type[] { typeof(string) });
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[] { managementPath.Path });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
