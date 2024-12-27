using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000059 RID: 89
	internal class IpcPort : IDisposable
	{
		// Token: 0x060002CB RID: 715 RVA: 0x0000DCBC File Offset: 0x0000CCBC
		private IpcPort(string portName, PipeHandle handle)
		{
			this._portName = portName;
			this._handle = handle;
			this._cacheable = true;
			ThreadPool.BindHandle(this._handle.Handle);
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000DCEA File Offset: 0x0000CCEA
		internal string Name
		{
			get
			{
				return this._portName;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000DCF2 File Offset: 0x0000CCF2
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000DCFA File Offset: 0x0000CCFA
		internal bool Cacheable
		{
			get
			{
				return this._cacheable;
			}
			set
			{
				this._cacheable = value;
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000DD04 File Offset: 0x0000CD04
		internal static CommonSecurityDescriptor CreateSecurityDescriptor(SecurityIdentifier userSid)
		{
			SecurityIdentifier securityIdentifier = new SecurityIdentifier("S-1-5-2");
			DiscretionaryAcl discretionaryAcl = new DiscretionaryAcl(false, false, 1);
			discretionaryAcl.AddAccess(AccessControlType.Deny, securityIdentifier, -1, InheritanceFlags.None, PropagationFlags.None);
			if (userSid != null)
			{
				discretionaryAcl.AddAccess(AccessControlType.Allow, userSid, -1, InheritanceFlags.None, PropagationFlags.None);
			}
			discretionaryAcl.AddAccess(AccessControlType.Allow, WindowsIdentity.GetCurrent().User, -1, InheritanceFlags.None, PropagationFlags.None);
			return new CommonSecurityDescriptor(false, false, ControlFlags.OwnerDefaulted | ControlFlags.GroupDefaulted | ControlFlags.DiscretionaryAclPresent, null, null, null, discretionaryAcl);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000DD64 File Offset: 0x0000CD64
		internal static IpcPort Create(string portName, CommonSecurityDescriptor securityDescriptor, bool exclusive)
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new NotSupportedException(CoreChannel.GetResourceString("Remoting_Ipc_Win9x"));
			}
			string text = "\\\\.\\pipe\\" + portName;
			SECURITY_ATTRIBUTES security_ATTRIBUTES = new SECURITY_ATTRIBUTES();
			security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
			if (securityDescriptor == null)
			{
				securityDescriptor = IpcPort.s_securityDescriptor;
			}
			byte[] array = new byte[securityDescriptor.BinaryLength];
			securityDescriptor.GetBinaryForm(array, 0);
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			security_ATTRIBUTES.lpSecurityDescriptor = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
			PipeHandle pipeHandle = NativePipe.CreateNamedPipe(text, 1073741827U | (exclusive ? 524288U : 0U), 0U, 255U, 8192U, 8192U, uint.MaxValue, security_ATTRIBUTES);
			gchandle.Free();
			if (pipeHandle.Handle.ToInt32() == -1)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_CreateIpcFailed"), new object[] { IpcPort.GetMessage(lastWin32Error) }));
			}
			return new IpcPort(portName, pipeHandle);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000DE64 File Offset: 0x0000CE64
		public bool WaitForConnect()
		{
			return NativePipe.ConnectNamedPipe(this._handle, null) || (long)Marshal.GetLastWin32Error() == 535L;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000DE94 File Offset: 0x0000CE94
		internal static IpcPort Connect(string portName, bool secure, TokenImpersonationLevel impersonationLevel, int timeout)
		{
			string text = "\\\\.\\pipe\\" + portName;
			uint num = 1048576U;
			if (secure)
			{
				switch (impersonationLevel)
				{
				case TokenImpersonationLevel.None:
					num = 1048576U;
					break;
				case TokenImpersonationLevel.Identification:
					num = 1114112U;
					break;
				case TokenImpersonationLevel.Impersonation:
					num = 1179648U;
					break;
				case TokenImpersonationLevel.Delegation:
					num = 1245184U;
					break;
				}
			}
			PipeHandle pipeHandle;
			int lastWin32Error;
			for (;;)
			{
				pipeHandle = NativePipe.CreateFile(text, 3221225472U, 3U, IntPtr.Zero, 3U, 1073741952U | num, IntPtr.Zero);
				if (pipeHandle.Handle.ToInt32() != -1)
				{
					break;
				}
				lastWin32Error = Marshal.GetLastWin32Error();
				if ((long)lastWin32Error != 231L)
				{
					goto Block_4;
				}
				if (!NativePipe.WaitNamedPipe(text, timeout))
				{
					goto Block_5;
				}
			}
			return new IpcPort(portName, pipeHandle);
			Block_4:
			throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_ConnectIpcFailed"), new object[] { IpcPort.GetMessage(lastWin32Error) }));
			Block_5:
			throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_Busy"), new object[] { IpcPort.GetMessage(lastWin32Error) }));
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000DFA4 File Offset: 0x0000CFA4
		internal static string GetMessage(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = NativePipe.FormatMessage(12800, NativePipe.NULL, errorCode, 0, stringBuilder, stringBuilder.Capacity, NativePipe.NULL);
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			return string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_UnknownError_Num"), new object[] { errorCode.ToString(CultureInfo.InvariantCulture) });
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000E014 File Offset: 0x0000D014
		internal void ImpersonateClient()
		{
			if (!NativePipe.ImpersonateNamedPipeClient(this._handle))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_ImpersonationFailed"), new object[] { IpcPort.GetMessage(lastWin32Error) }));
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000E064 File Offset: 0x0000D064
		internal unsafe int Read(byte[] data, int offset, int length)
		{
			int num = 0;
			bool flag;
			fixed (byte* ptr = data)
			{
				flag = NativePipe.ReadFile(this._handle, ptr + offset, length, ref num, IntPtr.Zero);
			}
			if (!flag)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_ReadFailure"), new object[] { IpcPort.GetMessage(lastWin32Error) }));
			}
			return num;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000E0E4 File Offset: 0x0000D0E4
		internal unsafe IAsyncResult BeginRead(byte[] data, int offset, int size, AsyncCallback callback, object state)
		{
			PipeAsyncResult pipeAsyncResult = new PipeAsyncResult(callback);
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, pipeAsyncResult);
			NativeOverlapped* ptr = overlapped.UnsafePack(IpcPort.IOCallback, data);
			pipeAsyncResult._overlapped = ptr;
			bool flag;
			fixed (byte* ptr2 = data)
			{
				flag = NativePipe.ReadFile(this._handle, ptr2 + offset, size, IntPtr.Zero, ptr);
			}
			if (!flag)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if ((long)lastWin32Error == 109L)
				{
					pipeAsyncResult.CallUserCallback();
				}
				else if ((long)lastWin32Error != 997L)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_ReadFailure"), new object[] { IpcPort.GetMessage(lastWin32Error) }));
				}
			}
			return pipeAsyncResult;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000E1A8 File Offset: 0x0000D1A8
		private unsafe static void AsyncFSCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(pOverlapped);
			PipeAsyncResult pipeAsyncResult = (PipeAsyncResult)overlapped.AsyncResult;
			pipeAsyncResult._numBytes = (int)numBytes;
			if ((ulong)errorCode == 109UL)
			{
				errorCode = 0U;
			}
			pipeAsyncResult._errorCode = (int)errorCode;
			AsyncCallback userCallback = pipeAsyncResult._userCallback;
			userCallback(pipeAsyncResult);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000E1F0 File Offset: 0x0000D1F0
		internal unsafe int EndRead(IAsyncResult iar)
		{
			PipeAsyncResult pipeAsyncResult = iar as PipeAsyncResult;
			NativeOverlapped* overlapped = pipeAsyncResult._overlapped;
			if (overlapped != null)
			{
				Overlapped.Free(overlapped);
			}
			if (pipeAsyncResult._errorCode != 0)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_ReadFailure"), new object[] { IpcPort.GetMessage(pipeAsyncResult._errorCode) }));
			}
			return pipeAsyncResult._numBytes;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000E254 File Offset: 0x0000D254
		internal unsafe void Write(byte[] data, int offset, int size)
		{
			int num = 0;
			bool flag;
			fixed (byte* ptr = data)
			{
				flag = NativePipe.WriteFile(this._handle, ptr + offset, size, ref num, IntPtr.Zero);
			}
			if (!flag)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Ipc_WriteFailure"), new object[] { IpcPort.GetMessage(lastWin32Error) }));
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000E2D4 File Offset: 0x0000D2D4
		~IpcPort()
		{
			this.Dispose();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000E300 File Offset: 0x0000D300
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				this._handle.Close();
				this.isDisposed = true;
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000E322 File Offset: 0x0000D322
		public bool IsDisposed
		{
			get
			{
				return this.isDisposed;
			}
		}

		// Token: 0x04000200 RID: 512
		private const string prefix = "\\\\.\\pipe\\";

		// Token: 0x04000201 RID: 513
		private const string networkSidSddlForm = "S-1-5-2";

		// Token: 0x04000202 RID: 514
		private const string authenticatedUserSidSddlForm = "S-1-5-11";

		// Token: 0x04000203 RID: 515
		private PipeHandle _handle;

		// Token: 0x04000204 RID: 516
		private string _portName;

		// Token: 0x04000205 RID: 517
		private bool _cacheable;

		// Token: 0x04000206 RID: 518
		private static CommonSecurityDescriptor s_securityDescriptor = IpcPort.CreateSecurityDescriptor(null);

		// Token: 0x04000207 RID: 519
		private static readonly IOCompletionCallback IOCallback = new IOCompletionCallback(IpcPort.AsyncFSCallback);

		// Token: 0x04000208 RID: 520
		private bool isDisposed;
	}
}
