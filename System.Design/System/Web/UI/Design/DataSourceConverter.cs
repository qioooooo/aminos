using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataSourceConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

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

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		protected virtual bool IsValidDataSource(IComponent component)
		{
			return component is IEnumerable || component is IListSource;
		}
	}
}
