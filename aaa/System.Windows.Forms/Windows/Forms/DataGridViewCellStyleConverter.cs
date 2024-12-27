using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000323 RID: 803
	public class DataGridViewCellStyleConverter : TypeConverter
	{
		// Token: 0x060033BB RID: 13243 RVA: 0x000B5787 File Offset: 0x000B4787
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000B57A0 File Offset: 0x000B47A0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is DataGridViewCellStyle)
			{
				ConstructorInfo constructor = value.GetType().GetConstructor(new Type[0]);
				return new InstanceDescriptor(constructor, new object[0], false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
