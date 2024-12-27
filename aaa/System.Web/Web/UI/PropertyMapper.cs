using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200045C RID: 1116
	internal sealed class PropertyMapper
	{
		// Token: 0x060034E8 RID: 13544 RVA: 0x000E4F26 File Offset: 0x000E3F26
		private PropertyMapper()
		{
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000E4F30 File Offset: 0x000E3F30
		internal static MemberInfo GetMemberInfo(Type ctrlType, string name, out string nameForCodeGen)
		{
			Type type = ctrlType;
			PropertyInfo propertyInfo = null;
			FieldInfo fieldInfo = null;
			string text = PropertyMapper.MapNameToPropertyName(name);
			nameForCodeGen = null;
			int i = 0;
			while (i < text.Length)
			{
				int num = text.IndexOf('.', i);
				string text2;
				if (num < 0)
				{
					text2 = text.Substring(i);
					i = text.Length;
				}
				else
				{
					text2 = text.Substring(i, num - i);
					i = num + 1;
				}
				BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
				try
				{
					propertyInfo = type.GetProperty(text2, bindingFlags);
				}
				catch (AmbiguousMatchException)
				{
					bindingFlags |= BindingFlags.DeclaredOnly;
					propertyInfo = type.GetProperty(text2, bindingFlags);
				}
				if (propertyInfo == null)
				{
					fieldInfo = type.GetField(text2, bindingFlags);
					if (fieldInfo == null)
					{
						nameForCodeGen = null;
						break;
					}
				}
				text2 = null;
				if (propertyInfo != null)
				{
					type = propertyInfo.PropertyType;
					text2 = propertyInfo.Name;
				}
				else
				{
					type = fieldInfo.FieldType;
					text2 = fieldInfo.Name;
				}
				if (!PropertyMapper.IsTypeCLSCompliant(type))
				{
					throw new HttpException(SR.GetString("Property_Not_ClsCompliant", new object[] { name, ctrlType.FullName, type.FullName }));
				}
				if (text2 != null)
				{
					if (nameForCodeGen == null)
					{
						nameForCodeGen = text2;
					}
					else
					{
						nameForCodeGen = nameForCodeGen + "." + text2;
					}
				}
			}
			if (propertyInfo != null)
			{
				return propertyInfo;
			}
			return fieldInfo;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000E5070 File Offset: 0x000E4070
		private static bool IsTypeCLSCompliant(Type type)
		{
			return type != typeof(sbyte) && type != typeof(TypedReference) && type != typeof(ushort) && type != typeof(uint) && type != typeof(ulong) && type != typeof(UIntPtr);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000E50CE File Offset: 0x000E40CE
		internal static string MapNameToPropertyName(string attrName)
		{
			return attrName.Replace('-', '.');
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x000E50DC File Offset: 0x000E40DC
		internal static object LocatePropertyObject(object obj, string mappedName, out string propertyName, bool inDesigner)
		{
			object obj2 = obj;
			obj.GetType();
			propertyName = null;
			int i = 0;
			while (i < mappedName.Length)
			{
				int num = mappedName.IndexOf('.', i);
				if (num < 0)
				{
					break;
				}
				propertyName = mappedName.Substring(i, num - i);
				i = num + 1;
				obj2 = FastPropertyAccessor.GetProperty(obj2, propertyName, inDesigner);
				if (obj2 == null)
				{
					return null;
				}
			}
			if (i > 0)
			{
				propertyName = mappedName.Substring(i);
			}
			else
			{
				propertyName = mappedName;
			}
			return obj2;
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x000E5144 File Offset: 0x000E4144
		internal static PropertyDescriptor GetMappedPropertyDescriptor(object obj, string mappedName, out object childObject, out string propertyName, bool inDesigner)
		{
			childObject = PropertyMapper.LocatePropertyObject(obj, mappedName, out propertyName, inDesigner);
			if (childObject == null)
			{
				return null;
			}
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(childObject, inDesigner);
			return properties[propertyName];
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000E5178 File Offset: 0x000E4178
		internal static void SetMappedPropertyValue(object obj, string mappedName, object value, bool inDesigner)
		{
			string text;
			object obj2 = PropertyMapper.LocatePropertyObject(obj, mappedName, out text, inDesigner);
			if (obj2 == null)
			{
				return;
			}
			FastPropertyAccessor.SetProperty(obj2, text, value, inDesigner);
		}

		// Token: 0x04002507 RID: 9479
		private const char PERSIST_CHAR = '-';

		// Token: 0x04002508 RID: 9480
		private const char OM_CHAR = '.';

		// Token: 0x04002509 RID: 9481
		private const string STR_OM_CHAR = ".";
	}
}
