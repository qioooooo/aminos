using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000531 RID: 1329
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060028A1 RID: 10401 RVA: 0x000A7F98 File Offset: 0x000A6F98
		private SafeRegistryHandle()
			: base(true)
		{
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000A7FA1 File Offset: 0x000A6FA1
		internal static uint RegOpenKeyEx(IntPtr key, string subKey, uint ulOptions, uint samDesired, out SafeRegistryHandle resultSubKey)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(key, subKey, ulOptions, samDesired, out resultSubKey);
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000A7FAE File Offset: 0x000A6FAE
		internal uint RegOpenKeyEx(string subKey, uint ulOptions, uint samDesired, out SafeRegistryHandle resultSubKey)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(this, subKey, ulOptions, samDesired, out resultSubKey);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000A7FBB File Offset: 0x000A6FBB
		internal uint RegCloseKey()
		{
			base.Close();
			return this.resClose;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000A7FCC File Offset: 0x000A6FCC
		internal uint QueryValue(string name, out object data)
		{
			data = null;
			byte[] array = null;
			uint num = 0U;
			uint num3;
			uint num2;
			for (;;)
			{
				num2 = UnsafeNclNativeMethods.RegistryHelper.RegQueryValueEx(this, name, IntPtr.Zero, out num3, array, ref num);
				if (num2 != 234U && (array != null || num2 != 0U))
				{
					break;
				}
				array = new byte[num];
			}
			if (num2 != 0U)
			{
				return num2;
			}
			uint num4 = num3;
			if (num4 == 3U)
			{
				if ((ulong)num != (ulong)((long)array.Length))
				{
					byte[] array2 = array;
					array = new byte[num];
					Buffer.BlockCopy(array2, 0, array, 0, (int)num);
				}
				data = array;
				return 0U;
			}
			return 50U;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000A803C File Offset: 0x000A703C
		internal uint RegNotifyChangeKeyValue(bool watchSubTree, uint notifyFilter, SafeWaitHandle regEvent, bool async)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegNotifyChangeKeyValue(this, watchSubTree, notifyFilter, regEvent, async);
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000A8049 File Offset: 0x000A7049
		internal static uint RegOpenCurrentUser(uint samDesired, out SafeRegistryHandle resultKey)
		{
			if (ComNetOS.IsWin9x)
			{
				return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(UnsafeNclNativeMethods.RegistryHelper.HKEY_CURRENT_USER, null, 0U, samDesired, out resultKey);
			}
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenCurrentUser(samDesired, out resultKey);
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000A8068 File Offset: 0x000A7068
		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				this.resClose = UnsafeNclNativeMethods.RegistryHelper.RegCloseKey(this.handle);
			}
			base.SetHandleAsInvalid();
			return true;
		}

		// Token: 0x0400278E RID: 10126
		private uint resClose;
	}
}
