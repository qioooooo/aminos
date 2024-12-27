using System;

namespace System.Web.Configuration
{
	// Token: 0x020001BF RID: 447
	internal enum ClsCtx
	{
		// Token: 0x04001750 RID: 5968
		Inproc = 3,
		// Token: 0x04001751 RID: 5969
		Server = 21,
		// Token: 0x04001752 RID: 5970
		All = 23,
		// Token: 0x04001753 RID: 5971
		InprocServer = 1,
		// Token: 0x04001754 RID: 5972
		InprocHandler,
		// Token: 0x04001755 RID: 5973
		LocalServer = 4,
		// Token: 0x04001756 RID: 5974
		InprocServer16 = 8,
		// Token: 0x04001757 RID: 5975
		RemoteServer = 16,
		// Token: 0x04001758 RID: 5976
		InprocHandler16 = 32,
		// Token: 0x04001759 RID: 5977
		InprocServerX86 = 64,
		// Token: 0x0400175A RID: 5978
		InprocHandlerX86 = 128,
		// Token: 0x0400175B RID: 5979
		EServerHandler = 256,
		// Token: 0x0400175C RID: 5980
		Reserved = 512,
		// Token: 0x0400175D RID: 5981
		NoCodeDownload = 1024,
		// Token: 0x0400175E RID: 5982
		NoWX86Translation = 2048,
		// Token: 0x0400175F RID: 5983
		NoCustomMarshal = 4096,
		// Token: 0x04001760 RID: 5984
		EnableCodeDownload = 8192,
		// Token: 0x04001761 RID: 5985
		NoFailureLog = 16384,
		// Token: 0x04001762 RID: 5986
		DisableAAA = 32768,
		// Token: 0x04001763 RID: 5987
		EnableAAA = 65536,
		// Token: 0x04001764 RID: 5988
		FromDefaultContext = 131072
	}
}
