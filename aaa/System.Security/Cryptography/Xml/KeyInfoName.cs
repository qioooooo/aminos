using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200009D RID: 157
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class KeyInfoName : KeyInfoClause
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000FC0D File Offset: 0x0000EC0D
		public KeyInfoName()
			: this(null)
		{
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000FC16 File Offset: 0x0000EC16
		public KeyInfoName(string keyName)
		{
			this.Value = keyName;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000FC25 File Offset: 0x0000EC25
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x0000FC2D File Offset: 0x0000EC2D
		public string Value
		{
			get
			{
				return this.m_keyName;
			}
			set
			{
				this.m_keyName = value;
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FC38 File Offset: 0x0000EC38
		public override XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000FC5C File Offset: 0x0000EC5C
		internal override XmlElement GetXml(XmlDocument xmlDocument)
		{
			XmlElement xmlElement = xmlDocument.CreateElement("KeyName", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement.AppendChild(xmlDocument.CreateTextNode(this.m_keyName));
			return xmlElement;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FC90 File Offset: 0x0000EC90
		public override void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_keyName = value.InnerText.Trim();
		}

		// Token: 0x040004F6 RID: 1270
		private string m_keyName;
	}
}
