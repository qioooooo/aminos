using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200017F RID: 383
	internal class NamespaceDecl
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x0004F0F4 File Offset: 0x0004E0F4
		internal string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x0004F0FC File Offset: 0x0004E0FC
		internal string Uri
		{
			get
			{
				return this.nsUri;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0004F104 File Offset: 0x0004E104
		internal string PrevDefaultNsUri
		{
			get
			{
				return this.prevDefaultNsUri;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x0004F10C File Offset: 0x0004E10C
		internal NamespaceDecl Next
		{
			get
			{
				return this.next;
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0004F114 File Offset: 0x0004E114
		internal NamespaceDecl(string prefix, string nsUri, string prevDefaultNsUri, NamespaceDecl next)
		{
			this.Init(prefix, nsUri, prevDefaultNsUri, next);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0004F127 File Offset: 0x0004E127
		internal void Init(string prefix, string nsUri, string prevDefaultNsUri, NamespaceDecl next)
		{
			this.prefix = prefix;
			this.nsUri = nsUri;
			this.prevDefaultNsUri = prevDefaultNsUri;
			this.next = next;
		}

		// Token: 0x04000AD8 RID: 2776
		private string prefix;

		// Token: 0x04000AD9 RID: 2777
		private string nsUri;

		// Token: 0x04000ADA RID: 2778
		private string prevDefaultNsUri;

		// Token: 0x04000ADB RID: 2779
		private NamespaceDecl next;
	}
}
