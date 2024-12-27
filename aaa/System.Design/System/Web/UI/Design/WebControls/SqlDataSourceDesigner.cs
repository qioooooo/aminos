using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Data;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003D7 RID: 983
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SqlDataSourceDesigner : DataSourceDesigner
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x000C19D8 File Offset: 0x000C09D8
		public override bool CanConfigure
		{
			get
			{
				IDataEnvironment dataEnvironment = (IDataEnvironment)base.Component.Site.GetService(typeof(IDataEnvironment));
				return dataEnvironment != null;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x000C1A0C File Offset: 0x000C0A0C
		public override bool CanRefreshSchema
		{
			get
			{
				string connectionString = this.ConnectionString;
				return connectionString != null && connectionString.Trim().Length != 0 && this.SelectCommand.Trim().Length != 0;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002429 RID: 9257 RVA: 0x000C1A48 File Offset: 0x000C0A48
		// (set) Token: 0x0600242A RID: 9258 RVA: 0x000C1A50 File Offset: 0x000C0A50
		public string ConnectionString
		{
			get
			{
				return this.GetConnectionString();
			}
			set
			{
				if (value != this.ConnectionString)
				{
					this.SqlDataSource.ConnectionString = value;
					this.UpdateDesignTimeHtml();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x000C1A7D File Offset: 0x000C0A7D
		// (set) Token: 0x0600242C RID: 9260 RVA: 0x000C1A80 File Offset: 0x000C0A80
		[SRDescription("SqlDataSourceDesigner_DeleteQuery")]
		[TypeConverter(typeof(SqlDataSourceQueryConverter))]
		[Category("Data")]
		[DefaultValue(DataSourceOperation.Delete)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Editor(typeof(SqlDataSourceQueryEditor), typeof(UITypeEditor))]
		[MergableProperty(false)]
		public DataSourceOperation DeleteQuery
		{
			get
			{
				return DataSourceOperation.Delete;
			}
			set
			{
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x000C1A82 File Offset: 0x000C0A82
		// (set) Token: 0x0600242E RID: 9262 RVA: 0x000C1A85 File Offset: 0x000C0A85
		[TypeConverter(typeof(SqlDataSourceQueryConverter))]
		[Category("Data")]
		[DefaultValue(DataSourceOperation.Insert)]
		[SRDescription("SqlDataSourceDesigner_InsertQuery")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Editor(typeof(SqlDataSourceQueryEditor), typeof(UITypeEditor))]
		[MergableProperty(false)]
		public DataSourceOperation InsertQuery
		{
			get
			{
				return DataSourceOperation.Insert;
			}
			set
			{
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x000C1A87 File Offset: 0x000C0A87
		// (set) Token: 0x06002430 RID: 9264 RVA: 0x000C1A94 File Offset: 0x000C0A94
		public string ProviderName
		{
			get
			{
				return this.SqlDataSource.ProviderName;
			}
			set
			{
				if (value != this.ProviderName)
				{
					this.SqlDataSource.ProviderName = value;
					this.UpdateDesignTimeHtml();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x000C1AC4 File Offset: 0x000C0AC4
		// (set) Token: 0x06002432 RID: 9266 RVA: 0x000C1AED File Offset: 0x000C0AED
		internal bool SaveConfiguredConnectionState
		{
			get
			{
				object obj = base.DesignerState["SaveConfiguredConnectionState"];
				return obj == null || (bool)obj;
			}
			set
			{
				base.DesignerState["SaveConfiguredConnectionState"] = value;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x000C1B05 File Offset: 0x000C0B05
		// (set) Token: 0x06002434 RID: 9268 RVA: 0x000C1B12 File Offset: 0x000C0B12
		public string SelectCommand
		{
			get
			{
				return this.SqlDataSource.SelectCommand;
			}
			set
			{
				if (value != this.SelectCommand)
				{
					this.SqlDataSource.SelectCommand = value;
					this.UpdateDesignTimeHtml();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x000C1B3F File Offset: 0x000C0B3F
		// (set) Token: 0x06002436 RID: 9270 RVA: 0x000C1B42 File Offset: 0x000C0B42
		[Category("Data")]
		[TypeConverter(typeof(SqlDataSourceQueryConverter))]
		[DefaultValue(DataSourceOperation.Select)]
		[SRDescription("SqlDataSourceDesigner_SelectQuery")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Editor(typeof(SqlDataSourceQueryEditor), typeof(UITypeEditor))]
		[MergableProperty(false)]
		public DataSourceOperation SelectQuery
		{
			get
			{
				return DataSourceOperation.Select;
			}
			set
			{
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x000C1B44 File Offset: 0x000C0B44
		internal SqlDataSource SqlDataSource
		{
			get
			{
				return (SqlDataSource)base.Component;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x000C1B51 File Offset: 0x000C0B51
		// (set) Token: 0x06002439 RID: 9273 RVA: 0x000C1B68 File Offset: 0x000C0B68
		internal Hashtable TableQueryState
		{
			get
			{
				return base.DesignerState["TableQueryState"] as Hashtable;
			}
			set
			{
				base.DesignerState["TableQueryState"] = value;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x000C1B7B File Offset: 0x000C0B7B
		// (set) Token: 0x0600243B RID: 9275 RVA: 0x000C1B7E File Offset: 0x000C0B7E
		[Category("Data")]
		[Editor(typeof(SqlDataSourceQueryEditor), typeof(UITypeEditor))]
		[DefaultValue(DataSourceOperation.Update)]
		[SRDescription("SqlDataSourceDesigner_UpdateQuery")]
		[TypeConverter(typeof(SqlDataSourceQueryConverter))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		public DataSourceOperation UpdateQuery
		{
			get
			{
				return DataSourceOperation.Update;
			}
			set
			{
			}
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000C1B80 File Offset: 0x000C0B80
		internal DbCommand BuildSelectCommand(DbProviderFactory factory, DbConnection connection, string commandText, ParameterCollection parameters, SqlDataSourceCommandType commandType)
		{
			DbCommand dbCommand = SqlDataSourceDesigner.CreateCommand(factory, commandText, connection);
			if (parameters != null && parameters.Count > 0)
			{
				IOrderedDictionary values = parameters.GetValues(null, null);
				string parameterPrefix = SqlDataSourceDesigner.GetParameterPrefix(factory);
				for (int i = 0; i < parameters.Count; i++)
				{
					Parameter parameter = parameters[i];
					DbParameter dbParameter = SqlDataSourceDesigner.CreateParameter(factory);
					dbParameter.ParameterName = parameterPrefix + parameter.Name;
					if (parameter.DbType != DbType.Object)
					{
						SqlParameter sqlParameter = dbParameter as SqlParameter;
						if (sqlParameter == null)
						{
							dbParameter.DbType = parameter.DbType;
						}
						else if (parameter.DbType == DbType.Date)
						{
							sqlParameter.SqlDbType = SqlDbType.Date;
						}
						else if (parameter.DbType == DbType.Time)
						{
							sqlParameter.SqlDbType = SqlDbType.Time;
						}
						else
						{
							dbParameter.DbType = parameter.DbType;
						}
					}
					else
					{
						if (parameter.Type != TypeCode.Empty && parameter.Type != TypeCode.DBNull)
						{
							dbParameter.DbType = parameter.GetDatabaseType();
						}
						if (parameter.Type == TypeCode.Empty && SqlDataSourceDesigner.ProviderRequiresDbTypeSet(factory))
						{
							dbParameter.DbType = DbType.Object;
						}
					}
					dbParameter.Value = values[i];
					if (dbParameter.Value == null)
					{
						dbParameter.Value = DBNull.Value;
					}
					if (Parameter.ConvertDbTypeToTypeCode(dbParameter.DbType) == TypeCode.String)
					{
						if (dbParameter.Value is string && dbParameter.Value != null)
						{
							dbParameter.Size = ((string)dbParameter.Value).Length;
						}
						else
						{
							dbParameter.Size = 1;
						}
					}
					dbCommand.Parameters.Add(dbParameter);
				}
			}
			dbCommand.CommandType = SqlDataSourceDesigner.GetCommandType(commandType);
			return dbCommand;
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000C1D20 File Offset: 0x000C0D20
		public override void Configure()
		{
			try
			{
				this.SuppressDataSourceEvents();
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConfigureDataSourceChangeCallback), null, SR.GetString("DataSource_ConfigureTransactionDescription"));
			}
			finally
			{
				this.ResumeDataSourceEvents();
			}
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000C1D70 File Offset: 0x000C0D70
		private bool ConfigureDataSourceChangeCallback(object context)
		{
			IServiceProvider site = base.Component.Site;
			IDataEnvironment dataEnvironment = (IDataEnvironment)site.GetService(typeof(IDataEnvironment));
			if (dataEnvironment == null)
			{
				return false;
			}
			IDataSourceViewSchema dataSourceViewSchema = this.GetView("DefaultView").Schema;
			bool flag = false;
			if (dataSourceViewSchema == null)
			{
				this._forceSchemaRetrieval = true;
				dataSourceViewSchema = this.GetView("DefaultView").Schema;
				this._forceSchemaRetrieval = false;
				if (dataSourceViewSchema != null)
				{
					flag = true;
				}
			}
			SqlDataSourceWizardForm sqlDataSourceWizardForm = this.CreateConfigureDataSourceWizardForm(site, dataEnvironment);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(site, sqlDataSourceWizardForm);
			if (dialogResult == DialogResult.OK)
			{
				this.OnComponentChanged(this, new ComponentChangedEventArgs(base.Component, null, null, null));
				IDataSourceViewSchema dataSourceViewSchema2 = null;
				try
				{
					this._forceSchemaRetrieval = true;
					dataSourceViewSchema2 = this.GetView("DefaultView").Schema;
				}
				finally
				{
					this._forceSchemaRetrieval = false;
				}
				if (!flag && !DataSourceDesigner.ViewSchemasEquivalent(dataSourceViewSchema, dataSourceViewSchema2))
				{
					this.OnSchemaRefreshed(EventArgs.Empty);
				}
				this.OnDataSourceChanged(EventArgs.Empty);
				return true;
			}
			return false;
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000C1E68 File Offset: 0x000C0E68
		internal static bool ConnectionsEqual(DesignerDataConnection connection1, DesignerDataConnection connection2)
		{
			if (connection1 == null || connection2 == null)
			{
				return false;
			}
			if (connection1.ConnectionString != connection2.ConnectionString)
			{
				return false;
			}
			string text = ((connection1.ProviderName.Trim().Length == 0) ? "System.Data.SqlClient" : connection1.ProviderName);
			string text2 = ((connection2.ProviderName.Trim().Length == 0) ? "System.Data.SqlClient" : connection2.ProviderName);
			return text == text2;
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000C1ED9 File Offset: 0x000C0ED9
		internal static TypeCode ConvertDbTypeToTypeCode(DbType dbType)
		{
			return Parameter.ConvertDbTypeToTypeCode(dbType);
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000C1EE1 File Offset: 0x000C0EE1
		internal static DbType ConvertTypeCodeToDbType(TypeCode typeCode)
		{
			return Parameter.ConvertTypeCodeToDbType(typeCode);
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x000C1EEC File Offset: 0x000C0EEC
		internal void CopyList(ICollection source, IList dest)
		{
			dest.Clear();
			foreach (object obj in source)
			{
				ICloneable cloneable = (ICloneable)obj;
				object obj2 = cloneable.Clone();
				base.RegisterClone(cloneable, obj2);
				dest.Add(obj2);
			}
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000C1F58 File Offset: 0x000C0F58
		internal virtual SqlDataSourceWizardForm CreateConfigureDataSourceWizardForm(IServiceProvider serviceProvider, IDataEnvironment dataEnvironment)
		{
			return new SqlDataSourceWizardForm(serviceProvider, this, dataEnvironment);
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000C1F64 File Offset: 0x000C0F64
		internal static DbCommand CreateCommand(DbProviderFactory factory, string commandText, DbConnection connection)
		{
			DbCommand dbCommand = factory.CreateCommand();
			dbCommand.CommandText = commandText;
			dbCommand.Connection = connection;
			return dbCommand;
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x000C1F88 File Offset: 0x000C0F88
		internal static DbDataAdapter CreateDataAdapter(DbProviderFactory factory, DbCommand command)
		{
			DbDataAdapter dbDataAdapter = factory.CreateDataAdapter();
			((IDbDataAdapter)dbDataAdapter).SelectCommand = command;
			return dbDataAdapter;
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x000C1FA4 File Offset: 0x000C0FA4
		internal static DbParameter CreateParameter(DbProviderFactory factory)
		{
			return factory.CreateParameter();
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x000C1FAC File Offset: 0x000C0FAC
		internal static Parameter CreateParameter(DbProviderFactory factory, string name, DbType dbType)
		{
			if (SqlDataSourceDesigner.IsNewSqlServer2008Type(factory, dbType))
			{
				return new Parameter(name, dbType);
			}
			return new Parameter(name, SqlDataSourceDesigner.ConvertDbTypeToTypeCode(dbType));
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x000C1FCB File Offset: 0x000C0FCB
		protected virtual SqlDesignerDataSourceView CreateView(string viewName)
		{
			return new SqlDesignerDataSourceView(this, viewName);
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x000C1FD4 File Offset: 0x000C0FD4
		protected virtual void DeriveParameters(string providerName, DbCommand command)
		{
			if (string.Equals(providerName, "System.Data.Odbc", StringComparison.OrdinalIgnoreCase))
			{
				OdbcCommandBuilder.DeriveParameters((OdbcCommand)command);
				return;
			}
			if (string.Equals(providerName, "System.Data.OleDb", StringComparison.OrdinalIgnoreCase))
			{
				OleDbCommandBuilder.DeriveParameters((OleDbCommand)command);
				return;
			}
			if (string.Equals(providerName, "System.Data.SqlClient", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(providerName))
			{
				SqlCommandBuilder.DeriveParameters((SqlCommand)command);
				return;
			}
			UIServiceHelper.ShowError(this.SqlDataSource.Site, SR.GetString("SqlDataSourceDesigner_InferStoredProcedureNotSupported", new object[] { providerName }));
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x000C205D File Offset: 0x000C105D
		private static CommandType GetCommandType(SqlDataSourceCommandType commandType)
		{
			if (commandType == SqlDataSourceCommandType.Text)
			{
				return CommandType.Text;
			}
			return CommandType.StoredProcedure;
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x000C2065 File Offset: 0x000C1065
		protected virtual string GetConnectionString()
		{
			return this.SqlDataSource.ConnectionString;
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x000C2074 File Offset: 0x000C1074
		internal static DbProviderFactory GetDbProviderFactory(string providerName)
		{
			if (providerName.Length == 0)
			{
				providerName = "System.Data.SqlClient";
			}
			return DbProviderFactories.GetFactory(providerName);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000C2098 File Offset: 0x000C1098
		internal static DbConnection GetDesignTimeConnection(IServiceProvider serviceProvider, DesignerDataConnection connection)
		{
			if (serviceProvider != null)
			{
				IDataEnvironment dataEnvironment = (IDataEnvironment)serviceProvider.GetService(typeof(IDataEnvironment));
				if (dataEnvironment != null)
				{
					if (string.IsNullOrEmpty(connection.ProviderName))
					{
						connection = new DesignerDataConnection(connection.Name, "System.Data.SqlClient", connection.ConnectionString);
					}
					return dataEnvironment.GetDesignTimeConnection(connection);
				}
			}
			return null;
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x000C20EF File Offset: 0x000C10EF
		public override DesignerDataSourceView GetView(string viewName)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				viewName = "DefaultView";
			}
			if (string.Equals(viewName, "DefaultView", StringComparison.OrdinalIgnoreCase))
			{
				if (this._view == null)
				{
					this._view = this.CreateView(viewName);
				}
				return this._view;
			}
			return null;
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000C212C File Offset: 0x000C112C
		public override string[] GetViewNames()
		{
			return new string[] { "DefaultView" };
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000C2149 File Offset: 0x000C1149
		internal static string GetParameterPlaceholderPrefix(DbProviderFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			if (factory == SqlClientFactory.Instance)
			{
				return "@";
			}
			if (factory == OracleClientFactory.Instance)
			{
				return ":";
			}
			return "?";
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000C217A File Offset: 0x000C117A
		internal static string GetParameterPrefix(DbProviderFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			if (factory == SqlClientFactory.Instance)
			{
				return "@";
			}
			return string.Empty;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000C21A0 File Offset: 0x000C11A0
		private static string[] GetParameterPrefixes()
		{
			return new string[] { "@", "?", ":" };
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000C21D0 File Offset: 0x000C11D0
		protected internal virtual Parameter[] InferParameterNames(DesignerDataConnection connection, string commandText, SqlDataSourceCommandType commandType)
		{
			Cursor cursor = Cursor.Current;
			Parameter[] array;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				if (commandText.Length == 0)
				{
					UIServiceHelper.ShowError(this.SqlDataSource.Site, SR.GetString("SqlDataSourceDesigner_NoCommand"));
					array = null;
				}
				else if (commandType == SqlDataSourceCommandType.Text)
				{
					array = SqlDataSourceParameterParser.ParseCommandText(connection.ProviderName, commandText);
				}
				else
				{
					DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(connection.ProviderName);
					DbConnection dbConnection = null;
					try
					{
						dbConnection = SqlDataSourceDesigner.GetDesignTimeConnection(base.Component.Site, connection);
					}
					catch (Exception ex)
					{
						if (dbConnection == null)
						{
							UIServiceHelper.ShowError(this.SqlDataSource.Site, ex, SR.GetString("SqlDataSourceDesigner_CouldNotCreateConnection"));
							return null;
						}
					}
					if (dbConnection == null)
					{
						UIServiceHelper.ShowError(this.SqlDataSource.Site, SR.GetString("SqlDataSourceDesigner_CouldNotCreateConnection"));
						array = null;
					}
					else
					{
						DbCommand dbCommand = this.BuildSelectCommand(dbProviderFactory, dbConnection, commandText, null, commandType);
						dbCommand.CommandType = CommandType.StoredProcedure;
						try
						{
							this.DeriveParameters(connection.ProviderName, dbCommand);
						}
						catch (Exception ex2)
						{
							UIServiceHelper.ShowError(this.SqlDataSource.Site, SR.GetString("SqlDataSourceDesigner_InferStoredProcedureError", new object[] { ex2.Message }));
							return null;
						}
						finally
						{
							if (dbCommand.Connection.State == ConnectionState.Open)
							{
								dbConnection.Close();
							}
						}
						int count = dbCommand.Parameters.Count;
						Parameter[] array2 = new Parameter[count];
						for (int i = 0; i < count; i++)
						{
							IDataParameter dataParameter = dbCommand.Parameters[i];
							if (dataParameter != null)
							{
								string text = SqlDataSourceDesigner.StripParameterPrefix(dataParameter.ParameterName);
								array2[i] = SqlDataSourceDesigner.CreateParameter(dbProviderFactory, text, dataParameter.DbType);
								array2[i].Direction = dataParameter.Direction;
							}
						}
						array = array2;
					}
				}
			}
			finally
			{
				Cursor.Current = cursor;
			}
			return array;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000C23F8 File Offset: 0x000C13F8
		internal static bool IsNewSqlServer2008Type(DbProviderFactory factory, DbType type)
		{
			return factory is SqlClientFactory && (type == DbType.Date || type == DbType.DateTime2 || type == DbType.DateTimeOffset || type == DbType.Time);
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000C241C File Offset: 0x000C141C
		internal DataTable LoadSchema()
		{
			if (!this._forceSchemaRetrieval)
			{
				object obj = base.DesignerState["DataSourceSchemaConnectionStringHash"];
				string text = base.DesignerState["DataSourceSchemaProviderName"] as string;
				string text2 = base.DesignerState["DataSourceSchemaSelectMethod"] as string;
				if (string.IsNullOrEmpty(text))
				{
					text = "System.Data.SqlClient";
				}
				if (string.IsNullOrEmpty(this.ConnectionString))
				{
					return null;
				}
				DesignerDataConnection designerDataConnection = new DesignerDataConnection(string.Empty, this.ProviderName, this.ConnectionString);
				string connectionString = designerDataConnection.ConnectionString;
				int hashCode = connectionString.GetHashCode();
				string text3 = designerDataConnection.ProviderName;
				string selectCommand = this.SelectCommand;
				if (string.IsNullOrEmpty(text3))
				{
					text3 = "System.Data.SqlClient";
				}
				if (obj == null || (int)obj != hashCode || !string.Equals(text, text3, StringComparison.OrdinalIgnoreCase) || !string.Equals(text2, selectCommand, StringComparison.Ordinal))
				{
					return null;
				}
			}
			DataTable dataTable = base.DesignerState["DataSourceSchema"] as DataTable;
			if (dataTable != null)
			{
				dataTable.TableName = "DefaultView";
				return dataTable;
			}
			return null;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000C2528 File Offset: 0x000C1528
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor;
			foreach (string text in SqlDataSourceDesigner._hiddenProperties)
			{
				propertyDescriptor = (PropertyDescriptor)properties[text];
				if (propertyDescriptor != null)
				{
					properties[text] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
				}
			}
			properties["DeleteQuery"] = TypeDescriptor.CreateProperty(base.GetType(), "DeleteQuery", typeof(DataSourceOperation), new Attribute[0]);
			properties["InsertQuery"] = TypeDescriptor.CreateProperty(base.GetType(), "InsertQuery", typeof(DataSourceOperation), new Attribute[0]);
			properties["SelectQuery"] = TypeDescriptor.CreateProperty(base.GetType(), "SelectQuery", typeof(DataSourceOperation), new Attribute[0]);
			properties["UpdateQuery"] = TypeDescriptor.CreateProperty(base.GetType(), "UpdateQuery", typeof(DataSourceOperation), new Attribute[0]);
			propertyDescriptor = (PropertyDescriptor)properties["ConnectionString"];
			properties["ConnectionString"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
			propertyDescriptor = (PropertyDescriptor)properties["ProviderName"];
			properties["ProviderName"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
			propertyDescriptor = (PropertyDescriptor)properties["SelectCommand"];
			properties["SelectCommand"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000C26CD File Offset: 0x000C16CD
		private static bool ProviderRequiresDbTypeSet(DbProviderFactory factory)
		{
			return factory == OleDbFactory.Instance || factory == OdbcFactory.Instance;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000C26E4 File Offset: 0x000C16E4
		public override void RefreshSchema(bool preferSilent)
		{
			try
			{
				this.SuppressDataSourceEvents();
				IServiceProvider site = this.SqlDataSource.Site;
				if (!this.CanRefreshSchema)
				{
					if (!preferSilent)
					{
						UIServiceHelper.ShowError(site, SR.GetString("SqlDataSourceDesigner_RefreshSchemaRequiresSettings"));
					}
				}
				else
				{
					IDataSourceViewSchema dataSourceViewSchema = this.GetView("DefaultView").Schema;
					bool flag = false;
					if (dataSourceViewSchema == null)
					{
						this._forceSchemaRetrieval = true;
						dataSourceViewSchema = this.GetView("DefaultView").Schema;
						this._forceSchemaRetrieval = false;
						flag = true;
					}
					DesignerDataConnection designerDataConnection = new DesignerDataConnection(string.Empty, this.ProviderName, this.ConnectionString);
					bool flag2;
					if (preferSilent)
					{
						flag2 = this.RefreshSchema(designerDataConnection, this.SelectCommand, this.SqlDataSource.SelectCommandType, this.SqlDataSource.SelectParameters, true);
					}
					else
					{
						Parameter[] array = this.InferParameterNames(designerDataConnection, this.SelectCommand, this.SqlDataSource.SelectCommandType);
						if (array == null)
						{
							return;
						}
						ParameterCollection parameterCollection = new ParameterCollection();
						ParameterCollection parameterCollection2 = new ParameterCollection();
						foreach (object obj in this.SqlDataSource.SelectParameters)
						{
							ICloneable cloneable = (ICloneable)obj;
							parameterCollection2.Add((Parameter)cloneable.Clone());
						}
						foreach (Parameter parameter in array)
						{
							if (parameter.Direction == ParameterDirection.Input || parameter.Direction == ParameterDirection.InputOutput)
							{
								Parameter parameter2 = parameterCollection2[parameter.Name];
								if (parameter2 != null)
								{
									parameter.DefaultValue = parameter2.DefaultValue;
									if (parameter.DbType == DbType.Object && parameter.Type == TypeCode.Empty)
									{
										parameter.DbType = parameter2.DbType;
										parameter.Type = parameter2.Type;
									}
									parameterCollection2.Remove(parameter2);
								}
								parameterCollection.Add(parameter);
							}
						}
						if (parameterCollection.Count > 0)
						{
							SqlDataSourceRefreshSchemaForm sqlDataSourceRefreshSchemaForm = new SqlDataSourceRefreshSchemaForm(site, this, parameterCollection);
							DialogResult dialogResult = UIServiceHelper.ShowDialog(site, sqlDataSourceRefreshSchemaForm);
							flag2 = dialogResult == DialogResult.OK;
						}
						else
						{
							flag2 = this.RefreshSchema(designerDataConnection, this.SelectCommand, this.SqlDataSource.SelectCommandType, parameterCollection, false);
						}
					}
					if (flag2)
					{
						IDataSourceViewSchema schema = this.GetView("DefaultView").Schema;
						if (flag && DataSourceDesigner.ViewSchemasEquivalent(dataSourceViewSchema, schema))
						{
							this.OnDataSourceChanged(EventArgs.Empty);
						}
						else if (!DataSourceDesigner.ViewSchemasEquivalent(dataSourceViewSchema, schema))
						{
							this.OnSchemaRefreshed(EventArgs.Empty);
						}
					}
				}
			}
			finally
			{
				this.ResumeDataSourceEvents();
			}
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000C298C File Offset: 0x000C198C
		internal bool RefreshSchema(DesignerDataConnection connection, string commandText, SqlDataSourceCommandType commandType, ParameterCollection parameters, bool preferSilent)
		{
			IServiceProvider site = this.SqlDataSource.Site;
			DbCommand dbCommand = null;
			try
			{
				DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(connection.ProviderName);
				DbConnection designTimeConnection = SqlDataSourceDesigner.GetDesignTimeConnection(base.Component.Site, connection);
				if (designTimeConnection == null)
				{
					if (!preferSilent)
					{
						UIServiceHelper.ShowError(this.SqlDataSource.Site, SR.GetString("SqlDataSourceDesigner_CouldNotCreateConnection"));
					}
					return false;
				}
				dbCommand = this.BuildSelectCommand(dbProviderFactory, designTimeConnection, commandText, parameters, commandType);
				DbDataAdapter dbDataAdapter = SqlDataSourceDesigner.CreateDataAdapter(dbProviderFactory, dbCommand);
				dbDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				DataSet dataSet = new DataSet();
				dbDataAdapter.FillSchema(dataSet, SchemaType.Source, "DefaultView");
				DataTable dataTable = dataSet.Tables["DefaultView"];
				if (dataTable == null)
				{
					if (!preferSilent)
					{
						UIServiceHelper.ShowError(site, SR.GetString("SqlDataSourceDesigner_CannotGetSchema"));
					}
					return false;
				}
				this.SaveSchema(connection, commandText, dataTable);
				return true;
			}
			catch (Exception ex)
			{
				if (!preferSilent)
				{
					UIServiceHelper.ShowError(site, ex, SR.GetString("SqlDataSourceDesigner_CannotGetSchema"));
				}
			}
			finally
			{
				if (dbCommand != null && dbCommand.Connection.State == ConnectionState.Open)
				{
					dbCommand.Connection.Close();
				}
			}
			return false;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000C2ABC File Offset: 0x000C1ABC
		private void SaveSchema(DesignerDataConnection connection, string selectCommand, DataTable schemaTable)
		{
			base.DesignerState["DataSourceSchema"] = schemaTable;
			base.DesignerState["DataSourceSchemaConnectionStringHash"] = connection.ConnectionString.GetHashCode();
			base.DesignerState["DataSourceSchemaProviderName"] = connection.ProviderName;
			base.DesignerState["DataSourceSchemaSelectMethod"] = selectCommand;
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000C2B24 File Offset: 0x000C1B24
		internal static string StripParameterPrefix(string parameterName)
		{
			foreach (string text in SqlDataSourceDesigner.GetParameterPrefixes())
			{
				if (parameterName.StartsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					return parameterName.Substring(text.Length);
				}
			}
			return parameterName;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000C2B65 File Offset: 0x000C1B65
		internal static bool SupportsNamedParameters(DbProviderFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			return factory == SqlClientFactory.Instance || factory == OracleClientFactory.Instance;
		}

		// Token: 0x040018E5 RID: 6373
		internal const string AspNetDatabaseObjectPrefix = "AspNet_";

		// Token: 0x040018E6 RID: 6374
		internal const string DefaultProviderName = "System.Data.SqlClient";

		// Token: 0x040018E7 RID: 6375
		internal const string DefaultViewName = "DefaultView";

		// Token: 0x040018E8 RID: 6376
		private const string DesignerStateDataSourceSchemaKey = "DataSourceSchema";

		// Token: 0x040018E9 RID: 6377
		private const string DesignerStateDataSourceSchemaConnectionStringHashKey = "DataSourceSchemaConnectionStringHash";

		// Token: 0x040018EA RID: 6378
		private const string DesignerStateDataSourceSchemaProviderNameKey = "DataSourceSchemaProviderName";

		// Token: 0x040018EB RID: 6379
		private const string DesignerStateDataSourceSchemaSelectCommandKey = "DataSourceSchemaSelectMethod";

		// Token: 0x040018EC RID: 6380
		private const string DesignerStateTableQueryStateKey = "TableQueryState";

		// Token: 0x040018ED RID: 6381
		private const string DesignerStateSaveConfiguredConnectionStateKey = "SaveConfiguredConnectionState";

		// Token: 0x040018EE RID: 6382
		private static readonly string[] _hiddenProperties = new string[] { "DeleteCommand", "DeleteParameters", "InsertCommand", "InsertParameters", "SelectParameters", "UpdateCommand", "UpdateParameters" };

		// Token: 0x040018EF RID: 6383
		private DesignerDataSourceView _view;

		// Token: 0x040018F0 RID: 6384
		private bool _forceSchemaRetrieval;
	}
}
