using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x0200000A RID: 10
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("E0C0FFE4-7eea-4ee2-b7e4-0080c7eb0b74")]
	public interface IVsaError
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000051 RID: 81
		int Line
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000052 RID: 82
		int Severity
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000053 RID: 83
		string Description
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000054 RID: 84
		string LineText
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000055 RID: 85
		IVsaItem SourceItem
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000056 RID: 86
		int EndColumn
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000057 RID: 87
		int StartColumn
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000058 RID: 88
		int Number
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000059 RID: 89
		string SourceMoniker
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}
	}
}
