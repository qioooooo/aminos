using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeNativeMethods
	{
		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IsWindowEnabled(IntPtr hwnd);

		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IsWindowVisible(IntPtr hwnd);

		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern void GetLocalTime(NativeTypes.SystemTime systime);

		private SafeNativeMethods()
		{
		}
	}
}
