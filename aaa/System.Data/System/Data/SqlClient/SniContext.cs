using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000319 RID: 793
	internal enum SniContext
	{
		// Token: 0x04001B1B RID: 6939
		Undefined,
		// Token: 0x04001B1C RID: 6940
		Snix_Connect,
		// Token: 0x04001B1D RID: 6941
		Snix_PreLoginBeforeSuccessfullWrite,
		// Token: 0x04001B1E RID: 6942
		Snix_PreLogin,
		// Token: 0x04001B1F RID: 6943
		Snix_LoginSspi,
		// Token: 0x04001B20 RID: 6944
		Snix_ProcessSspi,
		// Token: 0x04001B21 RID: 6945
		Snix_Login,
		// Token: 0x04001B22 RID: 6946
		Snix_EnableMars,
		// Token: 0x04001B23 RID: 6947
		Snix_AutoEnlist,
		// Token: 0x04001B24 RID: 6948
		Snix_GetMarsSession,
		// Token: 0x04001B25 RID: 6949
		Snix_Execute,
		// Token: 0x04001B26 RID: 6950
		Snix_Read,
		// Token: 0x04001B27 RID: 6951
		Snix_Close,
		// Token: 0x04001B28 RID: 6952
		Snix_SendRows
	}
}
