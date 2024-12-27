using System;
using System.Diagnostics;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000184 RID: 388
	internal class OutKeywords
	{
		// Token: 0x0600103C RID: 4156 RVA: 0x0004F5C4 File Offset: 0x0004E5C4
		internal OutKeywords(XmlNameTable nameTable)
		{
			this._AtomEmpty = nameTable.Add(string.Empty);
			this._AtomLang = nameTable.Add("lang");
			this._AtomSpace = nameTable.Add("space");
			this._AtomXmlns = nameTable.Add("xmlns");
			this._AtomXml = nameTable.Add("xml");
			this._AtomXmlNamespace = nameTable.Add("http://www.w3.org/XML/1998/namespace");
			this._AtomXmlnsNamespace = nameTable.Add("http://www.w3.org/2000/xmlns/");
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x0004F64E File Offset: 0x0004E64E
		internal string Empty
		{
			get
			{
				return this._AtomEmpty;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0004F656 File Offset: 0x0004E656
		internal string Lang
		{
			get
			{
				return this._AtomLang;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0004F65E File Offset: 0x0004E65E
		internal string Space
		{
			get
			{
				return this._AtomSpace;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0004F666 File Offset: 0x0004E666
		internal string Xmlns
		{
			get
			{
				return this._AtomXmlns;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0004F66E File Offset: 0x0004E66E
		internal string Xml
		{
			get
			{
				return this._AtomXml;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0004F676 File Offset: 0x0004E676
		internal string XmlNamespace
		{
			get
			{
				return this._AtomXmlNamespace;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0004F67E File Offset: 0x0004E67E
		internal string XmlnsNamespace
		{
			get
			{
				return this._AtomXmlnsNamespace;
			}
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0004F686 File Offset: 0x0004E686
		[Conditional("DEBUG")]
		private void CheckKeyword(string keyword)
		{
		}

		// Token: 0x04000AE7 RID: 2791
		private string _AtomEmpty;

		// Token: 0x04000AE8 RID: 2792
		private string _AtomLang;

		// Token: 0x04000AE9 RID: 2793
		private string _AtomSpace;

		// Token: 0x04000AEA RID: 2794
		private string _AtomXmlns;

		// Token: 0x04000AEB RID: 2795
		private string _AtomXml;

		// Token: 0x04000AEC RID: 2796
		private string _AtomXmlNamespace;

		// Token: 0x04000AED RID: 2797
		private string _AtomXmlnsNamespace;
	}
}
