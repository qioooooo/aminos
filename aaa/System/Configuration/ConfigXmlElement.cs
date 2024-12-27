using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F2 RID: 1778
	internal sealed class ConfigXmlElement : XmlElement, IConfigErrorInfo
	{
		// Token: 0x060036E3 RID: 14051 RVA: 0x000E9C5B File Offset: 0x000E8C5B
		public ConfigXmlElement(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc)
			: base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060036E4 RID: 14052 RVA: 0x000E9C78 File Offset: 0x000E8C78
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x060036E5 RID: 14053 RVA: 0x000E9C80 File Offset: 0x000E8C80
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000E9C88 File Offset: 0x000E8C88
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

		// Token: 0x040031A1 RID: 12705
		private int _line;

		// Token: 0x040031A2 RID: 12706
		private string _filename;
	}
}
