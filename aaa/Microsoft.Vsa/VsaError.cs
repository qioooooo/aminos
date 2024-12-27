using System;
using System.Runtime.InteropServices;

namespace Microsoft.Vsa
{
	// Token: 0x02000014 RID: 20
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[Guid("c216f805-8fab-3d33-bf81-7b1035e917b9")]
	public enum VsaError
	{
		// Token: 0x0400001B RID: 27
		AppDomainCannotBeSet = -2146226176,
		// Token: 0x0400001C RID: 28
		AppDomainInvalid,
		// Token: 0x0400001D RID: 29
		ApplicationBaseCannotBeSet,
		// Token: 0x0400001E RID: 30
		ApplicationBaseInvalid,
		// Token: 0x0400001F RID: 31
		AssemblyExpected,
		// Token: 0x04000020 RID: 32
		AssemblyNameInvalid,
		// Token: 0x04000021 RID: 33
		BadAssembly,
		// Token: 0x04000022 RID: 34
		CachedAssemblyInvalid,
		// Token: 0x04000023 RID: 35
		CallbackUnexpected,
		// Token: 0x04000024 RID: 36
		CodeDOMNotAvailable,
		// Token: 0x04000025 RID: 37
		CompiledStateNotFound,
		// Token: 0x04000026 RID: 38
		DebugInfoNotSupported,
		// Token: 0x04000027 RID: 39
		ElementNameInvalid,
		// Token: 0x04000028 RID: 40
		ElementNotFound,
		// Token: 0x04000029 RID: 41
		EngineBusy,
		// Token: 0x0400002A RID: 42
		EngineCannotClose,
		// Token: 0x0400002B RID: 43
		EngineCannotReset,
		// Token: 0x0400002C RID: 44
		EngineClosed,
		// Token: 0x0400002D RID: 45
		EngineEmpty = -2146226159,
		// Token: 0x0400002E RID: 46
		EngineInitialized = -2146226157,
		// Token: 0x0400002F RID: 47
		EngineNameInUse,
		// Token: 0x04000030 RID: 48
		EngineNotCompiled,
		// Token: 0x04000031 RID: 49
		EngineNotInitialized,
		// Token: 0x04000032 RID: 50
		EngineNotRunning,
		// Token: 0x04000033 RID: 51
		EngineRunning,
		// Token: 0x04000034 RID: 52
		EventSourceInvalid,
		// Token: 0x04000035 RID: 53
		EventSourceNameInUse,
		// Token: 0x04000036 RID: 54
		EventSourceNameInvalid,
		// Token: 0x04000037 RID: 55
		EventSourceNotFound,
		// Token: 0x04000038 RID: 56
		EventSourceTypeInvalid,
		// Token: 0x04000039 RID: 57
		GetCompiledStateFailed,
		// Token: 0x0400003A RID: 58
		GlobalInstanceInvalid,
		// Token: 0x0400003B RID: 59
		GlobalInstanceTypeInvalid,
		// Token: 0x0400003C RID: 60
		InternalCompilerError,
		// Token: 0x0400003D RID: 61
		ItemCannotBeRemoved,
		// Token: 0x0400003E RID: 62
		ItemFlagNotSupported,
		// Token: 0x0400003F RID: 63
		ItemNameInUse,
		// Token: 0x04000040 RID: 64
		ItemNameInvalid,
		// Token: 0x04000041 RID: 65
		ItemNotFound,
		// Token: 0x04000042 RID: 66
		ItemTypeNotSupported,
		// Token: 0x04000043 RID: 67
		LCIDNotSupported,
		// Token: 0x04000044 RID: 68
		LoadElementFailed,
		// Token: 0x04000045 RID: 69
		NotificationInvalid,
		// Token: 0x04000046 RID: 70
		OptionInvalid,
		// Token: 0x04000047 RID: 71
		OptionNotSupported,
		// Token: 0x04000048 RID: 72
		RevokeFailed,
		// Token: 0x04000049 RID: 73
		RootMonikerAlreadySet,
		// Token: 0x0400004A RID: 74
		RootMonikerInUse,
		// Token: 0x0400004B RID: 75
		RootMonikerInvalid,
		// Token: 0x0400004C RID: 76
		RootMonikerNotSet,
		// Token: 0x0400004D RID: 77
		RootMonikerProtocolInvalid,
		// Token: 0x0400004E RID: 78
		RootNamespaceInvalid,
		// Token: 0x0400004F RID: 79
		RootNamespaceNotSet,
		// Token: 0x04000050 RID: 80
		SaveCompiledStateFailed,
		// Token: 0x04000051 RID: 81
		SaveElementFailed,
		// Token: 0x04000052 RID: 82
		SiteAlreadySet,
		// Token: 0x04000053 RID: 83
		SiteInvalid,
		// Token: 0x04000054 RID: 84
		SiteNotSet,
		// Token: 0x04000055 RID: 85
		SourceItemNotAvailable,
		// Token: 0x04000056 RID: 86
		SourceMonikerNotAvailable,
		// Token: 0x04000057 RID: 87
		URLInvalid,
		// Token: 0x04000058 RID: 88
		BrowserNotExist,
		// Token: 0x04000059 RID: 89
		DebuggeeNotStarted,
		// Token: 0x0400005A RID: 90
		EngineNameInvalid,
		// Token: 0x0400005B RID: 91
		EngineNotExist,
		// Token: 0x0400005C RID: 92
		FileFormatUnsupported,
		// Token: 0x0400005D RID: 93
		FileTypeUnknown,
		// Token: 0x0400005E RID: 94
		ItemCannotBeRenamed,
		// Token: 0x0400005F RID: 95
		MissingSource,
		// Token: 0x04000060 RID: 96
		NotInitCompleted,
		// Token: 0x04000061 RID: 97
		NameTooLong,
		// Token: 0x04000062 RID: 98
		ProcNameInUse,
		// Token: 0x04000063 RID: 99
		ProcNameInvalid,
		// Token: 0x04000064 RID: 100
		VsaServerDown,
		// Token: 0x04000065 RID: 101
		MissingPdb,
		// Token: 0x04000066 RID: 102
		NotClientSideAndNoUrl,
		// Token: 0x04000067 RID: 103
		CannotAttachToWebServer,
		// Token: 0x04000068 RID: 104
		EngineNameNotSet,
		// Token: 0x04000069 RID: 105
		UnknownError = -2146225921
	}
}
