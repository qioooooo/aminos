using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ObjectDataSourceDesigner : DataSourceDesigner
	{
		internal Type SelectMethodReturnType
		{
			get
			{
				if (this._selectMethodReturnType == null)
				{
					string text = base.DesignerState["DataSourceSchemaSelectMethodReturnTypeName"] as string;
					if (!string.IsNullOrEmpty(text))
					{
						this._selectMethodReturnType = ObjectDataSourceDesigner.GetType(base.Component.Site, text, true);
					}
				}
				return this._selectMethodReturnType;
			}
		}

		public override bool CanConfigure
		{
			get
			{
				return this.TypeServiceAvailable;
			}
		}

		public override bool CanRefreshSchema
		{
			get
			{
				return !string.IsNullOrEmpty(this.TypeName) && !string.IsNullOrEmpty(this.SelectMethod) && this.TypeServiceAvailable;
			}
		}

		internal object ShowOnlyDataComponentsState
		{
			get
			{
				return base.DesignerState["ShowOnlyDataComponentsState"];
			}
			set
			{
				base.DesignerState["ShowOnlyDataComponentsState"] = value;
			}
		}

		private bool TypeServiceAvailable
		{
			get
			{
				IServiceProvider site = base.Component.Site;
				if (site == null)
				{
					return false;
				}
				ITypeResolutionService typeResolutionService = (ITypeResolutionService)site.GetService(typeof(ITypeResolutionService));
				ITypeDiscoveryService typeDiscoveryService = (ITypeDiscoveryService)site.GetService(typeof(ITypeDiscoveryService));
				return typeResolutionService != null || typeDiscoveryService != null;
			}
		}

		internal ObjectDataSource ObjectDataSource
		{
			get
			{
				return (ObjectDataSource)base.Component;
			}
		}

		public string SelectMethod
		{
			get
			{
				return this.ObjectDataSource.SelectMethod;
			}
			set
			{
				if (value != this.SelectMethod)
				{
					this.ObjectDataSource.SelectMethod = value;
					this.UpdateDesignTimeHtml();
					if (this.CanRefreshSchema && !this._inWizard)
					{
						this.RefreshSchema(true);
						return;
					}
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		public string TypeName
		{
			get
			{
				return this.ObjectDataSource.TypeName;
			}
			set
			{
				if (value != this.TypeName)
				{
					this.ObjectDataSource.TypeName = value;
					this.UpdateDesignTimeHtml();
					if (this.CanRefreshSchema)
					{
						this.RefreshSchema(true);
						return;
					}
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		public override void Configure()
		{
			this._inWizard = true;
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConfigureDataSourceChangeCallback), null, SR.GetString("DataSource_ConfigureTransactionDescription"));
			this._inWizard = false;
		}

		private bool ConfigureDataSourceChangeCallback(object context)
		{
			bool flag;
			try
			{
				this.SuppressDataSourceEvents();
				IServiceProvider site = base.Component.Site;
				ObjectDataSourceWizardForm objectDataSourceWizardForm = new ObjectDataSourceWizardForm(site, this);
				DialogResult dialogResult = UIServiceHelper.ShowDialog(site, objectDataSourceWizardForm);
				if (dialogResult == DialogResult.OK)
				{
					this.OnDataSourceChanged(EventArgs.Empty);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			finally
			{
				this.ResumeDataSourceEvents();
			}
			return flag;
		}

		private static DataTable[] ConvertSchemaToDataTables(TypeSchema schema)
		{
			if (schema == null)
			{
				return null;
			}
			IDataSourceViewSchema[] views = schema.GetViews();
			if (views == null)
			{
				return null;
			}
			DataTable[] array = new DataTable[views.Length];
			for (int i = 0; i < views.Length; i++)
			{
				IDataSourceViewSchema dataSourceViewSchema = views[i];
				array[i] = new DataTable(dataSourceViewSchema.Name);
				IDataSourceFieldSchema[] fields = dataSourceViewSchema.GetFields();
				if (fields != null)
				{
					List<DataColumn> list = new List<DataColumn>();
					foreach (IDataSourceFieldSchema dataSourceFieldSchema in fields)
					{
						DataColumn dataColumn = new DataColumn();
						dataColumn.AllowDBNull = dataSourceFieldSchema.Nullable;
						dataColumn.AutoIncrement = dataSourceFieldSchema.Identity;
						dataColumn.ColumnName = dataSourceFieldSchema.Name;
						dataColumn.DataType = dataSourceFieldSchema.DataType;
						if (dataColumn.DataType == typeof(string))
						{
							dataColumn.MaxLength = dataSourceFieldSchema.Length;
						}
						dataColumn.ReadOnly = dataSourceFieldSchema.IsReadOnly;
						dataColumn.Unique = dataSourceFieldSchema.IsUnique;
						array[i].Columns.Add(dataColumn);
						if (dataSourceFieldSchema.PrimaryKey)
						{
							list.Add(dataColumn);
						}
					}
					if (list.Count > 0)
					{
						array[i].PrimaryKey = list.ToArray();
					}
				}
			}
			return array;
		}

		private static Parameter CreateMergedParameter(ParameterInfo methodParameter, Parameter[] parameters)
		{
			foreach (Parameter parameter in parameters)
			{
				if (ObjectDataSourceDesigner.ParametersMatch(methodParameter, parameter))
				{
					return parameter;
				}
			}
			Parameter parameter2 = new Parameter(methodParameter.Name);
			if (methodParameter.IsOut)
			{
				parameter2.Direction = ParameterDirection.Output;
			}
			else if (methodParameter.ParameterType.IsByRef)
			{
				parameter2.Direction = ParameterDirection.InputOutput;
			}
			else
			{
				parameter2.Direction = ParameterDirection.Input;
			}
			ObjectDataSourceDesigner.SetParameterType(parameter2, methodParameter.ParameterType);
			return parameter2;
		}

		internal static Type GetType(IServiceProvider serviceProvider, string typeName, bool silent)
		{
			ITypeResolutionService typeResolutionService = null;
			if (serviceProvider != null)
			{
				typeResolutionService = (ITypeResolutionService)serviceProvider.GetService(typeof(ITypeResolutionService));
			}
			if (typeResolutionService == null)
			{
				return null;
			}
			Type type;
			try
			{
				type = typeResolutionService.GetType(typeName, true, true);
			}
			catch (Exception ex)
			{
				if (!silent)
				{
					UIServiceHelper.ShowError(serviceProvider, ex, SR.GetString("ObjectDataSourceDesigner_CannotGetType", new object[] { typeName }));
				}
				type = null;
			}
			return type;
		}

		private static Type RemoveNullableFromType(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments()[0];
			}
			else if (type.IsByRef)
			{
				type = type.GetElementType();
			}
			return type;
		}

		private static DbType GetDbTypeForType(Type type)
		{
			type = ObjectDataSourceDesigner.RemoveNullableFromType(type);
			if (typeof(DateTimeOffset).IsAssignableFrom(type))
			{
				return DbType.DateTimeOffset;
			}
			if (typeof(TimeSpan).IsAssignableFrom(type))
			{
				return DbType.Time;
			}
			if (typeof(Guid).IsAssignableFrom(type))
			{
				return DbType.Guid;
			}
			return DbType.Object;
		}

		private static TypeCode GetTypeCodeForType(Type type)
		{
			type = ObjectDataSourceDesigner.RemoveNullableFromType(type);
			if (typeof(bool).IsAssignableFrom(type))
			{
				return TypeCode.Boolean;
			}
			if (typeof(byte).IsAssignableFrom(type))
			{
				return TypeCode.Byte;
			}
			if (typeof(char).IsAssignableFrom(type))
			{
				return TypeCode.Char;
			}
			if (typeof(DateTime).IsAssignableFrom(type))
			{
				return TypeCode.DateTime;
			}
			if (typeof(DBNull).IsAssignableFrom(type))
			{
				return TypeCode.DBNull;
			}
			if (typeof(decimal).IsAssignableFrom(type))
			{
				return TypeCode.Decimal;
			}
			if (typeof(double).IsAssignableFrom(type))
			{
				return TypeCode.Double;
			}
			if (typeof(short).IsAssignableFrom(type))
			{
				return TypeCode.Int16;
			}
			if (typeof(int).IsAssignableFrom(type))
			{
				return TypeCode.Int32;
			}
			if (typeof(long).IsAssignableFrom(type))
			{
				return TypeCode.Int64;
			}
			if (typeof(sbyte).IsAssignableFrom(type))
			{
				return TypeCode.SByte;
			}
			if (typeof(float).IsAssignableFrom(type))
			{
				return TypeCode.Single;
			}
			if (typeof(string).IsAssignableFrom(type))
			{
				return TypeCode.String;
			}
			if (typeof(ushort).IsAssignableFrom(type))
			{
				return TypeCode.UInt16;
			}
			if (typeof(uint).IsAssignableFrom(type))
			{
				return TypeCode.UInt32;
			}
			if (typeof(ulong).IsAssignableFrom(type))
			{
				return TypeCode.UInt64;
			}
			return TypeCode.Object;
		}

		public override DesignerDataSourceView GetView(string viewName)
		{
			string[] viewNames = this.GetViewNames();
			if (viewNames != null && viewNames.Length > 0)
			{
				if (string.IsNullOrEmpty(viewName))
				{
					viewName = viewNames[0];
				}
				foreach (string text in viewNames)
				{
					if (string.Equals(viewName, text, StringComparison.OrdinalIgnoreCase))
					{
						return new ObjectDesignerDataSourceView(this, viewName);
					}
				}
				return null;
			}
			return new ObjectDesignerDataSourceView(this, string.Empty);
		}

		public override string[] GetViewNames()
		{
			List<string> list = new List<string>();
			DataTable[] array = this.LoadSchema();
			if (array != null && array.Length > 0)
			{
				foreach (DataTable dataTable in array)
				{
					list.Add(dataTable.TableName);
				}
			}
			return list.ToArray();
		}

		internal static bool IsMatchingMethod(MethodInfo method, string methodName, ParameterCollection parameters, Type dataObjectType)
		{
			if (!string.Equals(methodName, method.Name, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			ParameterInfo[] parameters2 = method.GetParameters();
			if (dataObjectType != null && ((parameters2.Length == 1 && parameters2[0].ParameterType == dataObjectType) || (parameters2.Length == 2 && parameters2[0].ParameterType == dataObjectType && parameters2[1].ParameterType == dataObjectType)))
			{
				return true;
			}
			if (parameters2.Length != parameters.Count)
			{
				return false;
			}
			Hashtable hashtable = new Hashtable(StringComparer.Create(CultureInfo.InvariantCulture, true));
			foreach (object obj in parameters)
			{
				Parameter parameter = (Parameter)obj;
				if (!hashtable.Contains(parameter.Name))
				{
					hashtable.Add(parameter.Name, null);
				}
			}
			foreach (ParameterInfo parameterInfo in parameters2)
			{
				if (!hashtable.Contains(parameterInfo.Name))
				{
					return false;
				}
			}
			return true;
		}

		internal DataTable[] LoadSchema()
		{
			if (!this._forceSchemaRetrieval)
			{
				string text = base.DesignerState["DataSourceSchemaTypeName"] as string;
				string text2 = base.DesignerState["DataSourceSchemaSelectMethod"] as string;
				if (!string.Equals(text, this.TypeName, StringComparison.OrdinalIgnoreCase) || !string.Equals(text2, this.SelectMethod, StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
			}
			DataTable[] array = null;
			Pair pair = base.DesignerState["DataSourceSchema"] as Pair;
			if (pair != null)
			{
				string[] array2 = pair.First as string[];
				DataTable[] array3 = pair.Second as DataTable[];
				if (array2 != null && array3 != null)
				{
					int num = array2.Length;
					array = new DataTable[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = array3[i].Clone();
						array[i].TableName = array2[i];
					}
				}
			}
			return array;
		}

		internal static Parameter[] MergeParameters(Parameter[] parameters, MethodInfo methodInfo)
		{
			ParameterInfo[] parameters2 = methodInfo.GetParameters();
			Parameter[] array = new Parameter[parameters2.Length];
			for (int i = 0; i < parameters2.Length; i++)
			{
				ParameterInfo parameterInfo = parameters2[i];
				array[i] = ObjectDataSourceDesigner.CreateMergedParameter(parameterInfo, parameters);
			}
			return array;
		}

		internal static void MergeParameters(ParameterCollection parameters, MethodInfo methodInfo, Type dataObjectType)
		{
			Parameter[] array = new Parameter[parameters.Count];
			parameters.CopyTo(array, 0);
			parameters.Clear();
			if (methodInfo == null)
			{
				return;
			}
			if (dataObjectType == null)
			{
				ParameterInfo[] parameters2 = methodInfo.GetParameters();
				foreach (ParameterInfo parameterInfo in parameters2)
				{
					Parameter parameter = ObjectDataSourceDesigner.CreateMergedParameter(parameterInfo, array);
					if (parameters[parameter.Name] == null)
					{
						parameters.Add(parameter);
					}
				}
			}
		}

		private static bool ParametersMatch(ParameterInfo methodParameter, Parameter parameter)
		{
			if (!string.Equals(methodParameter.Name, parameter.Name, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			switch (parameter.Direction)
			{
			case ParameterDirection.Input:
				if (methodParameter.IsOut || methodParameter.ParameterType.IsByRef)
				{
					return false;
				}
				break;
			case ParameterDirection.Output:
				if (!methodParameter.IsOut)
				{
					return false;
				}
				break;
			case ParameterDirection.InputOutput:
				if (!methodParameter.ParameterType.IsByRef)
				{
					return false;
				}
				break;
			case ParameterDirection.ReturnValue:
				return false;
			}
			DbType dbTypeForType = ObjectDataSourceDesigner.GetDbTypeForType(methodParameter.ParameterType);
			if (dbTypeForType != DbType.Object)
			{
				return dbTypeForType == parameter.DbType;
			}
			TypeCode typeCodeForType = ObjectDataSourceDesigner.GetTypeCodeForType(methodParameter.ParameterType);
			return ((typeCodeForType == TypeCode.Object || typeCodeForType == TypeCode.Empty) && (parameter.Type == TypeCode.Object || parameter.Type == TypeCode.Empty)) || typeCodeForType == parameter.Type;
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["TypeName"];
			properties["TypeName"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
			propertyDescriptor = (PropertyDescriptor)properties["SelectMethod"];
			properties["SelectMethod"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
		}

		public override void RefreshSchema(bool preferSilent)
		{
			try
			{
				this.SuppressDataSourceEvents();
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					Type type = ObjectDataSourceDesigner.GetType(base.Component.Site, this.TypeName, preferSilent);
					if (type != null)
					{
						MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						MethodInfo methodInfo = null;
						MethodInfo methodInfo2 = null;
						bool flag = false;
						Type type2 = null;
						if (!string.IsNullOrEmpty(this.ObjectDataSource.DataObjectTypeName))
						{
							type2 = ObjectDataSourceDesigner.GetType(base.Component.Site, this.ObjectDataSource.DataObjectTypeName, preferSilent);
						}
						foreach (MethodInfo methodInfo3 in methods)
						{
							if (string.Equals(methodInfo3.Name, this.SelectMethod, StringComparison.OrdinalIgnoreCase))
							{
								if (methodInfo2 != null && methodInfo2.ReturnType != methodInfo3.ReturnType)
								{
									flag = true;
								}
								else
								{
									methodInfo2 = methodInfo3;
								}
								if (ObjectDataSourceDesigner.IsMatchingMethod(methodInfo3, this.SelectMethod, this.ObjectDataSource.SelectParameters, type2))
								{
									methodInfo = methodInfo3;
									break;
								}
							}
						}
						if (methodInfo == null && methodInfo2 != null && !flag)
						{
							methodInfo = methodInfo2;
						}
						if (methodInfo != null)
						{
							this.RefreshSchema(methodInfo.ReflectedType, methodInfo.Name, methodInfo.ReturnType, preferSilent);
						}
					}
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
			finally
			{
				this.ResumeDataSourceEvents();
			}
		}

		internal void RefreshSchema(Type objectType, string methodName, Type schemaType, bool preferSilent)
		{
			if (objectType != null && !string.IsNullOrEmpty(methodName) && schemaType != null)
			{
				try
				{
					TypeSchema typeSchema = new TypeSchema(schemaType);
					this._forceSchemaRetrieval = true;
					DataTable[] array = this.LoadSchema();
					this._forceSchemaRetrieval = false;
					IDataSourceSchema dataSourceSchema = ((array == null) ? null : new ObjectDataSourceDesigner.DataTableArraySchema(array));
					this.SaveSchema(objectType, methodName, ObjectDataSourceDesigner.ConvertSchemaToDataTables(typeSchema), schemaType);
					DataTable[] array2 = this.LoadSchema();
					IDataSourceSchema dataSourceSchema2 = ((array2 == null) ? null : new ObjectDataSourceDesigner.DataTableArraySchema(array2));
					if (!DataSourceDesigner.SchemasEquivalent(dataSourceSchema, dataSourceSchema2))
					{
						this.OnSchemaRefreshed(EventArgs.Empty);
					}
				}
				catch (Exception ex)
				{
					if (!preferSilent)
					{
						UIServiceHelper.ShowError(base.Component.Site, ex, SR.GetString("ObjectDataSourceDesigner_CannotGetSchema", new object[] { schemaType.FullName }));
					}
				}
			}
		}

		private void SaveSchema(Type objectType, string methodName, DataTable[] schemaTables, Type schemaType)
		{
			Pair pair = null;
			if (schemaTables != null)
			{
				int num = schemaTables.Length;
				string[] array = new string[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = schemaTables[i].TableName;
					schemaTables[i].TableName = "Table" + i.ToString(CultureInfo.InvariantCulture);
				}
				pair = new Pair(array, schemaTables);
			}
			base.DesignerState["DataSourceSchema"] = pair;
			base.DesignerState["DataSourceSchemaTypeName"] = ((objectType == null) ? string.Empty : objectType.FullName);
			base.DesignerState["DataSourceSchemaSelectMethod"] = methodName;
			string text = base.DesignerState["DataSourceSchemaSelectMethodReturnTypeName"] as string;
			if (!string.Equals(text, schemaType.FullName, StringComparison.OrdinalIgnoreCase))
			{
				base.DesignerState["DataSourceSchemaSelectMethodReturnTypeName"] = schemaType.FullName;
				this._selectMethodReturnType = schemaType;
			}
		}

		internal static void SetParameterType(Parameter parameter, Type type)
		{
			parameter.DbType = ObjectDataSourceDesigner.GetDbTypeForType(type);
			if (parameter.DbType == DbType.Object)
			{
				parameter.Type = ObjectDataSourceDesigner.GetTypeCodeForType(type);
				return;
			}
			parameter.Type = TypeCode.Empty;
		}

		internal const BindingFlags MethodFilter = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		private const string DesignerStateDataSourceSchemaKey = "DataSourceSchema";

		private const string DesignerStateDataSourceSchemaTypeNameKey = "DataSourceSchemaTypeName";

		private const string DesignerStateDataSourceSchemaSelectMethodKey = "DataSourceSchemaSelectMethod";

		private const string DesignerStateDataSourceSchemaSelectMethodReturnTypeNameKey = "DataSourceSchemaSelectMethodReturnTypeName";

		private const string DesignerStateShowOnlyDataComponentsStateKey = "ShowOnlyDataComponentsState";

		private bool _inWizard;

		private Type _selectMethodReturnType;

		private bool _forceSchemaRetrieval;

		private sealed class DataTableArraySchema : IDataSourceSchema
		{
			public DataTableArraySchema(DataTable[] tables)
			{
				this._tables = tables;
			}

			public IDataSourceViewSchema[] GetViews()
			{
				DataSetViewSchema[] array = new DataSetViewSchema[this._tables.Length];
				for (int i = 0; i < this._tables.Length; i++)
				{
					array[i] = new DataSetViewSchema(this._tables[i]);
				}
				return array;
			}

			private DataTable[] _tables;
		}
	}
}
