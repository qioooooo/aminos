using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace System.Data
{
	// Token: 0x0200019C RID: 412
	internal sealed class AllowedTypesSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06001843 RID: 6211 RVA: 0x00236990 File Offset: 0x00235D90
		public object Create(object parent, object configContext, XmlNode section)
		{
			XmlAttribute xmlAttribute = section.Attributes["auditOnly"];
			bool flag = false;
			if (xmlAttribute != null)
			{
				bool.TryParse(xmlAttribute.Value, out flag);
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode != null && xmlNode.Attributes != null)
				{
					string name = xmlNode.Name;
					XmlAttribute xmlAttribute2 = xmlNode.Attributes["type"];
					string text = ((xmlAttribute2 == null) ? null : xmlAttribute2.Value);
					if (name == "add")
					{
						dictionary[text] = null;
					}
					else if (name == "remove")
					{
						dictionary.Remove(text);
					}
					else
					{
						if (!(name == "clear"))
						{
							throw ExceptionBuilder.ConfigElementNotAllowed(xmlNode);
						}
						dictionary.Clear();
					}
				}
			}
			return new AllowedTypesSectionHandler.Data
			{
				AuditMode = flag,
				AllowedTypes = dictionary.Keys
			};
		}

		// Token: 0x0200019D RID: 413
		internal sealed class Data
		{
			// Token: 0x04000D0E RID: 3342
			internal bool AuditMode;

			// Token: 0x04000D0F RID: 3343
			internal IEnumerable<string> AllowedTypes = new List<string>();
		}
	}
}
