using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200004F RID: 79
	internal sealed class ConfigXmlText : XmlText, IConfigErrorInfo
	{
		// Token: 0x06000345 RID: 837 RVA: 0x000123CC File Offset: 0x000113CC
		public ConfigXmlText(string filename, int line, string strData, XmlDocument doc)
			: base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000123E5 File Offset: 0x000113E5
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000347 RID: 839 RVA: 0x000123ED File Offset: 0x000113ED
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000123F8 File Offset: 0x000113F8
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

		// Token: 0x040002C6 RID: 710
		private int _line;

		// Token: 0x040002C7 RID: 711
		private string _filename;
	}
}
