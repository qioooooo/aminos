using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200012A RID: 298
	internal sealed class BooleanFunctions : ValueQuery
	{
		// Token: 0x06001185 RID: 4485 RVA: 0x0004DE65 File Offset: 0x0004CE65
		public BooleanFunctions(Function.FunctionType funcType, Query arg)
		{
			this.arg = arg;
			this.funcType = funcType;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0004DE7B File Offset: 0x0004CE7B
		private BooleanFunctions(BooleanFunctions other)
			: base(other)
		{
			this.arg = Query.Clone(other.arg);
			this.funcType = other.funcType;
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0004DEA1 File Offset: 0x0004CEA1
		public override void SetXsltContext(XsltContext context)
		{
			if (this.arg != null)
			{
				this.arg.SetXsltContext(context);
			}
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0004DEB8 File Offset: 0x0004CEB8
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			Function.FunctionType functionType = this.funcType;
			switch (functionType)
			{
			case Function.FunctionType.FuncBoolean:
				return this.toBoolean(nodeIterator);
			case Function.FunctionType.FuncNumber:
				break;
			case Function.FunctionType.FuncTrue:
				return true;
			case Function.FunctionType.FuncFalse:
				return false;
			case Function.FunctionType.FuncNot:
				return this.Not(nodeIterator);
			default:
				if (functionType == Function.FunctionType.FuncLang)
				{
					return this.Lang(nodeIterator);
				}
				break;
			}
			return false;
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0004DF2A File Offset: 0x0004CF2A
		internal static bool toBoolean(double number)
		{
			return number != 0.0 && !double.IsNaN(number);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0004DF43 File Offset: 0x0004CF43
		internal static bool toBoolean(string str)
		{
			return str.Length > 0;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0004DF50 File Offset: 0x0004CF50
		internal bool toBoolean(XPathNodeIterator nodeIterator)
		{
			object obj = this.arg.Evaluate(nodeIterator);
			if (obj is XPathNodeIterator)
			{
				return this.arg.Advance() != null;
			}
			if (obj is string)
			{
				return BooleanFunctions.toBoolean((string)obj);
			}
			if (obj is double)
			{
				return BooleanFunctions.toBoolean((double)obj);
			}
			return !(obj is bool) || (bool)obj;
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x0004DFBC File Offset: 0x0004CFBC
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.Boolean;
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0004DFBF File Offset: 0x0004CFBF
		private bool Not(XPathNodeIterator nodeIterator)
		{
			return !(bool)this.arg.Evaluate(nodeIterator);
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0004DFD8 File Offset: 0x0004CFD8
		private bool Lang(XPathNodeIterator nodeIterator)
		{
			string text = this.arg.Evaluate(nodeIterator).ToString();
			string xmlLang = nodeIterator.Current.XmlLang;
			return xmlLang.StartsWith(text, StringComparison.OrdinalIgnoreCase) && (xmlLang.Length == text.Length || xmlLang[text.Length] == '-');
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004E02F File Offset: 0x0004D02F
		public override XPathNodeIterator Clone()
		{
			return new BooleanFunctions(this);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0004E038 File Offset: 0x0004D038
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

		// Token: 0x04000B40 RID: 2880
		private Query arg;

		// Token: 0x04000B41 RID: 2881
		private Function.FunctionType funcType;
	}
}
