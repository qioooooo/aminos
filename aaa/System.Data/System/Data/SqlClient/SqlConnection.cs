using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002CB RID: 715
	[DefaultEvent("InfoMessage")]
	public sealed class SqlConnection : DbConnection, ICloneable
	{
		// Token: 0x0600248A RID: 9354 RVA: 0x00278114 File Offset: 0x00277514
		public SqlConnection(string connectionString)
			: this()
		{
			this.ConnectionString = connectionString;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x00278130 File Offset: 0x00277530
		private SqlConnection(SqlConnection connection)
		{
			this.ObjectID = Interlocked.Increment(ref SqlConnection._objectTypeCount);
			base..ctor();
			GC.SuppressFinalize(this);
			this.CopyFrom(connection);
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600248C RID: 9356 RVA: 0x00278160 File Offset: 0x00277560
		// (set) Token: 0x0600248D RID: 9357 RVA: 0x00278174 File Offset: 0x00277574
		[ResCategory("DataCategory_Data")]
		[ResDescription("SqlConnection_StatisticsEnabled")]
		[DefaultValue(false)]
		public bool StatisticsEnabled
		{
			get
			{
				return this._collectstats;
			}
			set
			{
				if (this.IsContextConnection)
				{
					if (value)
					{
						throw SQL.NotAvailableOnContextConnection();
					}
				}
				else
				{
					if (value)
					{
						if (ConnectionState.Open == this.State)
						{
							if (this._statistics == null)
							{
								this._statistics = new SqlStatistics();
								ADP.TimerCurrent(out this._statistics._openTimestamp);
							}
							this.Parser.Statistics = this._statistics;
						}
					}
					else if (this._statistics != null && ConnectionState.Open == this.State)
					{
						TdsParser parser = this.Parser;
						parser.Statistics = null;
						ADP.TimerCurrent(out this._statistics._closeTimestamp);
					}
					this._collectstats = value;
				}
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x00278210 File Offset: 0x00277610
		// (set) Token: 0x0600248F RID: 9359 RVA: 0x00278224 File Offset: 0x00277624
		internal bool AsycCommandInProgress
		{
			get
			{
				return this._AsycCommandInProgress;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._AsycCommandInProgress = value;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x00278238 File Offset: 0x00277638
		internal bool IsContextConnection
		{
			get
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)this.ConnectionOptions;
				bool flag = false;
				if (sqlConnectionString != null)
				{
					flag = sqlConnectionString.ContextConnection;
				}
				return flag;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x00278260 File Offset: 0x00277660
		internal SqlConnectionString.TransactionBindingEnum TransactionBinding
		{
			get
			{
				return ((SqlConnectionString)this.ConnectionOptions).TransactionBinding;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x00278280 File Offset: 0x00277680
		internal SqlConnectionString.TypeSystem TypeSystem
		{
			get
			{
				return ((SqlConnectionString)this.ConnectionOptions).TypeSystemVersion;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x002782A0 File Offset: 0x002776A0
		protected override DbProviderFactory DbProviderFactory
		{
			get
			{
				return SqlClientFactory.Instance;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x002782B4 File Offset: 0x002776B4
		// (set) Token: 0x06002495 RID: 9365 RVA: 0x002782C8 File Offset: 0x002776C8
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("SqlConnection_ConnectionString")]
		[RecommendedAsConfigurable(true)]
		[DefaultValue("")]
		[ResCategory("DataCategory_Data")]
		[Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlConnectionStringEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public override string ConnectionString
		{
			get
			{
				return this.ConnectionString_Get();
			}
			set
			{
				this.ConnectionString_Set(value);
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x002782DC File Offset: 0x002776DC
		[ResDescription("SqlConnection_ConnectionTimeout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int ConnectionTimeout
		{
			get
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)this.ConnectionOptions;
				if (sqlConnectionString == null)
				{
					return 15;
				}
				return sqlConnectionString.ConnectTimeout;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06002497 RID: 9367 RVA: 0x00278304 File Offset: 0x00277704
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("SqlConnection_Database")]
		public override string Database
		{
			get
			{
				SqlInternalConnection sqlInternalConnection = this.InnerConnection as SqlInternalConnection;
				string text;
				if (sqlInternalConnection != null)
				{
					text = sqlInternalConnection.CurrentDatabase;
				}
				else
				{
					SqlConnectionString sqlConnectionString = (SqlConnectionString)this.ConnectionOptions;
					text = ((sqlConnectionString != null) ? sqlConnectionString.InitialCatalog : "");
				}
				return text;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002498 RID: 9368 RVA: 0x00278348 File Offset: 0x00277748
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(true)]
		[ResDescription("SqlConnection_DataSource")]
		public override string DataSource
		{
			get
			{
				SqlInternalConnection sqlInternalConnection = this.InnerConnection as SqlInternalConnection;
				string text;
				if (sqlInternalConnection != null)
				{
					text = sqlInternalConnection.CurrentDataSource;
				}
				else
				{
					SqlConnectionString sqlConnectionString = (SqlConnectionString)this.ConnectionOptions;
					text = ((sqlConnectionString != null) ? sqlConnectionString.DataSource : "");
				}
				return text;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x0027838C File Offset: 0x0027778C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("SqlConnection_PacketSize")]
		public int PacketSize
		{
			get
			{
				if (this.IsContextConnection)
				{
					throw SQL.NotAvailableOnContextConnection();
				}
				SqlInternalConnectionTds sqlInternalConnectionTds = this.InnerConnection as SqlInternalConnectionTds;
				int num;
				if (sqlInternalConnectionTds != null)
				{
					num = sqlInternalConnectionTds.PacketSize;
				}
				else
				{
					SqlConnectionString sqlConnectionString = (SqlConnectionString)this.ConnectionOptions;
					num = ((sqlConnectionString != null) ? sqlConnectionString.PacketSize : 8000);
				}
				return num;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x002783E0 File Offset: 0x002777E0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[ResDescription("SqlConnection_ServerVersion")]
		public override string ServerVersion
		{
			get
			{
				return this.GetOpenConnection().ServerVersion;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x002783F8 File Offset: 0x002777F8
		internal SqlStatistics Statistics
		{
			get
			{
				return this._statistics;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x0027840C File Offset: 0x0027780C
		[ResDescription("SqlConnection_WorkstationId")]
		[ResCategory("DataCategory_Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string WorkstationId
		{
			get
			{
				if (this.IsContextConnection)
				{
					throw SQL.NotAvailableOnContextConnection();
				}
				SqlConnectionString sqlConnectionString = (SqlConnectionString)this.ConnectionOptions;
				string text = ((sqlConnectionString != null) ? sqlConnectionString.WorkstationId : null);
				if (text == null)
				{
					text = Environment.MachineName;
				}
				return text;
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x0600249D RID: 9373 RVA: 0x0027844C File Offset: 0x0027784C
		// (remove) Token: 0x0600249E RID: 9374 RVA: 0x0027846C File Offset: 0x0027786C
		[ResDescription("DbConnection_InfoMessage")]
		[ResCategory("DataCategory_InfoMessage")]
		public event SqlInfoMessageEventHandler InfoMessage
		{
			add
			{
				base.Events.AddHandler(SqlConnection.EventInfoMessage, value);
			}
			remove
			{
				base.Events.RemoveHandler(SqlConnection.EventInfoMessage, value);
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x0027848C File Offset: 0x0027788C
		// (set) Token: 0x060024A0 RID: 9376 RVA: 0x002784A0 File Offset: 0x002778A0
		public bool FireInfoMessageEventOnUserErrors
		{
			get
			{
				return this._fireInfoMessageEventOnUserErrors;
			}
			set
			{
				this._fireInfoMessageEventOnUserErrors = value;
			}
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x002784B4 File Offset: 0x002778B4
		public new SqlTransaction BeginTransaction()
		{
			return this.BeginTransaction(IsolationLevel.Unspecified, null);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x002784CC File Offset: 0x002778CC
		public new SqlTransaction BeginTransaction(IsolationLevel iso)
		{
			return this.BeginTransaction(iso, null);
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x002784E4 File Offset: 0x002778E4
		public SqlTransaction BeginTransaction(string transactionName)
		{
			return this.BeginTransaction(IsolationLevel.Unspecified, transactionName);
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x002784FC File Offset: 0x002778FC
		public SqlTransaction BeginTransaction(IsolationLevel iso, string transactionName)
		{
			SqlStatistics sqlStatistics = null;
			string text = (ADP.IsEmpty(transactionName) ? "None" : transactionName);
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlConnection.BeginTransaction|API> %d#, iso=%d{ds.IsolationLevel}, transactionName='%ls'\n", this.ObjectID, (int)iso, text);
			SqlTransaction sqlTransaction2;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				SqlTransaction sqlTransaction = this.GetOpenConnection().BeginSqlTransaction(iso, transactionName);
				GC.KeepAlive(this);
				sqlTransaction2 = sqlTransaction;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return sqlTransaction2;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x00278580 File Offset: 0x00277980
		public override void ChangeDatabase(string database)
		{
			SqlStatistics sqlStatistics = null;
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this);
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.InnerConnection.ChangeDatabase(database);
			}
			catch (OutOfMemoryException ex)
			{
				this.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x00278650 File Offset: 0x00277A50
		public static void ClearAllPools()
		{
			new SqlClientPermission(PermissionState.Unrestricted).Demand();
			SqlConnectionFactory.SingletonInstance.ClearAllPools();
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x00278674 File Offset: 0x00277A74
		public static void ClearPool(SqlConnection connection)
		{
			ADP.CheckArgumentNull(connection, "connection");
			DbConnectionOptions userConnectionOptions = connection.UserConnectionOptions;
			if (userConnectionOptions != null)
			{
				userConnectionOptions.DemandPermission();
				if (connection.IsContextConnection)
				{
					throw SQL.NotAvailableOnContextConnection();
				}
				SqlConnectionFactory.SingletonInstance.ClearPool(connection);
			}
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x002786B8 File Offset: 0x00277AB8
		object ICloneable.Clone()
		{
			SqlConnection sqlConnection = new SqlConnection(this);
			Bid.Trace("<sc.SqlConnection.Clone|API> %d#, clone=%d#\n", this.ObjectID, sqlConnection.ObjectID);
			return sqlConnection;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x002786E4 File Offset: 0x00277AE4
		public override void Close()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlConnection.Close|API> %d#", this.ObjectID);
			try
			{
				SqlStatistics sqlStatistics = null;
				SNIHandle snihandle = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this);
					sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
					lock (this.InnerConnection)
					{
						this.InnerConnection.CloseConnection(this, this.ConnectionFactory);
					}
					if (this.Statistics != null)
					{
						ADP.TimerCurrent(out this._statistics._closeTimestamp);
					}
				}
				catch (OutOfMemoryException ex)
				{
					this.Abort(ex);
					throw;
				}
				catch (StackOverflowException ex2)
				{
					this.Abort(ex2);
					throw;
				}
				catch (ThreadAbortException ex3)
				{
					this.Abort(ex3);
					SqlInternalConnection.BestEffortCleanup(snihandle);
					throw;
				}
				finally
				{
					SqlStatistics.StopTimer(sqlStatistics);
				}
			}
			finally
			{
				SqlDebugContext sdc = this._sdc;
				this._sdc = null;
				Bid.ScopeLeave(ref intPtr);
				if (sdc != null)
				{
					sdc.Dispose();
				}
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x00278850 File Offset: 0x00277C50
		public new SqlCommand CreateCommand()
		{
			return new SqlCommand(null, this);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x00278864 File Offset: 0x00277C64
		private void DisposeMe(bool disposing)
		{
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x00278874 File Offset: 0x00277C74
		public void EnlistDistributedTransaction(ITransaction transaction)
		{
			if (this.IsContextConnection)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			this.EnlistDistributedTransactionHelper(transaction);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x00278898 File Offset: 0x00277C98
		public override void Open()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlConnection.Open|API> %d#", this.ObjectID);
			try
			{
				if (this.StatisticsEnabled)
				{
					if (this._statistics == null)
					{
						this._statistics = new SqlStatistics();
					}
					else
					{
						this._statistics.ContinueOnNewConnection();
					}
				}
				SNIHandle snihandle = null;
				SqlStatistics sqlStatistics = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
					this.InnerConnection.OpenConnection(this, this.ConnectionFactory);
					snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this);
					SqlInternalConnectionSmi sqlInternalConnectionSmi = this.InnerConnection as SqlInternalConnectionSmi;
					if (sqlInternalConnectionSmi != null)
					{
						sqlInternalConnectionSmi.AutomaticEnlistment();
					}
					else
					{
						if (this.StatisticsEnabled)
						{
							ADP.TimerCurrent(out this._statistics._openTimestamp);
							this.Parser.Statistics = this._statistics;
						}
						else
						{
							this.Parser.Statistics = null;
							this._statistics = null;
						}
						this.CompleteOpen();
					}
				}
				catch (OutOfMemoryException ex)
				{
					this.Abort(ex);
					throw;
				}
				catch (StackOverflowException ex2)
				{
					this.Abort(ex2);
					throw;
				}
				catch (ThreadAbortException ex3)
				{
					this.Abort(ex3);
					SqlInternalConnection.BestEffortCleanup(snihandle);
					throw;
				}
				finally
				{
					SqlStatistics.StopTimer(sqlStatistics);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x00278A28 File Offset: 0x00277E28
		internal bool HasLocalTransaction
		{
			get
			{
				return this.GetOpenConnection().HasLocalTransaction;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x00278A40 File Offset: 0x00277E40
		internal bool HasLocalTransactionFromAPI
		{
			get
			{
				return this.GetOpenConnection().HasLocalTransactionFromAPI;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060024B0 RID: 9392 RVA: 0x00278A58 File Offset: 0x00277E58
		internal bool IsShiloh
		{
			get
			{
				return this.GetOpenConnection().IsShiloh;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060024B1 RID: 9393 RVA: 0x00278A70 File Offset: 0x00277E70
		internal bool IsYukonOrNewer
		{
			get
			{
				return this.GetOpenConnection().IsYukonOrNewer;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x00278A88 File Offset: 0x00277E88
		internal bool IsKatmaiOrNewer
		{
			get
			{
				return this.GetOpenConnection().IsKatmaiOrNewer;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060024B3 RID: 9395 RVA: 0x00278AA0 File Offset: 0x00277EA0
		internal TdsParser Parser
		{
			get
			{
				SqlInternalConnectionTds sqlInternalConnectionTds = this.GetOpenConnection() as SqlInternalConnectionTds;
				if (sqlInternalConnectionTds == null)
				{
					throw SQL.NotAvailableOnContextConnection();
				}
				return sqlInternalConnectionTds.Parser;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x00278AC8 File Offset: 0x00277EC8
		internal bool Asynchronous
		{
			get
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)this.ConnectionOptions;
				return sqlConnectionString != null && sqlConnectionString.Asynchronous;
			}
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00278AEC File Offset: 0x00277EEC
		internal void AddPreparedCommand(SqlCommand cmd)
		{
			this.GetOpenConnection().AddPreparedCommand(cmd);
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00278B08 File Offset: 0x00277F08
		internal void ValidateConnectionForExecute(string method, SqlCommand command)
		{
			SqlInternalConnection openConnection = this.GetOpenConnection(method);
			openConnection.ValidateConnectionForExecute(command);
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x00278B24 File Offset: 0x00277F24
		internal static string FixupDatabaseTransactionName(string name)
		{
			if (!ADP.IsEmpty(name))
			{
				return "[" + name.Replace("]", "]]") + "]";
			}
			return name;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00278B5C File Offset: 0x00277F5C
		internal void OnError(SqlException exception, bool breakConnection)
		{
			if (breakConnection && ConnectionState.Open == this.State)
			{
				Bid.Trace("<sc.SqlConnection.OnError|INFO> %d#, Connection broken.\n", this.ObjectID);
				this.Close();
			}
			if (exception.Class >= 11)
			{
				throw exception;
			}
			this.OnInfoMessage(new SqlInfoMessageEventArgs(exception));
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00278BA4 File Offset: 0x00277FA4
		internal void RemovePreparedCommand(SqlCommand cmd)
		{
			this.GetOpenConnection().RemovePreparedCommand(cmd);
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x00278BC0 File Offset: 0x00277FC0
		private void CompleteOpen()
		{
			if (!this.GetOpenConnection().IsYukonOrNewer && Debugger.IsAttached)
			{
				bool flag = false;
				try
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
					flag = true;
				}
				catch (SecurityException ex)
				{
					ADP.TraceExceptionWithoutRethrow(ex);
				}
				if (flag)
				{
					this.CheckSQLDebugOnConnect();
				}
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00278C20 File Offset: 0x00278020
		internal SqlInternalConnection GetOpenConnection()
		{
			SqlInternalConnection sqlInternalConnection = this.InnerConnection as SqlInternalConnection;
			if (sqlInternalConnection == null)
			{
				throw ADP.ClosedConnectionError();
			}
			return sqlInternalConnection;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00278C44 File Offset: 0x00278044
		internal SqlInternalConnection GetOpenConnection(string method)
		{
			DbConnectionInternal innerConnection = this.InnerConnection;
			SqlInternalConnection sqlInternalConnection = innerConnection as SqlInternalConnection;
			if (sqlInternalConnection == null)
			{
				throw ADP.OpenConnectionRequired(method, innerConnection.State);
			}
			return sqlInternalConnection;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00278C70 File Offset: 0x00278070
		internal SqlInternalConnectionTds GetOpenTdsConnection()
		{
			SqlInternalConnectionTds sqlInternalConnectionTds = this.InnerConnection as SqlInternalConnectionTds;
			if (sqlInternalConnectionTds == null)
			{
				throw ADP.ClosedConnectionError();
			}
			return sqlInternalConnectionTds;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00278C94 File Offset: 0x00278094
		internal SqlInternalConnectionTds GetOpenTdsConnection(string method)
		{
			SqlInternalConnectionTds sqlInternalConnectionTds = this.InnerConnection as SqlInternalConnectionTds;
			if (sqlInternalConnectionTds == null)
			{
				throw ADP.OpenConnectionRequired(method, sqlInternalConnectionTds.State);
			}
			return sqlInternalConnectionTds;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x00278CC0 File Offset: 0x002780C0
		internal void OnInfoMessage(SqlInfoMessageEventArgs imevent)
		{
			if (Bid.TraceOn)
			{
				Bid.Trace("<sc.SqlConnection.OnInfoMessage|API|INFO> %d#, Message='%ls'\n", this.ObjectID, (imevent != null) ? imevent.Message : "");
			}
			SqlInfoMessageEventHandler sqlInfoMessageEventHandler = (SqlInfoMessageEventHandler)base.Events[SqlConnection.EventInfoMessage];
			if (sqlInfoMessageEventHandler != null)
			{
				try
				{
					sqlInfoMessageEventHandler(this, imevent);
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex);
				}
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x00278D48 File Offset: 0x00278148
		private void CheckSQLDebugOnConnect()
		{
			uint currentProcessId = (uint)SafeNativeMethods.GetCurrentProcessId();
			string text;
			if (ADP.IsPlatformNT5)
			{
				text = "Global\\SqlClientSSDebug";
			}
			else
			{
				text = "SqlClientSSDebug";
			}
			text += currentProcessId.ToString(CultureInfo.InvariantCulture);
			IntPtr intPtr = NativeMethods.OpenFileMappingA(4, false, text);
			if (ADP.PtrZero != intPtr)
			{
				IntPtr intPtr2 = NativeMethods.MapViewOfFile(intPtr, 4, 0, 0, IntPtr.Zero);
				if (ADP.PtrZero != intPtr2)
				{
					SqlDebugContext sqlDebugContext = new SqlDebugContext();
					sqlDebugContext.hMemMap = intPtr;
					sqlDebugContext.pMemMap = intPtr2;
					sqlDebugContext.pid = currentProcessId;
					this.CheckSQLDebug(sqlDebugContext);
					this._sdc = sqlDebugContext;
				}
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x00278DE0 File Offset: 0x002781E0
		internal void CheckSQLDebug()
		{
			if (this._sdc != null)
			{
				this.CheckSQLDebug(this._sdc);
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x00278E04 File Offset: 0x00278204
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private void CheckSQLDebug(SqlDebugContext sdc)
		{
			uint currentThreadId = (uint)AppDomain.GetCurrentThreadId();
			SqlConnection.RefreshMemoryMappedData(sdc);
			if (!sdc.active && sdc.fOption)
			{
				sdc.active = true;
				sdc.tid = currentThreadId;
				try
				{
					this.IssueSQLDebug(1U, sdc.machineName, sdc.pid, sdc.dbgpid, sdc.sdiDllName, sdc.data);
					sdc.tid = 0U;
				}
				catch
				{
					sdc.active = false;
					throw;
				}
			}
			if (sdc.active)
			{
				if (!sdc.fOption)
				{
					sdc.Dispose();
					this.IssueSQLDebug(0U, null, 0U, 0U, null, null);
					return;
				}
				if (sdc.tid != currentThreadId)
				{
					sdc.tid = currentThreadId;
					try
					{
						this.IssueSQLDebug(2U, null, sdc.pid, sdc.tid, null, null);
					}
					catch
					{
						sdc.tid = 0U;
						throw;
					}
				}
			}
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x00278F00 File Offset: 0x00278300
		private void IssueSQLDebug(uint option, string machineName, uint pid, uint id, string sdiDllName, byte[] data)
		{
			if (this.GetOpenConnection().IsYukonOrNewer)
			{
				return;
			}
			SqlCommand sqlCommand = new SqlCommand("sp_sdidebug", this);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.VarChar, TdsEnums.SQLDEBUG_MODE_NAMES[(int)((UIntPtr)option)].Length);
			sqlParameter.Value = TdsEnums.SQLDEBUG_MODE_NAMES[(int)((UIntPtr)option)];
			sqlCommand.Parameters.Add(sqlParameter);
			if (option == 1U)
			{
				sqlParameter = new SqlParameter(null, SqlDbType.VarChar, sdiDllName.Length);
				sqlParameter.Value = sdiDllName;
				sqlCommand.Parameters.Add(sqlParameter);
				sqlParameter = new SqlParameter(null, SqlDbType.VarChar, machineName.Length);
				sqlParameter.Value = machineName;
				sqlCommand.Parameters.Add(sqlParameter);
			}
			if (option != 0U)
			{
				sqlParameter = new SqlParameter(null, SqlDbType.Int);
				sqlParameter.Value = pid;
				sqlCommand.Parameters.Add(sqlParameter);
				sqlParameter = new SqlParameter(null, SqlDbType.Int);
				sqlParameter.Value = id;
				sqlCommand.Parameters.Add(sqlParameter);
			}
			if (option == 1U)
			{
				sqlParameter = new SqlParameter(null, SqlDbType.VarBinary, (data != null) ? data.Length : 0);
				sqlParameter.Value = data;
				sqlCommand.Parameters.Add(sqlParameter);
			}
			sqlCommand.ExecuteNonQuery();
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x00279028 File Offset: 0x00278428
		public static void ChangePassword(string connectionString, string newPassword)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlConnection.ChangePassword|API>");
			try
			{
				if (ADP.IsEmpty(connectionString))
				{
					throw SQL.ChangePasswordArgumentMissing("connectionString");
				}
				if (ADP.IsEmpty(newPassword))
				{
					throw SQL.ChangePasswordArgumentMissing("newPassword");
				}
				if (128 < newPassword.Length)
				{
					throw ADP.InvalidArgumentLength("newPassword", 128);
				}
				SqlConnectionString sqlConnectionString = SqlConnectionFactory.FindSqlConnectionOptions(connectionString);
				if (sqlConnectionString.IntegratedSecurity)
				{
					throw SQL.ChangePasswordConflictsWithSSPI();
				}
				if (!ADP.IsEmpty(sqlConnectionString.AttachDBFilename))
				{
					throw SQL.ChangePasswordUseOfUnallowedKey("attachdbfilename");
				}
				if (sqlConnectionString.ContextConnection)
				{
					throw SQL.ChangePasswordUseOfUnallowedKey("context connection");
				}
				PermissionSet permissionSet = sqlConnectionString.CreatePermissionSet();
				permissionSet.Demand();
				using (SqlInternalConnectionTds sqlInternalConnectionTds = new SqlInternalConnectionTds(null, sqlConnectionString, null, newPassword, null, false))
				{
					if (!sqlInternalConnectionTds.IsYukonOrNewer)
					{
						throw SQL.ChangePasswordRequiresYukon();
					}
				}
				SqlConnectionFactory.SingletonInstance.ClearPool(connectionString);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x00279140 File Offset: 0x00278540
		private static void RefreshMemoryMappedData(SqlDebugContext sdc)
		{
			MEMMAP memmap = (MEMMAP)Marshal.PtrToStructure(sdc.pMemMap, typeof(MEMMAP));
			sdc.dbgpid = memmap.dbgpid;
			sdc.fOption = memmap.fOption == 1U;
			Encoding encoding = Encoding.GetEncoding(1252);
			sdc.machineName = encoding.GetString(memmap.rgbMachineName, 0, memmap.rgbMachineName.Length);
			sdc.sdiDllName = encoding.GetString(memmap.rgbDllName, 0, memmap.rgbDllName.Length);
			sdc.data = memmap.rgbData;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x002791DC File Offset: 0x002785DC
		public void ResetStatistics()
		{
			if (this.IsContextConnection)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			if (this.Statistics != null)
			{
				this.Statistics.Reset();
				if (ConnectionState.Open == this.State)
				{
					ADP.TimerCurrent(out this._statistics._openTimestamp);
				}
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00279224 File Offset: 0x00278624
		public IDictionary RetrieveStatistics()
		{
			if (this.IsContextConnection)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			if (this.Statistics != null)
			{
				this.UpdateStatistics();
				return this.Statistics.GetHashtable();
			}
			return new SqlStatistics().GetHashtable();
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00279264 File Offset: 0x00278664
		private void UpdateStatistics()
		{
			if (ConnectionState.Open == this.State)
			{
				ADP.TimerCurrent(out this._statistics._closeTimestamp);
			}
			this.Statistics.UpdateStatistics();
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00279298 File Offset: 0x00278698
		internal static void CheckGetExtendedUDTInfo(SqlMetaDataPriv metaData, bool fThrow)
		{
			if (metaData.udtType == null)
			{
				metaData.udtType = Type.GetType(metaData.udtAssemblyQualifiedName, fThrow);
				if (fThrow && metaData.udtType == null)
				{
					throw SQL.UDTUnexpectedResult(metaData.udtAssemblyQualifiedName);
				}
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x002792D8 File Offset: 0x002786D8
		internal object GetUdtValue(object value, SqlMetaDataPriv metaData, bool returnDBNull)
		{
			if (returnDBNull && ADP.IsNull(value))
			{
				return DBNull.Value;
			}
			if (ADP.IsNull(value))
			{
				Type udtType = metaData.udtType;
				return udtType.InvokeMember("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, new object[0], CultureInfo.InvariantCulture);
			}
			MemoryStream memoryStream = new MemoryStream((byte[])value);
			return SerializationHelperSql9.Deserialize(memoryStream, metaData.udtType);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00279344 File Offset: 0x00278744
		internal byte[] GetBytes(object o)
		{
			Format format = Format.Native;
			int num = 0;
			return this.GetBytes(o, out format, out num);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00279360 File Offset: 0x00278760
		internal byte[] GetBytes(object o, out Format format, out int maxSize)
		{
			SqlUdtInfo infoFromType = AssemblyCache.GetInfoFromType(o.GetType());
			maxSize = infoFromType.MaxByteSize;
			format = infoFromType.SerializationFormat;
			if (maxSize < -1 || maxSize >= 65535)
			{
				throw new InvalidOperationException(o.GetType() + ": invalid Size");
			}
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream((maxSize < 0) ? 0 : maxSize))
			{
				SerializationHelperSql9.Serialize(memoryStream, o);
				array = memoryStream.ToArray();
			}
			return array;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x002793F4 File Offset: 0x002787F4
		public SqlConnection()
		{
			this.ObjectID = Interlocked.Increment(ref SqlConnection._objectTypeCount);
			base..ctor();
			GC.SuppressFinalize(this);
			this._innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00279428 File Offset: 0x00278828
		private void CopyFrom(SqlConnection connection)
		{
			ADP.CheckArgumentNull(connection, "connection");
			this._userConnectionOptions = connection.UserConnectionOptions;
			this._poolGroup = connection.PoolGroup;
			if (DbConnectionClosedNeverOpened.SingletonInstance == connection._innerConnection)
			{
				this._innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
				return;
			}
			this._innerConnection = DbConnectionClosedPreviouslyOpened.SingletonInstance;
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x0027947C File Offset: 0x0027887C
		internal int CloseCount
		{
			get
			{
				return this._closeCount;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x00279490 File Offset: 0x00278890
		internal DbConnectionFactory ConnectionFactory
		{
			get
			{
				return SqlConnection._connectionFactory;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060024D1 RID: 9425 RVA: 0x002794A4 File Offset: 0x002788A4
		internal DbConnectionOptions ConnectionOptions
		{
			get
			{
				DbConnectionPoolGroup poolGroup = this.PoolGroup;
				if (poolGroup == null)
				{
					return null;
				}
				return poolGroup.ConnectionOptions;
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x002794C4 File Offset: 0x002788C4
		private string ConnectionString_Get()
		{
			Bid.Trace("<prov.DbConnectionHelper.ConnectionString_Get|API> %d#\n", this.ObjectID);
			bool shouldHidePassword = this.InnerConnection.ShouldHidePassword;
			DbConnectionOptions userConnectionOptions = this.UserConnectionOptions;
			if (userConnectionOptions == null)
			{
				return "";
			}
			return userConnectionOptions.UsersConnectionString(shouldHidePassword);
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00279504 File Offset: 0x00278904
		private void ConnectionString_Set(string value)
		{
			DbConnectionOptions dbConnectionOptions = null;
			DbConnectionPoolGroup connectionPoolGroup = this.ConnectionFactory.GetConnectionPoolGroup(value, null, ref dbConnectionOptions);
			DbConnectionInternal innerConnection = this.InnerConnection;
			bool flag = innerConnection.AllowSetConnectionString;
			if (flag)
			{
				flag = this.SetInnerConnectionFrom(DbConnectionClosedBusy.SingletonInstance, innerConnection);
				if (flag)
				{
					this._userConnectionOptions = dbConnectionOptions;
					this._poolGroup = connectionPoolGroup;
					this._innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
				}
			}
			if (!flag)
			{
				throw ADP.OpenConnectionPropertySet("ConnectionString", innerConnection.State);
			}
			if (Bid.TraceOn)
			{
				string text = ((dbConnectionOptions != null) ? dbConnectionOptions.UsersConnectionStringForTrace() : "");
				Bid.Trace("<prov.DbConnectionHelper.ConnectionString_Set|API> %d#, '%ls'\n", this.ObjectID, text);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x0027959C File Offset: 0x0027899C
		internal DbConnectionInternal InnerConnection
		{
			get
			{
				return this._innerConnection;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060024D5 RID: 9429 RVA: 0x002795B0 File Offset: 0x002789B0
		// (set) Token: 0x060024D6 RID: 9430 RVA: 0x002795C4 File Offset: 0x002789C4
		internal DbConnectionPoolGroup PoolGroup
		{
			get
			{
				return this._poolGroup;
			}
			set
			{
				this._poolGroup = value;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x002795D8 File Offset: 0x002789D8
		[ResDescription("DbConnection_State")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override ConnectionState State
		{
			get
			{
				return this.InnerConnection.State;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060024D8 RID: 9432 RVA: 0x002795F0 File Offset: 0x002789F0
		internal DbConnectionOptions UserConnectionOptions
		{
			get
			{
				return this._userConnectionOptions;
			}
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x00279604 File Offset: 0x00278A04
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Abort(Exception e)
		{
			DbConnectionInternal innerConnection = this._innerConnection;
			if (ConnectionState.Open == innerConnection.State)
			{
				Interlocked.CompareExchange<DbConnectionInternal>(ref this._innerConnection, DbConnectionClosedPreviouslyOpened.SingletonInstance, innerConnection);
				innerConnection.DoomThisConnection();
			}
			if (e is OutOfMemoryException)
			{
				Bid.Trace("<prov.DbConnectionHelper.Abort|RES|INFO|CPOOL> %d#, Aborting operation due to asynchronous exception: %ls\n", this.ObjectID, "OutOfMemory");
				return;
			}
			Bid.Trace("<prov.DbConnectionHelper.Abort|RES|INFO|CPOOL> %d#, Aborting operation due to asynchronous exception: %ls\n", this.ObjectID, e.ToString());
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x00279670 File Offset: 0x00278A70
		internal void AddWeakReference(object value, int tag)
		{
			this.InnerConnection.AddWeakReference(value, tag);
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x0027968C File Offset: 0x00278A8C
		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<prov.DbConnectionHelper.BeginDbTransaction|API> %d#, isolationLevel=%d{ds.IsolationLevel}", this.ObjectID, (int)isolationLevel);
			DbTransaction dbTransaction2;
			try
			{
				DbTransaction dbTransaction = this.InnerConnection.BeginTransaction(isolationLevel);
				dbTransaction2 = dbTransaction;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dbTransaction2;
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x002796E4 File Offset: 0x00278AE4
		protected override DbCommand CreateDbCommand()
		{
			DbCommand dbCommand = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<prov.DbConnectionHelper.CreateDbCommand|API> %d#\n", this.ObjectID);
			try
			{
				DbProviderFactory providerFactory = this.ConnectionFactory.ProviderFactory;
				dbCommand = providerFactory.CreateCommand();
				dbCommand.Connection = this;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dbCommand;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x00279748 File Offset: 0x00278B48
		private static CodeAccessPermission CreateExecutePermission()
		{
			DBDataPermission dbdataPermission = (DBDataPermission)SqlConnectionFactory.SingletonInstance.ProviderFactory.CreatePermission(PermissionState.None);
			dbdataPermission.Add(string.Empty, string.Empty, KeyRestrictionBehavior.AllowOnly);
			return dbdataPermission;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x00279780 File Offset: 0x00278B80
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._userConnectionOptions = null;
				this._poolGroup = null;
				this.Close();
			}
			this.DisposeMe(disposing);
			base.Dispose(disposing);
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x002797B4 File Offset: 0x00278BB4
		private void EnlistDistributedTransactionHelper(ITransaction transaction)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(SqlConnection.ExecutePermission);
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Demand();
			Bid.Trace("<prov.DbConnectionHelper.EnlistDistributedTransactionHelper|RES|TRAN> %d#, Connection enlisting in a transaction.\n", this.ObjectID);
			Transaction transaction2 = null;
			if (transaction != null)
			{
				transaction2 = TransactionInterop.GetTransactionFromDtcTransaction((IDtcTransaction)transaction);
			}
			this.InnerConnection.EnlistTransaction(transaction2);
			GC.KeepAlive(this);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x0027981C File Offset: 0x00278C1C
		public override void EnlistTransaction(Transaction transaction)
		{
			SqlConnection.ExecutePermission.Demand();
			Bid.Trace("<prov.DbConnectionHelper.EnlistTransaction|RES|TRAN> %d#, Connection enlisting in a transaction.\n", this.ObjectID);
			DbConnectionInternal innerConnection = this.InnerConnection;
			if (!innerConnection.HasEnlistedTransaction)
			{
				innerConnection.EnlistTransaction(transaction);
				GC.KeepAlive(this);
				return;
			}
			if (innerConnection.EnlistedTransaction.Equals(transaction))
			{
				return;
			}
			throw ADP.TransactionPresent();
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x00279874 File Offset: 0x00278C74
		private DbMetaDataFactory GetMetaDataFactory(DbConnectionInternal internalConnection)
		{
			return this.ConnectionFactory.GetMetaDataFactory(this._poolGroup, internalConnection);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x00279894 File Offset: 0x00278C94
		internal DbMetaDataFactory GetMetaDataFactoryInternal(DbConnectionInternal internalConnection)
		{
			return this.GetMetaDataFactory(internalConnection);
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x002798A8 File Offset: 0x00278CA8
		public override DataTable GetSchema()
		{
			return this.GetSchema(DbMetaDataCollectionNames.MetaDataCollections, null);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x002798C4 File Offset: 0x00278CC4
		public override DataTable GetSchema(string collectionName)
		{
			return this.GetSchema(collectionName, null);
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x002798DC File Offset: 0x00278CDC
		public override DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			SqlConnection.ExecutePermission.Demand();
			return this.InnerConnection.GetSchema(this.ConnectionFactory, this.PoolGroup, this, collectionName, restrictionValues);
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x00279910 File Offset: 0x00278D10
		internal void NotifyWeakReference(int message)
		{
			this.InnerConnection.NotifyWeakReference(message);
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x0027992C File Offset: 0x00278D2C
		internal void PermissionDemand()
		{
			DbConnectionPoolGroup poolGroup = this.PoolGroup;
			DbConnectionOptions dbConnectionOptions = ((poolGroup != null) ? poolGroup.ConnectionOptions : null);
			if (dbConnectionOptions == null || dbConnectionOptions.IsEmpty)
			{
				throw ADP.NoConnectionString();
			}
			DbConnectionOptions userConnectionOptions = this.UserConnectionOptions;
			userConnectionOptions.DemandPermission();
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x0027996C File Offset: 0x00278D6C
		internal void RemoveWeakReference(object value)
		{
			this.InnerConnection.RemoveWeakReference(value);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x00279988 File Offset: 0x00278D88
		internal void SetInnerConnectionEvent(DbConnectionInternal to)
		{
			ConnectionState connectionState = this._innerConnection.State & ConnectionState.Open;
			ConnectionState connectionState2 = to.State & ConnectionState.Open;
			if (connectionState != connectionState2 && connectionState2 == ConnectionState.Closed)
			{
				this._closeCount++;
			}
			this._innerConnection = to;
			if (connectionState == ConnectionState.Closed && ConnectionState.Open == connectionState2)
			{
				this.OnStateChange(DbConnectionInternal.StateChangeOpen);
				return;
			}
			if (ConnectionState.Open == connectionState && connectionState2 == ConnectionState.Closed)
			{
				this.OnStateChange(DbConnectionInternal.StateChangeClosed);
				return;
			}
			if (connectionState != connectionState2)
			{
				this.OnStateChange(new StateChangeEventArgs(connectionState, connectionState2));
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x00279A00 File Offset: 0x00278E00
		internal bool SetInnerConnectionFrom(DbConnectionInternal to, DbConnectionInternal from)
		{
			return from == Interlocked.CompareExchange<DbConnectionInternal>(ref this._innerConnection, to, from);
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x00279A20 File Offset: 0x00278E20
		internal void SetInnerConnectionTo(DbConnectionInternal to)
		{
			this._innerConnection = to;
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00279A34 File Offset: 0x00278E34
		[Conditional("DEBUG")]
		internal static void VerifyExecutePermission()
		{
			try
			{
				SqlConnection.ExecutePermission.Demand();
			}
			catch (SecurityException)
			{
				throw;
			}
		}

		// Token: 0x04001748 RID: 5960
		private static readonly object EventInfoMessage = new object();

		// Token: 0x04001749 RID: 5961
		private SqlDebugContext _sdc;

		// Token: 0x0400174A RID: 5962
		private bool _AsycCommandInProgress;

		// Token: 0x0400174B RID: 5963
		internal SqlStatistics _statistics;

		// Token: 0x0400174C RID: 5964
		private bool _collectstats;

		// Token: 0x0400174D RID: 5965
		private bool _fireInfoMessageEventOnUserErrors;

		// Token: 0x0400174E RID: 5966
		private static readonly DbConnectionFactory _connectionFactory = SqlConnectionFactory.SingletonInstance;

		// Token: 0x0400174F RID: 5967
		internal static readonly CodeAccessPermission ExecutePermission = SqlConnection.CreateExecutePermission();

		// Token: 0x04001750 RID: 5968
		private DbConnectionOptions _userConnectionOptions;

		// Token: 0x04001751 RID: 5969
		private DbConnectionPoolGroup _poolGroup;

		// Token: 0x04001752 RID: 5970
		private DbConnectionInternal _innerConnection;

		// Token: 0x04001753 RID: 5971
		private int _closeCount;

		// Token: 0x04001754 RID: 5972
		private static int _objectTypeCount;

		// Token: 0x04001755 RID: 5973
		internal readonly int ObjectID;
	}
}
