using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Schema;
using System.Xml.XmlConfiguration;

namespace System.Xml
{
	internal class XmlTextReaderImpl : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
	{
		internal XmlTextReaderImpl()
		{
			this.curNode = new XmlTextReaderImpl.NodeData();
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.NoData;
		}

		internal XmlTextReaderImpl(XmlNameTable nt)
		{
			this.v1Compat = true;
			this.outerReader = this;
			this.nameTable = nt;
			nt.Add(string.Empty);
			this.xmlResolver = new XmlUrlResolver();
			this.Xml = nt.Add("xml");
			this.XmlNs = nt.Add("xmlns");
			this.nodes = new XmlTextReaderImpl.NodeData[8];
			this.nodes[0] = new XmlTextReaderImpl.NodeData();
			this.curNode = this.nodes[0];
			this.stringBuilder = new BufferBuilder();
			this.xmlContext = new XmlTextReaderImpl.XmlContext();
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl;
			this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			this.entityHandling = EntityHandling.ExpandCharEntities;
			this.whitespaceHandling = WhitespaceHandling.All;
			this.closeInput = true;
			this.maxCharactersInDocument = 0L;
			this.maxCharactersFromEntities = 10000000L;
			this.charactersInDocument = 0L;
			this.charactersFromEntities = 0L;
			this.ps.lineNo = 1;
			this.ps.lineStartPos = -1;
		}

		private XmlTextReaderImpl(XmlResolver resolver, XmlReaderSettings settings, XmlParserContext context)
		{
			this.v1Compat = false;
			this.outerReader = this;
			this.xmlContext = new XmlTextReaderImpl.XmlContext();
			XmlNameTable xmlNameTable = settings.NameTable;
			if (context == null)
			{
				if (xmlNameTable == null)
				{
					xmlNameTable = new NameTable();
				}
				else
				{
					this.nameTableFromSettings = true;
				}
				this.nameTable = xmlNameTable;
				this.namespaceManager = new XmlNamespaceManager(xmlNameTable);
			}
			else
			{
				this.SetupFromParserContext(context, settings);
				xmlNameTable = this.nameTable;
			}
			xmlNameTable.Add(string.Empty);
			this.Xml = xmlNameTable.Add("xml");
			this.XmlNs = xmlNameTable.Add("xmlns");
			this.xmlResolver = resolver;
			this.nodes = new XmlTextReaderImpl.NodeData[8];
			this.nodes[0] = new XmlTextReaderImpl.NodeData();
			this.curNode = this.nodes[0];
			this.stringBuilder = new BufferBuilder();
			this.entityHandling = EntityHandling.ExpandEntities;
			this.xmlResolverIsSet = settings.IsXmlResolverSet;
			this.whitespaceHandling = (settings.IgnoreWhitespace ? WhitespaceHandling.Significant : WhitespaceHandling.All);
			this.normalize = true;
			this.ignorePIs = settings.IgnoreProcessingInstructions;
			this.ignoreComments = settings.IgnoreComments;
			this.checkCharacters = settings.CheckCharacters;
			this.lineNumberOffset = settings.LineNumberOffset;
			this.linePositionOffset = settings.LinePositionOffset;
			this.ps.lineNo = this.lineNumberOffset + 1;
			this.ps.lineStartPos = -this.linePositionOffset - 1;
			this.curNode.SetLineInfo(this.ps.LineNo - 1, this.ps.LinePos - 1);
			this.prohibitDtd = settings.ProhibitDtd;
			this.maxCharactersInDocument = settings.MaxCharactersInDocument;
			this.maxCharactersFromEntities = settings.MaxCharactersFromEntities;
			this.charactersInDocument = 0L;
			this.charactersFromEntities = 0L;
			this.fragmentParserContext = context;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl;
			this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			switch (settings.ConformanceLevel)
			{
			case ConformanceLevel.Auto:
				this.fragmentType = XmlNodeType.None;
				this.fragment = true;
				return;
			case ConformanceLevel.Fragment:
				this.fragmentType = XmlNodeType.Element;
				this.fragment = true;
				return;
			}
			this.fragmentType = XmlNodeType.Document;
		}

		internal XmlTextReaderImpl(Stream input)
			: this(string.Empty, input, new NameTable())
		{
		}

		internal XmlTextReaderImpl(Stream input, XmlNameTable nt)
			: this(string.Empty, input, nt)
		{
		}

		internal XmlTextReaderImpl(string url, Stream input)
			: this(url, input, new NameTable())
		{
		}

		internal XmlTextReaderImpl(string url, Stream input, XmlNameTable nt)
			: this(nt)
		{
			this.namespaceManager = new XmlNamespaceManager(nt);
			if (url == null || url.Length == 0)
			{
				this.InitStreamInput(input, null);
			}
			else
			{
				this.InitStreamInput(url, input, null);
			}
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
		}

		internal XmlTextReaderImpl(TextReader input)
			: this(string.Empty, input, new NameTable())
		{
		}

		internal XmlTextReaderImpl(TextReader input, XmlNameTable nt)
			: this(string.Empty, input, nt)
		{
		}

		internal XmlTextReaderImpl(string url, TextReader input)
			: this(url, input, new NameTable())
		{
		}

		internal XmlTextReaderImpl(string url, TextReader input, XmlNameTable nt)
			: this(nt)
		{
			this.namespaceManager = new XmlNamespaceManager(nt);
			this.reportedBaseUri = ((url != null) ? url : string.Empty);
			this.InitTextReaderInput(this.reportedBaseUri, input);
			this.reportedEncoding = this.ps.encoding;
		}

		internal XmlTextReaderImpl(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
			: this((context != null && context.NameTable != null) ? context.NameTable : new NameTable())
		{
			Encoding encoding = ((context != null) ? context.Encoding : null);
			if (context == null || context.BaseURI == null || context.BaseURI.Length == 0)
			{
				this.InitStreamInput(xmlFragment, encoding);
			}
			else
			{
				this.InitStreamInput(this.xmlResolver.ResolveUri(null, context.BaseURI), xmlFragment, encoding);
			}
			this.InitFragmentReader(fragType, context, false);
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
		}

		internal XmlTextReaderImpl(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
			: this((context == null || context.NameTable == null) ? new NameTable() : context.NameTable)
		{
			if (context == null)
			{
				this.InitStringInput(string.Empty, Encoding.Unicode, xmlFragment);
			}
			else
			{
				this.reportedBaseUri = context.BaseURI;
				this.InitStringInput(context.BaseURI, Encoding.Unicode, xmlFragment);
			}
			this.InitFragmentReader(fragType, context, false);
			this.reportedEncoding = this.ps.encoding;
		}

		internal XmlTextReaderImpl(string xmlFragment, XmlParserContext context)
			: this((context == null || context.NameTable == null) ? new NameTable() : context.NameTable)
		{
			this.InitStringInput((context == null) ? string.Empty : context.BaseURI, Encoding.Unicode, "<?xml " + xmlFragment + "?>");
			this.InitFragmentReader(XmlNodeType.XmlDeclaration, context, true);
		}

		public XmlTextReaderImpl(string url)
			: this(url, new NameTable())
		{
		}

		public XmlTextReaderImpl(string url, XmlNameTable nt)
			: this(nt)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url.Length == 0)
			{
				throw new ArgumentException(Res.GetString("Xml_EmptyUrl"), "url");
			}
			this.namespaceManager = new XmlNamespaceManager(nt);
			this.compressedStack = CompressedStack.Capture();
			this.url = url;
			this.ps.baseUri = this.xmlResolver.ResolveUri(null, url);
			this.ps.baseUriStr = this.ps.baseUri.ToString();
			this.reportedBaseUri = this.ps.baseUriStr;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.OpenUrl;
		}

		internal XmlTextReaderImpl(Stream stream, byte[] bytes, int byteCount, XmlReaderSettings settings, Uri baseUri, string baseUriStr, XmlParserContext context, bool closeInput)
			: this(settings.GetXmlResolver(), settings, context)
		{
			Encoding encoding = null;
			if (context != null)
			{
				if (context.BaseURI != null && context.BaseURI.Length > 0 && !this.UriEqual(baseUri, baseUriStr, context.BaseURI, settings.GetXmlResolver()))
				{
					if (baseUriStr.Length > 0)
					{
						this.Throw("Xml_DoubleBaseUri");
					}
					baseUriStr = context.BaseURI;
				}
				encoding = context.Encoding;
			}
			this.InitStreamInput(baseUri, baseUriStr, stream, bytes, byteCount, encoding);
			this.closeInput = closeInput;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
			if (context != null && context.HasDtdInfo)
			{
				if (this.prohibitDtd)
				{
					this.ThrowWithoutLineInfo("Xml_DtdIsProhibitedEx", string.Empty);
				}
				this.ParseDtdFromParserContext();
			}
		}

		internal XmlTextReaderImpl(TextReader input, XmlReaderSettings settings, string baseUriStr, XmlParserContext context)
			: this(settings.GetXmlResolver(), settings, context)
		{
			if (context != null && context.BaseURI != null)
			{
				baseUriStr = context.BaseURI;
			}
			this.InitTextReaderInput(baseUriStr, input);
			this.closeInput = settings.CloseInput;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
			if (context != null && context.HasDtdInfo)
			{
				if (this.prohibitDtd)
				{
					this.ThrowWithoutLineInfo("Xml_DtdIsProhibitedEx", string.Empty);
				}
				this.ParseDtdFromParserContext();
			}
		}

		internal XmlTextReaderImpl(string xmlFragment, XmlParserContext context, XmlReaderSettings settings)
			: this(null, settings, context)
		{
			this.InitStringInput(string.Empty, Encoding.Unicode, xmlFragment);
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
		}

		public override XmlReaderSettings Settings
		{
			get
			{
				if (this.v1Compat)
				{
					return null;
				}
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				if (this.nameTableFromSettings)
				{
					xmlReaderSettings.NameTable = this.nameTable;
				}
				XmlNodeType xmlNodeType = this.fragmentType;
				switch (xmlNodeType)
				{
				case XmlNodeType.None:
					break;
				case XmlNodeType.Element:
					xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
					goto IL_0057;
				default:
					if (xmlNodeType == XmlNodeType.Document)
					{
						xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
						goto IL_0057;
					}
					break;
				}
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
				IL_0057:
				xmlReaderSettings.CheckCharacters = this.checkCharacters;
				xmlReaderSettings.LineNumberOffset = this.lineNumberOffset;
				xmlReaderSettings.LinePositionOffset = this.linePositionOffset;
				xmlReaderSettings.IgnoreWhitespace = this.whitespaceHandling == WhitespaceHandling.Significant;
				xmlReaderSettings.IgnoreProcessingInstructions = this.ignorePIs;
				xmlReaderSettings.IgnoreComments = this.ignoreComments;
				xmlReaderSettings.ProhibitDtd = this.prohibitDtd;
				xmlReaderSettings.MaxCharactersInDocument = this.maxCharactersInDocument;
				xmlReaderSettings.MaxCharactersFromEntities = this.maxCharactersFromEntities;
				xmlReaderSettings.ReadOnly = true;
				return xmlReaderSettings;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return this.curNode.type;
			}
		}

		public override string Name
		{
			get
			{
				return this.curNode.GetNameWPrefix(this.nameTable);
			}
		}

		public override string LocalName
		{
			get
			{
				return this.curNode.localName;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return this.curNode.ns;
			}
		}

		public override string Prefix
		{
			get
			{
				return this.curNode.prefix;
			}
		}

		public override bool HasValue
		{
			get
			{
				return XmlReader.HasValueInternal(this.curNode.type);
			}
		}

		public override string Value
		{
			get
			{
				if (this.parsingFunction >= XmlTextReaderImpl.ParsingFunction.PartialTextValue)
				{
					if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.PartialTextValue)
					{
						this.FinishPartialValue();
						this.parsingFunction = this.nextParsingFunction;
					}
					else
					{
						this.FinishOtherValueIterator();
					}
				}
				return this.curNode.StringValue;
			}
		}

		public override int Depth
		{
			get
			{
				return this.curNode.depth;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.reportedBaseUri;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.curNode.IsEmptyElement;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.curNode.IsDefaultAttribute;
			}
		}

		public override char QuoteChar
		{
			get
			{
				if (this.curNode.type != XmlNodeType.Attribute)
				{
					return '"';
				}
				return this.curNode.quoteChar;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				return this.xmlContext.xmlSpace;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.xmlContext.xmlLang;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return this.readState;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.parsingFunction == XmlTextReaderImpl.ParsingFunction.Eof;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		public override int AttributeCount
		{
			get
			{
				return this.attrCount;
			}
		}

		public override string GetAttribute(string name)
		{
			int num;
			if (name.IndexOf(':') == -1)
			{
				num = this.GetIndexOfAttributeWithoutPrefix(name);
			}
			else
			{
				num = this.GetIndexOfAttributeWithPrefix(name);
			}
			if (num < 0)
			{
				return null;
			}
			return this.nodes[num].StringValue;
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			namespaceURI = ((namespaceURI == null) ? string.Empty : this.nameTable.Get(namespaceURI));
			localName = this.nameTable.Get(localName);
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].localName, localName) && Ref.Equal(this.nodes[i].ns, namespaceURI))
				{
					return this.nodes[i].StringValue;
				}
			}
			return null;
		}

		public override string GetAttribute(int i)
		{
			if (i < 0 || i >= this.attrCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			return this.nodes[this.index + i + 1].StringValue;
		}

		public override bool MoveToAttribute(string name)
		{
			int num;
			if (name.IndexOf(':') == -1)
			{
				num = this.GetIndexOfAttributeWithoutPrefix(name);
			}
			else
			{
				num = this.GetIndexOfAttributeWithPrefix(name);
			}
			if (num >= 0)
			{
				if (this.InAttributeValueIterator)
				{
					this.FinishAttributeValueIterator();
				}
				this.curAttrIndex = num - this.index - 1;
				this.curNode = this.nodes[num];
				return true;
			}
			return false;
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			namespaceURI = ((namespaceURI == null) ? string.Empty : this.nameTable.Get(namespaceURI));
			localName = this.nameTable.Get(localName);
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].localName, localName) && Ref.Equal(this.nodes[i].ns, namespaceURI))
				{
					this.curAttrIndex = i - this.index - 1;
					this.curNode = this.nodes[i];
					if (this.InAttributeValueIterator)
					{
						this.FinishAttributeValueIterator();
					}
					return true;
				}
			}
			return false;
		}

		public override void MoveToAttribute(int i)
		{
			if (i < 0 || i >= this.attrCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
			}
			this.curAttrIndex = i;
			this.curNode = this.nodes[this.index + 1 + this.curAttrIndex];
		}

		public override bool MoveToFirstAttribute()
		{
			if (this.attrCount == 0)
			{
				return false;
			}
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
			}
			this.curAttrIndex = 0;
			this.curNode = this.nodes[this.index + 1];
			return true;
		}

		public override bool MoveToNextAttribute()
		{
			if (this.curAttrIndex + 1 < this.attrCount)
			{
				if (this.InAttributeValueIterator)
				{
					this.FinishAttributeValueIterator();
				}
				this.curNode = this.nodes[this.index + 1 + ++this.curAttrIndex];
				return true;
			}
			return false;
		}

		public override bool MoveToElement()
		{
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
			}
			else if (this.curNode.type != XmlNodeType.Attribute)
			{
				return false;
			}
			this.curAttrIndex = -1;
			this.curNode = this.nodes[this.index];
			return true;
		}

		public override bool Read()
		{
			for (;;)
			{
				switch (this.parsingFunction)
				{
				case XmlTextReaderImpl.ParsingFunction.ElementContent:
					goto IL_0077;
				case XmlTextReaderImpl.ParsingFunction.NoData:
					goto IL_02D9;
				case XmlTextReaderImpl.ParsingFunction.OpenUrl:
					this.OpenUrl();
					break;
				case XmlTextReaderImpl.ParsingFunction.SwitchToInteractive:
					this.readState = ReadState.Interactive;
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl:
					break;
				case XmlTextReaderImpl.ParsingFunction.DocumentContent:
					goto IL_007E;
				case XmlTextReaderImpl.ParsingFunction.MoveToElementContent:
					this.ResetAttributes();
					this.index++;
					this.curNode = this.AddNode(this.index, this.index);
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ElementContent;
					continue;
				case XmlTextReaderImpl.ParsingFunction.PopElementContext:
					this.PopElementContext();
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.PopEmptyElementContext:
					this.curNode = this.nodes[this.index];
					this.curNode.IsEmptyElement = false;
					this.ResetAttributes();
					this.PopElementContext();
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.ResetAttributesRootLevel:
					this.ResetAttributes();
					this.curNode = this.nodes[this.index];
					this.parsingFunction = ((this.index == 0) ? XmlTextReaderImpl.ParsingFunction.DocumentContent : XmlTextReaderImpl.ParsingFunction.ElementContent);
					continue;
				case XmlTextReaderImpl.ParsingFunction.Error:
				case XmlTextReaderImpl.ParsingFunction.Eof:
				case XmlTextReaderImpl.ParsingFunction.ReaderClosed:
					return false;
				case XmlTextReaderImpl.ParsingFunction.EntityReference:
					goto IL_01A5;
				case XmlTextReaderImpl.ParsingFunction.InIncrementalRead:
					goto IL_02B0;
				case XmlTextReaderImpl.ParsingFunction.FragmentAttribute:
					goto IL_02B8;
				case XmlTextReaderImpl.ParsingFunction.ReportEndEntity:
					goto IL_01B9;
				case XmlTextReaderImpl.ParsingFunction.AfterResolveEntityInContent:
					this.curNode = this.AddNode(this.index, this.index);
					this.reportedEncoding = this.ps.encoding;
					this.reportedBaseUri = this.ps.baseUriStr;
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.AfterResolveEmptyEntityInContent:
					goto IL_0218;
				case XmlTextReaderImpl.ParsingFunction.XmlDeclarationFragment:
					goto IL_02BF;
				case XmlTextReaderImpl.ParsingFunction.GoToEof:
					goto IL_02CF;
				case XmlTextReaderImpl.ParsingFunction.PartialTextValue:
					this.SkipPartialTextValue();
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadAttributeValue:
					this.FinishAttributeValueIterator();
					this.curNode = this.nodes[this.index];
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadValueChunk:
					this.FinishReadValueChunk();
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary:
					this.FinishReadContentAsBinary();
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary:
					this.FinishReadElementContentAsBinary();
					continue;
				default:
					continue;
				}
				this.readState = ReadState.Interactive;
				this.parsingFunction = this.nextParsingFunction;
				if (this.ParseXmlDeclaration(false))
				{
					goto Block_1;
				}
				this.reportedEncoding = this.ps.encoding;
			}
			IL_0077:
			return this.ParseElementContent();
			IL_007E:
			return this.ParseDocumentContent();
			Block_1:
			this.reportedEncoding = this.ps.encoding;
			return true;
			IL_01A5:
			this.parsingFunction = this.nextParsingFunction;
			this.ParseEntityReference();
			return true;
			IL_01B9:
			this.SetupEndEntityNodeInContent();
			this.parsingFunction = this.nextParsingFunction;
			return true;
			IL_0218:
			this.curNode = this.AddNode(this.index, this.index);
			this.curNode.SetValueNode(XmlNodeType.Text, string.Empty);
			this.curNode.SetLineInfo(this.ps.lineNo, this.ps.LinePos);
			this.reportedEncoding = this.ps.encoding;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.parsingFunction = this.nextParsingFunction;
			return true;
			IL_02B0:
			this.FinishIncrementalRead();
			return true;
			IL_02B8:
			return this.ParseFragmentAttribute();
			IL_02BF:
			this.ParseXmlDeclarationFragment();
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.GoToEof;
			return true;
			IL_02CF:
			this.OnEof();
			return false;
			IL_02D9:
			this.ThrowWithoutLineInfo("Xml_MissingRoot");
			return false;
		}

		public override void Close()
		{
			this.Close(this.closeInput);
		}

		public override void Skip()
		{
			if (this.readState != ReadState.Interactive)
			{
				return;
			}
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
				this.curNode = this.nodes[this.index];
			}
			else
			{
				XmlTextReaderImpl.ParsingFunction parsingFunction = this.parsingFunction;
				if (parsingFunction != XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
				{
					switch (parsingFunction)
					{
					case XmlTextReaderImpl.ParsingFunction.PartialTextValue:
						this.SkipPartialTextValue();
						break;
					case XmlTextReaderImpl.ParsingFunction.InReadValueChunk:
						this.FinishReadValueChunk();
						break;
					case XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary:
						this.FinishReadContentAsBinary();
						break;
					case XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary:
						this.FinishReadElementContentAsBinary();
						break;
					}
				}
				else
				{
					this.FinishIncrementalRead();
				}
			}
			switch (this.curNode.type)
			{
			case XmlNodeType.Element:
				break;
			case XmlNodeType.Attribute:
				this.outerReader.MoveToElement();
				break;
			default:
				goto IL_00E4;
			}
			if (!this.curNode.IsEmptyElement)
			{
				int num = this.index;
				this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipContent;
				while (this.outerReader.Read() && this.index > num)
				{
				}
				this.parsingMode = XmlTextReaderImpl.ParsingMode.Full;
			}
			IL_00E4:
			this.outerReader.Read();
		}

		public override string LookupNamespace(string prefix)
		{
			if (!this.supportNamespaces)
			{
				return null;
			}
			return this.namespaceManager.LookupNamespace(prefix);
		}

		public override bool ReadAttributeValue()
		{
			if (this.parsingFunction != XmlTextReaderImpl.ParsingFunction.InReadAttributeValue)
			{
				if (this.curNode.type != XmlNodeType.Attribute)
				{
					return false;
				}
				if (this.readState != ReadState.Interactive || this.curAttrIndex < 0)
				{
					return false;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
				{
					this.FinishReadValueChunk();
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
				{
					this.FinishReadContentAsBinary();
				}
				if (this.curNode.nextAttrValueChunk == null || this.entityHandling == EntityHandling.ExpandEntities)
				{
					XmlTextReaderImpl.NodeData nodeData = this.AddNode(this.index + this.attrCount + 1, this.curNode.depth + 1);
					nodeData.SetValueNode(XmlNodeType.Text, this.curNode.StringValue);
					nodeData.lineInfo = this.curNode.lineInfo2;
					nodeData.depth = this.curNode.depth + 1;
					nodeData.nextAttrValueChunk = null;
					this.curNode = nodeData;
				}
				else
				{
					this.curNode = this.curNode.nextAttrValueChunk;
					this.AddNode(this.index + this.attrCount + 1, this.index + 2);
					this.nodes[this.index + this.attrCount + 1] = this.curNode;
					this.fullAttrCleanup = true;
				}
				this.nextParsingFunction = this.parsingFunction;
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InReadAttributeValue;
				this.attributeValueBaseEntityId = this.ps.entityId;
				return true;
			}
			else
			{
				if (this.ps.entityId != this.attributeValueBaseEntityId)
				{
					return this.ParseAttributeValueChunk();
				}
				if (this.curNode.nextAttrValueChunk != null)
				{
					this.curNode = this.curNode.nextAttrValueChunk;
					this.nodes[this.index + this.attrCount + 1] = this.curNode;
					return true;
				}
				return false;
			}
		}

		public override void ResolveEntity()
		{
			if (this.curNode.type != XmlNodeType.EntityReference)
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadAttributeValue || this.parsingFunction == XmlTextReaderImpl.ParsingFunction.FragmentAttribute)
			{
				switch (this.HandleGeneralEntityReference(this.curNode.localName, true, true, this.curNode.LinePos))
				{
				case XmlTextReaderImpl.EntityType.Expanded:
				case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
					if (this.ps.charsUsed - this.ps.charPos == 0)
					{
						this.emptyEntityInAttributeResolved = true;
						goto IL_0157;
					}
					goto IL_0157;
				case XmlTextReaderImpl.EntityType.FakeExpanded:
					this.emptyEntityInAttributeResolved = true;
					goto IL_0157;
				}
				throw new XmlException("Xml_InternalError");
			}
			switch (this.HandleGeneralEntityReference(this.curNode.localName, false, true, this.curNode.LinePos))
			{
			case XmlTextReaderImpl.EntityType.Expanded:
			case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
				this.nextParsingFunction = this.parsingFunction;
				if (this.ps.charsUsed - this.ps.charPos == 0 && !this.ps.entity.IsExternal)
				{
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.AfterResolveEmptyEntityInContent;
					goto IL_0157;
				}
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.AfterResolveEntityInContent;
				goto IL_0157;
			case XmlTextReaderImpl.EntityType.FakeExpanded:
				this.nextParsingFunction = this.parsingFunction;
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.AfterResolveEmptyEntityInContent;
				goto IL_0157;
			}
			throw new XmlException("Xml_InternalError");
			IL_0157:
			this.ps.entityResolvedManually = true;
			this.index++;
		}

		internal XmlReader OuterReader
		{
			get
			{
				return this.outerReader;
			}
			set
			{
				this.outerReader = value;
			}
		}

		internal void MoveOffEntityReference()
		{
			if (this.outerReader.NodeType == XmlNodeType.EntityReference && this.parsingFunction == XmlTextReaderImpl.ParsingFunction.AfterResolveEntityInContent && !this.outerReader.Read())
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		public override string ReadString()
		{
			this.MoveOffEntityReference();
			return base.ReadString();
		}

		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
			{
				if (this.incReadDecoder == this.base64Decoder)
				{
					return this.ReadContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (!XmlReader.CanReadContentAs(this.curNode.type))
				{
					throw base.CreateReadContentAsException("ReadContentAsBase64");
				}
				if (!this.InitReadContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBase64Decoder();
			return this.ReadContentAsBinary(buffer, index, count);
		}

		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
			{
				if (this.incReadDecoder == this.binHexDecoder)
				{
					return this.ReadContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (!XmlReader.CanReadContentAs(this.curNode.type))
				{
					throw base.CreateReadContentAsException("ReadContentAsBinHex");
				}
				if (!this.InitReadContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBinHexDecoder();
			return this.ReadContentAsBinary(buffer, index, count);
		}

		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
			{
				if (this.incReadDecoder == this.base64Decoder)
				{
					return this.ReadElementContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (this.curNode.type != XmlNodeType.Element)
				{
					throw base.CreateReadElementContentAsException("ReadElementContentAsBinHex");
				}
				if (!this.InitReadElementContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBase64Decoder();
			return this.ReadElementContentAsBinary(buffer, index, count);
		}

		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
			{
				if (this.incReadDecoder == this.binHexDecoder)
				{
					return this.ReadElementContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (this.curNode.type != XmlNodeType.Element)
				{
					throw base.CreateReadElementContentAsException("ReadElementContentAsBinHex");
				}
				if (!this.InitReadElementContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBinHexDecoder();
			return this.ReadElementContentAsBinary(buffer, index, count);
		}

		public override bool CanReadValueChunk
		{
			get
			{
				return true;
			}
		}

		public override int ReadValueChunk(char[] buffer, int index, int count)
		{
			if (!XmlReader.HasValueInternal(this.curNode.type))
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidReadValueChunk", new object[] { this.curNode.type }));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction != XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.PartialTextValue)
				{
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue;
				}
				else
				{
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnCachedValue;
					this.nextNextParsingFunction = this.nextParsingFunction;
					this.nextParsingFunction = this.parsingFunction;
				}
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InReadValueChunk;
				this.readValueOffset = 0;
			}
			if (count == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = this.curNode.CopyTo(this.readValueOffset, buffer, index + num, count - num);
			num += num2;
			this.readValueOffset += num2;
			if (num == count)
			{
				char c = buffer[index + count - 1];
				if (c >= '\ud800' && c <= '\udbff')
				{
					num--;
					this.readValueOffset--;
					if (num == 0)
					{
						this.Throw("Xml_NotEnoughSpaceForSurrogatePair");
					}
				}
				return num;
			}
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue)
			{
				this.curNode.SetValue(string.Empty);
				bool flag = false;
				int num3 = 0;
				int num4 = 0;
				while (num < count && !flag)
				{
					int num5 = 0;
					flag = this.ParseText(out num3, out num4, ref num5);
					int num6 = count - num;
					if (num6 > num4 - num3)
					{
						num6 = num4 - num3;
					}
					Buffer.BlockCopy(this.ps.chars, num3 * 2, buffer, (index + num) * 2, num6 * 2);
					num += num6;
					num3 += num6;
				}
				this.incReadState = (flag ? XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnCachedValue : XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue);
				if (num == count)
				{
					char c2 = buffer[index + count - 1];
					if (c2 >= '\ud800' && c2 <= '\udbff')
					{
						num--;
						num3--;
						if (num == 0)
						{
							this.Throw("Xml_NotEnoughSpaceForSurrogatePair");
						}
					}
				}
				this.readValueOffset = 0;
				this.curNode.SetValue(this.ps.chars, num3, num4 - num3);
			}
			return num;
		}

		public bool HasLineInfo()
		{
			return true;
		}

		public int LineNumber
		{
			get
			{
				return this.curNode.LineNo;
			}
		}

		public int LinePosition
		{
			get
			{
				return this.curNode.LinePos;
			}
		}

		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.GetNamespacesInScope(scope);
		}

		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.LookupNamespace(prefix);
		}

		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.LookupPrefix(namespaceName);
		}

		internal IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.namespaceManager.GetNamespacesInScope(scope);
		}

		internal string LookupPrefix(string namespaceName)
		{
			return this.namespaceManager.LookupPrefix(namespaceName);
		}

		internal bool Namespaces
		{
			get
			{
				return this.supportNamespaces;
			}
			set
			{
				if (this.readState != ReadState.Initial)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				this.supportNamespaces = value;
				if (value)
				{
					if (this.namespaceManager is XmlTextReaderImpl.NoNamespaceManager)
					{
						if (this.fragment && this.fragmentParserContext != null && this.fragmentParserContext.NamespaceManager != null)
						{
							this.namespaceManager = this.fragmentParserContext.NamespaceManager;
						}
						else
						{
							this.namespaceManager = new XmlNamespaceManager(this.nameTable);
						}
					}
					this.xmlContext.defaultNamespace = this.namespaceManager.LookupNamespace(string.Empty);
					return;
				}
				if (!(this.namespaceManager is XmlTextReaderImpl.NoNamespaceManager))
				{
					this.namespaceManager = new XmlTextReaderImpl.NoNamespaceManager();
				}
				this.xmlContext.defaultNamespace = string.Empty;
			}
		}

		internal bool Normalization
		{
			get
			{
				return this.normalize;
			}
			set
			{
				if (this.readState == ReadState.Closed)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				this.normalize = value;
				if (this.ps.entity == null || this.ps.entity.IsExternal)
				{
					this.ps.eolNormalized = !value;
				}
			}
		}

		internal Encoding Encoding
		{
			get
			{
				if (this.readState != ReadState.Interactive)
				{
					return null;
				}
				return this.reportedEncoding;
			}
		}

		internal WhitespaceHandling WhitespaceHandling
		{
			get
			{
				return this.whitespaceHandling;
			}
			set
			{
				if (this.readState == ReadState.Closed)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				if (value > WhitespaceHandling.None)
				{
					throw new XmlException("Xml_WhitespaceHandling", string.Empty);
				}
				this.whitespaceHandling = value;
			}
		}

		internal bool ProhibitDtd
		{
			get
			{
				return this.prohibitDtd;
			}
			set
			{
				this.prohibitDtd = value;
			}
		}

		internal EntityHandling EntityHandling
		{
			get
			{
				return this.entityHandling;
			}
			set
			{
				if (value != EntityHandling.ExpandEntities && value != EntityHandling.ExpandCharEntities)
				{
					throw new XmlException("Xml_EntityHandling", string.Empty);
				}
				this.entityHandling = value;
			}
		}

		internal XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
				this.xmlResolverIsSet = true;
				this.ps.baseUri = null;
				for (int i = 0; i <= this.parsingStatesStackTop; i++)
				{
					this.parsingStatesStack[i].baseUri = null;
				}
			}
		}

		internal void ResetState()
		{
			if (this.fragment)
			{
				this.Throw(new InvalidOperationException(Res.GetString("Xml_InvalidResetStateCall")));
			}
			if (this.readState == ReadState.Initial)
			{
				return;
			}
			this.ResetAttributes();
			while (this.namespaceManager.PopScope())
			{
			}
			while (this.InEntity)
			{
				this.HandleEntityEnd(true);
			}
			this.readState = ReadState.Initial;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl;
			this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			this.curNode = this.nodes[0];
			this.curNode.Clear(XmlNodeType.None);
			this.curNode.SetLineInfo(0, 0);
			this.index = 0;
			this.rootElementParsed = false;
			this.charactersInDocument = 0L;
			this.charactersFromEntities = 0L;
			this.afterResetState = true;
		}

		internal TextReader GetRemainder()
		{
			XmlTextReaderImpl.ParsingFunction parsingFunction = this.parsingFunction;
			if (parsingFunction != XmlTextReaderImpl.ParsingFunction.OpenUrl)
			{
				switch (parsingFunction)
				{
				case XmlTextReaderImpl.ParsingFunction.Eof:
				case XmlTextReaderImpl.ParsingFunction.ReaderClosed:
					return new StringReader(string.Empty);
				case XmlTextReaderImpl.ParsingFunction.InIncrementalRead:
					if (!this.InEntity)
					{
						this.stringBuilder.Append(this.ps.chars, this.incReadLeftStartPos, this.incReadLeftEndPos - this.incReadLeftStartPos);
					}
					break;
				}
			}
			else
			{
				this.OpenUrl();
			}
			while (this.InEntity)
			{
				this.HandleEntityEnd(true);
			}
			this.ps.appendMode = false;
			do
			{
				this.stringBuilder.Append(this.ps.chars, this.ps.charPos, this.ps.charsUsed - this.ps.charPos);
				this.ps.charPos = this.ps.charsUsed;
			}
			while (this.ReadData() != 0);
			this.OnEof();
			string text = this.stringBuilder.ToString();
			this.stringBuilder.Length = 0;
			return new StringReader(text);
		}

		internal int ReadChars(char[] buffer, int index, int count)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				if (this.incReadDecoder != this.readCharsDecoder)
				{
					if (this.readCharsDecoder == null)
					{
						this.readCharsDecoder = new IncrementalReadCharsDecoder();
					}
					this.readCharsDecoder.Reset();
					this.incReadDecoder = this.readCharsDecoder;
				}
				return this.IncrementalRead(buffer, index, count);
			}
			if (this.curNode.type != XmlNodeType.Element)
			{
				return 0;
			}
			if (this.curNode.IsEmptyElement)
			{
				this.outerReader.Read();
				return 0;
			}
			if (this.readCharsDecoder == null)
			{
				this.readCharsDecoder = new IncrementalReadCharsDecoder();
			}
			this.InitIncrementalRead(this.readCharsDecoder);
			return this.IncrementalRead(buffer, index, count);
		}

		internal int ReadBase64(byte[] array, int offset, int len)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				if (this.incReadDecoder != this.base64Decoder)
				{
					this.InitBase64Decoder();
				}
				return this.IncrementalRead(array, offset, len);
			}
			if (this.curNode.type != XmlNodeType.Element)
			{
				return 0;
			}
			if (this.curNode.IsEmptyElement)
			{
				this.outerReader.Read();
				return 0;
			}
			if (this.base64Decoder == null)
			{
				this.base64Decoder = new Base64Decoder();
			}
			this.InitIncrementalRead(this.base64Decoder);
			return this.IncrementalRead(array, offset, len);
		}

		internal int ReadBinHex(byte[] array, int offset, int len)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				if (this.incReadDecoder != this.binHexDecoder)
				{
					this.InitBinHexDecoder();
				}
				return this.IncrementalRead(array, offset, len);
			}
			if (this.curNode.type != XmlNodeType.Element)
			{
				return 0;
			}
			if (this.curNode.IsEmptyElement)
			{
				this.outerReader.Read();
				return 0;
			}
			if (this.binHexDecoder == null)
			{
				this.binHexDecoder = new BinHexDecoder();
			}
			this.InitIncrementalRead(this.binHexDecoder);
			return this.IncrementalRead(array, offset, len);
		}

		internal XmlNameTable DtdParserProxy_NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		internal XmlNamespaceManager DtdParserProxy_NamespaceManager
		{
			get
			{
				return this.namespaceManager;
			}
		}

		internal bool DtdParserProxy_DtdValidation
		{
			get
			{
				return this.DtdValidation;
			}
		}

		internal bool DtdParserProxy_Normalization
		{
			get
			{
				return this.normalize;
			}
		}

		internal bool DtdParserProxy_Namespaces
		{
			get
			{
				return this.supportNamespaces;
			}
		}

		internal bool DtdParserProxy_V1CompatibilityMode
		{
			get
			{
				return this.v1Compat;
			}
		}

		internal Uri DtdParserProxy_BaseUri
		{
			get
			{
				if (this.ps.baseUriStr.Length > 0 && this.ps.baseUri == null && this.xmlResolver != null)
				{
					this.ps.baseUri = this.xmlResolver.ResolveUri(null, this.ps.baseUriStr);
				}
				return this.ps.baseUri;
			}
		}

		internal bool DtdParserProxy_IsEof
		{
			get
			{
				return this.ps.isEof;
			}
		}

		internal char[] DtdParserProxy_ParsingBuffer
		{
			get
			{
				return this.ps.chars;
			}
		}

		internal int DtdParserProxy_ParsingBufferLength
		{
			get
			{
				return this.ps.charsUsed;
			}
		}

		internal int DtdParserProxy_CurrentPosition
		{
			get
			{
				return this.ps.charPos;
			}
			set
			{
				this.ps.charPos = value;
			}
		}

		internal int DtdParserProxy_EntityStackLength
		{
			get
			{
				return this.parsingStatesStackTop + 1;
			}
		}

		internal bool DtdParserProxy_IsEntityEolNormalized
		{
			get
			{
				return this.ps.eolNormalized;
			}
		}

		internal ValidationEventHandler DtdParserProxy_EventHandler
		{
			get
			{
				return this.validationEventHandler;
			}
			set
			{
				this.validationEventHandler = value;
			}
		}

		internal void DtdParserProxy_OnNewLine(int pos)
		{
			this.OnNewLine(pos);
		}

		internal int DtdParserProxy_LineNo
		{
			get
			{
				return this.ps.LineNo;
			}
		}

		internal int DtdParserProxy_LineStartPosition
		{
			get
			{
				return this.ps.lineStartPos;
			}
		}

		internal int DtdParserProxy_ReadData()
		{
			return this.ReadData();
		}

		internal void DtdParserProxy_SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception)
		{
			if (this.DtdValidation)
			{
				this.SendValidationEvent(severity, exception);
			}
		}

		internal int DtdParserProxy_ParseNumericCharRef(BufferBuilder internalSubsetBuilder)
		{
			XmlTextReaderImpl.EntityType entityType;
			return this.ParseNumericCharRef(true, internalSubsetBuilder, out entityType);
		}

		internal int DtdParserProxy_ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder)
		{
			return this.ParseNamedCharRef(expand, internalSubsetBuilder);
		}

		internal void DtdParserProxy_ParsePI(BufferBuilder sb)
		{
			if (sb == null)
			{
				XmlTextReaderImpl.ParsingMode parsingMode = this.parsingMode;
				this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipNode;
				this.ParsePI(null);
				this.parsingMode = parsingMode;
				return;
			}
			this.ParsePI(sb);
		}

		internal void DtdParserProxy_ParseComment(BufferBuilder sb)
		{
			try
			{
				if (sb == null)
				{
					XmlTextReaderImpl.ParsingMode parsingMode = this.parsingMode;
					this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipNode;
					this.ParseCDataOrComment(XmlNodeType.Comment);
					this.parsingMode = parsingMode;
				}
				else
				{
					XmlTextReaderImpl.NodeData nodeData = this.curNode;
					this.curNode = this.AddNode(this.index + this.attrCount + 1, this.index);
					this.ParseCDataOrComment(XmlNodeType.Comment);
					this.curNode.CopyTo(sb);
					this.curNode = nodeData;
				}
			}
			catch (XmlException ex)
			{
				if (!(ex.ResString == "Xml_UnexpectedEOF") || this.ps.entity == null)
				{
					throw;
				}
				this.SendValidationEvent(XmlSeverityType.Error, "Sch_ParEntityRefNesting", null, this.ps.LineNo, this.ps.LinePos);
			}
		}

		private bool IsResolverNull
		{
			get
			{
				return this.xmlResolver == null || (XmlReaderSection.ProhibitDefaultUrlResolver && !this.xmlResolverIsSet);
			}
		}

		internal bool DtdParserProxy_PushEntity(SchemaEntity entity, int entityId)
		{
			if (entity.IsExternal)
			{
				return !this.IsResolverNull && this.PushExternalEntity(entity, entityId);
			}
			this.PushInternalEntity(entity, entityId);
			return true;
		}

		internal bool DtdParserProxy_PopEntity(out SchemaEntity oldEntity, out int newEntityId)
		{
			if (this.parsingStatesStackTop == -1)
			{
				oldEntity = null;
				newEntityId = -1;
				return false;
			}
			oldEntity = this.ps.entity;
			this.PopEntity();
			newEntityId = this.ps.entityId;
			return true;
		}

		internal bool DtdParserProxy_PushExternalSubset(string systemId, string publicId)
		{
			if (this.IsResolverNull)
			{
				return false;
			}
			if (this.ps.baseUriStr.Length > 0 && this.ps.baseUri == null)
			{
				this.ps.baseUri = this.xmlResolver.ResolveUri(null, this.ps.baseUriStr);
			}
			Stream stream = null;
			Uri uri;
			if (publicId == null || publicId.Length == 0)
			{
				uri = this.xmlResolver.ResolveUri(this.ps.baseUri, systemId);
				try
				{
					stream = this.OpenStream(uri);
					goto IL_014A;
				}
				catch (Exception ex)
				{
					if (this.v1Compat)
					{
						throw;
					}
					this.Throw(new XmlException("Xml_ErrorOpeningExternalDtd", new string[]
					{
						uri.ToString(),
						ex.Message
					}, ex, 0, 0));
					goto IL_014A;
				}
			}
			try
			{
				uri = this.xmlResolver.ResolveUri(this.ps.baseUri, publicId);
				stream = this.OpenStream(uri);
			}
			catch (Exception)
			{
				uri = this.xmlResolver.ResolveUri(this.ps.baseUri, systemId);
				try
				{
					stream = this.OpenStream(uri);
				}
				catch (Exception ex2)
				{
					if (this.v1Compat)
					{
						throw;
					}
					this.Throw(new XmlException("Xml_ErrorOpeningExternalDtd", new string[]
					{
						uri.ToString(),
						ex2.Message
					}, ex2, 0, 0));
				}
			}
			IL_014A:
			if (stream == null)
			{
				this.ThrowWithoutLineInfo("Xml_CannotResolveExternalSubset", new string[]
				{
					(publicId != null) ? publicId : string.Empty,
					systemId
				});
			}
			this.PushParsingState();
			if (this.v1Compat)
			{
				this.InitStreamInput(uri, stream, null);
			}
			else
			{
				this.InitStreamInput(uri, stream, null);
			}
			this.ps.entity = null;
			this.ps.entityId = 0;
			int charPos = this.ps.charPos;
			if (this.v1Compat)
			{
				this.EatWhitespaces(null);
			}
			if (!this.ParseXmlDeclaration(true))
			{
				this.ps.charPos = charPos;
			}
			return true;
		}

		internal void DtdParserProxy_PushInternalDtd(string baseUri, string internalDtd)
		{
			this.PushParsingState();
			this.RegisterConsumedCharacters((long)internalDtd.Length, false);
			this.InitStringInput(baseUri, Encoding.Unicode, internalDtd);
			this.ps.entity = null;
			this.ps.entityId = 0;
			this.ps.eolNormalized = false;
		}

		internal void DtdParserProxy_Throw(Exception e)
		{
			this.Throw(e);
		}

		internal void DtdParserProxy_OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddAttributeNoChecks("SYSTEM", this.index);
			nodeData.SetValue(systemId);
			nodeData.lineInfo = keywordLineInfo;
			nodeData.lineInfo2 = systemLiteralLineInfo;
		}

		internal void DtdParserProxy_OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddAttributeNoChecks("PUBLIC", this.index);
			nodeData.SetValue(publicId);
			nodeData.lineInfo = keywordLineInfo;
			nodeData.lineInfo2 = publicLiteralLineInfo;
		}

		private void Throw(int pos, string res, string arg)
		{
			this.ps.charPos = pos;
			this.Throw(res, arg);
		}

		private void Throw(int pos, string res, string[] args)
		{
			this.ps.charPos = pos;
			this.Throw(res, args);
		}

		private void Throw(int pos, string res)
		{
			this.ps.charPos = pos;
			this.Throw(res, string.Empty);
		}

		private void Throw(string res)
		{
			this.Throw(res, string.Empty);
		}

		private void Throw(string res, int lineNo, int linePos)
		{
			this.Throw(new XmlException(res, string.Empty, lineNo, linePos, this.ps.baseUriStr));
		}

		private void Throw(string res, string arg)
		{
			this.Throw(new XmlException(res, arg, this.ps.LineNo, this.ps.LinePos, this.ps.baseUriStr));
		}

		private void Throw(string res, string arg, int lineNo, int linePos)
		{
			this.Throw(new XmlException(res, arg, lineNo, linePos, this.ps.baseUriStr));
		}

		private void Throw(string res, string[] args)
		{
			this.Throw(new XmlException(res, args, this.ps.LineNo, this.ps.LinePos, this.ps.baseUriStr));
		}

		private void Throw(Exception e)
		{
			this.SetErrorState();
			XmlException ex = e as XmlException;
			if (ex != null)
			{
				this.curNode.SetLineInfo(ex.LineNumber, ex.LinePosition);
			}
			throw e;
		}

		private void ReThrow(Exception e, int lineNo, int linePos)
		{
			this.Throw(new XmlException(e.Message, null, lineNo, linePos, this.ps.baseUriStr));
		}

		private void ThrowWithoutLineInfo(string res)
		{
			this.Throw(new XmlException(res, string.Empty, this.ps.baseUriStr));
		}

		private void ThrowWithoutLineInfo(string res, string arg)
		{
			this.Throw(new XmlException(res, arg, this.ps.baseUriStr));
		}

		private void ThrowWithoutLineInfo(string res, string[] args)
		{
			this.Throw(new XmlException(res, args, this.ps.baseUriStr));
		}

		private void ThrowInvalidChar(int pos, char invChar)
		{
			if (pos == 0 && this.curNode.type == XmlNodeType.None && this.ps.textReader != null && this.ps.charsUsed >= 2 && ((this.ps.chars[0] == '\u0001' && this.ps.chars[1] == '\u0004') || this.ps.chars[0] == 'ß' || this.ps.chars[1] == 'ÿ'))
			{
				this.Throw(pos, "Xml_BinaryXmlReadAsText", XmlException.BuildCharExceptionStr(invChar));
				return;
			}
			this.Throw(pos, "Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(invChar));
		}

		private void SetErrorState()
		{
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.Error;
			this.readState = ReadState.Error;
		}

		private void SendValidationEvent(XmlSeverityType severity, string code, string arg, int lineNo, int linePos)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.ps.baseUriStr, lineNo, linePos));
		}

		private void SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception)
		{
			if (this.validationEventHandler != null)
			{
				this.validationEventHandler(this, new ValidationEventArgs(exception, severity));
			}
		}

		private bool InAttributeValueIterator
		{
			get
			{
				return this.attrCount > 0 && this.parsingFunction >= XmlTextReaderImpl.ParsingFunction.InReadAttributeValue;
			}
		}

		private void FinishAttributeValueIterator()
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
			{
				this.FinishReadValueChunk();
			}
			else if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
			{
				this.FinishReadContentAsBinary();
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadAttributeValue)
			{
				while (this.ps.entityId != this.attributeValueBaseEntityId)
				{
					this.HandleEntityEnd(false);
				}
				this.parsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = ((this.index > 0) ? XmlTextReaderImpl.ParsingFunction.ElementContent : XmlTextReaderImpl.ParsingFunction.DocumentContent);
				this.emptyEntityInAttributeResolved = false;
			}
		}

		private bool DtdValidation
		{
			get
			{
				return this.validationEventHandler != null;
			}
		}

		private void InitStreamInput(Stream stream, Encoding encoding)
		{
			this.InitStreamInput(null, string.Empty, stream, null, 0, encoding);
		}

		private void InitStreamInput(string baseUriStr, Stream stream, Encoding encoding)
		{
			this.InitStreamInput(null, baseUriStr, stream, null, 0, encoding);
		}

		private void InitStreamInput(Uri baseUri, Stream stream, Encoding encoding)
		{
			this.InitStreamInput(baseUri, baseUri.ToString(), stream, null, 0, encoding);
		}

		private void InitStreamInput(Uri baseUri, string baseUriStr, Stream stream, Encoding encoding)
		{
			this.InitStreamInput(baseUri, baseUriStr, stream, null, 0, encoding);
		}

		private void InitStreamInput(Uri baseUri, string baseUriStr, Stream stream, byte[] bytes, int byteCount, Encoding encoding)
		{
			this.ps.stream = stream;
			this.ps.baseUri = baseUri;
			this.ps.baseUriStr = baseUriStr;
			int num;
			if (bytes != null)
			{
				this.ps.bytes = bytes;
				this.ps.bytesUsed = byteCount;
				num = this.ps.bytes.Length;
			}
			else
			{
				num = XmlReader.CalcBufferSize(stream);
				if (this.ps.bytes == null || this.ps.bytes.Length < num)
				{
					this.ps.bytes = new byte[num];
				}
			}
			if (this.ps.chars == null || this.ps.chars.Length < num + 1)
			{
				this.ps.chars = new char[num + 1];
			}
			this.ps.bytePos = 0;
			while (this.ps.bytesUsed < 4 && this.ps.bytes.Length - this.ps.bytesUsed > 0)
			{
				int num2 = stream.Read(this.ps.bytes, this.ps.bytesUsed, this.ps.bytes.Length - this.ps.bytesUsed);
				if (num2 == 0)
				{
					this.ps.isStreamEof = true;
					break;
				}
				this.ps.bytesUsed = this.ps.bytesUsed + num2;
			}
			if (encoding == null)
			{
				encoding = this.DetectEncoding();
			}
			this.SetupEncoding(encoding);
			byte[] preamble = this.ps.encoding.GetPreamble();
			int num3 = preamble.Length;
			int num4 = 0;
			while (num4 < num3 && num4 < this.ps.bytesUsed && this.ps.bytes[num4] == preamble[num4])
			{
				num4++;
			}
			if (num4 == num3)
			{
				this.ps.bytePos = num3;
			}
			this.documentStartBytePos = this.ps.bytePos;
			this.ps.eolNormalized = !this.normalize;
			this.ps.appendMode = true;
			this.ReadData();
		}

		private void InitTextReaderInput(string baseUriStr, TextReader input)
		{
			this.ps.textReader = input;
			this.ps.baseUriStr = baseUriStr;
			this.ps.baseUri = null;
			if (this.ps.chars == null)
			{
				this.ps.chars = new char[4097];
			}
			this.ps.encoding = Encoding.Unicode;
			this.ps.eolNormalized = !this.normalize;
			this.ps.appendMode = true;
			this.ReadData();
		}

		private void InitStringInput(string baseUriStr, Encoding originalEncoding, string str)
		{
			this.ps.baseUriStr = baseUriStr;
			this.ps.baseUri = null;
			int length = str.Length;
			this.ps.chars = new char[length + 1];
			str.CopyTo(0, this.ps.chars, 0, str.Length);
			this.ps.charsUsed = length;
			this.ps.chars[length] = '\0';
			this.ps.encoding = originalEncoding;
			this.ps.eolNormalized = !this.normalize;
			this.ps.isEof = true;
		}

		private void InitFragmentReader(XmlNodeType fragmentType, XmlParserContext parserContext, bool allowXmlDeclFragment)
		{
			this.fragmentParserContext = parserContext;
			if (parserContext != null)
			{
				if (parserContext.NamespaceManager != null)
				{
					this.namespaceManager = parserContext.NamespaceManager;
					this.xmlContext.defaultNamespace = this.namespaceManager.LookupNamespace(string.Empty);
				}
				else
				{
					this.namespaceManager = new XmlNamespaceManager(this.nameTable);
				}
				this.ps.baseUriStr = parserContext.BaseURI;
				this.ps.baseUri = null;
				this.xmlContext.xmlLang = parserContext.XmlLang;
				this.xmlContext.xmlSpace = parserContext.XmlSpace;
			}
			else
			{
				this.namespaceManager = new XmlNamespaceManager(this.nameTable);
				this.ps.baseUriStr = string.Empty;
				this.ps.baseUri = null;
			}
			this.reportedBaseUri = this.ps.baseUriStr;
			switch (fragmentType)
			{
			case XmlNodeType.Element:
				this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
				break;
			case XmlNodeType.Attribute:
				this.ps.appendMode = false;
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractive;
				this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.FragmentAttribute;
				break;
			default:
				if (fragmentType != XmlNodeType.Document)
				{
					if (fragmentType == XmlNodeType.XmlDeclaration)
					{
						if (allowXmlDeclFragment)
						{
							this.ps.appendMode = false;
							this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractive;
							this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.XmlDeclarationFragment;
							break;
						}
					}
					this.Throw("Xml_PartialContentNodeTypeNotSupportedEx", fragmentType.ToString());
					return;
				}
				break;
			}
			this.fragmentType = fragmentType;
			this.fragment = true;
		}

		private void OpenUrl()
		{
			XmlResolver xmlResolver;
			if (this.ps.baseUri != null)
			{
				xmlResolver = this.xmlResolver;
			}
			else
			{
				xmlResolver = ((this.xmlResolver == null) ? new XmlUrlResolver() : this.xmlResolver);
				this.ps.baseUri = xmlResolver.ResolveUri(null, this.url);
				this.ps.baseUriStr = this.ps.baseUri.ToString();
			}
			try
			{
				CompressedStack.Run(this.compressedStack, new ContextCallback(this.OpenUrlDelegate), xmlResolver);
			}
			catch
			{
				this.SetErrorState();
				throw;
			}
			if (this.ps.stream == null)
			{
				this.ThrowWithoutLineInfo("Xml_CannotResolveUrl", this.ps.baseUriStr);
			}
			this.InitStreamInput(this.ps.baseUri, this.ps.baseUriStr, this.ps.stream, null);
			this.reportedEncoding = this.ps.encoding;
		}

		private void OpenUrlDelegate(object xmlResolver)
		{
			this.ps.stream = (Stream)((XmlResolver)xmlResolver).GetEntity(this.ps.baseUri, null, typeof(Stream));
		}

		private Encoding DetectEncoding()
		{
			if (this.ps.bytesUsed < 2)
			{
				return null;
			}
			int num = ((int)this.ps.bytes[0] << 8) | (int)this.ps.bytes[1];
			int num2 = ((this.ps.bytesUsed >= 4) ? (((int)this.ps.bytes[2] << 8) | (int)this.ps.bytes[3]) : 0);
			int num3 = num;
			if (num3 <= 15360)
			{
				if (num3 != 0)
				{
					if (num3 != 60)
					{
						if (num3 == 15360)
						{
							int num4 = num2;
							if (num4 == 0)
							{
								return Ucs4Encoding.UCS4_Littleendian;
							}
							if (num4 == 16128)
							{
								return Encoding.Unicode;
							}
						}
					}
					else
					{
						int num5 = num2;
						if (num5 == 0)
						{
							return Ucs4Encoding.UCS4_3412;
						}
						if (num5 == 63)
						{
							return Encoding.BigEndianUnicode;
						}
					}
				}
				else
				{
					int num6 = num2;
					if (num6 <= 15360)
					{
						if (num6 == 60)
						{
							return Ucs4Encoding.UCS4_Bigendian;
						}
						if (num6 == 15360)
						{
							return Ucs4Encoding.UCS4_2143;
						}
					}
					else
					{
						if (num6 == 65279)
						{
							return Ucs4Encoding.UCS4_Bigendian;
						}
						if (num6 == 65534)
						{
							return Ucs4Encoding.UCS4_2143;
						}
					}
				}
			}
			else if (num3 <= 61371)
			{
				if (num3 != 19567)
				{
					if (num3 == 61371)
					{
						if ((num2 & 65280) == 48896)
						{
							return new UTF8Encoding(true, true);
						}
					}
				}
				else if (num2 == 42900)
				{
					this.Throw("Xml_UnknownEncoding", "ebcdic");
				}
			}
			else if (num3 != 65279)
			{
				if (num3 == 65534)
				{
					if (num2 == 0)
					{
						return Ucs4Encoding.UCS4_Littleendian;
					}
					return Encoding.Unicode;
				}
			}
			else
			{
				if (num2 == 0)
				{
					return Ucs4Encoding.UCS4_3412;
				}
				return Encoding.BigEndianUnicode;
			}
			return null;
		}

		private void SetupEncoding(Encoding encoding)
		{
			if (encoding == null)
			{
				this.ps.encoding = Encoding.UTF8;
				this.ps.decoder = new SafeAsciiDecoder();
				return;
			}
			this.ps.encoding = encoding;
			switch (this.ps.encoding.CodePage)
			{
			case 1200:
				this.ps.decoder = new UTF16Decoder(false);
				return;
			case 1201:
				this.ps.decoder = new UTF16Decoder(true);
				return;
			default:
				this.ps.decoder = encoding.GetDecoder();
				return;
			}
		}

		private void SwitchEncoding(Encoding newEncoding)
		{
			if ((newEncoding.CodePage != this.ps.encoding.CodePage || this.ps.decoder is SafeAsciiDecoder) && !this.afterResetState)
			{
				this.UnDecodeChars();
				this.ps.appendMode = false;
				this.SetupEncoding(newEncoding);
				this.ReadData();
			}
		}

		private Encoding CheckEncoding(string newEncodingName)
		{
			if (this.ps.stream == null)
			{
				return this.ps.encoding;
			}
			if (string.Compare(newEncodingName, "ucs-2", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "utf-16", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "iso-10646-ucs-2", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "ucs-4", StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (this.ps.encoding.CodePage != Encoding.BigEndianUnicode.CodePage && this.ps.encoding.CodePage != Encoding.Unicode.CodePage && string.Compare(newEncodingName, "ucs-4", StringComparison.OrdinalIgnoreCase) != 0)
				{
					if (this.afterResetState)
					{
						this.Throw("Xml_EncodingSwitchAfterResetState", newEncodingName);
					}
					else
					{
						this.ThrowWithoutLineInfo("Xml_MissingByteOrderMark");
					}
				}
				return this.ps.encoding;
			}
			Encoding encoding = null;
			if (string.Compare(newEncodingName, "utf-8", StringComparison.OrdinalIgnoreCase) == 0)
			{
				encoding = new UTF8Encoding(true, true);
			}
			else
			{
				try
				{
					encoding = Encoding.GetEncoding(newEncodingName);
					if (encoding.CodePage == -1)
					{
						this.Throw("Xml_UnknownEncoding", newEncodingName);
					}
				}
				catch (NotSupportedException)
				{
					this.Throw("Xml_UnknownEncoding", newEncodingName);
				}
				catch (ArgumentException)
				{
					this.Throw("Xml_UnknownEncoding", newEncodingName);
				}
			}
			if (this.afterResetState && this.ps.encoding.CodePage != encoding.CodePage)
			{
				this.Throw("Xml_EncodingSwitchAfterResetState", newEncodingName);
			}
			return encoding;
		}

		private void UnDecodeChars()
		{
			if (this.maxCharactersInDocument > 0L)
			{
				this.charactersInDocument -= (long)(this.ps.charsUsed - this.ps.charPos);
			}
			if (this.maxCharactersFromEntities > 0L && this.InEntity)
			{
				this.charactersFromEntities -= (long)(this.ps.charsUsed - this.ps.charPos);
			}
			this.ps.bytePos = this.documentStartBytePos;
			if (this.ps.charPos > 0)
			{
				this.ps.bytePos = this.ps.bytePos + this.ps.encoding.GetByteCount(this.ps.chars, 0, this.ps.charPos);
			}
			this.ps.charsUsed = this.ps.charPos;
			this.ps.isEof = false;
		}

		private void SwitchEncodingToUTF8()
		{
			this.SwitchEncoding(new UTF8Encoding(true, true));
		}

		private int ReadData()
		{
			if (this.ps.isEof)
			{
				return 0;
			}
			int num;
			if (this.ps.appendMode)
			{
				if (this.ps.charsUsed == this.ps.chars.Length - 1)
				{
					for (int i = 0; i < this.attrCount; i++)
					{
						this.nodes[this.index + i + 1].OnBufferInvalidated();
					}
					char[] array = new char[this.ps.chars.Length * 2];
					Buffer.BlockCopy(this.ps.chars, 0, array, 0, this.ps.chars.Length * 2);
					this.ps.chars = array;
				}
				if (this.ps.stream != null && this.ps.bytesUsed - this.ps.bytePos < 6 && this.ps.bytes.Length - this.ps.bytesUsed < 6)
				{
					byte[] array2 = new byte[this.ps.bytes.Length * 2];
					Buffer.BlockCopy(this.ps.bytes, 0, array2, 0, this.ps.bytesUsed);
					this.ps.bytes = array2;
				}
				num = this.ps.chars.Length - this.ps.charsUsed - 1;
				if (num > 80)
				{
					num = 80;
				}
			}
			else
			{
				int num2 = this.ps.chars.Length;
				if (num2 - this.ps.charsUsed <= num2 / 2)
				{
					for (int j = 0; j < this.attrCount; j++)
					{
						this.nodes[this.index + j + 1].OnBufferInvalidated();
					}
					int num3 = this.ps.charsUsed - this.ps.charPos;
					if (num3 < num2 - 1)
					{
						this.ps.lineStartPos = this.ps.lineStartPos - this.ps.charPos;
						if (num3 > 0)
						{
							Buffer.BlockCopy(this.ps.chars, this.ps.charPos * 2, this.ps.chars, 0, num3 * 2);
						}
						this.ps.charPos = 0;
						this.ps.charsUsed = num3;
					}
					else
					{
						char[] array3 = new char[this.ps.chars.Length * 2];
						Buffer.BlockCopy(this.ps.chars, 0, array3, 0, this.ps.chars.Length * 2);
						this.ps.chars = array3;
					}
				}
				if (this.ps.stream != null)
				{
					int num4 = this.ps.bytesUsed - this.ps.bytePos;
					if (num4 <= 128)
					{
						if (num4 == 0)
						{
							this.ps.bytesUsed = 0;
						}
						else
						{
							Buffer.BlockCopy(this.ps.bytes, this.ps.bytePos, this.ps.bytes, 0, num4);
							this.ps.bytesUsed = num4;
						}
						this.ps.bytePos = 0;
					}
				}
				num = this.ps.chars.Length - this.ps.charsUsed - 1;
			}
			if (this.ps.stream != null)
			{
				if (!this.ps.isStreamEof && this.ps.bytePos == this.ps.bytesUsed && this.ps.bytes.Length - this.ps.bytesUsed > 0)
				{
					int num5 = this.ps.stream.Read(this.ps.bytes, this.ps.bytesUsed, this.ps.bytes.Length - this.ps.bytesUsed);
					if (num5 == 0)
					{
						this.ps.isStreamEof = true;
					}
					this.ps.bytesUsed = this.ps.bytesUsed + num5;
				}
				int bytePos = this.ps.bytePos;
				num = this.GetChars(num);
				if (num == 0 && this.ps.bytePos != bytePos)
				{
					return this.ReadData();
				}
			}
			else if (this.ps.textReader != null)
			{
				num = this.ps.textReader.Read(this.ps.chars, this.ps.charsUsed, this.ps.chars.Length - this.ps.charsUsed - 1);
				this.ps.charsUsed = this.ps.charsUsed + num;
			}
			else
			{
				num = 0;
			}
			this.RegisterConsumedCharacters((long)num, this.InEntity);
			if (num == 0)
			{
				this.ps.isEof = true;
			}
			this.ps.chars[this.ps.charsUsed] = '\0';
			return num;
		}

		private int GetChars(int maxCharsCount)
		{
			int num = this.ps.bytesUsed - this.ps.bytePos;
			if (num == 0)
			{
				return 0;
			}
			int num2;
			try
			{
				bool flag;
				this.ps.decoder.Convert(this.ps.bytes, this.ps.bytePos, num, this.ps.chars, this.ps.charsUsed, maxCharsCount, false, out num, out num2, out flag);
			}
			catch (ArgumentException)
			{
				this.InvalidCharRecovery(ref num, out num2);
			}
			this.ps.bytePos = this.ps.bytePos + num;
			this.ps.charsUsed = this.ps.charsUsed + num2;
			return num2;
		}

		private void InvalidCharRecovery(ref int bytesCount, out int charsCount)
		{
			int num = 0;
			int i = 0;
			try
			{
				while (i < bytesCount)
				{
					int num2;
					int num3;
					bool flag;
					this.ps.decoder.Convert(this.ps.bytes, this.ps.bytePos + i, 1, this.ps.chars, this.ps.charsUsed + num, 1, false, out num2, out num3, out flag);
					num += num3;
					i += num2;
				}
			}
			catch (ArgumentException)
			{
			}
			if (num == 0)
			{
				this.Throw(this.ps.charsUsed, "Xml_InvalidCharInThisEncoding");
			}
			charsCount = num;
			bytesCount = i;
		}

		internal void Close(bool closeInput)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.ReaderClosed)
			{
				return;
			}
			while (this.InEntity)
			{
				this.PopParsingState();
			}
			this.ps.Close(closeInput);
			this.curNode = XmlTextReaderImpl.NodeData.None;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ReaderClosed;
			this.reportedEncoding = null;
			this.reportedBaseUri = string.Empty;
			this.readState = ReadState.Closed;
			this.fullAttrCleanup = false;
			this.ResetAttributes();
		}

		private void ShiftBuffer(int sourcePos, int destPos, int count)
		{
			Buffer.BlockCopy(this.ps.chars, sourcePos * 2, this.ps.chars, destPos * 2, count * 2);
		}

		private unsafe bool ParseXmlDeclaration(bool isTextDecl)
		{
			while (this.ps.charsUsed - this.ps.charPos < 6)
			{
				if (this.ReadData() == 0)
				{
					IL_07EC:
					if (!isTextDecl)
					{
						this.parsingFunction = this.nextParsingFunction;
					}
					if (this.afterResetState)
					{
						int codePage = this.ps.encoding.CodePage;
						if (codePage != Encoding.UTF8.CodePage && codePage != Encoding.Unicode.CodePage && codePage != Encoding.BigEndianUnicode.CodePage && !(this.ps.encoding is Ucs4Encoding))
						{
							this.Throw("Xml_EncodingSwitchAfterResetState", (this.ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
						}
					}
					if (this.ps.decoder is SafeAsciiDecoder)
					{
						this.SwitchEncodingToUTF8();
					}
					this.ps.appendMode = false;
					return false;
				}
			}
			if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, 5, "<?xml") && !this.xmlCharType.IsNameChar(this.ps.chars[this.ps.charPos + 5]))
			{
				if (!isTextDecl)
				{
					this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos + 2);
					this.curNode.SetNamedNode(XmlNodeType.XmlDeclaration, this.Xml);
				}
				this.ps.charPos = this.ps.charPos + 5;
				BufferBuilder bufferBuilder = (isTextDecl ? new BufferBuilder() : this.stringBuilder);
				int num = 0;
				Encoding encoding = null;
				for (;;)
				{
					int length = bufferBuilder.Length;
					int num2 = this.EatWhitespaces((num == 0) ? null : bufferBuilder);
					if (this.ps.chars[this.ps.charPos] == '?')
					{
						bufferBuilder.Length = length;
						if (this.ps.chars[this.ps.charPos + 1] == '>')
						{
							break;
						}
						if (this.ps.charPos + 1 == this.ps.charsUsed)
						{
							goto IL_07C4;
						}
						this.ThrowUnexpectedToken("'>'");
					}
					if (num2 == 0 && num != 0)
					{
						this.ThrowUnexpectedToken("?>");
					}
					int num3 = this.ParseName();
					XmlTextReaderImpl.NodeData nodeData = null;
					char c = this.ps.chars[this.ps.charPos];
					if (c != 'e')
					{
						if (c != 's')
						{
							if (c != 'v' || !XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos, "version") || num != 0)
							{
								goto IL_03BB;
							}
							if (!isTextDecl)
							{
								nodeData = this.AddAttributeNoChecks("version", 0);
							}
						}
						else
						{
							if (!XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos, "standalone") || (num != 1 && num != 2) || isTextDecl)
							{
								goto IL_03BB;
							}
							if (!isTextDecl)
							{
								nodeData = this.AddAttributeNoChecks("standalone", 0);
							}
							num = 2;
						}
					}
					else
					{
						if (!XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos, "encoding") || (num != 1 && (!isTextDecl || num != 0)))
						{
							goto IL_03BB;
						}
						if (!isTextDecl)
						{
							nodeData = this.AddAttributeNoChecks("encoding", 0);
						}
						num = 1;
					}
					IL_03D0:
					if (!isTextDecl)
					{
						nodeData.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
					}
					bufferBuilder.Append(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos);
					this.ps.charPos = num3;
					if (this.ps.chars[this.ps.charPos] != '=')
					{
						this.EatWhitespaces(bufferBuilder);
						if (this.ps.chars[this.ps.charPos] != '=')
						{
							this.ThrowUnexpectedToken("=");
						}
					}
					bufferBuilder.Append('=');
					this.ps.charPos = this.ps.charPos + 1;
					char c2 = this.ps.chars[this.ps.charPos];
					if (c2 != '"' && c2 != '\'')
					{
						this.EatWhitespaces(bufferBuilder);
						c2 = this.ps.chars[this.ps.charPos];
						if (c2 != '"' && c2 != '\'')
						{
							this.ThrowUnexpectedToken("\"", "'");
						}
					}
					bufferBuilder.Append(c2);
					this.ps.charPos = this.ps.charPos + 1;
					if (!isTextDecl)
					{
						nodeData.quoteChar = c2;
						nodeData.SetLineInfo2(this.ps.LineNo, this.ps.LinePos);
					}
					int num4 = this.ps.charPos;
					char[] chars;
					for (;;)
					{
						chars = this.ps.chars;
						while ((this.xmlCharType.charProperties[chars[num4]] & 128) != 0)
						{
							num4++;
						}
						if (this.ps.chars[num4] == c2)
						{
							break;
						}
						if (num4 != this.ps.charsUsed)
						{
							goto IL_07AF;
						}
						if (this.ReadData() == 0)
						{
							goto Block_57;
						}
					}
					switch (num)
					{
					case 0:
						if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos, "1.0"))
						{
							if (!isTextDecl)
							{
								nodeData.SetValue(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
							}
							num = 1;
						}
						else
						{
							string text = new string(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
							this.Throw("Xml_InvalidVersionNumber", text);
						}
						break;
					case 1:
					{
						string text2 = new string(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
						encoding = this.CheckEncoding(text2);
						if (!isTextDecl)
						{
							nodeData.SetValue(text2);
						}
						num = 2;
						break;
					}
					case 2:
						if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos, "yes"))
						{
							this.standalone = true;
						}
						else if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos, "no"))
						{
							this.standalone = false;
						}
						else
						{
							this.Throw("Xml_InvalidXmlDecl", this.ps.LineNo, this.ps.LinePos - 1);
						}
						if (!isTextDecl)
						{
							nodeData.SetValue(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
						}
						num = 3;
						break;
					}
					bufferBuilder.Append(chars, this.ps.charPos, num4 - this.ps.charPos);
					bufferBuilder.Append(c2);
					this.ps.charPos = num4 + 1;
					continue;
					Block_57:
					this.Throw("Xml_UnclosedQuote");
					goto IL_07C4;
					IL_07AF:
					this.Throw(isTextDecl ? "Xml_InvalidTextDecl" : "Xml_InvalidXmlDecl");
					goto IL_07C4;
					IL_03BB:
					this.Throw(isTextDecl ? "Xml_InvalidTextDecl" : "Xml_InvalidXmlDecl");
					goto IL_03D0;
					IL_07C4:
					if (this.ps.isEof || this.ReadData() == 0)
					{
						this.Throw("Xml_UnexpectedEOF1");
					}
				}
				if (num == 0)
				{
					this.Throw(isTextDecl ? "Xml_InvalidTextDecl" : "Xml_InvalidXmlDecl");
				}
				this.ps.charPos = this.ps.charPos + 2;
				if (!isTextDecl)
				{
					this.curNode.SetValue(bufferBuilder.ToString());
					bufferBuilder.Length = 0;
					this.nextParsingFunction = this.parsingFunction;
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ResetAttributesRootLevel;
				}
				if (encoding == null)
				{
					if (isTextDecl)
					{
						this.Throw("Xml_InvalidTextDecl");
					}
					if (this.afterResetState)
					{
						int codePage2 = this.ps.encoding.CodePage;
						if (codePage2 != Encoding.UTF8.CodePage && codePage2 != Encoding.Unicode.CodePage && codePage2 != Encoding.BigEndianUnicode.CodePage && !(this.ps.encoding is Ucs4Encoding))
						{
							this.Throw("Xml_EncodingSwitchAfterResetState", (this.ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
						}
					}
					if (this.ps.decoder is SafeAsciiDecoder)
					{
						this.SwitchEncodingToUTF8();
					}
				}
				else
				{
					this.SwitchEncoding(encoding);
				}
				this.ps.appendMode = false;
				return true;
			}
			goto IL_07EC;
		}

		private bool ParseDocumentContent()
		{
			int num;
			for (;;)
			{
				bool flag = false;
				num = this.ps.charPos;
				char[] array = this.ps.chars;
				if (array[num] == '<')
				{
					flag = true;
					if (this.ps.charsUsed - num >= 4)
					{
						num++;
						char c = array[num];
						if (c != '!')
						{
							if (c != '/')
							{
								if (c != '?')
								{
									goto IL_01CC;
								}
								this.ps.charPos = num + 1;
								if (this.ParsePI())
								{
									break;
								}
								continue;
							}
							else
							{
								this.Throw(num + 1, "Xml_UnexpectedEndTag");
							}
						}
						else
						{
							num++;
							if (this.ps.charsUsed - num >= 2)
							{
								if (array[num] == '-')
								{
									if (array[num + 1] == '-')
									{
										this.ps.charPos = num + 2;
										if (this.ParseComment())
										{
											return true;
										}
										continue;
									}
									else
									{
										this.ThrowUnexpectedToken(num + 1, "-");
									}
								}
								else if (array[num] == '[')
								{
									if (this.fragmentType != XmlNodeType.Document)
									{
										num++;
										if (this.ps.charsUsed - num >= 6)
										{
											if (XmlConvert.StrEqual(array, num, 6, "CDATA["))
											{
												goto Block_13;
											}
											this.ThrowUnexpectedToken(num, "CDATA[");
										}
									}
									else
									{
										this.Throw(this.ps.charPos, "Xml_InvalidRootData");
									}
								}
								else
								{
									if (this.fragmentType == XmlNodeType.Document || this.fragmentType == XmlNodeType.None)
									{
										goto IL_0164;
									}
									if (this.ParseUnexpectedToken(num) == "DOCTYPE")
									{
										this.Throw("Xml_BadDTDLocation");
									}
									else
									{
										this.ThrowUnexpectedToken(num, "<!--", "<[CDATA[");
									}
								}
							}
						}
					}
				}
				else if (array[num] == '&')
				{
					if (this.fragmentType != XmlNodeType.Document)
					{
						if (this.fragmentType == XmlNodeType.None)
						{
							this.fragmentType = XmlNodeType.Element;
						}
						int num2;
						switch (this.HandleEntityReference(false, XmlTextReaderImpl.EntityExpandType.OnlyGeneral, out num2))
						{
						case XmlTextReaderImpl.EntityType.CharacterDec:
						case XmlTextReaderImpl.EntityType.CharacterHex:
						case XmlTextReaderImpl.EntityType.CharacterNamed:
							if (this.ParseText())
							{
								return true;
							}
							continue;
						case XmlTextReaderImpl.EntityType.Unexpanded:
							goto IL_0279;
						}
						array = this.ps.chars;
						num = this.ps.charPos;
						continue;
					}
					this.Throw(num, "Xml_InvalidRootData");
				}
				else if (num != this.ps.charsUsed && (!this.v1Compat || array[num] != '\0'))
				{
					if (this.fragmentType == XmlNodeType.Document)
					{
						if (this.ParseRootLevelWhitespace())
						{
							return true;
						}
					}
					else if (this.ParseText())
					{
						goto Block_30;
					}
					array = this.ps.chars;
					num = this.ps.charPos;
					continue;
				}
				if (this.ReadData() != 0)
				{
					num = this.ps.charPos;
					num = this.ps.charPos;
					array = this.ps.chars;
				}
				else
				{
					if (flag)
					{
						this.Throw("Xml_InvalidRootData");
					}
					if (!this.InEntity)
					{
						goto IL_0374;
					}
					if (this.HandleEntityEnd(true))
					{
						goto Block_36;
					}
				}
			}
			return true;
			Block_13:
			this.ps.charPos = num + 6;
			this.ParseCData();
			if (this.fragmentType == XmlNodeType.None)
			{
				this.fragmentType = XmlNodeType.Element;
			}
			return true;
			IL_0164:
			this.fragmentType = XmlNodeType.Document;
			this.ps.charPos = num;
			this.ParseDoctypeDecl();
			return true;
			IL_01CC:
			if (this.rootElementParsed)
			{
				if (this.fragmentType == XmlNodeType.Document)
				{
					this.Throw(num, "Xml_MultipleRoots");
				}
				if (this.fragmentType == XmlNodeType.None)
				{
					this.fragmentType = XmlNodeType.Element;
				}
			}
			this.ps.charPos = num;
			this.rootElementParsed = true;
			this.ParseElement();
			return true;
			IL_0279:
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.EntityReference)
			{
				this.parsingFunction = this.nextParsingFunction;
			}
			this.ParseEntityReference();
			return true;
			Block_30:
			if (this.fragmentType == XmlNodeType.None && this.curNode.type == XmlNodeType.Text)
			{
				this.fragmentType = XmlNodeType.Element;
			}
			return true;
			Block_36:
			this.SetupEndEntityNodeInContent();
			return true;
			IL_0374:
			if (!this.rootElementParsed && this.fragmentType == XmlNodeType.Document)
			{
				this.ThrowWithoutLineInfo("Xml_MissingRoot");
			}
			if (this.fragmentType == XmlNodeType.None)
			{
				this.fragmentType = (this.rootElementParsed ? XmlNodeType.Document : XmlNodeType.Element);
			}
			this.OnEof();
			return false;
		}

		private bool ParseElementContent()
		{
			int num;
			for (;;)
			{
				num = this.ps.charPos;
				char[] chars = this.ps.chars;
				char c = chars[num];
				if (c != '&')
				{
					if (c == '<')
					{
						char c2 = chars[num + 1];
						if (c2 != '!')
						{
							if (c2 == '/')
							{
								goto IL_013B;
							}
							if (c2 == '?')
							{
								this.ps.charPos = num + 2;
								if (this.ParsePI())
								{
									break;
								}
								continue;
							}
							else if (num + 1 != this.ps.charsUsed)
							{
								goto Block_14;
							}
						}
						else
						{
							num += 2;
							if (this.ps.charsUsed - num >= 2)
							{
								if (chars[num] == '-')
								{
									if (chars[num + 1] == '-')
									{
										this.ps.charPos = num + 2;
										if (this.ParseComment())
										{
											return true;
										}
										continue;
									}
									else
									{
										this.ThrowUnexpectedToken(num + 1, "-");
									}
								}
								else if (chars[num] == '[')
								{
									num++;
									if (this.ps.charsUsed - num >= 6)
									{
										if (XmlConvert.StrEqual(chars, num, 6, "CDATA["))
										{
											goto Block_12;
										}
										this.ThrowUnexpectedToken(num, "CDATA[");
									}
								}
								else if (this.ParseUnexpectedToken(num) == "DOCTYPE")
								{
									this.Throw("Xml_BadDTDLocation");
								}
								else
								{
									this.ThrowUnexpectedToken(num, "<!--", "<[CDATA[");
								}
							}
						}
					}
					else if (num != this.ps.charsUsed)
					{
						if (this.ParseText())
						{
							return true;
						}
						continue;
					}
					if (this.ReadData() == 0)
					{
						if (this.ps.charsUsed - this.ps.charPos != 0)
						{
							this.ThrowUnclosedElements();
						}
						if (!this.InEntity)
						{
							if (this.index == 0 && this.fragmentType != XmlNodeType.Document)
							{
								goto Block_22;
							}
							this.ThrowUnclosedElements();
						}
						if (this.HandleEntityEnd(true))
						{
							goto Block_23;
						}
					}
				}
				else if (this.ParseText())
				{
					return true;
				}
			}
			return true;
			Block_12:
			this.ps.charPos = num + 6;
			this.ParseCData();
			return true;
			IL_013B:
			this.ps.charPos = num + 2;
			this.ParseEndElement();
			return true;
			Block_14:
			this.ps.charPos = num + 1;
			this.ParseElement();
			return true;
			Block_22:
			this.OnEof();
			return false;
			Block_23:
			this.SetupEndEntityNodeInContent();
			return true;
		}

		private void ThrowUnclosedElements()
		{
			if (this.index == 0 && this.curNode.type != XmlNodeType.Element)
			{
				this.Throw(this.ps.charsUsed, "Xml_UnexpectedEOF1");
				return;
			}
			int i = ((this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead) ? this.index : (this.index - 1));
			this.stringBuilder.Length = 0;
			while (i >= 0)
			{
				XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
				if (nodeData.type == XmlNodeType.Element)
				{
					this.stringBuilder.Append(nodeData.GetNameWPrefix(this.nameTable));
					if (i > 0)
					{
						this.stringBuilder.Append(", ");
					}
					else
					{
						this.stringBuilder.Append(".");
					}
				}
				i--;
			}
			this.Throw(this.ps.charsUsed, "Xml_UnexpectedEOFInElementContent", this.stringBuilder.ToString());
		}

		private unsafe void ParseElement()
		{
			int num = this.ps.charPos;
			char[] array = this.ps.chars;
			int num2 = -1;
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			while ((this.xmlCharType.charProperties[array[num]] & 4) != 0)
			{
				num++;
				for (;;)
				{
					if ((this.xmlCharType.charProperties[array[num]] & 8) == 0)
					{
						if (array[num] != ':')
						{
							goto IL_00A3;
						}
						if (num2 == -1)
						{
							break;
						}
						if (this.supportNamespaces)
						{
							goto Block_5;
						}
						num++;
					}
					else
					{
						num++;
					}
				}
				num2 = num;
				num++;
				continue;
				Block_5:
				this.Throw(num, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(':'));
				break;
				IL_00A3:
				if (num >= this.ps.charsUsed)
				{
					break;
				}
				IL_00C6:
				this.namespaceManager.PushScope();
				if (num2 == -1 || !this.supportNamespaces)
				{
					this.curNode.SetNamedNode(XmlNodeType.Element, this.nameTable.Add(array, this.ps.charPos, num - this.ps.charPos));
				}
				else
				{
					int charPos = this.ps.charPos;
					int num3 = num2 - charPos;
					if (num3 == this.lastPrefix.Length && XmlConvert.StrEqual(array, charPos, num3, this.lastPrefix))
					{
						this.curNode.SetNamedNode(XmlNodeType.Element, this.nameTable.Add(array, num2 + 1, num - num2 - 1), this.lastPrefix, null);
					}
					else
					{
						this.curNode.SetNamedNode(XmlNodeType.Element, this.nameTable.Add(array, num2 + 1, num - num2 - 1), this.nameTable.Add(array, this.ps.charPos, num3), null);
						this.lastPrefix = this.curNode.prefix;
					}
				}
				char c = array[num];
				bool flag = (this.xmlCharType.charProperties[c] & 1) != 0;
				if (flag)
				{
					this.ps.charPos = num;
					this.ParseAttributes();
					return;
				}
				if (c == '>')
				{
					this.ps.charPos = num + 1;
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.MoveToElementContent;
				}
				else if (c == '/')
				{
					if (num + 1 == this.ps.charsUsed)
					{
						this.ps.charPos = num;
						if (this.ReadData() == 0)
						{
							this.Throw(num, "Xml_UnexpectedEOF", ">");
						}
						num = this.ps.charPos;
						array = this.ps.chars;
					}
					if (array[num + 1] == '>')
					{
						this.curNode.IsEmptyElement = true;
						this.nextParsingFunction = this.parsingFunction;
						this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopEmptyElementContext;
						this.ps.charPos = num + 2;
					}
					else
					{
						this.ThrowUnexpectedToken(num, ">");
					}
				}
				else
				{
					this.Throw(num, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(c));
				}
				if (this.addDefaultAttributesAndNormalize)
				{
					this.AddDefaultAttributesAndNormalize();
				}
				this.ElementNamespaceLookup();
				return;
			}
			num = this.ParseQName(out num2);
			array = this.ps.chars;
			goto IL_00C6;
		}

		private void AddDefaultAttributesAndNormalize()
		{
			this.qName.Init(this.curNode.localName, this.curNode.prefix);
			SchemaInfo dtdSchemaInfo = this.dtdParserProxy.DtdSchemaInfo;
			SchemaElementDecl schemaElementDecl;
			if ((schemaElementDecl = dtdSchemaInfo.GetElementDecl(this.qName)) == null && (schemaElementDecl = (SchemaElementDecl)dtdSchemaInfo.UndeclaredElementDecls[this.qName]) == null)
			{
				return;
			}
			if (this.normalize && schemaElementDecl.HasNonCDataAttribute)
			{
				for (int i = this.index + 1; i < this.index + 1 + this.attrCount; i++)
				{
					XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
					this.qName.Init(nodeData.localName, nodeData.prefix);
					SchemaAttDef attDef = schemaElementDecl.GetAttDef(this.qName);
					if (attDef != null && attDef.SchemaType.Datatype.TokenizedType != XmlTokenizedType.CDATA)
					{
						if (this.DtdValidation && this.standalone && attDef.IsDeclaredInExternal)
						{
							string stringValue = nodeData.StringValue;
							nodeData.TrimSpacesInValue();
							if (stringValue != nodeData.StringValue)
							{
								this.SendValidationEvent(XmlSeverityType.Error, "Sch_StandAloneNormalization", nodeData.GetNameWPrefix(this.nameTable), nodeData.LineNo, nodeData.LinePos);
							}
						}
						else
						{
							nodeData.TrimSpacesInValue();
						}
					}
				}
			}
			SchemaAttDef[] defaultAttDefs = schemaElementDecl.DefaultAttDefs;
			if (defaultAttDefs != null)
			{
				int num = this.attrCount;
				XmlTextReaderImpl.NodeData[] array = null;
				if (this.attrCount >= 250)
				{
					array = new XmlTextReaderImpl.NodeData[this.attrCount];
					Array.Copy(this.nodes, this.index + 1, array, 0, this.attrCount);
					Array.Sort(array, XmlTextReaderImpl.SchemaAttDefToNodeDataComparer.Instance);
				}
				foreach (SchemaAttDef schemaAttDef in defaultAttDefs)
				{
					if (this.AddDefaultAttribute(schemaAttDef, true, array) && this.DtdValidation && this.standalone && schemaAttDef.IsDeclaredInExternal)
					{
						this.SendValidationEvent(XmlSeverityType.Error, "Sch_UnSpecifiedDefaultAttributeInExternalStandalone", schemaAttDef.Name.Name, this.curNode.LineNo, this.curNode.LinePos);
					}
				}
				if (num == 0 && this.attrNeedNamespaceLookup)
				{
					this.AttributeNamespaceLookup();
					this.attrNeedNamespaceLookup = false;
				}
			}
		}

		private unsafe void ParseEndElement()
		{
			XmlTextReaderImpl.NodeData nodeData = this.nodes[this.index - 1];
			int length = nodeData.prefix.Length;
			int length2 = nodeData.localName.Length;
			while (this.ps.charsUsed - this.ps.charPos < length + length2 + 1 && this.ReadData() != 0)
			{
			}
			char[] array = this.ps.chars;
			int num;
			if (nodeData.prefix.Length == 0)
			{
				if (!XmlConvert.StrEqual(array, this.ps.charPos, length2, nodeData.localName))
				{
					this.ThrowTagMismatch(nodeData);
				}
				num = length2;
			}
			else
			{
				int num2 = this.ps.charPos + length;
				if (!XmlConvert.StrEqual(array, this.ps.charPos, length, nodeData.prefix) || array[num2] != ':' || !XmlConvert.StrEqual(array, num2 + 1, length2, nodeData.localName))
				{
					this.ThrowTagMismatch(nodeData);
				}
				num = length2 + length + 1;
			}
			int num3;
			for (;;)
			{
				num3 = this.ps.charPos + num;
				array = this.ps.chars;
				if (num3 != this.ps.charsUsed)
				{
					if ((this.xmlCharType.charProperties[array[num3]] & 8) != 0 || array[num3] == ':')
					{
						this.ThrowTagMismatch(nodeData);
					}
					while ((this.xmlCharType.charProperties[array[num3]] & 1) != 0)
					{
						num3++;
					}
					if (array[num3] == '>')
					{
						break;
					}
					if (num3 != this.ps.charsUsed)
					{
						this.ThrowUnexpectedToken(num3, ">");
					}
				}
				if (this.ReadData() == 0)
				{
					this.ThrowUnclosedElements();
				}
			}
			this.index--;
			this.curNode = this.nodes[this.index];
			nodeData.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			nodeData.type = XmlNodeType.EndElement;
			this.ps.charPos = num3 + 1;
			this.nextParsingFunction = ((this.index > 0) ? this.parsingFunction : XmlTextReaderImpl.ParsingFunction.DocumentContent);
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopElementContext;
		}

		private void ThrowTagMismatch(XmlTextReaderImpl.NodeData startTag)
		{
			if (startTag.type == XmlNodeType.Element)
			{
				int num2;
				int num = this.ParseQName(out num2);
				this.Throw("Xml_TagMismatch", new string[]
				{
					startTag.GetNameWPrefix(this.nameTable),
					startTag.lineInfo.lineNo.ToString(CultureInfo.InvariantCulture),
					new string(this.ps.chars, this.ps.charPos, num - this.ps.charPos)
				});
				return;
			}
			this.Throw("Xml_UnexpectedEndTag");
		}

		private unsafe void ParseAttributes()
		{
			int num = this.ps.charPos;
			char[] array = this.ps.chars;
			for (;;)
			{
				IL_001A:
				int num2 = 0;
				char c;
				while ((this.xmlCharType.charProperties[c = array[num]] & 1) != 0)
				{
					if (c == '\n')
					{
						this.OnNewLine(num + 1);
						num2++;
					}
					else if (c == '\r')
					{
						if (array[num + 1] == '\n')
						{
							this.OnNewLine(num + 2);
							num2++;
							num++;
						}
						else if (num + 1 != this.ps.charsUsed)
						{
							this.OnNewLine(num + 1);
							num2++;
						}
						else
						{
							this.ps.charPos = num;
							IL_042C:
							this.ps.lineNo = this.ps.lineNo - num2;
							if (this.ReadData() != 0)
							{
								num = this.ps.charPos;
								array = this.ps.chars;
								goto IL_001A;
							}
							this.ThrowUnclosedElements();
							goto IL_001A;
						}
					}
					num++;
				}
				char c2;
				if ((this.xmlCharType.charProperties[c2 = array[num]] & 4) == 0)
				{
					if (c2 == '>')
					{
						break;
					}
					if (c2 == '/')
					{
						if (num + 1 == this.ps.charsUsed)
						{
							goto IL_042C;
						}
						if (array[num + 1] == '>')
						{
							goto Block_10;
						}
						this.ThrowUnexpectedToken(num + 1, ">");
					}
					else
					{
						if (num == this.ps.charsUsed)
						{
							goto IL_042C;
						}
						if (c2 != ':' || this.supportNamespaces)
						{
							this.Throw(num, "Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(c2));
						}
					}
				}
				if (num == this.ps.charPos)
				{
					this.Throw("Xml_ExpectingWhiteSpace", this.ParseUnexpectedToken());
				}
				this.ps.charPos = num;
				int linePos = this.ps.LinePos;
				int num3 = -1;
				num++;
				for (;;)
				{
					char c3;
					if ((this.xmlCharType.charProperties[c3 = array[num]] & 8) == 0)
					{
						if (c3 != ':')
						{
							goto IL_023F;
						}
						if (num3 != -1)
						{
							if (this.supportNamespaces)
							{
								goto Block_17;
							}
							num++;
						}
						else
						{
							num3 = num;
							num++;
							if ((this.xmlCharType.charProperties[array[num]] & 4) == 0)
							{
								goto IL_0228;
							}
							num++;
						}
					}
					else
					{
						num++;
					}
				}
				IL_0262:
				XmlTextReaderImpl.NodeData nodeData = this.AddAttribute(num, num3);
				nodeData.SetLineInfo(this.ps.LineNo, linePos);
				if (array[num] != '=')
				{
					this.ps.charPos = num;
					this.EatWhitespaces(null);
					num = this.ps.charPos;
					if (array[num] != '=')
					{
						this.ThrowUnexpectedToken("=");
					}
				}
				num++;
				char c4 = array[num];
				if (c4 != '"' && c4 != '\'')
				{
					this.ps.charPos = num;
					this.EatWhitespaces(null);
					num = this.ps.charPos;
					c4 = array[num];
					if (c4 != '"' && c4 != '\'')
					{
						this.ThrowUnexpectedToken("\"", "'");
					}
				}
				num++;
				this.ps.charPos = num;
				nodeData.quoteChar = c4;
				nodeData.SetLineInfo2(this.ps.LineNo, this.ps.LinePos);
				char c5;
				while ((this.xmlCharType.charProperties[c5 = array[num]] & 128) != 0)
				{
					num++;
				}
				if (c5 == c4)
				{
					nodeData.SetValue(array, this.ps.charPos, num - this.ps.charPos);
					num++;
					this.ps.charPos = num;
				}
				else
				{
					this.ParseAttributeValueSlow(num, c4, nodeData);
					num = this.ps.charPos;
					array = this.ps.chars;
				}
				if (nodeData.prefix.Length == 0)
				{
					if (Ref.Equal(nodeData.localName, this.XmlNs))
					{
						this.OnDefaultNamespaceDecl(nodeData);
						continue;
					}
					continue;
				}
				else
				{
					if (Ref.Equal(nodeData.prefix, this.XmlNs))
					{
						this.OnNamespaceDecl(nodeData);
						continue;
					}
					if (Ref.Equal(nodeData.prefix, this.Xml))
					{
						this.OnXmlReservedAttribute(nodeData);
						continue;
					}
					continue;
				}
				Block_17:
				this.Throw(num, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(':'));
				goto IL_0262;
				IL_0228:
				num = this.ParseQName(out num3);
				array = this.ps.chars;
				goto IL_0262;
				IL_023F:
				if (num == this.ps.charsUsed)
				{
					num = this.ParseQName(out num3);
					array = this.ps.chars;
					goto IL_0262;
				}
				goto IL_0262;
			}
			this.ps.charPos = num + 1;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.MoveToElementContent;
			goto IL_046F;
			Block_10:
			this.ps.charPos = num + 2;
			this.curNode.IsEmptyElement = true;
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopEmptyElementContext;
			IL_046F:
			if (this.addDefaultAttributesAndNormalize)
			{
				this.AddDefaultAttributesAndNormalize();
			}
			this.ElementNamespaceLookup();
			if (this.attrNeedNamespaceLookup)
			{
				this.AttributeNamespaceLookup();
				this.attrNeedNamespaceLookup = false;
			}
			if (this.attrDuplWalkCount >= 250)
			{
				this.AttributeDuplCheck();
			}
		}

		private void ElementNamespaceLookup()
		{
			if (this.curNode.prefix.Length == 0)
			{
				this.curNode.ns = this.xmlContext.defaultNamespace;
				return;
			}
			this.curNode.ns = this.LookupNamespace(this.curNode);
		}

		private void AttributeNamespaceLookup()
		{
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
				if (nodeData.type == XmlNodeType.Attribute && nodeData.prefix.Length > 0)
				{
					nodeData.ns = this.LookupNamespace(nodeData);
				}
			}
		}

		private void AttributeDuplCheck()
		{
			if (this.attrCount < 250)
			{
				for (int i = this.index + 1; i < this.index + 1 + this.attrCount; i++)
				{
					XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
					for (int j = i + 1; j < this.index + 1 + this.attrCount; j++)
					{
						if (Ref.Equal(nodeData.localName, this.nodes[j].localName) && Ref.Equal(nodeData.ns, this.nodes[j].ns))
						{
							this.Throw("Xml_DupAttributeName", this.nodes[j].GetNameWPrefix(this.nameTable), this.nodes[j].LineNo, this.nodes[j].LinePos);
						}
					}
				}
				return;
			}
			if (this.attrDuplSortingArray == null || this.attrDuplSortingArray.Length < this.attrCount)
			{
				this.attrDuplSortingArray = new XmlTextReaderImpl.NodeData[this.attrCount];
			}
			Array.Copy(this.nodes, this.index + 1, this.attrDuplSortingArray, 0, this.attrCount);
			Array.Sort<XmlTextReaderImpl.NodeData>(this.attrDuplSortingArray, 0, this.attrCount);
			XmlTextReaderImpl.NodeData nodeData2 = this.attrDuplSortingArray[0];
			for (int k = 1; k < this.attrCount; k++)
			{
				XmlTextReaderImpl.NodeData nodeData3 = this.attrDuplSortingArray[k];
				if (Ref.Equal(nodeData2.localName, nodeData3.localName) && Ref.Equal(nodeData2.ns, nodeData3.ns))
				{
					this.Throw("Xml_DupAttributeName", nodeData3.GetNameWPrefix(this.nameTable), nodeData3.LineNo, nodeData3.LinePos);
				}
				nodeData2 = nodeData3;
			}
		}

		private void OnDefaultNamespaceDecl(XmlTextReaderImpl.NodeData attr)
		{
			if (!this.supportNamespaces)
			{
				return;
			}
			string text = this.nameTable.Add(attr.StringValue);
			attr.ns = this.nameTable.Add("http://www.w3.org/2000/xmlns/");
			if (!this.curNode.xmlContextPushed)
			{
				this.PushXmlContext();
			}
			this.xmlContext.defaultNamespace = text;
			this.AddNamespace(string.Empty, text, attr);
		}

		private void OnNamespaceDecl(XmlTextReaderImpl.NodeData attr)
		{
			if (!this.supportNamespaces)
			{
				return;
			}
			string text = this.nameTable.Add(attr.StringValue);
			if (text.Length == 0)
			{
				this.Throw("Xml_BadNamespaceDecl", attr.lineInfo2.lineNo, attr.lineInfo2.linePos - 1);
			}
			this.AddNamespace(attr.localName, text, attr);
		}

		private void OnXmlReservedAttribute(XmlTextReaderImpl.NodeData attr)
		{
			string localName;
			if ((localName = attr.localName) != null)
			{
				if (localName == "space")
				{
					if (!this.curNode.xmlContextPushed)
					{
						this.PushXmlContext();
					}
					string text;
					if ((text = XmlConvert.TrimString(attr.StringValue)) != null)
					{
						if (text == "preserve")
						{
							this.xmlContext.xmlSpace = XmlSpace.Preserve;
							return;
						}
						if (text == "default")
						{
							this.xmlContext.xmlSpace = XmlSpace.Default;
							return;
						}
					}
					this.Throw("Xml_InvalidXmlSpace", attr.StringValue, attr.lineInfo.lineNo, attr.lineInfo.linePos);
					return;
				}
				if (!(localName == "lang"))
				{
					return;
				}
				if (!this.curNode.xmlContextPushed)
				{
					this.PushXmlContext();
				}
				this.xmlContext.xmlLang = attr.StringValue;
			}
		}

		private unsafe void ParseAttributeValueSlow(int curPos, char quoteChar, XmlTextReaderImpl.NodeData attr)
		{
			int num = curPos;
			char[] array = this.ps.chars;
			int entityId = this.ps.entityId;
			int num2 = 0;
			LineInfo lineInfo = new LineInfo(this.ps.lineNo, this.ps.LinePos);
			XmlTextReaderImpl.NodeData nodeData = null;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[array[num]] & 128) == 0)
				{
					if (num - this.ps.charPos > 0)
					{
						this.stringBuilder.Append(array, this.ps.charPos, num - this.ps.charPos);
						this.ps.charPos = num;
					}
					if (array[num] == quoteChar && entityId == this.ps.entityId)
					{
						goto IL_0654;
					}
					char c = array[num];
					if (c <= '"')
					{
						switch (c)
						{
						case '\t':
							num++;
							if (this.normalize)
							{
								this.stringBuilder.Append(' ');
								this.ps.charPos = this.ps.charPos + 1;
								continue;
							}
							continue;
						case '\n':
							num++;
							this.OnNewLine(num);
							if (this.normalize)
							{
								this.stringBuilder.Append(' ');
								this.ps.charPos = this.ps.charPos + 1;
								continue;
							}
							continue;
						case '\v':
						case '\f':
							goto IL_0500;
						case '\r':
							if (array[num + 1] == '\n')
							{
								num += 2;
								if (this.normalize)
								{
									this.stringBuilder.Append(this.ps.eolNormalized ? "  " : " ");
									this.ps.charPos = num;
								}
							}
							else
							{
								if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
								{
									goto IL_055F;
								}
								num++;
								if (this.normalize)
								{
									this.stringBuilder.Append(' ');
									this.ps.charPos = num;
								}
							}
							this.OnNewLine(num);
							continue;
						default:
							if (c != '"')
							{
								goto IL_0500;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '&':
						{
							if (num - this.ps.charPos > 0)
							{
								this.stringBuilder.Append(array, this.ps.charPos, num - this.ps.charPos);
							}
							this.ps.charPos = num;
							int entityId2 = this.ps.entityId;
							LineInfo lineInfo2 = new LineInfo(this.ps.lineNo, this.ps.LinePos + 1);
							switch (this.HandleEntityReference(true, XmlTextReaderImpl.EntityExpandType.All, out num))
							{
							case XmlTextReaderImpl.EntityType.CharacterDec:
							case XmlTextReaderImpl.EntityType.CharacterHex:
							case XmlTextReaderImpl.EntityType.CharacterNamed:
								break;
							case XmlTextReaderImpl.EntityType.Expanded:
							case XmlTextReaderImpl.EntityType.Skipped:
								goto IL_04E3;
							case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
								if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full && entityId2 == entityId)
								{
									int num3 = this.stringBuilder.Length - num2;
									if (num3 > 0)
									{
										XmlTextReaderImpl.NodeData nodeData2 = new XmlTextReaderImpl.NodeData();
										nodeData2.lineInfo = lineInfo;
										nodeData2.depth = attr.depth + 1;
										nodeData2.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString(num2, num3));
										this.AddAttributeChunkToList(attr, nodeData2, ref nodeData);
									}
									XmlTextReaderImpl.NodeData nodeData3 = new XmlTextReaderImpl.NodeData();
									nodeData3.lineInfo = lineInfo2;
									nodeData3.depth = attr.depth + 1;
									nodeData3.SetNamedNode(XmlNodeType.EntityReference, this.ps.entity.Name.Name);
									this.AddAttributeChunkToList(attr, nodeData3, ref nodeData);
									this.fullAttrCleanup = true;
								}
								num = this.ps.charPos;
								break;
							case XmlTextReaderImpl.EntityType.Unexpanded:
								if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full && this.ps.entityId == entityId)
								{
									int num4 = this.stringBuilder.Length - num2;
									if (num4 > 0)
									{
										XmlTextReaderImpl.NodeData nodeData4 = new XmlTextReaderImpl.NodeData();
										nodeData4.lineInfo = lineInfo;
										nodeData4.depth = attr.depth + 1;
										nodeData4.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString(num2, num4));
										this.AddAttributeChunkToList(attr, nodeData4, ref nodeData);
									}
									this.ps.charPos = this.ps.charPos + 1;
									string text = this.ParseEntityName();
									XmlTextReaderImpl.NodeData nodeData5 = new XmlTextReaderImpl.NodeData();
									nodeData5.lineInfo = lineInfo2;
									nodeData5.depth = attr.depth + 1;
									nodeData5.SetNamedNode(XmlNodeType.EntityReference, text);
									this.AddAttributeChunkToList(attr, nodeData5, ref nodeData);
									this.stringBuilder.Append('&');
									this.stringBuilder.Append(text);
									this.stringBuilder.Append(';');
									num2 = this.stringBuilder.Length;
									lineInfo.Set(this.ps.LineNo, this.ps.LinePos);
									this.fullAttrCleanup = true;
								}
								else
								{
									this.ps.charPos = this.ps.charPos + 1;
									this.ParseEntityName();
								}
								num = this.ps.charPos;
								break;
							default:
								goto IL_04E3;
							}
							IL_04EF:
							array = this.ps.chars;
							continue;
							IL_04E3:
							num = this.ps.charPos;
							goto IL_04EF;
						}
						case '\'':
							break;
						default:
							switch (c)
							{
							case '<':
								this.Throw(num, "Xml_BadAttributeChar", XmlException.BuildCharExceptionStr('<'));
								goto IL_055F;
							case '=':
								goto IL_0500;
							case '>':
								break;
							default:
								goto IL_0500;
							}
							break;
						}
					}
					num++;
					continue;
					IL_0500:
					if (num != this.ps.charsUsed)
					{
						char c2 = array[num];
						if (c2 >= '\ud800' && c2 <= '\udbff')
						{
							if (num + 1 == this.ps.charsUsed)
							{
								goto IL_055F;
							}
							num++;
							if (array[num] >= '\udc00' && array[num] <= '\udfff')
							{
								num++;
								continue;
							}
						}
						this.ThrowInvalidChar(num, c2);
					}
					IL_055F:
					if (this.ReadData() == 0)
					{
						if (this.ps.charsUsed - this.ps.charPos > 0)
						{
							if (this.ps.chars[this.ps.charPos] != '\r')
							{
								this.Throw("Xml_UnexpectedEOF1");
							}
						}
						else
						{
							if (!this.InEntity)
							{
								if (this.fragmentType == XmlNodeType.Attribute)
								{
									break;
								}
								this.Throw("Xml_UnclosedQuote");
							}
							if (this.HandleEntityEnd(true))
							{
								this.Throw("Xml_InternalError");
							}
							if (entityId == this.ps.entityId)
							{
								num2 = this.stringBuilder.Length;
								lineInfo.Set(this.ps.LineNo, this.ps.LinePos);
							}
						}
					}
					num = this.ps.charPos;
					array = this.ps.chars;
				}
				else
				{
					num++;
				}
			}
			if (entityId != this.ps.entityId)
			{
				this.Throw("Xml_EntityRefNesting");
			}
			IL_0654:
			if (attr.nextAttrValueChunk != null)
			{
				int num5 = this.stringBuilder.Length - num2;
				if (num5 > 0)
				{
					XmlTextReaderImpl.NodeData nodeData6 = new XmlTextReaderImpl.NodeData();
					nodeData6.lineInfo = lineInfo;
					nodeData6.depth = attr.depth + 1;
					nodeData6.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString(num2, num5));
					this.AddAttributeChunkToList(attr, nodeData6, ref nodeData);
				}
			}
			this.ps.charPos = num + 1;
			attr.SetValue(this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
		}

		private void AddAttributeChunkToList(XmlTextReaderImpl.NodeData attr, XmlTextReaderImpl.NodeData chunk, ref XmlTextReaderImpl.NodeData lastChunk)
		{
			if (lastChunk == null)
			{
				lastChunk = chunk;
				attr.nextAttrValueChunk = chunk;
				return;
			}
			lastChunk.nextAttrValueChunk = chunk;
			lastChunk = chunk;
		}

		private bool ParseText()
		{
			int num = 0;
			if (this.parsingMode != XmlTextReaderImpl.ParsingMode.Full)
			{
				int num2;
				int num3;
				while (!this.ParseText(out num2, out num3, ref num))
				{
				}
			}
			else
			{
				this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
				int num2;
				int num3;
				if (this.ParseText(out num2, out num3, ref num))
				{
					if (num3 - num2 != 0)
					{
						XmlNodeType textNodeType = this.GetTextNodeType(num);
						if (textNodeType != XmlNodeType.None)
						{
							this.curNode.SetValueNode(textNodeType, this.ps.chars, num2, num3 - num2);
							return true;
						}
					}
				}
				else if (this.v1Compat)
				{
					do
					{
						this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					}
					while (!this.ParseText(out num2, out num3, ref num));
					this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					XmlNodeType textNodeType2 = this.GetTextNodeType(num);
					if (textNodeType2 != XmlNodeType.None)
					{
						this.curNode.SetValueNode(textNodeType2, this.stringBuilder.ToString());
						this.stringBuilder.Length = 0;
						return true;
					}
					this.stringBuilder.Length = 0;
				}
				else
				{
					if (num > 32)
					{
						this.curNode.SetValueNode(XmlNodeType.Text, this.ps.chars, num2, num3 - num2);
						this.nextParsingFunction = this.parsingFunction;
						this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PartialTextValue;
						return true;
					}
					this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					bool flag;
					do
					{
						flag = this.ParseText(out num2, out num3, ref num);
						this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					}
					while (!flag && num <= 32 && this.stringBuilder.Length < 4096);
					XmlNodeType xmlNodeType = ((this.stringBuilder.Length < 4096) ? this.GetTextNodeType(num) : XmlNodeType.Text);
					if (xmlNodeType != XmlNodeType.None)
					{
						this.curNode.SetValueNode(xmlNodeType, this.stringBuilder.ToString());
						this.stringBuilder.Length = 0;
						if (!flag)
						{
							this.nextParsingFunction = this.parsingFunction;
							this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PartialTextValue;
						}
						return true;
					}
					this.stringBuilder.Length = 0;
					if (!flag)
					{
						while (!this.ParseText(out num2, out num3, ref num))
						{
						}
					}
				}
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.ReportEndEntity)
			{
				this.SetupEndEntityNodeInContent();
				this.parsingFunction = this.nextParsingFunction;
				return true;
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.EntityReference)
			{
				this.parsingFunction = this.nextNextParsingFunction;
				this.ParseEntityReference();
				return true;
			}
			return false;
		}

		private unsafe bool ParseText(out int startPos, out int endPos, ref int outOrChars)
		{
			char[] array = this.ps.chars;
			int num = this.ps.charPos;
			int num2 = 0;
			int num3 = -1;
			int num4 = outOrChars;
			char c;
			int num7;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[c = array[num]] & 64) == 0)
				{
					char c2 = c;
					if (c2 <= '&')
					{
						switch (c2)
						{
						case '\t':
							num++;
							continue;
						case '\n':
							num++;
							this.OnNewLine(num);
							continue;
						case '\v':
						case '\f':
							break;
						case '\r':
							if (array[num + 1] == '\n')
							{
								if (!this.ps.eolNormalized && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
								{
									if (num - this.ps.charPos > 0)
									{
										if (num2 == 0)
										{
											num2 = 1;
											num3 = num;
										}
										else
										{
											this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
											num3 = num - num2;
											num2++;
										}
									}
									else
									{
										this.ps.charPos = this.ps.charPos + 1;
									}
								}
								num += 2;
							}
							else
							{
								if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
								{
									goto IL_036A;
								}
								if (!this.ps.eolNormalized)
								{
									array[num] = '\n';
								}
								num++;
							}
							this.OnNewLine(num);
							continue;
						default:
							if (c2 == '&')
							{
								int num6;
								XmlTextReaderImpl.EntityType entityType;
								int num5;
								if ((num5 = this.ParseCharRefInline(num, out num6, out entityType)) > 0)
								{
									if (num2 > 0)
									{
										this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
									}
									num3 = num - num2;
									num2 += num5 - num - num6;
									num = num5;
									if (!this.xmlCharType.IsWhiteSpace(array[num5 - num6]) || (this.v1Compat && entityType == XmlTextReaderImpl.EntityType.CharacterDec))
									{
										num4 |= 255;
										continue;
									}
									continue;
								}
								else
								{
									if (num > this.ps.charPos)
									{
										goto IL_0415;
									}
									switch (this.HandleEntityReference(false, XmlTextReaderImpl.EntityExpandType.All, out num))
									{
									case XmlTextReaderImpl.EntityType.CharacterDec:
										if (!this.v1Compat)
										{
											goto IL_0229;
										}
										num4 |= 255;
										break;
									case XmlTextReaderImpl.EntityType.CharacterHex:
									case XmlTextReaderImpl.EntityType.CharacterNamed:
										goto IL_0229;
									case XmlTextReaderImpl.EntityType.Expanded:
									case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
									case XmlTextReaderImpl.EntityType.Skipped:
										goto IL_0251;
									case XmlTextReaderImpl.EntityType.Unexpanded:
										goto IL_01FC;
									default:
										goto IL_0251;
									}
									IL_025D:
									array = this.ps.chars;
									continue;
									IL_0251:
									num = this.ps.charPos;
									goto IL_025D;
									IL_0229:
									if (!this.xmlCharType.IsWhiteSpace(this.ps.chars[num - 1]))
									{
										num4 |= 255;
										goto IL_025D;
									}
									goto IL_025D;
								}
							}
							break;
						}
					}
					else
					{
						if (c2 == '<')
						{
							goto IL_0415;
						}
						if (c2 == ']')
						{
							if (this.ps.charsUsed - num >= 3 || this.ps.isEof)
							{
								if (array[num + 1] == ']' && array[num + 2] == '>')
								{
									this.Throw(num, "Xml_CDATAEndInText");
								}
								num4 |= 93;
								num++;
								continue;
							}
							goto IL_036A;
						}
					}
					if (num != this.ps.charsUsed)
					{
						char c3 = array[num];
						if (c3 >= '\ud800' && c3 <= '\udbff')
						{
							if (num + 1 == this.ps.charsUsed)
							{
								goto IL_036A;
							}
							num++;
							if (array[num] >= '\udc00' && array[num] <= '\udfff')
							{
								num++;
								num4 |= (int)c3;
								continue;
							}
						}
						num7 = num - this.ps.charPos;
						if (this.ZeroEndingStream(num))
						{
							goto Block_31;
						}
						this.ThrowInvalidChar(this.ps.charPos + num7, c3);
					}
					IL_036A:
					if (num > this.ps.charPos)
					{
						goto IL_0415;
					}
					if (this.ReadData() == 0)
					{
						if (this.ps.charsUsed - this.ps.charPos > 0)
						{
							if (this.ps.chars[this.ps.charPos] != '\r')
							{
								this.Throw("Xml_UnexpectedEOF1");
							}
						}
						else
						{
							if (!this.InEntity)
							{
								goto IL_0409;
							}
							if (this.HandleEntityEnd(true))
							{
								goto Block_37;
							}
						}
					}
					num = this.ps.charPos;
					array = this.ps.chars;
				}
				else
				{
					num4 |= (int)c;
					num++;
				}
			}
			IL_01FC:
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.EntityReference;
			goto IL_0409;
			Block_31:
			array = this.ps.chars;
			num = this.ps.charPos + num7;
			goto IL_0415;
			Block_37:
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ReportEndEntity;
			IL_0409:
			startPos = (endPos = num);
			return true;
			IL_0415:
			if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full && num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
			}
			startPos = this.ps.charPos;
			endPos = num - num2;
			this.ps.charPos = num;
			outOrChars = num4;
			return c == '<';
		}

		private void FinishPartialValue()
		{
			this.curNode.CopyTo(this.readValueOffset, this.stringBuilder);
			int num = 0;
			int num2;
			int num3;
			while (!this.ParseText(out num2, out num3, ref num))
			{
				this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
			}
			this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
			this.curNode.SetValue(this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
		}

		private void FinishOtherValueIterator()
		{
			switch (this.parsingFunction)
			{
			case XmlTextReaderImpl.ParsingFunction.InReadAttributeValue:
				break;
			case XmlTextReaderImpl.ParsingFunction.InReadValueChunk:
				if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue)
				{
					this.FinishPartialValue();
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnCachedValue;
					return;
				}
				if (this.readValueOffset > 0)
				{
					this.curNode.SetValue(this.curNode.StringValue.Substring(this.readValueOffset));
					this.readValueOffset = 0;
					return;
				}
				break;
			case XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary:
			case XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary:
				switch (this.incReadState)
				{
				case XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue:
					if (this.readValueOffset > 0)
					{
						this.curNode.SetValue(this.curNode.StringValue.Substring(this.readValueOffset));
						this.readValueOffset = 0;
						return;
					}
					break;
				case XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue:
					this.FinishPartialValue();
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue;
					return;
				case XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End:
					this.curNode.SetValue(string.Empty);
					break;
				default:
					return;
				}
				break;
			default:
				return;
			}
		}

		private void SkipPartialTextValue()
		{
			int num = 0;
			this.parsingFunction = this.nextParsingFunction;
			int num2;
			int num3;
			while (!this.ParseText(out num2, out num3, ref num))
			{
			}
		}

		private void FinishReadValueChunk()
		{
			this.readValueOffset = 0;
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue)
			{
				this.SkipPartialTextValue();
				return;
			}
			this.parsingFunction = this.nextParsingFunction;
			this.nextParsingFunction = this.nextNextParsingFunction;
		}

		private void FinishReadContentAsBinary()
		{
			this.readValueOffset = 0;
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue)
			{
				this.SkipPartialTextValue();
			}
			else
			{
				this.parsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = this.nextNextParsingFunction;
			}
			if (this.incReadState != XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End)
			{
				while (this.MoveToNextContentNode(true))
				{
				}
			}
		}

		private void FinishReadElementContentAsBinary()
		{
			this.FinishReadContentAsBinary();
			if (this.curNode.type != XmlNodeType.EndElement)
			{
				this.Throw("Xml_InvalidNodeType", this.curNode.type.ToString());
			}
			this.outerReader.Read();
		}

		private bool ParseRootLevelWhitespace()
		{
			XmlNodeType whitespaceType = this.GetWhitespaceType();
			if (whitespaceType == XmlNodeType.None)
			{
				this.EatWhitespaces(null);
				if (this.ps.chars[this.ps.charPos] == '<' || this.ps.charsUsed - this.ps.charPos == 0 || this.ZeroEndingStream(this.ps.charPos))
				{
					return false;
				}
			}
			else
			{
				this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
				this.EatWhitespaces(this.stringBuilder);
				if (this.ps.chars[this.ps.charPos] == '<' || this.ps.charsUsed - this.ps.charPos == 0 || this.ZeroEndingStream(this.ps.charPos))
				{
					if (this.stringBuilder.Length > 0)
					{
						this.curNode.SetValueNode(whitespaceType, this.stringBuilder.ToString());
						this.stringBuilder.Length = 0;
						return true;
					}
					return false;
				}
			}
			if (this.xmlCharType.IsCharData(this.ps.chars[this.ps.charPos]))
			{
				this.Throw("Xml_InvalidRootData");
			}
			else
			{
				this.ThrowInvalidChar(this.ps.charPos, this.ps.chars[this.ps.charPos]);
			}
			return false;
		}

		private void ParseEntityReference()
		{
			this.ps.charPos = this.ps.charPos + 1;
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			this.curNode.SetNamedNode(XmlNodeType.EntityReference, this.ParseEntityName());
		}

		private XmlTextReaderImpl.EntityType HandleEntityReference(bool isInAttributeValue, XmlTextReaderImpl.EntityExpandType expandType, out int charRefEndPos)
		{
			if (this.ps.charPos + 1 == this.ps.charsUsed && this.ReadData() == 0)
			{
				this.Throw("Xml_UnexpectedEOF1");
			}
			if (this.ps.chars[this.ps.charPos + 1] == '#')
			{
				XmlTextReaderImpl.EntityType entityType;
				charRefEndPos = this.ParseNumericCharRef(expandType != XmlTextReaderImpl.EntityExpandType.OnlyGeneral, null, out entityType);
				return entityType;
			}
			charRefEndPos = this.ParseNamedCharRef(expandType != XmlTextReaderImpl.EntityExpandType.OnlyGeneral, null);
			if (charRefEndPos >= 0)
			{
				return XmlTextReaderImpl.EntityType.CharacterNamed;
			}
			if (expandType != XmlTextReaderImpl.EntityExpandType.OnlyCharacter && (this.entityHandling == EntityHandling.ExpandEntities || (isInAttributeValue && this.validatingReaderCompatFlag)))
			{
				this.ps.charPos = this.ps.charPos + 1;
				int linePos = this.ps.LinePos;
				int num;
				try
				{
					num = this.ParseName();
				}
				catch (XmlException)
				{
					this.Throw("Xml_ErrorParsingEntityName", this.ps.LineNo, linePos);
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				if (this.ps.chars[num] != ';')
				{
					this.ThrowUnexpectedToken(num, ";");
				}
				int linePos2 = this.ps.LinePos;
				string text = this.nameTable.Add(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
				this.ps.charPos = num + 1;
				charRefEndPos = -1;
				XmlTextReaderImpl.EntityType entityType2 = this.HandleGeneralEntityReference(text, isInAttributeValue, false, linePos2);
				this.reportedBaseUri = this.ps.baseUriStr;
				this.reportedEncoding = this.ps.encoding;
				return entityType2;
			}
			return XmlTextReaderImpl.EntityType.Unexpanded;
		}

		private XmlTextReaderImpl.EntityType HandleGeneralEntityReference(string name, bool isInAttributeValue, bool pushFakeEntityIfNullResolver, int entityStartLinePos)
		{
			SchemaEntity schemaEntity = null;
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name);
			if (this.dtdParserProxy == null && this.fragmentParserContext != null && this.fragmentParserContext.HasDtdInfo && !this.prohibitDtd)
			{
				this.ParseDtdFromParserContext();
			}
			if (this.dtdParserProxy == null || (schemaEntity = (SchemaEntity)this.dtdParserProxy.DtdSchemaInfo.GeneralEntities[xmlQualifiedName]) == null)
			{
				if (this.disableUndeclaredEntityCheck)
				{
					schemaEntity = new SchemaEntity(new XmlQualifiedName(name), false);
					schemaEntity.Text = string.Empty;
				}
				else
				{
					this.Throw("Xml_UndeclaredEntity", name, this.ps.LineNo, entityStartLinePos);
				}
			}
			if (schemaEntity.IsProcessed)
			{
				this.Throw("Xml_RecursiveGenEntity", name, this.ps.LineNo, entityStartLinePos);
			}
			if (!schemaEntity.NData.IsEmpty)
			{
				if (this.disableUndeclaredEntityCheck)
				{
					schemaEntity = new SchemaEntity(new XmlQualifiedName(name), false);
					schemaEntity.Text = string.Empty;
				}
				else
				{
					this.Throw("Xml_UnparsedEntityRef", name, this.ps.LineNo, entityStartLinePos);
				}
			}
			if (this.standalone && schemaEntity.DeclaredInExternal)
			{
				this.Throw("Xml_ExternalEntityInStandAloneDocument", schemaEntity.Name.Name, this.ps.LineNo, entityStartLinePos);
			}
			if (schemaEntity.IsExternal)
			{
				if (isInAttributeValue)
				{
					this.Throw("Xml_ExternalEntityInAttValue", name, this.ps.LineNo, entityStartLinePos);
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				if (this.parsingMode == XmlTextReaderImpl.ParsingMode.SkipContent)
				{
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				if (this.IsResolverNull)
				{
					if (pushFakeEntityIfNullResolver)
					{
						this.PushExternalEntity(schemaEntity, ++this.nextEntityId);
						this.curNode.entityId = this.ps.entityId;
						return XmlTextReaderImpl.EntityType.FakeExpanded;
					}
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				else
				{
					this.PushExternalEntity(schemaEntity, ++this.nextEntityId);
					this.curNode.entityId = this.ps.entityId;
					if (!isInAttributeValue || !this.validatingReaderCompatFlag)
					{
						return XmlTextReaderImpl.EntityType.Expanded;
					}
					return XmlTextReaderImpl.EntityType.ExpandedInAttribute;
				}
			}
			else
			{
				if (this.parsingMode == XmlTextReaderImpl.ParsingMode.SkipContent)
				{
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				int num = this.nextEntityId++;
				this.PushInternalEntity(schemaEntity, num);
				this.curNode.entityId = num;
				if (!isInAttributeValue || !this.validatingReaderCompatFlag)
				{
					return XmlTextReaderImpl.EntityType.Expanded;
				}
				return XmlTextReaderImpl.EntityType.ExpandedInAttribute;
			}
		}

		private bool InEntity
		{
			get
			{
				return this.parsingStatesStackTop >= 0;
			}
		}

		private bool HandleEntityEnd(bool checkEntityNesting)
		{
			if (this.parsingStatesStackTop == -1)
			{
				this.Throw("Xml_InternalError");
			}
			if (this.ps.entityResolvedManually)
			{
				this.index--;
				if (checkEntityNesting && this.ps.entityId != this.nodes[this.index].entityId)
				{
					this.Throw("Xml_IncompleteEntity");
				}
				this.lastEntity = this.ps.entity;
				this.PopEntity();
				this.curNode.entityId = this.ps.entityId;
				return true;
			}
			if (checkEntityNesting && this.ps.entityId != this.nodes[this.index].entityId)
			{
				this.Throw("Xml_IncompleteEntity");
			}
			this.PopEntity();
			this.curNode.entityId = this.ps.entityId;
			this.reportedEncoding = this.ps.encoding;
			this.reportedBaseUri = this.ps.baseUriStr;
			return false;
		}

		private void SetupEndEntityNodeInContent()
		{
			this.reportedEncoding = this.ps.encoding;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.curNode = this.nodes[this.index];
			this.curNode.SetNamedNode(XmlNodeType.EndEntity, this.lastEntity.Name.Name);
			this.curNode.lineInfo.Set(this.ps.lineNo, this.ps.LinePos - 1);
			if (this.index == 0 && this.parsingFunction == XmlTextReaderImpl.ParsingFunction.ElementContent)
			{
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			}
		}

		private void SetupEndEntityNodeInAttribute()
		{
			this.curNode = this.nodes[this.index + this.attrCount + 1];
			XmlTextReaderImpl.NodeData nodeData = this.curNode;
			nodeData.lineInfo.linePos = nodeData.lineInfo.linePos + this.curNode.localName.Length;
			this.curNode.type = XmlNodeType.EndEntity;
		}

		private bool ParsePI()
		{
			return this.ParsePI(null);
		}

		private bool ParsePI(BufferBuilder piInDtdStringBuilder)
		{
			if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
			{
				this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			}
			int num = this.ParseName();
			string text = this.nameTable.Add(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
			if (string.Compare(text, "xml", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.Throw(text.Equals("xml") ? "Xml_XmlDeclNotFirst" : "Xml_InvalidPIName", text);
			}
			this.ps.charPos = num;
			if (piInDtdStringBuilder == null)
			{
				if (!this.ignorePIs && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
				{
					this.curNode.SetNamedNode(XmlNodeType.ProcessingInstruction, text);
				}
			}
			else
			{
				piInDtdStringBuilder.Append(text);
			}
			char c = this.ps.chars[this.ps.charPos];
			if (this.EatWhitespaces(piInDtdStringBuilder) == 0)
			{
				if (this.ps.charsUsed - this.ps.charPos < 2)
				{
					this.ReadData();
				}
				if (c != '?' || this.ps.chars[this.ps.charPos + 1] != '>')
				{
					this.Throw("Xml_BadNameChar", XmlException.BuildCharExceptionStr(c));
				}
			}
			int num2;
			int num3;
			if (this.ParsePIValue(out num2, out num3))
			{
				if (piInDtdStringBuilder == null)
				{
					if (this.ignorePIs)
					{
						return false;
					}
					if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
					{
						this.curNode.SetValue(this.ps.chars, num2, num3 - num2);
					}
				}
				else
				{
					piInDtdStringBuilder.Append(this.ps.chars, num2, num3 - num2);
				}
			}
			else
			{
				BufferBuilder bufferBuilder;
				if (piInDtdStringBuilder == null)
				{
					if (this.ignorePIs || this.parsingMode != XmlTextReaderImpl.ParsingMode.Full)
					{
						while (!this.ParsePIValue(out num2, out num3))
						{
						}
						return false;
					}
					bufferBuilder = this.stringBuilder;
				}
				else
				{
					bufferBuilder = piInDtdStringBuilder;
				}
				do
				{
					bufferBuilder.Append(this.ps.chars, num2, num3 - num2);
				}
				while (!this.ParsePIValue(out num2, out num3));
				bufferBuilder.Append(this.ps.chars, num2, num3 - num2);
				if (piInDtdStringBuilder == null)
				{
					this.curNode.SetValue(this.stringBuilder.ToString());
					this.stringBuilder.Length = 0;
				}
			}
			return true;
		}

		private unsafe bool ParsePIValue(out int outStartPos, out int outEndPos)
		{
			if (this.ps.charsUsed - this.ps.charPos < 2 && this.ReadData() == 0)
			{
				this.Throw(this.ps.charsUsed, "Xml_UnexpectedEOF", "PI");
			}
			int num = this.ps.charPos;
			char[] chars = this.ps.chars;
			int num2 = 0;
			int num3 = -1;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[chars[num]] & 64) == 0 || chars[num] == '?')
				{
					char c = chars[num];
					if (c <= '&')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							num++;
							this.OnNewLine(num);
							continue;
						case '\v':
						case '\f':
							goto IL_01F2;
						case '\r':
							if (chars[num + 1] == '\n')
							{
								if (!this.ps.eolNormalized && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
								{
									if (num - this.ps.charPos > 0)
									{
										if (num2 == 0)
										{
											num2 = 1;
											num3 = num;
										}
										else
										{
											this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
											num3 = num - num2;
											num2++;
										}
									}
									else
									{
										this.ps.charPos = this.ps.charPos + 1;
									}
								}
								num += 2;
							}
							else
							{
								if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
								{
									goto IL_0256;
								}
								if (!this.ps.eolNormalized)
								{
									chars[num] = '\n';
								}
								num++;
							}
							this.OnNewLine(num);
							continue;
						default:
							if (c != '&')
							{
								goto IL_01F2;
							}
							break;
						}
					}
					else if (c != '<')
					{
						if (c != '?')
						{
							if (c != ']')
							{
								goto IL_01F2;
							}
						}
						else
						{
							if (chars[num + 1] == '>')
							{
								break;
							}
							if (num + 1 != this.ps.charsUsed)
							{
								num++;
								continue;
							}
							goto IL_0256;
						}
					}
					num++;
					continue;
					IL_01F2:
					if (num == this.ps.charsUsed)
					{
						goto IL_0256;
					}
					char c2 = chars[num];
					if (c2 >= '\ud800' && c2 <= '\udbff')
					{
						if (num + 1 == this.ps.charsUsed)
						{
							goto IL_0256;
						}
						num++;
						if (chars[num] >= '\udc00' && chars[num] <= '\udfff')
						{
							num++;
							continue;
						}
					}
					this.ThrowInvalidChar(num, c2);
				}
				else
				{
					num++;
				}
			}
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num + 2;
			return true;
			IL_0256:
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num;
			return false;
		}

		private bool ParseComment()
		{
			if (this.ignoreComments)
			{
				XmlTextReaderImpl.ParsingMode parsingMode = this.parsingMode;
				this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipNode;
				this.ParseCDataOrComment(XmlNodeType.Comment);
				this.parsingMode = parsingMode;
				return false;
			}
			this.ParseCDataOrComment(XmlNodeType.Comment);
			return true;
		}

		private void ParseCData()
		{
			this.ParseCDataOrComment(XmlNodeType.CDATA);
		}

		private void ParseCDataOrComment(XmlNodeType type)
		{
			int num;
			int num2;
			if (this.parsingMode != XmlTextReaderImpl.ParsingMode.Full)
			{
				while (!this.ParseCDataOrComment(type, out num, out num2))
				{
				}
				return;
			}
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			if (this.ParseCDataOrComment(type, out num, out num2))
			{
				this.curNode.SetValueNode(type, this.ps.chars, num, num2 - num);
				return;
			}
			do
			{
				this.stringBuilder.Append(this.ps.chars, num, num2 - num);
			}
			while (!this.ParseCDataOrComment(type, out num, out num2));
			this.stringBuilder.Append(this.ps.chars, num, num2 - num);
			this.curNode.SetValueNode(type, this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
		}

		private unsafe bool ParseCDataOrComment(XmlNodeType type, out int outStartPos, out int outEndPos)
		{
			if (this.ps.charsUsed - this.ps.charPos < 3 && this.ReadData() == 0)
			{
				this.Throw("Xml_UnexpectedEOF", (type == XmlNodeType.Comment) ? "Comment" : "CDATA");
			}
			int num = this.ps.charPos;
			char[] chars = this.ps.chars;
			int num2 = 0;
			int num3 = -1;
			char c = ((type == XmlNodeType.Comment) ? '-' : ']');
			char c3;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[chars[num]] & 64) == 0 || chars[num] == c)
				{
					if (chars[num] == c)
					{
						if (chars[num + 1] == c)
						{
							if (chars[num + 2] == '>')
							{
								break;
							}
							if (num + 2 == this.ps.charsUsed)
							{
								goto IL_028F;
							}
							if (type == XmlNodeType.Comment)
							{
								this.Throw(num, "Xml_InvalidCommentChars");
							}
						}
						else if (num + 1 == this.ps.charsUsed)
						{
							goto IL_028F;
						}
						num++;
					}
					else
					{
						char c2 = chars[num];
						if (c2 <= '&')
						{
							switch (c2)
							{
							case '\t':
								break;
							case '\n':
								num++;
								this.OnNewLine(num);
								continue;
							case '\v':
							case '\f':
								goto IL_0230;
							case '\r':
								if (chars[num + 1] == '\n')
								{
									if (!this.ps.eolNormalized && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
									{
										if (num - this.ps.charPos > 0)
										{
											if (num2 == 0)
											{
												num2 = 1;
												num3 = num;
											}
											else
											{
												this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
												num3 = num - num2;
												num2++;
											}
										}
										else
										{
											this.ps.charPos = this.ps.charPos + 1;
										}
									}
									num += 2;
								}
								else
								{
									if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
									{
										goto IL_028F;
									}
									if (!this.ps.eolNormalized)
									{
										chars[num] = '\n';
									}
									num++;
								}
								this.OnNewLine(num);
								continue;
							default:
								if (c2 != '&')
								{
									goto IL_0230;
								}
								break;
							}
						}
						else if (c2 != '<' && c2 != ']')
						{
							goto IL_0230;
						}
						num++;
						continue;
						IL_0230:
						if (num == this.ps.charsUsed)
						{
							goto IL_028F;
						}
						c3 = chars[num];
						if (c3 < '\ud800' || c3 > '\udbff')
						{
							goto IL_0286;
						}
						if (num + 1 == this.ps.charsUsed)
						{
							goto IL_028F;
						}
						num++;
						if (chars[num] < '\udc00' || chars[num] > '\udfff')
						{
							goto IL_0286;
						}
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num + 3;
			return true;
			IL_0286:
			this.ThrowInvalidChar(num, c3);
			IL_028F:
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num;
			return false;
		}

		private void ParseDoctypeDecl()
		{
			if (this.prohibitDtd)
			{
				this.ThrowWithoutLineInfo(this.v1Compat ? "Xml_DtdIsProhibited" : "Xml_DtdIsProhibitedEx", string.Empty);
			}
			while (this.ps.charsUsed - this.ps.charPos < 8)
			{
				if (this.ReadData() == 0)
				{
					this.Throw("Xml_UnexpectedEOF", "DOCTYPE");
				}
			}
			if (!XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, 7, "DOCTYPE"))
			{
				this.ThrowUnexpectedToken((!this.rootElementParsed && this.dtdParserProxy == null) ? "DOCTYPE" : "<!--");
			}
			if (!this.xmlCharType.IsWhiteSpace(this.ps.chars[this.ps.charPos + 7]))
			{
				this.Throw("Xml_ExpectingWhiteSpace", this.ParseUnexpectedToken(this.ps.charPos + 7));
			}
			if (this.dtdParserProxy != null)
			{
				this.Throw(this.ps.charPos - 2, "Xml_MultipleDTDsProvided");
			}
			if (this.rootElementParsed)
			{
				this.Throw(this.ps.charPos - 2, "Xml_DtdAfterRootElement");
			}
			this.ps.charPos = this.ps.charPos + 8;
			this.EatWhitespaces(null);
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			this.dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(this);
			this.dtdParserProxy.Parse(true);
			SchemaInfo dtdSchemaInfo = this.dtdParserProxy.DtdSchemaInfo;
			if ((this.validatingReaderCompatFlag || !this.v1Compat) && (dtdSchemaInfo.HasDefaultAttributes || dtdSchemaInfo.HasNonCDataAttributes))
			{
				this.addDefaultAttributesAndNormalize = true;
				this.qName = new XmlQualifiedName();
			}
			this.curNode.SetNamedNode(XmlNodeType.DocumentType, dtdSchemaInfo.DocTypeName.ToString());
			this.curNode.SetValue(this.dtdParserProxy.InternalDtdSubset);
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ResetAttributesRootLevel;
		}

		private int EatWhitespaces(BufferBuilder sb)
		{
			int num = this.ps.charPos;
			int num2 = 0;
			char[] array = this.ps.chars;
			for (;;)
			{
				char c = array[num];
				switch (c)
				{
				case '\t':
					break;
				case '\n':
					num++;
					this.OnNewLine(num);
					continue;
				case '\v':
				case '\f':
					goto IL_00F9;
				case '\r':
					if (array[num + 1] == '\n')
					{
						int num3 = num - this.ps.charPos;
						if (sb != null && !this.ps.eolNormalized)
						{
							if (num3 > 0)
							{
								sb.Append(array, this.ps.charPos, num3);
								num2 += num3;
							}
							this.ps.charPos = num + 1;
						}
						num += 2;
					}
					else
					{
						if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
						{
							goto IL_014F;
						}
						if (!this.ps.eolNormalized)
						{
							array[num] = '\n';
						}
						num++;
					}
					this.OnNewLine(num);
					continue;
				default:
					if (c != ' ')
					{
						goto IL_00F9;
					}
					break;
				}
				num++;
				continue;
				IL_014F:
				int num4 = num - this.ps.charPos;
				if (num4 > 0)
				{
					if (sb != null)
					{
						sb.Append(this.ps.chars, this.ps.charPos, num4);
					}
					this.ps.charPos = num;
					num2 += num4;
				}
				if (this.ReadData() == 0)
				{
					if (this.ps.charsUsed - this.ps.charPos == 0)
					{
						return num2;
					}
					if (this.ps.chars[this.ps.charPos] != '\r')
					{
						this.Throw("Xml_UnexpectedEOF1");
					}
				}
				num = this.ps.charPos;
				array = this.ps.chars;
				continue;
				IL_00F9:
				if (num != this.ps.charsUsed)
				{
					break;
				}
				goto IL_014F;
			}
			int num5 = num - this.ps.charPos;
			if (num5 > 0)
			{
				if (sb != null)
				{
					sb.Append(this.ps.chars, this.ps.charPos, num5);
				}
				this.ps.charPos = num;
				num2 += num5;
			}
			return num2;
		}

		private int ParseCharRefInline(int startPos, out int charCount, out XmlTextReaderImpl.EntityType entityType)
		{
			if (this.ps.chars[startPos + 1] == '#')
			{
				return this.ParseNumericCharRefInline(startPos, true, null, out charCount, out entityType);
			}
			charCount = 1;
			entityType = XmlTextReaderImpl.EntityType.CharacterNamed;
			return this.ParseNamedCharRefInline(startPos, true, null);
		}

		private int ParseNumericCharRef(bool expand, BufferBuilder internalSubsetBuilder, out XmlTextReaderImpl.EntityType entityType)
		{
			int num3;
			int num;
			for (;;)
			{
				int num2;
				num = (num2 = this.ParseNumericCharRefInline(this.ps.charPos, expand, internalSubsetBuilder, out num3, out entityType));
				if (num2 != -2)
				{
					break;
				}
				if (this.ReadData() == 0)
				{
					this.Throw("Xml_UnexpectedEOF");
				}
			}
			if (expand)
			{
				this.ps.charPos = num - num3;
			}
			return num;
		}

		private int ParseNumericCharRefInline(int startPos, bool expand, BufferBuilder internalSubsetBuilder, out int charCount, out XmlTextReaderImpl.EntityType entityType)
		{
			int num = 0;
			char[] chars = this.ps.chars;
			int num2 = startPos + 2;
			charCount = 0;
			string text;
			if (chars[num2] == 'x')
			{
				num2++;
				text = "Xml_BadHexEntity";
				for (;;)
				{
					char c = chars[num2];
					if (c >= '0' && c <= '9')
					{
						num = num * 16 + (int)c - 48;
					}
					else if (c >= 'a' && c <= 'f')
					{
						num = num * 16 + 10 + (int)c - 97;
					}
					else
					{
						if (c < 'A' || c > 'F')
						{
							break;
						}
						num = num * 16 + 10 + (int)c - 65;
					}
					num2++;
				}
				entityType = XmlTextReaderImpl.EntityType.CharacterHex;
			}
			else
			{
				if (num2 >= this.ps.charsUsed)
				{
					entityType = XmlTextReaderImpl.EntityType.Unexpanded;
					return -2;
				}
				text = "Xml_BadDecimalEntity";
				while (chars[num2] >= '0' && chars[num2] <= '9')
				{
					num = num * 10 + (int)chars[num2] - 48;
					num2++;
				}
				entityType = XmlTextReaderImpl.EntityType.CharacterDec;
			}
			if (chars[num2] != ';')
			{
				if (num2 == this.ps.charsUsed)
				{
					return -2;
				}
				this.Throw(num2, text);
			}
			if (num <= 65535)
			{
				char c2 = (char)num;
				if ((!this.xmlCharType.IsCharData(c2) || (c2 >= '\udc00' && c2 <= '\udeff')) && ((this.v1Compat && this.normalize) || (!this.v1Compat && this.checkCharacters)))
				{
					this.ThrowInvalidChar((this.ps.chars[this.ps.charPos + 2] == 'x') ? (this.ps.charPos + 3) : (this.ps.charPos + 2), c2);
				}
				if (expand)
				{
					if (internalSubsetBuilder != null)
					{
						internalSubsetBuilder.Append(this.ps.chars, this.ps.charPos, num2 - this.ps.charPos + 1);
					}
					chars[num2] = c2;
				}
				charCount = 1;
				return num2 + 1;
			}
			int num3 = num - 65536;
			int num4 = 56320 + num3 % 1024;
			int num5 = 55296 + num3 / 1024;
			if (this.normalize)
			{
				char c3 = (char)num5;
				if (c3 >= '\ud800' && c3 <= '\udbff')
				{
					c3 = (char)num4;
					if (c3 >= '\udc00' && c3 <= '\udfff')
					{
						goto IL_0259;
					}
				}
				this.ThrowInvalidChar((this.ps.chars[this.ps.charPos + 2] == 'x') ? (this.ps.charPos + 3) : (this.ps.charPos + 2), (char)num);
			}
			IL_0259:
			if (expand)
			{
				if (internalSubsetBuilder != null)
				{
					internalSubsetBuilder.Append(this.ps.chars, this.ps.charPos, num2 - this.ps.charPos + 1);
				}
				chars[num2 - 1] = (char)num5;
				chars[num2] = (char)num4;
			}
			charCount = 2;
			return num2 + 1;
		}

		private int ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder)
		{
			int num;
			for (;;)
			{
				switch (num = this.ParseNamedCharRefInline(this.ps.charPos, expand, internalSubsetBuilder))
				{
				case -2:
					if (this.ReadData() == 0)
					{
						return -1;
					}
					continue;
				case -1:
					return -1;
				}
				break;
			}
			if (expand)
			{
				this.ps.charPos = num - 1;
			}
			return num;
		}

		private int ParseNamedCharRefInline(int startPos, bool expand, BufferBuilder internalSubsetBuilder)
		{
			int num = startPos + 1;
			char[] chars = this.ps.chars;
			char c = chars[num];
			if (c <= 'g')
			{
				if (c != 'a')
				{
					if (c == 'g')
					{
						if (this.ps.charsUsed - num < 3)
						{
							return -2;
						}
						if (chars[num + 1] == 't' && chars[num + 2] == ';')
						{
							num += 3;
							char c2 = '>';
							goto IL_0175;
						}
						return -1;
					}
				}
				else
				{
					num++;
					if (chars[num] == 'm')
					{
						if (this.ps.charsUsed - num < 3)
						{
							return -2;
						}
						if (chars[num + 1] == 'p' && chars[num + 2] == ';')
						{
							num += 3;
							char c2 = '&';
							goto IL_0175;
						}
						return -1;
					}
					else if (chars[num] == 'p')
					{
						if (this.ps.charsUsed - num < 4)
						{
							return -2;
						}
						if (chars[num + 1] == 'o' && chars[num + 2] == 's' && chars[num + 3] == ';')
						{
							num += 4;
							char c2 = '\'';
							goto IL_0175;
						}
						return -1;
					}
					else
					{
						if (num < this.ps.charsUsed)
						{
							return -1;
						}
						return -2;
					}
				}
			}
			else if (c != 'l')
			{
				if (c == 'q')
				{
					if (this.ps.charsUsed - num < 5)
					{
						return -2;
					}
					if (chars[num + 1] == 'u' && chars[num + 2] == 'o' && chars[num + 3] == 't' && chars[num + 4] == ';')
					{
						num += 5;
						char c2 = '"';
						goto IL_0175;
					}
					return -1;
				}
			}
			else
			{
				if (this.ps.charsUsed - num < 3)
				{
					return -2;
				}
				if (chars[num + 1] == 't' && chars[num + 2] == ';')
				{
					num += 3;
					char c2 = '<';
					goto IL_0175;
				}
				return -1;
			}
			return -1;
			IL_0175:
			if (expand)
			{
				if (internalSubsetBuilder != null)
				{
					internalSubsetBuilder.Append(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
				}
				char c2;
				this.ps.chars[num - 1] = c2;
			}
			return num;
		}

		private int ParseName()
		{
			int num;
			return this.ParseQName(false, 0, out num);
		}

		private int ParseQName(out int colonPos)
		{
			return this.ParseQName(true, 0, out colonPos);
		}

		private unsafe int ParseQName(bool isQName, int startOffset, out int colonPos)
		{
			int num = -1;
			int num2 = this.ps.charPos + startOffset;
			for (;;)
			{
				char[] array = this.ps.chars;
				if ((this.xmlCharType.charProperties[array[num2]] & 4) == 0)
				{
					if (num2 == this.ps.charsUsed)
					{
						if (this.ReadDataInName(ref num2))
						{
							continue;
						}
						this.Throw(num2, "Xml_UnexpectedEOF", "Name");
					}
					if (array[num2] != ':' || this.supportNamespaces)
					{
						this.Throw(num2, "Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(array[num2]));
					}
				}
				num2++;
				for (;;)
				{
					if ((this.xmlCharType.charProperties[array[num2]] & 8) == 0)
					{
						if (array[num2] == ':')
						{
							break;
						}
						if (num2 != this.ps.charsUsed)
						{
							goto IL_0111;
						}
						if (!this.ReadDataInName(ref num2))
						{
							goto IL_0100;
						}
						array = this.ps.chars;
					}
					else
					{
						num2++;
					}
				}
				if ((num != -1 || !isQName) && this.supportNamespaces)
				{
					this.Throw(num2, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(':'));
				}
				num = num2 - this.ps.charPos;
				num2++;
			}
			IL_0100:
			this.Throw(num2, "Xml_UnexpectedEOF", "Name");
			IL_0111:
			colonPos = ((num == -1) ? (-1) : (this.ps.charPos + num));
			return num2;
		}

		private bool ReadDataInName(ref int pos)
		{
			int num = pos - this.ps.charPos;
			bool flag = this.ReadData() != 0;
			pos = this.ps.charPos + num;
			return flag;
		}

		private string ParseEntityName()
		{
			int num;
			try
			{
				num = this.ParseName();
			}
			catch (XmlException)
			{
				this.Throw("Xml_ErrorParsingEntityName");
				return null;
			}
			if (this.ps.chars[num] != ';')
			{
				this.Throw("Xml_ErrorParsingEntityName");
			}
			string text = this.nameTable.Add(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
			this.ps.charPos = num + 1;
			return text;
		}

		private XmlTextReaderImpl.NodeData AddNode(int nodeIndex, int nodeDepth)
		{
			XmlTextReaderImpl.NodeData nodeData = this.nodes[nodeIndex];
			if (nodeData != null)
			{
				nodeData.depth = nodeDepth;
				return nodeData;
			}
			return this.AllocNode(nodeIndex, nodeDepth);
		}

		private XmlTextReaderImpl.NodeData AllocNode(int nodeIndex, int nodeDepth)
		{
			if (nodeIndex >= this.nodes.Length - 1)
			{
				XmlTextReaderImpl.NodeData[] array = new XmlTextReaderImpl.NodeData[this.nodes.Length * 2];
				Array.Copy(this.nodes, 0, array, 0, this.nodes.Length);
				this.nodes = array;
			}
			XmlTextReaderImpl.NodeData nodeData = this.nodes[nodeIndex];
			if (nodeData == null)
			{
				nodeData = new XmlTextReaderImpl.NodeData();
				this.nodes[nodeIndex] = nodeData;
			}
			nodeData.depth = nodeDepth;
			return nodeData;
		}

		private XmlTextReaderImpl.NodeData AddAttributeNoChecks(string name, int attrDepth)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddNode(this.index + this.attrCount + 1, attrDepth);
			nodeData.SetNamedNode(XmlNodeType.Attribute, this.nameTable.Add(name));
			this.attrCount++;
			return nodeData;
		}

		private XmlTextReaderImpl.NodeData AddAttribute(int endNamePos, int colonPos)
		{
			if (colonPos == -1 || !this.supportNamespaces)
			{
				string text = this.nameTable.Add(this.ps.chars, this.ps.charPos, endNamePos - this.ps.charPos);
				return this.AddAttribute(text, string.Empty, text);
			}
			this.attrNeedNamespaceLookup = true;
			int charPos = this.ps.charPos;
			int num = colonPos - charPos;
			if (num == this.lastPrefix.Length && XmlConvert.StrEqual(this.ps.chars, charPos, num, this.lastPrefix))
			{
				return this.AddAttribute(this.nameTable.Add(this.ps.chars, colonPos + 1, endNamePos - colonPos - 1), this.lastPrefix, null);
			}
			string text2 = this.nameTable.Add(this.ps.chars, charPos, num);
			this.lastPrefix = text2;
			return this.AddAttribute(this.nameTable.Add(this.ps.chars, colonPos + 1, endNamePos - colonPos - 1), text2, null);
		}

		private XmlTextReaderImpl.NodeData AddAttribute(string localName, string prefix, string nameWPrefix)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddNode(this.index + this.attrCount + 1, this.index + 1);
			nodeData.SetNamedNode(XmlNodeType.Attribute, localName, prefix, nameWPrefix);
			int num = 1 << (int)localName[0];
			if ((this.attrHashtable & num) == 0)
			{
				this.attrHashtable |= num;
			}
			else if (this.attrDuplWalkCount < 250)
			{
				this.attrDuplWalkCount++;
				for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
				{
					XmlTextReaderImpl.NodeData nodeData2 = this.nodes[i];
					if (Ref.Equal(nodeData2.localName, nodeData.localName))
					{
						this.attrDuplWalkCount = 250;
						break;
					}
				}
			}
			this.attrCount++;
			return nodeData;
		}

		private void PopElementContext()
		{
			this.namespaceManager.PopScope();
			if (this.curNode.xmlContextPushed)
			{
				this.PopXmlContext();
			}
		}

		private void OnNewLine(int pos)
		{
			this.ps.lineNo = this.ps.lineNo + 1;
			this.ps.lineStartPos = pos - 1;
		}

		private void OnEof()
		{
			this.curNode = this.nodes[0];
			this.curNode.Clear(XmlNodeType.None);
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.Eof;
			this.readState = ReadState.EndOfFile;
			this.reportedEncoding = null;
		}

		private string LookupNamespace(XmlTextReaderImpl.NodeData node)
		{
			string text = this.namespaceManager.LookupNamespace(node.prefix);
			if (text != null)
			{
				return text;
			}
			this.Throw("Xml_UnknownNs", node.prefix, node.LineNo, node.LinePos);
			return null;
		}

		private void AddNamespace(string prefix, string uri, XmlTextReaderImpl.NodeData attr)
		{
			if (uri == "http://www.w3.org/2000/xmlns/")
			{
				if (Ref.Equal(prefix, this.XmlNs))
				{
					this.Throw("Xml_XmlnsPrefix", attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
				}
				else
				{
					this.Throw("Xml_NamespaceDeclXmlXmlns", prefix, attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
				}
			}
			else if (uri == "http://www.w3.org/XML/1998/namespace" && !Ref.Equal(prefix, this.Xml) && !this.v1Compat)
			{
				this.Throw("Xml_NamespaceDeclXmlXmlns", prefix, attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
			}
			if (uri.Length == 0 && prefix.Length > 0)
			{
				this.Throw("Xml_BadNamespaceDecl", attr.lineInfo.lineNo, attr.lineInfo.linePos);
			}
			try
			{
				this.namespaceManager.AddNamespace(prefix, uri);
			}
			catch (ArgumentException ex)
			{
				this.ReThrow(ex, attr.lineInfo.lineNo, attr.lineInfo.linePos);
			}
		}

		private void ResetAttributes()
		{
			if (this.fullAttrCleanup)
			{
				this.FullAttributeCleanup();
			}
			this.curAttrIndex = -1;
			this.attrCount = 0;
			this.attrHashtable = 0;
			this.attrDuplWalkCount = 0;
		}

		private void FullAttributeCleanup()
		{
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
				nodeData.nextAttrValueChunk = null;
				nodeData.IsDefaultAttribute = false;
			}
			this.fullAttrCleanup = false;
		}

		private void PushXmlContext()
		{
			this.xmlContext = new XmlTextReaderImpl.XmlContext(this.xmlContext);
			this.curNode.xmlContextPushed = true;
		}

		private void PopXmlContext()
		{
			this.xmlContext = this.xmlContext.previousContext;
			this.curNode.xmlContextPushed = false;
		}

		private XmlNodeType GetWhitespaceType()
		{
			if (this.whitespaceHandling != WhitespaceHandling.None)
			{
				if (this.xmlContext.xmlSpace == XmlSpace.Preserve)
				{
					return XmlNodeType.SignificantWhitespace;
				}
				if (this.whitespaceHandling == WhitespaceHandling.All)
				{
					return XmlNodeType.Whitespace;
				}
			}
			return XmlNodeType.None;
		}

		private XmlNodeType GetTextNodeType(int orChars)
		{
			if (orChars > 32)
			{
				return XmlNodeType.Text;
			}
			return this.GetWhitespaceType();
		}

		private bool PushExternalEntity(SchemaEntity entity, int entityId)
		{
			if (!this.IsResolverNull)
			{
				Uri uri = ((entity.BaseURI.Length > 0) ? this.xmlResolver.ResolveUri(null, entity.BaseURI) : null);
				Uri uri2 = this.xmlResolver.ResolveUri(uri, entity.Url);
				Stream stream = null;
				try
				{
					stream = this.OpenStream(uri2);
				}
				catch (Exception ex)
				{
					if (this.v1Compat)
					{
						throw;
					}
					this.Throw(new XmlException("Xml_ErrorOpeningExternalEntity", new string[]
					{
						uri2.ToString(),
						ex.Message
					}, ex, 0, 0));
				}
				if (stream == null)
				{
					this.Throw("Xml_CannotResolveEntity", entity.Name.Name);
				}
				this.PushParsingState();
				if (this.v1Compat)
				{
					this.InitStreamInput(uri2, stream, null);
				}
				else
				{
					this.InitStreamInput(uri2, stream, null);
				}
				this.ps.entity = entity;
				this.ps.entityId = entityId;
				entity.IsProcessed = true;
				int charPos = this.ps.charPos;
				if (this.v1Compat)
				{
					this.EatWhitespaces(null);
				}
				if (!this.ParseXmlDeclaration(true))
				{
					this.ps.charPos = charPos;
				}
				return true;
			}
			Encoding encoding = this.ps.encoding;
			this.PushParsingState();
			this.InitStringInput(entity.Url, encoding, string.Empty);
			this.ps.entity = entity;
			this.ps.entityId = entityId;
			this.RegisterConsumedCharacters(0L, true);
			return false;
		}

		private void PushInternalEntity(SchemaEntity entity, int entityId)
		{
			Encoding encoding = this.ps.encoding;
			this.PushParsingState();
			this.InitStringInput((entity.DeclaredURI != null) ? entity.DeclaredURI : string.Empty, encoding, entity.Text);
			this.ps.entity = entity;
			this.ps.entityId = entityId;
			this.ps.lineNo = entity.Line;
			this.ps.lineStartPos = -entity.Pos - 1;
			this.ps.eolNormalized = true;
			entity.IsProcessed = true;
			this.RegisterConsumedCharacters((long)entity.Text.Length, true);
		}

		private void PopEntity()
		{
			if (this.ps.entity != null)
			{
				this.ps.entity.IsProcessed = false;
			}
			if (this.ps.stream != null)
			{
				this.ps.stream.Close();
			}
			this.PopParsingState();
			this.curNode.entityId = this.ps.entityId;
		}

		private void PushParsingState()
		{
			if (this.parsingStatesStack == null)
			{
				this.parsingStatesStack = new XmlTextReaderImpl.ParsingState[2];
			}
			else if (this.parsingStatesStackTop + 1 == this.parsingStatesStack.Length)
			{
				XmlTextReaderImpl.ParsingState[] array = new XmlTextReaderImpl.ParsingState[this.parsingStatesStack.Length * 2];
				Array.Copy(this.parsingStatesStack, 0, array, 0, this.parsingStatesStack.Length);
				this.parsingStatesStack = array;
			}
			this.parsingStatesStackTop++;
			this.parsingStatesStack[this.parsingStatesStackTop] = this.ps;
			this.ps.Clear();
		}

		private void PopParsingState()
		{
			this.ps.Close(true);
			this.ps = this.parsingStatesStack[this.parsingStatesStackTop--];
		}

		private void InitIncrementalRead(IncrementalReadDecoder decoder)
		{
			this.ResetAttributes();
			decoder.Reset();
			this.incReadDecoder = decoder;
			this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
			this.incReadDepth = 1;
			this.incReadLeftStartPos = this.ps.charPos;
			this.incReadLeftEndPos = this.ps.charPos;
			this.incReadLineInfo.Set(this.ps.LineNo, this.ps.LinePos);
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InIncrementalRead;
		}

		private int IncrementalRead(Array array, int index, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "buffer" : "array");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "count" : "len");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "index" : "offset");
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "count" : "len");
			}
			if (count == 0)
			{
				return 0;
			}
			this.curNode.lineInfo = this.incReadLineInfo;
			this.incReadDecoder.SetNextOutputBuffer(array, index, count);
			this.IncrementalRead();
			return this.incReadDecoder.DecodedCount;
		}

		private unsafe int IncrementalRead()
		{
			int num = 0;
			int num3;
			int num4;
			int num5;
			int num7;
			for (;;)
			{
				int num2 = this.incReadLeftEndPos - this.incReadLeftStartPos;
				if (num2 > 0)
				{
					try
					{
						num3 = this.incReadDecoder.Decode(this.ps.chars, this.incReadLeftStartPos, num2);
					}
					catch (XmlException ex)
					{
						this.ReThrow(ex, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
						return 0;
					}
					if (num3 < num2)
					{
						break;
					}
					this.incReadLeftStartPos = 0;
					this.incReadLeftEndPos = 0;
					this.incReadLineInfo.linePos = this.incReadLineInfo.linePos + num3;
					if (this.incReadDecoder.IsFull)
					{
						return num3;
					}
				}
				num4 = 0;
				num5 = 0;
				int num10;
				for (;;)
				{
					switch (this.incReadState)
					{
					case XmlTextReaderImpl.IncrementalReadState.Text:
					case XmlTextReaderImpl.IncrementalReadState.StartTag:
					case XmlTextReaderImpl.IncrementalReadState.Attributes:
					case XmlTextReaderImpl.IncrementalReadState.AttributeValue:
						goto IL_01E2;
					case XmlTextReaderImpl.IncrementalReadState.PI:
						if (this.ParsePIValue(out num4, out num5))
						{
							this.ps.charPos = this.ps.charPos - 2;
							this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						}
						break;
					case XmlTextReaderImpl.IncrementalReadState.CDATA:
						if (this.ParseCDataOrComment(XmlNodeType.CDATA, out num4, out num5))
						{
							this.ps.charPos = this.ps.charPos - 3;
							this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						}
						break;
					case XmlTextReaderImpl.IncrementalReadState.Comment:
						if (this.ParseCDataOrComment(XmlNodeType.Comment, out num4, out num5))
						{
							this.ps.charPos = this.ps.charPos - 3;
							this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						}
						break;
					case XmlTextReaderImpl.IncrementalReadState.ReadData:
						if (this.ReadData() == 0)
						{
							this.ThrowUnclosedElements();
						}
						this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						num4 = this.ps.charPos;
						num5 = num4;
						goto IL_01E2;
					case XmlTextReaderImpl.IncrementalReadState.EndElement:
						goto IL_0182;
					case XmlTextReaderImpl.IncrementalReadState.End:
						return num;
					default:
						goto IL_01E2;
					}
					IL_06DB:
					int num6 = num5 - num4;
					if (num6 <= 0)
					{
						continue;
					}
					try
					{
						num7 = this.incReadDecoder.Decode(this.ps.chars, num4, num6);
					}
					catch (XmlException ex2)
					{
						this.ReThrow(ex2, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
						return 0;
					}
					num += num7;
					if (this.incReadDecoder.IsFull)
					{
						goto Block_51;
					}
					continue;
					IL_01E2:
					char[] array = this.ps.chars;
					num4 = this.ps.charPos;
					num5 = num4;
					int num8;
					for (;;)
					{
						this.incReadLineInfo.Set(this.ps.LineNo, this.ps.LinePos);
						if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.Attributes)
						{
							char c;
							while ((this.xmlCharType.charProperties[c = array[num5]] & 128) != 0)
							{
								if (c == '/')
								{
									break;
								}
								num5++;
							}
						}
						else
						{
							while ((this.xmlCharType.charProperties[array[num5]] & 128) != 0)
							{
								num5++;
							}
						}
						if (array[num5] == '&' || array[num5] == '\t')
						{
							num5++;
						}
						else
						{
							if (num5 - num4 > 0)
							{
								break;
							}
							char c2 = array[num5];
							if (c2 <= '"')
							{
								if (c2 == '\n')
								{
									num5++;
									this.OnNewLine(num5);
									continue;
								}
								if (c2 == '\r')
								{
									if (array[num5 + 1] == '\n')
									{
										num5 += 2;
									}
									else
									{
										if (num5 + 1 >= this.ps.charsUsed)
										{
											goto IL_06C7;
										}
										num5++;
									}
									this.OnNewLine(num5);
									continue;
								}
								if (c2 != '"')
								{
									goto IL_06AD;
								}
							}
							else if (c2 != '\'')
							{
								if (c2 == '/')
								{
									if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.Attributes)
									{
										if (this.ps.charsUsed - num5 < 2)
										{
											goto IL_06C7;
										}
										if (array[num5 + 1] == '>')
										{
											this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
											this.incReadDepth--;
										}
									}
									num5++;
									continue;
								}
								switch (c2)
								{
								case '<':
								{
									if (this.incReadState != XmlTextReaderImpl.IncrementalReadState.Text)
									{
										num5++;
										continue;
									}
									if (this.ps.charsUsed - num5 < 2)
									{
										goto IL_06C7;
									}
									char c3 = array[num5 + 1];
									if (c3 != '!')
									{
										if (c3 != '/')
										{
											if (c3 == '?')
											{
												goto Block_29;
											}
											int num9;
											num8 = this.ParseQName(true, 1, out num9);
											if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos + 1, num8 - this.ps.charPos - 1, this.curNode.localName) && (this.ps.chars[num8] == '>' || this.ps.chars[num8] == '/' || this.xmlCharType.IsWhiteSpace(this.ps.chars[num8])))
											{
												goto IL_05B1;
											}
											num5 = num8;
											num4 = this.ps.charPos;
											array = this.ps.chars;
											continue;
										}
										else
										{
											int num11;
											num10 = this.ParseQName(true, 2, out num11);
											if (!XmlConvert.StrEqual(array, this.ps.charPos + 2, num10 - this.ps.charPos - 2, this.curNode.GetNameWPrefix(this.nameTable)) || (this.ps.chars[num10] != '>' && !this.xmlCharType.IsWhiteSpace(this.ps.chars[num10])))
											{
												num5 = num10;
												continue;
											}
											if (--this.incReadDepth > 0)
											{
												num5 = num10 + 1;
												continue;
											}
											goto IL_04AE;
										}
									}
									else
									{
										if (this.ps.charsUsed - num5 < 4)
										{
											goto IL_06C7;
										}
										if (array[num5 + 2] == '-' && array[num5 + 3] == '-')
										{
											goto Block_32;
										}
										if (this.ps.charsUsed - num5 < 9)
										{
											goto IL_06C7;
										}
										if (XmlConvert.StrEqual(array, num5 + 2, 7, "[CDATA["))
										{
											goto Block_34;
										}
										continue;
									}
									break;
								}
								case '=':
									goto IL_06AD;
								case '>':
									if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.Attributes)
									{
										this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
									}
									num5++;
									continue;
								default:
									goto IL_06AD;
								}
							}
							switch (this.incReadState)
							{
							case XmlTextReaderImpl.IncrementalReadState.Attributes:
								this.curNode.quoteChar = array[num5];
								this.incReadState = XmlTextReaderImpl.IncrementalReadState.AttributeValue;
								break;
							case XmlTextReaderImpl.IncrementalReadState.AttributeValue:
								if (array[num5] == this.curNode.quoteChar)
								{
									this.incReadState = XmlTextReaderImpl.IncrementalReadState.Attributes;
								}
								break;
							}
							num5++;
							continue;
							IL_06AD:
							if (num5 == this.ps.charsUsed)
							{
								goto IL_06C7;
							}
							num5++;
						}
					}
					IL_06CE:
					this.ps.charPos = num5;
					goto IL_06DB;
					IL_06C7:
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadData;
					goto IL_06CE;
					IL_05B1:
					this.incReadDepth++;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.Attributes;
					num5 = num8;
					goto IL_06CE;
					Block_34:
					num5 += 9;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.CDATA;
					goto IL_06CE;
					Block_32:
					num5 += 4;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.Comment;
					goto IL_06CE;
					Block_29:
					num5 += 2;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.PI;
					goto IL_06CE;
				}
				IL_04AE:
				this.ps.charPos = num10;
				if (this.xmlCharType.IsWhiteSpace(this.ps.chars[num10]))
				{
					this.EatWhitespaces(null);
				}
				if (this.ps.chars[this.ps.charPos] != '>')
				{
					this.ThrowUnexpectedToken(">");
				}
				this.ps.charPos = this.ps.charPos + 1;
				this.incReadState = XmlTextReaderImpl.IncrementalReadState.EndElement;
			}
			this.incReadLeftStartPos += num3;
			this.incReadLineInfo.linePos = this.incReadLineInfo.linePos + num3;
			return num3;
			IL_0182:
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopElementContext;
			this.nextParsingFunction = ((this.index > 0 || this.fragmentType != XmlNodeType.Document) ? XmlTextReaderImpl.ParsingFunction.ElementContent : XmlTextReaderImpl.ParsingFunction.DocumentContent);
			this.outerReader.Read();
			this.incReadState = XmlTextReaderImpl.IncrementalReadState.End;
			return num;
			Block_51:
			this.incReadLeftStartPos = num4 + num7;
			this.incReadLeftEndPos = num5;
			this.incReadLineInfo.linePos = this.incReadLineInfo.linePos + num7;
			return num;
		}

		private void FinishIncrementalRead()
		{
			this.incReadDecoder = new IncrementalReadDummyDecoder();
			this.IncrementalRead();
			this.incReadDecoder = null;
		}

		private bool ParseFragmentAttribute()
		{
			if (this.curNode.type == XmlNodeType.None)
			{
				this.curNode.type = XmlNodeType.Attribute;
				this.curAttrIndex = 0;
				this.ParseAttributeValueSlow(this.ps.charPos, ' ', this.curNode);
			}
			else
			{
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InReadAttributeValue;
			}
			if (this.ReadAttributeValue())
			{
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.FragmentAttribute;
				return true;
			}
			this.OnEof();
			return false;
		}

		private unsafe bool ParseAttributeValueChunk()
		{
			char[] array = this.ps.chars;
			int num = this.ps.charPos;
			this.curNode = this.AddNode(this.index + this.attrCount + 1, this.index + 2);
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			if (this.emptyEntityInAttributeResolved)
			{
				this.curNode.SetValueNode(XmlNodeType.Text, string.Empty);
				this.emptyEntityInAttributeResolved = false;
				return true;
			}
			for (;;)
			{
				if ((this.xmlCharType.charProperties[array[num]] & 128) == 0)
				{
					char c = array[num];
					if (c <= '"')
					{
						switch (c)
						{
						case '\t':
						case '\n':
							if (this.normalize)
							{
								array[num] = ' ';
							}
							num++;
							continue;
						case '\v':
						case '\f':
							goto IL_0258;
						case '\r':
							num++;
							continue;
						default:
							if (c != '"')
							{
								goto IL_0258;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '&':
							if (num - this.ps.charPos > 0)
							{
								this.stringBuilder.Append(array, this.ps.charPos, num - this.ps.charPos);
							}
							this.ps.charPos = num;
							switch (this.HandleEntityReference(true, XmlTextReaderImpl.EntityExpandType.OnlyCharacter, out num))
							{
							case XmlTextReaderImpl.EntityType.CharacterDec:
							case XmlTextReaderImpl.EntityType.CharacterHex:
							case XmlTextReaderImpl.EntityType.CharacterNamed:
								array = this.ps.chars;
								if (this.normalize && this.xmlCharType.IsWhiteSpace(array[this.ps.charPos]) && num - this.ps.charPos == 1)
								{
									array[this.ps.charPos] = ' ';
								}
								break;
							case XmlTextReaderImpl.EntityType.Unexpanded:
								goto IL_01F8;
							}
							array = this.ps.chars;
							continue;
						case '\'':
							break;
						default:
							switch (c)
							{
							case '<':
								this.Throw(num, "Xml_BadAttributeChar", XmlException.BuildCharExceptionStr('<'));
								goto IL_02B3;
							case '=':
								goto IL_0258;
							case '>':
								break;
							default:
								goto IL_0258;
							}
							break;
						}
					}
					num++;
					continue;
					IL_0258:
					if (num != this.ps.charsUsed)
					{
						char c2 = array[num];
						if (c2 >= '\ud800' && c2 <= '\udbff')
						{
							if (num + 1 == this.ps.charsUsed)
							{
								goto IL_02B3;
							}
							num++;
							if (array[num] >= '\udc00' && array[num] <= '\udfff')
							{
								num++;
								continue;
							}
						}
						this.ThrowInvalidChar(num, c2);
					}
					IL_02B3:
					if (num - this.ps.charPos > 0)
					{
						this.stringBuilder.Append(array, this.ps.charPos, num - this.ps.charPos);
						this.ps.charPos = num;
					}
					if (this.ReadData() == 0)
					{
						if (this.stringBuilder.Length > 0)
						{
							goto IL_0337;
						}
						if (this.HandleEntityEnd(false))
						{
							goto Block_24;
						}
					}
					num = this.ps.charPos;
					array = this.ps.chars;
				}
				else
				{
					num++;
				}
			}
			IL_01F8:
			if (this.stringBuilder.Length == 0)
			{
				XmlTextReaderImpl.NodeData nodeData = this.curNode;
				nodeData.lineInfo.linePos = nodeData.lineInfo.linePos + 1;
				this.ps.charPos = this.ps.charPos + 1;
				this.curNode.SetNamedNode(XmlNodeType.EntityReference, this.ParseEntityName());
				return true;
			}
			goto IL_0337;
			Block_24:
			this.SetupEndEntityNodeInAttribute();
			return true;
			IL_0337:
			if (num - this.ps.charPos > 0)
			{
				this.stringBuilder.Append(array, this.ps.charPos, num - this.ps.charPos);
				this.ps.charPos = num;
			}
			this.curNode.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
			return true;
		}

		private void ParseXmlDeclarationFragment()
		{
			try
			{
				this.ParseXmlDeclaration(false);
			}
			catch (XmlException ex)
			{
				this.ReThrow(ex, ex.LineNumber, ex.LinePosition - 6);
			}
		}

		private void ThrowUnexpectedToken(int pos, string expectedToken)
		{
			this.ThrowUnexpectedToken(pos, expectedToken, null);
		}

		private void ThrowUnexpectedToken(string expectedToken1)
		{
			this.ThrowUnexpectedToken(expectedToken1, null);
		}

		private void ThrowUnexpectedToken(int pos, string expectedToken1, string expectedToken2)
		{
			this.ps.charPos = pos;
			this.ThrowUnexpectedToken(expectedToken1, expectedToken2);
		}

		private void ThrowUnexpectedToken(string expectedToken1, string expectedToken2)
		{
			string text = this.ParseUnexpectedToken();
			if (expectedToken2 != null)
			{
				this.Throw("Xml_UnexpectedTokens2", new string[] { text, expectedToken1, expectedToken2 });
				return;
			}
			this.Throw("Xml_UnexpectedTokenEx", new string[] { text, expectedToken1 });
		}

		private string ParseUnexpectedToken(int pos)
		{
			this.ps.charPos = pos;
			return this.ParseUnexpectedToken();
		}

		private string ParseUnexpectedToken()
		{
			if (this.xmlCharType.IsNCNameChar(this.ps.chars[this.ps.charPos]))
			{
				int num = this.ps.charPos + 1;
				while (this.xmlCharType.IsNCNameChar(this.ps.chars[num]))
				{
					num++;
				}
				return new string(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
			}
			return new string(this.ps.chars, this.ps.charPos, 1);
		}

		private int GetIndexOfAttributeWithoutPrefix(string name)
		{
			name = this.nameTable.Get(name);
			if (name == null)
			{
				return -1;
			}
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].localName, name) && this.nodes[i].prefix.Length == 0)
				{
					return i;
				}
			}
			return -1;
		}

		private int GetIndexOfAttributeWithPrefix(string name)
		{
			name = this.nameTable.Add(name);
			if (name == null)
			{
				return -1;
			}
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].GetNameWPrefix(this.nameTable), name))
				{
					return i;
				}
			}
			return -1;
		}

		private Stream OpenStream(Uri uri)
		{
			return (Stream)this.xmlResolver.GetEntity(uri, null, typeof(Stream));
		}

		private bool ZeroEndingStream(int pos)
		{
			if (this.v1Compat && pos == this.ps.charsUsed - 1 && this.ps.chars[pos] == '\0' && this.ReadData() == 0 && this.ps.isStreamEof)
			{
				this.ps.charsUsed = this.ps.charsUsed - 1;
				return true;
			}
			return false;
		}

		private void ParseDtdFromParserContext()
		{
			this.dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(this.fragmentParserContext.BaseURI, this.fragmentParserContext.DocTypeName, this.fragmentParserContext.PublicId, this.fragmentParserContext.SystemId, this.fragmentParserContext.InternalSubset, this);
			this.dtdParserProxy.Parse(false);
			SchemaInfo dtdSchemaInfo = this.dtdParserProxy.DtdSchemaInfo;
			if ((this.validatingReaderCompatFlag || !this.v1Compat) && (dtdSchemaInfo.HasDefaultAttributes || dtdSchemaInfo.HasNonCDataAttributes))
			{
				this.addDefaultAttributesAndNormalize = true;
				this.qName = new XmlQualifiedName();
			}
		}

		private bool InitReadContentAsBinary()
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
			{
				throw new InvalidOperationException(Res.GetString("Xml_MixingReadValueChunkWithBinary"));
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				throw new InvalidOperationException(Res.GetString("Xml_MixingV1StreamingWithV2Binary"));
			}
			if (!XmlReader.IsTextualNode(this.curNode.type) && !this.MoveToNextContentNode(false))
			{
				return false;
			}
			this.SetupReadContentAsBinaryState(XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary);
			this.incReadLineInfo.Set(this.curNode.LineNo, this.curNode.LinePos);
			return true;
		}

		private bool InitReadElementContentAsBinary()
		{
			bool isEmptyElement = this.curNode.IsEmptyElement;
			this.outerReader.Read();
			if (isEmptyElement)
			{
				return false;
			}
			if (!this.MoveToNextContentNode(false))
			{
				if (this.curNode.type != XmlNodeType.EndElement)
				{
					this.Throw("Xml_InvalidNodeType", this.curNode.type.ToString());
				}
				this.outerReader.Read();
				return false;
			}
			this.SetupReadContentAsBinaryState(XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary);
			this.incReadLineInfo.Set(this.curNode.LineNo, this.curNode.LinePos);
			return true;
		}

		private bool MoveToNextContentNode(bool moveIfOnContentNode)
		{
			for (;;)
			{
				switch (this.curNode.type)
				{
				case XmlNodeType.Attribute:
					goto IL_0052;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					if (!moveIfOnContentNode)
					{
						return true;
					}
					goto IL_006B;
				case XmlNodeType.EntityReference:
					this.outerReader.ResolveEntity();
					goto IL_006B;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.EndEntity:
					goto IL_006B;
				}
				break;
				IL_006B:
				moveIfOnContentNode = false;
				if (!this.outerReader.Read())
				{
					return false;
				}
			}
			return false;
			IL_0052:
			return !moveIfOnContentNode;
		}

		private void SetupReadContentAsBinaryState(XmlTextReaderImpl.ParsingFunction inReadBinaryFunction)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.PartialTextValue)
			{
				this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue;
			}
			else
			{
				this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue;
				this.nextNextParsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = this.parsingFunction;
			}
			this.readValueOffset = 0;
			this.parsingFunction = inReadBinaryFunction;
		}

		private void SetupFromParserContext(XmlParserContext context, XmlReaderSettings settings)
		{
			XmlNameTable xmlNameTable = settings.NameTable;
			this.nameTableFromSettings = xmlNameTable != null;
			if (context.NamespaceManager != null)
			{
				if (xmlNameTable != null && xmlNameTable != context.NamespaceManager.NameTable)
				{
					throw new XmlException("Xml_NametableMismatch");
				}
				this.namespaceManager = context.NamespaceManager;
				this.xmlContext.defaultNamespace = this.namespaceManager.LookupNamespace(string.Empty);
				xmlNameTable = this.namespaceManager.NameTable;
			}
			else if (context.NameTable != null)
			{
				if (xmlNameTable != null && xmlNameTable != context.NameTable)
				{
					throw new XmlException("Xml_NametableMismatch");
				}
				xmlNameTable = context.NameTable;
			}
			else if (xmlNameTable == null)
			{
				xmlNameTable = new NameTable();
			}
			this.nameTable = xmlNameTable;
			if (this.namespaceManager == null)
			{
				this.namespaceManager = new XmlNamespaceManager(xmlNameTable);
			}
			this.xmlContext.xmlSpace = context.XmlSpace;
			this.xmlContext.xmlLang = context.XmlLang;
		}

		internal SchemaInfo DtdSchemaInfo
		{
			get
			{
				if (this.dtdParserProxy != null)
				{
					return this.dtdParserProxy.DtdSchemaInfo;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(this, value);
					if ((this.validatingReaderCompatFlag || !this.v1Compat) && (value.HasDefaultAttributes || value.HasNonCDataAttributes))
					{
						this.addDefaultAttributesAndNormalize = true;
						this.qName = new XmlQualifiedName();
						return;
					}
				}
				else
				{
					this.dtdParserProxy = null;
				}
			}
		}

		internal bool XmlValidatingReaderCompatibilityMode
		{
			set
			{
				this.validatingReaderCompatFlag = value;
				if (value)
				{
					this.nameTable.Add("http://www.w3.org/2001/XMLSchema");
					this.nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
					this.nameTable.Add("urn:schemas-microsoft-com:datatypes");
				}
			}
		}

		internal ValidationEventHandler ValidationEventHandler
		{
			set
			{
				this.validationEventHandler = value;
			}
		}

		internal XmlNodeType FragmentType
		{
			get
			{
				return this.fragmentType;
			}
		}

		internal void ChangeCurrentNodeType(XmlNodeType newNodeType)
		{
			this.curNode.type = newNodeType;
		}

		internal XmlResolver GetResolver()
		{
			if (this.IsResolverNull)
			{
				return null;
			}
			return this.xmlResolver;
		}

		internal object InternalSchemaType
		{
			get
			{
				return this.curNode.schemaType;
			}
			set
			{
				this.curNode.schemaType = value;
			}
		}

		internal object InternalTypedValue
		{
			get
			{
				return this.curNode.typedValue;
			}
			set
			{
				this.curNode.typedValue = value;
			}
		}

		internal bool StandAlone
		{
			get
			{
				return this.standalone;
			}
		}

		internal override XmlNamespaceManager NamespaceManager
		{
			get
			{
				return this.namespaceManager;
			}
		}

		internal bool V1Compat
		{
			get
			{
				return this.v1Compat;
			}
		}

		internal ConformanceLevel V1ComformanceLevel
		{
			get
			{
				if (this.fragmentType != XmlNodeType.Element)
				{
					return ConformanceLevel.Document;
				}
				return ConformanceLevel.Fragment;
			}
		}

		internal bool AddDefaultAttribute(SchemaAttDef attrDef, bool definedInDtd)
		{
			return this.AddDefaultAttribute(attrDef, definedInDtd, null);
		}

		private bool AddDefaultAttribute(SchemaAttDef attrDef, bool definedInDtd, XmlTextReaderImpl.NodeData[] nameSortedNodeData)
		{
			string text = attrDef.Name.Name;
			string text2 = attrDef.Prefix;
			string text3 = attrDef.Name.Namespace;
			if (definedInDtd)
			{
				if (text2.Length > 0)
				{
					this.attrNeedNamespaceLookup = true;
				}
			}
			else
			{
				text3 = this.nameTable.Add(text3);
				if (text2.Length == 0 && text3.Length > 0)
				{
					text2 = this.namespaceManager.LookupPrefix(text3);
					if (text2 == null)
					{
						text2 = string.Empty;
					}
				}
			}
			text = this.nameTable.Add(text);
			text2 = this.nameTable.Add(text2);
			if (definedInDtd && nameSortedNodeData != null)
			{
				if (Array.BinarySearch(nameSortedNodeData, attrDef, XmlTextReaderImpl.SchemaAttDefToNodeDataComparer.Instance) >= 0)
				{
					return false;
				}
			}
			else
			{
				for (int i = this.index + 1; i < this.index + 1 + this.attrCount; i++)
				{
					if (this.nodes[i].localName == text && (this.nodes[i].prefix == text2 || (this.nodes[i].ns == text3 && text3 != null)))
					{
						return false;
					}
				}
			}
			if (definedInDtd && this.DtdValidation && !attrDef.DefaultValueChecked)
			{
				attrDef.CheckDefaultValue(this.dtdParserProxy.DtdSchemaInfo, this.dtdParserProxy);
			}
			XmlTextReaderImpl.NodeData nodeData = this.AddAttribute(text, text2, (text2.Length > 0) ? null : text);
			if (!definedInDtd)
			{
				nodeData.ns = text3;
			}
			nodeData.SetValue(attrDef.DefaultValueExpanded);
			nodeData.IsDefaultAttribute = true;
			nodeData.schemaType = ((attrDef.SchemaType == null) ? attrDef.Datatype : attrDef.SchemaType);
			nodeData.typedValue = attrDef.DefaultValueTyped;
			nodeData.lineInfo.Set(attrDef.LineNum, attrDef.LinePos);
			nodeData.lineInfo2.Set(attrDef.ValueLineNum, attrDef.ValueLinePos);
			if (nodeData.prefix.Length == 0)
			{
				if (Ref.Equal(nodeData.localName, this.XmlNs))
				{
					this.OnDefaultNamespaceDecl(nodeData);
					if (!definedInDtd && this.nodes[this.index].prefix.Length == 0)
					{
						this.nodes[this.index].ns = this.xmlContext.defaultNamespace;
					}
				}
			}
			else if (Ref.Equal(nodeData.prefix, this.XmlNs))
			{
				this.OnNamespaceDecl(nodeData);
				if (!definedInDtd)
				{
					string localName = nodeData.localName;
					for (int j = this.index; j < this.index + this.attrCount + 1; j++)
					{
						if (this.nodes[j].prefix.Equals(localName))
						{
							this.nodes[j].ns = this.namespaceManager.LookupNamespace(localName);
						}
					}
				}
			}
			else if (attrDef.Reserved != SchemaAttDef.Reserve.None)
			{
				this.OnXmlReservedAttribute(nodeData);
			}
			this.fullAttrCleanup = true;
			return true;
		}

		internal bool DisableUndeclaredEntityCheck
		{
			set
			{
				this.disableUndeclaredEntityCheck = value;
			}
		}

		private int ReadContentAsBinary(byte[] buffer, int index, int count)
		{
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End)
			{
				return 0;
			}
			this.incReadDecoder.SetNextOutputBuffer(buffer, index, count);
			int num;
			int num2;
			int num3;
			XmlTextReaderImpl.ParsingFunction parsingFunction;
			for (;;)
			{
				num = 0;
				try
				{
					num = this.curNode.CopyToBinary(this.incReadDecoder, this.readValueOffset);
				}
				catch (XmlException ex)
				{
					this.curNode.AdjustLineInfo(this.readValueOffset, this.ps.eolNormalized, ref this.incReadLineInfo);
					this.ReThrow(ex, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
				}
				this.readValueOffset += num;
				if (this.incReadDecoder.IsFull)
				{
					break;
				}
				if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue)
				{
					this.curNode.SetValue(string.Empty);
					bool flag = false;
					num2 = 0;
					num3 = 0;
					while (!this.incReadDecoder.IsFull && !flag)
					{
						int num4 = 0;
						this.incReadLineInfo.Set(this.ps.LineNo, this.ps.LinePos);
						flag = this.ParseText(out num2, out num3, ref num4);
						try
						{
							num = this.incReadDecoder.Decode(this.ps.chars, num2, num3 - num2);
						}
						catch (XmlException ex2)
						{
							this.ReThrow(ex2, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
						}
						num2 += num;
					}
					this.incReadState = (flag ? XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue : XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue);
					this.readValueOffset = 0;
					if (this.incReadDecoder.IsFull)
					{
						goto Block_8;
					}
				}
				parsingFunction = this.parsingFunction;
				this.parsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = this.nextNextParsingFunction;
				if (!this.MoveToNextContentNode(true))
				{
					goto Block_9;
				}
				this.SetupReadContentAsBinaryState(parsingFunction);
				this.incReadLineInfo.Set(this.curNode.LineNo, this.curNode.LinePos);
			}
			return this.incReadDecoder.DecodedCount;
			Block_8:
			this.curNode.SetValue(this.ps.chars, num2, num3 - num2);
			XmlTextReaderImpl.AdjustLineInfo(this.ps.chars, num2 - num, num2, this.ps.eolNormalized, ref this.incReadLineInfo);
			this.curNode.SetLineInfo(this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
			return this.incReadDecoder.DecodedCount;
			Block_9:
			this.SetupReadContentAsBinaryState(parsingFunction);
			this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End;
			return this.incReadDecoder.DecodedCount;
		}

		private int ReadElementContentAsBinary(byte[] buffer, int index, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			int num = this.ReadContentAsBinary(buffer, index, count);
			if (num > 0)
			{
				return num;
			}
			if (this.curNode.type != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.curNode.type.ToString(), this);
			}
			this.parsingFunction = this.nextParsingFunction;
			this.nextParsingFunction = this.nextNextParsingFunction;
			this.outerReader.Read();
			return 0;
		}

		private void InitBase64Decoder()
		{
			if (this.base64Decoder == null)
			{
				this.base64Decoder = new Base64Decoder();
			}
			else
			{
				this.base64Decoder.Reset();
			}
			this.incReadDecoder = this.base64Decoder;
		}

		private void InitBinHexDecoder()
		{
			if (this.binHexDecoder == null)
			{
				this.binHexDecoder = new BinHexDecoder();
			}
			else
			{
				this.binHexDecoder.Reset();
			}
			this.incReadDecoder = this.binHexDecoder;
		}

		private bool UriEqual(Uri uri1, string uri1Str, string uri2Str, XmlResolver resolver)
		{
			if (uri1 == null || resolver == null)
			{
				return uri1Str == uri2Str;
			}
			Uri uri2 = resolver.ResolveUri(null, uri2Str);
			return uri1.Equals(uri2);
		}

		private void RegisterConsumedCharacters(long characters, bool inEntityReference)
		{
			if (this.maxCharactersInDocument > 0L)
			{
				long num = this.charactersInDocument + characters;
				if (num < this.charactersInDocument)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[] { "MaxCharactersInDocument", "" });
				}
				else
				{
					this.charactersInDocument = num;
				}
				if (this.charactersInDocument > this.maxCharactersInDocument)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[] { "MaxCharactersInDocument", "" });
				}
			}
			if (this.maxCharactersFromEntities > 0L && inEntityReference)
			{
				long num2 = this.charactersFromEntities + characters;
				if (num2 < this.charactersFromEntities)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[] { "MaxCharactersFromEntities", "" });
				}
				else
				{
					this.charactersFromEntities = num2;
				}
				if (this.charactersFromEntities > this.maxCharactersFromEntities && XmlTextReaderSection.LimitCharactersFromEntities)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[] { "MaxCharactersFromEntities", "" });
				}
			}
		}

		internal static void AdjustLineInfo(char[] chars, int startPos, int endPos, bool isNormalized, ref LineInfo lineInfo)
		{
			int num = -1;
			for (int i = startPos; i < endPos; i++)
			{
				char c = chars[i];
				if (c != '\n')
				{
					if (c == '\r')
					{
						if (!isNormalized)
						{
							lineInfo.lineNo++;
							num = i;
							if (i + 1 < endPos && chars[i + 1] == '\n')
							{
								i++;
								num++;
							}
						}
					}
				}
				else
				{
					lineInfo.lineNo++;
					num = i;
				}
			}
			if (num >= 0)
			{
				lineInfo.linePos = endPos - num;
			}
		}

		private const int MaxBytesToMove = 128;

		private const int ApproxXmlDeclLength = 80;

		private const int NodesInitialSize = 8;

		private const int InitialAttributesCount = 4;

		private const int InitialParsingStateStackSize = 2;

		private const int InitialParsingStatesDepth = 2;

		private const int DtdChidrenInitialSize = 2;

		private const int MaxByteSequenceLen = 6;

		private const int MaxAttrDuplWalkCount = 250;

		private const int MinWhitespaceLookahedCount = 4096;

		private const string XmlDeclarationBegining = "<?xml";

		internal const int SurHighStart = 55296;

		internal const int SurHighEnd = 56319;

		internal const int SurLowStart = 56320;

		internal const int SurLowEnd = 57343;

		private XmlCharType xmlCharType = XmlCharType.Instance;

		private XmlTextReaderImpl.ParsingState ps;

		private XmlTextReaderImpl.ParsingFunction parsingFunction;

		private XmlTextReaderImpl.ParsingFunction nextParsingFunction;

		private XmlTextReaderImpl.ParsingFunction nextNextParsingFunction;

		private XmlTextReaderImpl.NodeData[] nodes;

		private XmlTextReaderImpl.NodeData curNode;

		private int index;

		private int curAttrIndex = -1;

		private int attrCount;

		private int attrHashtable;

		private int attrDuplWalkCount;

		private bool attrNeedNamespaceLookup;

		private bool fullAttrCleanup;

		private XmlTextReaderImpl.NodeData[] attrDuplSortingArray;

		private XmlNameTable nameTable;

		private bool nameTableFromSettings;

		private XmlResolver xmlResolver;

		private string url = string.Empty;

		private CompressedStack compressedStack;

		private bool normalize;

		private bool supportNamespaces = true;

		private WhitespaceHandling whitespaceHandling;

		private bool prohibitDtd;

		private EntityHandling entityHandling;

		private bool ignorePIs;

		private bool ignoreComments;

		private bool checkCharacters;

		private int lineNumberOffset;

		private int linePositionOffset;

		private bool closeInput;

		private long maxCharactersInDocument;

		private long maxCharactersFromEntities;

		private bool v1Compat;

		private XmlNamespaceManager namespaceManager;

		private string lastPrefix = string.Empty;

		private XmlTextReaderImpl.XmlContext xmlContext;

		private XmlTextReaderImpl.ParsingState[] parsingStatesStack;

		private int parsingStatesStackTop = -1;

		private string reportedBaseUri;

		private Encoding reportedEncoding;

		private XmlTextReaderImpl.DtdParserProxy dtdParserProxy;

		private XmlNodeType fragmentType = XmlNodeType.Document;

		private bool fragment;

		private XmlParserContext fragmentParserContext;

		private IncrementalReadDecoder incReadDecoder;

		private XmlTextReaderImpl.IncrementalReadState incReadState;

		private int incReadDepth;

		private int incReadLeftStartPos;

		private int incReadLeftEndPos;

		private LineInfo incReadLineInfo;

		private IncrementalReadCharsDecoder readCharsDecoder;

		private BinHexDecoder binHexDecoder;

		private Base64Decoder base64Decoder;

		private int attributeValueBaseEntityId;

		private bool emptyEntityInAttributeResolved;

		private ValidationEventHandler validationEventHandler;

		private bool validatingReaderCompatFlag;

		private bool addDefaultAttributesAndNormalize;

		private XmlQualifiedName qName;

		private BufferBuilder stringBuilder;

		private bool rootElementParsed;

		private bool standalone;

		private int nextEntityId = 1;

		private XmlTextReaderImpl.ParsingMode parsingMode;

		private ReadState readState;

		private SchemaEntity lastEntity;

		private bool afterResetState;

		private int documentStartBytePos;

		private int readValueOffset;

		private long charactersInDocument;

		private long charactersFromEntities;

		private bool disableUndeclaredEntityCheck;

		private XmlReader outerReader;

		private bool xmlResolverIsSet;

		private string Xml;

		private string XmlNs;

		private enum ParsingFunction
		{
			ElementContent,
			NoData,
			OpenUrl,
			SwitchToInteractive,
			SwitchToInteractiveXmlDecl,
			DocumentContent,
			MoveToElementContent,
			PopElementContext,
			PopEmptyElementContext,
			ResetAttributesRootLevel,
			Error,
			Eof,
			ReaderClosed,
			EntityReference,
			InIncrementalRead,
			FragmentAttribute,
			ReportEndEntity,
			AfterResolveEntityInContent,
			AfterResolveEmptyEntityInContent,
			XmlDeclarationFragment,
			GoToEof,
			PartialTextValue,
			InReadAttributeValue,
			InReadValueChunk,
			InReadContentAsBinary,
			InReadElementContentAsBinary
		}

		private enum ParsingMode
		{
			Full,
			SkipNode,
			SkipContent
		}

		private enum EntityType
		{
			CharacterDec,
			CharacterHex,
			CharacterNamed,
			Expanded,
			ExpandedInAttribute,
			Skipped,
			Unexpanded,
			FakeExpanded
		}

		private enum EntityExpandType
		{
			OnlyGeneral,
			OnlyCharacter,
			All
		}

		private enum IncrementalReadState
		{
			Text,
			StartTag,
			PI,
			CDATA,
			Comment,
			Attributes,
			AttributeValue,
			ReadData,
			EndElement,
			End,
			ReadValueChunk_OnCachedValue,
			ReadValueChunk_OnPartialValue,
			ReadContentAsBinary_OnCachedValue,
			ReadContentAsBinary_OnPartialValue,
			ReadContentAsBinary_End
		}

		private struct ParsingState
		{
			internal void Clear()
			{
				this.chars = null;
				this.charPos = 0;
				this.charsUsed = 0;
				this.encoding = null;
				this.stream = null;
				this.decoder = null;
				this.bytes = null;
				this.bytePos = 0;
				this.bytesUsed = 0;
				this.textReader = null;
				this.lineNo = 1;
				this.lineStartPos = -1;
				this.baseUriStr = string.Empty;
				this.baseUri = null;
				this.isEof = false;
				this.isStreamEof = false;
				this.eolNormalized = true;
				this.entityResolvedManually = false;
			}

			internal void Close(bool closeInput)
			{
				if (closeInput)
				{
					if (this.stream != null)
					{
						this.stream.Close();
						return;
					}
					if (this.textReader != null)
					{
						this.textReader.Close();
					}
				}
			}

			internal int LineNo
			{
				get
				{
					return this.lineNo;
				}
			}

			internal int LinePos
			{
				get
				{
					return this.charPos - this.lineStartPos;
				}
			}

			internal char[] chars;

			internal int charPos;

			internal int charsUsed;

			internal Encoding encoding;

			internal bool appendMode;

			internal Stream stream;

			internal Decoder decoder;

			internal byte[] bytes;

			internal int bytePos;

			internal int bytesUsed;

			internal TextReader textReader;

			internal int lineNo;

			internal int lineStartPos;

			internal string baseUriStr;

			internal Uri baseUri;

			internal bool isEof;

			internal bool isStreamEof;

			internal SchemaEntity entity;

			internal int entityId;

			internal bool eolNormalized;

			internal bool entityResolvedManually;
		}

		private class XmlContext
		{
			internal XmlContext()
			{
				this.xmlSpace = XmlSpace.None;
				this.xmlLang = string.Empty;
				this.defaultNamespace = string.Empty;
				this.previousContext = null;
			}

			internal XmlContext(XmlTextReaderImpl.XmlContext previousContext)
			{
				this.xmlSpace = previousContext.xmlSpace;
				this.xmlLang = previousContext.xmlLang;
				this.defaultNamespace = previousContext.defaultNamespace;
				this.previousContext = previousContext;
			}

			internal XmlSpace xmlSpace;

			internal string xmlLang;

			internal string defaultNamespace;

			internal XmlTextReaderImpl.XmlContext previousContext;
		}

		private class NoNamespaceManager : XmlNamespaceManager
		{
			public override string DefaultNamespace
			{
				get
				{
					return string.Empty;
				}
			}

			public override void PushScope()
			{
			}

			public override bool PopScope()
			{
				return false;
			}

			public override void AddNamespace(string prefix, string uri)
			{
			}

			public override void RemoveNamespace(string prefix, string uri)
			{
			}

			public override IEnumerator GetEnumerator()
			{
				return null;
			}

			public override IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
			{
				return null;
			}

			public override string LookupNamespace(string prefix)
			{
				return string.Empty;
			}

			public override string LookupPrefix(string uri)
			{
				return null;
			}

			public override bool HasNamespace(string prefix)
			{
				return false;
			}
		}

		internal class DtdParserProxy : IDtdParserAdapter
		{
			internal DtdParserProxy(XmlTextReaderImpl reader)
			{
				this.reader = reader;
				this.dtdParser = new DtdParser(this);
			}

			internal DtdParserProxy(XmlTextReaderImpl reader, SchemaInfo schemaInfo)
			{
				this.reader = reader;
				this.schemaInfo = schemaInfo;
			}

			internal DtdParserProxy(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, XmlTextReaderImpl reader)
			{
				this.reader = reader;
				this.dtdParser = new DtdParser(baseUri, docTypeName, publicId, systemId, internalSubset, this);
			}

			internal void Parse(bool saveInternalSubset)
			{
				if (this.dtdParser == null)
				{
					throw new InvalidOperationException();
				}
				this.dtdParser.Parse(saveInternalSubset);
			}

			internal SchemaInfo DtdSchemaInfo
			{
				get
				{
					if (this.dtdParser == null)
					{
						return this.schemaInfo;
					}
					return this.dtdParser.SchemaInfo;
				}
			}

			internal string InternalDtdSubset
			{
				get
				{
					if (this.dtdParser == null)
					{
						throw new InvalidOperationException();
					}
					return this.dtdParser.InternalSubset;
				}
			}

			XmlNameTable IDtdParserAdapter.NameTable
			{
				get
				{
					return this.reader.DtdParserProxy_NameTable;
				}
			}

			XmlNamespaceManager IDtdParserAdapter.NamespaceManager
			{
				get
				{
					return this.reader.DtdParserProxy_NamespaceManager;
				}
			}

			bool IDtdParserAdapter.DtdValidation
			{
				get
				{
					return this.reader.DtdParserProxy_DtdValidation;
				}
			}

			bool IDtdParserAdapter.Normalization
			{
				get
				{
					return this.reader.DtdParserProxy_Normalization;
				}
			}

			bool IDtdParserAdapter.Namespaces
			{
				get
				{
					return this.reader.DtdParserProxy_Namespaces;
				}
			}

			bool IDtdParserAdapter.V1CompatibilityMode
			{
				get
				{
					return this.reader.DtdParserProxy_V1CompatibilityMode;
				}
			}

			Uri IDtdParserAdapter.BaseUri
			{
				get
				{
					return this.reader.DtdParserProxy_BaseUri;
				}
			}

			bool IDtdParserAdapter.IsEof
			{
				get
				{
					return this.reader.DtdParserProxy_IsEof;
				}
			}

			char[] IDtdParserAdapter.ParsingBuffer
			{
				get
				{
					return this.reader.DtdParserProxy_ParsingBuffer;
				}
			}

			int IDtdParserAdapter.ParsingBufferLength
			{
				get
				{
					return this.reader.DtdParserProxy_ParsingBufferLength;
				}
			}

			int IDtdParserAdapter.CurrentPosition
			{
				get
				{
					return this.reader.DtdParserProxy_CurrentPosition;
				}
				set
				{
					this.reader.DtdParserProxy_CurrentPosition = value;
				}
			}

			int IDtdParserAdapter.EntityStackLength
			{
				get
				{
					return this.reader.DtdParserProxy_EntityStackLength;
				}
			}

			bool IDtdParserAdapter.IsEntityEolNormalized
			{
				get
				{
					return this.reader.DtdParserProxy_IsEntityEolNormalized;
				}
			}

			ValidationEventHandler IDtdParserAdapter.EventHandler
			{
				get
				{
					return this.reader.DtdParserProxy_EventHandler;
				}
				set
				{
					this.reader.DtdParserProxy_EventHandler = value;
				}
			}

			void IDtdParserAdapter.OnNewLine(int pos)
			{
				this.reader.DtdParserProxy_OnNewLine(pos);
			}

			int IDtdParserAdapter.LineNo
			{
				get
				{
					return this.reader.DtdParserProxy_LineNo;
				}
			}

			int IDtdParserAdapter.LineStartPosition
			{
				get
				{
					return this.reader.DtdParserProxy_LineStartPosition;
				}
			}

			int IDtdParserAdapter.ReadData()
			{
				return this.reader.DtdParserProxy_ReadData();
			}

			void IDtdParserAdapter.SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception)
			{
				this.reader.DtdParserProxy_SendValidationEvent(severity, exception);
			}

			int IDtdParserAdapter.ParseNumericCharRef(BufferBuilder internalSubsetBuilder)
			{
				return this.reader.DtdParserProxy_ParseNumericCharRef(internalSubsetBuilder);
			}

			int IDtdParserAdapter.ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder)
			{
				return this.reader.DtdParserProxy_ParseNamedCharRef(expand, internalSubsetBuilder);
			}

			void IDtdParserAdapter.ParsePI(BufferBuilder sb)
			{
				this.reader.DtdParserProxy_ParsePI(sb);
			}

			void IDtdParserAdapter.ParseComment(BufferBuilder sb)
			{
				this.reader.DtdParserProxy_ParseComment(sb);
			}

			bool IDtdParserAdapter.PushEntity(SchemaEntity entity, int entityId)
			{
				return this.reader.DtdParserProxy_PushEntity(entity, entityId);
			}

			bool IDtdParserAdapter.PopEntity(out SchemaEntity oldEntity, out int newEntityId)
			{
				return this.reader.DtdParserProxy_PopEntity(out oldEntity, out newEntityId);
			}

			bool IDtdParserAdapter.PushExternalSubset(string systemId, string publicId)
			{
				return this.reader.DtdParserProxy_PushExternalSubset(systemId, publicId);
			}

			void IDtdParserAdapter.PushInternalDtd(string baseUri, string internalDtd)
			{
				this.reader.DtdParserProxy_PushInternalDtd(baseUri, internalDtd);
			}

			void IDtdParserAdapter.Throw(Exception e)
			{
				this.reader.DtdParserProxy_Throw(e);
			}

			void IDtdParserAdapter.OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo)
			{
				this.reader.DtdParserProxy_OnSystemId(systemId, keywordLineInfo, systemLiteralLineInfo);
			}

			void IDtdParserAdapter.OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo)
			{
				this.reader.DtdParserProxy_OnPublicId(publicId, keywordLineInfo, publicLiteralLineInfo);
			}

			private XmlTextReaderImpl reader;

			private DtdParser dtdParser;

			private SchemaInfo schemaInfo;
		}

		private class NodeData : IComparable
		{
			internal static XmlTextReaderImpl.NodeData None
			{
				get
				{
					if (XmlTextReaderImpl.NodeData.s_None == null)
					{
						XmlTextReaderImpl.NodeData.s_None = new XmlTextReaderImpl.NodeData();
					}
					return XmlTextReaderImpl.NodeData.s_None;
				}
			}

			internal NodeData()
			{
				this.Clear(XmlNodeType.None);
				this.xmlContextPushed = false;
			}

			internal int LineNo
			{
				get
				{
					return this.lineInfo.lineNo;
				}
			}

			internal int LinePos
			{
				get
				{
					return this.lineInfo.linePos;
				}
			}

			internal bool IsEmptyElement
			{
				get
				{
					return this.type == XmlNodeType.Element && this.isEmptyOrDefault;
				}
				set
				{
					this.isEmptyOrDefault = value;
				}
			}

			internal bool IsDefaultAttribute
			{
				get
				{
					return this.type == XmlNodeType.Attribute && this.isEmptyOrDefault;
				}
				set
				{
					this.isEmptyOrDefault = value;
				}
			}

			internal bool ValueBuffered
			{
				get
				{
					return this.value == null;
				}
			}

			internal string StringValue
			{
				get
				{
					if (this.value == null)
					{
						this.value = new string(this.chars, this.valueStartPos, this.valueLength);
					}
					return this.value;
				}
			}

			internal void TrimSpacesInValue()
			{
				if (this.ValueBuffered)
				{
					XmlComplianceUtil.StripSpaces(this.chars, this.valueStartPos, ref this.valueLength);
					return;
				}
				this.value = XmlComplianceUtil.StripSpaces(this.value);
			}

			internal void Clear(XmlNodeType type)
			{
				this.type = type;
				this.ClearName();
				this.value = string.Empty;
				this.valueStartPos = -1;
				this.nameWPrefix = string.Empty;
				this.schemaType = null;
				this.typedValue = null;
			}

			internal void ClearName()
			{
				this.localName = string.Empty;
				this.prefix = string.Empty;
				this.ns = string.Empty;
				this.nameWPrefix = string.Empty;
			}

			internal void SetLineInfo(int lineNo, int linePos)
			{
				this.lineInfo.Set(lineNo, linePos);
			}

			internal void SetLineInfo2(int lineNo, int linePos)
			{
				this.lineInfo2.Set(lineNo, linePos);
			}

			internal void SetValueNode(XmlNodeType type, string value)
			{
				this.type = type;
				this.ClearName();
				this.value = value;
				this.valueStartPos = -1;
			}

			internal void SetValueNode(XmlNodeType type, char[] chars, int startPos, int len)
			{
				this.type = type;
				this.ClearName();
				this.value = null;
				this.chars = chars;
				this.valueStartPos = startPos;
				this.valueLength = len;
			}

			internal void SetNamedNode(XmlNodeType type, string localName)
			{
				this.SetNamedNode(type, localName, string.Empty, localName);
			}

			internal void SetNamedNode(XmlNodeType type, string localName, string prefix, string nameWPrefix)
			{
				this.type = type;
				this.localName = localName;
				this.prefix = prefix;
				this.nameWPrefix = nameWPrefix;
				this.ns = string.Empty;
				this.value = string.Empty;
				this.valueStartPos = -1;
			}

			internal void SetValue(string value)
			{
				this.valueStartPos = -1;
				this.value = value;
			}

			internal void SetValue(char[] chars, int startPos, int len)
			{
				this.value = null;
				this.chars = chars;
				this.valueStartPos = startPos;
				this.valueLength = len;
			}

			internal void OnBufferInvalidated()
			{
				if (this.value == null)
				{
					this.value = new string(this.chars, this.valueStartPos, this.valueLength);
				}
				this.valueStartPos = -1;
			}

			internal void CopyTo(BufferBuilder sb)
			{
				this.CopyTo(0, sb);
			}

			internal void CopyTo(int valueOffset, BufferBuilder sb)
			{
				if (this.value == null)
				{
					sb.Append(this.chars, this.valueStartPos + valueOffset, this.valueLength - valueOffset);
					return;
				}
				if (valueOffset <= 0)
				{
					sb.Append(this.value);
					return;
				}
				sb.Append(this.value, valueOffset, this.value.Length - valueOffset);
			}

			internal int CopyTo(int valueOffset, char[] buffer, int offset, int length)
			{
				if (this.value == null)
				{
					int num = this.valueLength - valueOffset;
					if (num > length)
					{
						num = length;
					}
					Buffer.BlockCopy(this.chars, (this.valueStartPos + valueOffset) * 2, buffer, offset * 2, num * 2);
					return num;
				}
				int num2 = this.value.Length - valueOffset;
				if (num2 > length)
				{
					num2 = length;
				}
				this.value.CopyTo(valueOffset, buffer, offset, num2);
				return num2;
			}

			internal int CopyToBinary(IncrementalReadDecoder decoder, int valueOffset)
			{
				if (this.value == null)
				{
					return decoder.Decode(this.chars, this.valueStartPos + valueOffset, this.valueLength - valueOffset);
				}
				return decoder.Decode(this.value, valueOffset, this.value.Length - valueOffset);
			}

			internal void AdjustLineInfo(int valueOffset, bool isNormalized, ref LineInfo lineInfo)
			{
				if (valueOffset == 0)
				{
					return;
				}
				if (this.valueStartPos != -1)
				{
					XmlTextReaderImpl.AdjustLineInfo(this.chars, this.valueStartPos, this.valueStartPos + valueOffset, isNormalized, ref lineInfo);
					return;
				}
				char[] array = this.value.ToCharArray(0, valueOffset);
				XmlTextReaderImpl.AdjustLineInfo(array, 0, array.Length, isNormalized, ref lineInfo);
			}

			internal string GetNameWPrefix(XmlNameTable nt)
			{
				if (this.nameWPrefix != null)
				{
					return this.nameWPrefix;
				}
				return this.CreateNameWPrefix(nt);
			}

			internal string CreateNameWPrefix(XmlNameTable nt)
			{
				if (this.prefix.Length == 0)
				{
					this.nameWPrefix = this.localName;
				}
				else
				{
					this.nameWPrefix = nt.Add(this.prefix + ":" + this.localName);
				}
				return this.nameWPrefix;
			}

			int IComparable.CompareTo(object obj)
			{
				XmlTextReaderImpl.NodeData nodeData = obj as XmlTextReaderImpl.NodeData;
				if (nodeData == null)
				{
					return this.GetHashCode().CompareTo(nodeData.GetHashCode());
				}
				if (!Ref.Equal(this.localName, nodeData.localName))
				{
					return string.CompareOrdinal(this.localName, nodeData.localName);
				}
				if (Ref.Equal(this.ns, nodeData.ns))
				{
					return 0;
				}
				return string.CompareOrdinal(this.ns, nodeData.ns);
			}

			private static XmlTextReaderImpl.NodeData s_None;

			internal XmlNodeType type;

			internal string localName;

			internal string prefix;

			internal string ns;

			internal string nameWPrefix;

			private string value;

			private char[] chars;

			private int valueStartPos;

			private int valueLength;

			internal LineInfo lineInfo;

			internal LineInfo lineInfo2;

			internal char quoteChar;

			internal int depth;

			private bool isEmptyOrDefault;

			internal int entityId;

			internal bool xmlContextPushed;

			internal XmlTextReaderImpl.NodeData nextAttrValueChunk;

			internal object schemaType;

			internal object typedValue;
		}

		private class SchemaAttDefToNodeDataComparer : IComparer
		{
			internal static IComparer Instance
			{
				get
				{
					return XmlTextReaderImpl.SchemaAttDefToNodeDataComparer.s_instance;
				}
			}

			public int Compare(object x, object y)
			{
				if (x == null)
				{
					if (y != null)
					{
						return -1;
					}
					return 0;
				}
				else
				{
					if (y == null)
					{
						return 1;
					}
					XmlTextReaderImpl.NodeData nodeData = x as XmlTextReaderImpl.NodeData;
					string text;
					string text2;
					if (nodeData != null)
					{
						text = nodeData.localName;
						text2 = nodeData.prefix;
					}
					else
					{
						SchemaAttDef schemaAttDef = x as SchemaAttDef;
						if (schemaAttDef == null)
						{
							throw new XmlException("Xml_DefaultException", string.Empty);
						}
						text = schemaAttDef.Name.Name;
						text2 = schemaAttDef.Prefix;
					}
					nodeData = y as XmlTextReaderImpl.NodeData;
					string text3;
					string text4;
					if (nodeData != null)
					{
						text3 = nodeData.localName;
						text4 = nodeData.prefix;
					}
					else
					{
						SchemaAttDef schemaAttDef2 = y as SchemaAttDef;
						if (schemaAttDef2 == null)
						{
							throw new XmlException("Xml_DefaultException", string.Empty);
						}
						text3 = schemaAttDef2.Name.Name;
						text4 = schemaAttDef2.Prefix;
					}
					int num = string.Compare(text, text3, StringComparison.Ordinal);
					if (num != 0)
					{
						return num;
					}
					return string.Compare(text2, text4, StringComparison.Ordinal);
				}
			}

			private static IComparer s_instance = new XmlTextReaderImpl.SchemaAttDefToNodeDataComparer();
		}
	}
}
