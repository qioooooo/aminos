using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200052A RID: 1322
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseSocketAndEvent : SafeCloseSocket
	{
		// Token: 0x06002885 RID: 10373 RVA: 0x000A7A5B File Offset: 0x000A6A5B
		internal SafeCloseSocketAndEvent()
		{
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000A7A64 File Offset: 0x000A6A64
		protected override bool ReleaseHandle()
		{
			bool flag = base.ReleaseHandle();
			this.DeleteEvent();
			return flag;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000A7A80 File Offset: 0x000A6A80
		internal static SafeCloseSocketAndEvent CreateWSASocketWithEvent(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, bool autoReset, bool signaled)
		{
			SafeCloseSocketAndEvent safeCloseSocketAndEvent = new SafeCloseSocketAndEvent();
			SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.CreateWSASocket(addressFamily, socketType, protocolType), safeCloseSocketAndEvent);
			if (safeCloseSocketAndEvent.IsInvalid)
			{
				throw new SocketException();
			}
			safeCloseSocketAndEvent.waitHandle = new AutoResetEvent(false);
			SafeCloseSocketAndEvent.CompleteInitialization(safeCloseSocketAndEvent);
			return safeCloseSocketAndEvent;
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000A7AC4 File Offset: 0x000A6AC4
		internal static void CompleteInitialization(SafeCloseSocketAndEvent socketAndEventHandle)
		{
			SafeWaitHandle safeWaitHandle = socketAndEventHandle.waitHandle.SafeWaitHandle;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				safeWaitHandle.DangerousAddRef(ref flag);
			}
			catch
			{
				if (flag)
				{
					safeWaitHandle.DangerousRelease();
					socketAndEventHandle.waitHandle = null;
					flag = false;
				}
			}
			finally
			{
				if (flag)
				{
					safeWaitHandle.Dispose();
				}
			}
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000A7B2C File Offset: 0x000A6B2C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void DeleteEvent()
		{
			try
			{
				if (this.waitHandle != null)
				{
					this.waitHandle.SafeWaitHandle.DangerousRelease();
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x000A7B68 File Offset: 0x000A6B68
		internal WaitHandle GetEventHandle()
		{
			return this.waitHandle;
		}

		// Token: 0x04002789 RID: 10121
		private AutoResetEvent waitHandle;
	}
}
