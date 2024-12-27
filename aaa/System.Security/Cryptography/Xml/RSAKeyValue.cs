using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200009F RID: 159
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class RSAKeyValue : KeyInfoClause
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0000FF05 File Offset: 0x0000EF05
		public RSAKeyValue()
		{
			this.m_key = RSA.Create();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000FF18 File Offset: 0x0000EF18
		public RSAKeyValue(RSA key)
		{
			this.m_key = key;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000FF27 File Offset: 0x0000EF27
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000FF2F File Offset: 0x0000EF2F
		public RSA Key
		{
			get
			{
				return this.m_key;
			}
			set
			{
				this.m_key = value;
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000FF38 File Offset: 0x0000EF38
		public override XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000FF5C File Offset: 0x0000EF5C
		internal override XmlElement GetXml(XmlDocument xmlDocument)
		{
			RSAParameters rsaparameters = this.m_key.ExportParameters(false);
			XmlElement xmlElement = xmlDocument.CreateElement("KeyValue", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement2 = xmlDocument.CreateElement("RSAKeyValue", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement3 = xmlDocument.CreateElement("Modulus", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement3.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(rsaparameters.Modulus)));
			xmlElement2.AppendChild(xmlElement3);
			XmlElement xmlElement4 = xmlDocument.CreateElement("Exponent", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement4.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(rsaparameters.Exponent)));
			xmlElement2.AppendChild(xmlElement4);
			xmlElement.AppendChild(xmlElement2);
			return xmlElement;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00010008 File Offset: 0x0000F008
		public override void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_key.FromXmlString(value.OuterXml);
		}

		// Token: 0x040004F8 RID: 1272
		private RSA m_key;
	}
}
