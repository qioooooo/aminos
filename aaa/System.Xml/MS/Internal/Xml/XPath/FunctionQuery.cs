using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200013A RID: 314
	internal sealed class FunctionQuery : ExtensionQuery
	{
		// Token: 0x060011FF RID: 4607 RVA: 0x0004F0A2 File Offset: 0x0004E0A2
		public FunctionQuery(string prefix, string name, List<Query> args)
			: base(prefix, name)
		{
			this.args = args;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0004F0B4 File Offset: 0x0004E0B4
		private FunctionQuery(FunctionQuery other)
			: base(other)
		{
			this.function = other.function;
			Query[] array = new Query[other.args.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Query.Clone(other.args[i]);
			}
			this.args = array;
			this.args = array;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0004F118 File Offset: 0x0004E118
		public override void SetXsltContext(XsltContext context)
		{
			if (context == null)
			{
				throw XPathException.Create("Xp_NoContext");
			}
			if (this.xsltContext != context)
			{
				this.xsltContext = context;
				foreach (Query query in this.args)
				{
					query.SetXsltContext(context);
				}
				XPathResultType[] array = new XPathResultType[this.args.Count];
				for (int i = 0; i < this.args.Count; i++)
				{
					array[i] = this.args[i].StaticType;
				}
				this.function = this.xsltContext.ResolveFunction(this.prefix, this.name, array);
				if (this.function == null)
				{
					throw XPathException.Create("Xp_UndefFunc", base.QName);
				}
			}
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0004F1F8 File Offset: 0x0004E1F8
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			if (this.xsltContext == null)
			{
				throw XPathException.Create("Xp_NoContext");
			}
			object[] array = new object[this.args.Count];
			for (int i = 0; i < this.args.Count; i++)
			{
				array[i] = this.args[i].Evaluate(nodeIterator);
				if (array[i] is XPathNodeIterator)
				{
					array[i] = new XPathSelectionIterator(nodeIterator.Current, this.args[i]);
				}
			}
			object obj;
			try
			{
				obj = base.ProcessResult(this.function.Invoke(this.xsltContext, array, nodeIterator.Current));
			}
			catch (Exception ex)
			{
				throw XPathException.Create("Xp_FunctionFailed", base.QName, ex);
			}
			return obj;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0004F2C0 File Offset: 0x0004E2C0
		public override XPathNavigator MatchNode(XPathNavigator navigator)
		{
			if (this.name != "key" && this.prefix.Length != 0)
			{
				throw XPathException.Create("Xp_InvalidPattern");
			}
			this.Evaluate(new XPathSingletonIterator(navigator, true));
			XPathNavigator xpathNavigator;
			while ((xpathNavigator = this.Advance()) != null)
			{
				if (xpathNavigator.IsSamePosition(navigator))
				{
					return xpathNavigator;
				}
			}
			return xpathNavigator;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0004F320 File Offset: 0x0004E320
		public override XPathResultType StaticType
		{
			get
			{
				XPathResultType xpathResultType = ((this.function != null) ? this.function.ReturnType : XPathResultType.Any);
				if (xpathResultType == XPathResultType.Error)
				{
					xpathResultType = XPathResultType.Any;
				}
				return xpathResultType;
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0004F34B File Offset: 0x0004E34B
		public override XPathNodeIterator Clone()
		{
			return new FunctionQuery(this);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0004F354 File Offset: 0x0004E354
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("name", (this.prefix.Length != 0) ? (this.prefix + ':' + this.name) : this.name);
			foreach (Query query in this.args)
			{
				query.PrintQuery(w);
			}
			w.WriteEndElement();
		}

		// Token: 0x04000B5A RID: 2906
		private IList<Query> args;

		// Token: 0x04000B5B RID: 2907
		private IXsltContextFunction function;
	}
}
