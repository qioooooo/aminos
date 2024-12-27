using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F4 RID: 1780
	internal sealed class ConfigXmlText : XmlText, IConfigErrorInfo
	{
		// Token: 0x060036EB RID: 14059 RVA: 0x000E9D24 File Offset: 0x000E8D24
		public ConfigXmlText(string filename, int line, string strData, XmlDocument doc)
			: base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x060036EC RID: 14060 RVA: 0x000E9D3D File Offset: 0x000E8D3D
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x060036ED RID: 14061 RVA: 0x000E9D45 File Offset: 0x000E8D45
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000E9D50 File Offset: 0x000E8D50
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlText configXmlText = xmlNode as ConfigXmlText;
			if (configXmlText != null)
			{
				configXmlText._line = this._line;
				configXmlText._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x040031A5 RID: 12709
		private int _line;

		// Token: 0x040031A6 RID: 12710
		private string _filename;
	}
}
