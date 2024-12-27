using System;
using System.Configuration;
using System.Globalization;
using System.Xml;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001E9 RID: 489
	internal static class HandlerBase
	{
		// Token: 0x0600100E RID: 4110 RVA: 0x000351DC File Offset: 0x000341DC
		private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attrib);
			if (fRequired && xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_missing_required_attribute", new object[] { attrib, node.Name }), node);
			}
			return xmlNode;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00035224 File Offset: 0x00034224
		private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = andRemoveAttribute.Value;
			}
			return andRemoveAttribute;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00035246 File Offset: 0x00034246
		internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00035251 File Offset: 0x00034251
		internal static XmlNode GetAndRemoveRequiredNonEmptyStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveNonEmptyStringAttributeInternal(node, attrib, true, ref val);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003525C File Offset: 0x0003425C
		private static XmlNode GetAndRemoveNonEmptyStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveStringAttributeInternal = HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, fRequired, ref val);
			if (andRemoveStringAttributeInternal != null && val.Length == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Empty_attribute", new object[] { attrib }), andRemoveStringAttributeInternal);
			}
			return andRemoveStringAttributeInternal;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000352A0 File Offset: 0x000342A0
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

		// Token: 0x06001014 RID: 4116 RVA: 0x0003533C File Offset: 0x0003433C
		private static XmlNode GetAndRemoveNonNegativeAttributeInternal(XmlNode node, string attrib, bool fRequired, ref int val)
		{
			XmlNode andRemoveIntegerAttributeInternal = HandlerBase.GetAndRemoveIntegerAttributeInternal(node, attrib, fRequired, ref val);
			if (andRemoveIntegerAttributeInternal != null && val < 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_nonnegative_integer_attribute", new object[] { attrib }), andRemoveIntegerAttributeInternal);
			}
			return andRemoveIntegerAttributeInternal;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00035379 File Offset: 0x00034379
		internal static XmlNode GetAndRemoveNonNegativeIntegerAttribute(XmlNode node, string attrib, ref int val)
		{
			return HandlerBase.GetAndRemoveNonNegativeAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00035384 File Offset: 0x00034384
		internal static void CheckForUnrecognizedAttributes(XmlNode node)
		{
			if (node.Attributes.Count != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { node.Attributes[0].Name }), node.Attributes[0]);
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x000353D6 File Offset: 0x000343D6
		internal static void CheckForNonElement(XmlNode node)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_elements_only"), node);
			}
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x000353F2 File Offset: 0x000343F2
		internal static bool IsIgnorableAlsoCheckForNonElement(XmlNode node)
		{
			if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.Whitespace)
			{
				return true;
			}
			HandlerBase.CheckForNonElement(node);
			return false;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00035410 File Offset: 0x00034410
		internal static void CheckForChildNodes(XmlNode node)
		{
			if (node.HasChildNodes)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_no_child_nodes"), node.FirstChild);
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00035430 File Offset: 0x00034430
		internal static void ThrowUnrecognizedElement(XmlNode node)
		{
			throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), node);
		}
	}
}
