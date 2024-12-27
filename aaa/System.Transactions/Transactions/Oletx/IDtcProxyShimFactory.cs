using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000085 RID: 133
	[Guid("467C8BCB-BDDE-4885-B143-317107468275")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface IDtcProxyShimFactory
	{
		// Token: 0x06000361 RID: 865
		void ConnectToProxy([MarshalAs(UnmanagedType.LPWStr)] string nodeName, Guid resourceManagerIdentifier, IntPtr managedIdentifier, [MarshalAs(UnmanagedType.Bool)] out bool nodeNameMatches, [MarshalAs(UnmanagedType.U4)] out uint whereaboutsSize, out CoTaskMemHandle whereaboutsBuffer, [MarshalAs(UnmanagedType.Interface)] out IResourceManagerShim resourceManagerShim);

		// Token: 0x06000362 RID: 866
		void GetNotification(out IntPtr managedIdentifier, [MarshalAs(UnmanagedType.I4)] out ShimNotificationType shimNotificationType, [MarshalAs(UnmanagedType.Bool)] out bool isSinglePhase, [MarshalAs(UnmanagedType.Bool)] out bool abortingHint, [MarshalAs(UnmanagedType.Bool)] out bool releaseRequired, [MarshalAs(UnmanagedType.U4)] out uint prepareInfoSize, out CoTaskMemHandle prepareInfo);

		// Token: 0x06000363 RID: 867
		void ReleaseNotificationLock();

		// Token: 0x06000364 RID: 868
		void BeginTransaction([MarshalAs(UnmanagedType.U4)] uint timeout, OletxTransactionIsolationLevel isolationLevel, IntPtr managedIdentifier, out Guid transactionIdentifier, [MarshalAs(UnmanagedType.Interface)] out ITransactionShim transactionShim);

		// Token: 0x06000365 RID: 869
		void CreateResourceManager(Guid resourceManagerIdentifier, IntPtr managedIdentifier, [MarshalAs(UnmanagedType.Interface)] out IResourceManagerShim resourceManagerShim);

		// Token: 0x06000366 RID: 870
		void Import([MarshalAs(UnmanagedType.U4)] uint cookieSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] cookie, IntPtr managedIdentifier, out Guid transactionIdentifier, out OletxTransactionIsolationLevel isolationLevel, [MarshalAs(UnmanagedType.Interface)] out ITransactionShim transactionShim);

		// Token: 0x06000367 RID: 871
		void ReceiveTransaction([MarshalAs(UnmanagedType.U4)] uint propgationTokenSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] propgationToken, IntPtr managedIdentifier, out Guid transactionIdentifier, out OletxTransactionIsolationLevel isolationLevel, [MarshalAs(UnmanagedType.Interface)] out ITransactionShim transactionShim);

		// Token: 0x06000368 RID: 872
		void CreateTransactionShim([MarshalAs(UnmanagedType.Interface)] IDtcTransaction transactionNative, IntPtr managedIdentifier, out Guid transactionIdentifier, out OletxTransactionIsolationLevel isolationLevel, [MarshalAs(UnmanagedType.Interface)] out ITransactionShim transactionShim);
	}
}
