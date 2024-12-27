using System;

namespace SafeNet.Sentinel
{
	// Token: 0x02000008 RID: 8
	public enum AdminStatus
	{
		// Token: 0x04000015 RID: 21
		StatusOk,
		// Token: 0x04000016 RID: 22
		InsufMem = 3,
		// Token: 0x04000017 RID: 23
		InvalidContext = 6001,
		// Token: 0x04000018 RID: 24
		LmNotFound,
		// Token: 0x04000019 RID: 25
		LmTooOld,
		// Token: 0x0400001A RID: 26
		BadParameters,
		// Token: 0x0400001B RID: 27
		LocalNetWorkError,
		// Token: 0x0400001C RID: 28
		CannotReadFile,
		// Token: 0x0400001D RID: 29
		ScopeError,
		// Token: 0x0400001E RID: 30
		PasswordRequired,
		// Token: 0x0400001F RID: 31
		CannotSetPassword,
		// Token: 0x04000020 RID: 32
		UpdateError,
		// Token: 0x04000021 RID: 33
		LocalOnly,
		// Token: 0x04000022 RID: 34
		BadValue,
		// Token: 0x04000023 RID: 35
		ReadOnly,
		// Token: 0x04000024 RID: 36
		ElementUndefined,
		// Token: 0x04000025 RID: 37
		InvalidPointer,
		// Token: 0x04000026 RID: 38
		NoIntegratedLm,
		// Token: 0x04000027 RID: 39
		ResultTooBig,
		// Token: 0x04000028 RID: 40
		ScopeResultsEmpty = 6019,
		// Token: 0x04000029 RID: 41
		InvalidVendorCode = 6022,
		// Token: 0x0400002A RID: 42
		UnknownVendorCode = 6034,
		// Token: 0x0400002B RID: 43
		ConnectMissing = 6051,
		// Token: 0x0400002C RID: 44
		DotNetDllBroken
	}
}
