using System;
using System.Collections;
using System.Configuration;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x0200022F RID: 559
	internal class ProtocolsConfiguration
	{
		// Token: 0x06001E19 RID: 7705 RVA: 0x00087130 File Offset: 0x00086130
		internal ProtocolsConfiguration(XmlNode section)
		{
			HandlerBase.CheckForUnrecognizedAttributes(section);
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!this.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "add")
					{
						string text = HandlerBase.RemoveRequiredAttribute(xmlNode, "id");
						string text2 = HandlerBase.RemoveRequiredAttribute(xmlNode, "processHandlerType");
						string text3 = HandlerBase.RemoveRequiredAttribute(xmlNode, "appDomainHandlerType");
						bool flag = true;
						HandlerBase.GetAndRemoveBooleanAttribute(xmlNode, "validate", ref flag);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						HandlerBase.CheckForNonCommentChildNodes(xmlNode);
						try
						{
							this._protocolEntries[text] = new ProtocolsConfigurationEntry(text, text2, text3, flag, ConfigurationErrorsException.GetFilename(xmlNode), ConfigurationErrorsException.GetLineNumber(xmlNode));
							continue;
						}
						catch
						{
							continue;
						}
					}
					HandlerBase.ThrowUnrecognizedElement(xmlNode);
				}
			}
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x00087240 File Offset: 0x00086240
		private bool IsIgnorableAlsoCheckForNonElement(XmlNode node)
		{
			if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.Whitespace)
			{
				return true;
			}
			if (node.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_elements_only"), node);
			}
			return false;
		}

		// Token: 0x040019A3 RID: 6563
		private Hashtable _protocolEntries = new Hashtable();
	}
}
