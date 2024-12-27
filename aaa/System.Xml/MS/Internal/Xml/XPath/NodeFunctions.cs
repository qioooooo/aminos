using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200014C RID: 332
	internal sealed class NodeFunctions : ValueQuery
	{
		// Token: 0x06001286 RID: 4742 RVA: 0x00050C06 File Offset: 0x0004FC06
		public NodeFunctions(Function.FunctionType funcType, Query arg)
		{
			this.funcType = funcType;
			this.arg = arg;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00050C1C File Offset: 0x0004FC1C
		public override void SetXsltContext(XsltContext context)
		{
			this.xsltContext = (context.Whitespace ? context : null);
			if (this.arg != null)
			{
				this.arg.SetXsltContext(context);
			}
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00050C44 File Offset: 0x0004FC44
		private XPathNavigator EvaluateArg(XPathNodeIterator context)
		{
			if (this.arg == null)
			{
				return context.Current;
			}
			this.arg.Evaluate(context);
			return this.arg.Advance();
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00050C70 File Offset: 0x0004FC70
		public override object Evaluate(XPathNodeIterator context)
		{
			switch (this.funcType)
			{
			case Function.FunctionType.FuncLast:
				return (double)context.Count;
			case Function.FunctionType.FuncPosition:
				return (double)context.CurrentPosition;
			case Function.FunctionType.FuncCount:
			{
				this.arg.Evaluate(context);
				int num = 0;
				if (this.xsltContext != null)
				{
					XPathNavigator xpathNavigator;
					while ((xpathNavigator = this.arg.Advance()) != null)
					{
						if (xpathNavigator.NodeType != XPathNodeType.Whitespace || this.xsltContext.PreserveWhitespace(xpathNavigator))
						{
							num++;
						}
					}
				}
				else
				{
					while (this.arg.Advance() != null)
					{
						num++;
					}
				}
				return (double)num;
			}
			case Function.FunctionType.FuncLocalName:
			{
				XPathNavigator xpathNavigator2 = this.EvaluateArg(context);
				if (xpathNavigator2 != null)
				{
					return xpathNavigator2.LocalName;
				}
				break;
			}
			case Function.FunctionType.FuncNameSpaceUri:
			{
				XPathNavigator xpathNavigator2 = this.EvaluateArg(context);
				if (xpathNavigator2 != null)
				{
					return xpathNavigator2.NamespaceURI;
				}
				break;
			}
			case Function.FunctionType.FuncName:
			{
				XPathNavigator xpathNavigator2 = this.EvaluateArg(context);
				if (xpathNavigator2 != null)
				{
					return xpathNavigator2.Name;
				}
				break;
			}
			}
			return string.Empty;
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x00050D61 File Offset: 0x0004FD61
		public override XPathResultType StaticType
		{
			get
			{
				return Function.ReturnTypes[(int)this.funcType];
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00050D70 File Offset: 0x0004FD70
		public override XPathNodeIterator Clone()
		{
			return new NodeFunctions(this.funcType, Query.Clone(this.arg))
			{
				xsltContext = this.xsltContext
			};
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00050DA4 File Offset: 0x0004FDA4
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("name", this.funcType.ToString());
			if (this.arg != null)
			{
				this.arg.PrintQuery(w);
			}
			w.WriteEndElement();
		}

		// Token: 0x04000B9C RID: 2972
		private Query arg;

		// Token: 0x04000B9D RID: 2973
		private Function.FunctionType funcType;

		// Token: 0x04000B9E RID: 2974
		private XsltContext xsltContext;
	}
}
