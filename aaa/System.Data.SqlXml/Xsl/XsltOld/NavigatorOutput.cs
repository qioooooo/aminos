using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000183 RID: 387
	internal class NavigatorOutput : RecordOutput
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x0004F39D File Offset: 0x0004E39D
		internal XPathNavigator Navigator
		{
			get
			{
				return ((IXPathNavigable)this.doc).CreateNavigator();
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0004F3AA File Offset: 0x0004E3AA
		internal NavigatorOutput(string baseUri)
		{
			this.doc = new XPathDocument();
			this.wr = this.doc.LoadFromWriter(XPathDocument.LoadFlags.AtomizeNames, baseUri);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0004F3D0 File Offset: 0x0004E3D0
		public Processor.OutputResult RecordDone(RecordBuilder record)
		{
			BuilderInfo mainNode = record.MainNode;
			this.documentIndex++;
			switch (mainNode.NodeType)
			{
			case XmlNodeType.Element:
			{
				this.wr.WriteStartElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
				for (int i = 0; i < record.AttributeCount; i++)
				{
					this.documentIndex++;
					BuilderInfo builderInfo = (BuilderInfo)record.AttributeList[i];
					if (builderInfo.NamespaceURI == "http://www.w3.org/2000/xmlns/")
					{
						if (builderInfo.Prefix.Length == 0)
						{
							this.wr.WriteNamespaceDeclaration(string.Empty, builderInfo.Value);
						}
						else
						{
							this.wr.WriteNamespaceDeclaration(builderInfo.LocalName, builderInfo.Value);
						}
					}
					else
					{
						this.wr.WriteAttributeString(builderInfo.Prefix, builderInfo.LocalName, builderInfo.NamespaceURI, builderInfo.Value);
					}
				}
				this.wr.StartElementContent();
				if (mainNode.IsEmptyTag)
				{
					this.wr.WriteEndElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
				}
				break;
			}
			case XmlNodeType.Text:
				this.wr.WriteString(mainNode.Value);
				break;
			case XmlNodeType.ProcessingInstruction:
				this.wr.WriteProcessingInstruction(mainNode.LocalName, mainNode.Value);
				break;
			case XmlNodeType.Comment:
				this.wr.WriteComment(mainNode.Value);
				break;
			case XmlNodeType.SignificantWhitespace:
				this.wr.WriteString(mainNode.Value);
				break;
			case XmlNodeType.EndElement:
				this.wr.WriteEndElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
				break;
			}
			record.Reset();
			return Processor.OutputResult.Continue;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0004F5B4 File Offset: 0x0004E5B4
		public void TheEnd()
		{
			this.wr.Close();
		}

		// Token: 0x04000AE4 RID: 2788
		private XPathDocument doc;

		// Token: 0x04000AE5 RID: 2789
		private int documentIndex;

		// Token: 0x04000AE6 RID: 2790
		private XmlRawWriter wr;
	}
}
