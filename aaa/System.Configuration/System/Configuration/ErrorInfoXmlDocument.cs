using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000062 RID: 98
	internal sealed class ErrorInfoXmlDocument : XmlDocument, IConfigErrorInfo
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00013286 File Offset: 0x00012286
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				if (this._reader == null)
				{
					return 0;
				}
				if (this._lineOffset > 0)
				{
					return this._reader.LineNumber + this._lineOffset - 1;
				}
				return this._reader.LineNumber;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003BC RID: 956 RVA: 0x000132BB File Offset: 0x000122BB
		internal int LineNumber
		{
			get
			{
				return ((IConfigErrorInfo)this).LineNumber;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003BD RID: 957 RVA: 0x000132C3 File Offset: 0x000122C3
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000132CC File Offset: 0x000122CC
		public override void Load(string filename)
		{
			this._filename = filename;
			try
			{
				this._reader = new XmlTextReader(filename);
				this._reader.XmlResolver = null;
				base.Load(this._reader);
			}
			finally
			{
				if (this._reader != null)
				{
					this._reader.Close();
					this._reader = null;
				}
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00013334 File Offset: 0x00012334
		private void LoadFromConfigXmlReader(ConfigXmlReader reader)
		{
			this._filename = ((IConfigErrorInfo)reader).Filename;
			this._lineOffset = ((IConfigErrorInfo)reader).LineNumber + 1;
			try
			{
				this._reader = reader;
				base.Load(this._reader);
			}
			finally
			{
				if (this._reader != null)
				{
					this._reader.Close();
					this._reader = null;
				}
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000133A0 File Offset: 0x000123A0
		internal static XmlNode CreateSectionXmlNode(ConfigXmlReader reader)
		{
			ErrorInfoXmlDocument errorInfoXmlDocument = new ErrorInfoXmlDocument();
			errorInfoXmlDocument.LoadFromConfigXmlReader(reader);
			return errorInfoXmlDocument.DocumentElement;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000133C2 File Offset: 0x000123C2
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlAttribute(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000133D9 File Offset: 0x000123D9
		public override XmlElement CreateElement(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlElement(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000133F0 File Offset: 0x000123F0
		public override XmlText CreateTextNode(string text)
		{
			return new ConfigXmlText(this._filename, this.LineNumber, text, this);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00013405 File Offset: 0x00012405
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new ConfigXmlCDataSection(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001341A File Offset: 0x0001241A
		public override XmlComment CreateComment(string data)
		{
			return new ConfigXmlComment(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001342F File Offset: 0x0001242F
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string data)
		{
			return new ConfigXmlSignificantWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00013444 File Offset: 0x00012444
		public override XmlWhitespace CreateWhitespace(string data)
		{
			return new ConfigXmlWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x040002F2 RID: 754
		private XmlTextReader _reader;

		// Token: 0x040002F3 RID: 755
		private int _lineOffset;

		// Token: 0x040002F4 RID: 756
		private string _filename;
	}
}
