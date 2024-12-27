using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000008 RID: 8
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[Guid("E0C0FFE2-7eea-4ee2-b7e4-0080c7eb0b74")]
	public interface IVsaSite
	{
		// Token: 0x0600004A RID: 74
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void GetCompiledState(out byte[] pe, out byte[] debugInfo);

		// Token: 0x0600004B RID: 75
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		bool OnCompilerError(IVsaError error);

		// Token: 0x0600004C RID: 76
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetGlobalInstance(string name);

		// Token: 0x0600004D RID: 77
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetEventSourceInstance(string itemName, string eventSourceName);

		// Token: 0x0600004E RID: 78
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Notify(string notify, object info);
	}
}
