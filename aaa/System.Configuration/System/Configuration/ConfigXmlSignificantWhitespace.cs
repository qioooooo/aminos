using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200004E RID: 78
	internal sealed class ConfigXmlSignificantWhitespace : XmlSignificantWhitespace, IConfigErrorInfo
	{
		// Token: 0x06000341 RID: 833 RVA: 0x0001236A File Offset: 0x0001136A
		public ConfigXmlSignificantWhitespace(string filename, int line, string strData, XmlDocument doc)
			: base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00012383 File Offset: 0x00011383
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0001238B File Offset: 0x0001138B
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00012394 File Offset: 0x00011394
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlSignificantWhitespace configXmlSignificantWhitespace = xmlNode as ConfigXmlSignificantWhitespace;
			if (configXmlSignificantWhitespace != null)
			{
				configXmlSignificantWhitespace._line = this._line;
				configXmlSignificantWhitespace._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x040002C4 RID: 708
		private int _line;

		// Token: 0x040002C5 RID: 709
		private string _filename;
	}
}
