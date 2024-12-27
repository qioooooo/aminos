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
	// Token: 0x02000486 RID: 1158
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ObjectDataSourceDesigner : DataSourceDesigner
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x000E6EEC File Offset: 0x000E5EEC
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

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06002A06 RID: 10758 RVA: 0x000E6F3D File Offset: 0x000E5F3D
		public override bool CanConfigure
		{
			get
			{
				return this.TypeServiceAvailable;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002A07 RID: 10759 RVA: 0x000E6F45 File Offset: 0x000E5F45
		public override bool CanRefreshSchema
		{
			get
			{
				return !string.IsNullOrEmpty(this.TypeName) && !string.IsNullOrEmpty(this.SelectMethod) && this.TypeServiceAvailable;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002A08 RID: 10760 RVA: 0x000E6F69 File Offset: 0x000E5F69
		// (set) Token: 0x06002A09 RID: 10761 RVA: 0x000E6F7B File Offset: 0x000E5F7B
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

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002A0A RID: 10762 RVA: 0x000E6F90 File Offset: 0x000E5F90
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

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002A0B RID: 10763 RVA: 0x000E6FE6 File Offset: 0x000E5FE6
		internal ObjectDataSource ObjectDataSource
		{
			get
			{
				return (ObjectDataSource)base.Component;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002A0C RID: 10764 RVA: 0x000E6FF3 File Offset: 0x000E5FF3
		// (set) Token: 0x06002A0D RID: 10765 RVA: 0x000E7000 File Offset: 0x000E6000
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

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002A0E RID: 10766 RVA: 0x000E7050 File Offset: 0x000E6050
		// (set) Token: 0x06002A0F RID: 10767 RVA: 0x000E705D File Offset: 0x000E605D
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

		// Token: 0x06002A10 RID: 10768 RVA: 0x000E709A File Offset: 0x000E609A
		public override void Configure()
		{
			this._inWizard = true;
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConfigureDataSourceChangeCallback), null, SR.GetString("DataSource_ConfigureTransactionDescription"));
			this._inWizard = false;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000E70CC File Offset: 0x000E60CC
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

		// Token: 0x06002A12 RID: 10770 RVA: 0x000E712C File Offset: 0x000E612C
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

		// Token: 0x06002A13 RID: 10771 RVA: 0x000E7268 File Offset: 0x000E6268
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

		// Token: 0x06002A14 RID: 10772 RVA: 0x000E72E4 File Offset: 0x000E62E4
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

		// Token: 0x06002A15 RID: 10773 RVA: 0x000E7354 File Offset: 0x000E6354
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

		// Token: 0x06002A16 RID: 10774 RVA: 0x000E7390 File Offset: 0x000E6390
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

		// Token: 0x06002A17 RID: 10775 RVA: 0x000E73E8 File Offset: 0x000E63E8
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

		// Token: 0x06002A18 RID: 10776 RVA: 0x000E7548 File Offset: 0x000E6548
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

		// Token: 0x06002A19 RID: 10777 RVA: 0x000E75B0 File Offset: 0x000E65B0
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

		// Token: 0x06002A1A RID: 10778 RVA: 0x000E7600 File Offset: 0x000E6600
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

		// Token: 0x06002A1B RID: 10779 RVA: 0x000E770C File Offset: 0x000E670C
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

		// Token: 0x06002A1C RID: 10780 RVA: 0x000E77E8 File Offset: 0x000E67E8
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

		// Token: 0x06002A1D RID: 10781 RVA: 0x000E7824 File Offset: 0x000E6824
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

		// Token: 0x06002A1E RID: 10782 RVA: 0x000E7894 File Offset: 0x000E6894
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

		// Token: 0x06002A1F RID: 10783 RVA: 0x000E795C File Offset: 0x000E695C
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["TypeName"];
			properties["TypeName"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
			propertyDescriptor = (PropertyDescriptor)properties["SelectMethod"];
			properties["SelectMethod"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000E79CC File Offset: 0x000E69CC
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

		// Token: 0x06002A21 RID: 10785 RVA: 0x000E7B34 File Offset: 0x000E6B34
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

		// Token: 0x06002A22 RID: 10786 RVA: 0x000E7C04 File Offset: 0x000E6C04
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

		// Token: 0x06002A23 RID: 10787 RVA: 0x000E7CE7 File Offset: 0x000E6CE7
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

		// Token: 0x04001CC7 RID: 7367
		internal const BindingFlags MethodFilter = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		// Token: 0x04001CC8 RID: 7368
		private const string DesignerStateDataSourceSchemaKey = "DataSourceSchema";

		// Token: 0x04001CC9 RID: 7369
		private const string DesignerStateDataSourceSchemaTypeNameKey = "DataSourceSchemaTypeName";

		// Token: 0x04001CCA RID: 7370
		private const string DesignerStateDataSourceSchemaSelectMethodKey = "DataSourceSchemaSelectMethod";

		// Token: 0x04001CCB RID: 7371
		private const string DesignerStateDataSourceSchemaSelectMethodReturnTypeNameKey = "DataSourceSchemaSelectMethodReturnTypeName";

		// Token: 0x04001CCC RID: 7372
		private const string DesignerStateShowOnlyDataComponentsStateKey = "ShowOnlyDataComponentsState";

		// Token: 0x04001CCD RID: 7373
		private bool _inWizard;

		// Token: 0x04001CCE RID: 7374
		private Type _selectMethodReturnType;

		// Token: 0x04001CCF RID: 7375
		private bool _forceSchemaRetrieval;

		// Token: 0x02000487 RID: 1159
		private sealed class DataTableArraySchema : IDataSourceSchema
		{
			// Token: 0x06002A25 RID: 10789 RVA: 0x000E7D1B File Offset: 0x000E6D1B
			public DataTableArraySchema(DataTable[] tables)
			{
				this._tables = tables;
			}

			// Token: 0x06002A26 RID: 10790 RVA: 0x000E7D2C File Offset: 0x000E6D2C
			public IDataSourceViewSchema[] GetViews()
			{
				DataSetViewSchema[] array = new DataSetViewSchema[this._tables.Length];
				for (int i = 0; i < this._tables.Length; i++)
				{
					array[i] = new DataSetViewSchema(this._tables[i]);
				}
				return array;
			}

			// Token: 0x04001CD0 RID: 7376
			private DataTable[] _tables;
		}
	}
}
