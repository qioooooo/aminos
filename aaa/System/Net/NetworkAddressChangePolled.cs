using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020003EF RID: 1007
	internal class NetworkAddressChangePolled : IDisposable
	{
		// Token: 0x0600208A RID: 8330 RVA: 0x000804A8 File Offset: 0x0007F4A8
		internal NetworkAddressChangePolled()
		{
			Socket.InitializeSockets();
			if (Socket.SupportsIPv4)
			{
				int num = -1;
				this.ipv4Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP, true, false);
				UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.ipv4Socket, -2147195266, ref num);
			}
			if (Socket.OSSupportsIPv6)
			{
				int num = -1;
				this.ipv6Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.IP, true, false);
				UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.ipv6Socket, -2147195266, ref num);
			}
			this.Setup(StartIPOptions.Both);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00080520 File Offset: 0x0007F520
		private void Setup(StartIPOptions startIPOptions)
		{
			if (Socket.SupportsIPv4 && (startIPOptions & StartIPOptions.StartIPv4) != StartIPOptions.None)
			{
				int num;
				SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(this.ipv4Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num, SafeNativeOverlapped.Zero, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					NetworkInformationException ex = new NetworkInformationException();
					if ((long)ex.ErrorCode != 10035L)
					{
						this.Dispose();
						return;
					}
				}
				socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.ipv4Socket, this.ipv4Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
				if (socketError != SocketError.Success)
				{
					this.Dispose();
					return;
				}
			}
			if (Socket.OSSupportsIPv6 && (startIPOptions & StartIPOptions.StartIPv6) != StartIPOptions.None)
			{
				int num;
				SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(this.ipv6Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num, SafeNativeOverlapped.Zero, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					NetworkInformationException ex2 = new NetworkInformationException();
					if ((long)ex2.ErrorCode != 10035L)
					{
						this.Dispose();
						return;
					}
				}
				socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.ipv6Socket, this.ipv6Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
				if (socketError != SocketError.Success)
				{
					this.Dispose();
				}
			}
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x00080624 File Offset: 0x0007F624
		internal bool CheckAndReset()
		{
			if (!this.disposed)
			{
				lock (this)
				{
					if (!this.disposed)
					{
						StartIPOptions startIPOptions = StartIPOptions.None;
						if (this.ipv4Socket != null && this.ipv4Socket.GetEventHandle().WaitOne(0, false))
						{
							startIPOptions |= StartIPOptions.StartIPv4;
						}
						if (this.ipv6Socket != null && this.ipv6Socket.GetEventHandle().WaitOne(0, false))
						{
							startIPOptions |= StartIPOptions.StartIPv6;
						}
						if (startIPOptions != StartIPOptions.None)
						{
							this.Setup(startIPOptions);
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000806B8 File Offset: 0x0007F6B8
		public void Dispose()
		{
			if (!this.disposed)
			{
				lock (this)
				{
					if (!this.disposed)
					{
						if (this.ipv6Socket != null)
						{
							this.ipv6Socket.Close();
							this.ipv6Socket = null;
						}
						if (this.ipv4Socket != null)
						{
							this.ipv4Socket.Close();
							this.ipv6Socket = null;
						}
						this.disposed = true;
					}
				}
			}
		}

		// Token: 0x04001FBD RID: 8125
		private bool disposed;

		// Token: 0x04001FBE RID: 8126
		private SafeCloseSocketAndEvent ipv4Socket;

		// Token: 0x04001FBF RID: 8127
		private SafeCloseSocketAndEvent ipv6Socket;
	}
}
