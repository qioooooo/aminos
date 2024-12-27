using System;
using System.Collections;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200071C RID: 1820
	public class SingleTagSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x060037A9 RID: 14249 RVA: 0x000EBD94 File Offset: 0x000EAD94
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable hashtable;
			if (parent == null)
			{
				hashtable = new Hashtable();
			}
			else
			{
				hashtable = new Hashtable((IDictionary)parent);
			}
			HandlerBase.CheckForChildNodes(section);
			foreach (object obj in section.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				hashtable[xmlAttribute.Name] = xmlAttribute.Value;
			}
			return hashtable;
		}
	}
}
