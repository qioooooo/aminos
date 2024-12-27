using System;
using System.Configuration.Internal;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F1 RID: 1777
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ConfigXmlDocument : XmlDocument, IConfigErrorInfo
	{
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060036D5 RID: 14037 RVA: 0x000E9A88 File Offset: 0x000E8A88
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

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060036D6 RID: 14038 RVA: 0x000E9ABD File Offset: 0x000E8ABD
		public int LineNumber
		{
			get
			{
				return ((IConfigErrorInfo)this).LineNumber;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060036D7 RID: 14039 RVA: 0x000E9AC5 File Offset: 0x000E8AC5
		public string Filename
		{
			get
			{
				return ConfigurationException.SafeFilename(this._filename);
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060036D8 RID: 14040 RVA: 0x000E9AD2 File Offset: 0x000E8AD2
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x000E9ADC File Offset: 0x000E8ADC
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

		// Token: 0x060036DA RID: 14042 RVA: 0x000E9B44 File Offset: 0x000E8B44
		public void LoadSingleElement(string filename, XmlTextReader sourceReader)
		{
			this._filename = filename;
			this._lineOffset = sourceReader.LineNumber;
			string text = sourceReader.ReadOuterXml();
			try
			{
				this._reader = new XmlTextReader(new StringReader(text), sourceReader.NameTable);
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

		// Token: 0x060036DB RID: 14043 RVA: 0x000E9BBC File Offset: 0x000E8BBC
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlAttribute(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x000E9BD3 File Offset: 0x000E8BD3
		public override XmlElement CreateElement(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlElement(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000E9BEA File Offset: 0x000E8BEA
		public override XmlText CreateTextNode(string text)
		{
			return new ConfigXmlText(this._filename, this.LineNumber, text, this);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000E9BFF File Offset: 0x000E8BFF
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new ConfigXmlCDataSection(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000E9C14 File Offset: 0x000E8C14
		public override XmlComment CreateComment(string data)
		{
			return new ConfigXmlComment(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000E9C29 File Offset: 0x000E8C29
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string data)
		{
			return new ConfigXmlSignificantWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000E9C3E File Offset: 0x000E8C3E
		public override XmlWhitespace CreateWhitespace(string data)
		{
			return new ConfigXmlWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x0400319E RID: 12702
		private XmlTextReader _reader;

		// Token: 0x0400319F RID: 12703
		private int _lineOffset;

		// Token: 0x040031A0 RID: 12704
		private string _filename;
	}
}
