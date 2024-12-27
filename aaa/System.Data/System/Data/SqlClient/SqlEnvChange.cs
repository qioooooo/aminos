using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000325 RID: 805
	internal sealed class SqlEnvChange
	{
		// Token: 0x04001B8D RID: 7053
		internal byte type;

		// Token: 0x04001B8E RID: 7054
		internal byte oldLength;

		// Token: 0x04001B8F RID: 7055
		internal int newLength;

		// Token: 0x04001B90 RID: 7056
		internal int length;

		// Token: 0x04001B91 RID: 7057
		internal string newValue;

		// Token: 0x04001B92 RID: 7058
		internal string oldValue;

		// Token: 0x04001B93 RID: 7059
		internal byte[] newBinValue;

		// Token: 0x04001B94 RID: 7060
		internal byte[] oldBinValue;

		// Token: 0x04001B95 RID: 7061
		internal long newLongValue;

		// Token: 0x04001B96 RID: 7062
		internal long oldLongValue;

		// Token: 0x04001B97 RID: 7063
		internal SqlCollation newCollation;

		// Token: 0x04001B98 RID: 7064
		internal SqlCollation oldCollation;

		// Token: 0x04001B99 RID: 7065
		internal RoutingInfo newRoutingInfo;
	}
}
