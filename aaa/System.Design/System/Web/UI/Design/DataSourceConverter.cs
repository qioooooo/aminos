using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000354 RID: 852
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataSourceConverter : TypeConverter
	{
		// Token: 0x06001FEB RID: 8171 RVA: 0x000B5D9F File Offset: 0x000B4D9F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000B5DB1 File Offset: 0x000B4DB1
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value.GetType() == typeof(string))
			{
				return (string)value;
			}
			throw base.GetConvertFromException(value);
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000B5DDC File Offset: 0x000B4DDC
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			object[] array = null;
			if (context != null)
			{
				ArrayList arrayList = new ArrayList();
				IContainer container = context.Container;
				if (container != null)
				{
					ComponentCollection components = container.Components;
					foreach (object obj in ((IEnumerable)components))
					{
						IComponent component = (IComponent)obj;
						if (this.IsValidDataSource(component) && !Marshal.IsComObject(component))
						{
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["Modifiers"];
							if (propertyDescriptor != null)
							{
								MemberAttributes memberAttributes = (MemberAttributes)propertyDescriptor.GetValue(component);
								if ((memberAttributes & MemberAttributes.AccessMask) == MemberAttributes.Private)
								{
									continue;
								}
							}
							ISite site = component.Site;
							if (site != null)
							{
								string name = site.Name;
								if (name != null)
								{
									arrayList.Add(name);
								}
							}
						}
					}
				}
				array = arrayList.ToArray();
				Array.Sort(array, Comparer.Default);
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000B5EDC File Offset: 0x000B4EDC
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000B5EDF File Offset: 0x000B4EDF
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x000B5EE2 File Offset: 0x000B4EE2
		protected virtual bool IsValidDataSource(IComponent component)
		{
			return component is IEnumerable || component is IListSource;
		}
	}
}
