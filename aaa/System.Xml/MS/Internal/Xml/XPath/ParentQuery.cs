using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000153 RID: 339
	internal sealed class ParentQuery : CacheAxisQuery
	{
		// Token: 0x060012B4 RID: 4788 RVA: 0x00051318 File Offset: 0x00050318
		public ParentQuery(Query qyInput, string Name, string Prefix, XPathNodeType Type)
			: base(qyInput, Name, Prefix, Type)
		{
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00051325 File Offset: 0x00050325
		private ParentQuery(ParentQuery other)
			: base(other)
		{
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00051330 File Offset: 0x00050330
		public override object Evaluate(XPathNodeIterator context)
		{
			base.Evaluate(context);
			XPathNavigator xpathNavigator;
			while ((xpathNavigator = this.qyInput.Advance()) != null)
			{
				xpathNavigator = xpathNavigator.Clone();
				if (xpathNavigator.MoveToParent() && this.matches(xpathNavigator))
				{
					base.Insert(this.outputBuffer, xpathNavigator);
				}
			}
			return this;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0005137D File Offset: 0x0005037D
		public override XPathNodeIterator Clone()
		{
			return new ParentQuery(this);
		}
	}
}
