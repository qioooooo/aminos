using System;

namespace System.Web.Services
{
	// Token: 0x02000017 RID: 23
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
	public sealed class WebServiceBindingAttribute : Attribute
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002B68 File Offset: 0x00001B68
		public WebServiceBindingAttribute()
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002B70 File Offset: 0x00001B70
		public WebServiceBindingAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002B7F File Offset: 0x00001B7F
		public WebServiceBindingAttribute(string name, string ns)
		{
			this.name = name;
			this.ns = ns;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002B95 File Offset: 0x00001B95
		public WebServiceBindingAttribute(string name, string ns, string location)
		{
			this.name = name;
			this.ns = ns;
			this.location = location;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002BB2 File Offset: 0x00001BB2
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002BBA File Offset: 0x00001BBA
		public WsiProfiles ConformsTo
		{
			get
			{
				return this.claims;
			}
			set
			{
				this.claims = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002BC3 File Offset: 0x00001BC3
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002BCB File Offset: 0x00001BCB
		public bool EmitConformanceClaims
		{
			get
			{
				return this.emitClaims;
			}
			set
			{
				this.emitClaims = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002BD4 File Offset: 0x00001BD4
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002BEA File Offset: 0x00001BEA
		public string Location
		{
			get
			{
				if (this.location != null)
				{
					return this.location;
				}
				return string.Empty;
			}
			set
			{
				this.location = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002BF3 File Offset: 0x00001BF3
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002C09 File Offset: 0x00001C09
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002C12 File Offset: 0x00001C12
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002C28 File Offset: 0x00001C28
		public string Namespace
		{
			get
			{
				if (this.ns != null)
				{
					return this.ns;
				}
				return string.Empty;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x0400021D RID: 541
		private string name;

		// Token: 0x0400021E RID: 542
		private string ns;

		// Token: 0x0400021F RID: 543
		private string location;

		// Token: 0x04000220 RID: 544
		private WsiProfiles claims;

		// Token: 0x04000221 RID: 545
		private bool emitClaims;
	}
}
