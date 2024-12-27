using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000235 RID: 565
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("07662534-750b-4ed5-9cfb-1c5bc5acfd07")]
	[ComImport]
	internal interface IStateManager
	{
		// Token: 0x060015DC RID: 5596
		void PrepareApplicationState([In] UIntPtr Inputs, ref UIntPtr Outputs);

		// Token: 0x060015DD RID: 5597
		void SetApplicationRunningState([In] uint Flags, [In] IActContext Context, [In] uint RunningState, out uint Disposition);

		// Token: 0x060015DE RID: 5598
		void GetApplicationStateFilesystemLocation([In] uint Flags, [In] IDefinitionAppId Appidentity, [In] IDefinitionIdentity ComponentIdentity, [In] UIntPtr Coordinates, [MarshalAs(UnmanagedType.LPWStr)] out string Path);

		// Token: 0x060015DF RID: 5599
		void Scavenge([In] uint Flags, out uint Disposition);
	}
}
