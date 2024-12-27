using System;
using System.Configuration;
using System.Xml;

namespace System.DirectoryServices
{
	// Token: 0x02000041 RID: 65
	internal class SearchWaitHandler : IConfigurationSectionHandler
	{
		// Token: 0x060001DA RID: 474 RVA: 0x00007B4C File Offset: 0x00006B4C
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string name;
				if ((name = xmlNode.Name) != null && name == "DirectorySearcher")
				{
					if (flag)
					{
						throw new ConfigurationErrorsException(Res.GetString("ConfigSectionsUnique", new object[] { "DirectorySearcher" }));
					}
					HandlerBase.RemoveBooleanAttribute(xmlNode, "waitForPagedSearchData", ref flag2);
					flag = true;
				}
			}
			return flag2;
		}
	}
}
