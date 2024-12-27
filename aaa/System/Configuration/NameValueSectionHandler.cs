using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000701 RID: 1793
	public class NameValueSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06003734 RID: 14132 RVA: 0x000EADB4 File Offset: 0x000E9DB4
		public object Create(object parent, object context, XmlNode section)
		{
			return NameValueSectionHandler.CreateStatic(parent, section, this.KeyAttributeName, this.ValueAttributeName);
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x000EADC9 File Offset: 0x000E9DC9
		internal static object CreateStatic(object parent, XmlNode section)
		{
			return NameValueSectionHandler.CreateStatic(parent, section, "key", "value");
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x000EADDC File Offset: 0x000E9DDC
		internal static object CreateStatic(object parent, XmlNode section, string keyAttriuteName, string valueAttributeName)
		{
			ReadOnlyNameValueCollection readOnlyNameValueCollection;
			if (parent == null)
			{
				readOnlyNameValueCollection = new ReadOnlyNameValueCollection(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				ReadOnlyNameValueCollection readOnlyNameValueCollection2 = (ReadOnlyNameValueCollection)parent;
				readOnlyNameValueCollection = new ReadOnlyNameValueCollection(readOnlyNameValueCollection2);
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "add")
					{
						string text = HandlerBase.RemoveRequiredAttribute(xmlNode, keyAttriuteName);
						string text2 = HandlerBase.RemoveRequiredAttribute(xmlNode, valueAttributeName, true);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection[text] = text2;
					}
					else if (xmlNode.Name == "remove")
					{
						string text3 = HandlerBase.RemoveRequiredAttribute(xmlNode, keyAttriuteName);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection.Remove(text3);
					}
					else if (xmlNode.Name.Equals("clear"))
					{
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection.Clear();
					}
					else
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
				}
			}
			readOnlyNameValueCollection.SetReadOnly();
			return readOnlyNameValueCollection;
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x000EAEF8 File Offset: 0x000E9EF8
		protected virtual string KeyAttributeName
		{
			get
			{
				return "key";
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06003738 RID: 14136 RVA: 0x000EAEFF File Offset: 0x000E9EFF
		protected virtual string ValueAttributeName
		{
			get
			{
				return "value";
			}
		}

		// Token: 0x040031B0 RID: 12720
		private const string defaultKeyAttribute = "key";

		// Token: 0x040031B1 RID: 12721
		private const string defaultValueAttribute = "value";
	}
}
