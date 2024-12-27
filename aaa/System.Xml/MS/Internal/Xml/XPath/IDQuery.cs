using System;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000144 RID: 324
	internal sealed class IDQuery : CacheOutputQuery
	{
		// Token: 0x0600123E RID: 4670 RVA: 0x0004FD74 File Offset: 0x0004ED74
		public IDQuery(Query arg)
			: base(arg)
		{
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0004FD7D File Offset: 0x0004ED7D
		private IDQuery(IDQuery other)
			: base(other)
		{
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0004FD88 File Offset: 0x0004ED88
		public override object Evaluate(XPathNodeIterator context)
		{
			object obj = base.Evaluate(context);
			XPathNavigator xpathNavigator = context.Current.Clone();
			switch (base.GetXPathType(obj))
			{
			case XPathResultType.Number:
				this.ProcessIds(xpathNavigator, StringFunctions.toString((double)obj));
				break;
			case XPathResultType.String:
				this.ProcessIds(xpathNavigator, (string)obj);
				break;
			case XPathResultType.Boolean:
				this.ProcessIds(xpathNavigator, StringFunctions.toString((bool)obj));
				break;
			case XPathResultType.NodeSet:
			{
				XPathNavigator xpathNavigator2;
				while ((xpathNavigator2 = this.input.Advance()) != null)
				{
					this.ProcessIds(xpathNavigator, xpathNavigator2.Value);
				}
				break;
			}
			case (XPathResultType)4:
				this.ProcessIds(xpathNavigator, ((XPathNavigator)obj).Value);
				break;
			}
			return this;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0004FE38 File Offset: 0x0004EE38
		private void ProcessIds(XPathNavigator contextNode, string val)
		{
			string[] array = XmlConvert.SplitString(val);
			for (int i = 0; i < array.Length; i++)
			{
				if (contextNode.MoveToId(array[i]))
				{
					base.Insert(this.outputBuffer, contextNode);
				}
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0004FE74 File Offset: 0x0004EE74
		public override XPathNavigator MatchNode(XPathNavigator context)
		{
			this.Evaluate(new XPathSingletonIterator(context, true));
			XPathNavigator xpathNavigator;
			while ((xpathNavigator = this.Advance()) != null)
			{
				if (xpathNavigator.IsSamePosition(context))
				{
					return context;
				}
			}
			return null;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0004FEA7 File Offset: 0x0004EEA7
		public override XPathNodeIterator Clone()
		{
			return new IDQuery(this);
		}
	}
}
