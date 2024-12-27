using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200077E RID: 1918
	internal class MainWindowFinder
	{
		// Token: 0x06003B34 RID: 15156 RVA: 0x000FBE6C File Offset: 0x000FAE6C
		public IntPtr FindMainWindow(int processId)
		{
			this.bestHandle = (IntPtr)0;
			this.processId = processId;
			NativeMethods.EnumThreadWindowsCallback enumThreadWindowsCallback = new NativeMethods.EnumThreadWindowsCallback(this.EnumWindowsCallback);
			NativeMethods.EnumWindows(enumThreadWindowsCallback, IntPtr.Zero);
			GC.KeepAlive(enumThreadWindowsCallback);
			return this.bestHandle;
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000FBEB1 File Offset: 0x000FAEB1
		private bool IsMainWindow(IntPtr handle)
		{
			return !(NativeMethods.GetWindow(new HandleRef(this, handle), 4) != (IntPtr)0) && NativeMethods.IsWindowVisible(new HandleRef(this, handle));
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000FBEE0 File Offset: 0x000FAEE0
		private bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
		{
			int num;
			NativeMethods.GetWindowThreadProcessId(new HandleRef(this, handle), out num);
			if (num == this.processId && this.IsMainWindow(handle))
			{
				this.bestHandle = handle;
				return false;
			}
			return true;
		}

		// Token: 0x040033D4 RID: 13268
		private IntPtr bestHandle;

		// Token: 0x040033D5 RID: 13269
		private int processId;
	}
}
