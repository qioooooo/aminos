using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006EF RID: 1775
	internal sealed class ConfigXmlCDataSection : XmlCDataSection, IConfigErrorInfo
	{
		// Token: 0x060036CD RID: 14029 RVA: 0x000E99C0 File Offset: 0x000E89C0
		public ConfigXmlCDataSection(string filename, int line, string data, XmlDocument doc)
			: base(data, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x000E99D9 File Offset: 0x000E89D9
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x060036CF RID: 14031 RVA: 0x000E99E1 File Offset: 0x000E89E1
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x000E99EC File Offset: 0x000E89EC
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

		// Token: 0x0400319A RID: 12698
		private int _line;

		// Token: 0x0400319B RID: 12699
		private string _filename;
	}
}
