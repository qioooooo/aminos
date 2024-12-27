using System;
using System.Collections;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F6 RID: 1782
	public class DictionarySectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x060036F3 RID: 14067 RVA: 0x000E9DEC File Offset: 0x000E8DEC
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable hashtable;
			if (parent == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				hashtable = (Hashtable)((Hashtable)parent).Clone();
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "add")
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						string text = HandlerBase.RemoveRequiredAttribute(xmlNode, this.KeyAttributeName);
						string text2;
						if (this.ValueRequired)
						{
							text2 = HandlerBase.RemoveRequiredAttribute(xmlNode, this.ValueAttributeName);
						}
						else
						{
							text2 = HandlerBase.RemoveAttribute(xmlNode, this.ValueAttributeName);
						}
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						if (text2 == null)
						{
							text2 = "";
						}
						hashtable[text] = text2;
					}
					else if (xmlNode.Name == "remove")
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						string text3 = HandlerBase.RemoveRequiredAttribute(xmlNode, this.KeyAttributeName);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						hashtable.Remove(text3);
					}
					else if (xmlNode.Name.Equals("clear"))
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						hashtable.Clear();
					}
					else
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
				}
			}
			return hashtable;
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060036F4 RID: 14068 RVA: 0x000E9F44 File Offset: 0x000E8F44
		protected virtual string KeyAttributeName
		{
			get
			{
				return "key";
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060036F5 RID: 14069 RVA: 0x000E9F4B File Offset: 0x000E8F4B
		protected virtual string ValueAttributeName
		{
			get
			{
				return "value";
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060036F6 RID: 14070 RVA: 0x000E9F52 File Offset: 0x000E8F52
		internal virtual bool ValueRequired
		{
			get
			{
				return false;
			}
		}
	}
}
