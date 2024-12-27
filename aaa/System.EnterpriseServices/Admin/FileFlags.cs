using System;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000054 RID: 84
	[Serializable]
	internal enum FileFlags
	{
		// Token: 0x040000B5 RID: 181
		Loadable = 1,
		// Token: 0x040000B6 RID: 182
		COM,
		// Token: 0x040000B7 RID: 183
		ContainsPS = 4,
		// Token: 0x040000B8 RID: 184
		ContainsComp = 8,
		// Token: 0x040000B9 RID: 185
		ContainsTLB = 16,
		// Token: 0x040000BA RID: 186
		SelfReg = 32,
		// Token: 0x040000BB RID: 187
		SelfUnReg = 64,
		// Token: 0x040000BC RID: 188
		UnloadableDLL = 128,
		// Token: 0x040000BD RID: 189
		DoesNotExists = 256,
		// Token: 0x040000BE RID: 190
		AlreadyInstalled = 512,
		// Token: 0x040000BF RID: 191
		BadTLB = 1024,
		// Token: 0x040000C0 RID: 192
		GetClassObjFailed = 2048,
		// Token: 0x040000C1 RID: 193
		ClassNotAvailable = 4096,
		// Token: 0x040000C2 RID: 194
		Registrar = 8192,
		// Token: 0x040000C3 RID: 195
		NoRegistrar = 16384,
		// Token: 0x040000C4 RID: 196
		DLLRegsvrFailed = 32768,
		// Token: 0x040000C5 RID: 197
		RegTLBFailed = 65536,
		// Token: 0x040000C6 RID: 198
		RegistrarFailed = 131072,
		// Token: 0x040000C7 RID: 199
		Error = 262144
	}
}
