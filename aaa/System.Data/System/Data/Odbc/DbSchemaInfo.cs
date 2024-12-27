using System;

namespace System.Data.Odbc
{
	// Token: 0x020001BA RID: 442
	internal sealed class DbSchemaInfo
	{
		// Token: 0x06001952 RID: 6482 RVA: 0x0023E7AC File Offset: 0x0023DBAC
		internal DbSchemaInfo()
		{
		}

		// Token: 0x04000E29 RID: 3625
		internal string _name;

		// Token: 0x04000E2A RID: 3626
		internal string _typename;

		// Token: 0x04000E2B RID: 3627
		internal Type _type;

		// Token: 0x04000E2C RID: 3628
		internal ODBC32.SQL_TYPE? _dbtype;

		// Token: 0x04000E2D RID: 3629
		internal object _scale;

		// Token: 0x04000E2E RID: 3630
		internal object _precision;

		// Token: 0x04000E2F RID: 3631
		internal int _columnlength;

		// Token: 0x04000E30 RID: 3632
		internal int _valueOffset;

		// Token: 0x04000E31 RID: 3633
		internal int _lengthOffset;

		// Token: 0x04000E32 RID: 3634
		internal ODBC32.SQL_C _sqlctype;

		// Token: 0x04000E33 RID: 3635
		internal ODBC32.SQL_TYPE _sql_type;
	}
}
