using System;
using System.Configuration;
using System.Reflection;

namespace System.Data.Common
{
	// Token: 0x0200013F RID: 319
	public static class DbProviderFactories
	{
		// Token: 0x060014E5 RID: 5349 RVA: 0x002286F8 File Offset: 0x00227AF8
		public static DbProviderFactory GetFactory(string providerInvariantName)
		{
			ADP.CheckArgumentLength(providerInvariantName, "providerInvariantName");
			DataSet configTable = DbProviderFactories.GetConfigTable();
			DataTable dataTable = ((configTable != null) ? configTable.Tables["DbProviderFactories"] : null);
			if (dataTable != null)
			{
				DataRow dataRow = dataTable.Rows.Find(providerInvariantName);
				if (dataRow != null)
				{
					return DbProviderFactories.GetFactory(dataRow);
				}
			}
			throw ADP.ConfigProviderNotFound();
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0022874C File Offset: 0x00227B4C
		public static DbProviderFactory GetFactory(DataRow providerRow)
		{
			ADP.CheckArgumentNull(providerRow, "providerRow");
			DataColumn dataColumn = providerRow.Table.Columns["AssemblyQualifiedName"];
			if (dataColumn != null)
			{
				string text = providerRow[dataColumn] as string;
				if (!ADP.IsEmpty(text))
				{
					Type type = Type.GetType(text);
					if (type != null)
					{
						FieldInfo field = type.GetField("Instance", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
						if (field != null && field.FieldType.IsSubclassOf(typeof(DbProviderFactory)))
						{
							object value = field.GetValue(null);
							if (value != null)
							{
								return (DbProviderFactory)value;
							}
						}
						throw ADP.ConfigProviderInvalid();
					}
					throw ADP.ConfigProviderNotInstalled();
				}
			}
			throw ADP.ConfigProviderMissing();
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x002287EC File Offset: 0x00227BEC
		public static DataTable GetFactoryClasses()
		{
			DataSet configTable = DbProviderFactories.GetConfigTable();
			DataTable dataTable = ((configTable != null) ? configTable.Tables["DbProviderFactories"] : null);
			if (dataTable != null)
			{
				dataTable = dataTable.Copy();
			}
			else
			{
				dataTable = DbProviderFactoriesConfigurationHandler.CreateProviderDataTable();
			}
			return dataTable;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0022882C File Offset: 0x00227C2C
		private static DataSet GetConfigTable()
		{
			DbProviderFactories.Initialize();
			return DbProviderFactories._configTable;
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00228844 File Offset: 0x00227C44
		private static void Initialize()
		{
			if (ConnectionState.Open != DbProviderFactories._initState)
			{
				lock (DbProviderFactories._lockobj)
				{
					switch (DbProviderFactories._initState)
					{
					case ConnectionState.Closed:
						DbProviderFactories._initState = ConnectionState.Connecting;
						try
						{
							DbProviderFactories._configTable = PrivilegedConfigurationManager.GetSection("system.data") as DataSet;
						}
						finally
						{
							DbProviderFactories._initState = ConnectionState.Open;
						}
						break;
					}
				}
			}
		}

		// Token: 0x04000C50 RID: 3152
		private const string AssemblyQualifiedName = "AssemblyQualifiedName";

		// Token: 0x04000C51 RID: 3153
		private const string Instance = "Instance";

		// Token: 0x04000C52 RID: 3154
		private const string InvariantName = "InvariantName";

		// Token: 0x04000C53 RID: 3155
		private static ConnectionState _initState;

		// Token: 0x04000C54 RID: 3156
		private static DataSet _configTable;

		// Token: 0x04000C55 RID: 3157
		private static object _lockobj = new object();
	}
}
