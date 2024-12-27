using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200047E RID: 1150
	internal class ListViewSubItemConverter : ExpandableObjectConverter
	{
		// Token: 0x0600436F RID: 17263 RVA: 0x000F1E55 File Offset: 0x000F0E55
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x000F1E70 File Offset: 0x000F0E70
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is ListViewItem.ListViewSubItem)
			{
				ListViewItem.ListViewSubItem listViewSubItem = (ListViewItem.ListViewSubItem)value;
				ConstructorInfo constructorInfo;
				if (listViewSubItem.CustomStyle)
				{
					constructorInfo = typeof(ListViewItem.ListViewSubItem).GetConstructor(new Type[]
					{
						typeof(ListViewItem),
						typeof(string),
						typeof(Color),
						typeof(Color),
						typeof(Font)
					});
					if (constructorInfo != null)
					{
						return new InstanceDescriptor(constructorInfo, new object[] { null, listViewSubItem.Text, listViewSubItem.ForeColor, listViewSubItem.BackColor, listViewSubItem.Font }, true);
					}
				}
				constructorInfo = typeof(ListViewItem.ListViewSubItem).GetConstructor(new Type[]
				{
					typeof(ListViewItem),
					typeof(string)
				});
				if (constructorInfo != null)
				{
					return new InstanceDescriptor(constructorInfo, new object[] { null, listViewSubItem.Text }, true);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
