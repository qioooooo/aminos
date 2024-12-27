using System;
using System.Globalization;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F7 RID: 1783
	internal class HandlerBase
	{
		// Token: 0x060036F8 RID: 14072 RVA: 0x000E9F5D File Offset: 0x000E8F5D
		private HandlerBase()
		{
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x000E9F68 File Offset: 0x000E8F68
		private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attrib);
			if (fRequired && xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_missing_required_attribute", new object[] { attrib, node.Name }), node);
			}
			return xmlNode;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x000E9FB0 File Offset: 0x000E8FB0
		private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = andRemoveAttribute.Value;
			}
			return andRemoveAttribute;
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000E9FD2 File Offset: 0x000E8FD2
		internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x000E9FE0 File Offset: 0x000E8FE0
		private static XmlNode GetAndRemoveBooleanAttributeInternal(XmlNode node, string attrib, bool fRequired, ref bool val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				try
				{
					val = bool.Parse(andRemoveAttribute.Value);
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString(SR.GetString("Config_invalid_boolean_attribute", new object[] { andRemoveAttribute.Name })), ex, andRemoveAttribute);
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x000EA044 File Offset: 0x000E9044
		internal static XmlNode GetAndRemoveBooleanAttribute(XmlNode node, string attrib, ref bool val)
		{
			return HandlerBase.GetAndRemoveBooleanAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x000EA050 File Offset: 0x000E9050
		private static XmlNode GetAndRemoveIntegerAttributeInternal(XmlNode node, string attrib, bool fRequired, ref int val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				if (andRemoveAttribute.Value.Trim() != andRemoveAttribute.Value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_integer_attribute", new object[] { andRemoveAttribute.Name }), andRemoveAttribute);
				}
				try
				{
					val = int.Parse(andRemoveAttribute.Value, CultureInfo.InvariantCulture);
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_integer_attribute", new object[] { andRemoveAttribute.Name }), ex, andRemoveAttribute);
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x000EA0EC File Offset: 0x000E90EC
		internal static XmlNode GetAndRemoveIntegerAttribute(XmlNode node, string attrib, ref int val)
		{
			return HandlerBase.GetAndRemoveIntegerAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x000EA0F8 File Offset: 0x000E90F8
		internal static void CheckForUnrecognizedAttributes(XmlNode node)
		{
			if (node.Attributes.Count != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { node.Attributes[0].Name }), node);
			}
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x000EA140 File Offset: 0x000E9140
		internal static string RemoveAttribute(XmlNode node, string name)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode != null)
			{
				return xmlNode.Value;
			}
			return null;
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x000EA165 File Offset: 0x000E9165
		internal static string RemoveRequiredAttribute(XmlNode node, string name)
		{
			return HandlerBase.RemoveRequiredAttribute(node, name, false);
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x000EA170 File Offset: 0x000E9170
		internal static string RemoveRequiredAttribute(XmlNode node, string name, bool allowEmpty)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_missing", new object[] { name }), node);
			}
			if (string.IsNullOrEmpty(xmlNode.Value) && !allowEmpty)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_empty", new object[] { name }), node);
			}
			return xmlNode.Value;
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x000EA1DD File Offset: 0x000E91DD
		internal static void CheckForNonElement(XmlNode node)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_elements_only"), node);
			}
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x000EA1F9 File Offset: 0x000E91F9
		internal static bool IsIgnorableAlsoCheckForNonElement(XmlNode node)
		{
			if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.Whitespace)
			{
				return true;
			}
			HandlerBase.CheckForNonElement(node);
			return false;
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x000EA217 File Offset: 0x000E9217
		internal static void CheckForChildNodes(XmlNode node)
		{
			if (node.HasChildNodes)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_no_child_nodes"), node.FirstChild);
			}
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x000EA237 File Offset: 0x000E9237
		internal static void ThrowUnrecognizedElement(XmlNode node)
		{
			throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), node);
		}
	}
}
