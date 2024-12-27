using System;
using System.ComponentModel;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Data
{
	// Token: 0x020000ED RID: 237
	internal class XMLSchema
	{
		// Token: 0x06000DD0 RID: 3536 RVA: 0x00201798 File Offset: 0x00200B98
		internal static TypeConverter GetConverter(Type type)
		{
			CodeAccessPermission codeAccessPermission = (CodeAccessPermission)new HostProtectionAttribute
			{
				SharedState = true
			}.CreatePermission();
			codeAccessPermission.Assert();
			TypeConverter converter;
			try
			{
				converter = TypeDescriptor.GetConverter(type);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return converter;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x002017F0 File Offset: 0x00200BF0
		internal static void SetProperties(object instance, XmlAttributeCollection attrs)
		{
			for (int i = 0; i < attrs.Count; i++)
			{
				if (attrs[i].NamespaceURI == "urn:schemas-microsoft-com:xml-msdata")
				{
					string localName = attrs[i].LocalName;
					string value = attrs[i].Value;
					if (!(localName == "DefaultValue") && !(localName == "RemotingFormat") && (!(localName == "Expression") || !(instance is DataColumn)))
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(instance)[localName];
						if (propertyDescriptor != null)
						{
							Type propertyType = propertyDescriptor.PropertyType;
							TypeConverter converter = XMLSchema.GetConverter(propertyType);
							object obj;
							if (converter.CanConvertFrom(typeof(string)))
							{
								obj = converter.ConvertFromString(value);
							}
							else if (propertyType == typeof(Type))
							{
								obj = Type.GetType(value);
							}
							else
							{
								if (propertyType != typeof(CultureInfo))
								{
									throw ExceptionBuilder.CannotConvert(value, propertyType.FullName);
								}
								obj = new CultureInfo(value);
							}
							propertyDescriptor.SetValue(instance, obj);
						}
					}
				}
			}
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0020190C File Offset: 0x00200D0C
		internal static bool FEqualIdentity(XmlNode node, string name, string ns)
		{
			return node != null && node.LocalName == name && node.NamespaceURI == ns;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0020193C File Offset: 0x00200D3C
		internal static bool GetBooleanAttribute(XmlElement element, string attrName, string attrNS, bool defVal)
		{
			string attribute = element.GetAttribute(attrName, attrNS);
			if (attribute == null || attribute.Length == 0)
			{
				return defVal;
			}
			if (attribute == "true" || attribute == "1")
			{
				return true;
			}
			if (attribute == "false" || attribute == "0")
			{
				return false;
			}
			throw ExceptionBuilder.InvalidAttributeValue(attrName, attribute);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x002019A0 File Offset: 0x00200DA0
		internal static string GenUniqueColumnName(string proposedName, DataTable table)
		{
			if (table.Columns.IndexOf(proposedName) >= 0)
			{
				for (int i = 0; i <= table.Columns.Count; i++)
				{
					string text = proposedName + "_" + i.ToString(CultureInfo.InvariantCulture);
					if (table.Columns.IndexOf(text) < 0)
					{
						return text;
					}
				}
			}
			return proposedName;
		}
	}
}
