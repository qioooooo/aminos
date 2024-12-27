using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B4 RID: 180
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlQueryOutput : XmlWriter
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00028F02 File Offset: 0x00027F02
		internal XmlQueryOutput(XmlQueryRuntime runtime, XmlSequenceWriter seqwrt)
		{
			this.runtime = runtime;
			this.seqwrt = seqwrt;
			this.xstate = XmlState.WithinSequence;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00028F1F File Offset: 0x00027F1F
		internal XmlQueryOutput(XmlQueryRuntime runtime, XmlEventCache xwrt)
		{
			this.runtime = runtime;
			this.xwrt = xwrt;
			this.xstate = XmlState.WithinContent;
			this.depth = 1;
			this.rootType = XPathNodeType.Root;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00028F4A File Offset: 0x00027F4A
		internal XmlSequenceWriter SequenceWriter
		{
			get
			{
				return this.seqwrt;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x00028F52 File Offset: 0x00027F52
		// (set) Token: 0x0600083B RID: 2107 RVA: 0x00028F5C File Offset: 0x00027F5C
		internal XmlRawWriter Writer
		{
			get
			{
				return this.xwrt;
			}
			set
			{
				IRemovableWriter removableWriter = value as IRemovableWriter;
				if (removableWriter != null)
				{
					removableWriter.OnRemoveWriterEvent = new OnRemoveWriter(this.SetWrappedWriter);
				}
				this.xwrt = value;
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00028F8C File Offset: 0x00027F8C
		private void SetWrappedWriter(XmlRawWriter writer)
		{
			if (this.Writer is XmlAttributeCache)
			{
				this.attrCache = (XmlAttributeCache)this.Writer;
			}
			this.Writer = writer;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00028FB3 File Offset: 0x00027FB3
		public override void WriteStartDocument()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00028FBA File Offset: 0x00027FBA
		public override void WriteStartDocument(bool standalone)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00028FC1 File Offset: 0x00027FC1
		public override void WriteEndDocument()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00028FC8 File Offset: 0x00027FC8
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00028FD0 File Offset: 0x00027FD0
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.ConstructWithinContent(XPathNodeType.Element);
			this.WriteStartElementUnchecked(prefix, localName, ns);
			this.WriteNamespaceDeclarationUnchecked(prefix, ns);
			if (this.attrCache == null)
			{
				this.attrCache = new XmlAttributeCache();
			}
			this.attrCache.Init(this.Writer);
			this.Writer = this.attrCache;
			this.attrCache = null;
			this.PushElementNames(prefix, localName, ns);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00029038 File Offset: 0x00028038
		public override void WriteEndElement()
		{
			if (this.xstate == XmlState.EnumAttrs)
			{
				this.StartElementContentUnchecked();
			}
			string text;
			string text2;
			string text3;
			this.PopElementNames(out text, out text2, out text3);
			this.WriteEndElementUnchecked(text, text2, text3);
			if (this.depth == 0)
			{
				this.EndTree();
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00029077 File Offset: 0x00028077
		public override void WriteFullEndElement()
		{
			this.WriteEndElement();
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00029080 File Offset: 0x00028080
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (prefix.Length == 5 && prefix == "xmlns")
			{
				this.WriteStartNamespace(localName);
				return;
			}
			this.ConstructInEnumAttrs(XPathNodeType.Attribute);
			if (ns.Length != 0 && this.depth != 0)
			{
				prefix = this.CheckAttributePrefix(prefix, ns);
			}
			this.WriteStartAttributeUnchecked(prefix, localName, ns);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000290D5 File Offset: 0x000280D5
		public override void WriteEndAttribute()
		{
			if (this.xstate == XmlState.WithinNmsp)
			{
				this.WriteEndNamespace();
				return;
			}
			this.WriteEndAttributeUnchecked();
			if (this.depth == 0)
			{
				this.EndTree();
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x000290FB File Offset: 0x000280FB
		public override void WriteComment(string text)
		{
			this.WriteStartComment();
			this.WriteCommentString(text);
			this.WriteEndComment();
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00029110 File Offset: 0x00028110
		public override void WriteProcessingInstruction(string target, string text)
		{
			this.WriteStartProcessingInstruction(target);
			this.WriteProcessingInstructionString(text);
			this.WriteEndProcessingInstruction();
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00029126 File Offset: 0x00028126
		public override void WriteEntityRef(string name)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0002912D File Offset: 0x0002812D
		public override void WriteCharEntity(char ch)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00029134 File Offset: 0x00028134
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0002913B File Offset: 0x0002813B
		public override void WriteWhitespace(string ws)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00029142 File Offset: 0x00028142
		public override void WriteString(string text)
		{
			this.WriteString(text, false);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0002914C File Offset: 0x0002814C
		public override void WriteChars(char[] buffer, int index, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00029153 File Offset: 0x00028153
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0002915A File Offset: 0x0002815A
		public override void WriteRaw(string data)
		{
			this.WriteString(data, true);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00029164 File Offset: 0x00028164
		public override void WriteCData(string text)
		{
			this.WriteString(text, false);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002916E File Offset: 0x0002816E
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x00029175 File Offset: 0x00028175
		public override WriteState WriteState
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0002917C File Offset: 0x0002817C
		public override void Close()
		{
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002917E File Offset: 0x0002817E
		public override void Flush()
		{
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00029180 File Offset: 0x00028180
		public override string LookupPrefix(string ns)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00029187 File Offset: 0x00028187
		public override XmlSpace XmlSpace
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0002918E File Offset: 0x0002818E
		public override string XmlLang
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00029195 File Offset: 0x00028195
		public void StartTree(XPathNodeType rootType)
		{
			this.Writer = this.seqwrt.StartTree(rootType, this.nsmgr, this.runtime.NameTable);
			this.rootType = rootType;
			this.xstate = ((rootType == XPathNodeType.Attribute || rootType == XPathNodeType.Namespace) ? XmlState.EnumAttrs : XmlState.WithinContent);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x000291D3 File Offset: 0x000281D3
		public void EndTree()
		{
			this.seqwrt.EndTree();
			this.xstate = XmlState.WithinSequence;
			this.Writer = null;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000291F0 File Offset: 0x000281F0
		public void WriteStartElementUnchecked(string prefix, string localName, string ns)
		{
			if (this.nsmgr != null)
			{
				this.nsmgr.PushScope();
			}
			this.Writer.WriteStartElement(prefix, localName, ns);
			this.xstate = XmlState.EnumAttrs;
			this.depth++;
			this.useDefNmsp = ns.Length == 0;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00029242 File Offset: 0x00028242
		public void WriteStartElementUnchecked(string localName)
		{
			this.WriteStartElementUnchecked(string.Empty, localName, string.Empty);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00029255 File Offset: 0x00028255
		public void StartElementContentUnchecked()
		{
			if (this.cntNmsp != 0)
			{
				this.WriteCachedNamespaces();
			}
			this.Writer.StartElementContent();
			this.xstate = XmlState.WithinContent;
			this.useDefNmsp = false;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0002927E File Offset: 0x0002827E
		public void WriteEndElementUnchecked(string prefix, string localName, string ns)
		{
			this.Writer.WriteEndElement(prefix, localName, ns);
			this.xstate = XmlState.WithinContent;
			this.depth--;
			if (this.nsmgr != null)
			{
				this.nsmgr.PopScope();
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000292B7 File Offset: 0x000282B7
		public void WriteEndElementUnchecked(string localName)
		{
			this.WriteEndElementUnchecked(string.Empty, localName, string.Empty);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000292CA File Offset: 0x000282CA
		public void WriteStartAttributeUnchecked(string prefix, string localName, string ns)
		{
			this.Writer.WriteStartAttribute(prefix, localName, ns);
			this.xstate = XmlState.WithinAttr;
			this.depth++;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000292EF File Offset: 0x000282EF
		public void WriteStartAttributeUnchecked(string localName)
		{
			this.WriteStartAttributeUnchecked(string.Empty, localName, string.Empty);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00029302 File Offset: 0x00028302
		public void WriteEndAttributeUnchecked()
		{
			this.Writer.WriteEndAttribute();
			this.xstate = XmlState.EnumAttrs;
			this.depth--;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00029324 File Offset: 0x00028324
		public void WriteNamespaceDeclarationUnchecked(string prefix, string ns)
		{
			if (this.depth == 0)
			{
				this.Writer.WriteNamespaceDeclaration(prefix, ns);
				return;
			}
			if (this.nsmgr == null)
			{
				if (ns.Length == 0 && prefix.Length == 0)
				{
					return;
				}
				this.nsmgr = new XmlNamespaceManager(this.runtime.NameTable);
				this.nsmgr.PushScope();
			}
			if (this.nsmgr.LookupNamespace(prefix) != ns)
			{
				this.AddNamespace(prefix, ns);
			}
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0002939D File Offset: 0x0002839D
		public void WriteStringUnchecked(string text)
		{
			this.Writer.WriteString(text);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000293AB File Offset: 0x000283AB
		public void WriteRawUnchecked(string text)
		{
			this.Writer.WriteRaw(text);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x000293B9 File Offset: 0x000283B9
		public void WriteStartRoot()
		{
			if (this.xstate != XmlState.WithinSequence)
			{
				this.ThrowInvalidStateError(XPathNodeType.Root);
			}
			this.StartTree(XPathNodeType.Root);
			this.depth++;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x000293DF File Offset: 0x000283DF
		public void WriteEndRoot()
		{
			this.depth--;
			this.EndTree();
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000293F5 File Offset: 0x000283F5
		public void WriteStartElementLocalName(string localName)
		{
			this.WriteStartElement(string.Empty, localName, string.Empty);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00029408 File Offset: 0x00028408
		public void WriteStartAttributeLocalName(string localName)
		{
			this.WriteStartAttribute(string.Empty, localName, string.Empty);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0002941B File Offset: 0x0002841B
		public void WriteStartElementComputed(string tagName, int prefixMappingsIndex)
		{
			this.WriteStartComputed(XPathNodeType.Element, tagName, prefixMappingsIndex);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00029426 File Offset: 0x00028426
		public void WriteStartElementComputed(string tagName, string ns)
		{
			this.WriteStartComputed(XPathNodeType.Element, tagName, ns);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00029431 File Offset: 0x00028431
		public void WriteStartElementComputed(XPathNavigator navigator)
		{
			this.WriteStartComputed(XPathNodeType.Element, navigator);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0002943B File Offset: 0x0002843B
		public void WriteStartElementComputed(XmlQualifiedName name)
		{
			this.WriteStartComputed(XPathNodeType.Element, name);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00029445 File Offset: 0x00028445
		public void WriteStartAttributeComputed(string tagName, int prefixMappingsIndex)
		{
			this.WriteStartComputed(XPathNodeType.Attribute, tagName, prefixMappingsIndex);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00029450 File Offset: 0x00028450
		public void WriteStartAttributeComputed(string tagName, string ns)
		{
			this.WriteStartComputed(XPathNodeType.Attribute, tagName, ns);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0002945B File Offset: 0x0002845B
		public void WriteStartAttributeComputed(XPathNavigator navigator)
		{
			this.WriteStartComputed(XPathNodeType.Attribute, navigator);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00029465 File Offset: 0x00028465
		public void WriteStartAttributeComputed(XmlQualifiedName name)
		{
			this.WriteStartComputed(XPathNodeType.Attribute, name);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00029470 File Offset: 0x00028470
		public void WriteNamespaceDeclaration(string prefix, string ns)
		{
			this.ConstructInEnumAttrs(XPathNodeType.Namespace);
			if (this.nsmgr == null)
			{
				this.WriteNamespaceDeclarationUnchecked(prefix, ns);
			}
			else
			{
				string text = this.nsmgr.LookupNamespace(prefix);
				if (ns != text)
				{
					if (text != null && (prefix.Length != 0 || text.Length != 0 || this.useDefNmsp))
					{
						throw new XslTransformException("XmlIl_NmspConflict", new string[]
						{
							(prefix.Length == 0) ? "" : ":",
							prefix,
							ns,
							text
						});
					}
					this.AddNamespace(prefix, ns);
				}
			}
			if (this.depth == 0)
			{
				this.EndTree();
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00029513 File Offset: 0x00028513
		public void WriteStartNamespace(string prefix)
		{
			this.ConstructInEnumAttrs(XPathNodeType.Namespace);
			this.piTarget = prefix;
			this.nodeText.Clear();
			this.xstate = XmlState.WithinNmsp;
			this.depth++;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00029543 File Offset: 0x00028543
		public void WriteNamespaceString(string text)
		{
			this.nodeText.ConcatNoDelimiter(text);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00029551 File Offset: 0x00028551
		public void WriteEndNamespace()
		{
			this.xstate = XmlState.EnumAttrs;
			this.depth--;
			this.WriteNamespaceDeclaration(this.piTarget, this.nodeText.GetResult());
			if (this.depth == 0)
			{
				this.EndTree();
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0002958D File Offset: 0x0002858D
		public void WriteStartComment()
		{
			this.ConstructWithinContent(XPathNodeType.Comment);
			this.nodeText.Clear();
			this.xstate = XmlState.WithinComment;
			this.depth++;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000295B6 File Offset: 0x000285B6
		public void WriteCommentString(string text)
		{
			this.nodeText.ConcatNoDelimiter(text);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000295C4 File Offset: 0x000285C4
		public void WriteEndComment()
		{
			this.Writer.WriteComment(this.nodeText.GetResult());
			this.xstate = XmlState.WithinContent;
			this.depth--;
			if (this.depth == 0)
			{
				this.EndTree();
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00029600 File Offset: 0x00028600
		public void WriteStartProcessingInstruction(string target)
		{
			this.ConstructWithinContent(XPathNodeType.ProcessingInstruction);
			ValidateNames.ValidateNameThrow("", target, "", XPathNodeType.ProcessingInstruction, ValidateNames.Flags.AllExceptPrefixMapping);
			this.piTarget = target;
			this.nodeText.Clear();
			this.xstate = XmlState.WithinPI;
			this.depth++;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0002964D File Offset: 0x0002864D
		public void WriteProcessingInstructionString(string text)
		{
			this.nodeText.ConcatNoDelimiter(text);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002965C File Offset: 0x0002865C
		public void WriteEndProcessingInstruction()
		{
			this.Writer.WriteProcessingInstruction(this.piTarget, this.nodeText.GetResult());
			this.xstate = XmlState.WithinContent;
			this.depth--;
			if (this.depth == 0)
			{
				this.EndTree();
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000296A8 File Offset: 0x000286A8
		public void WriteItem(XPathItem item)
		{
			if (!item.IsNode)
			{
				this.seqwrt.WriteItem(item);
				return;
			}
			XPathNavigator xpathNavigator = (XPathNavigator)item;
			if (this.xstate == XmlState.WithinSequence)
			{
				this.seqwrt.WriteItem(xpathNavigator);
				return;
			}
			this.CopyNode(xpathNavigator);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000296F0 File Offset: 0x000286F0
		public void XsltCopyOf(XPathNavigator navigator)
		{
			RtfNavigator rtfNavigator = navigator as RtfNavigator;
			if (rtfNavigator != null)
			{
				rtfNavigator.CopyToWriter(this);
				return;
			}
			if (navigator.NodeType == XPathNodeType.Root)
			{
				if (navigator.MoveToFirstChild())
				{
					do
					{
						this.CopyNode(navigator);
					}
					while (navigator.MoveToNext());
					navigator.MoveToParent();
					return;
				}
			}
			else
			{
				this.CopyNode(navigator);
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0002973D File Offset: 0x0002873D
		public bool StartCopy(XPathNavigator navigator)
		{
			if (navigator.NodeType == XPathNodeType.Root)
			{
				return true;
			}
			if (this.StartCopy(navigator, true))
			{
				this.CopyNamespaces(navigator, XPathNamespaceScope.ExcludeXml);
				return true;
			}
			return false;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0002975E File Offset: 0x0002875E
		public void EndCopy(XPathNavigator navigator)
		{
			if (navigator.NodeType == XPathNodeType.Element)
			{
				this.WriteEndElement();
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0002976F File Offset: 0x0002876F
		private void AddNamespace(string prefix, string ns)
		{
			this.nsmgr.AddNamespace(prefix, ns);
			this.cntNmsp++;
			if (ns.Length == 0)
			{
				this.useDefNmsp = true;
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0002979C File Offset: 0x0002879C
		private void WriteString(string text, bool disableOutputEscaping)
		{
			switch (this.xstate)
			{
			case XmlState.WithinSequence:
				this.StartTree(XPathNodeType.Text);
				break;
			case XmlState.EnumAttrs:
				this.StartElementContentUnchecked();
				break;
			case XmlState.WithinContent:
				break;
			case XmlState.WithinAttr:
				this.WriteStringUnchecked(text);
				goto IL_0071;
			case XmlState.WithinNmsp:
				this.WriteNamespaceString(text);
				goto IL_0071;
			case XmlState.WithinComment:
				this.WriteCommentString(text);
				goto IL_0071;
			case XmlState.WithinPI:
				this.WriteProcessingInstructionString(text);
				goto IL_0071;
			default:
				goto IL_0071;
			}
			if (disableOutputEscaping)
			{
				this.WriteRawUnchecked(text);
			}
			else
			{
				this.WriteStringUnchecked(text);
			}
			IL_0071:
			if (this.depth == 0)
			{
				this.EndTree();
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00029828 File Offset: 0x00028828
		private void CopyNode(XPathNavigator navigator)
		{
			int num = this.depth;
			for (;;)
			{
				IL_0007:
				if (this.StartCopy(navigator, this.depth == num))
				{
					XPathNodeType nodeType = navigator.NodeType;
					if (navigator.MoveToFirstAttribute())
					{
						do
						{
							this.StartCopy(navigator, false);
						}
						while (navigator.MoveToNextAttribute());
						navigator.MoveToParent();
					}
					this.CopyNamespaces(navigator, (this.depth - 1 == num) ? XPathNamespaceScope.ExcludeXml : XPathNamespaceScope.Local);
					this.StartElementContentUnchecked();
					if (navigator.MoveToFirstChild())
					{
						continue;
					}
					this.EndCopy(navigator, this.depth - 1 == num);
				}
				while (this.depth != num)
				{
					if (navigator.MoveToNext())
					{
						goto IL_0007;
					}
					navigator.MoveToParent();
					this.EndCopy(navigator, this.depth - 1 == num);
				}
				break;
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000298DC File Offset: 0x000288DC
		private bool StartCopy(XPathNavigator navigator, bool callChk)
		{
			bool flag = false;
			switch (navigator.NodeType)
			{
			case XPathNodeType.Root:
				this.ThrowInvalidStateError(XPathNodeType.Root);
				break;
			case XPathNodeType.Element:
				if (callChk)
				{
					this.WriteStartElement(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
				}
				else
				{
					this.WriteStartElementUnchecked(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
				}
				flag = true;
				break;
			case XPathNodeType.Attribute:
				if (callChk)
				{
					this.WriteStartAttribute(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
				}
				else
				{
					this.WriteStartAttributeUnchecked(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
				}
				this.WriteString(navigator.Value);
				if (callChk)
				{
					this.WriteEndAttribute();
				}
				else
				{
					this.WriteEndAttributeUnchecked();
				}
				break;
			case XPathNodeType.Namespace:
				if (callChk)
				{
					XmlAttributeCache xmlAttributeCache = this.Writer as XmlAttributeCache;
					if (xmlAttributeCache != null && xmlAttributeCache.Count != 0)
					{
						throw new XslTransformException("XmlIl_NmspAfterAttr", new string[] { string.Empty });
					}
					this.WriteNamespaceDeclaration(navigator.LocalName, navigator.Value);
				}
				else
				{
					this.WriteNamespaceDeclarationUnchecked(navigator.LocalName, navigator.Value);
				}
				break;
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				if (callChk)
				{
					this.WriteString(navigator.Value, false);
				}
				else
				{
					this.WriteStringUnchecked(navigator.Value);
				}
				break;
			case XPathNodeType.ProcessingInstruction:
				this.WriteStartProcessingInstruction(navigator.LocalName);
				this.WriteProcessingInstructionString(navigator.Value);
				this.WriteEndProcessingInstruction();
				break;
			case XPathNodeType.Comment:
				this.WriteStartComment();
				this.WriteCommentString(navigator.Value);
				this.WriteEndComment();
				break;
			}
			return flag;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00029A76 File Offset: 0x00028A76
		private void EndCopy(XPathNavigator navigator, bool callChk)
		{
			if (callChk)
			{
				this.WriteEndElement();
				return;
			}
			this.WriteEndElementUnchecked(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00029A9A File Offset: 0x00028A9A
		private void CopyNamespaces(XPathNavigator navigator, XPathNamespaceScope nsScope)
		{
			if (navigator.NamespaceURI.Length == 0)
			{
				this.WriteNamespaceDeclarationUnchecked(string.Empty, string.Empty);
			}
			if (navigator.MoveToFirstNamespace(nsScope))
			{
				this.CopyNamespacesHelper(navigator, nsScope);
				navigator.MoveToParent();
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00029AD4 File Offset: 0x00028AD4
		private void CopyNamespacesHelper(XPathNavigator navigator, XPathNamespaceScope nsScope)
		{
			string localName = navigator.LocalName;
			string value = navigator.Value;
			if (navigator.MoveToNextNamespace(nsScope))
			{
				this.CopyNamespacesHelper(navigator, nsScope);
			}
			this.WriteNamespaceDeclarationUnchecked(localName, value);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00029B08 File Offset: 0x00028B08
		private void ConstructWithinContent(XPathNodeType rootType)
		{
			switch (this.xstate)
			{
			case XmlState.WithinSequence:
				this.StartTree(rootType);
				this.xstate = XmlState.WithinContent;
				return;
			case XmlState.EnumAttrs:
				this.StartElementContentUnchecked();
				return;
			case XmlState.WithinContent:
				break;
			default:
				this.ThrowInvalidStateError(rootType);
				break;
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00029B50 File Offset: 0x00028B50
		private void ConstructInEnumAttrs(XPathNodeType rootType)
		{
			switch (this.xstate)
			{
			case XmlState.WithinSequence:
				this.StartTree(rootType);
				this.xstate = XmlState.EnumAttrs;
				return;
			case XmlState.EnumAttrs:
				break;
			default:
				this.ThrowInvalidStateError(rootType);
				break;
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00029B8C File Offset: 0x00028B8C
		private void WriteCachedNamespaces()
		{
			while (this.cntNmsp != 0)
			{
				this.cntNmsp--;
				string text;
				string text2;
				this.nsmgr.GetNamespaceDeclaration(this.cntNmsp, out text, out text2);
				this.Writer.WriteNamespaceDeclaration(text, text2);
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00029BD4 File Offset: 0x00028BD4
		private XPathNodeType XmlStateToNodeType(XmlState xstate)
		{
			switch (xstate)
			{
			case XmlState.EnumAttrs:
				return XPathNodeType.Element;
			case XmlState.WithinContent:
				return XPathNodeType.Element;
			case XmlState.WithinAttr:
				return XPathNodeType.Attribute;
			case XmlState.WithinComment:
				return XPathNodeType.Comment;
			case XmlState.WithinPI:
				return XPathNodeType.ProcessingInstruction;
			}
			return XPathNodeType.Element;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00029C10 File Offset: 0x00028C10
		private string CheckAttributePrefix(string prefix, string ns)
		{
			if (this.nsmgr == null)
			{
				this.WriteNamespaceDeclarationUnchecked(prefix, ns);
			}
			else
			{
				for (;;)
				{
					string text = this.nsmgr.LookupNamespace(prefix);
					if (!(text != ns))
					{
						return prefix;
					}
					if (text == null)
					{
						break;
					}
					prefix = this.RemapPrefix(prefix, ns, false);
				}
				this.AddNamespace(prefix, ns);
			}
			return prefix;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00029C60 File Offset: 0x00028C60
		private string RemapPrefix(string prefix, string ns, bool isElemPrefix)
		{
			if (this.conflictPrefixes == null)
			{
				this.conflictPrefixes = new Hashtable(16);
			}
			if (this.nsmgr == null)
			{
				this.nsmgr = new XmlNamespaceManager(this.runtime.NameTable);
				this.nsmgr.PushScope();
			}
			string text = this.nsmgr.LookupPrefix(ns);
			if (text == null || (!isElemPrefix && text.Length == 0))
			{
				text = this.conflictPrefixes[ns] as string;
				if (text == null || !(text != prefix) || (!isElemPrefix && text.Length == 0))
				{
					text = string.Format(CultureInfo.InvariantCulture, "xp_{0}", new object[] { this.prefixIndex++ });
				}
			}
			this.conflictPrefixes[ns] = text;
			return text;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00029D30 File Offset: 0x00028D30
		private void WriteStartComputed(XPathNodeType nodeType, string tagName, int prefixMappingsIndex)
		{
			string text;
			string text2;
			string text3;
			this.runtime.ParseTagName(tagName, prefixMappingsIndex, out text, out text2, out text3);
			text = this.EnsureValidName(text, text2, text3, nodeType);
			if (nodeType == XPathNodeType.Element)
			{
				this.WriteStartElement(text, text2, text3);
				return;
			}
			this.WriteStartAttribute(text, text2, text3);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00029D74 File Offset: 0x00028D74
		private void WriteStartComputed(XPathNodeType nodeType, string tagName, string ns)
		{
			string text;
			string text2;
			ValidateNames.ParseQNameThrow(tagName, out text, out text2);
			text = this.EnsureValidName(text, text2, ns, nodeType);
			if (nodeType == XPathNodeType.Element)
			{
				this.WriteStartElement(text, text2, ns);
				return;
			}
			this.WriteStartAttribute(text, text2, ns);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00029DB0 File Offset: 0x00028DB0
		private void WriteStartComputed(XPathNodeType nodeType, XPathNavigator navigator)
		{
			string text = navigator.Prefix;
			string localName = navigator.LocalName;
			string namespaceURI = navigator.NamespaceURI;
			if (navigator.NodeType != nodeType)
			{
				text = this.EnsureValidName(text, localName, namespaceURI, nodeType);
			}
			if (nodeType == XPathNodeType.Element)
			{
				this.WriteStartElement(text, localName, namespaceURI);
				return;
			}
			this.WriteStartAttribute(text, localName, namespaceURI);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00029E00 File Offset: 0x00028E00
		private void WriteStartComputed(XPathNodeType nodeType, XmlQualifiedName name)
		{
			string text = ((name.Namespace.Length != 0) ? this.RemapPrefix(string.Empty, name.Namespace, nodeType == XPathNodeType.Element) : string.Empty);
			text = this.EnsureValidName(text, name.Name, name.Namespace, nodeType);
			if (nodeType == XPathNodeType.Element)
			{
				this.WriteStartElement(text, name.Name, name.Namespace);
				return;
			}
			this.WriteStartAttribute(text, name.Name, name.Namespace);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00029E77 File Offset: 0x00028E77
		private string EnsureValidName(string prefix, string localName, string ns, XPathNodeType nodeType)
		{
			if (!ValidateNames.ValidateName(prefix, localName, ns, nodeType, ValidateNames.Flags.AllExceptNCNames))
			{
				prefix = ((ns.Length != 0) ? this.RemapPrefix(string.Empty, ns, nodeType == XPathNodeType.Element) : string.Empty);
				ValidateNames.ValidateNameThrow(prefix, localName, ns, nodeType, ValidateNames.Flags.AllExceptNCNames);
			}
			return prefix;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00029EB4 File Offset: 0x00028EB4
		private void PushElementNames(string prefix, string localName, string ns)
		{
			if (this.stkNames == null)
			{
				this.stkNames = new Stack();
			}
			this.stkNames.Push(prefix);
			this.stkNames.Push(localName);
			this.stkNames.Push(ns);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00029EED File Offset: 0x00028EED
		private void PopElementNames(out string prefix, out string localName, out string ns)
		{
			ns = this.stkNames.Pop() as string;
			localName = this.stkNames.Pop() as string;
			prefix = this.stkNames.Pop() as string;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00029F28 File Offset: 0x00028F28
		private void ThrowInvalidStateError(XPathNodeType constructorType)
		{
			switch (constructorType)
			{
			case XPathNodeType.Root:
			case XPathNodeType.Element:
			case XPathNodeType.Text:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				break;
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				if (this.depth == 1)
				{
					throw new XslTransformException("XmlIl_BadXmlState", new string[]
					{
						constructorType.ToString(),
						this.rootType.ToString()
					});
				}
				if (this.xstate == XmlState.WithinContent)
				{
					throw new XslTransformException("XmlIl_BadXmlStateAttr", new string[] { string.Empty });
				}
				break;
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				goto IL_00CC;
			default:
				goto IL_00CC;
			}
			throw new XslTransformException("XmlIl_BadXmlState", new string[]
			{
				constructorType.ToString(),
				this.XmlStateToNodeType(this.xstate).ToString()
			});
			IL_00CC:
			throw new XslTransformException("XmlIl_BadXmlState", new string[]
			{
				"Unknown",
				this.XmlStateToNodeType(this.xstate).ToString()
			});
		}

		// Token: 0x04000584 RID: 1412
		private XmlRawWriter xwrt;

		// Token: 0x04000585 RID: 1413
		private XmlQueryRuntime runtime;

		// Token: 0x04000586 RID: 1414
		private XmlAttributeCache attrCache;

		// Token: 0x04000587 RID: 1415
		private int depth;

		// Token: 0x04000588 RID: 1416
		private XmlState xstate;

		// Token: 0x04000589 RID: 1417
		private XmlSequenceWriter seqwrt;

		// Token: 0x0400058A RID: 1418
		private XmlNamespaceManager nsmgr;

		// Token: 0x0400058B RID: 1419
		private int cntNmsp;

		// Token: 0x0400058C RID: 1420
		private Hashtable conflictPrefixes;

		// Token: 0x0400058D RID: 1421
		private int prefixIndex;

		// Token: 0x0400058E RID: 1422
		private string piTarget;

		// Token: 0x0400058F RID: 1423
		private StringConcat nodeText;

		// Token: 0x04000590 RID: 1424
		private Stack stkNames;

		// Token: 0x04000591 RID: 1425
		private XPathNodeType rootType;

		// Token: 0x04000592 RID: 1426
		private bool useDefNmsp;
	}
}
