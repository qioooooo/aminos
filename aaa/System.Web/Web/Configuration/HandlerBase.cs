using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x020001E6 RID: 486
	internal static class HandlerBase
	{
		// Token: 0x06001AE8 RID: 6888 RVA: 0x0007CA60 File Offset: 0x0007BA60
		private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attrib);
			if (fRequired && xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Missing_required_attribute", new object[] { attrib, node.Name }), node);
			}
			return xmlNode;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0007CAA8 File Offset: 0x0007BAA8
		private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = andRemoveAttribute.Value;
			}
			return andRemoveAttribute;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0007CACA File Offset: 0x0007BACA
		internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0007CAD5 File Offset: 0x0007BAD5
		internal static XmlNode GetAndRemoveRequiredStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, true, ref val);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0007CAE0 File Offset: 0x0007BAE0
		internal static XmlNode GetAndRemoveNonEmptyStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveNonEmptyStringAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x0007CAEB File Offset: 0x0007BAEB
		internal static XmlNode GetAndRemoveRequiredNonEmptyStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveNonEmptyStringAttributeInternal(node, attrib, true, ref val);
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x0007CAF8 File Offset: 0x0007BAF8
		private static XmlNode GetAndRemoveNonEmptyStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveStringAttributeInternal = HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, fRequired, ref val);
			if (andRemoveStringAttributeInternal != null && val.Length == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Empty_attribute", new object[] { attrib }), andRemoveStringAttributeInternal);
			}
			return andRemoveStringAttributeInternal;
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x0007CB3C File Offset: 0x0007BB3C
		private static XmlNode GetAndRemoveBooleanAttributeInternal(XmlNode node, string attrib, bool fRequired, ref bool val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				if (andRemoveAttribute.Value == "true")
				{
					val = true;
				}
				else
				{
					if (!(andRemoveAttribute.Value == "false"))
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_boolean_attribute", new object[] { andRemoveAttribute.Name }), andRemoveAttribute);
					}
					val = false;
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0007CBA6 File Offset: 0x0007BBA6
		internal static XmlNode GetAndRemoveBooleanAttribute(XmlNode node, string attrib, ref bool val)
		{
			return HandlerBase.GetAndRemoveBooleanAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0007CBB4 File Offset: 0x0007BBB4
		private static XmlNode GetAndRemoveIntegerAttributeInternal(XmlNode node, string attrib, bool fRequired, ref int val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				if (andRemoveAttribute.Value.Trim() != andRemoveAttribute.Value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_integer_attribute", new object[] { andRemoveAttribute.Name }), andRemoveAttribute);
				}
				try
				{
					val = int.Parse(andRemoveAttribute.Value, CultureInfo.InvariantCulture);
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_integer_attribute", new object[] { andRemoveAttribute.Name }), ex, andRemoveAttribute);
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0007CC50 File Offset: 0x0007BC50
		private static XmlNode GetAndRemovePositiveAttributeInternal(XmlNode node, string attrib, bool fRequired, ref int val)
		{
			XmlNode andRemoveIntegerAttributeInternal = HandlerBase.GetAndRemoveIntegerAttributeInternal(node, attrib, fRequired, ref val);
			if (andRemoveIntegerAttributeInternal != null && val <= 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_positive_integer_attribute", new object[] { attrib }), andRemoveIntegerAttributeInternal);
			}
			return andRemoveIntegerAttributeInternal;
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0007CC8D File Offset: 0x0007BC8D
		internal static XmlNode GetAndRemovePositiveIntegerAttribute(XmlNode node, string attrib, ref int val)
		{
			return HandlerBase.GetAndRemovePositiveAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0007CC98 File Offset: 0x0007BC98
		private static XmlNode GetAndRemoveTypeAttributeInternal(XmlNode node, string attrib, bool fRequired, ref Type val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = ConfigUtil.GetType(andRemoveAttribute.Value, andRemoveAttribute);
			}
			return andRemoveAttribute;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0007CCC0 File Offset: 0x0007BCC0
		internal static XmlNode GetAndRemoveTypeAttribute(XmlNode node, string attrib, ref Type val)
		{
			return HandlerBase.GetAndRemoveTypeAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0007CCCC File Offset: 0x0007BCCC
		internal static void CheckForbiddenAttribute(XmlNode node, string attrib)
		{
			XmlAttribute xmlAttribute = node.Attributes[attrib];
			if (xmlAttribute != null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { attrib }), xmlAttribute);
			}
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0007CD08 File Offset: 0x0007BD08
		internal static void CheckForUnrecognizedAttributes(XmlNode node)
		{
			if (node.Attributes.Count != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { node.Attributes[0].Name }), node.Attributes[0]);
			}
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0007CD5C File Offset: 0x0007BD5C
		internal static string RemoveAttribute(XmlNode node, string name)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode != null)
			{
				return xmlNode.Value;
			}
			return null;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0007CD81 File Offset: 0x0007BD81
		internal static string RemoveRequiredAttribute(XmlNode node, string name)
		{
			return HandlerBase.RemoveRequiredAttribute(node, name, false);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0007CD8C File Offset: 0x0007BD8C
		internal static string RemoveRequiredAttribute(XmlNode node, string name, bool allowEmpty)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_missing", new object[] { name }), node);
			}
			if (xmlNode.Value.Length == 0 && !allowEmpty)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_empty", new object[] { name }), node);
			}
			return xmlNode.Value;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0007CDFC File Offset: 0x0007BDFC
		internal static void CheckForNonCommentChildNodes(XmlNode node)
		{
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType != XmlNodeType.Comment)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_base_no_child_nodes"), xmlNode);
				}
			}
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0007CE68 File Offset: 0x0007BE68
		internal static void ThrowUnrecognizedElement(XmlNode node)
		{
			throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), node);
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0007CE7C File Offset: 0x0007BE7C
		internal static void CheckAssignableType(XmlNode node, Type baseType, Type type)
		{
			if (!baseType.IsAssignableFrom(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_doesnt_inherit_from_type", new object[] { type.FullName, baseType.FullName }), node);
			}
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x0007CEC0 File Offset: 0x0007BEC0
		internal static void CheckAssignableType(string filename, int lineNumber, Type baseType, Type type)
		{
			if (!baseType.IsAssignableFrom(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_doesnt_inherit_from_type", new object[] { type.FullName, baseType.FullName }), filename, lineNumber);
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0007CF02 File Offset: 0x0007BF02
		internal static bool IsServerConfiguration(object context)
		{
			return context is HttpConfigurationContext;
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0007CF10 File Offset: 0x0007BF10
		internal static bool CheckAndReadRegistryValue(ref string value, bool throwIfError)
		{
			if (value == null)
			{
				return true;
			}
			if (!StringUtil.StringStartsWithIgnoreCase(value, "registry:"))
			{
				return true;
			}
			StringBuilder stringBuilder = new StringBuilder(1024);
			if (UnsafeNativeMethods.GetCredentialFromRegistry(value, stringBuilder, 1024) == 0)
			{
				value = stringBuilder.ToString();
				return true;
			}
			if (throwIfError)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_registry_config"));
			}
			return false;
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0007CF70 File Offset: 0x0007BF70
		internal static bool CheckAndReadConnectionString(ref string connectionString, bool throwIfError)
		{
			ConnectionStringSettings connectionStringSettings = RuntimeConfig.GetConfig().ConnectionStrings.ConnectionStrings[connectionString];
			if (connectionStringSettings != null && connectionStringSettings.ConnectionString != null && connectionStringSettings.ConnectionString.Length > 0)
			{
				connectionString = connectionStringSettings.ConnectionString;
			}
			return HandlerBase.CheckAndReadRegistryValue(ref connectionString, throwIfError);
		}
	}
}
