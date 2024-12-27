using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200013A RID: 314
	internal class BeginEvent : Event
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x000473C4 File Offset: 0x000463C4
		public BeginEvent(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			this.nodeType = input.NodeType;
			this.namespaceUri = input.NamespaceURI;
			this.name = input.LocalName;
			this.prefix = input.Prefix;
			this.empty = input.IsEmptyTag;
			if (this.nodeType == XPathNodeType.Element)
			{
				this.htmlProps = HtmlElementProps.GetProps(this.name);
				return;
			}
			if (this.nodeType == XPathNodeType.Attribute)
			{
				this.htmlProps = HtmlAttributeProps.GetProps(this.name);
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00047450 File Offset: 0x00046450
		public override void ReplaceNamespaceAlias(Compiler compiler)
		{
			if (this.nodeType == XPathNodeType.Attribute && this.namespaceUri.Length == 0)
			{
				return;
			}
			NamespaceInfo namespaceInfo = compiler.FindNamespaceAlias(this.namespaceUri);
			if (namespaceInfo != null)
			{
				this.namespaceUri = namespaceInfo.nameSpace;
				if (namespaceInfo.prefix != null)
				{
					this.prefix = namespaceInfo.prefix;
				}
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000474A4 File Offset: 0x000464A4
		public override bool Output(Processor processor, ActionFrame frame)
		{
			return processor.BeginEvent(this.nodeType, this.prefix, this.name, this.namespaceUri, this.empty, this.htmlProps, false);
		}

		// Token: 0x04000906 RID: 2310
		private XPathNodeType nodeType;

		// Token: 0x04000907 RID: 2311
		private string namespaceUri;

		// Token: 0x04000908 RID: 2312
		private string name;

		// Token: 0x04000909 RID: 2313
		private string prefix;

		// Token: 0x0400090A RID: 2314
		private bool empty;

		// Token: 0x0400090B RID: 2315
		private object htmlProps;
	}
}
