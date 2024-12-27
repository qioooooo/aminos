using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200045B RID: 1115
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class PropertyConverter
	{
		// Token: 0x060034E4 RID: 13540 RVA: 0x000E4D08 File Offset: 0x000E3D08
		public static object EnumFromString(Type enumType, string value)
		{
			object obj;
			try
			{
				obj = Enum.Parse(enumType, value, true);
			}
			catch
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000E4D38 File Offset: 0x000E3D38
		public static string EnumToString(Type enumType, object enumValue)
		{
			string text = Enum.Format(enumType, enumValue, "G");
			return text.Replace('_', '-');
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000E4D5C File Offset: 0x000E3D5C
		public static object ObjectFromString(Type objType, MemberInfo propertyInfo, string value)
		{
			if (value == null)
			{
				return null;
			}
			if (objType.Equals(typeof(bool)) && value.Length == 0)
			{
				return null;
			}
			bool flag = true;
			object obj = null;
			try
			{
				if (objType.IsEnum)
				{
					flag = false;
					obj = PropertyConverter.EnumFromString(objType, value);
				}
				else if (objType.Equals(typeof(string)))
				{
					flag = false;
					obj = value;
				}
				else
				{
					PropertyDescriptor propertyDescriptor = null;
					if (propertyInfo != null)
					{
						propertyDescriptor = TypeDescriptor.GetProperties(propertyInfo.ReflectedType)[propertyInfo.Name];
					}
					if (propertyDescriptor != null)
					{
						TypeConverter converter = propertyDescriptor.Converter;
						if (converter != null && converter.CanConvertFrom(typeof(string)))
						{
							flag = false;
							obj = converter.ConvertFromInvariantString(value);
						}
					}
				}
			}
			catch
			{
			}
			if (flag)
			{
				MethodInfo methodInfo = objType.GetMethod("Parse", PropertyConverter.s_parseMethodTypesWithSOP);
				if (methodInfo != null)
				{
					object[] array = new object[]
					{
						value,
						CultureInfo.InvariantCulture
					};
					try
					{
						obj = Util.InvokeMethod(methodInfo, null, array);
						goto IL_010D;
					}
					catch
					{
						goto IL_010D;
					}
				}
				methodInfo = objType.GetMethod("Parse", PropertyConverter.s_parseMethodTypes);
				if (methodInfo != null)
				{
					object[] array2 = new object[] { value };
					try
					{
						obj = Util.InvokeMethod(methodInfo, null, array2);
					}
					catch
					{
					}
				}
			}
			IL_010D:
			if (obj == null)
			{
				throw new HttpException(SR.GetString("Type_not_creatable_from_string", new object[] { objType.FullName, value, propertyInfo.Name }));
			}
			return obj;
		}

		// Token: 0x04002505 RID: 9477
		private static readonly Type[] s_parseMethodTypes = new Type[] { typeof(string) };

		// Token: 0x04002506 RID: 9478
		private static readonly Type[] s_parseMethodTypesWithSOP = new Type[]
		{
			typeof(string),
			typeof(IServiceProvider)
		};
	}
}
