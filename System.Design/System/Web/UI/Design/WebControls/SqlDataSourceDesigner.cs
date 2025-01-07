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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SqlDataSourceDesigner : DataSourceDesigner
	{
		public override bool CanConfigure
		{
			get
			{
				IDataEnvironment dataEnvironment = (IDataEnvironment)base.Component.Site.GetService(typeof(IDataEnvironment));
				return dataEnvironment != null;
			}
		}

		public override bool CanRefreshSchema
		{
			get
			{
				string connectionString = this.ConnectionString;
				return connectionString != null && connectionString.Trim().Length != 0 && this.SelectCommand.Trim().Length != 0;
			}
		}

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

		internal SqlDataSource SqlDataSource
		{
			get
			{
				return (SqlDataSource)base.Component;
			}
		}

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

		internal static TypeCode ConvertDbTypeToTypeCode(DbType dbType)
		{
			return Parameter.ConvertDbTypeToTypeCode(dbType);
		}

		internal static DbType ConvertTypeCodeToDbType(TypeCode typeCode)
		{
			return Parameter.ConvertTypeCodeToDbType(typeCode);
		}

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

		internal virtual SqlDataSourceWizardForm CreateConfigureDataSourceWizardForm(IServiceProvider serviceProvider, IDataEnvironment dataEnvironment)
		{
			return new SqlDataSourceWizardForm(serviceProvider, this, dataEnvironment);
		}

		internal static DbCommand CreateCommand(DbProviderFactory factory, string commandText, DbConnection connection)
		{
			DbCommand dbCommand = factory.CreateCommand();
			dbCommand.CommandText = commandText;
			dbCommand.Connection = connection;
			return dbCommand;
		}

		internal static DbDataAdapter CreateDataAdapter(DbProviderFactory factory, DbCommand command)
		{
			DbDataAdapter dbDataAdapter = factory.CreateDataAdapter();
			((IDbDataAdapter)dbDataAdapter).SelectCommand = command;
			return dbDataAdapter;
		}

		internal static DbParameter CreateParameter(DbProviderFactory factory)
		{
			return factory.CreateParameter();
		}

		internal static Parameter CreateParameter(DbProviderFactory factory, string name, DbType dbType)
		{
			if (SqlDataSourceDesigner.IsNewSqlServer2008Type(factory, dbType))
			{
				return new Parameter(name, dbType);
			}
			return new Parameter(name, SqlDataSourceDesigner.ConvertDbTypeToTypeCode(dbType));
		}

		protected virtual SqlDesignerDataSourceView CreateView(string viewName)
		{
			return new SqlDesignerDataSourceView(this, viewName);
		}

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

		private static CommandType GetCommandType(SqlDataSourceCommandType commandType)
		{
			if (commandType == SqlDataSourceCommandType.Text)
			{
				return CommandType.Text;
			}
			return CommandType.StoredProcedure;
		}

		protected virtual string GetConnectionString()
		{
			return this.SqlDataSource.ConnectionString;
		}

		internal static DbProviderFactory GetDbProviderFactory(string providerName)
		{
			if (providerName.Length == 0)
			{
				providerName = "System.Data.SqlClient";
			}
			return DbProviderFactories.GetFactory(providerName);
		}

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

		public override string[] GetViewNames()
		{
			return new string[] { "DefaultView" };
		}

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

		private static string[] GetParameterPrefixes()
		{
			return new string[] { "@", "?", ":" };
		}

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

		internal static bool IsNewSqlServer2008Type(DbProviderFactory factory, DbType type)
		{
			return factory is SqlClientFactory && (type == DbType.Date || type == DbType.DateTime2 || type == DbType.DateTimeOffset || type == DbType.Time);
		}

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

		private static bool ProviderRequiresDbTypeSet(DbProviderFactory factory)
		{
			return factory == OleDbFactory.Instance || factory == OdbcFactory.Instance;
		}

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

		private void SaveSchema(DesignerDataConnection connection, string selectCommand, DataTable schemaTable)
		{
			base.DesignerState["DataSourceSchema"] = schemaTable;
			base.DesignerState["DataSourceSchemaConnectionStringHash"] = connection.ConnectionString.GetHashCode();
			base.DesignerState["DataSourceSchemaProviderName"] = connection.ProviderName;
			base.DesignerState["DataSourceSchemaSelectMethod"] = selectCommand;
		}

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

		internal static bool SupportsNamedParameters(DbProviderFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			return factory == SqlClientFactory.Instance || factory == OracleClientFactory.Instance;
		}

		internal const string AspNetDatabaseObjectPrefix = "AspNet_";

		internal const string DefaultProviderName = "System.Data.SqlClient";

		internal const string DefaultViewName = "DefaultView";

		private const string DesignerStateDataSourceSchemaKey = "DataSourceSchema";

		private const string DesignerStateDataSourceSchemaConnectionStringHashKey = "DataSourceSchemaConnectionStringHash";

		private const string DesignerStateDataSourceSchemaProviderNameKey = "DataSourceSchemaProviderName";

		private const string DesignerStateDataSourceSchemaSelectCommandKey = "DataSourceSchemaSelectMethod";

		private const string DesignerStateTableQueryStateKey = "TableQueryState";

		private const string DesignerStateSaveConfiguredConnectionStateKey = "SaveConfiguredConnectionState";

		private static readonly string[] _hiddenProperties = new string[] { "DeleteCommand", "DeleteParameters", "InsertCommand", "InsertParameters", "SelectParameters", "UpdateCommand", "UpdateParameters" };

		private DesignerDataSourceView _view;

		private bool _forceSchemaRetrieval;
	}
}
