using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200008B RID: 139
	internal class WbemErrorInfo
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x00021360 File Offset: 0x00020360
		public static IWbemClassObjectFreeThreaded GetErrorInfo()
		{
			IntPtr intPtr = WmiNetUtilsHelper.GetErrorInfo_f();
			if (IntPtr.Zero != intPtr && new IntPtr(-1) != intPtr)
			{
				IntPtr intPtr2;
				Marshal.QueryInterface(intPtr, ref IWbemClassObjectFreeThreaded.IID_IWbemClassObject, out intPtr2);
				Marshal.Release(intPtr);
				if (intPtr2 != IntPtr.Zero)
				{
					return new IWbemClassObjectFreeThreaded(intPtr2);
				}
			}
			return null;
		}
	}
}
