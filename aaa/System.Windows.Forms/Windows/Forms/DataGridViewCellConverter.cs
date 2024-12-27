using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000314 RID: 788
	internal class DataGridViewCellConverter : ExpandableObjectConverter
	{
		// Token: 0x06003338 RID: 13112 RVA: 0x000B3D0C File Offset: 0x000B2D0C
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000B3D28 File Offset: 0x000B2D28
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			DataGridViewCell dataGridViewCell = value as DataGridViewCell;
			if (destinationType == typeof(InstanceDescriptor) && dataGridViewCell != null)
			{
				ConstructorInfo constructor = dataGridViewCell.GetType().GetConstructor(new Type[0]);
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[0], false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
