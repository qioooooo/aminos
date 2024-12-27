using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000049 RID: 73
	internal sealed class ConfigXmlAttribute : XmlAttribute, IConfigErrorInfo
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0001213C File Offset: 0x0001113C
		public ConfigXmlAttribute(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc)
			: base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00012159 File Offset: 0x00011159
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00012161 File Offset: 0x00011161
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0001216C File Offset: 0x0001116C
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

		// Token: 0x040002B8 RID: 696
		private int _line;

		// Token: 0x040002B9 RID: 697
		private string _filename;
	}
}
