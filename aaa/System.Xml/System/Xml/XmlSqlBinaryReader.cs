using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020000F7 RID: 247
	internal sealed class XmlSqlBinaryReader : XmlReader, IXmlNamespaceResolver
	{
		// Token: 0x06000F16 RID: 3862 RVA: 0x000423BC File Offset: 0x000413BC
		public XmlSqlBinaryReader(Stream stream, byte[] data, int len, string baseUri, bool closeInput, XmlReaderSettings settings)
		{
			this.unicode = Encoding.Unicode;
			this.xmlCharType = XmlCharType.Instance;
			this.xnt = settings.NameTable;
			if (this.xnt == null)
			{
				this.xnt = new NameTable();
				this.xntFromSettings = false;
			}
			else
			{
				this.xntFromSettings = true;
			}
			this.xml = this.xnt.Add("xml");
			this.xmlns = this.xnt.Add("xmlns");
			this.nsxmlns = this.xnt.Add("http://www.w3.org/2000/xmlns/");
			this.baseUri = baseUri;
			this.state = XmlSqlBinaryReader.ScanState.Init;
			this.nodetype = XmlNodeType.None;
			this.token = BinXmlToken.Error;
			this.elementStack = new XmlSqlBinaryReader.ElemInfo[16];
			this.attributes = new XmlSqlBinaryReader.AttrInfo[8];
			this.attrHashTbl = new int[8];
			this.symbolTables.Init();
			this.qnameOther.Clear();
			this.qnameElement.Clear();
			this.xmlspacePreserve = false;
			this.hasher = new SecureStringHasher();
			this.namespaces = new Dictionary<string, XmlSqlBinaryReader.NamespaceDecl>(this.hasher);
			this.AddInitNamespace(string.Empty, string.Empty);
			this.AddInitNamespace(this.xml, this.xnt.Add("http://www.w3.org/XML/1998/namespace"));
			this.AddInitNamespace(this.xmlns, this.nsxmlns);
			this.valueType = XmlSqlBinaryReader.TypeOfString;
			this.inStrm = stream;
			if (data != null)
			{
				this.data = data;
				this.end = len;
				this.pos = 2;
				this.sniffed = true;
			}
			else
			{
				this.data = new byte[4096];
				this.end = stream.Read(this.data, 0, 4096);
				this.pos = 0;
				this.sniffed = false;
			}
			this.mark = -1;
			this.eof = 0 == this.end;
			this.offset = 0L;
			this.closeInput = closeInput;
			switch (settings.ConformanceLevel)
			{
			case ConformanceLevel.Auto:
				this.docState = 0;
				break;
			case ConformanceLevel.Fragment:
				this.docState = 9;
				break;
			case ConformanceLevel.Document:
				this.docState = 1;
				break;
			}
			this.checkCharacters = settings.CheckCharacters;
			this.prohibitDtd = settings.ProhibitDtd;
			this.ignoreWhitespace = settings.IgnoreWhitespace;
			this.ignorePIs = settings.IgnoreProcessingInstructions;
			this.ignoreComments = settings.IgnoreComments;
			if (XmlSqlBinaryReader.TokenTypeMap == null)
			{
				this.GenerateTokenTypeMap();
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00042630 File Offset: 0x00041630
		public override XmlReaderSettings Settings
		{
			get
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				if (this.xntFromSettings)
				{
					xmlReaderSettings.NameTable = this.xnt;
				}
				int num = this.docState;
				if (num != 0)
				{
					if (num != 9)
					{
						xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
					}
					else
					{
						xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
					}
				}
				else
				{
					xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
				}
				xmlReaderSettings.CheckCharacters = this.checkCharacters;
				xmlReaderSettings.IgnoreWhitespace = this.ignoreWhitespace;
				xmlReaderSettings.IgnoreProcessingInstructions = this.ignorePIs;
				xmlReaderSettings.IgnoreComments = this.ignoreComments;
				xmlReaderSettings.ProhibitDtd = this.prohibitDtd;
				xmlReaderSettings.CloseInput = this.closeInput;
				xmlReaderSettings.ReadOnly = true;
				return xmlReaderSettings;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x000426D2 File Offset: 0x000416D2
		public override XmlNodeType NodeType
		{
			get
			{
				return this.nodetype;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x000426DA File Offset: 0x000416DA
		public override string LocalName
		{
			get
			{
				return this.qnameOther.localname;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x000426E7 File Offset: 0x000416E7
		public override string NamespaceURI
		{
			get
			{
				return this.qnameOther.namespaceUri;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x000426F4 File Offset: 0x000416F4
		public override string Prefix
		{
			get
			{
				return this.qnameOther.prefix;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x00042701 File Offset: 0x00041701
		public override bool HasValue
		{
			get
			{
				if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
				{
					return this.textXmlReader.HasValue;
				}
				return XmlReader.HasValueInternal(this.nodetype);
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00042724 File Offset: 0x00041724
		public override string Value
		{
			get
			{
				if (this.stringValue != null)
				{
					return this.stringValue;
				}
				switch (this.state)
				{
				case XmlSqlBinaryReader.ScanState.Doc:
					switch (this.nodetype)
					{
					case XmlNodeType.Text:
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						return this.stringValue = this.ValueAsString(this.token);
					case XmlNodeType.CDATA:
						return this.stringValue = this.CDATAValue();
					case XmlNodeType.ProcessingInstruction:
					case XmlNodeType.Comment:
					case XmlNodeType.DocumentType:
						return this.stringValue = this.GetString(this.tokDataPos, this.tokLen);
					case XmlNodeType.XmlDeclaration:
						return this.stringValue = this.XmlDeclValue();
					}
					break;
				case XmlSqlBinaryReader.ScanState.XmlText:
					return this.textXmlReader.Value;
				case XmlSqlBinaryReader.ScanState.Attr:
				case XmlSqlBinaryReader.ScanState.AttrValPseudoValue:
					return this.stringValue = this.GetAttributeText(this.attrIndex - 1);
				case XmlSqlBinaryReader.ScanState.AttrVal:
					return this.stringValue = this.ValueAsString(this.token);
				}
				return string.Empty;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x00042850 File Offset: 0x00041850
		public override int Depth
		{
			get
			{
				int num = 0;
				switch (this.state)
				{
				case XmlSqlBinaryReader.ScanState.Doc:
					if (this.nodetype == XmlNodeType.Element || this.nodetype == XmlNodeType.EndElement)
					{
						num = -1;
					}
					break;
				case XmlSqlBinaryReader.ScanState.XmlText:
					num = this.textXmlReader.Depth;
					break;
				case XmlSqlBinaryReader.ScanState.Attr:
					break;
				case XmlSqlBinaryReader.ScanState.AttrVal:
				case XmlSqlBinaryReader.ScanState.AttrValPseudoValue:
					num = 1;
					break;
				default:
					return 0;
				}
				return this.elemDepth + num;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x000428B5 File Offset: 0x000418B5
		public override string BaseURI
		{
			get
			{
				return this.baseUri;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x000428C0 File Offset: 0x000418C0
		public override bool IsEmptyElement
		{
			get
			{
				switch (this.state)
				{
				case XmlSqlBinaryReader.ScanState.Doc:
				case XmlSqlBinaryReader.ScanState.XmlText:
					return this.isEmpty;
				default:
					return false;
				}
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x000428EC File Offset: 0x000418EC
		public override XmlSpace XmlSpace
		{
			get
			{
				if (XmlSqlBinaryReader.ScanState.XmlText != this.state)
				{
					for (int i = this.elemDepth; i >= 0; i--)
					{
						XmlSpace xmlSpace = this.elementStack[i].xmlSpace;
						if (xmlSpace != XmlSpace.None)
						{
							return xmlSpace;
						}
					}
					return XmlSpace.None;
				}
				return this.textXmlReader.XmlSpace;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x00042938 File Offset: 0x00041938
		public override string XmlLang
		{
			get
			{
				if (XmlSqlBinaryReader.ScanState.XmlText != this.state)
				{
					for (int i = this.elemDepth; i >= 0; i--)
					{
						string xmlLang = this.elementStack[i].xmlLang;
						if (xmlLang != null)
						{
							return xmlLang;
						}
					}
					return string.Empty;
				}
				return this.textXmlReader.XmlLang;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x00042987 File Offset: 0x00041987
		public override Type ValueType
		{
			get
			{
				return this.valueType;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x00042990 File Offset: 0x00041990
		public override int AttributeCount
		{
			get
			{
				switch (this.state)
				{
				case XmlSqlBinaryReader.ScanState.Doc:
				case XmlSqlBinaryReader.ScanState.Attr:
				case XmlSqlBinaryReader.ScanState.AttrVal:
				case XmlSqlBinaryReader.ScanState.AttrValPseudoValue:
					return this.attrCount;
				case XmlSqlBinaryReader.ScanState.XmlText:
					return this.textXmlReader.AttributeCount;
				default:
					return 0;
				}
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x000429D4 File Offset: 0x000419D4
		public override string GetAttribute(string name, string ns)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				return this.textXmlReader.GetAttribute(name, ns);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (ns == null)
			{
				ns = string.Empty;
			}
			int num = this.LocateAttribute(name, ns);
			if (-1 == num)
			{
				return null;
			}
			return this.GetAttribute(num);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00042A28 File Offset: 0x00041A28
		public override string GetAttribute(string name)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				return this.textXmlReader.GetAttribute(name);
			}
			int num = this.LocateAttribute(name);
			if (-1 == num)
			{
				return null;
			}
			return this.GetAttribute(num);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00042A60 File Offset: 0x00041A60
		public override string GetAttribute(int i)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				return this.textXmlReader.GetAttribute(i);
			}
			if (i < 0 || i >= this.attrCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			return this.GetAttributeText(i);
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00042A98 File Offset: 0x00041A98
		public override bool MoveToAttribute(string name, string ns)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				return this.UpdateFromTextReader(this.textXmlReader.MoveToAttribute(name, ns));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (ns == null)
			{
				ns = string.Empty;
			}
			int num = this.LocateAttribute(name, ns);
			if (-1 != num && this.state < XmlSqlBinaryReader.ScanState.Init)
			{
				this.PositionOnAttribute(num + 1);
				return true;
			}
			return false;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00042AFC File Offset: 0x00041AFC
		public override bool MoveToAttribute(string name)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				return this.UpdateFromTextReader(this.textXmlReader.MoveToAttribute(name));
			}
			int num = this.LocateAttribute(name);
			if (-1 != num && this.state < XmlSqlBinaryReader.ScanState.Init)
			{
				this.PositionOnAttribute(num + 1);
				return true;
			}
			return false;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00042B48 File Offset: 0x00041B48
		public override void MoveToAttribute(int i)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				this.textXmlReader.MoveToAttribute(i);
				this.UpdateFromTextReader(true);
				return;
			}
			if (i < 0 || i >= this.attrCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			this.PositionOnAttribute(i + 1);
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00042B94 File Offset: 0x00041B94
		public override bool MoveToFirstAttribute()
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				return this.UpdateFromTextReader(this.textXmlReader.MoveToFirstAttribute());
			}
			if (this.attrCount == 0)
			{
				return false;
			}
			this.PositionOnAttribute(1);
			return true;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00042BC4 File Offset: 0x00041BC4
		public override bool MoveToNextAttribute()
		{
			switch (this.state)
			{
			case XmlSqlBinaryReader.ScanState.Doc:
			case XmlSqlBinaryReader.ScanState.Attr:
			case XmlSqlBinaryReader.ScanState.AttrVal:
			case XmlSqlBinaryReader.ScanState.AttrValPseudoValue:
				if (this.attrIndex >= this.attrCount)
				{
					return false;
				}
				this.PositionOnAttribute(++this.attrIndex);
				return true;
			case XmlSqlBinaryReader.ScanState.XmlText:
				return this.UpdateFromTextReader(this.textXmlReader.MoveToNextAttribute());
			default:
				return false;
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00042C30 File Offset: 0x00041C30
		public override bool MoveToElement()
		{
			switch (this.state)
			{
			case XmlSqlBinaryReader.ScanState.XmlText:
				return this.UpdateFromTextReader(this.textXmlReader.MoveToElement());
			case XmlSqlBinaryReader.ScanState.Attr:
			case XmlSqlBinaryReader.ScanState.AttrVal:
			case XmlSqlBinaryReader.ScanState.AttrValPseudoValue:
				this.attrIndex = 0;
				this.qnameOther = this.qnameElement;
				if (XmlNodeType.Element == this.parentNodeType)
				{
					this.token = BinXmlToken.Element;
				}
				else if (XmlNodeType.XmlDeclaration == this.parentNodeType)
				{
					this.token = BinXmlToken.XmlDecl;
				}
				else if (XmlNodeType.DocumentType == this.parentNodeType)
				{
					this.token = BinXmlToken.DocType;
				}
				this.nodetype = this.parentNodeType;
				this.state = XmlSqlBinaryReader.ScanState.Doc;
				this.pos = this.posAfterAttrs;
				this.stringValue = null;
				return true;
			default:
				return false;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x00042CF1 File Offset: 0x00041CF1
		public override bool EOF
		{
			get
			{
				return this.state == XmlSqlBinaryReader.ScanState.EOF;
			}
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00042CFC File Offset: 0x00041CFC
		public override bool ReadAttributeValue()
		{
			this.stringValue = null;
			switch (this.state)
			{
			case XmlSqlBinaryReader.ScanState.XmlText:
				return this.UpdateFromTextReader(this.textXmlReader.ReadAttributeValue());
			case XmlSqlBinaryReader.ScanState.Attr:
				if (this.attributes[this.attrIndex - 1].val == null)
				{
					this.pos = this.attributes[this.attrIndex - 1].contentPos;
					BinXmlToken binXmlToken = this.RescanNextToken();
					if (BinXmlToken.Attr == binXmlToken || BinXmlToken.EndAttrs == binXmlToken)
					{
						return false;
					}
					this.token = binXmlToken;
					this.ReScanOverValue(binXmlToken);
					this.valueType = this.GetValueType(binXmlToken);
					this.state = XmlSqlBinaryReader.ScanState.AttrVal;
				}
				else
				{
					this.token = BinXmlToken.Error;
					this.valueType = XmlSqlBinaryReader.TypeOfString;
					this.state = XmlSqlBinaryReader.ScanState.AttrValPseudoValue;
				}
				this.qnameOther.Clear();
				this.nodetype = XmlNodeType.Text;
				return true;
			case XmlSqlBinaryReader.ScanState.AttrVal:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00042DE8 File Offset: 0x00041DE8
		public override void Close()
		{
			this.state = XmlSqlBinaryReader.ScanState.Closed;
			this.nodetype = XmlNodeType.None;
			this.token = BinXmlToken.Error;
			this.stringValue = null;
			if (this.textXmlReader != null)
			{
				this.textXmlReader.Close();
				this.textXmlReader = null;
			}
			if (this.inStrm != null && this.closeInput)
			{
				this.inStrm.Close();
			}
			this.inStrm = null;
			this.pos = (this.end = 0);
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x00042E5D File Offset: 0x00041E5D
		public override XmlNameTable NameTable
		{
			get
			{
				return this.xnt;
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00042E68 File Offset: 0x00041E68
		public override string LookupNamespace(string prefix)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				return this.textXmlReader.LookupNamespace(prefix);
			}
			XmlSqlBinaryReader.NamespaceDecl namespaceDecl;
			if (prefix != null && this.namespaces.TryGetValue(prefix, out namespaceDecl))
			{
				return namespaceDecl.uri;
			}
			return null;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00042EA6 File Offset: 0x00041EA6
		public override void ResolveEntity()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x00042EAD File Offset: 0x00041EAD
		public override ReadState ReadState
		{
			get
			{
				return XmlSqlBinaryReader.ScanState2ReadState[(int)this.state];
			}
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00042EBC File Offset: 0x00041EBC
		public override bool Read()
		{
			bool flag;
			try
			{
				switch (this.state)
				{
				case XmlSqlBinaryReader.ScanState.Doc:
					break;
				case XmlSqlBinaryReader.ScanState.XmlText:
					if (this.textXmlReader.Read())
					{
						return this.UpdateFromTextReader(true);
					}
					this.state = XmlSqlBinaryReader.ScanState.Doc;
					this.nodetype = XmlNodeType.None;
					this.isEmpty = false;
					break;
				case XmlSqlBinaryReader.ScanState.Attr:
				case XmlSqlBinaryReader.ScanState.AttrVal:
				case XmlSqlBinaryReader.ScanState.AttrValPseudoValue:
					this.MoveToElement();
					break;
				case XmlSqlBinaryReader.ScanState.Init:
					return this.ReadInit(false);
				default:
					return false;
				}
				flag = this.ReadDoc();
			}
			catch (OverflowException ex)
			{
				this.state = XmlSqlBinaryReader.ScanState.Error;
				throw new XmlException(ex.Message, ex);
			}
			catch
			{
				this.state = XmlSqlBinaryReader.ScanState.Error;
				throw;
			}
			return flag;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00042F7C File Offset: 0x00041F7C
		private bool SetupContentAsXXX(string name)
		{
			if (!XmlReader.CanReadContentAs(this.NodeType))
			{
				throw base.CreateReadContentAsException(name);
			}
			switch (this.state)
			{
			case XmlSqlBinaryReader.ScanState.Doc:
				if (this.NodeType == XmlNodeType.EndElement)
				{
					return true;
				}
				if (this.NodeType == XmlNodeType.ProcessingInstruction || this.NodeType == XmlNodeType.Comment)
				{
					while (this.Read() && (this.NodeType == XmlNodeType.ProcessingInstruction || this.NodeType == XmlNodeType.Comment))
					{
					}
					if (this.NodeType == XmlNodeType.EndElement)
					{
						return true;
					}
				}
				if (this.hasTypedValue)
				{
					return true;
				}
				break;
			case XmlSqlBinaryReader.ScanState.Attr:
			{
				this.pos = this.attributes[this.attrIndex - 1].contentPos;
				BinXmlToken binXmlToken = this.RescanNextToken();
				if (BinXmlToken.Attr != binXmlToken && BinXmlToken.EndAttrs != binXmlToken)
				{
					this.token = binXmlToken;
					this.ReScanOverValue(binXmlToken);
					return true;
				}
				break;
			}
			case XmlSqlBinaryReader.ScanState.AttrVal:
				return true;
			}
			return false;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00043058 File Offset: 0x00042058
		private int FinishContentAsXXX(int origPos)
		{
			if (this.state == XmlSqlBinaryReader.ScanState.Doc)
			{
				if (this.NodeType != XmlNodeType.Element && this.NodeType != XmlNodeType.EndElement)
				{
					while (this.Read())
					{
						XmlNodeType nodeType = this.NodeType;
						if (nodeType == XmlNodeType.Element)
						{
							break;
						}
						switch (nodeType)
						{
						case XmlNodeType.ProcessingInstruction:
						case XmlNodeType.Comment:
							break;
						default:
							if (nodeType != XmlNodeType.EndElement)
							{
								throw this.ThrowNotSupported("XmlBinary_ListsOfValuesNotSupported");
							}
							goto IL_004F;
						}
					}
				}
				IL_004F:
				return this.pos;
			}
			return origPos;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000430BC File Offset: 0x000420BC
		public override bool ReadContentAsBoolean()
		{
			int num = this.pos;
			bool flag = false;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsBoolean"))
				{
					try
					{
						BinXmlToken binXmlToken = this.token;
						switch (binXmlToken)
						{
						case BinXmlToken.SQL_SMALLINT:
						case BinXmlToken.SQL_INT:
						case BinXmlToken.SQL_REAL:
						case BinXmlToken.SQL_FLOAT:
						case BinXmlToken.SQL_MONEY:
						case BinXmlToken.SQL_BIT:
						case BinXmlToken.SQL_TINYINT:
						case BinXmlToken.SQL_BIGINT:
						case BinXmlToken.SQL_UUID:
						case BinXmlToken.SQL_DECIMAL:
						case BinXmlToken.SQL_NUMERIC:
						case BinXmlToken.SQL_BINARY:
						case BinXmlToken.SQL_VARBINARY:
						case BinXmlToken.SQL_DATETIME:
						case BinXmlToken.SQL_SMALLDATETIME:
						case BinXmlToken.SQL_SMALLMONEY:
						case BinXmlToken.SQL_IMAGE:
						case BinXmlToken.SQL_UDT:
							break;
						case BinXmlToken.SQL_CHAR:
						case BinXmlToken.SQL_NCHAR:
						case BinXmlToken.SQL_VARCHAR:
						case BinXmlToken.SQL_NVARCHAR:
						case BinXmlToken.SQL_TEXT:
						case BinXmlToken.SQL_NTEXT:
							goto IL_019C;
						case (BinXmlToken)21:
						case (BinXmlToken)25:
						case (BinXmlToken)26:
							goto IL_015B;
						default:
							switch (binXmlToken)
							{
							case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATEOFFSET:
							case BinXmlToken.XSD_KATMAI_TIME:
							case BinXmlToken.XSD_KATMAI_DATETIME:
							case BinXmlToken.XSD_KATMAI_DATE:
							case BinXmlToken.XSD_TIME:
							case BinXmlToken.XSD_DATETIME:
							case BinXmlToken.XSD_DATE:
							case BinXmlToken.XSD_BINHEX:
							case BinXmlToken.XSD_BASE64:
							case BinXmlToken.XSD_DECIMAL:
							case BinXmlToken.XSD_BYTE:
							case BinXmlToken.XSD_UNSIGNEDSHORT:
							case BinXmlToken.XSD_UNSIGNEDINT:
							case BinXmlToken.XSD_UNSIGNEDLONG:
							case BinXmlToken.XSD_QNAME:
								break;
							case (BinXmlToken)128:
								goto IL_015B;
							case BinXmlToken.XSD_BOOLEAN:
								flag = 0 != this.data[this.tokDataPos];
								goto IL_0185;
							default:
								switch (binXmlToken)
								{
								case BinXmlToken.EndElem:
								case BinXmlToken.Element:
									return XmlConvert.ToBoolean(string.Empty);
								default:
									goto IL_015B;
								}
								break;
							}
							break;
						}
						throw new InvalidCastException(Res.GetString("XmlBinary_CastNotSupported", new object[] { this.token, "Boolean" }));
						IL_015B:
						goto IL_019C;
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Boolean", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Boolean", ex2, null);
					}
					IL_0185:
					num = this.FinishContentAsXXX(num);
					return flag;
				}
			}
			finally
			{
				this.pos = num;
			}
			IL_019C:
			return base.ReadContentAsBoolean();
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x000432BC File Offset: 0x000422BC
		public override DateTime ReadContentAsDateTime()
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsDateTime"))
				{
					DateTime dateTime;
					try
					{
						BinXmlToken binXmlToken = this.token;
						switch (binXmlToken)
						{
						case BinXmlToken.SQL_SMALLINT:
						case BinXmlToken.SQL_INT:
						case BinXmlToken.SQL_REAL:
						case BinXmlToken.SQL_FLOAT:
						case BinXmlToken.SQL_MONEY:
						case BinXmlToken.SQL_BIT:
						case BinXmlToken.SQL_TINYINT:
						case BinXmlToken.SQL_BIGINT:
						case BinXmlToken.SQL_UUID:
						case BinXmlToken.SQL_DECIMAL:
						case BinXmlToken.SQL_NUMERIC:
						case BinXmlToken.SQL_BINARY:
						case BinXmlToken.SQL_VARBINARY:
						case BinXmlToken.SQL_SMALLMONEY:
						case BinXmlToken.SQL_IMAGE:
						case BinXmlToken.SQL_UDT:
							goto IL_010A;
						case BinXmlToken.SQL_CHAR:
						case BinXmlToken.SQL_NCHAR:
						case BinXmlToken.SQL_VARCHAR:
						case BinXmlToken.SQL_NVARCHAR:
						case BinXmlToken.SQL_TEXT:
						case BinXmlToken.SQL_NTEXT:
							goto IL_01A3;
						case BinXmlToken.SQL_DATETIME:
						case BinXmlToken.SQL_SMALLDATETIME:
							break;
						case (BinXmlToken)21:
						case (BinXmlToken)25:
						case (BinXmlToken)26:
							goto IL_014D;
						default:
							switch (binXmlToken)
							{
							case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATEOFFSET:
							case BinXmlToken.XSD_KATMAI_TIME:
							case BinXmlToken.XSD_KATMAI_DATETIME:
							case BinXmlToken.XSD_KATMAI_DATE:
							case BinXmlToken.XSD_TIME:
							case BinXmlToken.XSD_DATETIME:
							case BinXmlToken.XSD_DATE:
								break;
							case (BinXmlToken)128:
								goto IL_014D;
							case BinXmlToken.XSD_BINHEX:
							case BinXmlToken.XSD_BASE64:
							case BinXmlToken.XSD_BOOLEAN:
							case BinXmlToken.XSD_DECIMAL:
							case BinXmlToken.XSD_BYTE:
							case BinXmlToken.XSD_UNSIGNEDSHORT:
							case BinXmlToken.XSD_UNSIGNEDINT:
							case BinXmlToken.XSD_UNSIGNEDLONG:
							case BinXmlToken.XSD_QNAME:
								goto IL_010A;
							default:
								switch (binXmlToken)
								{
								case BinXmlToken.EndElem:
								case BinXmlToken.Element:
									return XmlConvert.ToDateTime(string.Empty, XmlDateTimeSerializationMode.RoundtripKind);
								default:
									goto IL_014D;
								}
								break;
							}
							break;
						}
						dateTime = this.ValueAsDateTime();
						goto IL_018C;
						IL_010A:
						throw new InvalidCastException(Res.GetString("XmlBinary_CastNotSupported", new object[] { this.token, "DateTime" }));
						IL_014D:
						goto IL_01A3;
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "DateTime", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "DateTime", ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "DateTime", ex3, null);
					}
					IL_018C:
					num = this.FinishContentAsXXX(num);
					return dateTime;
				}
			}
			finally
			{
				this.pos = num;
			}
			IL_01A3:
			return base.ReadContentAsDateTime();
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000434DC File Offset: 0x000424DC
		public override double ReadContentAsDouble()
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsDouble"))
				{
					double num2;
					try
					{
						BinXmlToken binXmlToken = this.token;
						switch (binXmlToken)
						{
						case BinXmlToken.SQL_SMALLINT:
						case BinXmlToken.SQL_INT:
						case BinXmlToken.SQL_MONEY:
						case BinXmlToken.SQL_BIT:
						case BinXmlToken.SQL_TINYINT:
						case BinXmlToken.SQL_BIGINT:
						case BinXmlToken.SQL_UUID:
						case BinXmlToken.SQL_DECIMAL:
						case BinXmlToken.SQL_NUMERIC:
						case BinXmlToken.SQL_BINARY:
						case BinXmlToken.SQL_VARBINARY:
						case BinXmlToken.SQL_DATETIME:
						case BinXmlToken.SQL_SMALLDATETIME:
						case BinXmlToken.SQL_SMALLMONEY:
						case BinXmlToken.SQL_IMAGE:
						case BinXmlToken.SQL_UDT:
							break;
						case BinXmlToken.SQL_REAL:
						case BinXmlToken.SQL_FLOAT:
							num2 = this.ValueAsDouble();
							goto IL_018B;
						case BinXmlToken.SQL_CHAR:
						case BinXmlToken.SQL_NCHAR:
						case BinXmlToken.SQL_VARCHAR:
						case BinXmlToken.SQL_NVARCHAR:
						case BinXmlToken.SQL_TEXT:
						case BinXmlToken.SQL_NTEXT:
							goto IL_01A2;
						case (BinXmlToken)21:
						case (BinXmlToken)25:
						case (BinXmlToken)26:
							goto IL_014C;
						default:
							switch (binXmlToken)
							{
							case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATEOFFSET:
							case BinXmlToken.XSD_KATMAI_TIME:
							case BinXmlToken.XSD_KATMAI_DATETIME:
							case BinXmlToken.XSD_KATMAI_DATE:
							case BinXmlToken.XSD_TIME:
							case BinXmlToken.XSD_DATETIME:
							case BinXmlToken.XSD_DATE:
							case BinXmlToken.XSD_BINHEX:
							case BinXmlToken.XSD_BASE64:
							case BinXmlToken.XSD_BOOLEAN:
							case BinXmlToken.XSD_DECIMAL:
							case BinXmlToken.XSD_BYTE:
							case BinXmlToken.XSD_UNSIGNEDSHORT:
							case BinXmlToken.XSD_UNSIGNEDINT:
							case BinXmlToken.XSD_UNSIGNEDLONG:
							case BinXmlToken.XSD_QNAME:
								break;
							case (BinXmlToken)128:
								goto IL_014C;
							default:
								switch (binXmlToken)
								{
								case BinXmlToken.EndElem:
								case BinXmlToken.Element:
									return XmlConvert.ToDouble(string.Empty);
								default:
									goto IL_014C;
								}
								break;
							}
							break;
						}
						throw new InvalidCastException(Res.GetString("XmlBinary_CastNotSupported", new object[] { this.token, "Double" }));
						IL_014C:
						goto IL_01A2;
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Double", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Double", ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Double", ex3, null);
					}
					IL_018B:
					num = this.FinishContentAsXXX(num);
					return num2;
				}
			}
			finally
			{
				this.pos = num;
			}
			IL_01A2:
			return base.ReadContentAsDouble();
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x000436F8 File Offset: 0x000426F8
		public override float ReadContentAsFloat()
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsFloat"))
				{
					float num2;
					try
					{
						BinXmlToken binXmlToken = this.token;
						switch (binXmlToken)
						{
						case BinXmlToken.SQL_SMALLINT:
						case BinXmlToken.SQL_INT:
						case BinXmlToken.SQL_MONEY:
						case BinXmlToken.SQL_BIT:
						case BinXmlToken.SQL_TINYINT:
						case BinXmlToken.SQL_BIGINT:
						case BinXmlToken.SQL_UUID:
						case BinXmlToken.SQL_DECIMAL:
						case BinXmlToken.SQL_NUMERIC:
						case BinXmlToken.SQL_BINARY:
						case BinXmlToken.SQL_VARBINARY:
						case BinXmlToken.SQL_DATETIME:
						case BinXmlToken.SQL_SMALLDATETIME:
						case BinXmlToken.SQL_SMALLMONEY:
						case BinXmlToken.SQL_IMAGE:
						case BinXmlToken.SQL_UDT:
							break;
						case BinXmlToken.SQL_REAL:
						case BinXmlToken.SQL_FLOAT:
							num2 = (float)this.ValueAsDouble();
							goto IL_018C;
						case BinXmlToken.SQL_CHAR:
						case BinXmlToken.SQL_NCHAR:
						case BinXmlToken.SQL_VARCHAR:
						case BinXmlToken.SQL_NVARCHAR:
						case BinXmlToken.SQL_TEXT:
						case BinXmlToken.SQL_NTEXT:
							goto IL_01A3;
						case (BinXmlToken)21:
						case (BinXmlToken)25:
						case (BinXmlToken)26:
							goto IL_014D;
						default:
							switch (binXmlToken)
							{
							case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATEOFFSET:
							case BinXmlToken.XSD_KATMAI_TIME:
							case BinXmlToken.XSD_KATMAI_DATETIME:
							case BinXmlToken.XSD_KATMAI_DATE:
							case BinXmlToken.XSD_TIME:
							case BinXmlToken.XSD_DATETIME:
							case BinXmlToken.XSD_DATE:
							case BinXmlToken.XSD_BINHEX:
							case BinXmlToken.XSD_BASE64:
							case BinXmlToken.XSD_BOOLEAN:
							case BinXmlToken.XSD_DECIMAL:
							case BinXmlToken.XSD_BYTE:
							case BinXmlToken.XSD_UNSIGNEDSHORT:
							case BinXmlToken.XSD_UNSIGNEDINT:
							case BinXmlToken.XSD_UNSIGNEDLONG:
							case BinXmlToken.XSD_QNAME:
								break;
							case (BinXmlToken)128:
								goto IL_014D;
							default:
								switch (binXmlToken)
								{
								case BinXmlToken.EndElem:
								case BinXmlToken.Element:
									return XmlConvert.ToSingle(string.Empty);
								default:
									goto IL_014D;
								}
								break;
							}
							break;
						}
						throw new InvalidCastException(Res.GetString("XmlBinary_CastNotSupported", new object[] { this.token, "Float" }));
						IL_014D:
						goto IL_01A3;
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Float", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Float", ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Float", ex3, null);
					}
					IL_018C:
					num = this.FinishContentAsXXX(num);
					return num2;
				}
			}
			finally
			{
				this.pos = num;
			}
			IL_01A3:
			return base.ReadContentAsFloat();
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00043918 File Offset: 0x00042918
		public override decimal ReadContentAsDecimal()
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsDecimal"))
				{
					decimal num2;
					try
					{
						BinXmlToken binXmlToken = this.token;
						switch (binXmlToken)
						{
						case BinXmlToken.SQL_SMALLINT:
						case BinXmlToken.SQL_INT:
						case BinXmlToken.SQL_MONEY:
						case BinXmlToken.SQL_BIT:
						case BinXmlToken.SQL_TINYINT:
						case BinXmlToken.SQL_BIGINT:
						case BinXmlToken.SQL_DECIMAL:
						case BinXmlToken.SQL_NUMERIC:
						case BinXmlToken.SQL_SMALLMONEY:
							break;
						case BinXmlToken.SQL_REAL:
						case BinXmlToken.SQL_FLOAT:
						case BinXmlToken.SQL_UUID:
						case BinXmlToken.SQL_BINARY:
						case BinXmlToken.SQL_VARBINARY:
						case BinXmlToken.SQL_DATETIME:
						case BinXmlToken.SQL_SMALLDATETIME:
						case BinXmlToken.SQL_IMAGE:
						case BinXmlToken.SQL_UDT:
							goto IL_010A;
						case BinXmlToken.SQL_CHAR:
						case BinXmlToken.SQL_NCHAR:
						case BinXmlToken.SQL_VARCHAR:
						case BinXmlToken.SQL_NVARCHAR:
						case BinXmlToken.SQL_TEXT:
						case BinXmlToken.SQL_NTEXT:
							goto IL_01A2;
						case (BinXmlToken)21:
						case (BinXmlToken)25:
						case (BinXmlToken)26:
							goto IL_014C;
						default:
							switch (binXmlToken)
							{
							case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATEOFFSET:
							case BinXmlToken.XSD_KATMAI_TIME:
							case BinXmlToken.XSD_KATMAI_DATETIME:
							case BinXmlToken.XSD_KATMAI_DATE:
							case BinXmlToken.XSD_TIME:
							case BinXmlToken.XSD_DATETIME:
							case BinXmlToken.XSD_DATE:
							case BinXmlToken.XSD_BINHEX:
							case BinXmlToken.XSD_BASE64:
							case BinXmlToken.XSD_BOOLEAN:
							case BinXmlToken.XSD_QNAME:
								goto IL_010A;
							case (BinXmlToken)128:
								goto IL_014C;
							case BinXmlToken.XSD_DECIMAL:
							case BinXmlToken.XSD_BYTE:
							case BinXmlToken.XSD_UNSIGNEDSHORT:
							case BinXmlToken.XSD_UNSIGNEDINT:
							case BinXmlToken.XSD_UNSIGNEDLONG:
								break;
							default:
								switch (binXmlToken)
								{
								case BinXmlToken.EndElem:
								case BinXmlToken.Element:
									return XmlConvert.ToDecimal(string.Empty);
								default:
									goto IL_014C;
								}
								break;
							}
							break;
						}
						num2 = this.ValueAsDecimal();
						goto IL_018B;
						IL_010A:
						throw new InvalidCastException(Res.GetString("XmlBinary_CastNotSupported", new object[] { this.token, "Decimal" }));
						IL_014C:
						goto IL_01A2;
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Decimal", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Decimal", ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Decimal", ex3, null);
					}
					IL_018B:
					num = this.FinishContentAsXXX(num);
					return num2;
				}
			}
			finally
			{
				this.pos = num;
			}
			IL_01A2:
			return base.ReadContentAsDecimal();
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00043B34 File Offset: 0x00042B34
		public override int ReadContentAsInt()
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsInt"))
				{
					int num2;
					try
					{
						BinXmlToken binXmlToken = this.token;
						switch (binXmlToken)
						{
						case BinXmlToken.SQL_SMALLINT:
						case BinXmlToken.SQL_INT:
						case BinXmlToken.SQL_MONEY:
						case BinXmlToken.SQL_BIT:
						case BinXmlToken.SQL_TINYINT:
						case BinXmlToken.SQL_BIGINT:
						case BinXmlToken.SQL_DECIMAL:
						case BinXmlToken.SQL_NUMERIC:
						case BinXmlToken.SQL_SMALLMONEY:
							break;
						case BinXmlToken.SQL_REAL:
						case BinXmlToken.SQL_FLOAT:
						case BinXmlToken.SQL_UUID:
						case BinXmlToken.SQL_BINARY:
						case BinXmlToken.SQL_VARBINARY:
						case BinXmlToken.SQL_DATETIME:
						case BinXmlToken.SQL_SMALLDATETIME:
						case BinXmlToken.SQL_IMAGE:
						case BinXmlToken.SQL_UDT:
							goto IL_010B;
						case BinXmlToken.SQL_CHAR:
						case BinXmlToken.SQL_NCHAR:
						case BinXmlToken.SQL_VARCHAR:
						case BinXmlToken.SQL_NVARCHAR:
						case BinXmlToken.SQL_TEXT:
						case BinXmlToken.SQL_NTEXT:
							goto IL_01A3;
						case (BinXmlToken)21:
						case (BinXmlToken)25:
						case (BinXmlToken)26:
							goto IL_014D;
						default:
							switch (binXmlToken)
							{
							case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATEOFFSET:
							case BinXmlToken.XSD_KATMAI_TIME:
							case BinXmlToken.XSD_KATMAI_DATETIME:
							case BinXmlToken.XSD_KATMAI_DATE:
							case BinXmlToken.XSD_TIME:
							case BinXmlToken.XSD_DATETIME:
							case BinXmlToken.XSD_DATE:
							case BinXmlToken.XSD_BINHEX:
							case BinXmlToken.XSD_BASE64:
							case BinXmlToken.XSD_BOOLEAN:
							case BinXmlToken.XSD_QNAME:
								goto IL_010B;
							case (BinXmlToken)128:
								goto IL_014D;
							case BinXmlToken.XSD_DECIMAL:
							case BinXmlToken.XSD_BYTE:
							case BinXmlToken.XSD_UNSIGNEDSHORT:
							case BinXmlToken.XSD_UNSIGNEDINT:
							case BinXmlToken.XSD_UNSIGNEDLONG:
								break;
							default:
								switch (binXmlToken)
								{
								case BinXmlToken.EndElem:
								case BinXmlToken.Element:
									return XmlConvert.ToInt32(string.Empty);
								default:
									goto IL_014D;
								}
								break;
							}
							break;
						}
						num2 = checked((int)this.ValueAsLong());
						goto IL_018C;
						IL_010B:
						throw new InvalidCastException(Res.GetString("XmlBinary_CastNotSupported", new object[] { this.token, "Int32" }));
						IL_014D:
						goto IL_01A3;
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Int32", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Int32", ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Int32", ex3, null);
					}
					IL_018C:
					num = this.FinishContentAsXXX(num);
					return num2;
				}
			}
			finally
			{
				this.pos = num;
			}
			IL_01A3:
			return base.ReadContentAsInt();
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00043D54 File Offset: 0x00042D54
		public override long ReadContentAsLong()
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsLong"))
				{
					long num2;
					try
					{
						BinXmlToken binXmlToken = this.token;
						switch (binXmlToken)
						{
						case BinXmlToken.SQL_SMALLINT:
						case BinXmlToken.SQL_INT:
						case BinXmlToken.SQL_MONEY:
						case BinXmlToken.SQL_BIT:
						case BinXmlToken.SQL_TINYINT:
						case BinXmlToken.SQL_BIGINT:
						case BinXmlToken.SQL_DECIMAL:
						case BinXmlToken.SQL_NUMERIC:
						case BinXmlToken.SQL_SMALLMONEY:
							break;
						case BinXmlToken.SQL_REAL:
						case BinXmlToken.SQL_FLOAT:
						case BinXmlToken.SQL_UUID:
						case BinXmlToken.SQL_BINARY:
						case BinXmlToken.SQL_VARBINARY:
						case BinXmlToken.SQL_DATETIME:
						case BinXmlToken.SQL_SMALLDATETIME:
						case BinXmlToken.SQL_IMAGE:
						case BinXmlToken.SQL_UDT:
							goto IL_010A;
						case BinXmlToken.SQL_CHAR:
						case BinXmlToken.SQL_NCHAR:
						case BinXmlToken.SQL_VARCHAR:
						case BinXmlToken.SQL_NVARCHAR:
						case BinXmlToken.SQL_TEXT:
						case BinXmlToken.SQL_NTEXT:
							goto IL_01A2;
						case (BinXmlToken)21:
						case (BinXmlToken)25:
						case (BinXmlToken)26:
							goto IL_014C;
						default:
							switch (binXmlToken)
							{
							case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
							case BinXmlToken.XSD_KATMAI_DATEOFFSET:
							case BinXmlToken.XSD_KATMAI_TIME:
							case BinXmlToken.XSD_KATMAI_DATETIME:
							case BinXmlToken.XSD_KATMAI_DATE:
							case BinXmlToken.XSD_TIME:
							case BinXmlToken.XSD_DATETIME:
							case BinXmlToken.XSD_DATE:
							case BinXmlToken.XSD_BINHEX:
							case BinXmlToken.XSD_BASE64:
							case BinXmlToken.XSD_BOOLEAN:
							case BinXmlToken.XSD_QNAME:
								goto IL_010A;
							case (BinXmlToken)128:
								goto IL_014C;
							case BinXmlToken.XSD_DECIMAL:
							case BinXmlToken.XSD_BYTE:
							case BinXmlToken.XSD_UNSIGNEDSHORT:
							case BinXmlToken.XSD_UNSIGNEDINT:
							case BinXmlToken.XSD_UNSIGNEDLONG:
								break;
							default:
								switch (binXmlToken)
								{
								case BinXmlToken.EndElem:
								case BinXmlToken.Element:
									return XmlConvert.ToInt64(string.Empty);
								default:
									goto IL_014C;
								}
								break;
							}
							break;
						}
						num2 = this.ValueAsLong();
						goto IL_018B;
						IL_010A:
						throw new InvalidCastException(Res.GetString("XmlBinary_CastNotSupported", new object[] { this.token, "Int64" }));
						IL_014C:
						goto IL_01A2;
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Int64", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Int64", ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Int64", ex3, null);
					}
					IL_018B:
					num = this.FinishContentAsXXX(num);
					return num2;
				}
			}
			finally
			{
				this.pos = num;
			}
			IL_01A2:
			return base.ReadContentAsLong();
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00043F70 File Offset: 0x00042F70
		public override object ReadContentAsObject()
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAsObject"))
				{
					object obj;
					try
					{
						if (this.NodeType == XmlNodeType.Element || this.NodeType == XmlNodeType.EndElement)
						{
							obj = string.Empty;
						}
						else
						{
							obj = this.ValueAsObject(this.token, false);
						}
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Object", ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Object", ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", "Object", ex3, null);
					}
					num = this.FinishContentAsXXX(num);
					return obj;
				}
			}
			finally
			{
				this.pos = num;
			}
			return base.ReadContentAsObject();
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0004404C File Offset: 0x0004304C
		public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			int num = this.pos;
			try
			{
				if (this.SetupContentAsXXX("ReadContentAs"))
				{
					object obj;
					try
					{
						if (this.NodeType == XmlNodeType.Element || this.NodeType == XmlNodeType.EndElement)
						{
							obj = string.Empty;
						}
						else if (returnType == this.ValueType || returnType == typeof(object))
						{
							obj = this.ValueAsObject(this.token, false);
						}
						else
						{
							obj = this.ValueAs(this.token, returnType, namespaceResolver);
						}
					}
					catch (InvalidCastException ex)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", returnType.ToString(), ex, null);
					}
					catch (FormatException ex2)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", returnType.ToString(), ex2, null);
					}
					catch (OverflowException ex3)
					{
						throw new XmlException("Xml_ReadContentAsFormatException", returnType.ToString(), ex3, null);
					}
					num = this.FinishContentAsXXX(num);
					return obj;
				}
			}
			finally
			{
				this.pos = num;
			}
			return base.ReadContentAs(returnType, namespaceResolver);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00044158 File Offset: 0x00043158
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				IXmlNamespaceResolver xmlNamespaceResolver = (IXmlNamespaceResolver)this.textXmlReader;
				return xmlNamespaceResolver.GetNamespacesInScope(scope);
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (XmlNamespaceScope.Local == scope)
			{
				if (this.elemDepth > 0)
				{
					for (XmlSqlBinaryReader.NamespaceDecl namespaceDecl = this.elementStack[this.elemDepth].nsdecls; namespaceDecl != null; namespaceDecl = namespaceDecl.scopeLink)
					{
						dictionary.Add(namespaceDecl.prefix, namespaceDecl.uri);
					}
				}
			}
			else
			{
				foreach (XmlSqlBinaryReader.NamespaceDecl namespaceDecl2 in this.namespaces.Values)
				{
					if ((namespaceDecl2.scope != -1 || (scope == XmlNamespaceScope.All && "xml" == namespaceDecl2.prefix)) && (namespaceDecl2.prefix.Length > 0 || namespaceDecl2.uri.Length > 0))
					{
						dictionary.Add(namespaceDecl2.prefix, namespaceDecl2.uri);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00044264 File Offset: 0x00043264
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			if (XmlSqlBinaryReader.ScanState.XmlText == this.state)
			{
				IXmlNamespaceResolver xmlNamespaceResolver = (IXmlNamespaceResolver)this.textXmlReader;
				return xmlNamespaceResolver.LookupPrefix(namespaceName);
			}
			if (namespaceName == null)
			{
				return null;
			}
			namespaceName = this.xnt.Get(namespaceName);
			if (namespaceName == null)
			{
				return null;
			}
			for (int i = this.elemDepth; i >= 0; i--)
			{
				for (XmlSqlBinaryReader.NamespaceDecl namespaceDecl = this.elementStack[i].nsdecls; namespaceDecl != null; namespaceDecl = namespaceDecl.scopeLink)
				{
					if (namespaceDecl.uri == namespaceName)
					{
						return namespaceDecl.prefix;
					}
				}
			}
			return null;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x000442E6 File Offset: 0x000432E6
		private void VerifyVersion(int requiredVersion, BinXmlToken token)
		{
			if ((int)this.version < requiredVersion)
			{
				throw this.ThrowUnexpectedToken(token);
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x000442FC File Offset: 0x000432FC
		private void AddInitNamespace(string prefix, string uri)
		{
			XmlSqlBinaryReader.NamespaceDecl namespaceDecl = new XmlSqlBinaryReader.NamespaceDecl(prefix, uri, this.elementStack[0].nsdecls, null, -1, true);
			this.elementStack[0].nsdecls = namespaceDecl;
			this.namespaces.Add(prefix, namespaceDecl);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00044344 File Offset: 0x00043344
		private void AddName()
		{
			string text = this.ParseText();
			int symCount;
			this.symbolTables.symCount = (symCount = this.symbolTables.symCount) + 1;
			int num = symCount;
			string[] array = this.symbolTables.symtable;
			if (num == array.Length)
			{
				string[] array2 = new string[checked(num * 2)];
				Array.Copy(array, 0, array2, 0, num);
				array = (this.symbolTables.symtable = array2);
			}
			array[num] = this.xnt.Add(text);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x000443B4 File Offset: 0x000433B4
		private void AddQName()
		{
			int num = this.ReadNameRef();
			int num2 = this.ReadNameRef();
			int num3 = this.ReadNameRef();
			int qnameCount;
			this.symbolTables.qnameCount = (qnameCount = this.symbolTables.qnameCount) + 1;
			int num4 = qnameCount;
			XmlSqlBinaryReader.QName[] array = this.symbolTables.qnametable;
			if (num4 == array.Length)
			{
				XmlSqlBinaryReader.QName[] array2 = new XmlSqlBinaryReader.QName[checked(num4 * 2)];
				Array.Copy(array, 0, array2, 0, num4);
				array = (this.symbolTables.qnametable = array2);
			}
			string[] symtable = this.symbolTables.symtable;
			string text = symtable[num2];
			string text2;
			string text3;
			if (num3 == 0)
			{
				if (num2 == 0 && num == 0)
				{
					return;
				}
				if (text.StartsWith("xmlns", StringComparison.Ordinal))
				{
					if (5 < text.Length)
					{
						if (6 == text.Length || ':' != text[5])
						{
							goto IL_0108;
						}
						text2 = this.xnt.Add(text.Substring(6));
						text = this.xmlns;
					}
					else
					{
						text2 = text;
						text = string.Empty;
					}
					text3 = this.nsxmlns;
					goto IL_00F4;
				}
				IL_0108:
				throw new XmlException("Xml_BadNamespaceDecl", null);
			}
			else
			{
				text2 = symtable[num3];
				text3 = symtable[num];
			}
			IL_00F4:
			array[num4].Set(text, text2, text3);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x000444D4 File Offset: 0x000434D4
		private void NameFlush()
		{
			this.symbolTables.symCount = (this.symbolTables.qnameCount = 1);
			Array.Clear(this.symbolTables.symtable, 1, this.symbolTables.symtable.Length - 1);
			Array.Clear(this.symbolTables.qnametable, 0, this.symbolTables.qnametable.Length);
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0004453C File Offset: 0x0004353C
		private void SkipExtn()
		{
			int num = this.ParseMB32();
			checked
			{
				this.pos += num;
				this.Fill(-1);
			}
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00044568 File Offset: 0x00043568
		private int ReadQNameRef()
		{
			int num = this.ParseMB32();
			if (num < 0 || num >= this.symbolTables.qnameCount)
			{
				throw new XmlException("XmlBin_InvalidQNameID", string.Empty);
			}
			return num;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000445A0 File Offset: 0x000435A0
		private int ReadNameRef()
		{
			int num = this.ParseMB32();
			if (num < 0 || num >= this.symbolTables.symCount)
			{
				throw new XmlException("XmlBin_InvalidQNameID", string.Empty);
			}
			return num;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000445D8 File Offset: 0x000435D8
		private bool FillAllowEOF()
		{
			if (this.eof)
			{
				return false;
			}
			byte[] array = this.data;
			int num = this.pos;
			int num2 = this.mark;
			int num3 = this.end;
			if (num2 == -1)
			{
				num2 = num;
			}
			if (num2 >= 0 && num2 < num3)
			{
				int num4 = num3 - num2;
				if (num4 > 7 * (array.Length / 8))
				{
					byte[] array2 = new byte[checked(array.Length * 2)];
					Array.Copy(array, num2, array2, 0, num4);
					array = (this.data = array2);
				}
				else
				{
					Array.Copy(array, num2, array, 0, num4);
				}
				num -= num2;
				num3 -= num2;
				this.tokDataPos -= num2;
				for (int i = 0; i < this.attrCount; i++)
				{
					this.attributes[i].AdjustPosition(-num2);
				}
				this.pos = num;
				this.mark = 0;
				this.offset += (long)num2;
			}
			else
			{
				this.pos -= num3;
				this.offset += (long)num3;
				this.tokDataPos -= num3;
				num3 = 0;
			}
			int num5 = array.Length - num3;
			int num6 = this.inStrm.Read(array, num3, num5);
			this.end = num3 + num6;
			this.eof = num6 <= 0;
			return num6 > 0;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0004471E File Offset: 0x0004371E
		private void Fill_(int require)
		{
			while (this.FillAllowEOF() && this.pos + require >= this.end)
			{
			}
			if (this.pos + require >= this.end)
			{
				throw this.ThrowXmlException("Xml_UnexpectedEOF1");
			}
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00044754 File Offset: 0x00043754
		private void Fill(int require)
		{
			if (this.pos + require >= this.end)
			{
				this.Fill_(require);
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00044770 File Offset: 0x00043770
		private byte ReadByte()
		{
			this.Fill(0);
			return this.data[this.pos++];
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0004479C File Offset: 0x0004379C
		private ushort ReadUShort()
		{
			this.Fill(1);
			int num = this.pos;
			byte[] array = this.data;
			ushort num2 = (ushort)((int)array[num] + ((int)array[num + 1] << 8));
			this.pos += 2;
			return num2;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x000447DC File Offset: 0x000437DC
		private int ParseMB32()
		{
			byte b = this.ReadByte();
			if (b > 127)
			{
				return this.ParseMB32_(b);
			}
			return (int)b;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00044800 File Offset: 0x00043800
		private int ParseMB32_(byte b)
		{
			uint num = (uint)(b & 127);
			b = this.ReadByte();
			uint num2 = (uint)(b & 127);
			num += num2 << 7;
			if (b > 127)
			{
				b = this.ReadByte();
				num2 = (uint)(b & 127);
				num += num2 << 14;
				if (b > 127)
				{
					b = this.ReadByte();
					num2 = (uint)(b & 127);
					num += num2 << 21;
					if (b > 127)
					{
						b = this.ReadByte();
						num2 = (uint)(b & 7);
						if (b > 7)
						{
							throw this.ThrowXmlException("XmlBinary_ValueTooBig");
						}
						num += num2 << 28;
					}
				}
			}
			return (int)num;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00044880 File Offset: 0x00043880
		private int ParseMB32(int pos)
		{
			byte[] array = this.data;
			byte b = array[pos++];
			uint num = (uint)(b & 127);
			if (b > 127)
			{
				b = array[pos++];
				uint num2 = (uint)(b & 127);
				num += num2 << 7;
				if (b > 127)
				{
					b = array[pos++];
					num2 = (uint)(b & 127);
					num += num2 << 14;
					if (b > 127)
					{
						b = array[pos++];
						num2 = (uint)(b & 127);
						num += num2 << 21;
						if (b > 127)
						{
							b = array[pos++];
							num2 = (uint)(b & 7);
							if (b > 7)
							{
								throw this.ThrowXmlException("XmlBinary_ValueTooBig");
							}
							num += num2 << 28;
						}
					}
				}
			}
			return (int)num;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0004491C File Offset: 0x0004391C
		private int ParseMB64()
		{
			byte b = this.ReadByte();
			if (b > 127)
			{
				return this.ParseMB32_(b);
			}
			return (int)b;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0004493E File Offset: 0x0004393E
		private BinXmlToken PeekToken()
		{
			while (this.pos >= this.end && this.FillAllowEOF())
			{
			}
			if (this.pos >= this.end)
			{
				return BinXmlToken.EOF;
			}
			return (BinXmlToken)this.data[this.pos];
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00044974 File Offset: 0x00043974
		private BinXmlToken ReadToken()
		{
			while (this.pos >= this.end && this.FillAllowEOF())
			{
			}
			if (this.pos >= this.end)
			{
				return BinXmlToken.EOF;
			}
			return (BinXmlToken)this.data[this.pos++];
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x000449C0 File Offset: 0x000439C0
		private BinXmlToken NextToken2(BinXmlToken token)
		{
			for (;;)
			{
				BinXmlToken binXmlToken = token;
				switch (binXmlToken)
				{
				case BinXmlToken.NmFlush:
					this.NameFlush();
					break;
				case BinXmlToken.Extn:
					this.SkipExtn();
					break;
				default:
					switch (binXmlToken)
					{
					case BinXmlToken.QName:
						this.AddQName();
						goto IL_004E;
					case BinXmlToken.Name:
						this.AddName();
						goto IL_004E;
					}
					return token;
				}
				IL_004E:
				token = this.ReadToken();
			}
			return token;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00044A24 File Offset: 0x00043A24
		private BinXmlToken NextToken1()
		{
			int num = this.pos;
			BinXmlToken binXmlToken;
			if (num >= this.end)
			{
				binXmlToken = this.ReadToken();
			}
			else
			{
				binXmlToken = (BinXmlToken)this.data[num];
				this.pos = num + 1;
			}
			if (binXmlToken >= BinXmlToken.NmFlush && binXmlToken <= BinXmlToken.Name)
			{
				return this.NextToken2(binXmlToken);
			}
			return binXmlToken;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00044A78 File Offset: 0x00043A78
		private BinXmlToken NextToken()
		{
			int num = this.pos;
			if (num < this.end)
			{
				BinXmlToken binXmlToken = (BinXmlToken)this.data[num];
				if (binXmlToken < BinXmlToken.NmFlush || binXmlToken > BinXmlToken.Name)
				{
					this.pos = num + 1;
					return binXmlToken;
				}
			}
			return this.NextToken1();
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00044AC0 File Offset: 0x00043AC0
		private BinXmlToken PeekNextToken()
		{
			BinXmlToken binXmlToken = this.NextToken();
			if (BinXmlToken.EOF != binXmlToken)
			{
				this.pos--;
			}
			return binXmlToken;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00044AE8 File Offset: 0x00043AE8
		private BinXmlToken RescanNextToken()
		{
			checked
			{
				BinXmlToken binXmlToken;
				for (;;)
				{
					binXmlToken = this.ReadToken();
					BinXmlToken binXmlToken2 = binXmlToken;
					switch (binXmlToken2)
					{
					case BinXmlToken.NmFlush:
						break;
					case BinXmlToken.Extn:
					{
						int num = this.ParseMB32();
						this.pos += num;
						break;
					}
					default:
						switch (binXmlToken2)
						{
						case BinXmlToken.QName:
							this.ParseMB32();
							this.ParseMB32();
							this.ParseMB32();
							continue;
						case BinXmlToken.Name:
						{
							int num2 = this.ParseMB32();
							this.pos += 2 * num2;
							continue;
						}
						}
						return binXmlToken;
					}
				}
				return binXmlToken;
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00044B70 File Offset: 0x00043B70
		private string ParseText()
		{
			int num = this.mark;
			string @string;
			try
			{
				if (num < 0)
				{
					this.mark = this.pos;
				}
				int num3;
				int num2 = this.ScanText(out num3);
				@string = this.GetString(num3, num2);
			}
			finally
			{
				if (num < 0)
				{
					this.mark = -1;
				}
			}
			return @string;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00044BC8 File Offset: 0x00043BC8
		private int ScanText(out int start)
		{
			int num = this.ParseMB32();
			int num2 = this.mark;
			int num3 = this.pos;
			checked
			{
				this.pos += num * 2;
				if (this.pos > this.end)
				{
					this.Fill(-1);
				}
			}
			start = num3 - (num2 - this.mark);
			return num;
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00044C1C File Offset: 0x00043C1C
		private string GetString(int pos, int cch)
		{
			checked
			{
				if (pos + cch * 2 > this.end)
				{
					throw new XmlException("Xml_UnexpectedEOF1", null);
				}
				if (cch == 0)
				{
					return string.Empty;
				}
				if ((pos & 1) == 0)
				{
					return this.GetStringAligned(this.data, pos, cch);
				}
				return this.unicode.GetString(this.data, pos, cch * 2);
			}
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00044C74 File Offset: 0x00043C74
		private unsafe string GetStringAligned(byte[] data, int offset, int cch)
		{
			fixed (byte* ptr = data)
			{
				char* ptr2 = (char*)ptr + offset / 2;
				return new string(ptr2, 0, cch);
			}
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00044CAC File Offset: 0x00043CAC
		private string GetAttributeText(int i)
		{
			string val = this.attributes[i].val;
			if (val != null)
			{
				return val;
			}
			int num = this.pos;
			string text;
			try
			{
				this.pos = this.attributes[i].contentPos;
				BinXmlToken binXmlToken = this.RescanNextToken();
				if (BinXmlToken.Attr == binXmlToken || BinXmlToken.EndAttrs == binXmlToken)
				{
					text = "";
				}
				else
				{
					this.token = binXmlToken;
					this.ReScanOverValue(binXmlToken);
					text = this.ValueAsString(binXmlToken);
				}
			}
			finally
			{
				this.pos = num;
			}
			return text;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00044D40 File Offset: 0x00043D40
		private int LocateAttribute(string name, string ns)
		{
			for (int i = 0; i < this.attrCount; i++)
			{
				if (this.attributes[i].name.MatchNs(name, ns))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00044D7C File Offset: 0x00043D7C
		private int LocateAttribute(string name)
		{
			string text;
			string text2;
			ValidateNames.SplitQName(name, out text, out text2);
			for (int i = 0; i < this.attrCount; i++)
			{
				if (this.attributes[i].name.MatchPrefix(text, text2))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00044DC4 File Offset: 0x00043DC4
		private void PositionOnAttribute(int i)
		{
			this.attrIndex = i;
			this.qnameOther = this.attributes[i - 1].name;
			if (this.state == XmlSqlBinaryReader.ScanState.Doc)
			{
				this.parentNodeType = this.nodetype;
			}
			this.token = BinXmlToken.Attr;
			this.nodetype = XmlNodeType.Attribute;
			this.state = XmlSqlBinaryReader.ScanState.Attr;
			this.valueType = XmlSqlBinaryReader.TypeOfObject;
			this.stringValue = null;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00044E30 File Offset: 0x00043E30
		private void GrowElements()
		{
			int num = this.elementStack.Length * 2;
			XmlSqlBinaryReader.ElemInfo[] array = new XmlSqlBinaryReader.ElemInfo[num];
			Array.Copy(this.elementStack, 0, array, 0, this.elementStack.Length);
			this.elementStack = array;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00044E6C File Offset: 0x00043E6C
		private void GrowAttributes()
		{
			int num = this.attributes.Length * 2;
			XmlSqlBinaryReader.AttrInfo[] array = new XmlSqlBinaryReader.AttrInfo[num];
			Array.Copy(this.attributes, 0, array, 0, this.attrCount);
			this.attributes = array;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00044EA6 File Offset: 0x00043EA6
		private void ClearAttributes()
		{
			if (this.attrCount != 0)
			{
				this.attrCount = 0;
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00044EB8 File Offset: 0x00043EB8
		private void PushNamespace(string prefix, string ns, bool implied)
		{
			if (prefix == "xml")
			{
				return;
			}
			int num = this.elemDepth;
			XmlSqlBinaryReader.NamespaceDecl namespaceDecl;
			this.namespaces.TryGetValue(prefix, out namespaceDecl);
			if (namespaceDecl != null)
			{
				if (namespaceDecl.uri == ns)
				{
					if (!implied && namespaceDecl.implied && namespaceDecl.scope == num)
					{
						namespaceDecl.implied = false;
					}
					return;
				}
				this.qnameElement.CheckPrefixNS(prefix, ns);
				if (prefix.Length != 0)
				{
					for (int i = 0; i < this.attrCount; i++)
					{
						if (this.attributes[i].name.prefix.Length != 0)
						{
							this.attributes[i].name.CheckPrefixNS(prefix, ns);
						}
					}
				}
			}
			XmlSqlBinaryReader.NamespaceDecl namespaceDecl2 = new XmlSqlBinaryReader.NamespaceDecl(prefix, ns, this.elementStack[num].nsdecls, namespaceDecl, num, implied);
			this.elementStack[num].nsdecls = namespaceDecl2;
			this.namespaces[prefix] = namespaceDecl2;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00044FB0 File Offset: 0x00043FB0
		private void PopNamespaces(XmlSqlBinaryReader.NamespaceDecl firstInScopeChain)
		{
			XmlSqlBinaryReader.NamespaceDecl scopeLink;
			for (XmlSqlBinaryReader.NamespaceDecl namespaceDecl = firstInScopeChain; namespaceDecl != null; namespaceDecl = scopeLink)
			{
				if (namespaceDecl.prevLink == null)
				{
					this.namespaces.Remove(namespaceDecl.prefix);
				}
				else
				{
					this.namespaces[namespaceDecl.prefix] = namespaceDecl.prevLink;
				}
				scopeLink = namespaceDecl.scopeLink;
				namespaceDecl.prevLink = null;
				namespaceDecl.scopeLink = null;
			}
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00045010 File Offset: 0x00044010
		private void GenerateImpliedXmlnsAttrs()
		{
			for (XmlSqlBinaryReader.NamespaceDecl namespaceDecl = this.elementStack[this.elemDepth].nsdecls; namespaceDecl != null; namespaceDecl = namespaceDecl.scopeLink)
			{
				if (namespaceDecl.implied)
				{
					if (this.attrCount == this.attributes.Length)
					{
						this.GrowAttributes();
					}
					XmlSqlBinaryReader.QName qname;
					if (namespaceDecl.prefix.Length == 0)
					{
						qname = new XmlSqlBinaryReader.QName(string.Empty, this.xmlns, this.nsxmlns);
					}
					else
					{
						qname = new XmlSqlBinaryReader.QName(this.xmlns, this.xnt.Add(namespaceDecl.prefix), this.nsxmlns);
					}
					this.attributes[this.attrCount].Set(qname, namespaceDecl.uri);
					this.attrCount++;
				}
			}
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x000450E0 File Offset: 0x000440E0
		private bool ReadInit(bool skipXmlDecl)
		{
			string text;
			if (!this.sniffed)
			{
				ushort num = this.ReadUShort();
				if (num != 65503)
				{
					text = "XmlBinary_InvalidSignature";
					goto IL_01F5;
				}
			}
			this.version = this.ReadByte();
			if (this.version != 1 && this.version != 2)
			{
				text = "XmlBinary_InvalidProtocolVersion";
			}
			else
			{
				if (1200 == this.ReadUShort())
				{
					this.state = XmlSqlBinaryReader.ScanState.Doc;
					if (BinXmlToken.XmlDecl == this.PeekToken())
					{
						this.pos++;
						this.attributes[0].Set(new XmlSqlBinaryReader.QName(string.Empty, this.xnt.Add("version"), string.Empty), this.ParseText());
						this.attrCount = 1;
						if (BinXmlToken.Encoding == this.PeekToken())
						{
							this.pos++;
							this.attributes[1].Set(new XmlSqlBinaryReader.QName(string.Empty, this.xnt.Add("encoding"), string.Empty), this.ParseText());
							this.attrCount++;
						}
						byte b = this.ReadByte();
						switch (b)
						{
						case 0:
							break;
						case 1:
						case 2:
							this.attributes[this.attrCount].Set(new XmlSqlBinaryReader.QName(string.Empty, this.xnt.Add("standalone"), string.Empty), (b == 1) ? "yes" : "no");
							this.attrCount++;
							break;
						default:
							text = "XmlBinary_InvalidStandalone";
							goto IL_01F5;
						}
						if (!skipXmlDecl)
						{
							XmlSqlBinaryReader.QName qname = new XmlSqlBinaryReader.QName(string.Empty, this.xnt.Add("xml"), string.Empty);
							this.qnameOther = (this.qnameElement = qname);
							this.nodetype = XmlNodeType.XmlDeclaration;
							this.posAfterAttrs = this.pos;
							return true;
						}
					}
					return this.ReadDoc();
				}
				text = "XmlBinary_UnsupportedCodePage";
			}
			IL_01F5:
			this.state = XmlSqlBinaryReader.ScanState.Error;
			throw new XmlException(text, null);
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x000452F0 File Offset: 0x000442F0
		private void ScanAttributes()
		{
			int num = -1;
			int num2 = -1;
			this.mark = this.pos;
			string text = null;
			bool flag = false;
			BinXmlToken binXmlToken;
			while (BinXmlToken.EndAttrs != (binXmlToken = this.NextToken()))
			{
				if (BinXmlToken.Attr == binXmlToken)
				{
					if (text != null)
					{
						this.PushNamespace(text, string.Empty, false);
						text = null;
					}
					if (this.attrCount == this.attributes.Length)
					{
						this.GrowAttributes();
					}
					XmlSqlBinaryReader.QName qname = this.symbolTables.qnametable[this.ReadQNameRef()];
					this.attributes[this.attrCount].Set(qname, this.pos);
					if (qname.prefix == "xml")
					{
						if (qname.localname == "lang")
						{
							num2 = this.attrCount;
						}
						else if (qname.localname == "space")
						{
							num = this.attrCount;
						}
					}
					else if (object.Equals(qname.namespaceUri, this.nsxmlns))
					{
						text = qname.localname;
						if (text == "xmlns")
						{
							text = string.Empty;
						}
					}
					else if (qname.prefix.Length != 0)
					{
						if (qname.namespaceUri.Length == 0)
						{
							throw new XmlException("Xml_PrefixForEmptyNs", string.Empty);
						}
						this.PushNamespace(qname.prefix, qname.namespaceUri, true);
					}
					else if (qname.namespaceUri.Length != 0)
					{
						throw this.ThrowXmlException("XmlBinary_AttrWithNsNoPrefix", qname.localname, qname.namespaceUri);
					}
					this.attrCount++;
					flag = false;
				}
				else
				{
					this.ScanOverValue(binXmlToken, true, true);
					if (flag)
					{
						throw this.ThrowNotSupported("XmlBinary_ListsOfValuesNotSupported");
					}
					string text2 = this.stringValue;
					if (text2 != null)
					{
						this.attributes[this.attrCount - 1].val = text2;
						this.stringValue = null;
					}
					if (text != null)
					{
						string text3 = this.xnt.Add(this.ValueAsString(binXmlToken));
						this.PushNamespace(text, text3, false);
						text = null;
					}
					flag = true;
				}
			}
			if (num != -1)
			{
				string attributeText = this.GetAttributeText(num);
				XmlSpace xmlSpace = XmlSpace.None;
				if (attributeText == "preserve")
				{
					xmlSpace = XmlSpace.Preserve;
				}
				else if (attributeText == "default")
				{
					xmlSpace = XmlSpace.Default;
				}
				this.elementStack[this.elemDepth].xmlSpace = xmlSpace;
				this.xmlspacePreserve = XmlSpace.Preserve == xmlSpace;
			}
			if (num2 != -1)
			{
				this.elementStack[this.elemDepth].xmlLang = this.GetAttributeText(num2);
			}
			if (this.attrCount < 200)
			{
				this.SimpleCheckForDuplicateAttributes();
				return;
			}
			this.HashCheckForDuplicateAttributes();
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0004559C File Offset: 0x0004459C
		private void SimpleCheckForDuplicateAttributes()
		{
			for (int i = 0; i < this.attrCount; i++)
			{
				string text;
				string text2;
				this.attributes[i].GetLocalnameAndNamespaceUri(out text, out text2);
				for (int j = i + 1; j < this.attrCount; j++)
				{
					if (this.attributes[j].MatchNS(text, text2))
					{
						throw new XmlException("Xml_DupAttributeName", this.attributes[i].name.ToString());
					}
				}
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00045620 File Offset: 0x00044620
		private void HashCheckForDuplicateAttributes()
		{
			int i;
			checked
			{
				for (i = 256; i < this.attrCount; i *= 2)
				{
				}
				if (this.attrHashTbl.Length < i)
				{
					this.attrHashTbl = new int[i];
				}
			}
			for (int j = 0; j < this.attrCount; j++)
			{
				string text;
				string text2;
				int localnameAndNamespaceUriAndHash = this.attributes[j].GetLocalnameAndNamespaceUriAndHash(this.hasher, out text, out text2);
				int num = localnameAndNamespaceUriAndHash & (i - 1);
				int num2 = this.attrHashTbl[num];
				this.attrHashTbl[num] = j + 1;
				this.attributes[j].prevHash = num2;
				while (num2 != 0)
				{
					num2--;
					if (this.attributes[num2].MatchHashNS(localnameAndNamespaceUriAndHash, text, text2))
					{
						throw new XmlException("Xml_DupAttributeName", this.attributes[j].name.ToString());
					}
					num2 = this.attributes[num2].prevHash;
				}
			}
			Array.Clear(this.attrHashTbl, 0, i);
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0004572C File Offset: 0x0004472C
		private string XmlDeclValue()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.attrCount; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(this.attributes[i].name.localname);
				stringBuilder.Append("=\"");
				stringBuilder.Append(this.attributes[i].val);
				stringBuilder.Append('"');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x000457B0 File Offset: 0x000447B0
		private string CDATAValue()
		{
			string text = this.GetString(this.tokDataPos, this.tokLen);
			StringBuilder stringBuilder = null;
			while (this.PeekToken() == BinXmlToken.CData)
			{
				this.pos++;
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(text.Length + text.Length / 2);
					stringBuilder.Append(text);
				}
				stringBuilder.Append(this.ParseText());
			}
			if (stringBuilder != null)
			{
				text = stringBuilder.ToString();
			}
			this.stringValue = text;
			return text;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00045830 File Offset: 0x00044830
		private void FinishCDATA()
		{
			for (;;)
			{
				switch (this.PeekToken())
				{
				case BinXmlToken.EndCData:
					goto IL_0036;
				case BinXmlToken.CData:
				{
					this.pos++;
					int num;
					this.ScanText(out num);
					continue;
				}
				}
				break;
			}
			throw new XmlException("XmlBin_MissingEndCDATA");
			IL_0036:
			this.pos++;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0004588C File Offset: 0x0004488C
		private void FinishEndElement()
		{
			XmlSqlBinaryReader.NamespaceDecl namespaceDecl = this.elementStack[this.elemDepth].Clear();
			this.PopNamespaces(namespaceDecl);
			this.elemDepth--;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000458C8 File Offset: 0x000448C8
		private bool ReadDoc()
		{
			XmlNodeType xmlNodeType = this.nodetype;
			if (xmlNodeType != XmlNodeType.Element)
			{
				if (xmlNodeType != XmlNodeType.CDATA)
				{
					if (xmlNodeType == XmlNodeType.EndElement)
					{
						this.FinishEndElement();
					}
				}
				else
				{
					this.FinishCDATA();
				}
			}
			else if (this.isEmpty)
			{
				this.FinishEndElement();
				this.isEmpty = false;
			}
			for (;;)
			{
				this.nodetype = XmlNodeType.None;
				this.mark = -1;
				if (this.qnameOther.localname.Length != 0)
				{
					this.qnameOther.Clear();
				}
				this.ClearAttributes();
				this.attrCount = 0;
				this.valueType = XmlSqlBinaryReader.TypeOfString;
				this.stringValue = null;
				this.hasTypedValue = false;
				this.token = this.NextToken();
				BinXmlToken binXmlToken = this.token;
				if (binXmlToken <= BinXmlToken.XSD_QNAME)
				{
					switch (binXmlToken)
					{
					case BinXmlToken.EOF:
						goto IL_01D1;
					case BinXmlToken.Error:
					case (BinXmlToken)21:
					case (BinXmlToken)25:
					case (BinXmlToken)26:
						goto IL_02B0;
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_CHAR:
					case BinXmlToken.SQL_NCHAR:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_VARCHAR:
					case BinXmlToken.SQL_NVARCHAR:
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.SQL_TEXT:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_NTEXT:
					case BinXmlToken.SQL_UDT:
						break;
					default:
						switch (binXmlToken)
						{
						case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
						case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
						case BinXmlToken.XSD_KATMAI_DATEOFFSET:
						case BinXmlToken.XSD_KATMAI_TIME:
						case BinXmlToken.XSD_KATMAI_DATETIME:
						case BinXmlToken.XSD_KATMAI_DATE:
						case BinXmlToken.XSD_TIME:
						case BinXmlToken.XSD_DATETIME:
						case BinXmlToken.XSD_DATE:
						case BinXmlToken.XSD_BINHEX:
						case BinXmlToken.XSD_BASE64:
						case BinXmlToken.XSD_BOOLEAN:
						case BinXmlToken.XSD_DECIMAL:
						case BinXmlToken.XSD_BYTE:
						case BinXmlToken.XSD_UNSIGNEDSHORT:
						case BinXmlToken.XSD_UNSIGNEDINT:
						case BinXmlToken.XSD_UNSIGNEDLONG:
						case BinXmlToken.XSD_QNAME:
							goto IL_027E;
						}
						goto Block_8;
					}
					IL_027E:
					this.ImplReadData(this.token);
					if (XmlNodeType.Text == this.nodetype)
					{
						goto Block_16;
					}
					if (!this.ignoreWhitespace || this.xmlspacePreserve)
					{
						return true;
					}
				}
				else
				{
					switch (binXmlToken)
					{
					case BinXmlToken.EndNest:
						goto IL_0261;
					case BinXmlToken.Nest:
						goto IL_024C;
					case BinXmlToken.XmlText:
						goto IL_0276;
					case (BinXmlToken)238:
					case BinXmlToken.QName:
					case BinXmlToken.Name:
					case BinXmlToken.EndCData:
					case BinXmlToken.EndAttrs:
					case BinXmlToken.Attr:
						goto IL_02B0;
					case BinXmlToken.CData:
						goto IL_0244;
					case BinXmlToken.Comment:
						this.ImplReadComment();
						if (!this.ignoreComments)
						{
							return true;
						}
						break;
					case BinXmlToken.PI:
						this.ImplReadPI();
						if (!this.ignorePIs)
						{
							return true;
						}
						break;
					case BinXmlToken.EndElem:
						goto IL_01FA;
					case BinXmlToken.Element:
						goto IL_01EF;
					default:
						if (binXmlToken != BinXmlToken.DocType)
						{
							goto Block_10;
						}
						this.ImplReadDoctype();
						if (this.prevNameInfo == null)
						{
							return true;
						}
						break;
					}
				}
			}
			Block_8:
			Block_10:
			goto IL_02B0;
			IL_01D1:
			if (this.elemDepth > 0)
			{
				throw new XmlException("Xml_UnexpectedEOF1", null);
			}
			this.state = XmlSqlBinaryReader.ScanState.EOF;
			return false;
			IL_01EF:
			this.ImplReadElement();
			return true;
			IL_01FA:
			this.ImplReadEndElement();
			return true;
			IL_0244:
			this.ImplReadCDATA();
			return true;
			IL_024C:
			this.ImplReadNest();
			this.sniffed = false;
			return this.ReadInit(true);
			IL_0261:
			if (this.prevNameInfo != null)
			{
				this.ImplReadEndNest();
				return this.ReadDoc();
			}
			goto IL_02B0;
			IL_0276:
			this.ImplReadXmlText();
			return true;
			Block_16:
			this.CheckAllowContent();
			return true;
			IL_02B0:
			throw this.ThrowUnexpectedToken(this.token);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00045B94 File Offset: 0x00044B94
		private void ImplReadData(BinXmlToken tokenType)
		{
			this.mark = this.pos;
			switch (tokenType)
			{
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_NVARCHAR:
				break;
			case BinXmlToken.SQL_VARBINARY:
				goto IL_0056;
			default:
				switch (tokenType)
				{
				case BinXmlToken.SQL_TEXT:
				case BinXmlToken.SQL_NTEXT:
					break;
				case BinXmlToken.SQL_IMAGE:
					goto IL_0056;
				default:
					goto IL_0056;
				}
				break;
			}
			this.valueType = XmlSqlBinaryReader.TypeOfString;
			this.hasTypedValue = false;
			goto IL_006F;
			IL_0056:
			this.valueType = this.GetValueType(this.token);
			this.hasTypedValue = true;
			IL_006F:
			this.nodetype = this.ScanOverValue(this.token, false, true);
			BinXmlToken binXmlToken = this.PeekNextToken();
			BinXmlToken binXmlToken2 = binXmlToken;
			switch (binXmlToken2)
			{
			case BinXmlToken.SQL_SMALLINT:
			case BinXmlToken.SQL_INT:
			case BinXmlToken.SQL_REAL:
			case BinXmlToken.SQL_FLOAT:
			case BinXmlToken.SQL_MONEY:
			case BinXmlToken.SQL_BIT:
			case BinXmlToken.SQL_TINYINT:
			case BinXmlToken.SQL_BIGINT:
			case BinXmlToken.SQL_UUID:
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
			case BinXmlToken.SQL_SMALLMONEY:
			case BinXmlToken.SQL_TEXT:
			case BinXmlToken.SQL_IMAGE:
			case BinXmlToken.SQL_NTEXT:
			case BinXmlToken.SQL_UDT:
				break;
			case (BinXmlToken)21:
			case (BinXmlToken)25:
			case (BinXmlToken)26:
				return;
			default:
				switch (binXmlToken2)
				{
				case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
				case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
				case BinXmlToken.XSD_KATMAI_DATEOFFSET:
				case BinXmlToken.XSD_KATMAI_TIME:
				case BinXmlToken.XSD_KATMAI_DATETIME:
				case BinXmlToken.XSD_KATMAI_DATE:
				case BinXmlToken.XSD_TIME:
				case BinXmlToken.XSD_DATETIME:
				case BinXmlToken.XSD_DATE:
				case BinXmlToken.XSD_BINHEX:
				case BinXmlToken.XSD_BASE64:
				case BinXmlToken.XSD_BOOLEAN:
				case BinXmlToken.XSD_DECIMAL:
				case BinXmlToken.XSD_BYTE:
				case BinXmlToken.XSD_UNSIGNEDSHORT:
				case BinXmlToken.XSD_UNSIGNEDINT:
				case BinXmlToken.XSD_UNSIGNEDLONG:
				case BinXmlToken.XSD_QNAME:
					break;
				case (BinXmlToken)128:
					return;
				default:
					return;
				}
				break;
			}
			throw this.ThrowNotSupported("XmlBinary_ListsOfValuesNotSupported");
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00045D04 File Offset: 0x00044D04
		private void ImplReadElement()
		{
			if (3 != this.docState || 9 != this.docState)
			{
				switch (this.docState)
				{
				case -1:
					throw this.ThrowUnexpectedToken(this.token);
				case 0:
					this.docState = 9;
					break;
				case 1:
				case 2:
					this.docState = 3;
					break;
				}
			}
			this.elemDepth++;
			if (this.elemDepth == this.elementStack.Length)
			{
				this.GrowElements();
			}
			XmlSqlBinaryReader.QName qname = this.symbolTables.qnametable[this.ReadQNameRef()];
			this.qnameOther = (this.qnameElement = qname);
			this.elementStack[this.elemDepth].Set(qname, this.xmlspacePreserve);
			this.PushNamespace(qname.prefix, qname.namespaceUri, true);
			BinXmlToken binXmlToken = this.PeekNextToken();
			if (BinXmlToken.Attr == binXmlToken)
			{
				this.ScanAttributes();
				binXmlToken = this.PeekNextToken();
			}
			this.GenerateImpliedXmlnsAttrs();
			if (BinXmlToken.EndElem == binXmlToken)
			{
				this.NextToken();
				this.isEmpty = true;
			}
			else if (BinXmlToken.SQL_NVARCHAR == binXmlToken)
			{
				if (this.mark < 0)
				{
					this.mark = this.pos;
				}
				this.pos++;
				if (this.ReadByte() == 0)
				{
					if (247 != this.ReadByte())
					{
						this.pos -= 3;
					}
					else
					{
						this.pos--;
					}
				}
				else
				{
					this.pos -= 2;
				}
			}
			this.nodetype = XmlNodeType.Element;
			this.valueType = XmlSqlBinaryReader.TypeOfObject;
			this.posAfterAttrs = this.pos;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00045EAC File Offset: 0x00044EAC
		private void ImplReadEndElement()
		{
			if (this.elemDepth == 0)
			{
				throw this.ThrowXmlException("Xml_UnexpectedEndTag");
			}
			int num = this.elemDepth;
			if (1 == num && 3 == this.docState)
			{
				this.docState = -1;
			}
			this.qnameOther = this.elementStack[num].name;
			this.xmlspacePreserve = this.elementStack[num].xmlspacePreserve;
			this.nodetype = XmlNodeType.EndElement;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00045F20 File Offset: 0x00044F20
		private void ImplReadDoctype()
		{
			if (this.prohibitDtd)
			{
				throw this.ThrowXmlException("Xml_DtdIsProhibited");
			}
			int num = this.docState;
			switch (num)
			{
			case 0:
			case 1:
				this.docState = 2;
				this.qnameOther.localname = this.ParseText();
				if (BinXmlToken.System == this.PeekToken())
				{
					this.pos++;
					this.attributes[this.attrCount++].Set(new XmlSqlBinaryReader.QName(string.Empty, this.xnt.Add("SYSTEM"), string.Empty), this.ParseText());
				}
				if (BinXmlToken.Public == this.PeekToken())
				{
					this.pos++;
					this.attributes[this.attrCount++].Set(new XmlSqlBinaryReader.QName(string.Empty, this.xnt.Add("PUBLIC"), string.Empty), this.ParseText());
				}
				if (BinXmlToken.Subset == this.PeekToken())
				{
					this.pos++;
					this.mark = this.pos;
					this.tokLen = this.ScanText(out this.tokDataPos);
				}
				else
				{
					this.tokLen = (this.tokDataPos = 0);
				}
				this.nodetype = XmlNodeType.DocumentType;
				this.posAfterAttrs = this.pos;
				return;
			default:
				if (num == 9)
				{
					throw this.ThrowXmlException("Xml_DtdNotAllowedInFragment");
				}
				throw this.ThrowXmlException("Xml_BadDTDLocation");
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000460AC File Offset: 0x000450AC
		private void ImplReadPI()
		{
			this.qnameOther.localname = this.symbolTables.symtable[this.ReadNameRef()];
			this.mark = this.pos;
			this.tokLen = this.ScanText(out this.tokDataPos);
			this.nodetype = XmlNodeType.ProcessingInstruction;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000460FB File Offset: 0x000450FB
		private void ImplReadComment()
		{
			this.nodetype = XmlNodeType.Comment;
			this.mark = this.pos;
			this.tokLen = this.ScanText(out this.tokDataPos);
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00046122 File Offset: 0x00045122
		private void ImplReadCDATA()
		{
			this.CheckAllowContent();
			this.nodetype = XmlNodeType.CDATA;
			this.mark = this.pos;
			this.tokLen = this.ScanText(out this.tokDataPos);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0004614F File Offset: 0x0004514F
		private void ImplReadNest()
		{
			this.CheckAllowContent();
			this.prevNameInfo = new XmlSqlBinaryReader.NestedBinXml(this.symbolTables, this.docState, this.prevNameInfo);
			this.symbolTables.Init();
			this.docState = 0;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00046188 File Offset: 0x00045188
		private void ImplReadEndNest()
		{
			XmlSqlBinaryReader.NestedBinXml nestedBinXml = this.prevNameInfo;
			this.symbolTables = nestedBinXml.symbolTables;
			this.docState = nestedBinXml.docState;
			this.prevNameInfo = nestedBinXml.next;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000461C0 File Offset: 0x000451C0
		private void ImplReadXmlText()
		{
			this.CheckAllowContent();
			string text = this.ParseText();
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.xnt);
			foreach (XmlSqlBinaryReader.NamespaceDecl namespaceDecl in this.namespaces.Values)
			{
				if (namespaceDecl.scope > 0)
				{
					xmlNamespaceManager.AddNamespace(namespaceDecl.prefix, namespaceDecl.uri);
				}
			}
			XmlReaderSettings settings = this.Settings;
			settings.ReadOnly = false;
			settings.NameTable = this.xnt;
			settings.ProhibitDtd = true;
			if (this.elemDepth != 0)
			{
				settings.ConformanceLevel = ConformanceLevel.Fragment;
			}
			settings.ReadOnly = true;
			XmlParserContext xmlParserContext = new XmlParserContext(this.xnt, xmlNamespaceManager, this.XmlLang, this.XmlSpace);
			this.textXmlReader = new XmlTextReaderImpl(text, xmlParserContext, settings);
			if (!this.textXmlReader.Read() || (this.textXmlReader.NodeType == XmlNodeType.XmlDeclaration && !this.textXmlReader.Read()))
			{
				this.state = XmlSqlBinaryReader.ScanState.Doc;
				this.ReadDoc();
				return;
			}
			this.state = XmlSqlBinaryReader.ScanState.XmlText;
			this.UpdateFromTextReader();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000462EC File Offset: 0x000452EC
		private void UpdateFromTextReader()
		{
			XmlReader xmlReader = this.textXmlReader;
			this.nodetype = xmlReader.NodeType;
			this.qnameOther.prefix = xmlReader.Prefix;
			this.qnameOther.localname = xmlReader.LocalName;
			this.qnameOther.namespaceUri = xmlReader.NamespaceURI;
			this.valueType = xmlReader.ValueType;
			this.isEmpty = xmlReader.IsEmptyElement;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00046357 File Offset: 0x00045357
		private bool UpdateFromTextReader(bool needUpdate)
		{
			if (needUpdate)
			{
				this.UpdateFromTextReader();
			}
			return needUpdate;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00046364 File Offset: 0x00045364
		private void CheckAllowContent()
		{
			int num = this.docState;
			if (num == 0)
			{
				this.docState = 9;
				return;
			}
			if (num != 3 && num != 9)
			{
				throw this.ThrowXmlException("Xml_InvalidRootData");
			}
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0004639C File Offset: 0x0004539C
		private void GenerateTokenTypeMap()
		{
			Type[] array = new Type[256];
			array[134] = typeof(bool);
			array[7] = typeof(byte);
			array[136] = typeof(sbyte);
			array[1] = typeof(short);
			array[137] = typeof(ushort);
			array[138] = typeof(uint);
			array[3] = typeof(float);
			array[4] = typeof(double);
			array[8] = typeof(long);
			array[139] = typeof(ulong);
			array[140] = typeof(XmlQualifiedName);
			Type typeFromHandle = typeof(int);
			array[6] = typeFromHandle;
			array[2] = typeFromHandle;
			Type typeFromHandle2 = typeof(decimal);
			array[20] = typeFromHandle2;
			array[5] = typeFromHandle2;
			array[10] = typeFromHandle2;
			array[11] = typeFromHandle2;
			array[135] = typeFromHandle2;
			Type typeFromHandle3 = typeof(DateTime);
			array[19] = typeFromHandle3;
			array[18] = typeFromHandle3;
			array[129] = typeFromHandle3;
			array[130] = typeFromHandle3;
			array[131] = typeFromHandle3;
			array[127] = typeFromHandle3;
			array[126] = typeFromHandle3;
			array[125] = typeFromHandle3;
			Type typeFromHandle4 = typeof(DateTimeOffset);
			array[124] = typeFromHandle4;
			array[123] = typeFromHandle4;
			array[122] = typeFromHandle4;
			Type typeFromHandle5 = typeof(byte[]);
			array[15] = typeFromHandle5;
			array[12] = typeFromHandle5;
			array[23] = typeFromHandle5;
			array[27] = typeFromHandle5;
			array[132] = typeFromHandle5;
			array[133] = typeFromHandle5;
			array[13] = XmlSqlBinaryReader.TypeOfString;
			array[16] = XmlSqlBinaryReader.TypeOfString;
			array[22] = XmlSqlBinaryReader.TypeOfString;
			array[14] = XmlSqlBinaryReader.TypeOfString;
			array[17] = XmlSqlBinaryReader.TypeOfString;
			array[24] = XmlSqlBinaryReader.TypeOfString;
			array[9] = XmlSqlBinaryReader.TypeOfString;
			if (XmlSqlBinaryReader.TokenTypeMap == null)
			{
				XmlSqlBinaryReader.TokenTypeMap = array;
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00046570 File Offset: 0x00045570
		private Type GetValueType(BinXmlToken token)
		{
			Type type = XmlSqlBinaryReader.TokenTypeMap[(int)token];
			if (type == null)
			{
				throw this.ThrowUnexpectedToken(token);
			}
			return type;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00046591 File Offset: 0x00045591
		private void ReScanOverValue(BinXmlToken token)
		{
			this.ScanOverValue(token, true, false);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000465A0 File Offset: 0x000455A0
		private XmlNodeType ScanOverValue(BinXmlToken token, bool attr, bool checkChars)
		{
			if (token != BinXmlToken.SQL_NVARCHAR)
			{
				return this.ScanOverAnyValue(token, attr, checkChars);
			}
			if (this.mark < 0)
			{
				this.mark = this.pos;
			}
			this.tokLen = this.ParseMB32();
			this.tokDataPos = this.pos;
			checked
			{
				this.pos += this.tokLen * 2;
				this.Fill(-1);
				if (checkChars && this.checkCharacters)
				{
					return this.CheckText(attr);
				}
				if (!attr)
				{
					return this.CheckTextIsWS();
				}
				return XmlNodeType.Text;
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00046624 File Offset: 0x00045624
		private XmlNodeType ScanOverAnyValue(BinXmlToken token, bool attr, bool checkChars)
		{
			if (this.mark < 0)
			{
				this.mark = this.pos;
			}
			checked
			{
				switch (token)
				{
				case BinXmlToken.SQL_SMALLINT:
					goto IL_010B;
				case BinXmlToken.SQL_INT:
				case BinXmlToken.SQL_REAL:
				case BinXmlToken.SQL_SMALLDATETIME:
				case BinXmlToken.SQL_SMALLMONEY:
					goto IL_0131;
				case BinXmlToken.SQL_FLOAT:
				case BinXmlToken.SQL_MONEY:
				case BinXmlToken.SQL_BIGINT:
				case BinXmlToken.SQL_DATETIME:
					goto IL_0157;
				case BinXmlToken.SQL_BIT:
				case BinXmlToken.SQL_TINYINT:
					break;
				case BinXmlToken.SQL_UUID:
					this.tokDataPos = this.pos;
					this.tokLen = 16;
					this.pos += 16;
					goto IL_02BB;
				case BinXmlToken.SQL_DECIMAL:
				case BinXmlToken.SQL_NUMERIC:
					goto IL_01A5;
				case BinXmlToken.SQL_BINARY:
				case BinXmlToken.SQL_VARBINARY:
				case BinXmlToken.SQL_IMAGE:
				case BinXmlToken.SQL_UDT:
					goto IL_01D5;
				case BinXmlToken.SQL_CHAR:
				case BinXmlToken.SQL_VARCHAR:
				case BinXmlToken.SQL_TEXT:
					this.tokLen = this.ParseMB64();
					this.tokDataPos = this.pos;
					this.pos += this.tokLen;
					if (checkChars && this.checkCharacters)
					{
						this.Fill(-1);
						string text = this.ValueAsString(token);
						XmlConvert.VerifyCharData(text, ExceptionType.XmlException);
						this.stringValue = text;
						goto IL_02BB;
					}
					goto IL_02BB;
				case BinXmlToken.SQL_NCHAR:
				case BinXmlToken.SQL_NVARCHAR:
				case BinXmlToken.SQL_NTEXT:
					return this.ScanOverValue(BinXmlToken.SQL_NVARCHAR, attr, checkChars);
				case (BinXmlToken)21:
				case (BinXmlToken)25:
				case (BinXmlToken)26:
					goto IL_02B3;
				default:
					switch (token)
					{
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
						this.VerifyVersion(2, token);
						this.tokDataPos = this.pos;
						this.tokLen = this.GetXsdKatmaiTokenLength(token);
						this.pos += this.tokLen;
						goto IL_02BB;
					case (BinXmlToken)128:
						goto IL_02B3;
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
					case BinXmlToken.XSD_UNSIGNEDLONG:
						goto IL_0157;
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
						goto IL_01D5;
					case BinXmlToken.XSD_BOOLEAN:
					case BinXmlToken.XSD_BYTE:
						break;
					case BinXmlToken.XSD_DECIMAL:
						goto IL_01A5;
					case BinXmlToken.XSD_UNSIGNEDSHORT:
						goto IL_010B;
					case BinXmlToken.XSD_UNSIGNEDINT:
						goto IL_0131;
					case BinXmlToken.XSD_QNAME:
						this.tokDataPos = this.pos;
						this.ParseMB32();
						goto IL_02BB;
					default:
						goto IL_02B3;
					}
					break;
				}
				this.tokDataPos = this.pos;
				this.tokLen = 1;
				this.pos++;
				goto IL_02BB;
			}
			IL_010B:
			this.tokDataPos = this.pos;
			this.tokLen = 2;
			checked
			{
				this.pos += 2;
				goto IL_02BB;
			}
			IL_0131:
			this.tokDataPos = this.pos;
			this.tokLen = 4;
			checked
			{
				this.pos += 4;
				goto IL_02BB;
			}
			IL_0157:
			this.tokDataPos = this.pos;
			this.tokLen = 8;
			checked
			{
				this.pos += 8;
				goto IL_02BB;
			}
			IL_01A5:
			this.tokDataPos = this.pos;
			this.tokLen = this.ParseMB64();
			checked
			{
				this.pos += this.tokLen;
				goto IL_02BB;
			}
			IL_01D5:
			this.tokLen = this.ParseMB64();
			this.tokDataPos = this.pos;
			checked
			{
				this.pos += this.tokLen;
				goto IL_02BB;
			}
			IL_02B3:
			throw this.ThrowUnexpectedToken(token);
			IL_02BB:
			this.Fill(-1);
			return XmlNodeType.Text;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x000468F4 File Offset: 0x000458F4
		private unsafe XmlNodeType CheckText(bool attr)
		{
			XmlCharType xmlCharType = this.xmlCharType;
			fixed (byte* ptr = this.data)
			{
				int num = this.pos;
				int num2 = this.tokDataPos;
				if (!attr)
				{
					for (;;)
					{
						int num3 = num2 + 2;
						if (num3 > num)
						{
							break;
						}
						if (ptr[num2 + 1] != 0 || (xmlCharType.charProperties[ptr[num2]] & 1) == 0)
						{
							goto IL_0076;
						}
						num2 = num3;
					}
					return this.xmlspacePreserve ? XmlNodeType.SignificantWhitespace : XmlNodeType.Whitespace;
				}
				char c;
				char c2;
				for (;;)
				{
					IL_0076:
					int num4 = num2 + 2;
					if (num4 > num)
					{
						break;
					}
					c = (char)((int)ptr[num2] | ((int)ptr[num2 + 1] << 8));
					if ((xmlCharType.charProperties[c] & 16) != 0)
					{
						num2 = num4;
					}
					else
					{
						if (c < '\ud800' || c > '\udbff')
						{
							goto IL_00C1;
						}
						if (num2 + 4 > num)
						{
							goto Block_9;
						}
						c2 = (char)((int)ptr[num2 + 2] | ((int)ptr[num2 + 3] << 8));
						if (c2 < '\udc00' || c2 > '\udfff')
						{
							goto IL_0102;
						}
						num2 += 4;
					}
				}
				return XmlNodeType.Text;
				IL_00C1:
				throw XmlConvert.CreateInvalidCharException(c, ExceptionType.XmlException);
				Block_9:
				throw this.ThrowXmlException("Xml_InvalidSurrogateMissingLowChar");
				IL_0102:
				throw XmlConvert.CreateInvalidSurrogatePairException(c, c2);
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00046A18 File Offset: 0x00045A18
		private XmlNodeType CheckTextIsWS()
		{
			byte[] array = this.data;
			int i = this.tokDataPos;
			while (i < this.pos)
			{
				if (array[i + 1] == 0)
				{
					byte b = array[i];
					switch (b)
					{
					case 9:
					case 10:
					case 13:
						break;
					case 11:
					case 12:
						return XmlNodeType.Text;
					default:
						if (b != 32)
						{
							return XmlNodeType.Text;
						}
						break;
					}
					i += 2;
					continue;
				}
				return XmlNodeType.Text;
			}
			if (this.xmlspacePreserve)
			{
				return XmlNodeType.SignificantWhitespace;
			}
			return XmlNodeType.Whitespace;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00046A7E File Offset: 0x00045A7E
		private void CheckValueTokenBounds()
		{
			if (this.end - this.tokDataPos < this.tokLen)
			{
				throw this.ThrowXmlException("Xml_UnexpectedEOF1");
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x00046AA4 File Offset: 0x00045AA4
		private int GetXsdKatmaiTokenLength(BinXmlToken token)
		{
			switch (token)
			{
			case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			{
				this.Fill(0);
				byte b = this.data[this.pos];
				return 6 + this.XsdKatmaiTimeScaleToValueLength(b);
			}
			case BinXmlToken.XSD_KATMAI_TIME:
			case BinXmlToken.XSD_KATMAI_DATETIME:
			{
				this.Fill(0);
				byte b = this.data[this.pos];
				return 4 + this.XsdKatmaiTimeScaleToValueLength(b);
			}
			case BinXmlToken.XSD_KATMAI_DATE:
				return 3;
			default:
				throw this.ThrowUnexpectedToken(this.token);
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00046B22 File Offset: 0x00045B22
		private int XsdKatmaiTimeScaleToValueLength(byte scale)
		{
			if (scale > 7)
			{
				throw new XmlException("SqlTypes_ArithOverflow", null);
			}
			return (int)XmlSqlBinaryReader.XsdKatmaiTimeScaleToValueLengthMap[(int)scale];
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00046B3C File Offset: 0x00045B3C
		private long ValueAsLong()
		{
			this.CheckValueTokenBounds();
			BinXmlToken binXmlToken = this.token;
			switch (binXmlToken)
			{
			case BinXmlToken.SQL_SMALLINT:
				return (long)this.GetInt16(this.tokDataPos);
			case BinXmlToken.SQL_INT:
				return (long)this.GetInt32(this.tokDataPos);
			case BinXmlToken.SQL_REAL:
			case BinXmlToken.SQL_FLOAT:
			{
				double num = this.ValueAsDouble();
				return (long)num;
			}
			case BinXmlToken.SQL_MONEY:
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
			case BinXmlToken.SQL_SMALLMONEY:
				break;
			case BinXmlToken.SQL_BIT:
			case BinXmlToken.SQL_TINYINT:
			{
				byte b = this.data[this.tokDataPos];
				return (long)((ulong)b);
			}
			case BinXmlToken.SQL_BIGINT:
				return this.GetInt64(this.tokDataPos);
			case BinXmlToken.SQL_UUID:
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
				goto IL_011F;
			default:
				switch (binXmlToken)
				{
				case BinXmlToken.XSD_DECIMAL:
					break;
				case BinXmlToken.XSD_BYTE:
				{
					sbyte b2 = (sbyte)this.data[this.tokDataPos];
					return (long)b2;
				}
				case BinXmlToken.XSD_UNSIGNEDSHORT:
					return (long)((ulong)this.GetUInt16(this.tokDataPos));
				case BinXmlToken.XSD_UNSIGNEDINT:
					return (long)((ulong)this.GetUInt32(this.tokDataPos));
				case BinXmlToken.XSD_UNSIGNEDLONG:
				{
					ulong @uint = this.GetUInt64(this.tokDataPos);
					return checked((long)@uint);
				}
				default:
					goto IL_011F;
				}
				break;
			}
			decimal num2 = this.ValueAsDecimal();
			return (long)num2;
			IL_011F:
			throw this.ThrowUnexpectedToken(this.token);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00046C74 File Offset: 0x00045C74
		private ulong ValueAsULong()
		{
			if (BinXmlToken.XSD_UNSIGNEDLONG == this.token)
			{
				this.CheckValueTokenBounds();
				return this.GetUInt64(this.tokDataPos);
			}
			throw this.ThrowUnexpectedToken(this.token);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00046CA4 File Offset: 0x00045CA4
		private decimal ValueAsDecimal()
		{
			this.CheckValueTokenBounds();
			BinXmlToken binXmlToken = this.token;
			switch (binXmlToken)
			{
			case BinXmlToken.SQL_SMALLINT:
			case BinXmlToken.SQL_INT:
			case BinXmlToken.SQL_BIT:
			case BinXmlToken.SQL_TINYINT:
			case BinXmlToken.SQL_BIGINT:
				break;
			case BinXmlToken.SQL_REAL:
				return new decimal(this.GetSingle(this.tokDataPos));
			case BinXmlToken.SQL_FLOAT:
				return new decimal(this.GetDouble(this.tokDataPos));
			case BinXmlToken.SQL_MONEY:
			{
				BinXmlSqlMoney binXmlSqlMoney = new BinXmlSqlMoney(this.GetInt64(this.tokDataPos));
				return binXmlSqlMoney.ToDecimal();
			}
			case BinXmlToken.SQL_UUID:
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
				goto IL_0124;
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
				goto IL_00FC;
			case BinXmlToken.SQL_SMALLMONEY:
			{
				BinXmlSqlMoney binXmlSqlMoney2 = new BinXmlSqlMoney(this.GetInt32(this.tokDataPos));
				return binXmlSqlMoney2.ToDecimal();
			}
			default:
				switch (binXmlToken)
				{
				case BinXmlToken.XSD_DECIMAL:
					goto IL_00FC;
				case BinXmlToken.XSD_BYTE:
				case BinXmlToken.XSD_UNSIGNEDSHORT:
				case BinXmlToken.XSD_UNSIGNEDINT:
					break;
				case BinXmlToken.XSD_UNSIGNEDLONG:
					return new decimal(this.ValueAsULong());
				default:
					goto IL_0124;
				}
				break;
			}
			return new decimal(this.ValueAsLong());
			IL_00FC:
			BinXmlSqlDecimal binXmlSqlDecimal = new BinXmlSqlDecimal(this.data, this.tokDataPos, this.token == BinXmlToken.XSD_DECIMAL);
			return binXmlSqlDecimal.ToDecimal();
			IL_0124:
			throw this.ThrowUnexpectedToken(this.token);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00046DE4 File Offset: 0x00045DE4
		private double ValueAsDouble()
		{
			this.CheckValueTokenBounds();
			BinXmlToken binXmlToken = this.token;
			switch (binXmlToken)
			{
			case BinXmlToken.SQL_SMALLINT:
			case BinXmlToken.SQL_INT:
			case BinXmlToken.SQL_BIT:
			case BinXmlToken.SQL_TINYINT:
			case BinXmlToken.SQL_BIGINT:
				break;
			case BinXmlToken.SQL_REAL:
				return (double)this.GetSingle(this.tokDataPos);
			case BinXmlToken.SQL_FLOAT:
				return this.GetDouble(this.tokDataPos);
			case BinXmlToken.SQL_MONEY:
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
			case BinXmlToken.SQL_SMALLMONEY:
				goto IL_00B3;
			case BinXmlToken.SQL_UUID:
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
				goto IL_00C0;
			default:
				switch (binXmlToken)
				{
				case BinXmlToken.XSD_DECIMAL:
					goto IL_00B3;
				case BinXmlToken.XSD_BYTE:
				case BinXmlToken.XSD_UNSIGNEDSHORT:
				case BinXmlToken.XSD_UNSIGNEDINT:
					break;
				case BinXmlToken.XSD_UNSIGNEDLONG:
					return this.ValueAsULong();
				default:
					goto IL_00C0;
				}
				break;
			}
			return (double)this.ValueAsLong();
			IL_00B3:
			return (double)this.ValueAsDecimal();
			IL_00C0:
			throw this.ThrowUnexpectedToken(this.token);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00046EC0 File Offset: 0x00045EC0
		private DateTime ValueAsDateTime()
		{
			this.CheckValueTokenBounds();
			BinXmlToken binXmlToken = this.token;
			switch (binXmlToken)
			{
			case BinXmlToken.SQL_DATETIME:
			{
				int num = this.tokDataPos;
				int @int = this.GetInt32(num);
				uint @uint = this.GetUInt32(num + 4);
				return BinXmlDateTime.SqlDateTimeToDateTime(@int, @uint);
			}
			case BinXmlToken.SQL_SMALLDATETIME:
			{
				int num2 = this.tokDataPos;
				short int2 = this.GetInt16(num2);
				ushort uint2 = this.GetUInt16(num2 + 2);
				return BinXmlDateTime.SqlSmallDateTimeToDateTime(int2, uint2);
			}
			default:
				switch (binXmlToken)
				{
				case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					return BinXmlDateTime.XsdKatmaiTimeOffsetToDateTime(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					return BinXmlDateTime.XsdKatmaiDateTimeOffsetToDateTime(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					return BinXmlDateTime.XsdKatmaiDateOffsetToDateTime(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_TIME:
					return BinXmlDateTime.XsdKatmaiTimeToDateTime(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATETIME:
					return BinXmlDateTime.XsdKatmaiDateTimeToDateTime(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATE:
					return BinXmlDateTime.XsdKatmaiDateToDateTime(this.data, this.tokDataPos);
				case BinXmlToken.XSD_TIME:
				{
					long int3 = this.GetInt64(this.tokDataPos);
					return BinXmlDateTime.XsdTimeToDateTime(int3);
				}
				case BinXmlToken.XSD_DATETIME:
				{
					long int4 = this.GetInt64(this.tokDataPos);
					return BinXmlDateTime.XsdDateTimeToDateTime(int4);
				}
				case BinXmlToken.XSD_DATE:
				{
					long int5 = this.GetInt64(this.tokDataPos);
					return BinXmlDateTime.XsdDateToDateTime(int5);
				}
				}
				throw this.ThrowUnexpectedToken(this.token);
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x00047024 File Offset: 0x00046024
		private DateTimeOffset ValueAsDateTimeOffset()
		{
			this.CheckValueTokenBounds();
			switch (this.token)
			{
			case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
				return BinXmlDateTime.XsdKatmaiTimeOffsetToDateTimeOffset(this.data, this.tokDataPos);
			case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
				return BinXmlDateTime.XsdKatmaiDateTimeOffsetToDateTimeOffset(this.data, this.tokDataPos);
			case BinXmlToken.XSD_KATMAI_DATEOFFSET:
				return BinXmlDateTime.XsdKatmaiDateOffsetToDateTimeOffset(this.data, this.tokDataPos);
			default:
				throw this.ThrowUnexpectedToken(this.token);
			}
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00047098 File Offset: 0x00046098
		private string ValueAsDateTimeString()
		{
			this.CheckValueTokenBounds();
			BinXmlToken binXmlToken = this.token;
			switch (binXmlToken)
			{
			case BinXmlToken.SQL_DATETIME:
			{
				int num = this.tokDataPos;
				int @int = this.GetInt32(num);
				uint @uint = this.GetUInt32(num + 4);
				return BinXmlDateTime.SqlDateTimeToString(@int, @uint);
			}
			case BinXmlToken.SQL_SMALLDATETIME:
			{
				int num2 = this.tokDataPos;
				short int2 = this.GetInt16(num2);
				ushort uint2 = this.GetUInt16(num2 + 2);
				return BinXmlDateTime.SqlSmallDateTimeToString(int2, uint2);
			}
			default:
				switch (binXmlToken)
				{
				case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					return BinXmlDateTime.XsdKatmaiTimeOffsetToString(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					return BinXmlDateTime.XsdKatmaiDateTimeOffsetToString(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					return BinXmlDateTime.XsdKatmaiDateOffsetToString(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_TIME:
					return BinXmlDateTime.XsdKatmaiTimeToString(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATETIME:
					return BinXmlDateTime.XsdKatmaiDateTimeToString(this.data, this.tokDataPos);
				case BinXmlToken.XSD_KATMAI_DATE:
					return BinXmlDateTime.XsdKatmaiDateToString(this.data, this.tokDataPos);
				case BinXmlToken.XSD_TIME:
				{
					long int3 = this.GetInt64(this.tokDataPos);
					return BinXmlDateTime.XsdTimeToString(int3);
				}
				case BinXmlToken.XSD_DATETIME:
				{
					long int4 = this.GetInt64(this.tokDataPos);
					return BinXmlDateTime.XsdDateTimeToString(int4);
				}
				case BinXmlToken.XSD_DATE:
				{
					long int5 = this.GetInt64(this.tokDataPos);
					return BinXmlDateTime.XsdDateToString(int5);
				}
				}
				throw this.ThrowUnexpectedToken(this.token);
			}
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000471FC File Offset: 0x000461FC
		private string ValueAsString(BinXmlToken token)
		{
			try
			{
				this.CheckValueTokenBounds();
				switch (token)
				{
				case BinXmlToken.SQL_SMALLINT:
				case BinXmlToken.SQL_INT:
				case BinXmlToken.SQL_BIT:
				case BinXmlToken.SQL_TINYINT:
				case BinXmlToken.SQL_BIGINT:
					break;
				case BinXmlToken.SQL_REAL:
					return XmlConvert.ToString(this.GetSingle(this.tokDataPos));
				case BinXmlToken.SQL_FLOAT:
					return XmlConvert.ToString(this.GetDouble(this.tokDataPos));
				case BinXmlToken.SQL_MONEY:
				{
					BinXmlSqlMoney binXmlSqlMoney = new BinXmlSqlMoney(this.GetInt64(this.tokDataPos));
					return binXmlSqlMoney.ToString();
				}
				case BinXmlToken.SQL_UUID:
				{
					int num = this.tokDataPos;
					int @int = this.GetInt32(num);
					short int2 = this.GetInt16(num + 4);
					short int3 = this.GetInt16(num + 6);
					Guid guid = new Guid(@int, int2, int3, this.data[num + 8], this.data[num + 9], this.data[num + 10], this.data[num + 11], this.data[num + 12], this.data[num + 13], this.data[num + 14], this.data[num + 15]);
					return guid.ToString();
				}
				case BinXmlToken.SQL_DECIMAL:
				case BinXmlToken.SQL_NUMERIC:
					goto IL_0265;
				case BinXmlToken.SQL_BINARY:
				case BinXmlToken.SQL_VARBINARY:
				case BinXmlToken.SQL_IMAGE:
				case BinXmlToken.SQL_UDT:
					goto IL_02CF;
				case BinXmlToken.SQL_CHAR:
				case BinXmlToken.SQL_VARCHAR:
				case BinXmlToken.SQL_TEXT:
				{
					int num2 = this.tokDataPos;
					int int4 = this.GetInt32(num2);
					Encoding encoding = Encoding.GetEncoding(int4);
					return encoding.GetString(this.data, num2 + 4, this.tokLen - 4);
				}
				case BinXmlToken.SQL_NCHAR:
				case BinXmlToken.SQL_NVARCHAR:
				case BinXmlToken.SQL_NTEXT:
					return this.GetString(this.tokDataPos, this.tokLen);
				case BinXmlToken.SQL_DATETIME:
				case BinXmlToken.SQL_SMALLDATETIME:
					goto IL_030B;
				case BinXmlToken.SQL_SMALLMONEY:
				{
					BinXmlSqlMoney binXmlSqlMoney2 = new BinXmlSqlMoney(this.GetInt32(this.tokDataPos));
					return binXmlSqlMoney2.ToString();
				}
				case (BinXmlToken)21:
				case (BinXmlToken)25:
				case (BinXmlToken)26:
					goto IL_0398;
				default:
					switch (token)
					{
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
						goto IL_030B;
					case (BinXmlToken)128:
						goto IL_0398;
					case BinXmlToken.XSD_BINHEX:
						return BinHexEncoder.Encode(this.data, this.tokDataPos, this.tokLen);
					case BinXmlToken.XSD_BASE64:
						goto IL_02CF;
					case BinXmlToken.XSD_BOOLEAN:
						if (this.data[this.tokDataPos] == 0)
						{
							return "false";
						}
						return "true";
					case BinXmlToken.XSD_DECIMAL:
						goto IL_0265;
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
						break;
					case BinXmlToken.XSD_UNSIGNEDLONG:
						return this.ValueAsULong().ToString(CultureInfo.InvariantCulture);
					case BinXmlToken.XSD_QNAME:
					{
						int num3 = this.ParseMB32(this.tokDataPos);
						if (num3 < 0 || num3 >= this.symbolTables.qnameCount)
						{
							throw new XmlException("XmlBin_InvalidQNameID", string.Empty);
						}
						XmlSqlBinaryReader.QName qname = this.symbolTables.qnametable[num3];
						if (qname.prefix.Length == 0)
						{
							return qname.localname;
						}
						return qname.prefix + ":" + qname.localname;
					}
					default:
						goto IL_0398;
					}
					break;
				}
				return this.ValueAsLong().ToString(CultureInfo.InvariantCulture);
				IL_0265:
				BinXmlSqlDecimal binXmlSqlDecimal = new BinXmlSqlDecimal(this.data, this.tokDataPos, token == BinXmlToken.XSD_DECIMAL);
				return binXmlSqlDecimal.ToString();
				IL_02CF:
				return Convert.ToBase64String(this.data, this.tokDataPos, this.tokLen);
				IL_030B:
				return this.ValueAsDateTimeString();
				IL_0398:
				throw this.ThrowUnexpectedToken(this.token);
			}
			catch
			{
				this.state = XmlSqlBinaryReader.ScanState.Error;
				throw;
			}
			string text;
			return text;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x000475D8 File Offset: 0x000465D8
		private object ValueAsObject(BinXmlToken token, bool returnInternalTypes)
		{
			this.CheckValueTokenBounds();
			switch (token)
			{
			case BinXmlToken.SQL_SMALLINT:
				return this.GetInt16(this.tokDataPos);
			case BinXmlToken.SQL_INT:
				return this.GetInt32(this.tokDataPos);
			case BinXmlToken.SQL_REAL:
				return this.GetSingle(this.tokDataPos);
			case BinXmlToken.SQL_FLOAT:
				return this.GetDouble(this.tokDataPos);
			case BinXmlToken.SQL_MONEY:
			{
				BinXmlSqlMoney binXmlSqlMoney = new BinXmlSqlMoney(this.GetInt64(this.tokDataPos));
				if (returnInternalTypes)
				{
					return binXmlSqlMoney;
				}
				return binXmlSqlMoney.ToDecimal();
			}
			case BinXmlToken.SQL_BIT:
				return (int)this.data[this.tokDataPos];
			case BinXmlToken.SQL_TINYINT:
				return this.data[this.tokDataPos];
			case BinXmlToken.SQL_BIGINT:
				return this.GetInt64(this.tokDataPos);
			case BinXmlToken.SQL_UUID:
			{
				int num = this.tokDataPos;
				int @int = this.GetInt32(num);
				short int2 = this.GetInt16(num + 4);
				short int3 = this.GetInt16(num + 6);
				Guid guid = new Guid(@int, int2, int3, this.data[num + 8], this.data[num + 9], this.data[num + 10], this.data[num + 11], this.data[num + 12], this.data[num + 13], this.data[num + 14], this.data[num + 15]);
				return guid.ToString();
			}
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
				break;
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_IMAGE:
			case BinXmlToken.SQL_UDT:
				goto IL_032D;
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_TEXT:
			{
				int num2 = this.tokDataPos;
				int int4 = this.GetInt32(num2);
				Encoding encoding = Encoding.GetEncoding(int4);
				return encoding.GetString(this.data, num2 + 4, this.tokLen - 4);
			}
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_NTEXT:
				return this.GetString(this.tokDataPos, this.tokLen);
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
				goto IL_0357;
			case BinXmlToken.SQL_SMALLMONEY:
			{
				BinXmlSqlMoney binXmlSqlMoney2 = new BinXmlSqlMoney(this.GetInt32(this.tokDataPos));
				if (returnInternalTypes)
				{
					return binXmlSqlMoney2;
				}
				return binXmlSqlMoney2.ToDecimal();
			}
			case (BinXmlToken)21:
			case (BinXmlToken)25:
			case (BinXmlToken)26:
				goto IL_03CE;
			default:
				switch (token)
				{
				case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
				case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
				case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					return this.ValueAsDateTimeOffset();
				case BinXmlToken.XSD_KATMAI_TIME:
				case BinXmlToken.XSD_KATMAI_DATETIME:
				case BinXmlToken.XSD_KATMAI_DATE:
				case BinXmlToken.XSD_TIME:
				case BinXmlToken.XSD_DATETIME:
				case BinXmlToken.XSD_DATE:
					goto IL_0357;
				case (BinXmlToken)128:
					goto IL_03CE;
				case BinXmlToken.XSD_BINHEX:
				case BinXmlToken.XSD_BASE64:
					goto IL_032D;
				case BinXmlToken.XSD_BOOLEAN:
					return 0 != this.data[this.tokDataPos];
				case BinXmlToken.XSD_DECIMAL:
					break;
				case BinXmlToken.XSD_BYTE:
				{
					sbyte b = (sbyte)this.data[this.tokDataPos];
					return b;
				}
				case BinXmlToken.XSD_UNSIGNEDSHORT:
					return this.GetUInt16(this.tokDataPos);
				case BinXmlToken.XSD_UNSIGNEDINT:
					return this.GetUInt32(this.tokDataPos);
				case BinXmlToken.XSD_UNSIGNEDLONG:
					return this.GetUInt64(this.tokDataPos);
				case BinXmlToken.XSD_QNAME:
				{
					int num3 = this.ParseMB32(this.tokDataPos);
					if (num3 < 0 || num3 >= this.symbolTables.qnameCount)
					{
						throw new XmlException("XmlBin_InvalidQNameID", string.Empty);
					}
					XmlSqlBinaryReader.QName qname = this.symbolTables.qnametable[num3];
					return new XmlQualifiedName(qname.localname, qname.namespaceUri);
				}
				default:
					goto IL_03CE;
				}
				break;
			}
			BinXmlSqlDecimal binXmlSqlDecimal = new BinXmlSqlDecimal(this.data, this.tokDataPos, token == BinXmlToken.XSD_DECIMAL);
			if (returnInternalTypes)
			{
				return binXmlSqlDecimal;
			}
			return binXmlSqlDecimal.ToDecimal();
			IL_032D:
			byte[] array = new byte[this.tokLen];
			Array.Copy(this.data, this.tokDataPos, array, 0, this.tokLen);
			return array;
			IL_0357:
			return this.ValueAsDateTime();
			IL_03CE:
			throw this.ThrowUnexpectedToken(this.token);
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x000479C0 File Offset: 0x000469C0
		private XmlValueConverter GetValueConverter(XmlTypeCode typeCode)
		{
			XmlSchemaSimpleType simpleTypeFromTypeCode = DatatypeImplementation.GetSimpleTypeFromTypeCode(typeCode);
			return simpleTypeFromTypeCode.ValueConverter;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x000479DC File Offset: 0x000469DC
		private object ValueAs(BinXmlToken token, Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			this.CheckValueTokenBounds();
			switch (token)
			{
			case BinXmlToken.SQL_SMALLINT:
			{
				int @int = (int)this.GetInt16(this.tokDataPos);
				return this.GetValueConverter(XmlTypeCode.Short).ChangeType(@int, returnType, namespaceResolver);
			}
			case BinXmlToken.SQL_INT:
			{
				int int2 = this.GetInt32(this.tokDataPos);
				return this.GetValueConverter(XmlTypeCode.Int).ChangeType(int2, returnType, namespaceResolver);
			}
			case BinXmlToken.SQL_REAL:
			{
				float single = this.GetSingle(this.tokDataPos);
				return this.GetValueConverter(XmlTypeCode.Float).ChangeType(single, returnType, namespaceResolver);
			}
			case BinXmlToken.SQL_FLOAT:
			{
				double @double = this.GetDouble(this.tokDataPos);
				return this.GetValueConverter(XmlTypeCode.Double).ChangeType(@double, returnType, namespaceResolver);
			}
			case BinXmlToken.SQL_MONEY:
				return this.GetValueConverter(XmlTypeCode.Decimal).ChangeType(new BinXmlSqlMoney(this.GetInt64(this.tokDataPos)).ToDecimal(), returnType, namespaceResolver);
			case BinXmlToken.SQL_BIT:
				return this.GetValueConverter(XmlTypeCode.NonNegativeInteger).ChangeType((int)this.data[this.tokDataPos], returnType, namespaceResolver);
			case BinXmlToken.SQL_TINYINT:
				return this.GetValueConverter(XmlTypeCode.UnsignedByte).ChangeType(this.data[this.tokDataPos], returnType, namespaceResolver);
			case BinXmlToken.SQL_BIGINT:
			{
				long int3 = this.GetInt64(this.tokDataPos);
				return this.GetValueConverter(XmlTypeCode.Long).ChangeType(int3, returnType, namespaceResolver);
			}
			case BinXmlToken.SQL_UUID:
				return this.GetValueConverter(XmlTypeCode.String).ChangeType(this.ValueAsString(token), returnType, namespaceResolver);
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
				break;
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_IMAGE:
			case BinXmlToken.SQL_UDT:
				goto IL_03FC;
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_TEXT:
			{
				int num = this.tokDataPos;
				int int4 = this.GetInt32(num);
				Encoding encoding = Encoding.GetEncoding(int4);
				return this.GetValueConverter(XmlTypeCode.UntypedAtomic).ChangeType(encoding.GetString(this.data, num + 4, this.tokLen - 4), returnType, namespaceResolver);
			}
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_NTEXT:
				return this.GetValueConverter(XmlTypeCode.String).ChangeType(this.GetString(this.tokDataPos, this.tokLen), returnType, namespaceResolver);
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
				goto IL_0446;
			case BinXmlToken.SQL_SMALLMONEY:
				return this.GetValueConverter(XmlTypeCode.Decimal).ChangeType(new BinXmlSqlMoney(this.GetInt32(this.tokDataPos)).ToDecimal(), returnType, namespaceResolver);
			case (BinXmlToken)21:
			case (BinXmlToken)25:
			case (BinXmlToken)26:
				goto IL_0533;
			default:
				switch (token)
				{
				case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
				case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
				case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					return this.GetValueConverter(XmlTypeCode.DateTime).ChangeType(this.ValueAsDateTimeOffset(), returnType, namespaceResolver);
				case BinXmlToken.XSD_KATMAI_TIME:
				case BinXmlToken.XSD_KATMAI_DATETIME:
				case BinXmlToken.XSD_KATMAI_DATE:
				case BinXmlToken.XSD_DATETIME:
					goto IL_0446;
				case (BinXmlToken)128:
					goto IL_0533;
				case BinXmlToken.XSD_TIME:
					return this.GetValueConverter(XmlTypeCode.Time).ChangeType(this.ValueAsDateTime(), returnType, namespaceResolver);
				case BinXmlToken.XSD_DATE:
					return this.GetValueConverter(XmlTypeCode.Date).ChangeType(this.ValueAsDateTime(), returnType, namespaceResolver);
				case BinXmlToken.XSD_BINHEX:
				case BinXmlToken.XSD_BASE64:
					goto IL_03FC;
				case BinXmlToken.XSD_BOOLEAN:
					return this.GetValueConverter(XmlTypeCode.Boolean).ChangeType(0 != this.data[this.tokDataPos], returnType, namespaceResolver);
				case BinXmlToken.XSD_DECIMAL:
					break;
				case BinXmlToken.XSD_BYTE:
					return this.GetValueConverter(XmlTypeCode.Byte).ChangeType((int)((sbyte)this.data[this.tokDataPos]), returnType, namespaceResolver);
				case BinXmlToken.XSD_UNSIGNEDSHORT:
				{
					int @uint = (int)this.GetUInt16(this.tokDataPos);
					return this.GetValueConverter(XmlTypeCode.UnsignedShort).ChangeType(@uint, returnType, namespaceResolver);
				}
				case BinXmlToken.XSD_UNSIGNEDINT:
				{
					long num2 = (long)((ulong)this.GetUInt32(this.tokDataPos));
					return this.GetValueConverter(XmlTypeCode.UnsignedInt).ChangeType(num2, returnType, namespaceResolver);
				}
				case BinXmlToken.XSD_UNSIGNEDLONG:
				{
					decimal num3 = this.GetUInt64(this.tokDataPos);
					return this.GetValueConverter(XmlTypeCode.UnsignedLong).ChangeType(num3, returnType, namespaceResolver);
				}
				case BinXmlToken.XSD_QNAME:
				{
					int num4 = this.ParseMB32(this.tokDataPos);
					if (num4 < 0 || num4 >= this.symbolTables.qnameCount)
					{
						throw new XmlException("XmlBin_InvalidQNameID", string.Empty);
					}
					XmlSqlBinaryReader.QName qname = this.symbolTables.qnametable[num4];
					return this.GetValueConverter(XmlTypeCode.QName).ChangeType(new XmlQualifiedName(qname.localname, qname.namespaceUri), returnType, namespaceResolver);
				}
				default:
					goto IL_0533;
				}
				break;
			}
			return this.GetValueConverter(XmlTypeCode.Decimal).ChangeType(new BinXmlSqlDecimal(this.data, this.tokDataPos, token == BinXmlToken.XSD_DECIMAL).ToDecimal(), returnType, namespaceResolver);
			IL_03FC:
			byte[] array = new byte[this.tokLen];
			Array.Copy(this.data, this.tokDataPos, array, 0, this.tokLen);
			return this.GetValueConverter((token == BinXmlToken.XSD_BINHEX) ? XmlTypeCode.HexBinary : XmlTypeCode.Base64Binary).ChangeType(array, returnType, namespaceResolver);
			IL_0446:
			return this.GetValueConverter(XmlTypeCode.DateTime).ChangeType(this.ValueAsDateTime(), returnType, namespaceResolver);
			IL_0533:
			throw this.ThrowUnexpectedToken(this.token);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00047F2C File Offset: 0x00046F2C
		private short GetInt16(int pos)
		{
			byte[] array = this.data;
			return (short)((int)array[pos] | ((int)array[pos + 1] << 8));
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00047F4C File Offset: 0x00046F4C
		private ushort GetUInt16(int pos)
		{
			byte[] array = this.data;
			return (ushort)((int)array[pos] | ((int)array[pos + 1] << 8));
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00047F6C File Offset: 0x00046F6C
		private int GetInt32(int pos)
		{
			byte[] array = this.data;
			return (int)array[pos] | ((int)array[pos + 1] << 8) | ((int)array[pos + 2] << 16) | ((int)array[pos + 3] << 24);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00047FA0 File Offset: 0x00046FA0
		private uint GetUInt32(int pos)
		{
			byte[] array = this.data;
			return (uint)((int)array[pos] | ((int)array[pos + 1] << 8) | ((int)array[pos + 2] << 16) | ((int)array[pos + 3] << 24));
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00047FD4 File Offset: 0x00046FD4
		private long GetInt64(int pos)
		{
			byte[] array = this.data;
			uint num = (uint)((int)array[pos] | ((int)array[pos + 1] << 8) | ((int)array[pos + 2] << 16) | ((int)array[pos + 3] << 24));
			uint num2 = (uint)((int)array[pos + 4] | ((int)array[pos + 5] << 8) | ((int)array[pos + 6] << 16) | ((int)array[pos + 7] << 24));
			return (long)(((ulong)num2 << 32) | (ulong)num);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00048030 File Offset: 0x00047030
		private ulong GetUInt64(int pos)
		{
			byte[] array = this.data;
			uint num = (uint)((int)array[pos] | ((int)array[pos + 1] << 8) | ((int)array[pos + 2] << 16) | ((int)array[pos + 3] << 24));
			uint num2 = (uint)((int)array[pos + 4] | ((int)array[pos + 5] << 8) | ((int)array[pos + 6] << 16) | ((int)array[pos + 7] << 24));
			return ((ulong)num2 << 32) | (ulong)num;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0004808C File Offset: 0x0004708C
		private unsafe float GetSingle(int offset)
		{
			byte[] array = this.data;
			uint num = (uint)((int)array[offset] | ((int)array[offset + 1] << 8) | ((int)array[offset + 2] << 16) | ((int)array[offset + 3] << 24));
			return *(float*)(&num);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x000480C4 File Offset: 0x000470C4
		private unsafe double GetDouble(int offset)
		{
			uint num = (uint)((int)this.data[offset] | ((int)this.data[offset + 1] << 8) | ((int)this.data[offset + 2] << 16) | ((int)this.data[offset + 3] << 24));
			uint num2 = (uint)((int)this.data[offset + 4] | ((int)this.data[offset + 5] << 8) | ((int)this.data[offset + 6] << 16) | ((int)this.data[offset + 7] << 24));
			ulong num3 = ((ulong)num2 << 32) | (ulong)num;
			return *(double*)(&num3);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00048144 File Offset: 0x00047144
		private Exception ThrowUnexpectedToken(BinXmlToken token)
		{
			return this.ThrowXmlException("XmlBinary_UnexpectedToken");
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00048151 File Offset: 0x00047151
		private Exception ThrowXmlException(string res)
		{
			this.state = XmlSqlBinaryReader.ScanState.Error;
			return new XmlException(res, null);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00048164 File Offset: 0x00047164
		private Exception ThrowXmlException(string res, string arg1, string arg2)
		{
			this.state = XmlSqlBinaryReader.ScanState.Error;
			return new XmlException(res, new string[] { arg1, arg2 });
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0004818E File Offset: 0x0004718E
		private Exception ThrowNotSupported(string res)
		{
			this.state = XmlSqlBinaryReader.ScanState.Error;
			return new NotSupportedException(Res.GetString(res));
		}

		// Token: 0x04000A0F RID: 2575
		internal static readonly Type TypeOfObject = typeof(object);

		// Token: 0x04000A10 RID: 2576
		internal static readonly Type TypeOfString = typeof(string);

		// Token: 0x04000A11 RID: 2577
		private static Type[] TokenTypeMap = null;

		// Token: 0x04000A12 RID: 2578
		private static byte[] XsdKatmaiTimeScaleToValueLengthMap = new byte[] { 3, 3, 3, 4, 4, 5, 5, 5 };

		// Token: 0x04000A13 RID: 2579
		private static ReadState[] ScanState2ReadState = new ReadState[]
		{
			ReadState.Interactive,
			ReadState.Interactive,
			ReadState.Interactive,
			ReadState.Interactive,
			ReadState.Interactive,
			ReadState.Initial,
			ReadState.Error,
			ReadState.EndOfFile,
			ReadState.Closed
		};

		// Token: 0x04000A14 RID: 2580
		private Stream inStrm;

		// Token: 0x04000A15 RID: 2581
		private byte[] data;

		// Token: 0x04000A16 RID: 2582
		private int pos;

		// Token: 0x04000A17 RID: 2583
		private int mark;

		// Token: 0x04000A18 RID: 2584
		private int end;

		// Token: 0x04000A19 RID: 2585
		private long offset;

		// Token: 0x04000A1A RID: 2586
		private bool eof;

		// Token: 0x04000A1B RID: 2587
		private bool sniffed;

		// Token: 0x04000A1C RID: 2588
		private bool isEmpty;

		// Token: 0x04000A1D RID: 2589
		private int docState;

		// Token: 0x04000A1E RID: 2590
		private XmlSqlBinaryReader.SymbolTables symbolTables;

		// Token: 0x04000A1F RID: 2591
		private XmlNameTable xnt;

		// Token: 0x04000A20 RID: 2592
		private bool xntFromSettings;

		// Token: 0x04000A21 RID: 2593
		private string xml;

		// Token: 0x04000A22 RID: 2594
		private string xmlns;

		// Token: 0x04000A23 RID: 2595
		private string nsxmlns;

		// Token: 0x04000A24 RID: 2596
		private string baseUri;

		// Token: 0x04000A25 RID: 2597
		private XmlSqlBinaryReader.ScanState state;

		// Token: 0x04000A26 RID: 2598
		private XmlNodeType nodetype;

		// Token: 0x04000A27 RID: 2599
		private BinXmlToken token;

		// Token: 0x04000A28 RID: 2600
		private int attrIndex;

		// Token: 0x04000A29 RID: 2601
		private XmlSqlBinaryReader.QName qnameOther;

		// Token: 0x04000A2A RID: 2602
		private XmlSqlBinaryReader.QName qnameElement;

		// Token: 0x04000A2B RID: 2603
		private XmlNodeType parentNodeType;

		// Token: 0x04000A2C RID: 2604
		private XmlSqlBinaryReader.ElemInfo[] elementStack;

		// Token: 0x04000A2D RID: 2605
		private int elemDepth;

		// Token: 0x04000A2E RID: 2606
		private XmlSqlBinaryReader.AttrInfo[] attributes;

		// Token: 0x04000A2F RID: 2607
		private int[] attrHashTbl;

		// Token: 0x04000A30 RID: 2608
		private int attrCount;

		// Token: 0x04000A31 RID: 2609
		private int posAfterAttrs;

		// Token: 0x04000A32 RID: 2610
		private bool xmlspacePreserve;

		// Token: 0x04000A33 RID: 2611
		private int tokLen;

		// Token: 0x04000A34 RID: 2612
		private int tokDataPos;

		// Token: 0x04000A35 RID: 2613
		private bool hasTypedValue;

		// Token: 0x04000A36 RID: 2614
		private Type valueType;

		// Token: 0x04000A37 RID: 2615
		private string stringValue;

		// Token: 0x04000A38 RID: 2616
		private Dictionary<string, XmlSqlBinaryReader.NamespaceDecl> namespaces;

		// Token: 0x04000A39 RID: 2617
		private XmlSqlBinaryReader.NestedBinXml prevNameInfo;

		// Token: 0x04000A3A RID: 2618
		private XmlReader textXmlReader;

		// Token: 0x04000A3B RID: 2619
		private bool closeInput;

		// Token: 0x04000A3C RID: 2620
		private bool checkCharacters;

		// Token: 0x04000A3D RID: 2621
		private bool ignoreWhitespace;

		// Token: 0x04000A3E RID: 2622
		private bool ignorePIs;

		// Token: 0x04000A3F RID: 2623
		private bool ignoreComments;

		// Token: 0x04000A40 RID: 2624
		private bool prohibitDtd;

		// Token: 0x04000A41 RID: 2625
		private SecureStringHasher hasher;

		// Token: 0x04000A42 RID: 2626
		private XmlCharType xmlCharType;

		// Token: 0x04000A43 RID: 2627
		private Encoding unicode;

		// Token: 0x04000A44 RID: 2628
		private byte version;

		// Token: 0x020000F8 RID: 248
		private enum ScanState
		{
			// Token: 0x04000A46 RID: 2630
			Doc,
			// Token: 0x04000A47 RID: 2631
			XmlText,
			// Token: 0x04000A48 RID: 2632
			Attr,
			// Token: 0x04000A49 RID: 2633
			AttrVal,
			// Token: 0x04000A4A RID: 2634
			AttrValPseudoValue,
			// Token: 0x04000A4B RID: 2635
			Init,
			// Token: 0x04000A4C RID: 2636
			Error,
			// Token: 0x04000A4D RID: 2637
			EOF,
			// Token: 0x04000A4E RID: 2638
			Closed
		}

		// Token: 0x020000F9 RID: 249
		internal struct QName
		{
			// Token: 0x06000FA1 RID: 4001 RVA: 0x00048225 File Offset: 0x00047225
			public QName(string prefix, string lname, string nsUri)
			{
				this.prefix = prefix;
				this.localname = lname;
				this.namespaceUri = nsUri;
			}

			// Token: 0x06000FA2 RID: 4002 RVA: 0x0004823C File Offset: 0x0004723C
			public void Set(string prefix, string lname, string nsUri)
			{
				this.prefix = prefix;
				this.localname = lname;
				this.namespaceUri = nsUri;
			}

			// Token: 0x06000FA3 RID: 4003 RVA: 0x00048254 File Offset: 0x00047254
			public void Clear()
			{
				this.prefix = (this.localname = (this.namespaceUri = string.Empty));
			}

			// Token: 0x06000FA4 RID: 4004 RVA: 0x0004827E File Offset: 0x0004727E
			public bool MatchNs(string lname, string nsUri)
			{
				return lname == this.localname && nsUri == this.namespaceUri;
			}

			// Token: 0x06000FA5 RID: 4005 RVA: 0x0004829C File Offset: 0x0004729C
			public bool MatchPrefix(string prefix, string lname)
			{
				return lname == this.localname && prefix == this.prefix;
			}

			// Token: 0x06000FA6 RID: 4006 RVA: 0x000482BC File Offset: 0x000472BC
			public void CheckPrefixNS(string prefix, string namespaceUri)
			{
				if (this.prefix == prefix && this.namespaceUri != namespaceUri)
				{
					throw new XmlException("XmlBinary_NoRemapPrefix", new string[] { prefix, this.namespaceUri, namespaceUri });
				}
			}

			// Token: 0x06000FA7 RID: 4007 RVA: 0x00048309 File Offset: 0x00047309
			public override int GetHashCode()
			{
				return this.prefix.GetHashCode() ^ this.localname.GetHashCode();
			}

			// Token: 0x06000FA8 RID: 4008 RVA: 0x00048322 File Offset: 0x00047322
			public int GetNSHashCode(SecureStringHasher hasher)
			{
				return hasher.GetHashCode(this.namespaceUri) ^ hasher.GetHashCode(this.localname);
			}

			// Token: 0x06000FA9 RID: 4009 RVA: 0x00048340 File Offset: 0x00047340
			public override bool Equals(object other)
			{
				if (other is XmlSqlBinaryReader.QName)
				{
					XmlSqlBinaryReader.QName qname = (XmlSqlBinaryReader.QName)other;
					return this == qname;
				}
				return false;
			}

			// Token: 0x06000FAA RID: 4010 RVA: 0x0004836A File Offset: 0x0004736A
			public override string ToString()
			{
				if (this.prefix.Length == 0)
				{
					return this.localname;
				}
				return this.prefix + ":" + this.localname;
			}

			// Token: 0x06000FAB RID: 4011 RVA: 0x00048398 File Offset: 0x00047398
			public static bool operator ==(XmlSqlBinaryReader.QName a, XmlSqlBinaryReader.QName b)
			{
				return a.prefix == b.prefix && a.localname == b.localname && a.namespaceUri == b.namespaceUri;
			}

			// Token: 0x06000FAC RID: 4012 RVA: 0x000483E4 File Offset: 0x000473E4
			public static bool operator !=(XmlSqlBinaryReader.QName a, XmlSqlBinaryReader.QName b)
			{
				return !(a == b);
			}

			// Token: 0x04000A4F RID: 2639
			public string prefix;

			// Token: 0x04000A50 RID: 2640
			public string localname;

			// Token: 0x04000A51 RID: 2641
			public string namespaceUri;
		}

		// Token: 0x020000FA RID: 250
		private struct ElemInfo
		{
			// Token: 0x06000FAD RID: 4013 RVA: 0x000483F0 File Offset: 0x000473F0
			public void Set(XmlSqlBinaryReader.QName name, bool xmlspacePreserve)
			{
				this.name = name;
				this.xmlLang = null;
				this.xmlSpace = XmlSpace.None;
				this.xmlspacePreserve = xmlspacePreserve;
			}

			// Token: 0x06000FAE RID: 4014 RVA: 0x00048410 File Offset: 0x00047410
			public XmlSqlBinaryReader.NamespaceDecl Clear()
			{
				XmlSqlBinaryReader.NamespaceDecl namespaceDecl = this.nsdecls;
				this.nsdecls = null;
				return namespaceDecl;
			}

			// Token: 0x04000A52 RID: 2642
			public XmlSqlBinaryReader.QName name;

			// Token: 0x04000A53 RID: 2643
			public string xmlLang;

			// Token: 0x04000A54 RID: 2644
			public XmlSpace xmlSpace;

			// Token: 0x04000A55 RID: 2645
			public bool xmlspacePreserve;

			// Token: 0x04000A56 RID: 2646
			public XmlSqlBinaryReader.NamespaceDecl nsdecls;
		}

		// Token: 0x020000FB RID: 251
		private struct AttrInfo
		{
			// Token: 0x06000FAF RID: 4015 RVA: 0x0004842C File Offset: 0x0004742C
			public void Set(XmlSqlBinaryReader.QName n, string v)
			{
				this.name = n;
				this.val = v;
				this.contentPos = 0;
				this.hashCode = 0;
				this.prevHash = 0;
			}

			// Token: 0x06000FB0 RID: 4016 RVA: 0x00048451 File Offset: 0x00047451
			public void Set(XmlSqlBinaryReader.QName n, int pos)
			{
				this.name = n;
				this.val = null;
				this.contentPos = pos;
				this.hashCode = 0;
				this.prevHash = 0;
			}

			// Token: 0x06000FB1 RID: 4017 RVA: 0x00048476 File Offset: 0x00047476
			public void GetLocalnameAndNamespaceUri(out string localname, out string namespaceUri)
			{
				localname = this.name.localname;
				namespaceUri = this.name.namespaceUri;
			}

			// Token: 0x06000FB2 RID: 4018 RVA: 0x00048494 File Offset: 0x00047494
			public int GetLocalnameAndNamespaceUriAndHash(SecureStringHasher hasher, out string localname, out string namespaceUri)
			{
				localname = this.name.localname;
				namespaceUri = this.name.namespaceUri;
				return this.hashCode = this.name.GetNSHashCode(hasher);
			}

			// Token: 0x06000FB3 RID: 4019 RVA: 0x000484D0 File Offset: 0x000474D0
			public bool MatchNS(string localname, string namespaceUri)
			{
				return this.name.MatchNs(localname, namespaceUri);
			}

			// Token: 0x06000FB4 RID: 4020 RVA: 0x000484DF File Offset: 0x000474DF
			public bool MatchHashNS(int hash, string localname, string namespaceUri)
			{
				return this.hashCode == hash && this.name.MatchNs(localname, namespaceUri);
			}

			// Token: 0x06000FB5 RID: 4021 RVA: 0x000484F9 File Offset: 0x000474F9
			public void AdjustPosition(int adj)
			{
				if (this.contentPos != 0)
				{
					this.contentPos += adj;
				}
			}

			// Token: 0x04000A57 RID: 2647
			public XmlSqlBinaryReader.QName name;

			// Token: 0x04000A58 RID: 2648
			public string val;

			// Token: 0x04000A59 RID: 2649
			public int contentPos;

			// Token: 0x04000A5A RID: 2650
			public int hashCode;

			// Token: 0x04000A5B RID: 2651
			public int prevHash;
		}

		// Token: 0x020000FC RID: 252
		private class NamespaceDecl
		{
			// Token: 0x06000FB6 RID: 4022 RVA: 0x00048511 File Offset: 0x00047511
			public NamespaceDecl(string prefix, string nsuri, XmlSqlBinaryReader.NamespaceDecl nextInScope, XmlSqlBinaryReader.NamespaceDecl prevDecl, int scope, bool implied)
			{
				this.prefix = prefix;
				this.uri = nsuri;
				this.scopeLink = nextInScope;
				this.prevLink = prevDecl;
				this.scope = scope;
				this.implied = implied;
			}

			// Token: 0x04000A5C RID: 2652
			public string prefix;

			// Token: 0x04000A5D RID: 2653
			public string uri;

			// Token: 0x04000A5E RID: 2654
			public XmlSqlBinaryReader.NamespaceDecl scopeLink;

			// Token: 0x04000A5F RID: 2655
			public XmlSqlBinaryReader.NamespaceDecl prevLink;

			// Token: 0x04000A60 RID: 2656
			public int scope;

			// Token: 0x04000A61 RID: 2657
			public bool implied;
		}

		// Token: 0x020000FD RID: 253
		private struct SymbolTables
		{
			// Token: 0x06000FB7 RID: 4023 RVA: 0x00048546 File Offset: 0x00047546
			public void Init()
			{
				this.symtable = new string[64];
				this.qnametable = new XmlSqlBinaryReader.QName[16];
				this.symtable[0] = string.Empty;
				this.symCount = 1;
				this.qnameCount = 1;
			}

			// Token: 0x04000A62 RID: 2658
			public string[] symtable;

			// Token: 0x04000A63 RID: 2659
			public int symCount;

			// Token: 0x04000A64 RID: 2660
			public XmlSqlBinaryReader.QName[] qnametable;

			// Token: 0x04000A65 RID: 2661
			public int qnameCount;
		}

		// Token: 0x020000FE RID: 254
		private class NestedBinXml
		{
			// Token: 0x06000FB8 RID: 4024 RVA: 0x0004857D File Offset: 0x0004757D
			public NestedBinXml(XmlSqlBinaryReader.SymbolTables symbolTables, int docState, XmlSqlBinaryReader.NestedBinXml next)
			{
				this.symbolTables = symbolTables;
				this.docState = docState;
				this.next = next;
			}

			// Token: 0x04000A66 RID: 2662
			public XmlSqlBinaryReader.SymbolTables symbolTables;

			// Token: 0x04000A67 RID: 2663
			public int docState;

			// Token: 0x04000A68 RID: 2664
			public XmlSqlBinaryReader.NestedBinXml next;
		}
	}
}
