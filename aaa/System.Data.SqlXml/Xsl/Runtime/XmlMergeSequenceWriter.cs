using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000BF RID: 191
	internal class XmlMergeSequenceWriter : XmlSequenceWriter
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x0002BF2C File Offset: 0x0002AF2C
		public XmlMergeSequenceWriter(XmlRawWriter xwrt)
		{
			this.xwrt = xwrt;
			this.lastItemWasAtomic = false;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002BF44 File Offset: 0x0002AF44
		public override XmlRawWriter StartTree(XPathNodeType rootType, IXmlNamespaceResolver nsResolver, XmlNameTable nameTable)
		{
			if (rootType == XPathNodeType.Attribute || rootType == XPathNodeType.Namespace)
			{
				throw new XslTransformException("XmlIl_TopLevelAttrNmsp", new string[] { string.Empty });
			}
			this.xwrt.NamespaceResolver = nsResolver;
			return this.xwrt;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002BF86 File Offset: 0x0002AF86
		public override void EndTree()
		{
			this.lastItemWasAtomic = false;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002BF90 File Offset: 0x0002AF90
		public override void WriteItem(XPathItem item)
		{
			if (!item.IsNode)
			{
				this.WriteString(item.Value);
				return;
			}
			XPathNavigator xpathNavigator = item as XPathNavigator;
			if (xpathNavigator.NodeType == XPathNodeType.Attribute || xpathNavigator.NodeType == XPathNodeType.Namespace)
			{
				throw new XslTransformException("XmlIl_TopLevelAttrNmsp", new string[] { string.Empty });
			}
			this.CopyNode(xpathNavigator);
			this.lastItemWasAtomic = false;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002BFF4 File Offset: 0x0002AFF4
		private void WriteString(string value)
		{
			if (this.lastItemWasAtomic)
			{
				this.xwrt.WriteWhitespace(" ");
			}
			else
			{
				this.lastItemWasAtomic = true;
			}
			this.xwrt.WriteString(value);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002C024 File Offset: 0x0002B024
		private void CopyNode(XPathNavigator nav)
		{
			int num = 0;
			for (;;)
			{
				IL_0002:
				if (this.CopyShallowNode(nav))
				{
					XPathNodeType nodeType = nav.NodeType;
					if (nodeType == XPathNodeType.Element)
					{
						if (nav.MoveToFirstAttribute())
						{
							do
							{
								this.CopyShallowNode(nav);
							}
							while (nav.MoveToNextAttribute());
							nav.MoveToParent();
						}
						XPathNamespaceScope xpathNamespaceScope = ((num == 0) ? XPathNamespaceScope.ExcludeXml : XPathNamespaceScope.Local);
						if (nav.MoveToFirstNamespace(xpathNamespaceScope))
						{
							this.CopyNamespaces(nav, xpathNamespaceScope);
							nav.MoveToParent();
						}
						this.xwrt.StartElementContent();
					}
					if (nav.MoveToFirstChild())
					{
						num++;
						continue;
					}
					if (nav.NodeType == XPathNodeType.Element)
					{
						this.xwrt.WriteEndElement(nav.Prefix, nav.LocalName, nav.NamespaceURI);
					}
				}
				while (num != 0)
				{
					if (nav.MoveToNext())
					{
						goto IL_0002;
					}
					num--;
					nav.MoveToParent();
					if (nav.NodeType == XPathNodeType.Element)
					{
						this.xwrt.WriteFullEndElement(nav.Prefix, nav.LocalName, nav.NamespaceURI);
					}
				}
				break;
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002C10C File Offset: 0x0002B10C
		private bool CopyShallowNode(XPathNavigator nav)
		{
			bool flag = false;
			switch (nav.NodeType)
			{
			case XPathNodeType.Root:
				flag = true;
				break;
			case XPathNodeType.Element:
				this.xwrt.WriteStartElement(nav.Prefix, nav.LocalName, nav.NamespaceURI);
				flag = true;
				break;
			case XPathNodeType.Attribute:
				this.xwrt.WriteStartAttribute(nav.Prefix, nav.LocalName, nav.NamespaceURI);
				this.xwrt.WriteString(nav.Value);
				this.xwrt.WriteEndAttribute();
				break;
			case XPathNodeType.Namespace:
				this.xwrt.WriteNamespaceDeclaration(nav.LocalName, nav.Value);
				break;
			case XPathNodeType.Text:
				this.xwrt.WriteString(nav.Value);
				break;
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				this.xwrt.WriteWhitespace(nav.Value);
				break;
			case XPathNodeType.ProcessingInstruction:
				this.xwrt.WriteProcessingInstruction(nav.LocalName, nav.Value);
				break;
			case XPathNodeType.Comment:
				this.xwrt.WriteComment(nav.Value);
				break;
			}
			return flag;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002C220 File Offset: 0x0002B220
		private void CopyNamespaces(XPathNavigator nav, XPathNamespaceScope nsScope)
		{
			string localName = nav.LocalName;
			string value = nav.Value;
			if (nav.MoveToNextNamespace(nsScope))
			{
				this.CopyNamespaces(nav, nsScope);
			}
			this.xwrt.WriteNamespaceDeclaration(localName, value);
		}

		// Token: 0x040005BD RID: 1469
		private XmlRawWriter xwrt;

		// Token: 0x040005BE RID: 1470
		private bool lastItemWasAtomic;
	}
}
