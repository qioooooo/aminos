using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000129 RID: 297
	internal sealed class BooleanExpr : ValueQuery
	{
		// Token: 0x0600117E RID: 4478 RVA: 0x0004DD24 File Offset: 0x0004CD24
		public BooleanExpr(Operator.Op op, Query opnd1, Query opnd2)
		{
			if (opnd1.StaticType != XPathResultType.Boolean)
			{
				opnd1 = new BooleanFunctions(Function.FunctionType.FuncBoolean, opnd1);
			}
			if (opnd2.StaticType != XPathResultType.Boolean)
			{
				opnd2 = new BooleanFunctions(Function.FunctionType.FuncBoolean, opnd2);
			}
			this.opnd1 = opnd1;
			this.opnd2 = opnd2;
			this.isOr = op == Operator.Op.OR;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0004DD73 File Offset: 0x0004CD73
		private BooleanExpr(BooleanExpr other)
			: base(other)
		{
			this.opnd1 = Query.Clone(other.opnd1);
			this.opnd2 = Query.Clone(other.opnd2);
			this.isOr = other.isOr;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0004DDAA File Offset: 0x0004CDAA
		public override void SetXsltContext(XsltContext context)
		{
			this.opnd1.SetXsltContext(context);
			this.opnd2.SetXsltContext(context);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0004DDC4 File Offset: 0x0004CDC4
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			object obj = this.opnd1.Evaluate(nodeIterator);
			if ((bool)obj == this.isOr)
			{
				return obj;
			}
			return this.opnd2.Evaluate(nodeIterator);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0004DDFA File Offset: 0x0004CDFA
		public override XPathNodeIterator Clone()
		{
			return new BooleanExpr(this);
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x0004DE02 File Offset: 0x0004CE02
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.Boolean;
			}
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0004DE08 File Offset: 0x0004CE08
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("op", (this.isOr ? Operator.Op.OR : Operator.Op.AND).ToString());
			this.opnd1.PrintQuery(w);
			this.opnd2.PrintQuery(w);
			w.WriteEndElement();
		}

		// Token: 0x04000B3D RID: 2877
		private Query opnd1;

		// Token: 0x04000B3E RID: 2878
		private Query opnd2;

		// Token: 0x04000B3F RID: 2879
		private bool isOr;
	}
}
