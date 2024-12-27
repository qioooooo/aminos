using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200004C RID: 76
	internal sealed class ConfigXmlElement : XmlElement, IConfigErrorInfo
	{
		// Token: 0x06000337 RID: 823 RVA: 0x0001226C File Offset: 0x0001126C
		public ConfigXmlElement(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc)
			: base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00012289 File Offset: 0x00011289
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00012291 File Offset: 0x00011291
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001229C File Offset: 0x0001129C
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlElement configXmlElement = xmlNode as ConfigXmlElement;
			if (configXmlElement != null)
			{
				configXmlElement._line = this._line;
				configXmlElement._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x040002BE RID: 702
		private int _line;

		// Token: 0x040002BF RID: 703
		private string _filename;
	}
}
