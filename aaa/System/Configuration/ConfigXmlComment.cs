using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F0 RID: 1776
	internal sealed class ConfigXmlComment : XmlComment, IConfigErrorInfo
	{
		// Token: 0x060036D1 RID: 14033 RVA: 0x000E9A24 File Offset: 0x000E8A24
		public ConfigXmlComment(string filename, int line, string comment, XmlDocument doc)
			: base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x000E9A3D File Offset: 0x000E8A3D
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x060036D3 RID: 14035 RVA: 0x000E9A45 File Offset: 0x000E8A45
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x000E9A50 File Offset: 0x000E8A50
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

		// Token: 0x0400319C RID: 12700
		private int _line;

		// Token: 0x0400319D RID: 12701
		private string _filename;
	}
}
