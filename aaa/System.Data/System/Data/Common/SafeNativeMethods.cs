using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Data.Common
{
	// Token: 0x0200015E RID: 350
	[SuppressUnmanagedCodeSecurity]
	internal static class SafeNativeMethods
	{
		// Token: 0x060015E6 RID: 5606
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("ole32.dll")]
		internal static extern IntPtr CoTaskMemAlloc(IntPtr cb);

		// Token: 0x060015E7 RID: 5607
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("ole32.dll")]
		internal static extern void CoTaskMemFree(IntPtr handle);

		// Token: 0x060015E8 RID: 5608
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int GetUserDefaultLCID();

		// Token: 0x060015E9 RID: 5609
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll")]
		internal static extern void ZeroMemory(IntPtr dest, IntPtr length);

		// Token: 0x060015EA RID: 5610 RVA: 0x0022D1B0 File Offset: 0x0022C5B0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal unsafe static IntPtr InterlockedExchangePointer(IntPtr lpAddress, IntPtr lpValue)
		{
			IntPtr intPtr = *(IntPtr*)lpAddress.ToPointer();
			IntPtr intPtr2;
			do
			{
				intPtr2 = intPtr;
				intPtr = Interlocked.CompareExchange(ref *(IntPtr*)lpAddress.ToPointer(), lpValue, intPtr2);
			}
			while (intPtr != intPtr2);
			return intPtr;
		}

		// Token: 0x060015EB RID: 5611
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetComputerNameExW", SetLastError = true)]
		internal static extern int GetComputerNameEx(int nameType, StringBuilder nameBuffer, ref int bufferSize);

		// Token: 0x060015EC RID: 5612
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int GetCurrentProcessId();

		// Token: 0x060015ED RID: 5613
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPTStr)] [In] string moduleName);

		// Token: 0x060015EE RID: 5614
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr GetProcAddress(IntPtr HModule, [MarshalAs(UnmanagedType.LPStr)] [In] string funcName);

		// Token: 0x060015EF RID: 5615
		[DllImport("kernel32.dll")]
		internal static extern void GetSystemTimeAsFileTime(out long lpSystemTimeAsFileTime);

		// Token: 0x060015F0 RID: 5616
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LocalAlloc(int flags, IntPtr countOfBytes);

		// Token: 0x060015F1 RID: 5617
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x060015F2 RID: 5618
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr SysAllocStringLen(string src, int len);

		// Token: 0x060015F3 RID: 5619
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oleaut32.dll")]
		internal static extern void SysFreeString(IntPtr bstr);

		// Token: 0x060015F4 RID: 5620
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
		private static extern void SetErrorInfo(int dwReserved, IntPtr pIErrorInfo);

		// Token: 0x060015F5 RID: 5621
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int ReleaseSemaphore(IntPtr handle, int releaseCount, IntPtr previousCount);

		// Token: 0x060015F6 RID: 5622
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int WaitForMultipleObjectsEx(uint nCount, IntPtr lpHandles, bool bWaitAll, uint dwMilliseconds, bool bAlertable);

		// Token: 0x060015F7 RID: 5623
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll")]
		internal static extern int WaitForSingleObjectEx(IntPtr lpHandles, uint dwMilliseconds, bool bAlertable);

		// Token: 0x060015F8 RID: 5624
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("ole32.dll", PreserveSig = false)]
		internal static extern void PropVariantClear(IntPtr pObject);

		// Token: 0x060015F9 RID: 5625
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oleaut32.dll", PreserveSig = false)]
		internal static extern void VariantClear(IntPtr pObject);

		// Token: 0x0200015F RID: 351
		internal sealed class Wrapper
		{
			// Token: 0x060015FA RID: 5626 RVA: 0x0022D1E8 File Offset: 0x0022C5E8
			private Wrapper()
			{
			}

			// Token: 0x060015FB RID: 5627 RVA: 0x0022D1FC File Offset: 0x0022C5FC
			internal static void ClearErrorInfo()
			{
				SafeNativeMethods.SetErrorInfo(0, ADP.PtrZero);
			}
		}
	}
}
