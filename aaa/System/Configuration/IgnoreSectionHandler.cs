using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006FA RID: 1786
	public class IgnoreSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x0600370D RID: 14093 RVA: 0x000EA249 File Offset: 0x000E9249
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			return null;
		}
	}
}
