using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200048D RID: 1165
	internal class ListViewGroupConverter : TypeConverter
	{
		// Token: 0x060045A1 RID: 17825 RVA: 0x000FD10B File Offset: 0x000FC10B
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return (sourceType == typeof(string) && context != null && context.Instance is ListViewItem) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x000FD134 File Offset: 0x000FC134
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || (destinationType == typeof(string) && context != null && context.Instance is ListViewItem) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x000FD16C File Offset: 0x000FC16C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (context != null && context.Instance != null)
				{
					ListViewItem listViewItem = context.Instance as ListViewItem;
					if (listViewItem != null && listViewItem.ListView != null)
					{
						foreach (object obj in listViewItem.ListView.Groups)
						{
							ListViewGroup listViewGroup = (ListViewGroup)obj;
							if (listViewGroup.Header == text)
							{
								return listViewGroup;
							}
						}
					}
				}
			}
			if (value == null || value.Equals(SR.GetString("toStringNone")))
			{
				return null;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x000FD238 File Offset: 0x000FC238
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is ListViewGroup)
			{
				ListViewGroup listViewGroup = (ListViewGroup)value;
				ConstructorInfo constructor = typeof(ListViewGroup).GetConstructor(new Type[]
				{
					typeof(string),
					typeof(HorizontalAlignment)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[] { listViewGroup.Header, listViewGroup.HeaderAlignment }, false);
				}
			}
			if (destinationType == typeof(string) && value == null)
			{
				return SR.GetString("toStringNone");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x000FD2F4 File Offset: 0x000FC2F4
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				ListViewItem listViewItem = context.Instance as ListViewItem;
				if (listViewItem != null && listViewItem.ListView != null)
				{
					ArrayList arrayList = new ArrayList();
					foreach (object obj in listViewItem.ListView.Groups)
					{
						ListViewGroup listViewGroup = (ListViewGroup)obj;
						arrayList.Add(listViewGroup);
					}
					arrayList.Add(null);
					return new TypeConverter.StandardValuesCollection(arrayList);
				}
			}
			return null;
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x000FD390 File Offset: 0x000FC390
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x000FD393 File Offset: 0x000FC393
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
