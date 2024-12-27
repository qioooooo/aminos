using System;
using System.Collections;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200009B RID: 155
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class KeyInfo : IEnumerable
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000F954 File Offset: 0x0000E954
		public KeyInfo()
		{
			this.m_KeyInfoClauses = new ArrayList();
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000F967 File Offset: 0x0000E967
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000F96F File Offset: 0x0000E96F
		public string Id
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F978 File Offset: 0x0000E978
		public XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F99C File Offset: 0x0000E99C
		internal XmlElement GetXml(XmlDocument xmlDocument)
		{
			XmlElement xmlElement = xmlDocument.CreateElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#");
			if (!string.IsNullOrEmpty(this.m_id))
			{
				xmlElement.SetAttribute("Id", this.m_id);
			}
			for (int i = 0; i < this.m_KeyInfoClauses.Count; i++)
			{
				XmlElement xml = ((KeyInfoClause)this.m_KeyInfoClauses[i]).GetXml(xmlDocument);
				if (xml != null)
				{
					xmlElement.AppendChild(xml);
				}
			}
			return xmlElement;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000FA14 File Offset: 0x0000EA14
		public void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_id = Utils.GetAttribute(value, "Id", "http://www.w3.org/2000/09/xmldsig#");
			if (!Utils.VerifyAttributes(value, "Id"))
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "KeyInfo");
			}
			for (XmlNode xmlNode = value.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null)
				{
					string text = xmlElement.NamespaceURI + " " + xmlElement.LocalName;
					if (text == "http://www.w3.org/2000/09/xmldsig# KeyValue")
					{
						if (!Utils.VerifyAttributes(xmlElement, null))
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "KeyInfo/KeyValue");
						}
						XmlNodeList childNodes = xmlElement.ChildNodes;
						foreach (object obj in childNodes)
						{
							XmlNode xmlNode2 = (XmlNode)obj;
							XmlElement xmlElement2 = xmlNode2 as XmlElement;
							if (xmlElement2 != null)
							{
								text = text + "/" + xmlElement2.LocalName;
								break;
							}
						}
					}
					KeyInfoClause keyInfoClause = Utils.CreateFromName<KeyInfoClause>(text);
					if (keyInfoClause == null)
					{
						keyInfoClause = new KeyInfoNode();
					}
					keyInfoClause.LoadXml(xmlElement);
					this.AddClause(keyInfoClause);
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000FB6C File Offset: 0x0000EB6C
		public int Count
		{
			get
			{
				return this.m_KeyInfoClauses.Count;
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000FB79 File Offset: 0x0000EB79
		public void AddClause(KeyInfoClause clause)
		{
			this.m_KeyInfoClauses.Add(clause);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000FB88 File Offset: 0x0000EB88
		public IEnumerator GetEnumerator()
		{
			return this.m_KeyInfoClauses.GetEnumerator();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000FB98 File Offset: 0x0000EB98
		public IEnumerator GetEnumerator(Type requestedObjectType)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.m_KeyInfoClauses)
			{
				if (requestedObjectType.Equals(obj.GetType()))
				{
					arrayList.Add(obj);
				}
			}
			return arrayList.GetEnumerator();
		}

		// Token: 0x040004F4 RID: 1268
		private string m_id;

		// Token: 0x040004F5 RID: 1269
		private ArrayList m_KeyInfoClauses;
	}
}
