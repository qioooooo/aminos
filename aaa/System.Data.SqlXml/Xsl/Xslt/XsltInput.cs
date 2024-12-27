using System;
using System.Diagnostics;
using System.Xml.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000122 RID: 290
	internal class XsltInput : IErrorHelper
	{
		// Token: 0x06000C7B RID: 3195 RVA: 0x0003F54C File Offset: 0x0003E54C
		public XsltInput(XmlReader reader, Compiler compiler)
		{
			XsltInput.EnsureExpandEntities(reader);
			IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
			this.reader = reader;
			this.readerLineInfo = ((xmlLineInfo != null && xmlLineInfo.HasLineInfo()) ? xmlLineInfo : null);
			this.topLevelReader = reader.ReadState == ReadState.Initial;
			this.scopeManager = new XsltInput.InputScopeManager(reader.NameTable);
			this.atoms = new KeywordsTable(reader.NameTable);
			this.compiler = compiler;
			this.textIsWhite = true;
			this.nodeType = XPathNodeType.Root;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0003F5F8 File Offset: 0x0003E5F8
		private static void EnsureExpandEntities(XmlReader reader)
		{
			XmlTextReader xmlTextReader = reader as XmlTextReader;
			if (xmlTextReader != null && xmlTextReader.EntityHandling != EntityHandling.ExpandEntities)
			{
				xmlTextReader.EntityHandling = EntityHandling.ExpandEntities;
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0003F620 File Offset: 0x0003E620
		public bool Start()
		{
			if (this.topLevelReader)
			{
				return this.MoveToNextSibling();
			}
			if (this.reader.ReadState != ReadState.Interactive)
			{
				return false;
			}
			this.StepOnNodeRdr();
			if (this.nodeType == XPathNodeType.Comment || this.nodeType == XPathNodeType.ProcessingInstruction)
			{
				return this.MoveToNextSibling();
			}
			return this.nodeType == XPathNodeType.Element;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0003F674 File Offset: 0x0003E674
		public void Finish()
		{
			this.reader.Read();
			this.FixLastLineInfo();
			if (this.topLevelReader)
			{
				while (this.reader.ReadState == ReadState.Interactive)
				{
					this.reader.Skip();
				}
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0003F6AC File Offset: 0x0003E6AC
		private void SetCachedProperties()
		{
			this.nodeType = XsltInput.ConvertNodeType(this.reader.NodeType);
			this.localName = this.reader.LocalName;
			this.namespaceName = this.reader.NamespaceURI;
			this.value = this.reader.Value;
			if (this.nodeType == XPathNodeType.Attribute && XsltInput.IsNamespace(this.reader))
			{
				this.nodeType = XPathNodeType.Namespace;
				this.namespaceName = string.Empty;
				if (this.localName == "xmlns")
				{
					this.localName = string.Empty;
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0003F747 File Offset: 0x0003E747
		public KeywordsTable Atoms
		{
			get
			{
				return this.atoms;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x0003F74F File Offset: 0x0003E74F
		public XPathNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0003F757 File Offset: 0x0003E757
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0003F75F File Offset: 0x0003E75F
		public string LocalName
		{
			get
			{
				return this.localName;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0003F767 File Offset: 0x0003E767
		public string NamespaceUri
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0003F76F File Offset: 0x0003E76F
		public string Prefix
		{
			get
			{
				return this.reader.Prefix;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0003F77C File Offset: 0x0003E77C
		public string BaseUri
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0003F789 File Offset: 0x0003E789
		public string QualifiedName
		{
			get
			{
				return this.reader.Name;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0003F796 File Offset: 0x0003E796
		public bool IsEmptyElement
		{
			get
			{
				return this.reader.IsEmptyElement;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0003F7A3 File Offset: 0x0003E7A3
		public XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0003F7B0 File Offset: 0x0003E7B0
		public string LookupXmlNamespace(string prefix)
		{
			string text = this.reader.LookupNamespace(prefix);
			if (text != null)
			{
				return this.NameTable.Add(text);
			}
			if (prefix.Length == 0)
			{
				return string.Empty;
			}
			this.ReportError("Xslt_InvalidPrefix", new string[] { prefix });
			return null;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0003F800 File Offset: 0x0003E800
		public bool MoveToNextAttOrNs()
		{
			if (this.NodeType == XPathNodeType.Element)
			{
				if (!this.reader.MoveToFirstAttribute())
				{
					this.reader.MoveToElement();
					return false;
				}
			}
			else if (!this.reader.MoveToNextAttribute())
			{
				this.reader.MoveToElement();
				this.nodeType = XPathNodeType.Element;
				return false;
			}
			this.SetCachedProperties();
			return true;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003F85A File Offset: 0x0003E85A
		[Conditional("DEBUG")]
		private void SetLastMove(XsltInput.Moves lastMove, bool lastResult)
		{
			this.lastMove = lastMove;
			this.lastResult = lastResult;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0003F86C File Offset: 0x0003E86C
		private void StepOnNodeRdr()
		{
			if (this.text == null)
			{
				this.SetCachedProperties();
			}
			else
			{
				this.value = this.text;
				this.localName = string.Empty;
				this.namespaceName = string.Empty;
				this.nodeType = ((!this.textIsWhite) ? XPathNodeType.Text : (this.textPreserveWS ? XPathNodeType.SignificantWhitespace : XPathNodeType.Whitespace));
			}
			if (this.NodeType == XPathNodeType.Element)
			{
				this.scopeManager.PushScope();
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0003F8DC File Offset: 0x0003E8DC
		private void StepOffNode()
		{
			if (this.NodeType == XPathNodeType.Element)
			{
				this.scopeManager.PopScope();
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0003F8F2 File Offset: 0x0003E8F2
		private bool MoveToFirstChildAny()
		{
			if (!this.reader.IsEmptyElement)
			{
				return this.ReadNextSibling();
			}
			this.nodeType = XPathNodeType.Element;
			return false;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0003F910 File Offset: 0x0003E910
		public bool MoveToFirstChild()
		{
			bool flag = this.MoveToFirstChildAny();
			if (flag && (this.NodeType == XPathNodeType.Comment || this.NodeType == XPathNodeType.ProcessingInstruction))
			{
				flag = this.MoveToNextSibling();
				if (!flag)
				{
					this.MoveToParent();
				}
			}
			return flag;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0003F94C File Offset: 0x0003E94C
		public bool MoveToNextSibling()
		{
			bool flag;
			do
			{
				this.StepOffNode();
				flag = this.ReadNextSibling();
			}
			while (flag && (this.NodeType == XPathNodeType.Comment || this.NodeType == XPathNodeType.ProcessingInstruction));
			return flag;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0003F97C File Offset: 0x0003E97C
		public bool MoveToParent()
		{
			return true;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0003F98C File Offset: 0x0003E98C
		public void SkipNode()
		{
			if (this.NodeType == XPathNodeType.Element && this.MoveToFirstChild())
			{
				do
				{
					this.SkipNode();
				}
				while (this.MoveToNextSibling());
				this.MoveToParent();
			}
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0003F9B4 File Offset: 0x0003E9B4
		private bool ReadNextSiblingHelper()
		{
			if (this.text != null)
			{
				this.text = null;
				this.textIsWhite = true;
				return this.reader.NodeType != XmlNodeType.EndElement;
			}
			while (this.reader.Read())
			{
				XmlNodeType xmlNodeType = this.reader.NodeType;
				switch (xmlNodeType)
				{
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					if (this.textIsWhite && !XsltInput.IsWhitespace(this.reader.Value))
					{
						this.textIsWhite = false;
					}
					break;
				case XmlNodeType.EntityReference:
					continue;
				default:
					switch (xmlNodeType)
					{
					case XmlNodeType.DocumentType:
						continue;
					case XmlNodeType.DocumentFragment:
					case XmlNodeType.Notation:
						break;
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						goto IL_0094;
					default:
						if (xmlNodeType == XmlNodeType.XmlDeclaration)
						{
							continue;
						}
						break;
					}
					return this.text != null || this.reader.NodeType != XmlNodeType.EndElement;
				}
				IL_0094:
				if (this.reader.Depth != 0 || this.text != null || !this.textIsWhite)
				{
					if (this.text == null)
					{
						this.SaveTextInfo();
					}
					this.text += this.reader.Value;
				}
			}
			return this.text != null;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0003FAD8 File Offset: 0x0003EAD8
		private bool ReadNextSibling()
		{
			bool flag = this.ReadNextSiblingHelper();
			this.FixLastLineInfo();
			if (flag)
			{
				this.StepOnNodeRdr();
				return true;
			}
			this.nodeType = XPathNodeType.Element;
			return false;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0003FB05 File Offset: 0x0003EB05
		private static bool IsNamespace(XmlReader reader)
		{
			return reader.Prefix == "xmlns" || (reader.Prefix.Length == 0 && reader.LocalName == "xmlns");
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0003FB3C File Offset: 0x0003EB3C
		private static bool IsWhitespace(string text)
		{
			return XmlCharType.Instance.IsOnlyWhitespace(text);
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0003FB58 File Offset: 0x0003EB58
		private static XPathNodeType ConvertNodeType(XmlNodeType xmlNodeType)
		{
			XPathNodeType xpathNodeType = XsltInput.XmlNodeType2XPathNodeType[(int)xmlNodeType];
			if (xpathNodeType == (XPathNodeType)(-1))
			{
				return XPathNodeType.All;
			}
			return xpathNodeType;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0003FB75 File Offset: 0x0003EB75
		public bool IsNs(string ns)
		{
			return Ref.Equal(ns, this.NamespaceUri);
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0003FB83 File Offset: 0x0003EB83
		public bool IsKeyword(string kwd)
		{
			return Ref.Equal(kwd, this.LocalName);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0003FB91 File Offset: 0x0003EB91
		public bool IsXsltNamespace()
		{
			return this.IsNs(this.atoms.UriXsl);
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0003FBA4 File Offset: 0x0003EBA4
		public bool IsNullNamespace()
		{
			return this.IsNs(string.Empty);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0003FBB1 File Offset: 0x0003EBB1
		public bool IsXsltAttribute(string kwd)
		{
			return this.IsKeyword(kwd) && this.IsNullNamespace();
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0003FBC4 File Offset: 0x0003EBC4
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x0003FBD1 File Offset: 0x0003EBD1
		public bool CanHaveApplyImports
		{
			get
			{
				return this.scopeManager.CanHaveApplyImports;
			}
			set
			{
				this.scopeManager.CanHaveApplyImports = value;
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0003FBDF File Offset: 0x0003EBDF
		public void AddExtensionNamespace(string uri)
		{
			this.scopeManager.AddExtensionNamespace(uri);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0003FBED File Offset: 0x0003EBED
		public bool IsExtensionNamespace(string uri)
		{
			return this.scopeManager.IsExtensionNamespace(uri);
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0003FBFB File Offset: 0x0003EBFB
		public bool ForwardCompatibility
		{
			get
			{
				return this.scopeManager.ForwardCompatibility;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0003FC08 File Offset: 0x0003EC08
		public XslVersion XslVersion
		{
			get
			{
				if (!this.scopeManager.ForwardCompatibility)
				{
					return XslVersion.Version10;
				}
				return XslVersion.ForwardsCompatible;
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0003FC1C File Offset: 0x0003EC1C
		public void SetVersion(string version, string attName)
		{
			double num = XPathConvert.StringToDouble(version);
			if (double.IsNaN(num))
			{
				this.ReportError("Xslt_InvalidAttrValue", new string[] { attName, version });
				num = 1.0;
			}
			this.scopeManager.ForwardCompatibility = num != 1.0;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0003FC77 File Offset: 0x0003EC77
		public XsltInput.ContextInfo GetAttributes()
		{
			return this.GetAttributes(0, 0, this.names, this.values);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0003FC90 File Offset: 0x0003EC90
		public XsltInput.ContextInfo GetAttributes(int required, string name, out string value)
		{
			this.names[0] = name;
			XsltInput.ContextInfo attributes = this.GetAttributes(required, 1, this.names, this.values);
			value = this.values[0];
			return attributes;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0003FCC8 File Offset: 0x0003ECC8
		public XsltInput.ContextInfo GetAttributes(int required, string name0, out string value0, string name1, out string value1)
		{
			this.names[0] = name0;
			this.names[1] = name1;
			XsltInput.ContextInfo attributes = this.GetAttributes(required, 2, this.names, this.values);
			value0 = this.values[0];
			value1 = this.values[1];
			return attributes;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0003FD14 File Offset: 0x0003ED14
		public XsltInput.ContextInfo GetAttributes(int required, string name0, out string value0, string name1, out string value1, string name2, out string value2)
		{
			this.names[0] = name0;
			this.names[1] = name1;
			this.names[2] = name2;
			XsltInput.ContextInfo attributes = this.GetAttributes(required, 3, this.names, this.values);
			value0 = this.values[0];
			value1 = this.values[1];
			value2 = this.values[2];
			return attributes;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0003FD74 File Offset: 0x0003ED74
		public XsltInput.ContextInfo GetAttributes(int required, string name0, out string value0, string name1, out string value1, string name2, out string value2, string name3, out string value3)
		{
			this.names[0] = name0;
			this.names[1] = name1;
			this.names[2] = name2;
			this.names[3] = name3;
			XsltInput.ContextInfo attributes = this.GetAttributes(required, 4, this.names, this.values);
			value0 = this.values[0];
			value1 = this.values[1];
			value2 = this.values[2];
			value3 = this.values[3];
			return attributes;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0003FDEC File Offset: 0x0003EDEC
		public XsltInput.ContextInfo GetAttributes(int required, string name0, out string value0, string name1, out string value1, string name2, out string value2, string name3, out string value3, string name4, out string value4)
		{
			this.names[0] = name0;
			this.names[1] = name1;
			this.names[2] = name2;
			this.names[3] = name3;
			this.names[4] = name4;
			XsltInput.ContextInfo attributes = this.GetAttributes(required, 5, this.names, this.values);
			value0 = this.values[0];
			value1 = this.values[1];
			value2 = this.values[2];
			value3 = this.values[3];
			value4 = this.values[4];
			return attributes;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0003FE78 File Offset: 0x0003EE78
		public XsltInput.ContextInfo GetAttributes(int required, string name0, out string value0, string name1, out string value1, string name2, out string value2, string name3, out string value3, string name4, out string value4, string name5, out string value5, string name6, out string value6, string name7, out string value7, string name8, out string value8)
		{
			this.names[0] = name0;
			this.names[1] = name1;
			this.names[2] = name2;
			this.names[3] = name3;
			this.names[4] = name4;
			this.names[5] = name5;
			this.names[6] = name6;
			this.names[7] = name7;
			this.names[8] = name8;
			XsltInput.ContextInfo attributes = this.GetAttributes(required, 9, this.names, this.values);
			value0 = this.values[0];
			value1 = this.values[1];
			value2 = this.values[2];
			value3 = this.values[3];
			value4 = this.values[4];
			value5 = this.values[5];
			value6 = this.values[6];
			value7 = this.values[7];
			value8 = this.values[8];
			return attributes;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0003FF58 File Offset: 0x0003EF58
		public XsltInput.ContextInfo GetAttributes(int required, string name0, out string value0, string name1, out string value1, string name2, out string value2, string name3, out string value3, string name4, out string value4, string name5, out string value5, string name6, out string value6, string name7, out string value7, string name8, out string value8, string name9, out string value9)
		{
			this.names[0] = name0;
			this.names[1] = name1;
			this.names[2] = name2;
			this.names[3] = name3;
			this.names[4] = name4;
			this.names[5] = name5;
			this.names[6] = name6;
			this.names[7] = name7;
			this.names[8] = name8;
			this.names[9] = name9;
			XsltInput.ContextInfo attributes = this.GetAttributes(required, 10, this.names, this.values);
			value0 = this.values[0];
			value1 = this.values[1];
			value2 = this.values[2];
			value3 = this.values[3];
			value4 = this.values[4];
			value5 = this.values[5];
			value6 = this.values[6];
			value7 = this.values[7];
			value8 = this.values[8];
			value9 = this.values[9];
			return attributes;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00040050 File Offset: 0x0003F050
		public XsltInput.ContextInfo GetAttributes(int required, int number, string[] names, string[] values)
		{
			for (int i = 0; i < number; i++)
			{
				values[i] = null;
			}
			string qualifiedName = this.QualifiedName;
			XsltInput.ContextInfo contextInfo = new XsltInput.ContextInfo(this);
			this.compiler.EnterForwardsCompatible();
			while (this.MoveToNextAttOrNs())
			{
				if (this.nodeType == XPathNodeType.Namespace)
				{
					contextInfo.AddNamespace(this);
				}
				else
				{
					contextInfo.AddAttribute(this);
					bool flag = false;
					int j = 0;
					while (j < number)
					{
						if (this.IsXsltAttribute(names[j]))
						{
							flag = true;
							values[j] = this.Value;
							if (Ref.Equal(names[j], this.Atoms.Version) && j < required)
							{
								this.SetVersion(this.Value, this.Atoms.Version);
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
					if (!flag && (this.IsNullNamespace() || this.IsXsltNamespace()))
					{
						this.ReportError("Xslt_InvalidAttribute", new string[] { this.QualifiedName, qualifiedName });
					}
				}
			}
			this.compiler.ExitForwardsCompatible(this.ForwardCompatibility);
			for (int k = 0; k < required; k++)
			{
				if (values[k] == null)
				{
					this.ReportError("Xslt_MissingAttribute", new string[] { names[k] });
				}
			}
			contextInfo.Finish(this);
			return contextInfo;
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00040197 File Offset: 0x0003F197
		public string Uri
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x000401A4 File Offset: 0x0003F1A4
		public int StartLine
		{
			get
			{
				if (this.readerLineInfo == null)
				{
					return 0;
				}
				if (this.OnTextNode)
				{
					return this.textStartLine;
				}
				return this.readerLineInfo.LineNumber;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x000401CA File Offset: 0x0003F1CA
		public int StartPos
		{
			get
			{
				if (this.readerLineInfo == null)
				{
					return 0;
				}
				if (this.OnTextNode)
				{
					return this.textStartPos;
				}
				return this.readerLineInfo.LinePosition - XsltInput.PositionAdjustment(this.reader.NodeType);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00040201 File Offset: 0x0003F201
		public int EndLine
		{
			get
			{
				if (this.readerLineInfo == null)
				{
					return 0;
				}
				return this.readerLineInfo.LineNumber;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00040218 File Offset: 0x0003F218
		public int EndPos
		{
			get
			{
				if (this.readerLineInfo == null)
				{
					return 0;
				}
				int linePosition = this.readerLineInfo.LinePosition;
				if (this.OnTextNode)
				{
					return linePosition - XsltInput.PositionAdjustment(this.reader.NodeType);
				}
				return linePosition + 1;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00040259 File Offset: 0x0003F259
		private bool OnTextNode
		{
			get
			{
				return this.text != null;
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00040268 File Offset: 0x0003F268
		private static int PositionAdjustment(XmlNodeType nt)
		{
			if (nt != XmlNodeType.Element)
			{
				switch (nt)
				{
				case XmlNodeType.CDATA:
					return 9;
				case XmlNodeType.EntityReference:
				case XmlNodeType.Entity:
					break;
				case XmlNodeType.ProcessingInstruction:
					return 2;
				case XmlNodeType.Comment:
					return 4;
				default:
					if (nt == XmlNodeType.EndElement)
					{
						return 2;
					}
					break;
				}
				return 0;
			}
			return 1;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x000402AA File Offset: 0x0003F2AA
		private void SaveTextInfo()
		{
			this.textPreserveWS = this.reader.XmlSpace == XmlSpace.Preserve;
			this.textStartLine = this.StartLine;
			this.textStartPos = this.StartPos;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000402D8 File Offset: 0x0003F2D8
		public ISourceLineInfo BuildLineInfo()
		{
			bool flag = this.nodeType == XPathNodeType.Attribute;
			if (this.lastLineInfo != null && !flag)
			{
				return this.lastLineInfo;
			}
			SourceLineInfo sourceLineInfo = new SourceLineInfo(this.Uri, this.StartLine, this.StartPos, this.EndLine, this.EndPos);
			if (!this.OnTextNode && !flag)
			{
				this.lastLineInfo = sourceLineInfo;
			}
			return sourceLineInfo;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00040338 File Offset: 0x0003F338
		public void FixLastLineInfo()
		{
			if (this.lastLineInfo != null)
			{
				this.lastLineInfo.SetEndLinePos(this.StartLine, this.StartPos);
				this.lastLineInfo = null;
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00040360 File Offset: 0x0003F360
		public void ReportError(string res, params string[] args)
		{
			this.compiler.ReportError(this.BuildLineInfo(), res, args);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00040375 File Offset: 0x0003F375
		public void ReportWarning(string res, params string[] args)
		{
			this.compiler.ReportWarning(this.BuildLineInfo(), res, args);
		}

		// Token: 0x040008A6 RID: 2214
		private XmlReader reader;

		// Token: 0x040008A7 RID: 2215
		private IXmlLineInfo readerLineInfo;

		// Token: 0x040008A8 RID: 2216
		private bool topLevelReader;

		// Token: 0x040008A9 RID: 2217
		private XsltInput.InputScopeManager scopeManager;

		// Token: 0x040008AA RID: 2218
		private KeywordsTable atoms;

		// Token: 0x040008AB RID: 2219
		private Compiler compiler;

		// Token: 0x040008AC RID: 2220
		private string text;

		// Token: 0x040008AD RID: 2221
		private bool textIsWhite;

		// Token: 0x040008AE RID: 2222
		private XPathNodeType nodeType;

		// Token: 0x040008AF RID: 2223
		private string localName;

		// Token: 0x040008B0 RID: 2224
		private string namespaceName;

		// Token: 0x040008B1 RID: 2225
		private string value;

		// Token: 0x040008B2 RID: 2226
		private XsltInput.Moves lastMove = XsltInput.Moves.Child;

		// Token: 0x040008B3 RID: 2227
		private bool lastResult = true;

		// Token: 0x040008B4 RID: 2228
		private static XPathNodeType[] XmlNodeType2XPathNodeType = new XPathNodeType[]
		{
			(XPathNodeType)(-1),
			XPathNodeType.Element,
			XPathNodeType.Attribute,
			XPathNodeType.Text,
			XPathNodeType.Text,
			(XPathNodeType)(-1),
			(XPathNodeType)(-1),
			XPathNodeType.ProcessingInstruction,
			XPathNodeType.Comment,
			(XPathNodeType)(-1),
			(XPathNodeType)(-1),
			(XPathNodeType)(-1),
			(XPathNodeType)(-1),
			XPathNodeType.Whitespace,
			XPathNodeType.SignificantWhitespace,
			XPathNodeType.Element,
			(XPathNodeType)(-1),
			(XPathNodeType)(-1)
		};

		// Token: 0x040008B5 RID: 2229
		private string[] names = new string[10];

		// Token: 0x040008B6 RID: 2230
		private string[] values = new string[10];

		// Token: 0x040008B7 RID: 2231
		private SourceLineInfo lastLineInfo;

		// Token: 0x040008B8 RID: 2232
		private bool textPreserveWS;

		// Token: 0x040008B9 RID: 2233
		private int textStartLine;

		// Token: 0x040008BA RID: 2234
		private int textStartPos;

		// Token: 0x02000123 RID: 291
		private enum Moves
		{
			// Token: 0x040008BC RID: 2236
			Next,
			// Token: 0x040008BD RID: 2237
			Child,
			// Token: 0x040008BE RID: 2238
			Parent
		}

		// Token: 0x02000124 RID: 292
		private class InputScopeManager
		{
			// Token: 0x06000CBB RID: 3259 RVA: 0x000403F8 File Offset: 0x0003F3F8
			public InputScopeManager(XmlNameTable nameTable)
			{
				this.nameTable = nameTable;
				this.records[0].scopeFlags = (XsltInput.InputScopeManager.ScopeFlags)0;
			}

			// Token: 0x06000CBC RID: 3260 RVA: 0x00040426 File Offset: 0x0003F426
			public void PushScope()
			{
				this.lastScopes++;
			}

			// Token: 0x06000CBD RID: 3261 RVA: 0x00040438 File Offset: 0x0003F438
			public void PopScope()
			{
				if (0 < this.lastScopes)
				{
					this.lastScopes--;
					return;
				}
				do
				{
					this.lastRecord--;
				}
				while (this.records[this.lastRecord].scopeCount == 0);
				this.lastScopes = this.records[this.lastRecord].scopeCount;
				this.lastScopes--;
			}

			// Token: 0x06000CBE RID: 3262 RVA: 0x000404B0 File Offset: 0x0003F4B0
			private void AddRecord()
			{
				this.records[this.lastRecord].scopeCount = this.lastScopes;
				this.lastRecord++;
				if (this.lastRecord == this.records.Length)
				{
					XsltInput.InputScopeManager.ScopeRecord[] array = new XsltInput.InputScopeManager.ScopeRecord[this.lastRecord * 2];
					Array.Copy(this.records, 0, array, 0, this.lastRecord);
					this.records = array;
				}
				this.lastScopes = 0;
			}

			// Token: 0x06000CBF RID: 3263 RVA: 0x00040528 File Offset: 0x0003F528
			private void SetFlag(bool value, XsltInput.InputScopeManager.ScopeFlags flag)
			{
				XsltInput.InputScopeManager.ScopeFlags scopeFlags = this.records[this.lastRecord].scopeFlags;
				if ((scopeFlags & flag) != (XsltInput.InputScopeManager.ScopeFlags)0 != value)
				{
					if (this.lastScopes != 0)
					{
						this.AddRecord();
						scopeFlags &= XsltInput.InputScopeManager.ScopeFlags.InheritedFlags;
					}
					this.records[this.lastRecord].scopeFlags = scopeFlags ^ flag;
				}
			}

			// Token: 0x170001A0 RID: 416
			// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00040588 File Offset: 0x0003F588
			// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x000405A8 File Offset: 0x0003F5A8
			public bool ForwardCompatibility
			{
				get
				{
					return (this.records[this.lastRecord].scopeFlags & XsltInput.InputScopeManager.ScopeFlags.ForwardCompatibility) != (XsltInput.InputScopeManager.ScopeFlags)0;
				}
				set
				{
					this.SetFlag(value, XsltInput.InputScopeManager.ScopeFlags.ForwardCompatibility);
				}
			}

			// Token: 0x170001A1 RID: 417
			// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x000405B2 File Offset: 0x0003F5B2
			// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x000405D2 File Offset: 0x0003F5D2
			public bool CanHaveApplyImports
			{
				get
				{
					return (this.records[this.lastRecord].scopeFlags & XsltInput.InputScopeManager.ScopeFlags.CanHaveApplyImports) != (XsltInput.InputScopeManager.ScopeFlags)0;
				}
				set
				{
					this.SetFlag(value, XsltInput.InputScopeManager.ScopeFlags.CanHaveApplyImports);
				}
			}

			// Token: 0x06000CC4 RID: 3268 RVA: 0x000405DC File Offset: 0x0003F5DC
			public void AddExtensionNamespace(string uri)
			{
				uri = this.nameTable.Add(uri);
				XsltInput.InputScopeManager.ScopeFlags scopeFlags = this.records[this.lastRecord].scopeFlags;
				if (this.lastScopes != 0 || (scopeFlags & XsltInput.InputScopeManager.ScopeFlags.NsExtension) != (XsltInput.InputScopeManager.ScopeFlags)0)
				{
					this.AddRecord();
					scopeFlags &= XsltInput.InputScopeManager.ScopeFlags.InheritedFlags;
				}
				this.records[this.lastRecord].nsUri = uri;
				this.records[this.lastRecord].scopeFlags = scopeFlags | XsltInput.InputScopeManager.ScopeFlags.NsExtension;
			}

			// Token: 0x06000CC5 RID: 3269 RVA: 0x00040660 File Offset: 0x0003F660
			public bool IsExtensionNamespace(string nsUri)
			{
				int num = this.lastRecord;
				while (0 <= num)
				{
					if ((this.records[num].scopeFlags & XsltInput.InputScopeManager.ScopeFlags.NsExtension) != (XsltInput.InputScopeManager.ScopeFlags)0 && this.records[num].nsUri == nsUri)
					{
						return true;
					}
					num--;
				}
				return false;
			}

			// Token: 0x06000CC6 RID: 3270 RVA: 0x000406AF File Offset: 0x0003F6AF
			[Conditional("DEBUG")]
			public void CheckEmpty()
			{
				this.PopScope();
			}

			// Token: 0x040008BF RID: 2239
			private XmlNameTable nameTable;

			// Token: 0x040008C0 RID: 2240
			private XsltInput.InputScopeManager.ScopeRecord[] records = new XsltInput.InputScopeManager.ScopeRecord[32];

			// Token: 0x040008C1 RID: 2241
			private int lastRecord;

			// Token: 0x040008C2 RID: 2242
			private int lastScopes;

			// Token: 0x02000125 RID: 293
			private enum ScopeFlags
			{
				// Token: 0x040008C4 RID: 2244
				ForwardCompatibility = 1,
				// Token: 0x040008C5 RID: 2245
				CanHaveApplyImports,
				// Token: 0x040008C6 RID: 2246
				NsExtension = 4,
				// Token: 0x040008C7 RID: 2247
				InheritedFlags = 3
			}

			// Token: 0x02000126 RID: 294
			private struct ScopeRecord
			{
				// Token: 0x040008C8 RID: 2248
				public int scopeCount;

				// Token: 0x040008C9 RID: 2249
				public XsltInput.InputScopeManager.ScopeFlags scopeFlags;

				// Token: 0x040008CA RID: 2250
				public string nsUri;
			}
		}

		// Token: 0x02000127 RID: 295
		internal class ContextInfo
		{
			// Token: 0x06000CC7 RID: 3271 RVA: 0x000406B7 File Offset: 0x0003F6B7
			public ContextInfo(XsltInput input)
			{
				this.lineInfo = input.BuildLineInfo();
				this.elemNameLength = input.QualifiedName.Length;
			}

			// Token: 0x06000CC8 RID: 3272 RVA: 0x000406DC File Offset: 0x0003F6DC
			public void AddAttribute(XsltInput input)
			{
			}

			// Token: 0x06000CC9 RID: 3273 RVA: 0x000406E0 File Offset: 0x0003F6E0
			public void AddNamespace(XsltInput input)
			{
				if (Ref.Equal(input.LocalName, input.Atoms.Xml))
				{
					return;
				}
				this.nsList = new NsDecl(this.nsList, input.LocalName, input.NameTable.Add(input.Value));
			}

			// Token: 0x06000CCA RID: 3274 RVA: 0x0004072E File Offset: 0x0003F72E
			public void Finish(XsltInput input)
			{
			}

			// Token: 0x06000CCB RID: 3275 RVA: 0x00040730 File Offset: 0x0003F730
			public void SaveExtendedLineInfo(XsltInput input)
			{
				this.elemNameLi = new SourceLineInfo(this.lineInfo.Uri, this.lineInfo.StartLine, this.lineInfo.StartPos + 1, this.lineInfo.StartLine, this.lineInfo.StartPos + 1 + this.elemNameLength);
				if (!input.IsEmptyElement)
				{
					this.endTagLi = input.BuildLineInfo();
					return;
				}
				this.endTagLi = new XsltInput.ContextInfo.EmptyElementEndTag(this.lineInfo);
			}

			// Token: 0x040008CB RID: 2251
			public NsDecl nsList;

			// Token: 0x040008CC RID: 2252
			public ISourceLineInfo lineInfo;

			// Token: 0x040008CD RID: 2253
			public ISourceLineInfo elemNameLi;

			// Token: 0x040008CE RID: 2254
			public ISourceLineInfo endTagLi;

			// Token: 0x040008CF RID: 2255
			private int elemNameLength;

			// Token: 0x02000128 RID: 296
			internal class EmptyElementEndTag : ISourceLineInfo
			{
				// Token: 0x06000CCC RID: 3276 RVA: 0x000407B0 File Offset: 0x0003F7B0
				public EmptyElementEndTag(ISourceLineInfo elementTagLi)
				{
					this.elementTagLi = elementTagLi;
				}

				// Token: 0x170001A2 RID: 418
				// (get) Token: 0x06000CCD RID: 3277 RVA: 0x000407BF File Offset: 0x0003F7BF
				public string Uri
				{
					get
					{
						return this.elementTagLi.Uri;
					}
				}

				// Token: 0x170001A3 RID: 419
				// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000407CC File Offset: 0x0003F7CC
				public int StartLine
				{
					get
					{
						return this.elementTagLi.EndLine;
					}
				}

				// Token: 0x170001A4 RID: 420
				// (get) Token: 0x06000CCF RID: 3279 RVA: 0x000407D9 File Offset: 0x0003F7D9
				public int StartPos
				{
					get
					{
						return this.elementTagLi.EndPos - 2;
					}
				}

				// Token: 0x170001A5 RID: 421
				// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x000407E8 File Offset: 0x0003F7E8
				public int EndLine
				{
					get
					{
						return this.elementTagLi.EndLine;
					}
				}

				// Token: 0x170001A6 RID: 422
				// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x000407F5 File Offset: 0x0003F7F5
				public int EndPos
				{
					get
					{
						return this.elementTagLi.EndPos;
					}
				}

				// Token: 0x170001A7 RID: 423
				// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x00040802 File Offset: 0x0003F802
				public bool IsNoSource
				{
					get
					{
						return this.elementTagLi.IsNoSource;
					}
				}

				// Token: 0x040008D0 RID: 2256
				private ISourceLineInfo elementTagLi;
			}
		}
	}
}
