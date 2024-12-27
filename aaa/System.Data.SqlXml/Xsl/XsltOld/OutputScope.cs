using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000185 RID: 389
	internal class OutputScope : DocumentScope
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0004F688 File Offset: 0x0004E688
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0004F690 File Offset: 0x0004E690
		internal string Namespace
		{
			get
			{
				return this.nsUri;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0004F698 File Offset: 0x0004E698
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x0004F6A0 File Offset: 0x0004E6A0
		internal string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0004F6A9 File Offset: 0x0004E6A9
		// (set) Token: 0x0600104A RID: 4170 RVA: 0x0004F6B1 File Offset: 0x0004E6B1
		internal XmlSpace Space
		{
			get
			{
				return this.space;
			}
			set
			{
				this.space = value;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0004F6BA File Offset: 0x0004E6BA
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x0004F6C2 File Offset: 0x0004E6C2
		internal string Lang
		{
			get
			{
				return this.lang;
			}
			set
			{
				this.lang = value;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0004F6CB File Offset: 0x0004E6CB
		// (set) Token: 0x0600104E RID: 4174 RVA: 0x0004F6D3 File Offset: 0x0004E6D3
		internal bool Mixed
		{
			get
			{
				return this.mixed;
			}
			set
			{
				this.mixed = value;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x0004F6DC File Offset: 0x0004E6DC
		// (set) Token: 0x06001050 RID: 4176 RVA: 0x0004F6E4 File Offset: 0x0004E6E4
		internal bool ToCData
		{
			get
			{
				return this.toCData;
			}
			set
			{
				this.toCData = value;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0004F6ED File Offset: 0x0004E6ED
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x0004F6F5 File Offset: 0x0004E6F5
		internal HtmlElementProps HtmlElementProps
		{
			get
			{
				return this.htmlElementProps;
			}
			set
			{
				this.htmlElementProps = value;
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0004F6FE File Offset: 0x0004E6FE
		internal OutputScope()
		{
			this.Init(string.Empty, string.Empty, string.Empty, XmlSpace.None, string.Empty, false);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0004F724 File Offset: 0x0004E724
		internal void Init(string name, string nspace, string prefix, XmlSpace space, string lang, bool mixed)
		{
			this.scopes = null;
			this.name = name;
			this.nsUri = nspace;
			this.prefix = prefix;
			this.space = space;
			this.lang = lang;
			this.mixed = mixed;
			this.toCData = false;
			this.htmlElementProps = null;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0004F774 File Offset: 0x0004E774
		internal bool FindPrefix(string urn, out string prefix)
		{
			for (NamespaceDecl namespaceDecl = this.scopes; namespaceDecl != null; namespaceDecl = namespaceDecl.Next)
			{
				if (Keywords.Equals(namespaceDecl.Uri, urn) && namespaceDecl.Prefix != null && namespaceDecl.Prefix.Length > 0)
				{
					prefix = namespaceDecl.Prefix;
					return true;
				}
			}
			prefix = string.Empty;
			return false;
		}

		// Token: 0x04000AEE RID: 2798
		private string name;

		// Token: 0x04000AEF RID: 2799
		private string nsUri;

		// Token: 0x04000AF0 RID: 2800
		private string prefix;

		// Token: 0x04000AF1 RID: 2801
		private XmlSpace space;

		// Token: 0x04000AF2 RID: 2802
		private string lang;

		// Token: 0x04000AF3 RID: 2803
		private bool mixed;

		// Token: 0x04000AF4 RID: 2804
		private bool toCData;

		// Token: 0x04000AF5 RID: 2805
		private HtmlElementProps htmlElementProps;
	}
}
