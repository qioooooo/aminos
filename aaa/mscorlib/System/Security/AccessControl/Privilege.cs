using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x02000919 RID: 2329
	internal sealed class Privilege
	{
		// Token: 0x06005489 RID: 21641 RVA: 0x001337B0 File Offset: 0x001327B0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private static Win32Native.LUID LuidFromPrivilege(string privilege)
		{
			Win32Native.LUID luid;
			luid.LowPart = 0U;
			luid.HighPart = 0U;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Privilege.privilegeLock.AcquireReaderLock(-1);
				if (Privilege.luids.Contains(privilege))
				{
					luid = (Win32Native.LUID)Privilege.luids[privilege];
					Privilege.privilegeLock.ReleaseReaderLock();
				}
				else
				{
					Privilege.privilegeLock.ReleaseReaderLock();
					if (!Win32Native.LookupPrivilegeValue(null, privilege, ref luid))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error == 8)
						{
							throw new OutOfMemoryException();
						}
						if (lastWin32Error == 5)
						{
							throw new UnauthorizedAccessException();
						}
						if (lastWin32Error == 1313)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPrivilegeName", new object[] { privilege }));
						}
						throw new InvalidOperationException();
					}
					else
					{
						Privilege.privilegeLock.AcquireWriterLock(-1);
					}
				}
			}
			finally
			{
				if (Privilege.privilegeLock.IsReaderLockHeld)
				{
					Privilege.privilegeLock.ReleaseReaderLock();
				}
				if (Privilege.privilegeLock.IsWriterLockHeld)
				{
					if (!Privilege.luids.Contains(privilege))
					{
						Privilege.luids[privilege] = luid;
						Privilege.privileges[luid] = privilege;
					}
					Privilege.privilegeLock.ReleaseWriterLock();
				}
			}
			return luid;
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x001338DC File Offset: 0x001328DC
		public Privilege(string privilegeName)
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				throw new NotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			if (privilegeName == null)
			{
				throw new ArgumentNullException("privilegeName");
			}
			this.luid = Privilege.LuidFromPrivilege(privilegeName);
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x0013392C File Offset: 0x0013292C
		~Privilege()
		{
			if (this.needToRevert)
			{
				this.Revert();
			}
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x00133960 File Offset: 0x00132960
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Enable()
		{
			this.ToggleState(true);
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x0600548D RID: 21645 RVA: 0x00133969 File Offset: 0x00132969
		public bool NeedToRevert
		{
			get
			{
				return this.needToRevert;
			}
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x00133974 File Offset: 0x00132974
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private void ToggleState(bool enable)
		{
			int num = 0;
			if (!this.currentThread.Equals(Thread.CurrentThread))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustBeSameThread"));
			}
			if (this.needToRevert)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustRevertPrivilege"));
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				try
				{
					this.tlsContents = Thread.GetData(Privilege.tlsSlot) as Privilege.TlsContents;
					if (this.tlsContents == null)
					{
						this.tlsContents = new Privilege.TlsContents();
						Thread.SetData(Privilege.tlsSlot, this.tlsContents);
					}
					else
					{
						this.tlsContents.IncrementReferenceCount();
					}
					Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE = default(Win32Native.TOKEN_PRIVILEGE);
					token_PRIVILEGE.PrivilegeCount = 1U;
					token_PRIVILEGE.Privilege.Luid = this.luid;
					token_PRIVILEGE.Privilege.Attributes = (enable ? 2U : 0U);
					Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE2 = default(Win32Native.TOKEN_PRIVILEGE);
					uint num2 = 0U;
					if (!Win32Native.AdjustTokenPrivileges(this.tlsContents.ThreadHandle, false, ref token_PRIVILEGE, (uint)Marshal.SizeOf(token_PRIVILEGE2), ref token_PRIVILEGE2, ref num2))
					{
						num = Marshal.GetLastWin32Error();
					}
					else if (1300 == Marshal.GetLastWin32Error())
					{
						num = 1300;
					}
					else
					{
						this.initialState = (token_PRIVILEGE2.Privilege.Attributes & 2U) != 0U;
						this.stateWasChanged = this.initialState != enable;
						this.needToRevert = this.tlsContents.IsImpersonating || this.stateWasChanged;
					}
				}
				finally
				{
					if (!this.needToRevert)
					{
						this.Reset();
					}
				}
			}
			if (num == 1300)
			{
				throw new PrivilegeNotHeldException(Privilege.privileges[this.luid] as string);
			}
			if (num == 8)
			{
				throw new OutOfMemoryException();
			}
			if (num == 5 || num == 1347)
			{
				throw new UnauthorizedAccessException();
			}
			if (num != 0)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x00133B68 File Offset: 0x00132B68
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Revert()
		{
			int num = 0;
			if (!this.currentThread.Equals(Thread.CurrentThread))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustBeSameThread"));
			}
			if (!this.NeedToRevert)
			{
				return;
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				bool flag = true;
				try
				{
					if (this.stateWasChanged && (this.tlsContents.ReferenceCountValue > 1 || !this.tlsContents.IsImpersonating))
					{
						Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE = default(Win32Native.TOKEN_PRIVILEGE);
						token_PRIVILEGE.PrivilegeCount = 1U;
						token_PRIVILEGE.Privilege.Luid = this.luid;
						token_PRIVILEGE.Privilege.Attributes = (this.initialState ? 2U : 0U);
						Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE2 = default(Win32Native.TOKEN_PRIVILEGE);
						uint num2 = 0U;
						if (!Win32Native.AdjustTokenPrivileges(this.tlsContents.ThreadHandle, false, ref token_PRIVILEGE, (uint)Marshal.SizeOf(token_PRIVILEGE2), ref token_PRIVILEGE2, ref num2))
						{
							num = Marshal.GetLastWin32Error();
							flag = false;
						}
					}
				}
				finally
				{
					if (flag)
					{
						this.Reset();
					}
				}
			}
			if (num == 8)
			{
				throw new OutOfMemoryException();
			}
			if (num == 5)
			{
				throw new UnauthorizedAccessException();
			}
			if (num != 0)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x00133C8C File Offset: 0x00132C8C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void Reset()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.stateWasChanged = false;
				this.initialState = false;
				this.needToRevert = false;
				if (this.tlsContents != null && this.tlsContents.DecrementReferenceCount() == 0)
				{
					this.tlsContents = null;
					Thread.SetData(Privilege.tlsSlot, null);
				}
			}
		}

		// Token: 0x04002BA4 RID: 11172
		public const string CreateToken = "SeCreateTokenPrivilege";

		// Token: 0x04002BA5 RID: 11173
		public const string AssignPrimaryToken = "SeAssignPrimaryTokenPrivilege";

		// Token: 0x04002BA6 RID: 11174
		public const string LockMemory = "SeLockMemoryPrivilege";

		// Token: 0x04002BA7 RID: 11175
		public const string IncreaseQuota = "SeIncreaseQuotaPrivilege";

		// Token: 0x04002BA8 RID: 11176
		public const string UnsolicitedInput = "SeUnsolicitedInputPrivilege";

		// Token: 0x04002BA9 RID: 11177
		public const string MachineAccount = "SeMachineAccountPrivilege";

		// Token: 0x04002BAA RID: 11178
		public const string TrustedComputingBase = "SeTcbPrivilege";

		// Token: 0x04002BAB RID: 11179
		public const string Security = "SeSecurityPrivilege";

		// Token: 0x04002BAC RID: 11180
		public const string TakeOwnership = "SeTakeOwnershipPrivilege";

		// Token: 0x04002BAD RID: 11181
		public const string LoadDriver = "SeLoadDriverPrivilege";

		// Token: 0x04002BAE RID: 11182
		public const string SystemProfile = "SeSystemProfilePrivilege";

		// Token: 0x04002BAF RID: 11183
		public const string SystemTime = "SeSystemtimePrivilege";

		// Token: 0x04002BB0 RID: 11184
		public const string ProfileSingleProcess = "SeProfileSingleProcessPrivilege";

		// Token: 0x04002BB1 RID: 11185
		public const string IncreaseBasePriority = "SeIncreaseBasePriorityPrivilege";

		// Token: 0x04002BB2 RID: 11186
		public const string CreatePageFile = "SeCreatePagefilePrivilege";

		// Token: 0x04002BB3 RID: 11187
		public const string CreatePermanent = "SeCreatePermanentPrivilege";

		// Token: 0x04002BB4 RID: 11188
		public const string Backup = "SeBackupPrivilege";

		// Token: 0x04002BB5 RID: 11189
		public const string Restore = "SeRestorePrivilege";

		// Token: 0x04002BB6 RID: 11190
		public const string Shutdown = "SeShutdownPrivilege";

		// Token: 0x04002BB7 RID: 11191
		public const string Debug = "SeDebugPrivilege";

		// Token: 0x04002BB8 RID: 11192
		public const string Audit = "SeAuditPrivilege";

		// Token: 0x04002BB9 RID: 11193
		public const string SystemEnvironment = "SeSystemEnvironmentPrivilege";

		// Token: 0x04002BBA RID: 11194
		public const string ChangeNotify = "SeChangeNotifyPrivilege";

		// Token: 0x04002BBB RID: 11195
		public const string RemoteShutdown = "SeRemoteShutdownPrivilege";

		// Token: 0x04002BBC RID: 11196
		public const string Undock = "SeUndockPrivilege";

		// Token: 0x04002BBD RID: 11197
		public const string SyncAgent = "SeSyncAgentPrivilege";

		// Token: 0x04002BBE RID: 11198
		public const string EnableDelegation = "SeEnableDelegationPrivilege";

		// Token: 0x04002BBF RID: 11199
		public const string ManageVolume = "SeManageVolumePrivilege";

		// Token: 0x04002BC0 RID: 11200
		public const string Impersonate = "SeImpersonatePrivilege";

		// Token: 0x04002BC1 RID: 11201
		public const string CreateGlobal = "SeCreateGlobalPrivilege";

		// Token: 0x04002BC2 RID: 11202
		public const string TrustedCredentialManagerAccess = "SeTrustedCredManAccessPrivilege";

		// Token: 0x04002BC3 RID: 11203
		public const string ReserveProcessor = "SeReserveProcessorPrivilege";

		// Token: 0x04002BC4 RID: 11204
		private static LocalDataStoreSlot tlsSlot = Thread.AllocateDataSlot();

		// Token: 0x04002BC5 RID: 11205
		private static Hashtable privileges = new Hashtable();

		// Token: 0x04002BC6 RID: 11206
		private static Hashtable luids = new Hashtable();

		// Token: 0x04002BC7 RID: 11207
		private static ReaderWriterLock privilegeLock = new ReaderWriterLock();

		// Token: 0x04002BC8 RID: 11208
		private bool needToRevert;

		// Token: 0x04002BC9 RID: 11209
		private bool initialState;

		// Token: 0x04002BCA RID: 11210
		private bool stateWasChanged;

		// Token: 0x04002BCB RID: 11211
		private Win32Native.LUID luid;

		// Token: 0x04002BCC RID: 11212
		private readonly Thread currentThread = Thread.CurrentThread;

		// Token: 0x04002BCD RID: 11213
		private Privilege.TlsContents tlsContents;

		// Token: 0x0200091A RID: 2330
		private sealed class TlsContents : IDisposable
		{
			// Token: 0x06005492 RID: 21650 RVA: 0x00133D1C File Offset: 0x00132D1C
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			public TlsContents()
			{
				int num = 0;
				int num2 = 0;
				bool flag = true;
				if (Privilege.TlsContents.processHandle.IsInvalid)
				{
					lock (Privilege.TlsContents.syncRoot)
					{
						if (Privilege.TlsContents.processHandle.IsInvalid && !Win32Native.OpenProcessToken(Win32Native.GetCurrentProcess(), TokenAccessLevels.Duplicate, ref Privilege.TlsContents.processHandle))
						{
							num2 = Marshal.GetLastWin32Error();
							flag = false;
						}
					}
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					try
					{
						SafeTokenHandle safeTokenHandle = this.threadHandle;
						num = Win32.OpenThreadToken(TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, WinSecurityContext.Process, out this.threadHandle);
						num &= 2147024895;
						if (num != 0)
						{
							if (flag)
							{
								this.threadHandle = safeTokenHandle;
								if (num != 1008)
								{
									flag = false;
								}
								if (flag)
								{
									num = 0;
									if (!Win32Native.DuplicateTokenEx(Privilege.TlsContents.processHandle, TokenAccessLevels.Impersonate | TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, IntPtr.Zero, Win32Native.SECURITY_IMPERSONATION_LEVEL.Impersonation, global::System.Security.Principal.TokenType.TokenImpersonation, ref this.threadHandle))
									{
										num = Marshal.GetLastWin32Error();
										flag = false;
									}
								}
								if (flag)
								{
									num = Win32.SetThreadToken(this.threadHandle);
									num &= 2147024895;
									if (num != 0)
									{
										flag = false;
									}
								}
								if (flag)
								{
									this.isImpersonating = true;
								}
							}
							else
							{
								num = num2;
							}
						}
						else
						{
							flag = true;
						}
					}
					finally
					{
						if (!flag)
						{
							this.Dispose();
						}
					}
				}
				if (num == 8)
				{
					throw new OutOfMemoryException();
				}
				if (num == 5 || num == 1347)
				{
					throw new UnauthorizedAccessException();
				}
				if (num != 0)
				{
					throw new InvalidOperationException();
				}
			}

			// Token: 0x06005493 RID: 21651 RVA: 0x00133E84 File Offset: 0x00132E84
			~TlsContents()
			{
				if (!this.disposed)
				{
					this.Dispose(false);
				}
			}

			// Token: 0x06005494 RID: 21652 RVA: 0x00133EBC File Offset: 0x00132EBC
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06005495 RID: 21653 RVA: 0x00133ECB File Offset: 0x00132ECB
			private void Dispose(bool disposing)
			{
				if (this.disposed)
				{
					return;
				}
				if (disposing && this.threadHandle != null)
				{
					this.threadHandle.Dispose();
					this.threadHandle = null;
				}
				if (this.isImpersonating)
				{
					Win32.RevertToSelf();
				}
				this.disposed = true;
			}

			// Token: 0x06005496 RID: 21654 RVA: 0x00133F08 File Offset: 0x00132F08
			public void IncrementReferenceCount()
			{
				this.referenceCount++;
			}

			// Token: 0x06005497 RID: 21655 RVA: 0x00133F18 File Offset: 0x00132F18
			public int DecrementReferenceCount()
			{
				int num = --this.referenceCount;
				if (num == 0)
				{
					this.Dispose();
				}
				return num;
			}

			// Token: 0x17000EA3 RID: 3747
			// (get) Token: 0x06005498 RID: 21656 RVA: 0x00133F41 File Offset: 0x00132F41
			public int ReferenceCountValue
			{
				get
				{
					return this.referenceCount;
				}
			}

			// Token: 0x17000EA4 RID: 3748
			// (get) Token: 0x06005499 RID: 21657 RVA: 0x00133F49 File Offset: 0x00132F49
			public SafeTokenHandle ThreadHandle
			{
				get
				{
					return this.threadHandle;
				}
			}

			// Token: 0x17000EA5 RID: 3749
			// (get) Token: 0x0600549A RID: 21658 RVA: 0x00133F51 File Offset: 0x00132F51
			public bool IsImpersonating
			{
				get
				{
					return this.isImpersonating;
				}
			}

			// Token: 0x04002BCE RID: 11214
			private bool disposed;

			// Token: 0x04002BCF RID: 11215
			private int referenceCount = 1;

			// Token: 0x04002BD0 RID: 11216
			private SafeTokenHandle threadHandle = new SafeTokenHandle(IntPtr.Zero);

			// Token: 0x04002BD1 RID: 11217
			private bool isImpersonating;

			// Token: 0x04002BD2 RID: 11218
			private static SafeTokenHandle processHandle = new SafeTokenHandle(IntPtr.Zero);

			// Token: 0x04002BD3 RID: 11219
			private static readonly object syncRoot = new object();
		}
	}
}
