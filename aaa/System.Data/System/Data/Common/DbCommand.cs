using System;
using System.ComponentModel;

namespace System.Data.Common
{
	// Token: 0x02000126 RID: 294
	public abstract class DbCommand : Component, IDbCommand, IDisposable
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060012F2 RID: 4850
		// (set) Token: 0x060012F3 RID: 4851
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbCommand_CommandText")]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.All)]
		public abstract string CommandText { get; set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060012F4 RID: 4852
		// (set) Token: 0x060012F5 RID: 4853
		[ResDescription("DbCommand_CommandTimeout")]
		[ResCategory("DataCategory_Data")]
		public abstract int CommandTimeout { get; set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060012F6 RID: 4854
		// (set) Token: 0x060012F7 RID: 4855
		[DefaultValue(CommandType.Text)]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbCommand_CommandType")]
		[ResCategory("DataCategory_Data")]
		public abstract CommandType CommandType { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x00220848 File Offset: 0x0021FC48
		// (set) Token: 0x060012F9 RID: 4857 RVA: 0x0022085C File Offset: 0x0021FC5C
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("DbCommand_Connection")]
		[ResCategory("DataCategory_Data")]
		public DbConnection Connection
		{
			get
			{
				return this.DbConnection;
			}
			set
			{
				this.DbConnection = value;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x00220870 File Offset: 0x0021FC70
		// (set) Token: 0x060012FB RID: 4859 RVA: 0x00220884 File Offset: 0x0021FC84
		IDbConnection IDbCommand.Connection
		{
			get
			{
				return this.DbConnection;
			}
			set
			{
				this.DbConnection = (DbConnection)value;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060012FC RID: 4860
		// (set) Token: 0x060012FD RID: 4861
		protected abstract DbConnection DbConnection { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060012FE RID: 4862
		protected abstract DbParameterCollection DbParameterCollection { get; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060012FF RID: 4863
		// (set) Token: 0x06001300 RID: 4864
		protected abstract DbTransaction DbTransaction { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001301 RID: 4865
		// (set) Token: 0x06001302 RID: 4866
		[DefaultValue(true)]
		[DesignOnly(true)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public abstract bool DesignTimeVisible { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x002208A0 File Offset: 0x0021FCA0
		[ResCategory("DataCategory_Data")]
		[Browsable(false)]
		[ResDescription("DbCommand_Parameters")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DbParameterCollection Parameters
		{
			get
			{
				return this.DbParameterCollection;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x002208B4 File Offset: 0x0021FCB4
		IDataParameterCollection IDbCommand.Parameters
		{
			get
			{
				return this.DbParameterCollection;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001305 RID: 4869 RVA: 0x002208C8 File Offset: 0x0021FCC8
		// (set) Token: 0x06001306 RID: 4870 RVA: 0x002208DC File Offset: 0x0021FCDC
		[DefaultValue(null)]
		[ResDescription("DbCommand_Transaction")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DbTransaction Transaction
		{
			get
			{
				return this.DbTransaction;
			}
			set
			{
				this.DbTransaction = value;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x002208F0 File Offset: 0x0021FCF0
		// (set) Token: 0x06001308 RID: 4872 RVA: 0x00220904 File Offset: 0x0021FD04
		IDbTransaction IDbCommand.Transaction
		{
			get
			{
				return this.DbTransaction;
			}
			set
			{
				this.DbTransaction = (DbTransaction)value;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001309 RID: 4873
		// (set) Token: 0x0600130A RID: 4874
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbCommand_UpdatedRowSource")]
		[DefaultValue(UpdateRowSource.Both)]
		public abstract UpdateRowSource UpdatedRowSource { get; set; }

		// Token: 0x0600130B RID: 4875
		public abstract void Cancel();

		// Token: 0x0600130C RID: 4876 RVA: 0x00220920 File Offset: 0x0021FD20
		public DbParameter CreateParameter()
		{
			return this.CreateDbParameter();
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00220934 File Offset: 0x0021FD34
		IDbDataParameter IDbCommand.CreateParameter()
		{
			return this.CreateDbParameter();
		}

		// Token: 0x0600130E RID: 4878
		protected abstract DbParameter CreateDbParameter();

		// Token: 0x0600130F RID: 4879
		protected abstract DbDataReader ExecuteDbDataReader(CommandBehavior behavior);

		// Token: 0x06001310 RID: 4880
		public abstract int ExecuteNonQuery();

		// Token: 0x06001311 RID: 4881 RVA: 0x00220948 File Offset: 0x0021FD48
		public DbDataReader ExecuteReader()
		{
			return this.ExecuteDbDataReader(CommandBehavior.Default);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0022095C File Offset: 0x0021FD5C
		IDataReader IDbCommand.ExecuteReader()
		{
			return this.ExecuteDbDataReader(CommandBehavior.Default);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00220970 File Offset: 0x0021FD70
		public DbDataReader ExecuteReader(CommandBehavior behavior)
		{
			return this.ExecuteDbDataReader(behavior);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00220984 File Offset: 0x0021FD84
		IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
		{
			return this.ExecuteDbDataReader(behavior);
		}

		// Token: 0x06001315 RID: 4885
		public abstract object ExecuteScalar();

		// Token: 0x06001316 RID: 4886
		public abstract void Prepare();
	}
}
