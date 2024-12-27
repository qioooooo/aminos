using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Sql;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Data.SqlClient
{
	// Token: 0x020002C8 RID: 712
	public sealed class SqlCommandBuilder : DbCommandBuilder
	{
		// Token: 0x06002456 RID: 9302 RVA: 0x00277438 File Offset: 0x00276838
		public SqlCommandBuilder()
		{
			GC.SuppressFinalize(this);
			base.QuotePrefix = "[";
			base.QuoteSuffix = "]";
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x00277468 File Offset: 0x00276868
		public SqlCommandBuilder(SqlDataAdapter adapter)
			: this()
		{
			this.DataAdapter = adapter;
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002458 RID: 9304 RVA: 0x00277484 File Offset: 0x00276884
		// (set) Token: 0x06002459 RID: 9305 RVA: 0x00277494 File Offset: 0x00276894
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override CatalogLocation CatalogLocation
		{
			get
			{
				return CatalogLocation.Start;
			}
			set
			{
				if (CatalogLocation.Start != value)
				{
					throw ADP.SingleValuedProperty("CatalogLocation", "Start");
				}
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x002774B8 File Offset: 0x002768B8
		// (set) Token: 0x0600245B RID: 9307 RVA: 0x002774CC File Offset: 0x002768CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string CatalogSeparator
		{
			get
			{
				return ".";
			}
			set
			{
				if ("." != value)
				{
					throw ADP.SingleValuedProperty("CatalogSeparator", ".");
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x002774F8 File Offset: 0x002768F8
		// (set) Token: 0x0600245D RID: 9309 RVA: 0x00277510 File Offset: 0x00276910
		[ResCategory("DataCategory_Update")]
		[ResDescription("SqlCommandBuilder_DataAdapter")]
		[DefaultValue(null)]
		public new SqlDataAdapter DataAdapter
		{
			get
			{
				return (SqlDataAdapter)base.DataAdapter;
			}
			set
			{
				base.DataAdapter = value;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x00277524 File Offset: 0x00276924
		// (set) Token: 0x0600245F RID: 9311 RVA: 0x00277538 File Offset: 0x00276938
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string QuotePrefix
		{
			get
			{
				return base.QuotePrefix;
			}
			set
			{
				if ("[" != value && "\"" != value)
				{
					throw ADP.DoubleValuedProperty("QuotePrefix", "[", "\"");
				}
				base.QuotePrefix = value;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x0027757C File Offset: 0x0027697C
		// (set) Token: 0x06002461 RID: 9313 RVA: 0x00277590 File Offset: 0x00276990
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string QuoteSuffix
		{
			get
			{
				return base.QuoteSuffix;
			}
			set
			{
				if ("]" != value && "\"" != value)
				{
					throw ADP.DoubleValuedProperty("QuoteSuffix", "]", "\"");
				}
				base.QuoteSuffix = value;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x002775D4 File Offset: 0x002769D4
		// (set) Token: 0x06002463 RID: 9315 RVA: 0x002775E8 File Offset: 0x002769E8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string SchemaSeparator
		{
			get
			{
				return ".";
			}
			set
			{
				if ("." != value)
				{
					throw ADP.SingleValuedProperty("SchemaSeparator", ".");
				}
			}
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x00277614 File Offset: 0x00276A14
		private void SqlRowUpdatingHandler(object sender, SqlRowUpdatingEventArgs ruevent)
		{
			base.RowUpdatingHandler(ruevent);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x00277628 File Offset: 0x00276A28
		public new SqlCommand GetInsertCommand()
		{
			return (SqlCommand)base.GetInsertCommand();
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x00277640 File Offset: 0x00276A40
		public new SqlCommand GetInsertCommand(bool useColumnsForParameterNames)
		{
			return (SqlCommand)base.GetInsertCommand(useColumnsForParameterNames);
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0027765C File Offset: 0x00276A5C
		public new SqlCommand GetUpdateCommand()
		{
			return (SqlCommand)base.GetUpdateCommand();
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x00277674 File Offset: 0x00276A74
		public new SqlCommand GetUpdateCommand(bool useColumnsForParameterNames)
		{
			return (SqlCommand)base.GetUpdateCommand(useColumnsForParameterNames);
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x00277690 File Offset: 0x00276A90
		public new SqlCommand GetDeleteCommand()
		{
			return (SqlCommand)base.GetDeleteCommand();
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x002776A8 File Offset: 0x00276AA8
		public new SqlCommand GetDeleteCommand(bool useColumnsForParameterNames)
		{
			return (SqlCommand)base.GetDeleteCommand(useColumnsForParameterNames);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x002776C4 File Offset: 0x00276AC4
		protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
		{
			SqlParameter sqlParameter = (SqlParameter)parameter;
			object obj = datarow[SchemaTableColumn.ProviderType];
			sqlParameter.SqlDbType = (SqlDbType)obj;
			sqlParameter.Offset = 0;
			if (sqlParameter.SqlDbType == SqlDbType.Udt && !sqlParameter.SourceColumnNullMapping)
			{
				sqlParameter.UdtTypeName = datarow["DataTypeName"] as string;
			}
			else
			{
				sqlParameter.UdtTypeName = string.Empty;
			}
			object obj2 = datarow[SchemaTableColumn.NumericPrecision];
			if (DBNull.Value != obj2)
			{
				byte b = (byte)((short)obj2);
				sqlParameter.PrecisionInternal = ((byte.MaxValue != b) ? b : 0);
			}
			obj2 = datarow[SchemaTableColumn.NumericScale];
			if (DBNull.Value != obj2)
			{
				byte b2 = (byte)((short)obj2);
				sqlParameter.ScaleInternal = ((byte.MaxValue != b2) ? b2 : 0);
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0027778C File Offset: 0x00276B8C
		protected override string GetParameterName(int parameterOrdinal)
		{
			return "@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x002777B0 File Offset: 0x00276BB0
		protected override string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x002777C8 File Offset: 0x00276BC8
		protected override string GetParameterPlaceholder(int parameterOrdinal)
		{
			return "@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x002777EC File Offset: 0x00276BEC
		private void ConsistentQuoteDelimiters(string quotePrefix, string quoteSuffix)
		{
			if (("\"" == quotePrefix && "\"" != quoteSuffix) || ("[" == quotePrefix && "]" != quoteSuffix))
			{
				throw ADP.InvalidPrefixSuffix();
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x00277834 File Offset: 0x00276C34
		public static void DeriveParameters(SqlCommand command)
		{
			SqlConnection.ExecutePermission.Demand();
			if (command == null)
			{
				throw ADP.ArgumentNull("command");
			}
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(command.Connection);
				command.DeriveParameters();
			}
			catch (OutOfMemoryException ex)
			{
				if (command != null && command.Connection != null)
				{
					command.Connection.Abort(ex);
				}
				throw;
			}
			catch (StackOverflowException ex2)
			{
				if (command != null && command.Connection != null)
				{
					command.Connection.Abort(ex2);
				}
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				if (command != null && command.Connection != null)
				{
					command.Connection.Abort(ex3);
				}
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0027791C File Offset: 0x00276D1C
		protected override DataTable GetSchemaTable(DbCommand srcCommand)
		{
			SqlCommand sqlCommand = srcCommand as SqlCommand;
			SqlNotificationRequest notification = sqlCommand.Notification;
			bool notificationAutoEnlist = sqlCommand.NotificationAutoEnlist;
			sqlCommand.Notification = null;
			sqlCommand.NotificationAutoEnlist = false;
			DataTable schemaTable;
			try
			{
				using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
				{
					schemaTable = sqlDataReader.GetSchemaTable();
				}
			}
			finally
			{
				sqlCommand.Notification = notification;
				sqlCommand.NotificationAutoEnlist = notificationAutoEnlist;
			}
			return schemaTable;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x002779B0 File Offset: 0x00276DB0
		protected override DbCommand InitializeCommand(DbCommand command)
		{
			SqlCommand sqlCommand = (SqlCommand)base.InitializeCommand(command);
			sqlCommand.NotificationAutoEnlist = false;
			return sqlCommand;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x002779D4 File Offset: 0x00276DD4
		public override string QuoteIdentifier(string unquotedIdentifier)
		{
			ADP.CheckArgumentNull(unquotedIdentifier, "unquotedIdentifier");
			string quoteSuffix = this.QuoteSuffix;
			string quotePrefix = this.QuotePrefix;
			this.ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
			return ADP.BuildQuotedString(quotePrefix, quoteSuffix, unquotedIdentifier);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x00277A0C File Offset: 0x00276E0C
		protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
		{
			if (adapter == base.DataAdapter)
			{
				((SqlDataAdapter)adapter).RowUpdating -= this.SqlRowUpdatingHandler;
				return;
			}
			((SqlDataAdapter)adapter).RowUpdating += this.SqlRowUpdatingHandler;
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x00277A54 File Offset: 0x00276E54
		public override string UnquoteIdentifier(string quotedIdentifier)
		{
			ADP.CheckArgumentNull(quotedIdentifier, "quotedIdentifier");
			string quoteSuffix = this.QuoteSuffix;
			string quotePrefix = this.QuotePrefix;
			this.ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
			string text;
			ADP.RemoveStringQuotes(quotePrefix, quoteSuffix, quotedIdentifier, out text);
			return text;
		}
	}
}
