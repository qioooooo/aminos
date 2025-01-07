using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.Data.Design
{
	internal sealed class ProviderManager
	{
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

		private static DataTable factoryTable = null;

		private static ProviderManager.CachedProviderData providerData = new ProviderManager.CachedProviderData();

		internal static Hashtable CustomDBProviders = null;

		internal static DbProviderFactory ActiveFactoryContext = null;

		private static readonly string PROVIDER_NAME = "Name";

		private static readonly string PROVIDER_INVARIANT_NAME = "InvariantName";

		private static readonly string PROVIDER_ASSEMBLY = "AssemblyQualifiedName";

		internal enum ProviderSupportedClasses
		{
			DbConnection,
			DbDataAdapter,
			DbParameter,
			DbCommand,
			DbCommandBuilder,
			DbDataSourceEnumerator,
			CodeAccessPermission,
			DbConnectionStringBuilder
		}

		private class CachedProviderData
		{
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

			public bool Matches(Type type)
			{
				return this.CachedFactory != null && this.CachedType != null && this.CachedType.Equals(type);
			}

			public bool Matches(string invariantName)
			{
				return this.CachedFactory != null && this.CachedInvariantProviderName != null && StringUtil.EqualValue(this.CachedInvariantProviderName, invariantName);
			}

			public bool Matches(DbProviderFactory factory)
			{
				return this.CachedFactory != null && this.CachedFactory.GetType().Equals(factory.GetType());
			}

			public void Initialize(DbProviderFactory factory, string invariantProviderName, string displayName)
			{
				this.CachedFactory = factory;
				this.CachedInvariantProviderName = invariantProviderName;
				this.CachedType = null;
				this.CachedDisplayName = displayName;
				this.ProviderTypeProperty = null;
				this.UseCachedPropertyValue = false;
			}

			public void Initialize(DbProviderFactory factory, string invariantProviderName, string displayName, Type type)
			{
				this.Initialize(factory, invariantProviderName, displayName);
				this.CachedType = type;
			}

			public DbProviderFactory CachedFactory;

			public Type CachedType;

			public string CachedInvariantProviderName = string.Empty;

			public string CachedDisplayName = string.Empty;

			private PropertyInfo providerTypeProperty;

			private bool useCachedPropertyValue;
		}
	}
}
