using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000AD RID: 173
	public class TrustRelationshipInformation
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x000209FA File Offset: 0x0001F9FA
		internal TrustRelationshipInformation()
		{
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00020A04 File Offset: 0x0001FA04
		internal TrustRelationshipInformation(DirectoryContext context, string source, TrustObject obj)
		{
			this.context = context;
			this.source = source;
			this.target = ((obj.DnsDomainName == null) ? obj.NetbiosDomainName : obj.DnsDomainName);
			if ((obj.Flags & 2) != 0 && (obj.Flags & 32) != 0)
			{
				this.direction = TrustDirection.Bidirectional;
			}
			else if ((obj.Flags & 2) != 0)
			{
				this.direction = TrustDirection.Outbound;
			}
			else if ((obj.Flags & 32) != 0)
			{
				this.direction = TrustDirection.Inbound;
			}
			this.type = obj.TrustType;
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00020A90 File Offset: 0x0001FA90
		public string SourceName
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00020A98 File Offset: 0x0001FA98
		public string TargetName
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00020AA0 File Offset: 0x0001FAA0
		public TrustType TrustType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00020AA8 File Offset: 0x0001FAA8
		public TrustDirection TrustDirection
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x0400046F RID: 1135
		internal string source;

		// Token: 0x04000470 RID: 1136
		internal string target;

		// Token: 0x04000471 RID: 1137
		internal TrustType type;

		// Token: 0x04000472 RID: 1138
		internal TrustDirection direction;

		// Token: 0x04000473 RID: 1139
		internal DirectoryContext context;
	}
}
