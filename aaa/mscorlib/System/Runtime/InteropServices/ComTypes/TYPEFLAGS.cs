﻿using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056B RID: 1387
	[Flags]
	[Serializable]
	public enum TYPEFLAGS : short
	{
		// Token: 0x04001AE7 RID: 6887
		TYPEFLAG_FAPPOBJECT = 1,
		// Token: 0x04001AE8 RID: 6888
		TYPEFLAG_FCANCREATE = 2,
		// Token: 0x04001AE9 RID: 6889
		TYPEFLAG_FLICENSED = 4,
		// Token: 0x04001AEA RID: 6890
		TYPEFLAG_FPREDECLID = 8,
		// Token: 0x04001AEB RID: 6891
		TYPEFLAG_FHIDDEN = 16,
		// Token: 0x04001AEC RID: 6892
		TYPEFLAG_FCONTROL = 32,
		// Token: 0x04001AED RID: 6893
		TYPEFLAG_FDUAL = 64,
		// Token: 0x04001AEE RID: 6894
		TYPEFLAG_FNONEXTENSIBLE = 128,
		// Token: 0x04001AEF RID: 6895
		TYPEFLAG_FOLEAUTOMATION = 256,
		// Token: 0x04001AF0 RID: 6896
		TYPEFLAG_FRESTRICTED = 512,
		// Token: 0x04001AF1 RID: 6897
		TYPEFLAG_FAGGREGATABLE = 1024,
		// Token: 0x04001AF2 RID: 6898
		TYPEFLAG_FREPLACEABLE = 2048,
		// Token: 0x04001AF3 RID: 6899
		TYPEFLAG_FDISPATCHABLE = 4096,
		// Token: 0x04001AF4 RID: 6900
		TYPEFLAG_FREVERSEBIND = 8192,
		// Token: 0x04001AF5 RID: 6901
		TYPEFLAG_FPROXY = 16384
	}
}
