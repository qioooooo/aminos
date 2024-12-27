using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200004B RID: 75
	internal sealed class ConfigXmlComment : XmlComment, IConfigErrorInfo
	{
		// Token: 0x06000333 RID: 819 RVA: 0x00012208 File Offset: 0x00011208
		public ConfigXmlComment(string filename, int line, string comment, XmlDocument doc)
			: base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00012221 File Offset: 0x00011221
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00012229 File Offset: 0x00011229
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00012234 File Offset: 0x00011234
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlComment configXmlComment = xmlNode as ConfigXmlComment;
			if (configXmlComment != null)
			{
				configXmlComment._line = this._line;
				configXmlComment._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x040002BC RID: 700
		private int _line;

		// Token: 0x040002BD RID: 701
		private string _filename;
	}
}
