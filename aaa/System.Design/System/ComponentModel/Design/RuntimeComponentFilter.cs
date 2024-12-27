using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x0200013D RID: 317
	internal static class RuntimeComponentFilter
	{
		// Token: 0x06000C55 RID: 3157 RVA: 0x000305DD File Offset: 0x0002F5DD
		public static void FilterProperties(IDictionary properties, ICollection makeReadWrite, ICollection makeBrowsable)
		{
			RuntimeComponentFilter.FilterProperties(properties, makeReadWrite, makeBrowsable, null);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000305E8 File Offset: 0x0002F5E8
		public static void FilterProperties(IDictionary properties, ICollection makeReadWrite, ICollection makeBrowsable, bool[] browsableSettings)
		{
			if (makeReadWrite != null)
			{
				foreach (object obj in makeReadWrite)
				{
					string text = (string)obj;
					PropertyDescriptor propertyDescriptor = properties[text] as PropertyDescriptor;
					if (propertyDescriptor != null)
					{
						properties[text] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { ReadOnlyAttribute.No });
					}
				}
			}
			if (makeBrowsable != null)
			{
				int num = -1;
				foreach (object obj2 in makeBrowsable)
				{
					string text2 = (string)obj2;
					PropertyDescriptor propertyDescriptor2 = properties[text2] as PropertyDescriptor;
					num++;
					if (propertyDescriptor2 != null)
					{
						Attribute attribute;
						if (browsableSettings == null || browsableSettings[num])
						{
							attribute = BrowsableAttribute.Yes;
						}
						else
						{
							attribute = BrowsableAttribute.No;
						}
						properties[text2] = TypeDescriptor.CreateProperty(propertyDescriptor2.ComponentType, propertyDescriptor2, new Attribute[] { attribute });
					}
				}
			}
		}
	}
}
