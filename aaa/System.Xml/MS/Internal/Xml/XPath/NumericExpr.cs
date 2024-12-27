using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200014E RID: 334
	internal sealed class NumericExpr : ValueQuery
	{
		// Token: 0x0600129B RID: 4763 RVA: 0x00051088 File Offset: 0x00050088
		public NumericExpr(Operator.Op op, Query opnd1, Query opnd2)
		{
			if (opnd1.StaticType != XPathResultType.Number)
			{
				opnd1 = new NumberFunctions(Function.FunctionType.FuncNumber, opnd1);
			}
			if (opnd2.StaticType != XPathResultType.Number)
			{
				opnd2 = new NumberFunctions(Function.FunctionType.FuncNumber, opnd2);
			}
			this.op = op;
			this.opnd1 = opnd1;
			this.opnd2 = opnd2;
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x000510D4 File Offset: 0x000500D4
		private NumericExpr(NumericExpr other)
			: base(other)
		{
			this.op = other.op;
			this.opnd1 = Query.Clone(other.opnd1);
			this.opnd2 = Query.Clone(other.opnd2);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0005110B File Offset: 0x0005010B
		public override void SetXsltContext(XsltContext context)
		{
			this.opnd1.SetXsltContext(context);
			this.opnd2.SetXsltContext(context);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00051125 File Offset: 0x00050125
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			return NumericExpr.GetValue(this.op, XmlConvert.ToXPathDouble(this.opnd1.Evaluate(nodeIterator)), XmlConvert.ToXPathDouble(this.opnd2.Evaluate(nodeIterator)));
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0005115C File Offset: 0x0005015C
		private static double GetValue(Operator.Op op, double n1, double n2)
		{
			switch (op)
			{
			case Operator.Op.PLUS:
				return n1 + n2;
			case Operator.Op.MINUS:
				return n1 - n2;
			case Operator.Op.MUL:
				return n1 * n2;
			case Operator.Op.MOD:
				return n1 % n2;
			case Operator.Op.DIV:
				return n1 / n2;
			default:
				return 0.0;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x000511A6 File Offset: 0x000501A6
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x000511A9 File Offset: 0x000501A9
		public override XPathNodeIterator Clone()
		{
			return new NumericExpr(this);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000511B4 File Offset: 0x000501B4
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("op", this.op.ToString());
			this.opnd1.PrintQuery(w);
			this.opnd2.PrintQuery(w);
			w.WriteEndElement();
		}

		// Token: 0x04000BA1 RID: 2977
		private Operator.Op op;

		// Token: 0x04000BA2 RID: 2978
		private Query opnd1;

		// Token: 0x04000BA3 RID: 2979
		private Query opnd2;
	}
}
