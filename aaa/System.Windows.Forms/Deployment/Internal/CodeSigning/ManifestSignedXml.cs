using System;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000139 RID: 313
	internal class ManifestSignedXml : SignedXml
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x000098D6 File Offset: 0x000088D6
		internal ManifestSignedXml()
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000098DE File Offset: 0x000088DE
		internal ManifestSignedXml(XmlElement elem)
			: base(elem)
		{
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000098E7 File Offset: 0x000088E7
		internal ManifestSignedXml(XmlDocument document)
			: base(document)
		{
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000098F0 File Offset: 0x000088F0
		internal ManifestSignedXml(XmlDocument document, bool verify)
			: base(document)
		{
			this.m_verify = verify;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00009900 File Offset: 0x00008900
		private static XmlElement FindIdElement(XmlElement context, string idValue)
		{
			if (context == null)
			{
				return null;
			}
			XmlElement xmlElement = context.SelectSingleNode("//*[@Id=\"" + idValue + "\"]") as XmlElement;
			if (xmlElement != null)
			{
				return xmlElement;
			}
			xmlElement = context.SelectSingleNode("//*[@id=\"" + idValue + "\"]") as XmlElement;
			if (xmlElement != null)
			{
				return xmlElement;
			}
			return context.SelectSingleNode("//*[@ID=\"" + idValue + "\"]") as XmlElement;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00009970 File Offset: 0x00008970
		public override XmlElement GetIdElement(XmlDocument document, string idValue)
		{
			if (this.m_verify)
			{
				return base.GetIdElement(document, idValue);
			}
			KeyInfo keyInfo = base.KeyInfo;
			if (keyInfo.Id != idValue)
			{
				return null;
			}
			return keyInfo.GetXml();
		}

		// Token: 0x04000EB3 RID: 3763
		private bool m_verify;
	}
}
