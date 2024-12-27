using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006EE RID: 1774
	internal sealed class ConfigXmlAttribute : XmlAttribute, IConfigErrorInfo
	{
		// Token: 0x060036C9 RID: 14025 RVA: 0x000E9958 File Offset: 0x000E8958
		public ConfigXmlAttribute(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc)
			: base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x060036CA RID: 14026 RVA: 0x000E9975 File Offset: 0x000E8975
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x060036CB RID: 14027 RVA: 0x000E997D File Offset: 0x000E897D
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000E9988 File Offset: 0x000E8988
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlAttribute configXmlAttribute = xmlNode as ConfigXmlAttribute;
			if (configXmlAttribute != null)
			{
				configXmlAttribute._line = this._line;
				configXmlAttribute._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04003198 RID: 12696
		private int _line;

		// Token: 0x04003199 RID: 12697
		private string _filename;
	}
}
