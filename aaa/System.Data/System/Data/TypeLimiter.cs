using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Data
{
	// Token: 0x020000E8 RID: 232
	internal sealed class TypeLimiter
	{
		// Token: 0x06000DA0 RID: 3488 RVA: 0x002007B0 File Offset: 0x001FFBB0
		private TypeLimiter(TypeLimiter.Scope scope)
		{
			this.m_instanceScope = scope;
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x002007CC File Offset: 0x001FFBCC
		private static bool IsTypeLimitingEnabled
		{
			get
			{
				if (!TypeLimiter.s_isOptedOutValueInitialized)
				{
					TypeLimiter.s_isOptedOut = TypeLimiter.ReadTypeLimitingRegistrySetting();
					TypeLimiter.s_isOptedOutValueInitialized = true;
				}
				return !TypeLimiter.s_isOptedOut;
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x002007FC File Offset: 0x001FFBFC
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool ReadTypeLimitingRegistrySetting()
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AppContext", false))
				{
					if (registryKey != null && registryKey.GetValueKind("Switch.System.Data.AllowArbitraryDataSetTypeInstantiation") == RegistryValueKind.String && "true".Equals((string)registryKey.GetValue("Switch.System.Data.AllowArbitraryDataSetTypeInstantiation"), StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00200894 File Offset: 0x001FFC94
		public static TypeLimiter Capture()
		{
			TypeLimiter.Scope scope = TypeLimiter.s_activeScope;
			if (scope == null)
			{
				return null;
			}
			return new TypeLimiter(scope);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x002008B4 File Offset: 0x001FFCB4
		public static void EnsureTypeIsAllowed(Type type)
		{
			TypeLimiter.EnsureTypeIsAllowed(type, null);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x002008C8 File Offset: 0x001FFCC8
		public static void EnsureTypeIsAllowed(Type type, TypeLimiter capturedLimiter)
		{
			if (type == null)
			{
				return;
			}
			TypeLimiter.Scope scope = ((capturedLimiter == null) ? null : capturedLimiter.m_instanceScope);
			scope = ((scope == null) ? TypeLimiter.s_activeScope : scope);
			if (scope == null)
			{
				return;
			}
			if (scope.IsAllowedType(type))
			{
				return;
			}
			DataSetTraceSource.TraceTypeNotAllowed(type);
			if (!SerializationConfig.IsAuditMode())
			{
				throw ExceptionBuilder.TypeNotAllowed(type);
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00200914 File Offset: 0x001FFD14
		public static IDisposable EnterRestrictedScope(DataSet dataSet)
		{
			if (!TypeLimiter.IsTypeLimitingEnabled)
			{
				return null;
			}
			TypeLimiter.Scope scope = new TypeLimiter.Scope(TypeLimiter.s_activeScope, TypeLimiter.GetPreviouslyDeclaredDataTypes(dataSet));
			TypeLimiter.s_activeScope = scope;
			return scope;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00200944 File Offset: 0x001FFD44
		public static IDisposable EnterRestrictedScope(DataTable dataTable)
		{
			if (!TypeLimiter.IsTypeLimitingEnabled)
			{
				return null;
			}
			TypeLimiter.Scope scope = new TypeLimiter.Scope(TypeLimiter.s_activeScope, TypeLimiter.GetPreviouslyDeclaredDataTypes(dataTable));
			TypeLimiter.s_activeScope = scope;
			return scope;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00200974 File Offset: 0x001FFD74
		private static IEnumerable<Type> GetPreviouslyDeclaredDataTypes(DataTable dataTable)
		{
			List<Type> list = new List<Type>();
			if (dataTable != null)
			{
				foreach (object obj in dataTable.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					list.Add(dataColumn.DataType);
				}
			}
			return list;
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x002009E8 File Offset: 0x001FFDE8
		private static IEnumerable<Type> GetPreviouslyDeclaredDataTypes(DataSet dataSet)
		{
			List<Type> list = new List<Type>();
			if (dataSet != null)
			{
				foreach (object obj in dataSet.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					list.AddRange(TypeLimiter.GetPreviouslyDeclaredDataTypes(dataTable));
				}
			}
			return list;
		}

		// Token: 0x04000952 RID: 2386
		private const string AppDomainDataSetDefaultAllowedTypesKey = "System.Data.DataSetDefaultAllowedTypes";

		// Token: 0x04000953 RID: 2387
		private const string AppContextOptOutSwitchName = "Switch.System.Data.AllowArbitraryDataSetTypeInstantiation";

		// Token: 0x04000954 RID: 2388
		private const string AppContextOptOutRegValuePath = "SOFTWARE\\Microsoft\\.NETFramework\\AppContext";

		// Token: 0x04000955 RID: 2389
		[ThreadStatic]
		private static TypeLimiter.Scope s_activeScope;

		// Token: 0x04000956 RID: 2390
		private TypeLimiter.Scope m_instanceScope;

		// Token: 0x04000957 RID: 2391
		private static bool s_isOptedOut;

		// Token: 0x04000958 RID: 2392
		private static volatile bool s_isOptedOutValueInitialized;

		// Token: 0x020000E9 RID: 233
		private sealed class Scope : IDisposable
		{
			// Token: 0x06000DAA RID: 3498 RVA: 0x00200A5C File Offset: 0x001FFE5C
			static Scope()
			{
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(bool), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(char), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(sbyte), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(byte), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(short), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(ushort), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(int), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(uint), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(long), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(ulong), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(float), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(double), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(decimal), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(DateTime), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(DateTimeOffset), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(TimeSpan), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(string), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(Guid), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlBinary), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlBoolean), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlByte), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlBytes), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlChars), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlDateTime), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlDecimal), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlDouble), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlGuid), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlInt16), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlInt32), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlInt64), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlMoney), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlSingle), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(SqlString), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(object), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(Type), null);
				TypeLimiter.Scope.s_allowedTypes.Add(typeof(Uri), null);
				Assembly assembly = null;
				try
				{
					assembly = Assembly.Load("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				}
				catch
				{
				}
				if (assembly != null)
				{
					TypeLimiter.Scope.s_allowedTypes.Add(assembly.GetType("System.Drawing.Color", true), null);
					TypeLimiter.Scope.s_allowedTypes.Add(assembly.GetType("System.Drawing.Point", true), null);
					TypeLimiter.Scope.s_allowedTypes.Add(assembly.GetType("System.Drawing.PointF", true), null);
					TypeLimiter.Scope.s_allowedTypes.Add(assembly.GetType("System.Drawing.Rectangle", true), null);
					TypeLimiter.Scope.s_allowedTypes.Add(assembly.GetType("System.Drawing.RectangleF", true), null);
					TypeLimiter.Scope.s_allowedTypes.Add(assembly.GetType("System.Drawing.Size", true), null);
					TypeLimiter.Scope.s_allowedTypes.Add(assembly.GetType("System.Drawing.SizeF", true), null);
				}
				TypeLimiter.Scope.s_allowedSuperTypes.Add(typeof(Enum), null);
			}

			// Token: 0x06000DAB RID: 3499 RVA: 0x00200E5C File Offset: 0x0020025C
			internal Scope(TypeLimiter.Scope previousScope, IEnumerable<Type> allowedTypes)
			{
				this.m_previousScope = previousScope;
				this.m_allowedTypes = new Dictionary<Type, object>();
				foreach (Type type in allowedTypes)
				{
					this.m_allowedTypes[type] = null;
				}
			}

			// Token: 0x06000DAC RID: 3500 RVA: 0x00200ED0 File Offset: 0x002002D0
			public void Dispose()
			{
				if (this != TypeLimiter.s_activeScope)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				TypeLimiter.s_activeScope = this.m_previousScope;
			}

			// Token: 0x06000DAD RID: 3501 RVA: 0x00200F04 File Offset: 0x00200304
			public bool IsAllowedType(Type type)
			{
				if (TypeLimiter.Scope.IsTypeUnconditionallyAllowed(type))
				{
					return true;
				}
				for (TypeLimiter.Scope scope = this; scope != null; scope = scope.m_previousScope)
				{
					if (scope.m_allowedTypes.ContainsKey(type))
					{
						return true;
					}
				}
				Type[] array = (Type[])AppDomain.CurrentDomain.GetData("System.Data.DataSetDefaultAllowedTypes");
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (type == array[i])
						{
							return true;
						}
					}
				}
				return SerializationConfig.IsTypeAllowed(type);
			}

			// Token: 0x06000DAE RID: 3502 RVA: 0x00200F74 File Offset: 0x00200374
			private static bool IsTypeUnconditionallyAllowed(Type type)
			{
				while (!TypeLimiter.Scope.s_allowedTypes.ContainsKey(type))
				{
					for (Type type2 = type.BaseType; type2 != null; type2 = type2.BaseType)
					{
						if (TypeLimiter.Scope.s_allowedSuperTypes.ContainsKey(type2))
						{
							return true;
						}
					}
					if (type.IsArray && type.GetArrayRank() == 1)
					{
						type = type.GetElementType();
					}
					else
					{
						if (!type.IsGenericType || type.IsGenericTypeDefinition || type.GetGenericTypeDefinition() != typeof(List<>))
						{
							return false;
						}
						type = type.GetGenericArguments()[0];
					}
				}
				return true;
			}

			// Token: 0x04000959 RID: 2393
			private static readonly Dictionary<Type, object> s_allowedTypes = new Dictionary<Type, object>();

			// Token: 0x0400095A RID: 2394
			private static readonly Dictionary<Type, object> s_allowedSuperTypes = new Dictionary<Type, object>();

			// Token: 0x0400095B RID: 2395
			private Dictionary<Type, object> m_allowedTypes;

			// Token: 0x0400095C RID: 2396
			private readonly TypeLimiter.Scope m_previousScope;
		}
	}
}
