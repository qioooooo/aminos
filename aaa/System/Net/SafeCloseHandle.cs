using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200050F RID: 1295
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002817 RID: 10263 RVA: 0x000A569A File Offset: 0x000A469A
		private SafeCloseHandle()
		{
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000A56A2 File Offset: 0x000A46A2
		internal IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000A56AA File Offset: 0x000A46AA
		protected override bool ReleaseHandle()
		{
			return this.IsInvalid || Interlocked.Increment(ref this._disposed) != 1 || UnsafeNclNativeMethods.SafeNetHandles.CloseHandle(this.handle);
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000A56D0 File Offset: 0x000A46D0
		internal static int GetSecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle safeHandle)
		{
			int num = -2146893055;
			bool flag = false;
			safeHandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.SafeNetHandles.QuerySecurityContextToken(ref phContext._handle, out safeHandle);
					phContext.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000A5748 File Offset: 0x000A4748
		internal static SafeCloseHandle CreateRequestQueueHandle()
		{
			SafeCloseHandle safeCloseHandle = null;
			uint num = UnsafeNclNativeMethods.SafeNetHandles.HttpCreateHttpHandle(out safeCloseHandle, 0U);
			if (safeCloseHandle != null && num != 0U)
			{
				safeCloseHandle.SetHandleAsInvalid();
				throw new HttpListenerException((int)num);
			}
			return safeCloseHandle;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000A5774 File Offset: 0x000A4774
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Abort()
		{
			this.ReleaseHandle();
			base.SetHandleAsInvalid();
		}

		// Token: 0x0400275E RID: 10078
		private const string SECURITY = "security.dll";

		// Token: 0x0400275F RID: 10079
		private const string ADVAPI32 = "advapi32.dll";

		// Token: 0x04002760 RID: 10080
		private const string HTTPAPI = "httpapi.dll";

		// Token: 0x04002761 RID: 10081
		private int _disposed;
	}
}
