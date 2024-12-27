using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002A4 RID: 676
	internal class ParameterPeekAheadValue
	{
		// Token: 0x04001673 RID: 5747
		internal IEnumerator<SqlDataRecord> Enumerator;

		// Token: 0x04001674 RID: 5748
		internal SqlDataRecord FirstRecord;
	}
}
