using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F3 RID: 1779
	internal sealed class ConfigXmlSignificantWhitespace : XmlSignificantWhitespace, IConfigErrorInfo
	{
		// Token: 0x060036E7 RID: 14055 RVA: 0x000E9CC0 File Offset: 0x000E8CC0
		public ConfigXmlSignificantWhitespace(string filename, int line, string strData, XmlDocument doc)
			: base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060036E8 RID: 14056 RVA: 0x000E9CD9 File Offset: 0x000E8CD9
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060036E9 RID: 14057 RVA: 0x000E9CE1 File Offset: 0x000E8CE1
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x000E9CEC File Offset: 0x000E8CEC
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

		// Token: 0x040031A3 RID: 12707
		private int _line;

		// Token: 0x040031A4 RID: 12708
		private string _filename;
	}
}
