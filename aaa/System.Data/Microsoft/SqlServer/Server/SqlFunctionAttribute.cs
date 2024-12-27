using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000284 RID: 644
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	[Serializable]
	public class SqlFunctionAttribute : Attribute
	{
		// Token: 0x060021B2 RID: 8626 RVA: 0x00269C30 File Offset: 0x00269030
		public SqlFunctionAttribute()
		{
			this.m_fDeterministic = false;
			this.m_eDataAccess = DataAccessKind.None;
			this.m_eSystemDataAccess = SystemDataAccessKind.None;
			this.m_fPrecise = false;
			this.m_fName = null;
			this.m_fTableDefinition = null;
			this.m_FillRowMethodName = null;
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060021B3 RID: 8627 RVA: 0x00269C74 File Offset: 0x00269074
		// (set) Token: 0x060021B4 RID: 8628 RVA: 0x00269C88 File Offset: 0x00269088
		public bool IsDeterministic
		{
			get
			{
				return this.m_fDeterministic;
			}
			set
			{
				this.m_fDeterministic = value;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x00269C9C File Offset: 0x0026909C
		// (set) Token: 0x060021B6 RID: 8630 RVA: 0x00269CB0 File Offset: 0x002690B0
		public DataAccessKind DataAccess
		{
			get
			{
				return this.m_eDataAccess;
			}
			set
			{
				this.m_eDataAccess = value;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060021B7 RID: 8631 RVA: 0x00269CC4 File Offset: 0x002690C4
		// (set) Token: 0x060021B8 RID: 8632 RVA: 0x00269CD8 File Offset: 0x002690D8
		public SystemDataAccessKind SystemDataAccess
		{
			get
			{
				return this.m_eSystemDataAccess;
			}
			set
			{
				this.m_eSystemDataAccess = value;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x00269CEC File Offset: 0x002690EC
		// (set) Token: 0x060021BA RID: 8634 RVA: 0x00269D00 File Offset: 0x00269100
		public bool IsPrecise
		{
			get
			{
				return this.m_fPrecise;
			}
			set
			{
				this.m_fPrecise = value;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x00269D14 File Offset: 0x00269114
		// (set) Token: 0x060021BC RID: 8636 RVA: 0x00269D28 File Offset: 0x00269128
		public string Name
		{
			get
			{
				return this.m_fName;
			}
			set
			{
				this.m_fName = value;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060021BD RID: 8637 RVA: 0x00269D3C File Offset: 0x0026913C
		// (set) Token: 0x060021BE RID: 8638 RVA: 0x00269D50 File Offset: 0x00269150
		public string TableDefinition
		{
			get
			{
				return this.m_fTableDefinition;
			}
			set
			{
				this.m_fTableDefinition = value;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x00269D64 File Offset: 0x00269164
		// (set) Token: 0x060021C0 RID: 8640 RVA: 0x00269D78 File Offset: 0x00269178
		public string FillRowMethodName
		{
			get
			{
				return this.m_FillRowMethodName;
			}
			set
			{
				this.m_FillRowMethodName = value;
			}
		}

		// Token: 0x04001617 RID: 5655
		private bool m_fDeterministic;

		// Token: 0x04001618 RID: 5656
		private DataAccessKind m_eDataAccess;

		// Token: 0x04001619 RID: 5657
		private SystemDataAccessKind m_eSystemDataAccess;

		// Token: 0x0400161A RID: 5658
		private bool m_fPrecise;

		// Token: 0x0400161B RID: 5659
		private string m_fName;

		// Token: 0x0400161C RID: 5660
		private string m_fTableDefinition;

		// Token: 0x0400161D RID: 5661
		private string m_FillRowMethodName;
	}
}
