using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000A8 RID: 168
	public class ForestTrustRelationshipCollision
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x000207E4 File Offset: 0x0001F7E4
		internal ForestTrustRelationshipCollision(ForestTrustCollisionType collisionType, TopLevelNameCollisionOptions TLNFlag, DomainCollisionOptions domainFlag, string record)
		{
			this.type = collisionType;
			this.tlnFlag = TLNFlag;
			this.domainFlag = domainFlag;
			this.record = record;
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00020809 File Offset: 0x0001F809
		public ForestTrustCollisionType CollisionType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00020811 File Offset: 0x0001F811
		public TopLevelNameCollisionOptions TopLevelNameCollisionOption
		{
			get
			{
				return this.tlnFlag;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00020819 File Offset: 0x0001F819
		public DomainCollisionOptions DomainCollisionOption
		{
			get
			{
				return this.domainFlag;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00020821 File Offset: 0x0001F821
		public string CollisionRecord
		{
			get
			{
				return this.record;
			}
		}

		// Token: 0x04000460 RID: 1120
		private ForestTrustCollisionType type;

		// Token: 0x04000461 RID: 1121
		private TopLevelNameCollisionOptions tlnFlag;

		// Token: 0x04000462 RID: 1122
		private DomainCollisionOptions domainFlag;

		// Token: 0x04000463 RID: 1123
		private string record;
	}
}
