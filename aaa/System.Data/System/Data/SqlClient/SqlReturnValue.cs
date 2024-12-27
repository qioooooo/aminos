using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200032D RID: 813
	internal sealed class SqlReturnValue : SqlMetaDataPriv
	{
		// Token: 0x06002A7F RID: 10879 RVA: 0x0029CF30 File Offset: 0x0029C330
		internal SqlReturnValue()
		{
			this.value = new SqlBuffer();
		}

		// Token: 0x04001BE8 RID: 7144
		internal ushort parmIndex;

		// Token: 0x04001BE9 RID: 7145
		internal string parameter;

		// Token: 0x04001BEA RID: 7146
		internal readonly SqlBuffer value;
	}
}
