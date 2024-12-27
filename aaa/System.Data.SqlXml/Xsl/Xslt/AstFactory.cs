using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200011A RID: 282
	internal static class AstFactory
	{
		// Token: 0x06000BF3 RID: 3059 RVA: 0x0003D681 File Offset: 0x0003C681
		public static XslNode XslNode(XslNodeType nodeType, QilName name, string arg, XslVersion xslVer)
		{
			return new XslNode(nodeType, name, arg, xslVer);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0003D68C File Offset: 0x0003C68C
		public static XslNode ApplyImports(QilName mode, Stylesheet sheet, XslVersion xslVer)
		{
			return new XslNode(XslNodeType.ApplyImports, mode, sheet, xslVer);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0003D697 File Offset: 0x0003C697
		public static XslNodeEx ApplyTemplates(QilName mode, string select, XsltInput.ContextInfo ctxInfo, XslVersion xslVer)
		{
			return new XslNodeEx(XslNodeType.ApplyTemplates, mode, select, ctxInfo, xslVer);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0003D6A3 File Offset: 0x0003C6A3
		public static XslNodeEx ApplyTemplates(QilName mode)
		{
			return new XslNodeEx(XslNodeType.ApplyTemplates, mode, null, XslVersion.Version10);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0003D6AE File Offset: 0x0003C6AE
		public static NodeCtor Attribute(string nameAvt, string nsAvt, XslVersion xslVer)
		{
			return new NodeCtor(XslNodeType.Attribute, nameAvt, nsAvt, xslVer);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0003D6B9 File Offset: 0x0003C6B9
		public static AttributeSet AttributeSet(QilName name)
		{
			return new AttributeSet(name, XslVersion.Version10);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003D6C2 File Offset: 0x0003C6C2
		public static XslNodeEx CallTemplate(QilName name, XsltInput.ContextInfo ctxInfo)
		{
			return new XslNodeEx(XslNodeType.CallTemplate, name, null, ctxInfo, XslVersion.Version10);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0003D6CE File Offset: 0x0003C6CE
		public static XslNode Choose()
		{
			return new XslNode(XslNodeType.Choose);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0003D6D6 File Offset: 0x0003C6D6
		public static XslNode Comment()
		{
			return new XslNode(XslNodeType.Comment);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0003D6DE File Offset: 0x0003C6DE
		public static XslNode Copy()
		{
			return new XslNode(XslNodeType.Copy);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0003D6E6 File Offset: 0x0003C6E6
		public static XslNode CopyOf(string select, XslVersion xslVer)
		{
			return new XslNode(XslNodeType.CopyOf, null, select, xslVer);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0003D6F2 File Offset: 0x0003C6F2
		public static NodeCtor Element(string nameAvt, string nsAvt, XslVersion xslVer)
		{
			return new NodeCtor(XslNodeType.Element, nameAvt, nsAvt, xslVer);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0003D6FE File Offset: 0x0003C6FE
		public static XslNode Error(string message)
		{
			return new XslNode(XslNodeType.Error, null, message, XslVersion.Version10);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0003D70A File Offset: 0x0003C70A
		public static XslNodeEx ForEach(string select, XsltInput.ContextInfo ctxInfo, XslVersion xslVer)
		{
			return new XslNodeEx(XslNodeType.ForEach, null, select, ctxInfo, xslVer);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0003D717 File Offset: 0x0003C717
		public static XslNode If(string test, XslVersion xslVer)
		{
			return new XslNode(XslNodeType.If, null, test, xslVer);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0003D723 File Offset: 0x0003C723
		public static Key Key(QilName name, string match, string use, XslVersion xslVer)
		{
			return new Key(name, match, use, xslVer);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0003D72E File Offset: 0x0003C72E
		public static XslNode List()
		{
			return new XslNode(XslNodeType.List);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0003D737 File Offset: 0x0003C737
		public static XslNode LiteralAttribute(QilName name, string value, XslVersion xslVer)
		{
			return new XslNode(XslNodeType.LiteralAttribute, name, value, xslVer);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0003D743 File Offset: 0x0003C743
		public static XslNode LiteralElement(QilName name)
		{
			return new XslNode(XslNodeType.LiteralElement, name, null, XslVersion.Version10);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0003D74F File Offset: 0x0003C74F
		public static XslNode Message(bool term)
		{
			return new XslNode(XslNodeType.Message, null, term, XslVersion.Version10);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0003D760 File Offset: 0x0003C760
		public static XslNode Nop()
		{
			return new XslNode(XslNodeType.Nop);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0003D76C File Offset: 0x0003C76C
		public static Number Number(NumberLevel level, string count, string from, string value, string format, string lang, string letterValue, string groupingSeparator, string groupingSize, XslVersion xslVer)
		{
			return new Number(level, count, from, value, format, lang, letterValue, groupingSeparator, groupingSize, xslVer);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0003D78E File Offset: 0x0003C78E
		public static XslNode Otherwise()
		{
			return new XslNode(XslNodeType.Otherwise);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0003D797 File Offset: 0x0003C797
		public static XslNode PI(string name, XslVersion xslVer)
		{
			return new XslNode(XslNodeType.PI, null, name, xslVer);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0003D7A3 File Offset: 0x0003C7A3
		public static Sort Sort(string select, string lang, string dataType, string order, string caseOrder, XslVersion xslVer)
		{
			return new Sort(select, lang, dataType, order, caseOrder, xslVer);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0003D7B2 File Offset: 0x0003C7B2
		public static Template Template(QilName name, string match, QilName mode, double priority, XslVersion xslVer)
		{
			return new Template(name, match, mode, priority, xslVer);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0003D7BF File Offset: 0x0003C7BF
		public static XslNode Text(string data)
		{
			return new Text(data, SerializationHints.None, XslVersion.Version10);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0003D7C9 File Offset: 0x0003C7C9
		public static XslNode Text(string data, SerializationHints hints)
		{
			return new Text(data, hints, XslVersion.Version10);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0003D7D3 File Offset: 0x0003C7D3
		public static XslNode UseAttributeSet(QilName name)
		{
			return new XslNode(XslNodeType.UseAttributeSet, name, null, XslVersion.Version10);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0003D7DF File Offset: 0x0003C7DF
		public static VarPar VarPar(XslNodeType nt, QilName name, string select, XslVersion xslVer)
		{
			return new VarPar(nt, name, select, xslVer);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0003D7EA File Offset: 0x0003C7EA
		public static VarPar WithParam(QilName name)
		{
			return AstFactory.VarPar(XslNodeType.WithParam, name, null, XslVersion.Version10);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0003D7F6 File Offset: 0x0003C7F6
		public static QilName QName(string local, string uri, string prefix)
		{
			return AstFactory.f.LiteralQName(local, uri, prefix);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0003D805 File Offset: 0x0003C805
		public static QilName QName(string local)
		{
			return AstFactory.f.LiteralQName(local);
		}

		// Token: 0x0400087B RID: 2171
		private static QilFactory f = new QilFactory();
	}
}
