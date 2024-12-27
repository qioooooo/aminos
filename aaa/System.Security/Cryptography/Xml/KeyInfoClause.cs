using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200009C RID: 156
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public abstract class KeyInfoClause
	{
		// Token: 0x060002EE RID: 750
		public abstract XmlElement GetXml();

		// Token: 0x060002EF RID: 751 RVA: 0x0000FBEC File Offset: 0x0000EBEC
		internal virtual XmlElement GetXml(XmlDocument xmlDocument)
		{
			XmlElement xml = this.GetXml();
			return (XmlElement)xmlDocument.ImportNode(xml, true);
		}

		// Token: 0x060002F0 RID: 752
		public abstract void LoadXml(XmlElement element);
	}
}
