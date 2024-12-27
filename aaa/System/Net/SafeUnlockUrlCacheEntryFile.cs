using System;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000530 RID: 1328
	internal sealed class SafeUnlockUrlCacheEntryFile : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600289D RID: 10397 RVA: 0x000A7EB4 File Offset: 0x000A6EB4
		private SafeUnlockUrlCacheEntryFile(string keyString)
			: base(true)
		{
			this.m_KeyString = keyString;
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000A7EC4 File Offset: 0x000A6EC4
		protected unsafe override bool ReleaseHandle()
		{
			fixed (char* keyString = this.m_KeyString)
			{
				UnsafeNclNativeMethods.SafeNetHandles.UnlockUrlCacheEntryFileW(keyString, 0);
			}
			base.SetHandle(IntPtr.Zero);
			this.m_KeyString = null;
			return true;
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000A7F04 File Offset: 0x000A6F04
		internal unsafe static _WinInetCache.Status GetAndLockFile(string key, byte* entryPtr, ref int entryBufSize, out SafeUnlockUrlCacheEntryFile handle)
		{
			if (ValidationHelper.IsBlankString(key))
			{
				throw new ArgumentNullException("key");
			}
			handle = new SafeUnlockUrlCacheEntryFile(key);
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = key);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			return SafeUnlockUrlCacheEntryFile.MustRunGetAndLockFile(ptr, entryPtr, ref entryBufSize, handle);
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000A7F48 File Offset: 0x000A6F48
		private unsafe static _WinInetCache.Status MustRunGetAndLockFile(char* key, byte* entryPtr, ref int entryBufSize, SafeUnlockUrlCacheEntryFile handle)
		{
			_WinInetCache.Status status = _WinInetCache.Status.Success;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (!UnsafeNclNativeMethods.SafeNetHandles.RetrieveUrlCacheEntryFileW(key, entryPtr, ref entryBufSize, 0))
				{
					status = (_WinInetCache.Status)Marshal.GetLastWin32Error();
					handle.SetHandleAsInvalid();
				}
				else
				{
					handle.SetHandle((IntPtr)1);
				}
			}
			return status;
		}

		// Token: 0x0400278D RID: 10125
		private string m_KeyString;
	}
}
