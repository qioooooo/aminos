using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000387 RID: 903
	internal class DataGridViewRowConverter : ExpandableObjectConverter
	{
		// Token: 0x06003785 RID: 14213 RVA: 0x000CA415 File Offset: 0x000C9415
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x000CA430 File Offset: 0x000C9430
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			DataGridViewRow dataGridViewRow = value as DataGridViewRow;
			if (destinationType == typeof(InstanceDescriptor) && dataGridViewRow != null)
			{
				ConstructorInfo constructor = dataGridViewRow.GetType().GetConstructor(new Type[0]);
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[0], false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
