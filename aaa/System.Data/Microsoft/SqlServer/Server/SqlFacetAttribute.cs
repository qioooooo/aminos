using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000281 RID: 641
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = false)]
	public class SqlFacetAttribute : Attribute
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x00269B54 File Offset: 0x00268F54
		// (set) Token: 0x060021A8 RID: 8616 RVA: 0x00269B68 File Offset: 0x00268F68
		public bool IsFixedLength
		{
			get
			{
				return this.m_IsFixedLength;
			}
			set
			{
				this.m_IsFixedLength = value;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x00269B7C File Offset: 0x00268F7C
		// (set) Token: 0x060021AA RID: 8618 RVA: 0x00269B90 File Offset: 0x00268F90
		public int MaxSize
		{
			get
			{
				return this.m_MaxSize;
			}
			set
			{
				this.m_MaxSize = value;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060021AB RID: 8619 RVA: 0x00269BA4 File Offset: 0x00268FA4
		// (set) Token: 0x060021AC RID: 8620 RVA: 0x00269BB8 File Offset: 0x00268FB8
		public int Precision
		{
			get
			{
				return this.m_Precision;
			}
			set
			{
				this.m_Precision = value;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x00269BCC File Offset: 0x00268FCC
		// (set) Token: 0x060021AE RID: 8622 RVA: 0x00269BE0 File Offset: 0x00268FE0
		public int Scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = value;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x00269BF4 File Offset: 0x00268FF4
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x00269C08 File Offset: 0x00269008
		public bool IsNullable
		{
			get
			{
				return this.m_IsNullable;
			}
			set
			{
				this.m_IsNullable = value;
			}
		}

		// Token: 0x0400160C RID: 5644
		private bool m_IsFixedLength;

		// Token: 0x0400160D RID: 5645
		private int m_MaxSize;

		// Token: 0x0400160E RID: 5646
		private int m_Scale;

		// Token: 0x0400160F RID: 5647
		private int m_Precision;

		// Token: 0x04001610 RID: 5648
		private bool m_IsNullable;
	}
}
