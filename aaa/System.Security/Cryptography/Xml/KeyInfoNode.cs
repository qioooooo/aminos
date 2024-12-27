using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000A4 RID: 164
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class KeyInfoNode : KeyInfoClause
	{
		// Token: 0x06000330 RID: 816 RVA: 0x00010AF0 File Offset: 0x0000FAF0
		public KeyInfoNode()
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00010AF8 File Offset: 0x0000FAF8
		public KeyInfoNode(XmlElement node)
		{
			this.m_node = node;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00010B07 File Offset: 0x0000FB07
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00010B0F File Offset: 0x0000FB0F
		public XmlElement Value
		{
			get
			{
				return this.m_node;
			}
			set
			{
				this.m_node = value;
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00010B18 File Offset: 0x0000FB18
		public override XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00010B39 File Offset: 0x0000FB39
		internal override XmlElement GetXml(XmlDocument xmlDocument)
		{
			return xmlDocument.ImportNode(this.m_node, true) as XmlElement;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00010B4D File Offset: 0x0000FB4D
		public override void LoadXml(XmlElement value)
		{
			this.m_node = value;
		}

		// Token: 0x04000503 RID: 1283
		private XmlElement m_node;
	}
}
