using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000094 RID: 148
	internal class SWCThunk
	{
		// Token: 0x0600010E RID: 270 RVA: 0x000043D0 File Offset: 0x000037D0
		private SWCThunk()
		{
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005404 File Offset: 0x00004804
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool IsSWCSupported()
		{
			IUnknown* ptr = null;
			IServiceTransactionConfig* ptr2 = null;
			int num = <Module>.CoCreateInstance(ref <Module>.CLSID_CServiceConfig, null, 1, ref <Module>.IID_IUnknown, (void**)(&ptr));
			if (num == -2147221164)
			{
				return false;
			}
			if (num >= 0)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>.IID_IServiceTransactionConfig, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
				if (num == -2147467262)
				{
					IUnknown* ptr3 = ptr;
					uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
					return false;
				}
			}
			if (ptr2 != null)
			{
				IServiceTransactionConfig* ptr4 = ptr2;
				uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
			}
			if (ptr != null)
			{
				IUnknown* ptr5 = ptr;
				uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr5, (IntPtr)(*(*(int*)ptr5 + 8)));
			}
			Marshal.ThrowExceptionForHR(num);
			return true;
		}
	}
}
