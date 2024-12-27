using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x020004BC RID: 1212
	[ComVisible(true)]
	public class WindowsImpersonationContext : IDisposable
	{
		// Token: 0x060030D1 RID: 12497 RVA: 0x000A875C File Offset: 0x000A775C
		private WindowsImpersonationContext()
		{
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000A8770 File Offset: 0x000A7770
		internal WindowsImpersonationContext(SafeTokenHandle safeTokenHandle, WindowsIdentity wi, bool isImpersonating, FrameSecurityDescriptor fsd)
		{
			if (WindowsIdentity.RunningOnWin2K)
			{
				if (safeTokenHandle.IsInvalid)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidImpersonationToken"));
				}
				if (isImpersonating)
				{
					if (!Win32Native.DuplicateHandle(Win32Native.GetCurrentProcess(), safeTokenHandle, Win32Native.GetCurrentProcess(), ref this.m_safeTokenHandle, 0U, true, 2U))
					{
						throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
					}
					this.m_wi = wi;
				}
				this.m_fsd = fsd;
			}
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000A87EC File Offset: 0x000A77EC
		public void Undo()
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				return;
			}
			if (this.m_safeTokenHandle.IsInvalid)
			{
				int num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
			}
			else
			{
				int num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
				num = Win32.ImpersonateLoggedOnUser(this.m_safeTokenHandle);
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
			}
			WindowsIdentity.UpdateThreadWI(this.m_wi);
			if (this.m_fsd != null)
			{
				this.m_fsd.SetTokenHandles(null, null);
			}
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000A8878 File Offset: 0x000A7878
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool UndoNoThrow()
		{
			bool flag = false;
			try
			{
				if (!WindowsIdentity.RunningOnWin2K)
				{
					return true;
				}
				int num;
				if (this.m_safeTokenHandle.IsInvalid)
				{
					num = Win32.RevertToSelf();
				}
				else
				{
					num = Win32.RevertToSelf();
					if (num >= 0)
					{
						num = Win32.ImpersonateLoggedOnUser(this.m_safeTokenHandle);
					}
				}
				flag = num >= 0;
				if (this.m_fsd != null)
				{
					this.m_fsd.SetTokenHandles(null, null);
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000A88F8 File Offset: 0x000A78F8
		[ComVisible(false)]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.m_safeTokenHandle != null && !this.m_safeTokenHandle.IsClosed)
			{
				this.Undo();
				this.m_safeTokenHandle.Dispose();
			}
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000A8923 File Offset: 0x000A7923
		[ComVisible(false)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0400187C RID: 6268
		private SafeTokenHandle m_safeTokenHandle = SafeTokenHandle.InvalidHandle;

		// Token: 0x0400187D RID: 6269
		private WindowsIdentity m_wi;

		// Token: 0x0400187E RID: 6270
		private FrameSecurityDescriptor m_fsd;
	}
}
