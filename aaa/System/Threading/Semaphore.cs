using System;
using System.IO.Ports;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Threading
{
	// Token: 0x02000217 RID: 535
	[ComVisible(false)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public sealed class Semaphore : WaitHandle
	{
		// Token: 0x06001212 RID: 4626 RVA: 0x0003CF57 File Offset: 0x0003BF57
		public Semaphore(int initialCount, int maximumCount)
			: this(initialCount, maximumCount, null)
		{
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0003CF64 File Offset: 0x0003BF64
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Semaphore(int initialCount, int maximumCount, string name)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (maximumCount < 1)
			{
				throw new ArgumentOutOfRangeException("maximumCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (initialCount > maximumCount)
			{
				throw new ArgumentException(SR.GetString("Argument_SemaphoreInitialMaximum"));
			}
			if (name != null && Semaphore.MAX_PATH < name.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_WaitHandleNameTooLong"));
			}
			SafeWaitHandle safeWaitHandle = SafeNativeMethods.CreateSemaphore(null, initialCount, maximumCount, name);
			if (safeWaitHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (name != null && name.Length != 0 && 6 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				InternalResources.WinIOError();
			}
			base.SafeWaitHandle = safeWaitHandle;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0003D02C File Offset: 0x0003C02C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Semaphore(int initialCount, int maximumCount, string name, out bool createdNew)
			: this(initialCount, maximumCount, name, out createdNew, null)
		{
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0003D03C File Offset: 0x0003C03C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public unsafe Semaphore(int initialCount, int maximumCount, string name, out bool createdNew, SemaphoreSecurity semaphoreSecurity)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (maximumCount < 1)
			{
				throw new ArgumentOutOfRangeException("maximumCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (initialCount > maximumCount)
			{
				throw new ArgumentException(SR.GetString("Argument_SemaphoreInitialMaximum"));
			}
			if (name != null && Semaphore.MAX_PATH < name.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_WaitHandleNameTooLong"));
			}
			SafeWaitHandle safeWaitHandle;
			if (semaphoreSecurity != null)
			{
				NativeMethods.SECURITY_ATTRIBUTES security_ATTRIBUTES = new NativeMethods.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				byte[] securityDescriptorBinaryForm = semaphoreSecurity.GetSecurityDescriptorBinaryForm();
				fixed (byte* ptr = securityDescriptorBinaryForm)
				{
					security_ATTRIBUTES.lpSecurityDescriptor = new SafeLocalMemHandle((IntPtr)((void*)ptr), false);
					safeWaitHandle = SafeNativeMethods.CreateSemaphore(security_ATTRIBUTES, initialCount, maximumCount, name);
				}
			}
			else
			{
				safeWaitHandle = SafeNativeMethods.CreateSemaphore(null, initialCount, maximumCount, name);
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (safeWaitHandle.IsInvalid)
			{
				if (name != null && name.Length != 0 && 6 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				InternalResources.WinIOError();
			}
			createdNew = lastWin32Error != 183;
			base.SafeWaitHandle = safeWaitHandle;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0003D174 File Offset: 0x0003C174
		private Semaphore(SafeWaitHandle handle)
		{
			base.SafeWaitHandle = handle;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0003D183 File Offset: 0x0003C183
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Semaphore OpenExisting(string name)
		{
			return Semaphore.OpenExisting(name, SemaphoreRights.Modify | SemaphoreRights.Synchronize);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0003D190 File Offset: 0x0003C190
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Semaphore OpenExisting(string name, SemaphoreRights rights)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "name" }), "name");
			}
			if (name != null && Semaphore.MAX_PATH < name.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_WaitHandleNameTooLong"));
			}
			SafeWaitHandle safeWaitHandle = SafeNativeMethods.OpenSemaphore((int)rights, false, name);
			if (safeWaitHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (2 == lastWin32Error || 123 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException();
				}
				if (name != null && name.Length != 0 && 6 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				InternalResources.WinIOError();
			}
			return new Semaphore(safeWaitHandle);
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0003D254 File Offset: 0x0003C254
		[PrePrepareMethod]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int Release()
		{
			return this.Release(1);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0003D260 File Offset: 0x0003C260
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int Release(int releaseCount)
		{
			if (releaseCount < 1)
			{
				throw new ArgumentOutOfRangeException("releaseCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			int num;
			if (!SafeNativeMethods.ReleaseSemaphore(base.SafeWaitHandle, releaseCount, out num))
			{
				throw new SemaphoreFullException();
			}
			return num;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0003D29D File Offset: 0x0003C29D
		public SemaphoreSecurity GetAccessControl()
		{
			return new SemaphoreSecurity(base.SafeWaitHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0003D2AC File Offset: 0x0003C2AC
		public void SetAccessControl(SemaphoreSecurity semaphoreSecurity)
		{
			if (semaphoreSecurity == null)
			{
				throw new ArgumentNullException("semaphoreSecurity");
			}
			semaphoreSecurity.Persist(base.SafeWaitHandle);
		}

		// Token: 0x04001083 RID: 4227
		private static int MAX_PATH = 260;
	}
}
