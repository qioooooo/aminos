using System;
using System.Configuration.Internal;
using System.IO;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200004D RID: 77
	internal sealed class ConfigXmlReader : XmlTextReader, IConfigErrorInfo
	{
		// Token: 0x0600033B RID: 827 RVA: 0x000122D4 File Offset: 0x000112D4
		internal ConfigXmlReader(string rawXml, string filename, int lineOffset)
			: this(rawXml, filename, lineOffset, false)
		{
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000122E0 File Offset: 0x000112E0
		internal ConfigXmlReader(string rawXml, string filename, int lineOffset, bool lineNumberIsConstant)
			: base(new StringReader(rawXml))
		{
			this._rawXml = rawXml;
			this._filename = filename;
			this._lineOffset = lineOffset;
			this._lineNumberIsConstant = lineNumberIsConstant;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0001230B File Offset: 0x0001130B
		internal ConfigXmlReader Clone()
		{
			return new ConfigXmlReader(this._rawXml, this._filename, this._lineOffset, this._lineNumberIsConstant);
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0001232A File Offset: 0x0001132A
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				if (this._lineNumberIsConstant)
				{
					return this._lineOffset;
				}
				if (this._lineOffset > 0)
				{
					return base.LineNumber + (this._lineOffset - 1);
				}
				return base.LineNumber;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0001235A File Offset: 0x0001135A
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00012362 File Offset: 0x00011362
		internal string RawXml
		{
			get
			{
				return this._rawXml;
			}
		}

		// Token: 0x040002C0 RID: 704
		private string _rawXml;

		// Token: 0x040002C1 RID: 705
		private int _lineOffset;

		// Token: 0x040002C2 RID: 706
		private string _filename;

		// Token: 0x040002C3 RID: 707
		private bool _lineNumberIsConstant;
	}
}
