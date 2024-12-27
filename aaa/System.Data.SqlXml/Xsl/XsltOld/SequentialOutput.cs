using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000192 RID: 402
	internal abstract class SequentialOutput : RecordOutput
	{
		// Token: 0x06001123 RID: 4387 RVA: 0x00052600 File Offset: 0x00051600
		private void CacheOuptutProps(XsltOutput output)
		{
			this.output = output;
			this.isXmlOutput = this.output.Method == XsltOutput.OutputMethod.Xml;
			this.isHtmlOutput = this.output.Method == XsltOutput.OutputMethod.Html;
			this.cdataElements = this.output.CDataElements;
			this.indentOutput = this.output.Indent;
			this.outputDoctype = this.output.DoctypeSystem != null || (this.isHtmlOutput && this.output.DoctypePublic != null);
			this.outputXmlDecl = this.isXmlOutput && !this.output.OmitXmlDeclaration && !this.omitXmlDeclCalled;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x000526B7 File Offset: 0x000516B7
		internal SequentialOutput(Processor processor)
		{
			this.processor = processor;
			this.CacheOuptutProps(processor.Output);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x000526E4 File Offset: 0x000516E4
		public void OmitXmlDecl()
		{
			this.omitXmlDeclCalled = true;
			this.outputXmlDecl = false;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x000526F4 File Offset: 0x000516F4
		private void WriteStartElement(RecordBuilder record)
		{
			BuilderInfo mainNode = record.MainNode;
			HtmlElementProps htmlElementProps = null;
			if (this.isHtmlOutput)
			{
				if (mainNode.Prefix.Length == 0)
				{
					htmlElementProps = mainNode.htmlProps;
					if (htmlElementProps == null && mainNode.search)
					{
						htmlElementProps = HtmlElementProps.GetProps(mainNode.LocalName);
					}
					record.Manager.CurrentElementScope.HtmlElementProps = htmlElementProps;
					mainNode.IsEmptyTag = false;
				}
			}
			else if (this.isXmlOutput && mainNode.Depth == 0)
			{
				if (this.secondRoot && (this.output.DoctypeSystem != null || this.output.Standalone))
				{
					throw XsltException.Create("Xslt_MultipleRoots", new string[0]);
				}
				this.secondRoot = true;
			}
			if (this.outputDoctype)
			{
				this.WriteDoctype(mainNode);
				this.outputDoctype = false;
			}
			if (this.cdataElements != null && this.cdataElements.Contains(new XmlQualifiedName(mainNode.LocalName, mainNode.NamespaceURI)) && this.isXmlOutput)
			{
				record.Manager.CurrentElementScope.ToCData = true;
			}
			this.Indent(record);
			this.Write('<');
			this.WriteName(mainNode.Prefix, mainNode.LocalName);
			this.WriteAttributes(record.AttributeList, record.AttributeCount, htmlElementProps);
			if (mainNode.IsEmptyTag)
			{
				this.Write(" />");
			}
			else
			{
				this.Write('>');
			}
			if (htmlElementProps != null && htmlElementProps.Head)
			{
				mainNode.Depth++;
				this.Indent(record);
				mainNode.Depth--;
				this.Write("<META http-equiv=\"Content-Type\" content=\"");
				this.Write(this.output.MediaType);
				this.Write("; charset=");
				this.Write(this.encoding.WebName);
				this.Write("\">");
			}
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x000528BC File Offset: 0x000518BC
		private void WriteTextNode(RecordBuilder record)
		{
			BuilderInfo mainNode = record.MainNode;
			OutputScope currentElementScope = record.Manager.CurrentElementScope;
			currentElementScope.Mixed = true;
			if (currentElementScope.HtmlElementProps != null && currentElementScope.HtmlElementProps.NoEntities)
			{
				this.Write(mainNode.Value);
				return;
			}
			if (currentElementScope.ToCData)
			{
				this.WriteCDataSection(mainNode.Value);
				return;
			}
			this.WriteTextNode(mainNode);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00052924 File Offset: 0x00051924
		private void WriteTextNode(BuilderInfo node)
		{
			for (int i = 0; i < node.TextInfoCount; i++)
			{
				string text = node.TextInfo[i];
				if (text == null)
				{
					i++;
					this.Write(node.TextInfo[i]);
				}
				else
				{
					this.WriteWithReplace(text, SequentialOutput.s_TextValueFind, SequentialOutput.s_TextValueReplace);
				}
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00052973 File Offset: 0x00051973
		private void WriteCDataSection(string value)
		{
			this.Write("<![CDATA[");
			this.WriteCData(value);
			this.Write("]]>");
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00052994 File Offset: 0x00051994
		private void WriteDoctype(BuilderInfo mainNode)
		{
			this.Indent(0);
			this.Write("<!DOCTYPE ");
			if (this.isXmlOutput)
			{
				this.WriteName(mainNode.Prefix, mainNode.LocalName);
			}
			else
			{
				this.WriteName(string.Empty, "html");
			}
			this.Write(' ');
			if (this.output.DoctypePublic != null)
			{
				this.Write("PUBLIC ");
				this.Write('"');
				this.Write(this.output.DoctypePublic);
				this.Write("\" ");
			}
			else
			{
				this.Write("SYSTEM ");
			}
			if (this.output.DoctypeSystem != null)
			{
				this.Write('"');
				this.Write(this.output.DoctypeSystem);
				this.Write('"');
			}
			this.Write('>');
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00052A68 File Offset: 0x00051A68
		private void WriteXmlDeclaration()
		{
			this.outputXmlDecl = false;
			this.Indent(0);
			this.Write("<?");
			this.WriteName(string.Empty, "xml");
			this.Write(" version=\"1.0\"");
			if (this.encoding != null)
			{
				this.Write(" encoding=\"");
				this.Write(this.encoding.WebName);
				this.Write('"');
			}
			if (this.output.HasStandalone)
			{
				this.Write(" standalone=\"");
				this.Write(this.output.Standalone ? "yes" : "no");
				this.Write('"');
			}
			this.Write("?>");
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00052B1F File Offset: 0x00051B1F
		private void WriteProcessingInstruction(RecordBuilder record)
		{
			this.Indent(record);
			this.WriteProcessingInstruction(record.MainNode);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00052B34 File Offset: 0x00051B34
		private void WriteProcessingInstruction(BuilderInfo node)
		{
			this.Write("<?");
			this.WriteName(node.Prefix, node.LocalName);
			this.Write(' ');
			this.Write(node.Value);
			if (this.isHtmlOutput)
			{
				this.Write('>');
				return;
			}
			this.Write("?>");
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00052B90 File Offset: 0x00051B90
		private void WriteEndElement(RecordBuilder record)
		{
			BuilderInfo mainNode = record.MainNode;
			HtmlElementProps htmlElementProps = record.Manager.CurrentElementScope.HtmlElementProps;
			if (htmlElementProps != null && htmlElementProps.Empty)
			{
				return;
			}
			this.Indent(record);
			this.Write("</");
			this.WriteName(record.MainNode.Prefix, record.MainNode.LocalName);
			this.Write('>');
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00052BF8 File Offset: 0x00051BF8
		public Processor.OutputResult RecordDone(RecordBuilder record)
		{
			if (this.output.Method == XsltOutput.OutputMethod.Unknown)
			{
				if (!this.DecideDefaultOutput(record.MainNode))
				{
					this.CacheRecord(record);
				}
				else
				{
					this.OutputCachedRecords();
					this.OutputRecord(record);
				}
			}
			else
			{
				this.OutputRecord(record);
			}
			record.Reset();
			return Processor.OutputResult.Continue;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00052C47 File Offset: 0x00051C47
		public void TheEnd()
		{
			this.OutputCachedRecords();
			this.Close();
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00052C58 File Offset: 0x00051C58
		private bool DecideDefaultOutput(BuilderInfo node)
		{
			XsltOutput.OutputMethod outputMethod = XsltOutput.OutputMethod.Xml;
			XmlNodeType nodeType = node.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				if (node.NamespaceURI.Length == 0 && string.Compare("html", node.LocalName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					outputMethod = XsltOutput.OutputMethod.Html;
				}
				goto IL_006F;
			case XmlNodeType.Attribute:
				return false;
			case XmlNodeType.Text:
				break;
			default:
				switch (nodeType)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					break;
				default:
					return false;
				}
				break;
			}
			if (this.xmlCharType.IsOnlyWhitespace(node.Value))
			{
				return false;
			}
			outputMethod = XsltOutput.OutputMethod.Xml;
			IL_006F:
			if (this.processor.SetDefaultOutput(outputMethod))
			{
				this.CacheOuptutProps(this.processor.Output);
			}
			return true;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00052CF4 File Offset: 0x00051CF4
		private void CacheRecord(RecordBuilder record)
		{
			if (this.outputCache == null)
			{
				this.outputCache = new ArrayList();
			}
			this.outputCache.Add(record.MainNode.Clone());
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00052D20 File Offset: 0x00051D20
		private void OutputCachedRecords()
		{
			if (this.outputCache == null)
			{
				return;
			}
			for (int i = 0; i < this.outputCache.Count; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)this.outputCache[i];
				this.OutputRecord(builderInfo);
			}
			this.outputCache = null;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00052D6C File Offset: 0x00051D6C
		private void OutputRecord(RecordBuilder record)
		{
			BuilderInfo mainNode = record.MainNode;
			if (this.outputXmlDecl)
			{
				this.WriteXmlDeclaration();
			}
			switch (mainNode.NodeType)
			{
			case XmlNodeType.Element:
				this.WriteStartElement(record);
				return;
			case XmlNodeType.Attribute:
			case XmlNodeType.CDATA:
			case XmlNodeType.Entity:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				break;
			case XmlNodeType.Text:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.WriteTextNode(record);
				return;
			case XmlNodeType.EntityReference:
				this.Write('&');
				this.WriteName(mainNode.Prefix, mainNode.LocalName);
				this.Write(';');
				return;
			case XmlNodeType.ProcessingInstruction:
				this.WriteProcessingInstruction(record);
				return;
			case XmlNodeType.Comment:
				this.Indent(record);
				this.Write("<!--");
				this.Write(mainNode.Value);
				this.Write("-->");
				return;
			case XmlNodeType.DocumentType:
				this.Write(mainNode.Value);
				return;
			case XmlNodeType.EndElement:
				this.WriteEndElement(record);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00052E54 File Offset: 0x00051E54
		private void OutputRecord(BuilderInfo node)
		{
			if (this.outputXmlDecl)
			{
				this.WriteXmlDeclaration();
			}
			this.Indent(0);
			switch (node.NodeType)
			{
			case XmlNodeType.Element:
			case XmlNodeType.Attribute:
			case XmlNodeType.CDATA:
			case XmlNodeType.Entity:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
			case XmlNodeType.EndElement:
				break;
			case XmlNodeType.Text:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.WriteTextNode(node);
				return;
			case XmlNodeType.EntityReference:
				this.Write('&');
				this.WriteName(node.Prefix, node.LocalName);
				this.Write(';');
				return;
			case XmlNodeType.ProcessingInstruction:
				this.WriteProcessingInstruction(node);
				return;
			case XmlNodeType.Comment:
				this.Write("<!--");
				this.Write(node.Value);
				this.Write("-->");
				return;
			case XmlNodeType.DocumentType:
				this.Write(node.Value);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00052F24 File Offset: 0x00051F24
		private void WriteName(string prefix, string name)
		{
			if (prefix != null && prefix.Length > 0)
			{
				this.Write(prefix);
				if (name == null || name.Length <= 0)
				{
					return;
				}
				this.Write(':');
			}
			this.Write(name);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00052F57 File Offset: 0x00051F57
		private void WriteXmlAttributeValue(string value)
		{
			this.WriteWithReplace(value, SequentialOutput.s_XmlAttributeValueFind, SequentialOutput.s_XmlAttributeValueReplace);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00052F6C File Offset: 0x00051F6C
		private void WriteHtmlAttributeValue(string value)
		{
			int length = value.Length;
			int i = 0;
			while (i < length)
			{
				char c = value[i];
				i++;
				char c2 = c;
				if (c2 != '"')
				{
					if (c2 == '&')
					{
						if (i != length && value[i] == '{')
						{
							this.Write(c);
						}
						else
						{
							this.Write("&amp;");
						}
					}
					else
					{
						this.Write(c);
					}
				}
				else
				{
					this.Write("&quot;");
				}
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00052FDC File Offset: 0x00051FDC
		private void WriteHtmlUri(string value)
		{
			int length = value.Length;
			int i = 0;
			while (i < length)
			{
				char c = value[i];
				i++;
				char c2 = c;
				if (c2 <= '\r')
				{
					if (c2 == '\n')
					{
						this.Write("&#xA;");
						continue;
					}
					if (c2 == '\r')
					{
						this.Write("&#xD;");
						continue;
					}
				}
				else
				{
					if (c2 == '"')
					{
						this.Write("&quot;");
						continue;
					}
					if (c2 == '&')
					{
						if (i != length && value[i] == '{')
						{
							this.Write(c);
							continue;
						}
						this.Write("&amp;");
						continue;
					}
				}
				if ('\u007f' < c)
				{
					if (this.utf8Encoding == null)
					{
						this.utf8Encoding = Encoding.UTF8;
						this.byteBuffer = new byte[this.utf8Encoding.GetMaxByteCount(1)];
					}
					int bytes = this.utf8Encoding.GetBytes(value, i - 1, 1, this.byteBuffer, 0);
					for (int j = 0; j < bytes; j++)
					{
						this.Write("%");
						uint num = (uint)this.byteBuffer[j];
						this.Write(num.ToString("X2", CultureInfo.InvariantCulture));
					}
				}
				else
				{
					this.Write(c);
				}
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00053118 File Offset: 0x00052118
		private void WriteWithReplace(string value, char[] find, string[] replace)
		{
			int length = value.Length;
			int i;
			for (i = 0; i < length; i++)
			{
				int num = value.IndexOfAny(find, i);
				if (num == -1)
				{
					break;
				}
				while (i < num)
				{
					this.Write(value[i]);
					i++;
				}
				char c = value[i];
				int num2 = find.Length - 1;
				while (0 <= num2)
				{
					if (find[num2] == c)
					{
						this.Write(replace[num2]);
						break;
					}
					num2--;
				}
			}
			if (i == 0)
			{
				this.Write(value);
				return;
			}
			while (i < length)
			{
				this.Write(value[i]);
				i++;
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x000531AB File Offset: 0x000521AB
		private void WriteCData(string value)
		{
			this.Write(value.Replace("]]>", "]]]]><![CDATA[>"));
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000531C4 File Offset: 0x000521C4
		private void WriteAttributes(ArrayList list, int count, HtmlElementProps htmlElementsProps)
		{
			for (int i = 0; i < count; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)list[i];
				string value = builderInfo.Value;
				bool flag = false;
				bool flag2 = false;
				if (htmlElementsProps != null && builderInfo.Prefix.Length == 0)
				{
					HtmlAttributeProps htmlAttributeProps = builderInfo.htmlAttrProps;
					if (htmlAttributeProps == null && builderInfo.search)
					{
						htmlAttributeProps = HtmlAttributeProps.GetProps(builderInfo.LocalName);
					}
					if (htmlAttributeProps != null)
					{
						flag = htmlElementsProps.AbrParent && htmlAttributeProps.Abr;
						flag2 = htmlElementsProps.UriParent && (htmlAttributeProps.Uri || (htmlElementsProps.NameParent && htmlAttributeProps.Name));
					}
				}
				this.Write(' ');
				this.WriteName(builderInfo.Prefix, builderInfo.LocalName);
				if (!flag || string.Compare(builderInfo.LocalName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.Write("=\"");
					if (flag2)
					{
						this.WriteHtmlUri(value);
					}
					else if (this.isHtmlOutput)
					{
						this.WriteHtmlAttributeValue(value);
					}
					else
					{
						this.WriteXmlAttributeValue(value);
					}
					this.Write('"');
				}
			}
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x000532D3 File Offset: 0x000522D3
		private void Indent(RecordBuilder record)
		{
			if (!record.Manager.CurrentElementScope.Mixed)
			{
				this.Indent(record.MainNode.Depth);
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000532F8 File Offset: 0x000522F8
		private void Indent(int depth)
		{
			if (this.firstLine)
			{
				if (this.indentOutput)
				{
					this.firstLine = false;
				}
				return;
			}
			this.Write("\r\n");
			int num = 2 * depth;
			while (0 < num)
			{
				this.Write(" ");
				num--;
			}
		}

		// Token: 0x0600113F RID: 4415
		internal abstract void Write(char outputChar);

		// Token: 0x06001140 RID: 4416
		internal abstract void Write(string outputText);

		// Token: 0x06001141 RID: 4417
		internal abstract void Close();

		// Token: 0x04000B59 RID: 2905
		private const char s_Colon = ':';

		// Token: 0x04000B5A RID: 2906
		private const char s_GreaterThan = '>';

		// Token: 0x04000B5B RID: 2907
		private const char s_LessThan = '<';

		// Token: 0x04000B5C RID: 2908
		private const char s_Space = ' ';

		// Token: 0x04000B5D RID: 2909
		private const char s_Quote = '"';

		// Token: 0x04000B5E RID: 2910
		private const char s_Semicolon = ';';

		// Token: 0x04000B5F RID: 2911
		private const char s_NewLine = '\n';

		// Token: 0x04000B60 RID: 2912
		private const char s_Return = '\r';

		// Token: 0x04000B61 RID: 2913
		private const char s_Ampersand = '&';

		// Token: 0x04000B62 RID: 2914
		private const string s_LessThanQuestion = "<?";

		// Token: 0x04000B63 RID: 2915
		private const string s_QuestionGreaterThan = "?>";

		// Token: 0x04000B64 RID: 2916
		private const string s_LessThanSlash = "</";

		// Token: 0x04000B65 RID: 2917
		private const string s_SlashGreaterThan = " />";

		// Token: 0x04000B66 RID: 2918
		private const string s_EqualQuote = "=\"";

		// Token: 0x04000B67 RID: 2919
		private const string s_DocType = "<!DOCTYPE ";

		// Token: 0x04000B68 RID: 2920
		private const string s_CommentBegin = "<!--";

		// Token: 0x04000B69 RID: 2921
		private const string s_CommentEnd = "-->";

		// Token: 0x04000B6A RID: 2922
		private const string s_CDataBegin = "<![CDATA[";

		// Token: 0x04000B6B RID: 2923
		private const string s_CDataEnd = "]]>";

		// Token: 0x04000B6C RID: 2924
		private const string s_VersionAll = " version=\"1.0\"";

		// Token: 0x04000B6D RID: 2925
		private const string s_Standalone = " standalone=\"";

		// Token: 0x04000B6E RID: 2926
		private const string s_EncodingStart = " encoding=\"";

		// Token: 0x04000B6F RID: 2927
		private const string s_Public = "PUBLIC ";

		// Token: 0x04000B70 RID: 2928
		private const string s_System = "SYSTEM ";

		// Token: 0x04000B71 RID: 2929
		private const string s_Html = "html";

		// Token: 0x04000B72 RID: 2930
		private const string s_QuoteSpace = "\" ";

		// Token: 0x04000B73 RID: 2931
		private const string s_CDataSplit = "]]]]><![CDATA[>";

		// Token: 0x04000B74 RID: 2932
		private const string s_EnLessThan = "&lt;";

		// Token: 0x04000B75 RID: 2933
		private const string s_EnGreaterThan = "&gt;";

		// Token: 0x04000B76 RID: 2934
		private const string s_EnAmpersand = "&amp;";

		// Token: 0x04000B77 RID: 2935
		private const string s_EnQuote = "&quot;";

		// Token: 0x04000B78 RID: 2936
		private const string s_EnNewLine = "&#xA;";

		// Token: 0x04000B79 RID: 2937
		private const string s_EnReturn = "&#xD;";

		// Token: 0x04000B7A RID: 2938
		private const string s_EndOfLine = "\r\n";

		// Token: 0x04000B7B RID: 2939
		private static char[] s_TextValueFind = new char[] { '&', '>', '<' };

		// Token: 0x04000B7C RID: 2940
		private static string[] s_TextValueReplace = new string[] { "&amp;", "&gt;", "&lt;" };

		// Token: 0x04000B7D RID: 2941
		private static char[] s_XmlAttributeValueFind = new char[] { '&', '>', '<', '"', '\n', '\r' };

		// Token: 0x04000B7E RID: 2942
		private static string[] s_XmlAttributeValueReplace = new string[] { "&amp;", "&gt;", "&lt;", "&quot;", "&#xA;", "&#xD;" };

		// Token: 0x04000B7F RID: 2943
		private Processor processor;

		// Token: 0x04000B80 RID: 2944
		protected Encoding encoding;

		// Token: 0x04000B81 RID: 2945
		private ArrayList outputCache;

		// Token: 0x04000B82 RID: 2946
		private bool firstLine = true;

		// Token: 0x04000B83 RID: 2947
		private bool secondRoot;

		// Token: 0x04000B84 RID: 2948
		private XsltOutput output;

		// Token: 0x04000B85 RID: 2949
		private bool isHtmlOutput;

		// Token: 0x04000B86 RID: 2950
		private bool isXmlOutput;

		// Token: 0x04000B87 RID: 2951
		private Hashtable cdataElements;

		// Token: 0x04000B88 RID: 2952
		private bool indentOutput;

		// Token: 0x04000B89 RID: 2953
		private bool outputDoctype;

		// Token: 0x04000B8A RID: 2954
		private bool outputXmlDecl;

		// Token: 0x04000B8B RID: 2955
		private bool omitXmlDeclCalled;

		// Token: 0x04000B8C RID: 2956
		private byte[] byteBuffer;

		// Token: 0x04000B8D RID: 2957
		private Encoding utf8Encoding;

		// Token: 0x04000B8E RID: 2958
		private XmlCharType xmlCharType = XmlCharType.Instance;
	}
}
