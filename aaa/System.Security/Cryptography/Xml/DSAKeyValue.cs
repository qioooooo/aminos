using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200009E RID: 158
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class DSAKeyValue : KeyInfoClause
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x0000FCBE File Offset: 0x0000ECBE
		public DSAKeyValue()
		{
			this.m_key = DSA.Create();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000FCD1 File Offset: 0x0000ECD1
		public DSAKeyValue(DSA key)
		{
			this.m_key = key;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000FCE0 File Offset: 0x0000ECE0
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000FCE8 File Offset: 0x0000ECE8
		public DSA Key
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

		// Token: 0x060002FC RID: 764 RVA: 0x0000FCF4 File Offset: 0x0000ECF4
		public override XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000FD18 File Offset: 0x0000ED18
		internal override XmlElement GetXml(XmlDocument xmlDocument)
		{
			DSAParameters dsaparameters = this.m_key.ExportParameters(false);
			XmlElement xmlElement = xmlDocument.CreateElement("KeyValue", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement2 = xmlDocument.CreateElement("DSAKeyValue", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement3 = xmlDocument.CreateElement("P", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement3.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(dsaparameters.P)));
			xmlElement2.AppendChild(xmlElement3);
			XmlElement xmlElement4 = xmlDocument.CreateElement("Q", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement4.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(dsaparameters.Q)));
			xmlElement2.AppendChild(xmlElement4);
			XmlElement xmlElement5 = xmlDocument.CreateElement("G", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement5.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(dsaparameters.G)));
			xmlElement2.AppendChild(xmlElement5);
			XmlElement xmlElement6 = xmlDocument.CreateElement("Y", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement6.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(dsaparameters.Y)));
			xmlElement2.AppendChild(xmlElement6);
			if (dsaparameters.J != null)
			{
				XmlElement xmlElement7 = xmlDocument.CreateElement("J", "http://www.w3.org/2000/09/xmldsig#");
				xmlElement7.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(dsaparameters.J)));
				xmlElement2.AppendChild(xmlElement7);
			}
			if (dsaparameters.Seed != null)
			{
				XmlElement xmlElement8 = xmlDocument.CreateElement("Seed", "http://www.w3.org/2000/09/xmldsig#");
				xmlElement8.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(dsaparameters.Seed)));
				xmlElement2.AppendChild(xmlElement8);
				XmlElement xmlElement9 = xmlDocument.CreateElement("PgenCounter", "http://www.w3.org/2000/09/xmldsig#");
				xmlElement9.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(Utils.ConvertIntToByteArray(dsaparameters.Counter))));
				xmlElement2.AppendChild(xmlElement9);
			}
			xmlElement.AppendChild(xmlElement2);
			return xmlElement;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000FEE4 File Offset: 0x0000EEE4
		public override void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_key.FromXmlString(value.OuterXml);
		}

		// Token: 0x040004F7 RID: 1271
		private DSA m_key;
	}
}
