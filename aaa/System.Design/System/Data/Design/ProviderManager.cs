using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.Data.Design
{
	// Token: 0x020000AE RID: 174
	internal sealed class ProviderManager
	{
		// Token: 0x06000802 RID: 2050 RVA: 0x0001314C File Offset: 0x0001214C
		public static DbProviderFactory GetFactoryFromType(Type type, ProviderManager.ProviderSupportedClasses kindOfObject)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (ProviderManager.providerData.Matches(type))
			{
				return ProviderManager.providerData.CachedFactory;
			}
			ProviderManager.EnsureFactoryTable();
			foreach (object obj in ProviderManager.factoryTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				DbProviderFactory factory = DbProviderFactories.GetFactory(dataRow);
				string text = (string)dataRow[ProviderManager.PROVIDER_NAME];
				object obj2 = ProviderManager.CreateObject(factory, kindOfObject, text);
				if (type.Equals(obj2.GetType()))
				{
					ProviderManager.providerData.Initialize(factory, (string)dataRow[ProviderManager.PROVIDER_INVARIANT_NAME], (string)dataRow[ProviderManager.PROVIDER_NAME], type);
					return factory;
				}
			}
			throw new InternalException(string.Format(CultureInfo.CurrentCulture, "Unable to find DbProviderFactory for type {0}", new object[] { type.ToString() }));
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00013260 File Offset: 0x00012260
		public static string GetInvariantProviderName(DbProviderFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			if (ProviderManager.providerData.Matches(factory))
			{
				return ProviderManager.providerData.CachedInvariantProviderName;
			}
			ProviderManager.EnsureFactoryTable();
			string assemblyQualifiedName = factory.GetType().AssemblyQualifiedName;
			foreach (object obj in ProviderManager.factoryTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if (StringUtil.EqualValue((string)dataRow[ProviderManager.PROVIDER_ASSEMBLY], assemblyQualifiedName))
				{
					ProviderManager.providerData.Initialize(factory, (string)dataRow[ProviderManager.PROVIDER_INVARIANT_NAME], (string)dataRow[ProviderManager.PROVIDER_NAME]);
					return ProviderManager.providerData.CachedInvariantProviderName;
				}
			}
			throw new InternalException(string.Format(CultureInfo.CurrentCulture, "Unable to get invariant name from factory. Factory type is {0}", new object[] { factory.GetType().ToString() }));
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00013370 File Offset: 0x00012370
		public static DbProviderFactory GetFactory(string invariantName)
		{
			if (StringUtil.EmptyOrSpace(invariantName))
			{
				throw new ArgumentNullException("invariantName");
			}
			if (ProviderManager.ActiveFactoryContext != null)
			{
				ProviderManager.providerData.Initialize(ProviderManager.ActiveFactoryContext, invariantName, invariantName);
				return ProviderManager.ActiveFactoryContext;
			}
			if (ProviderManager.CustomDBProviders != null && ProviderManager.CustomDBProviders.ContainsKey(invariantName))
			{
				DbProviderFactory dbProviderFactory = ProviderManager.CustomDBProviders[invariantName] as DbProviderFactory;
				if (dbProviderFactory != null)
				{
					ProviderManager.providerData.Initialize(dbProviderFactory, invariantName, invariantName);
					return dbProviderFactory;
				}
			}
			if (ProviderManager.providerData.Matches(invariantName))
			{
				return ProviderManager.providerData.CachedFactory;
			}
			ProviderManager.EnsureFactoryTable();
			DataRow[] array = ProviderManager.factoryTable.Select(string.Format(CultureInfo.CurrentCulture, "InvariantName = '{0}'", new object[] { invariantName }));
			if (array.Length == 0)
			{
				throw new InternalException(string.Format(CultureInfo.CurrentCulture, "Cannot find provider factory for provider named {0}", new object[] { invariantName }));
			}
			if (array.Length > 1)
			{
				throw new InternalException(string.Format(CultureInfo.CurrentCulture, "More that one data row for provider named {0}", new object[] { invariantName }));
			}
			DbProviderFactory factory = DbProviderFactories.GetFactory(array[0]);
			ProviderManager.providerData.Initialize(factory, invariantName, (string)array[0][ProviderManager.PROVIDER_NAME]);
			return factory;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x000134A4 File Offset: 0x000124A4
		public static PropertyInfo GetProviderTypeProperty(DbProviderFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory should not be null.");
			}
			if (ProviderManager.providerData.UseCachedPropertyValue)
			{
				return ProviderManager.providerData.ProviderTypeProperty;
			}
			ProviderManager.providerData.UseCachedPropertyValue = true;
			DbParameter dbParameter = factory.CreateParameter();
			PropertyInfo[] properties = dbParameter.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.PropertyType.IsEnum)
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(DbProviderSpecificTypePropertyAttribute), true);
					if (customAttributes.Length > 0 && ((DbProviderSpecificTypePropertyAttribute)customAttributes[0]).IsProviderSpecificTypeProperty)
					{
						ProviderManager.providerData.ProviderTypeProperty = propertyInfo;
						return propertyInfo;
					}
				}
			}
			ProviderManager.providerData.ProviderTypeProperty = null;
			return null;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00013568 File Offset: 0x00012568
		private static object CreateObject(DbProviderFactory factory, ProviderManager.ProviderSupportedClasses kindOfObject, string providerName)
		{
			switch (kindOfObject)
			{
			case ProviderManager.ProviderSupportedClasses.DbConnection:
				return factory.CreateConnection();
			case ProviderManager.ProviderSupportedClasses.DbDataAdapter:
				return factory.CreateDataAdapter();
			case ProviderManager.ProviderSupportedClasses.DbParameter:
				return factory.CreateParameter();
			case ProviderManager.ProviderSupportedClasses.DbCommand:
				return factory.CreateCommand();
			case ProviderManager.ProviderSupportedClasses.DbCommandBuilder:
				return factory.CreateCommandBuilder();
			case ProviderManager.ProviderSupportedClasses.DbDataSourceEnumerator:
				return factory.CreateDataSourceEnumerator();
			case ProviderManager.ProviderSupportedClasses.CodeAccessPermission:
				return factory.CreatePermission(PermissionState.None);
			default:
			{
				string text = string.Format(CultureInfo.CurrentCulture, "Cannot create object of provider class identified by enum {0} for provider {1}", new object[]
				{
					Enum.GetName(typeof(ProviderManager.ProviderSupportedClasses), kindOfObject),
					providerName
				});
				throw new InternalException(text);
			}
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00013607 File Offset: 0x00012607
		private static void EnsureFactoryTable()
		{
			if (ProviderManager.factoryTable == null)
			{
				ProviderManager.factoryTable = DbProviderFactories.GetFactoryClasses();
				if (ProviderManager.factoryTable == null)
				{
					throw new InternalException("Unable to get factory-table.");
				}
			}
		}

		// Token: 0x04000BD6 RID: 3030
		private static DataTable factoryTable = null;

		// Token: 0x04000BD7 RID: 3031
		private static ProviderManager.CachedProviderData providerData = new ProviderManager.CachedProviderData();

		// Token: 0x04000BD8 RID: 3032
		internal static Hashtable CustomDBProviders = null;

		// Token: 0x04000BD9 RID: 3033
		internal static DbProviderFactory ActiveFactoryContext = null;

		// Token: 0x04000BDA RID: 3034
		private static readonly string PROVIDER_NAME = "Name";

		// Token: 0x04000BDB RID: 3035
		private static readonly string PROVIDER_INVARIANT_NAME = "InvariantName";

		// Token: 0x04000BDC RID: 3036
		private static readonly string PROVIDER_ASSEMBLY = "AssemblyQualifiedName";

		// Token: 0x020000AF RID: 175
		internal enum ProviderSupportedClasses
		{
			// Token: 0x04000BDE RID: 3038
			DbConnection,
			// Token: 0x04000BDF RID: 3039
			DbDataAdapter,
			// Token: 0x04000BE0 RID: 3040
			DbParameter,
			// Token: 0x04000BE1 RID: 3041
			DbCommand,
			// Token: 0x04000BE2 RID: 3042
			DbCommandBuilder,
			// Token: 0x04000BE3 RID: 3043
			DbDataSourceEnumerator,
			// Token: 0x04000BE4 RID: 3044
			CodeAccessPermission,
			// Token: 0x04000BE5 RID: 3045
			DbConnectionStringBuilder
		}

		// Token: 0x020000B0 RID: 176
		private class CachedProviderData
		{
			// Token: 0x1700011D RID: 285
			// (get) Token: 0x0600080A RID: 2058 RVA: 0x00013670 File Offset: 0x00012670
			// (set) Token: 0x0600080B RID: 2059 RVA: 0x00013678 File Offset: 0x00012678
			public PropertyInfo ProviderTypeProperty
			{
				get
				{
					return this.providerTypeProperty;
				}
				set
				{
					this.providerTypeProperty = value;
				}
			}

			// Token: 0x1700011E RID: 286
			// (get) Token: 0x0600080C RID: 2060 RVA: 0x00013681 File Offset: 0x00012681
			// (set) Token: 0x0600080D RID: 2061 RVA: 0x00013689 File Offset: 0x00012689
			public bool UseCachedPropertyValue
			{
				get
				{
					return this.useCachedPropertyValue;
				}
				set
				{
					this.useCachedPropertyValue = value;
				}
			}

			// Token: 0x0600080E RID: 2062 RVA: 0x00013692 File Offset: 0x00012692
			public bool Matches(Type type)
			{
				return this.CachedFactory != null && this.CachedType != null && this.CachedType.Equals(type);
			}

			// Token: 0x0600080F RID: 2063 RVA: 0x000136B5 File Offset: 0x000126B5
			public bool Matches(string invariantName)
			{
				return this.CachedFactory != null && this.CachedInvariantProviderName != null && StringUtil.EqualValue(this.CachedInvariantProviderName, invariantName);
			}

			// Token: 0x06000810 RID: 2064 RVA: 0x000136D8 File Offset: 0x000126D8
			public bool Matches(DbProviderFactory factory)
			{
				return this.CachedFactory != null && this.CachedFactory.GetType().Equals(factory.GetType());
			}

			// Token: 0x06000811 RID: 2065 RVA: 0x000136FD File Offset: 0x000126FD
			public void Initialize(DbProviderFactory factory, string invariantProviderName, string displayName)
			{
				this.CachedFactory = factory;
				this.CachedInvariantProviderName = invariantProviderName;
				this.CachedType = null;
				this.CachedDisplayName = displayName;
				this.ProviderTypeProperty = null;
				this.UseCachedPropertyValue = false;
			}

			// Token: 0x06000812 RID: 2066 RVA: 0x00013729 File Offset: 0x00012729
			public void Initialize(DbProviderFactory factory, string invariantProviderName, string displayName, Type type)
			{
				this.Initialize(factory, invariantProviderName, displayName);
				this.CachedType = type;
			}

			// Token: 0x04000BE6 RID: 3046
			public DbProviderFactory CachedFactory;

			// Token: 0x04000BE7 RID: 3047
			public Type CachedType;

			// Token: 0x04000BE8 RID: 3048
			public string CachedInvariantProviderName = string.Empty;

			// Token: 0x04000BE9 RID: 3049
			public string CachedDisplayName = string.Empty;

			// Token: 0x04000BEA RID: 3050
			private PropertyInfo providerTypeProperty;

			// Token: 0x04000BEB RID: 3051
			private bool useCachedPropertyValue;
		}
	}
}
