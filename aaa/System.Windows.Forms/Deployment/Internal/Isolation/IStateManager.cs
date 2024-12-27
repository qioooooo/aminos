using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000134 RID: 308
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("07662534-750b-4ed5-9cfb-1c5bc5acfd07")]
	[ComImport]
	internal interface IStateManager
	{
		// Token: 0x06000470 RID: 1136
		void PrepareApplicationState([In] UIntPtr Inputs, ref UIntPtr Outputs);

		// Token: 0x06000471 RID: 1137
		void SetApplicationRunningState([In] uint Flags, [In] IActContext Context, [In] uint RunningState, out uint Disposition);

		// Token: 0x06000472 RID: 1138
		void GetApplicationStateFilesystemLocation([In] uint Flags, [In] IDefinitionAppId Appidentity, [In] IDefinitionIdentity ComponentIdentity, [In] UIntPtr Coordinates, [MarshalAs(UnmanagedType.LPWStr)] out string Path);

		// Token: 0x06000473 RID: 1139
		void Scavenge([In] uint Flags, out uint Disposition);
	}
}
