using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000050 RID: 80
	internal sealed class ConfigXmlWhitespace : XmlWhitespace, IConfigErrorInfo
	{
		// Token: 0x06000349 RID: 841 RVA: 0x00012430 File Offset: 0x00011430
		public ConfigXmlWhitespace(string filename, int line, string comment, XmlDocument doc)
			: base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00012449 File Offset: 0x00011449
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00012451 File Offset: 0x00011451
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001245C File Offset: 0x0001145C
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

		// Token: 0x040002C8 RID: 712
		private int _line;

		// Token: 0x040002C9 RID: 713
		private string _filename;
	}
}
