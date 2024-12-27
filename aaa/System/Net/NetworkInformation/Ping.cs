using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000623 RID: 1571
	public class Ping : Component, IDisposable
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06003040 RID: 12352 RVA: 0x000D027F File Offset: 0x000CF27F
		// (remove) Token: 0x06003041 RID: 12353 RVA: 0x000D0298 File Offset: 0x000CF298
		public event PingCompletedEventHandler PingCompleted;

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x000D02B1 File Offset: 0x000CF2B1
		// (set) Token: 0x06003043 RID: 12355 RVA: 0x000D02CC File Offset: 0x000CF2CC
		private bool InAsyncCall
		{
			get
			{
				return this.asyncFinished != null && !this.asyncFinished.WaitOne(0);
			}
			set
			{
				if (this.asyncFinished == null)
				{
					this.asyncFinished = new ManualResetEvent(!value);
					return;
				}
				if (value)
				{
					this.asyncFinished.Reset();
					return;
				}
				this.asyncFinished.Set();
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000D0304 File Offset: 0x000CF304
		private void CheckStart(bool async)
		{
			if (this.disposeRequested)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			int num = Interlocked.CompareExchange(ref this.status, 1, 0);
			if (num == 1)
			{
				throw new InvalidOperationException(SR.GetString("net_inasync"));
			}
			if (num == 2)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (async)
			{
				this.InAsyncCall = true;
			}
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x000D036B File Offset: 0x000CF36B
		private void Finish(bool async)
		{
			this.status = 0;
			if (async)
			{
				this.InAsyncCall = false;
			}
			if (this.disposeRequested)
			{
				this.InternalDispose();
			}
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x000D038C File Offset: 0x000CF38C
		protected void OnPingCompleted(PingCompletedEventArgs e)
		{
			if (this.PingCompleted != null)
			{
				this.PingCompleted(this, e);
			}
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x000D03A3 File Offset: 0x000CF3A3
		private void PingCompletedWaitCallback(object operationState)
		{
			this.OnPingCompleted((PingCompletedEventArgs)operationState);
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x000D03B1 File Offset: 0x000CF3B1
		public Ping()
		{
			this.onPingCompletedDelegate = new SendOrPostCallback(this.PingCompletedWaitCallback);
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x000D03CC File Offset: 0x000CF3CC
		private void InternalDispose()
		{
			this.disposeRequested = true;
			if (Interlocked.CompareExchange(ref this.status, 2, 0) != 0)
			{
				return;
			}
			if (this.pingSocket != null)
			{
				this.pingSocket.Close();
				this.pingSocket = null;
			}
			if (this.handlePingV4 != null)
			{
				this.handlePingV4.Close();
				this.handlePingV4 = null;
			}
			if (this.handlePingV6 != null)
			{
				this.handlePingV6.Close();
				this.handlePingV6 = null;
			}
			if (this.registeredWait != null)
			{
				this.registeredWait.Unregister(null);
			}
			if (this.pingEvent != null)
			{
				this.pingEvent.Close();
			}
			if (this.replyBuffer != null)
			{
				this.replyBuffer.Close();
			}
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x000D0479 File Offset: 0x000CF479
		void IDisposable.Dispose()
		{
			this.InternalDispose();
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000D0484 File Offset: 0x000CF484
		public void SendAsyncCancel()
		{
			lock (this)
			{
				if (!this.InAsyncCall)
				{
					return;
				}
				this.cancelled = true;
				if (this.pingSocket != null)
				{
					this.pingSocket.Close();
					this.pingSocket = null;
				}
			}
			this.asyncFinished.WaitOne();
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000D04EC File Offset: 0x000CF4EC
		private static void PingCallback(object state, bool signaled)
		{
			Ping ping = (Ping)state;
			PingCompletedEventArgs pingCompletedEventArgs = null;
			bool flag = false;
			AsyncOperation asyncOperation = null;
			SendOrPostCallback sendOrPostCallback = null;
			try
			{
				lock (ping)
				{
					flag = ping.cancelled;
					asyncOperation = ping.asyncOp;
					sendOrPostCallback = ping.onPingCompletedDelegate;
					if (!flag)
					{
						SafeLocalFree safeLocalFree = ping.replyBuffer;
						if (ping.ipv6)
						{
							UnsafeNetInfoNativeMethods.Icmp6ParseReplies(safeLocalFree.DangerousGetHandle(), 65791U);
						}
						else if (ComNetOS.IsPostWin2K)
						{
							UnsafeNetInfoNativeMethods.IcmpParseReplies(safeLocalFree.DangerousGetHandle(), 65791U);
						}
						else
						{
							UnsafeIcmpNativeMethods.IcmpParseReplies(safeLocalFree.DangerousGetHandle(), 65791U);
						}
						PingReply pingReply;
						if (ping.ipv6)
						{
							Icmp6EchoReply icmp6EchoReply = (Icmp6EchoReply)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(Icmp6EchoReply));
							pingReply = new PingReply(icmp6EchoReply, safeLocalFree.DangerousGetHandle(), ping.sendSize);
						}
						else
						{
							IcmpEchoReply icmpEchoReply = (IcmpEchoReply)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IcmpEchoReply));
							pingReply = new PingReply(icmpEchoReply);
						}
						pingCompletedEventArgs = new PingCompletedEventArgs(pingReply, null, false, asyncOperation.UserSuppliedState);
					}
				}
			}
			catch (Exception ex)
			{
				PingException ex2 = new PingException(SR.GetString("net_ping"), ex);
				pingCompletedEventArgs = new PingCompletedEventArgs(null, ex2, false, asyncOperation.UserSuppliedState);
			}
			catch
			{
				PingException ex3 = new PingException(SR.GetString("net_ping"), new Exception(SR.GetString("net_nonClsCompliantException")));
				pingCompletedEventArgs = new PingCompletedEventArgs(null, ex3, false, asyncOperation.UserSuppliedState);
			}
			finally
			{
				ping.FreeUnmanagedStructures();
				ping.Finish(true);
			}
			if (flag)
			{
				pingCompletedEventArgs = new PingCompletedEventArgs(null, null, true, asyncOperation.UserSuppliedState);
			}
			asyncOperation.PostOperationCompleted(sendOrPostCallback, pingCompletedEventArgs);
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000D06E8 File Offset: 0x000CF6E8
		private static void PingSendCallback(IAsyncResult result)
		{
			Ping ping = (Ping)result.AsyncState;
			PingCompletedEventArgs pingCompletedEventArgs = null;
			try
			{
				ping.pingSocket.EndSendTo(result);
				PingReply pingReply = null;
				if (!ping.cancelled)
				{
					EndPoint endPoint = new IPEndPoint(0L, 0);
					int num;
					do
					{
						num = ping.pingSocket.ReceiveFrom(ping.downlevelReplyBuffer, ref endPoint);
						if (Ping.CorrectPacket(ping.downlevelReplyBuffer, ping.packet))
						{
							goto IL_007B;
						}
					}
					while (Environment.TickCount - ping.startTime <= ping.llTimeout);
					pingReply = new PingReply(IPStatus.TimedOut);
					IL_007B:
					int num2 = Environment.TickCount - ping.startTime;
					if (pingReply == null)
					{
						pingReply = new PingReply(ping.downlevelReplyBuffer, num, ((IPEndPoint)endPoint).Address, num2);
					}
					pingCompletedEventArgs = new PingCompletedEventArgs(pingReply, null, false, ping.asyncOp.UserSuppliedState);
				}
			}
			catch (Exception ex)
			{
				PingReply pingReply2 = null;
				PingException ex2 = null;
				SocketException ex3 = ex as SocketException;
				if (ex3 != null)
				{
					if (ex3.ErrorCode == 10060)
					{
						pingReply2 = new PingReply(IPStatus.TimedOut);
					}
					else if (ex3.ErrorCode == 10040)
					{
						pingReply2 = new PingReply(IPStatus.PacketTooBig);
					}
				}
				if (pingReply2 == null)
				{
					ex2 = new PingException(SR.GetString("net_ping"), ex);
				}
				pingCompletedEventArgs = new PingCompletedEventArgs(pingReply2, ex2, false, ping.asyncOp.UserSuppliedState);
			}
			catch
			{
				PingException ex4 = new PingException(SR.GetString("net_ping"), new Exception(SR.GetString("net_nonClsCompliantException")));
				pingCompletedEventArgs = new PingCompletedEventArgs(null, ex4, false, ping.asyncOp.UserSuppliedState);
			}
			try
			{
				if (ping.cancelled)
				{
					pingCompletedEventArgs = new PingCompletedEventArgs(null, null, true, ping.asyncOp.UserSuppliedState);
				}
				ping.asyncOp.PostOperationCompleted(ping.onPingCompletedDelegate, pingCompletedEventArgs);
			}
			finally
			{
				ping.Finish(true);
			}
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000D08C8 File Offset: 0x000CF8C8
		public PingReply Send(string hostNameOrAddress)
		{
			return this.Send(hostNameOrAddress, 5000, this.DefaultSendBuffer, null);
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000D08DD File Offset: 0x000CF8DD
		public PingReply Send(string hostNameOrAddress, int timeout)
		{
			return this.Send(hostNameOrAddress, timeout, this.DefaultSendBuffer, null);
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000D08EE File Offset: 0x000CF8EE
		public PingReply Send(IPAddress address)
		{
			return this.Send(address, 5000, this.DefaultSendBuffer, null);
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000D0903 File Offset: 0x000CF903
		public PingReply Send(IPAddress address, int timeout)
		{
			return this.Send(address, timeout, this.DefaultSendBuffer, null);
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000D0914 File Offset: 0x000CF914
		public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer)
		{
			return this.Send(hostNameOrAddress, timeout, buffer, null);
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000D0920 File Offset: 0x000CF920
		public PingReply Send(IPAddress address, int timeout, byte[] buffer)
		{
			return this.Send(address, timeout, buffer, null);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000D092C File Offset: 0x000CF92C
		public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options)
		{
			if (ValidationHelper.IsBlankString(hostNameOrAddress))
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			IPAddress ipaddress;
			try
			{
				ipaddress = Dns.GetHostAddresses(hostNameOrAddress)[0];
			}
			catch (ArgumentException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new PingException(SR.GetString("net_ping"), ex);
			}
			return this.Send(ipaddress, timeout, buffer, options);
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000D0994 File Offset: 0x000CF994
		public PingReply Send(IPAddress address, int timeout, byte[] buffer, PingOptions options)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 65500)
			{
				throw new ArgumentException(SR.GetString("net_invalidPingBufferSize"), "buffer");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
			{
				throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "address");
			}
			IPAddress ipaddress;
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				ipaddress = new IPAddress(address.GetAddressBytes());
			}
			else
			{
				ipaddress = new IPAddress(address.GetAddressBytes(), address.ScopeId);
			}
			new NetworkInformationPermission(NetworkInformationAccess.Ping).Demand();
			this.CheckStart(false);
			PingReply pingReply;
			try
			{
				pingReply = this.InternalSend(ipaddress, buffer, timeout, options, false);
			}
			catch (Exception ex)
			{
				throw new PingException(SR.GetString("net_ping"), ex);
			}
			catch
			{
				throw new PingException(SR.GetString("net_ping"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			finally
			{
				this.Finish(false);
			}
			return pingReply;
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000D0AC8 File Offset: 0x000CFAC8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, object userToken)
		{
			this.SendAsync(hostNameOrAddress, 5000, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x000D0ADD File Offset: 0x000CFADD
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, int timeout, object userToken)
		{
			this.SendAsync(hostNameOrAddress, timeout, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x000D0AEE File Offset: 0x000CFAEE
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, object userToken)
		{
			this.SendAsync(address, 5000, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x000D0B03 File Offset: 0x000CFB03
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, int timeout, object userToken)
		{
			this.SendAsync(address, timeout, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x000D0B14 File Offset: 0x000CFB14
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, object userToken)
		{
			this.SendAsync(hostNameOrAddress, timeout, buffer, null, userToken);
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x000D0B22 File Offset: 0x000CFB22
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, int timeout, byte[] buffer, object userToken)
		{
			this.SendAsync(address, timeout, buffer, null, userToken);
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000D0B30 File Offset: 0x000CFB30
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options, object userToken)
		{
			if (ValidationHelper.IsBlankString(hostNameOrAddress))
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 65500)
			{
				throw new ArgumentException(SR.GetString("net_invalidPingBufferSize"), "buffer");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			IPAddress ipaddress;
			if (IPAddress.TryParse(hostNameOrAddress, out ipaddress))
			{
				this.SendAsync(ipaddress, timeout, buffer, options, userToken);
				return;
			}
			this.CheckStart(true);
			try
			{
				this.asyncOp = AsyncOperationManager.CreateOperation(userToken);
				Ping.AsyncStateObject asyncStateObject = new Ping.AsyncStateObject(hostNameOrAddress, buffer, timeout, options, userToken);
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ContinueAsyncSend), asyncStateObject);
			}
			catch (Exception ex)
			{
				this.Finish(true);
				throw new PingException(SR.GetString("net_ping"), ex);
			}
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000D0C04 File Offset: 0x000CFC04
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, int timeout, byte[] buffer, PingOptions options, object userToken)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 65500)
			{
				throw new ArgumentException(SR.GetString("net_invalidPingBufferSize"), "buffer");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
			{
				throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "address");
			}
			IPAddress ipaddress;
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				ipaddress = new IPAddress(address.GetAddressBytes());
			}
			else
			{
				ipaddress = new IPAddress(address.GetAddressBytes(), address.ScopeId);
			}
			new NetworkInformationPermission(NetworkInformationAccess.Ping).Demand();
			this.CheckStart(true);
			try
			{
				this.asyncOp = AsyncOperationManager.CreateOperation(userToken);
				this.InternalSend(ipaddress, buffer, timeout, options, true);
			}
			catch (Exception ex)
			{
				this.Finish(true);
				throw new PingException(SR.GetString("net_ping"), ex);
			}
			catch
			{
				this.Finish(true);
				throw new PingException(SR.GetString("net_ping"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000D0D40 File Offset: 0x000CFD40
		private void ContinueAsyncSend(object state)
		{
			Ping.AsyncStateObject asyncStateObject = (Ping.AsyncStateObject)state;
			try
			{
				IPAddress ipaddress = Dns.GetHostAddresses(asyncStateObject.hostName)[0];
				new NetworkInformationPermission(NetworkInformationAccess.Ping).Demand();
				this.InternalSend(ipaddress, asyncStateObject.buffer, asyncStateObject.timeout, asyncStateObject.options, true);
			}
			catch (Exception ex)
			{
				PingException ex2 = new PingException(SR.GetString("net_ping"), ex);
				PingCompletedEventArgs pingCompletedEventArgs = new PingCompletedEventArgs(null, ex2, false, this.asyncOp.UserSuppliedState);
				this.Finish(true);
				this.asyncOp.PostOperationCompleted(this.onPingCompletedDelegate, pingCompletedEventArgs);
			}
			catch
			{
				PingException ex3 = new PingException(SR.GetString("net_ping"), new Exception(SR.GetString("net_nonClsCompliantException")));
				PingCompletedEventArgs pingCompletedEventArgs2 = new PingCompletedEventArgs(null, ex3, false, this.asyncOp.UserSuppliedState);
				this.Finish(true);
				this.asyncOp.PostOperationCompleted(this.onPingCompletedDelegate, pingCompletedEventArgs2);
			}
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000D0E40 File Offset: 0x000CFE40
		private PingReply InternalSend(IPAddress address, byte[] buffer, int timeout, PingOptions options, bool async)
		{
			this.cancelled = false;
			if (address.AddressFamily == AddressFamily.InterNetworkV6 && !ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			if (!ComNetOS.IsWin2K)
			{
				return this.InternalDownLevelSend(address, buffer, timeout, options, async);
			}
			this.ipv6 = address.AddressFamily == AddressFamily.InterNetworkV6;
			this.sendSize = buffer.Length;
			if (!this.ipv6 && this.handlePingV4 == null)
			{
				if (ComNetOS.IsPostWin2K)
				{
					this.handlePingV4 = UnsafeNetInfoNativeMethods.IcmpCreateFile();
				}
				else
				{
					this.handlePingV4 = UnsafeIcmpNativeMethods.IcmpCreateFile();
				}
			}
			else if (this.ipv6 && this.handlePingV6 == null)
			{
				this.handlePingV6 = UnsafeNetInfoNativeMethods.Icmp6CreateFile();
			}
			IPOptions ipoptions = new IPOptions(options);
			if (this.replyBuffer == null)
			{
				this.replyBuffer = SafeLocalFree.LocalAlloc(65791);
			}
			if (this.registeredWait != null)
			{
				this.registeredWait.Unregister(null);
				this.registeredWait = null;
			}
			int num;
			try
			{
				if (async)
				{
					if (this.pingEvent == null)
					{
						this.pingEvent = new ManualResetEvent(false);
					}
					else
					{
						this.pingEvent.Reset();
					}
					this.registeredWait = ThreadPool.RegisterWaitForSingleObject(this.pingEvent, new WaitOrTimerCallback(Ping.PingCallback), this, -1, true);
				}
				this.SetUnmanagedStructures(buffer);
				if (!this.ipv6)
				{
					if (ComNetOS.IsPostWin2K)
					{
						if (async)
						{
							num = (int)UnsafeNetInfoNativeMethods.IcmpSendEcho2(this.handlePingV4, this.pingEvent.SafeWaitHandle, IntPtr.Zero, IntPtr.Zero, (uint)address.m_Address, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
						}
						else
						{
							num = (int)UnsafeNetInfoNativeMethods.IcmpSendEcho2(this.handlePingV4, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)address.m_Address, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
						}
					}
					else if (async)
					{
						num = (int)UnsafeIcmpNativeMethods.IcmpSendEcho2(this.handlePingV4, this.pingEvent.SafeWaitHandle, IntPtr.Zero, IntPtr.Zero, (uint)address.m_Address, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
					else
					{
						num = (int)UnsafeIcmpNativeMethods.IcmpSendEcho2(this.handlePingV4, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)address.m_Address, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
				}
				else
				{
					IPEndPoint ipendPoint = new IPEndPoint(address, 0);
					SocketAddress socketAddress = ipendPoint.Serialize();
					byte[] array = new byte[28];
					if (async)
					{
						num = (int)UnsafeNetInfoNativeMethods.Icmp6SendEcho2(this.handlePingV6, this.pingEvent.SafeWaitHandle, IntPtr.Zero, IntPtr.Zero, array, socketAddress.m_Buffer, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
					else
					{
						num = (int)UnsafeNetInfoNativeMethods.Icmp6SendEcho2(this.handlePingV6, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, array, socketAddress.m_Buffer, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
				}
			}
			catch
			{
				if (this.registeredWait != null)
				{
					this.registeredWait.Unregister(null);
				}
				throw;
			}
			if (num == 0)
			{
				num = Marshal.GetLastWin32Error();
				if (num != 0)
				{
					this.FreeUnmanagedStructures();
					return new PingReply((IPStatus)num);
				}
			}
			if (async)
			{
				return null;
			}
			this.FreeUnmanagedStructures();
			PingReply pingReply;
			if (this.ipv6)
			{
				Icmp6EchoReply icmp6EchoReply = (Icmp6EchoReply)Marshal.PtrToStructure(this.replyBuffer.DangerousGetHandle(), typeof(Icmp6EchoReply));
				pingReply = new PingReply(icmp6EchoReply, this.replyBuffer.DangerousGetHandle(), this.sendSize);
			}
			else
			{
				IcmpEchoReply icmpEchoReply = (IcmpEchoReply)Marshal.PtrToStructure(this.replyBuffer.DangerousGetHandle(), typeof(IcmpEchoReply));
				pingReply = new PingReply(icmpEchoReply);
			}
			GC.KeepAlive(this.replyBuffer);
			return pingReply;
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000D1218 File Offset: 0x000D0218
		private PingReply InternalDownLevelSend(IPAddress address, byte[] buffer, int timeout, PingOptions options, bool async)
		{
			PingReply pingReply;
			try
			{
				if (options == null)
				{
					options = new PingOptions();
				}
				if (this.downlevelReplyBuffer == null)
				{
					this.downlevelReplyBuffer = new byte[64000];
				}
				this.llTimeout = timeout;
				this.packet = new IcmpPacket(buffer);
				byte[] bytes = this.packet.GetBytes();
				IPEndPoint ipendPoint = new IPEndPoint(address, 0);
				EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
				if (this.pingSocket == null)
				{
					this.pingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
				}
				this.pingSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout);
				this.pingSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timeout);
				this.pingSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, options.Ttl);
				this.pingSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, options.DontFragment);
				this.startTime = Environment.TickCount;
				if (async)
				{
					this.pingSocket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, ipendPoint, new AsyncCallback(Ping.PingSendCallback), this);
					pingReply = null;
				}
				else
				{
					this.pingSocket.SendTo(bytes, bytes.Length, SocketFlags.None, ipendPoint);
					int num;
					do
					{
						num = this.pingSocket.ReceiveFrom(this.downlevelReplyBuffer, ref endPoint);
						if (Ping.CorrectPacket(this.downlevelReplyBuffer, this.packet))
						{
							goto IL_015B;
						}
					}
					while (Environment.TickCount - this.startTime <= this.llTimeout);
					return new PingReply(IPStatus.TimedOut);
					IL_015B:
					int num2 = Environment.TickCount - this.startTime;
					pingReply = new PingReply(this.downlevelReplyBuffer, num, ((IPEndPoint)endPoint).Address, num2);
				}
			}
			catch (SocketException ex)
			{
				if (ex.ErrorCode == 10060)
				{
					pingReply = new PingReply(IPStatus.TimedOut);
				}
				else
				{
					if (ex.ErrorCode != 10040)
					{
						throw ex;
					}
					PingReply pingReply2 = new PingReply(IPStatus.PacketTooBig);
					if (!async)
					{
						pingReply = pingReply2;
					}
					else
					{
						PingCompletedEventArgs pingCompletedEventArgs = new PingCompletedEventArgs(pingReply2, null, false, this.asyncOp.UserSuppliedState);
						this.asyncOp.PostOperationCompleted(this.onPingCompletedDelegate, pingCompletedEventArgs);
						pingReply = null;
					}
				}
			}
			return pingReply;
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000D143C File Offset: 0x000D043C
		private unsafe void SetUnmanagedStructures(byte[] buffer)
		{
			this.requestBuffer = SafeLocalFree.LocalAlloc(buffer.Length);
			byte* ptr = (byte*)(void*)this.requestBuffer.DangerousGetHandle();
			for (int i = 0; i < buffer.Length; i++)
			{
				ptr[i] = buffer[i];
			}
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000D147D File Offset: 0x000D047D
		private void FreeUnmanagedStructures()
		{
			if (this.requestBuffer != null)
			{
				this.requestBuffer.Close();
				this.requestBuffer = null;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x000D149C File Offset: 0x000D049C
		private byte[] DefaultSendBuffer
		{
			get
			{
				if (this.defaultSendBuffer == null)
				{
					this.defaultSendBuffer = new byte[32];
					for (int i = 0; i < 32; i++)
					{
						this.defaultSendBuffer[i] = (byte)(97 + i % 23);
					}
				}
				return this.defaultSendBuffer;
			}
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000D14E4 File Offset: 0x000D04E4
		internal static bool CorrectPacket(byte[] buffer, IcmpPacket packet)
		{
			if (buffer[20] == 0 && buffer[21] == 0)
			{
				if ((((int)buffer[25] << 8) | (int)buffer[24]) == (int)packet.Identifier && (((int)buffer[27] << 8) | (int)buffer[26]) == (int)packet.sequenceNumber)
				{
					return true;
				}
			}
			else if ((((int)buffer[53] << 8) | (int)buffer[52]) == (int)packet.Identifier && (((int)buffer[55] << 8) | (int)buffer[54]) == (int)packet.sequenceNumber)
			{
				return true;
			}
			return false;
		}

		// Token: 0x04002DF1 RID: 11761
		private const int MaxUdpPacket = 65791;

		// Token: 0x04002DF2 RID: 11762
		private const int MaxBufferSize = 65500;

		// Token: 0x04002DF3 RID: 11763
		private const int DefaultTimeout = 5000;

		// Token: 0x04002DF4 RID: 11764
		private const int DefaultSendBufferSize = 32;

		// Token: 0x04002DF5 RID: 11765
		private const int TimeoutErrorCode = 10060;

		// Token: 0x04002DF6 RID: 11766
		private const int PacketTooBigErrorCode = 10040;

		// Token: 0x04002DF7 RID: 11767
		private const int Free = 0;

		// Token: 0x04002DF8 RID: 11768
		private const int InProgress = 1;

		// Token: 0x04002DF9 RID: 11769
		private new const int Disposed = 2;

		// Token: 0x04002DFA RID: 11770
		private byte[] defaultSendBuffer;

		// Token: 0x04002DFB RID: 11771
		private bool ipv6;

		// Token: 0x04002DFC RID: 11772
		private bool cancelled;

		// Token: 0x04002DFD RID: 11773
		private bool disposeRequested;

		// Token: 0x04002DFE RID: 11774
		internal ManualResetEvent pingEvent;

		// Token: 0x04002DFF RID: 11775
		private RegisteredWaitHandle registeredWait;

		// Token: 0x04002E00 RID: 11776
		private SafeLocalFree requestBuffer;

		// Token: 0x04002E01 RID: 11777
		private SafeLocalFree replyBuffer;

		// Token: 0x04002E02 RID: 11778
		private int sendSize;

		// Token: 0x04002E03 RID: 11779
		private Socket pingSocket;

		// Token: 0x04002E04 RID: 11780
		private byte[] downlevelReplyBuffer;

		// Token: 0x04002E05 RID: 11781
		private SafeCloseIcmpHandle handlePingV4;

		// Token: 0x04002E06 RID: 11782
		private SafeCloseIcmpHandle handlePingV6;

		// Token: 0x04002E07 RID: 11783
		private int startTime;

		// Token: 0x04002E08 RID: 11784
		private IcmpPacket packet;

		// Token: 0x04002E09 RID: 11785
		private int llTimeout;

		// Token: 0x04002E0A RID: 11786
		private AsyncOperation asyncOp;

		// Token: 0x04002E0B RID: 11787
		private SendOrPostCallback onPingCompletedDelegate;

		// Token: 0x04002E0D RID: 11789
		private ManualResetEvent asyncFinished;

		// Token: 0x04002E0E RID: 11790
		private int status;

		// Token: 0x02000624 RID: 1572
		internal class AsyncStateObject
		{
			// Token: 0x06003065 RID: 12389 RVA: 0x000D154E File Offset: 0x000D054E
			internal AsyncStateObject(string hostName, byte[] buffer, int timeout, PingOptions options, object userToken)
			{
				this.hostName = hostName;
				this.buffer = buffer;
				this.timeout = timeout;
				this.options = options;
				this.userToken = userToken;
			}

			// Token: 0x04002E0F RID: 11791
			internal byte[] buffer;

			// Token: 0x04002E10 RID: 11792
			internal string hostName;

			// Token: 0x04002E11 RID: 11793
			internal int timeout;

			// Token: 0x04002E12 RID: 11794
			internal PingOptions options;

			// Token: 0x04002E13 RID: 11795
			internal object userToken;
		}
	}
}
