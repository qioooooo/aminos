using System;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x020001E1 RID: 481
	internal class ManifestSignedXml : SignedXml
	{
		// Token: 0x0600087B RID: 2171 RVA: 0x00021174 File Offset: 0x00020174
		internal ManifestSignedXml()
		{
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0002117C File Offset: 0x0002017C
		internal ManifestSignedXml(XmlElement elem)
			: base(elem)
		{
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00021185 File Offset: 0x00020185
		internal ManifestSignedXml(XmlDocument document)
			: base(document)
		{
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0002118E File Offset: 0x0002018E
		internal ManifestSignedXml(XmlDocument document, bool verify)
			: base(document)
		{
			this.m_verify = verify;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000211A0 File Offset: 0x000201A0
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

		// Token: 0x06000880 RID: 2176 RVA: 0x00021210 File Offset: 0x00020210
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

		// Token: 0x04000810 RID: 2064
		private bool m_verify;
	}
}
