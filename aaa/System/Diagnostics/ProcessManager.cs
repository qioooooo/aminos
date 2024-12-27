using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x0200077F RID: 1919
	internal static class ProcessManager
	{
		// Token: 0x06003B38 RID: 15160 RVA: 0x000FBF20 File Offset: 0x000FAF20
		static ProcessManager()
		{
			NativeMethods.LUID luid = default(NativeMethods.LUID);
			if (!NativeMethods.LookupPrivilegeValue(null, "SeDebugPrivilege", out luid))
			{
				return;
			}
			IntPtr zero = IntPtr.Zero;
			try
			{
				if (NativeMethods.OpenProcessToken(new HandleRef(null, NativeMethods.GetCurrentProcess()), 32, out zero))
				{
					NativeMethods.TokenPrivileges tokenPrivileges = new NativeMethods.TokenPrivileges();
					tokenPrivileges.PrivilegeCount = 1;
					tokenPrivileges.Luid = luid;
					tokenPrivileges.Attributes = 2;
					NativeMethods.AdjustTokenPrivileges(new HandleRef(null, zero), false, tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					SafeNativeMethods.CloseHandle(new HandleRef(null, zero));
				}
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x000FBFC8 File Offset: 0x000FAFC8
		public static bool IsNt
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06003B3A RID: 15162 RVA: 0x000FBFD7 File Offset: 0x000FAFD7
		public static bool IsOSOlderThanXP
		{
			get
			{
				return Environment.OSVersion.Version.Major < 5 || (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 0);
			}
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x000FC014 File Offset: 0x000FB014
		public static ProcessInfo[] GetProcessInfos(string machineName)
		{
			bool flag = ProcessManager.IsRemoteMachine(machineName);
			if (ProcessManager.IsNt)
			{
				if (!flag && Environment.OSVersion.Version.Major >= 5)
				{
					return NtProcessInfoHelper.GetProcessInfos();
				}
				return NtProcessManager.GetProcessInfos(machineName, flag);
			}
			else
			{
				if (flag)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinNTRequiredForRemote"));
				}
				return WinProcessManager.GetProcessInfos();
			}
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x000FC06A File Offset: 0x000FB06A
		public static int[] GetProcessIds()
		{
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetProcessIds();
			}
			return WinProcessManager.GetProcessIds();
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x000FC07E File Offset: 0x000FB07E
		public static int[] GetProcessIds(string machineName)
		{
			if (!ProcessManager.IsRemoteMachine(machineName))
			{
				return ProcessManager.GetProcessIds();
			}
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetProcessIds(machineName, true);
			}
			throw new PlatformNotSupportedException(SR.GetString("WinNTRequiredForRemote"));
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x000FC0AC File Offset: 0x000FB0AC
		public static bool IsProcessRunning(int processId, string machineName)
		{
			return ProcessManager.IsProcessRunning(processId, ProcessManager.GetProcessIds(machineName));
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x000FC0BA File Offset: 0x000FB0BA
		public static bool IsProcessRunning(int processId)
		{
			return ProcessManager.IsProcessRunning(processId, ProcessManager.GetProcessIds());
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x000FC0C8 File Offset: 0x000FB0C8
		private static bool IsProcessRunning(int processId, int[] processIds)
		{
			for (int i = 0; i < processIds.Length; i++)
			{
				if (processIds[i] == processId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x000FC0EC File Offset: 0x000FB0EC
		public static int GetProcessIdFromHandle(SafeProcessHandle processHandle)
		{
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetProcessIdFromHandle(processHandle);
			}
			throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x000FC10C File Offset: 0x000FB10C
		public static IntPtr GetMainWindowHandle(ProcessInfo processInfo)
		{
			MainWindowFinder mainWindowFinder = new MainWindowFinder();
			return mainWindowFinder.FindMainWindow(processInfo.processId);
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x000FC12B File Offset: 0x000FB12B
		public static ModuleInfo[] GetModuleInfos(int processId)
		{
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetModuleInfos(processId);
			}
			return WinProcessManager.GetModuleInfos(processId);
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x000FC144 File Offset: 0x000FB144
		public static SafeProcessHandle OpenProcess(int processId, int access, bool throwIfExited)
		{
			SafeProcessHandle safeProcessHandle = NativeMethods.OpenProcess(access, false, processId);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (!safeProcessHandle.IsInvalid)
			{
				return safeProcessHandle;
			}
			if (processId == 0)
			{
				throw new Win32Exception(5);
			}
			if (ProcessManager.IsProcessRunning(processId))
			{
				throw new Win32Exception(lastWin32Error);
			}
			if (throwIfExited)
			{
				throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[] { processId.ToString(CultureInfo.CurrentCulture) }));
			}
			return SafeProcessHandle.InvalidHandle;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x000FC1B4 File Offset: 0x000FB1B4
		public static SafeThreadHandle OpenThread(int threadId, int access)
		{
			SafeThreadHandle safeThreadHandle2;
			try
			{
				SafeThreadHandle safeThreadHandle = NativeMethods.OpenThread(access, false, threadId);
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (safeThreadHandle.IsInvalid)
				{
					if (lastWin32Error == 87)
					{
						throw new InvalidOperationException(SR.GetString("ThreadExited", new object[] { threadId.ToString(CultureInfo.CurrentCulture) }));
					}
					throw new Win32Exception(lastWin32Error);
				}
				else
				{
					safeThreadHandle2 = safeThreadHandle;
				}
			}
			catch (EntryPointNotFoundException ex)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"), ex);
			}
			return safeThreadHandle2;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x000FC238 File Offset: 0x000FB238
		public static bool IsRemoteMachine(string machineName)
		{
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			if (machineName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			string text;
			if (machineName.StartsWith("\\", StringComparison.Ordinal))
			{
				text = machineName.Substring(2);
			}
			else
			{
				text = machineName;
			}
			if (text.Equals("."))
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			SafeNativeMethods.GetComputerName(stringBuilder, new int[] { stringBuilder.Capacity });
			string text2 = stringBuilder.ToString();
			return string.Compare(text2, text, StringComparison.OrdinalIgnoreCase) != 0;
		}
	}
}
