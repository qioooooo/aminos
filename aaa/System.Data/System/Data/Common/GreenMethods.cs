using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Data.Common
{
	// Token: 0x0200014B RID: 331
	internal static class GreenMethods
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x0022A554 File Offset: 0x00229954
		internal static object SystemDataSqlClientSqlProviderServices_Instance()
		{
			if (GreenMethods.SystemDataSqlClientSqlProviderServices_Instance_FieldInfo == null)
			{
				Type type = Type.GetType("System.Data.SqlClient.SqlProviderServices, System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false);
				if (type != null)
				{
					GreenMethods.SystemDataSqlClientSqlProviderServices_Instance_FieldInfo = type.GetField("Instance", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
				}
			}
			return GreenMethods.SystemDataSqlClientSqlProviderServices_Instance_GetValue();
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0022A590 File Offset: 0x00229990
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private static object SystemDataSqlClientSqlProviderServices_Instance_GetValue()
		{
			object obj = null;
			if (GreenMethods.SystemDataSqlClientSqlProviderServices_Instance_FieldInfo != null)
			{
				obj = GreenMethods.SystemDataSqlClientSqlProviderServices_Instance_FieldInfo.GetValue(null);
			}
			return obj;
		}

		// Token: 0x04000C7F RID: 3199
		private const string ExtensionAssemblyRef = "System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		// Token: 0x04000C80 RID: 3200
		private const string SystemDataCommonDbProviderServices_TypeName = "System.Data.Common.DbProviderServices, System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		// Token: 0x04000C81 RID: 3201
		private const string SystemDataSqlClientSqlProviderServices_TypeName = "System.Data.SqlClient.SqlProviderServices, System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		// Token: 0x04000C82 RID: 3202
		internal static Type SystemDataCommonDbProviderServices_Type = Type.GetType("System.Data.Common.DbProviderServices, System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false);

		// Token: 0x04000C83 RID: 3203
		private static FieldInfo SystemDataSqlClientSqlProviderServices_Instance_FieldInfo;
	}
}
