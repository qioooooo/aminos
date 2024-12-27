using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000161 RID: 353
	internal sealed class VariableQuery : ExtensionQuery
	{
		// Token: 0x0600131F RID: 4895 RVA: 0x00053009 File Offset: 0x00052009
		public VariableQuery(string name, string prefix)
			: base(prefix, name)
		{
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00053013 File Offset: 0x00052013
		private VariableQuery(VariableQuery other)
			: base(other)
		{
			this.variable = other.variable;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00053028 File Offset: 0x00052028
		public override void SetXsltContext(XsltContext context)
		{
			if (context == null)
			{
				throw XPathException.Create("Xp_NoContext");
			}
			if (this.xsltContext != context)
			{
				this.xsltContext = context;
				this.variable = this.xsltContext.ResolveVariable(this.prefix, this.name);
				if (this.variable == null)
				{
					throw XPathException.Create("Xp_UndefVar", base.QName);
				}
			}
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00053089 File Offset: 0x00052089
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			if (this.xsltContext == null)
			{
				throw XPathException.Create("Xp_NoContext");
			}
			return base.ProcessResult(this.variable.Evaluate(this.xsltContext));
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001323 RID: 4899 RVA: 0x000530B8 File Offset: 0x000520B8
		public override XPathResultType StaticType
		{
			get
			{
				if (this.variable != null)
				{
					return base.GetXPathType(this.Evaluate(null));
				}
				XPathResultType xpathResultType = ((this.variable != null) ? this.variable.VariableType : XPathResultType.Any);
				if (xpathResultType == XPathResultType.Error)
				{
					xpathResultType = XPathResultType.Any;
				}
				return xpathResultType;
			}
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x000530F9 File Offset: 0x000520F9
		public override XPathNodeIterator Clone()
		{
			return new VariableQuery(this);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00053104 File Offset: 0x00052104
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("name", (this.prefix.Length != 0) ? (this.prefix + ':' + this.name) : this.name);
			w.WriteEndElement();
		}

		// Token: 0x04000BE3 RID: 3043
		private IXsltContextVariable variable;
	}
}
