using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000A0 RID: 160
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class KeyInfoRetrievalMethod : KeyInfoClause
	{
		// Token: 0x06000306 RID: 774 RVA: 0x00010029 File Offset: 0x0000F029
		public KeyInfoRetrievalMethod()
		{
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010031 File Offset: 0x0000F031
		public KeyInfoRetrievalMethod(string strUri)
		{
			this.m_uri = strUri;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010040 File Offset: 0x0000F040
		public KeyInfoRetrievalMethod(string strUri, string typeName)
		{
			this.m_uri = strUri;
			this.m_type = typeName;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00010056 File Offset: 0x0000F056
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0001005E File Offset: 0x0000F05E
		public string Uri
		{
			get
			{
				return this.m_uri;
			}
			set
			{
				this.m_uri = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00010067 File Offset: 0x0000F067
		// (set) Token: 0x0600030C RID: 780 RVA: 0x0001006F File Offset: 0x0000F06F
		[ComVisible(false)]
		public string Type
		{
			get
			{
				return this.m_type;
			}
			set
			{
				this.m_type = value;
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00010078 File Offset: 0x0000F078
		public override XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0001009C File Offset: 0x0000F09C
		internal override XmlElement GetXml(XmlDocument xmlDocument)
		{
			XmlElement xmlElement = xmlDocument.CreateElement("RetrievalMethod", "http://www.w3.org/2000/09/xmldsig#");
			if (!string.IsNullOrEmpty(this.m_uri))
			{
				xmlElement.SetAttribute("URI", this.m_uri);
			}
			if (!string.IsNullOrEmpty(this.m_type))
			{
				xmlElement.SetAttribute("Type", this.m_type);
			}
			return xmlElement;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000100F7 File Offset: 0x0000F0F7
		public override void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_uri = Utils.GetAttribute(value, "URI", "http://www.w3.org/2000/09/xmldsig#");
			this.m_type = Utils.GetAttribute(value, "Type", "http://www.w3.org/2000/09/xmldsig#");
		}

		// Token: 0x040004F9 RID: 1273
		private string m_uri;

		// Token: 0x040004FA RID: 1274
		private string m_type;
	}
}
