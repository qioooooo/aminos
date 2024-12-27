using System;
using System.ComponentModel;
using System.Transactions;

namespace System.Data.Common
{
	// Token: 0x02000129 RID: 297
	public abstract class DbConnection : Component, IDbConnection, IDisposable
	{
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600136A RID: 4970
		// (set) Token: 0x0600136B RID: 4971
		[RecommendedAsConfigurable(true)]
		[DefaultValue("")]
		[ResCategory("DataCategory_Data")]
		[RefreshProperties(RefreshProperties.All)]
		public abstract string ConnectionString { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x00222670 File Offset: 0x00221A70
		[ResCategory("DataCategory_Data")]
		public virtual int ConnectionTimeout
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600136D RID: 4973
		[ResCategory("DataCategory_Data")]
		public abstract string Database { get; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x0600136E RID: 4974
		[ResCategory("DataCategory_Data")]
		public abstract string DataSource { get; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x00222680 File Offset: 0x00221A80
		protected virtual DbProviderFactory DbProviderFactory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x00222690 File Offset: 0x00221A90
		internal DbProviderFactory ProviderFactory
		{
			get
			{
				return this.DbProviderFactory;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06001371 RID: 4977
		[Browsable(false)]
		public abstract string ServerVersion { get; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001372 RID: 4978
		[ResDescription("DbConnection_State")]
		[Browsable(false)]
		public abstract ConnectionState State { get; }

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001373 RID: 4979 RVA: 0x002226A4 File Offset: 0x00221AA4
		// (remove) Token: 0x06001374 RID: 4980 RVA: 0x002226C8 File Offset: 0x00221AC8
		[ResCategory("DataCategory_StateChange")]
		[ResDescription("DbConnection_StateChange")]
		public virtual event StateChangeEventHandler StateChange
		{
			add
			{
				this._stateChangeEventHandler = (StateChangeEventHandler)Delegate.Combine(this._stateChangeEventHandler, value);
			}
			remove
			{
				this._stateChangeEventHandler = (StateChangeEventHandler)Delegate.Remove(this._stateChangeEventHandler, value);
			}
		}

		// Token: 0x06001375 RID: 4981
		protected abstract DbTransaction BeginDbTransaction(IsolationLevel isolationLevel);

		// Token: 0x06001376 RID: 4982 RVA: 0x002226EC File Offset: 0x00221AEC
		public DbTransaction BeginTransaction()
		{
			return this.BeginDbTransaction(IsolationLevel.Unspecified);
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00222700 File Offset: 0x00221B00
		public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return this.BeginDbTransaction(isolationLevel);
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00222714 File Offset: 0x00221B14
		IDbTransaction IDbConnection.BeginTransaction()
		{
			return this.BeginDbTransaction(IsolationLevel.Unspecified);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00222728 File Offset: 0x00221B28
		IDbTransaction IDbConnection.BeginTransaction(IsolationLevel isolationLevel)
		{
			return this.BeginDbTransaction(isolationLevel);
		}

		// Token: 0x0600137A RID: 4986
		public abstract void Close();

		// Token: 0x0600137B RID: 4987
		public abstract void ChangeDatabase(string databaseName);

		// Token: 0x0600137C RID: 4988 RVA: 0x0022273C File Offset: 0x00221B3C
		public DbCommand CreateCommand()
		{
			return this.CreateDbCommand();
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00222750 File Offset: 0x00221B50
		IDbCommand IDbConnection.CreateCommand()
		{
			return this.CreateDbCommand();
		}

		// Token: 0x0600137E RID: 4990
		protected abstract DbCommand CreateDbCommand();

		// Token: 0x0600137F RID: 4991 RVA: 0x00222764 File Offset: 0x00221B64
		public virtual void EnlistTransaction(Transaction transaction)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00222778 File Offset: 0x00221B78
		public virtual DataTable GetSchema()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0022278C File Offset: 0x00221B8C
		public virtual DataTable GetSchema(string collectionName)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x002227A0 File Offset: 0x00221BA0
		public virtual DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x002227B4 File Offset: 0x00221BB4
		protected virtual void OnStateChange(StateChangeEventArgs stateChange)
		{
			StateChangeEventHandler stateChangeEventHandler = this._stateChangeEventHandler;
			if (stateChangeEventHandler != null)
			{
				stateChangeEventHandler(this, stateChange);
			}
		}

		// Token: 0x06001384 RID: 4996
		public abstract void Open();

		// Token: 0x04000BF7 RID: 3063
		private StateChangeEventHandler _stateChangeEventHandler;
	}
}
