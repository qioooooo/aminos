using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000178 RID: 376
	internal class DocumentScope
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0004D535 File Offset: 0x0004C535
		internal NamespaceDecl Scopes
		{
			get
			{
				return this.scopes;
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0004D53D File Offset: 0x0004C53D
		internal NamespaceDecl AddNamespace(string prefix, string uri, string prevDefaultNsUri)
		{
			this.scopes = new NamespaceDecl(prefix, uri, prevDefaultNsUri, this.scopes);
			return this.scopes;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0004D55C File Offset: 0x0004C55C
		internal string ResolveAtom(string prefix)
		{
			for (NamespaceDecl next = this.scopes; next != null; next = next.Next)
			{
				if (Keywords.Equals(next.Prefix, prefix))
				{
					return next.Uri;
				}
			}
			return null;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0004D594 File Offset: 0x0004C594
		internal string ResolveNonAtom(string prefix)
		{
			for (NamespaceDecl next = this.scopes; next != null; next = next.Next)
			{
				if (Keywords.Compare(next.Prefix, prefix))
				{
					return next.Uri;
				}
			}
			return null;
		}

		// Token: 0x040009F4 RID: 2548
		protected NamespaceDecl scopes;
	}
}
