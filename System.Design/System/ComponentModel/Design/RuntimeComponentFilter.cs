using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	internal static class RuntimeComponentFilter
	{
		public static void FilterProperties(IDictionary properties, ICollection makeReadWrite, ICollection makeBrowsable)
		{
			RuntimeComponentFilter.FilterProperties(properties, makeReadWrite, makeBrowsable, null);
		}

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
