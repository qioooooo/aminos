using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200014D RID: 333
	internal sealed class NumberFunctions : ValueQuery
	{
		// Token: 0x0600128D RID: 4749 RVA: 0x00050DF7 File Offset: 0x0004FDF7
		public NumberFunctions(Function.FunctionType ftype, Query arg)
		{
			this.arg = arg;
			this.ftype = ftype;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00050E0D File Offset: 0x0004FE0D
		private NumberFunctions(NumberFunctions other)
			: base(other)
		{
			this.arg = Query.Clone(other.arg);
			this.ftype = other.ftype;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00050E33 File Offset: 0x0004FE33
		public override void SetXsltContext(XsltContext context)
		{
			if (this.arg != null)
			{
				this.arg.SetXsltContext(context);
			}
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00050E49 File Offset: 0x0004FE49
		internal static double Number(bool arg)
		{
			if (!arg)
			{
				return 0.0;
			}
			return 1.0;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00050E61 File Offset: 0x0004FE61
		internal static double Number(string arg)
		{
			return XmlConvert.ToXPathDouble(arg);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00050E6C File Offset: 0x0004FE6C
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			Function.FunctionType functionType = this.ftype;
			if (functionType == Function.FunctionType.FuncNumber)
			{
				return this.Number(nodeIterator);
			}
			switch (functionType)
			{
			case Function.FunctionType.FuncSum:
				return this.Sum(nodeIterator);
			case Function.FunctionType.FuncFloor:
				return this.Floor(nodeIterator);
			case Function.FunctionType.FuncCeiling:
				return this.Ceiling(nodeIterator);
			case Function.FunctionType.FuncRound:
				return this.Round(nodeIterator);
			default:
				return null;
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00050EE4 File Offset: 0x0004FEE4
		private double Number(XPathNodeIterator nodeIterator)
		{
			if (this.arg == null)
			{
				return XmlConvert.ToXPathDouble(nodeIterator.Current.Value);
			}
			object obj = this.arg.Evaluate(nodeIterator);
			switch (base.GetXPathType(obj))
			{
			case XPathResultType.Number:
				return (double)obj;
			case XPathResultType.String:
				return NumberFunctions.Number((string)obj);
			case XPathResultType.Boolean:
				return NumberFunctions.Number((bool)obj);
			case XPathResultType.NodeSet:
			{
				XPathNavigator xpathNavigator = this.arg.Advance();
				if (xpathNavigator != null)
				{
					return NumberFunctions.Number(xpathNavigator.Value);
				}
				break;
			}
			case (XPathResultType)4:
				return NumberFunctions.Number(((XPathNavigator)obj).Value);
			}
			return double.NaN;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00050F90 File Offset: 0x0004FF90
		private double Sum(XPathNodeIterator nodeIterator)
		{
			double num = 0.0;
			this.arg.Evaluate(nodeIterator);
			XPathNavigator xpathNavigator;
			while ((xpathNavigator = this.arg.Advance()) != null)
			{
				num += NumberFunctions.Number(xpathNavigator.Value);
			}
			return num;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00050FD4 File Offset: 0x0004FFD4
		private double Floor(XPathNodeIterator nodeIterator)
		{
			return Math.Floor((double)this.arg.Evaluate(nodeIterator));
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00050FEC File Offset: 0x0004FFEC
		private double Ceiling(XPathNodeIterator nodeIterator)
		{
			return Math.Ceiling((double)this.arg.Evaluate(nodeIterator));
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00051004 File Offset: 0x00050004
		private double Round(XPathNodeIterator nodeIterator)
		{
			double num = XmlConvert.ToXPathDouble(this.arg.Evaluate(nodeIterator));
			return XmlConvert.XPathRound(num);
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x00051029 File Offset: 0x00050029
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0005102C File Offset: 0x0005002C
		public override XPathNodeIterator Clone()
		{
			return new NumberFunctions(this);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00051034 File Offset: 0x00050034
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("name", this.ftype.ToString());
			if (this.arg != null)
			{
				this.arg.PrintQuery(w);
			}
			w.WriteEndElement();
		}

		// Token: 0x04000B9F RID: 2975
		private Query arg;

		// Token: 0x04000BA0 RID: 2976
		private Function.FunctionType ftype;
	}
}
