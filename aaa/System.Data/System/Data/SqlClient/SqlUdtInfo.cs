using System;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x02000312 RID: 786
	internal class SqlUdtInfo
	{
		// Token: 0x06002916 RID: 10518 RVA: 0x002924B0 File Offset: 0x002918B0
		private SqlUdtInfo(SqlUserDefinedTypeAttribute attr)
		{
			this.SerializationFormat = attr.Format;
			this.IsByteOrdered = attr.IsByteOrdered;
			this.IsFixedLength = attr.IsFixedLength;
			this.MaxByteSize = attr.MaxByteSize;
			this.Name = attr.Name;
			this.ValidationMethodName = attr.ValidationMethodName;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x0029250C File Offset: 0x0029190C
		internal static SqlUdtInfo GetFromType(Type target)
		{
			SqlUdtInfo sqlUdtInfo = SqlUdtInfo.TryGetFromType(target);
			if (sqlUdtInfo == null)
			{
				throw InvalidUdtException.Create(target, "SqlUdtReason_NoUdtAttribute");
			}
			return sqlUdtInfo;
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x00292530 File Offset: 0x00291930
		internal static SqlUdtInfo TryGetFromType(Type target)
		{
			SqlUdtInfo sqlUdtInfo = null;
			object[] customAttributes = target.GetCustomAttributes(typeof(SqlUserDefinedTypeAttribute), false);
			if (customAttributes != null && customAttributes.Length == 1)
			{
				sqlUdtInfo = new SqlUdtInfo((SqlUserDefinedTypeAttribute)customAttributes[0]);
			}
			return sqlUdtInfo;
		}

		// Token: 0x040019A1 RID: 6561
		internal readonly Format SerializationFormat;

		// Token: 0x040019A2 RID: 6562
		internal readonly bool IsByteOrdered;

		// Token: 0x040019A3 RID: 6563
		internal readonly bool IsFixedLength;

		// Token: 0x040019A4 RID: 6564
		internal readonly int MaxByteSize;

		// Token: 0x040019A5 RID: 6565
		internal readonly string Name;

		// Token: 0x040019A6 RID: 6566
		internal readonly string ValidationMethodName;
	}
}
