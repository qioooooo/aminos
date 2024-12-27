using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200047D RID: 1149
	public class ListViewItemConverter : ExpandableObjectConverter
	{
		// Token: 0x0600436C RID: 17260 RVA: 0x000F1839 File Offset: 0x000F0839
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x000F1854 File Offset: 0x000F0854
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is ListViewItem)
			{
				ListViewItem listViewItem = (ListViewItem)value;
				int i = 1;
				while (i < listViewItem.SubItems.Count)
				{
					if (listViewItem.SubItems[i].CustomStyle)
					{
						if (!string.IsNullOrEmpty(listViewItem.ImageKey))
						{
							ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
							{
								typeof(ListViewItem.ListViewSubItem[]),
								typeof(string)
							});
							if (constructorInfo != null)
							{
								ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[listViewItem.SubItems.Count];
								((ICollection)listViewItem.SubItems).CopyTo(array, 0);
								return new InstanceDescriptor(constructorInfo, new object[] { array, listViewItem.ImageKey }, false);
							}
							break;
						}
						else
						{
							ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
							{
								typeof(ListViewItem.ListViewSubItem[]),
								typeof(int)
							});
							if (constructorInfo != null)
							{
								ListViewItem.ListViewSubItem[] array2 = new ListViewItem.ListViewSubItem[listViewItem.SubItems.Count];
								((ICollection)listViewItem.SubItems).CopyTo(array2, 0);
								return new InstanceDescriptor(constructorInfo, new object[] { array2, listViewItem.ImageIndex }, false);
							}
							break;
						}
					}
					else
					{
						i++;
					}
				}
				string[] array3 = new string[listViewItem.SubItems.Count];
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = listViewItem.SubItems[j].Text;
				}
				if (listViewItem.SubItems[0].CustomStyle)
				{
					if (!string.IsNullOrEmpty(listViewItem.ImageKey))
					{
						ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string[]),
							typeof(string),
							typeof(Color),
							typeof(Color),
							typeof(Font)
						});
						if (constructorInfo != null)
						{
							return new InstanceDescriptor(constructorInfo, new object[]
							{
								array3,
								listViewItem.ImageKey,
								listViewItem.SubItems[0].CustomForeColor ? listViewItem.ForeColor : Color.Empty,
								listViewItem.SubItems[0].CustomBackColor ? listViewItem.BackColor : Color.Empty,
								listViewItem.SubItems[0].CustomFont ? listViewItem.Font : null
							}, false);
						}
					}
					else
					{
						ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string[]),
							typeof(int),
							typeof(Color),
							typeof(Color),
							typeof(Font)
						});
						if (constructorInfo != null)
						{
							return new InstanceDescriptor(constructorInfo, new object[]
							{
								array3,
								listViewItem.ImageIndex,
								listViewItem.SubItems[0].CustomForeColor ? listViewItem.ForeColor : Color.Empty,
								listViewItem.SubItems[0].CustomBackColor ? listViewItem.BackColor : Color.Empty,
								listViewItem.SubItems[0].CustomFont ? listViewItem.Font : null
							}, false);
						}
					}
				}
				if (listViewItem.ImageIndex == -1 && string.IsNullOrEmpty(listViewItem.ImageKey) && listViewItem.SubItems.Count <= 1)
				{
					ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[] { typeof(string) });
					if (constructorInfo != null)
					{
						return new InstanceDescriptor(constructorInfo, new object[] { listViewItem.Text }, false);
					}
				}
				if (listViewItem.SubItems.Count <= 1)
				{
					if (!string.IsNullOrEmpty(listViewItem.ImageKey))
					{
						ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string),
							typeof(string)
						});
						if (constructorInfo != null)
						{
							return new InstanceDescriptor(constructorInfo, new object[] { listViewItem.Text, listViewItem.ImageKey }, false);
						}
					}
					else
					{
						ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string),
							typeof(int)
						});
						if (constructorInfo != null)
						{
							return new InstanceDescriptor(constructorInfo, new object[] { listViewItem.Text, listViewItem.ImageIndex }, false);
						}
					}
				}
				if (!string.IsNullOrEmpty(listViewItem.ImageKey))
				{
					ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
					{
						typeof(string[]),
						typeof(string)
					});
					if (constructorInfo != null)
					{
						return new InstanceDescriptor(constructorInfo, new object[] { array3, listViewItem.ImageKey }, false);
					}
				}
				else
				{
					ConstructorInfo constructorInfo = typeof(ListViewItem).GetConstructor(new Type[]
					{
						typeof(string[]),
						typeof(int)
					});
					if (constructorInfo != null)
					{
						return new InstanceDescriptor(constructorInfo, new object[] { array3, listViewItem.ImageIndex }, false);
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
