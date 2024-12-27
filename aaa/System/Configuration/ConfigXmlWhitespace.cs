using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F5 RID: 1781
	internal sealed class ConfigXmlWhitespace : XmlWhitespace, IConfigErrorInfo
	{
		// Token: 0x060036EF RID: 14063 RVA: 0x000E9D88 File Offset: 0x000E8D88
		public ConfigXmlWhitespace(string filename, int line, string comment, XmlDocument doc)
			: base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x060036F0 RID: 14064 RVA: 0x000E9DA1 File Offset: 0x000E8DA1
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060036F1 RID: 14065 RVA: 0x000E9DA9 File Offset: 0x000E8DA9
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x000E9DB4 File Offset: 0x000E8DB4
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlWhitespace configXmlWhitespace = xmlNode as ConfigXmlWhitespace;
			if (configXmlWhitespace != null)
			{
				configXmlWhitespace._line = this._line;
				configXmlWhitespace._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x040031A7 RID: 12711
		private int _line;

		// Token: 0x040031A8 RID: 12712
		private string _filename;
	}
}
