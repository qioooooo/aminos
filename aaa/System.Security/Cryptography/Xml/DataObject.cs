using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200009A RID: 154
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class DataObject
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000F643 File Offset: 0x0000E643
		public DataObject()
		{
			this.m_cachedXml = null;
			this.m_elData = new CanonicalXmlNodeList();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F660 File Offset: 0x0000E660
		public DataObject(string id, string mimeType, string encoding, XmlElement data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.m_id = id;
			this.m_mimeType = mimeType;
			this.m_encoding = encoding;
			this.m_elData = new CanonicalXmlNodeList();
			this.m_elData.Add(data);
			this.m_cachedXml = null;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000F6B7 File Offset: 0x0000E6B7
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000F6BF File Offset: 0x0000E6BF
		public string Id
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000F6CF File Offset: 0x0000E6CF
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000F6D7 File Offset: 0x0000E6D7
		public string MimeType
		{
			get
			{
				return this.m_mimeType;
			}
			set
			{
				this.m_mimeType = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000F6E7 File Offset: 0x0000E6E7
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000F6EF File Offset: 0x0000E6EF
		public string Encoding
		{
			get
			{
				return this.m_encoding;
			}
			set
			{
				this.m_encoding = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000F6FF File Offset: 0x0000E6FF
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000F708 File Offset: 0x0000E708
		public XmlNodeList Data
		{
			get
			{
				return this.m_elData;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_elData = new CanonicalXmlNodeList();
				foreach (object obj in value)
				{
					XmlNode xmlNode = (XmlNode)obj;
					this.m_elData.Add(xmlNode);
				}
				this.m_cachedXml = null;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000F784 File Offset: 0x0000E784
		private bool CacheValid
		{
			get
			{
				return this.m_cachedXml != null;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F794 File Offset: 0x0000E794
		public XmlElement GetXml()
		{
			if (this.CacheValid)
			{
				return this.m_cachedXml;
			}
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F7C4 File Offset: 0x0000E7C4
		internal XmlElement GetXml(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("Object", "http://www.w3.org/2000/09/xmldsig#");
			if (!string.IsNullOrEmpty(this.m_id))
			{
				xmlElement.SetAttribute("Id", this.m_id);
			}
			if (!string.IsNullOrEmpty(this.m_mimeType))
			{
				xmlElement.SetAttribute("MimeType", this.m_mimeType);
			}
			if (!string.IsNullOrEmpty(this.m_encoding))
			{
				xmlElement.SetAttribute("Encoding", this.m_encoding);
			}
			if (this.m_elData != null)
			{
				foreach (object obj in this.m_elData)
				{
					XmlNode xmlNode = (XmlNode)obj;
					xmlElement.AppendChild(document.ImportNode(xmlNode, true));
				}
			}
			return xmlElement;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F89C File Offset: 0x0000E89C
		public void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_id = Utils.GetAttribute(value, "Id", "http://www.w3.org/2000/09/xmldsig#");
			this.m_mimeType = Utils.GetAttribute(value, "MimeType", "http://www.w3.org/2000/09/xmldsig#");
			this.m_encoding = Utils.GetAttribute(value, "Encoding", "http://www.w3.org/2000/09/xmldsig#");
			foreach (object obj in value.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				this.m_elData.Add(xmlNode);
			}
			this.m_cachedXml = value;
		}

		// Token: 0x040004EF RID: 1263
		private string m_id;

		// Token: 0x040004F0 RID: 1264
		private string m_mimeType;

		// Token: 0x040004F1 RID: 1265
		private string m_encoding;

		// Token: 0x040004F2 RID: 1266
		private CanonicalXmlNodeList m_elData;

		// Token: 0x040004F3 RID: 1267
		private XmlElement m_cachedXml;
	}
}
