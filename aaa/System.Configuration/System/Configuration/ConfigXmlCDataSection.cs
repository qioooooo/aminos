using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200004A RID: 74
	internal sealed class ConfigXmlCDataSection : XmlCDataSection, IConfigErrorInfo
	{
		// Token: 0x0600032F RID: 815 RVA: 0x000121A4 File Offset: 0x000111A4
		public ConfigXmlCDataSection(string filename, int line, string data, XmlDocument doc)
			: base(data, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000330 RID: 816 RVA: 0x000121BD File Offset: 0x000111BD
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000331 RID: 817 RVA: 0x000121C5 File Offset: 0x000111C5
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000121D0 File Offset: 0x000111D0
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlCDataSection configXmlCDataSection = xmlNode as ConfigXmlCDataSection;
			if (configXmlCDataSection != null)
			{
				configXmlCDataSection._line = this._line;
				configXmlCDataSection._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x040002BA RID: 698
		private int _line;

		// Token: 0x040002BB RID: 699
		private string _filename;
	}
}
