using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x02000083 RID: 131
	internal class CrmMonitor
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00004194 File Offset: 0x00003594
		public unsafe CrmMonitor()
		{
			ICrmMonitor* ptr;
			int num = <Module>.CoCreateInstance(ref <Module>.CLSID_CRMRecoveryClerk, null, 21, ref <Module>.IID_ICrmMonitor, (void**)(&ptr));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			this._pMon = ptr;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000041D0 File Offset: 0x000035D0
		public unsafe object GetClerks()
		{
			ICrmMonitor* pMon = this._pMon;
			ICrmMonitorClerks* ptr;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.EnterpriseServices.Thunk.ICrmMonitorClerks**), pMon, &ptr, (IntPtr)(*(*(int*)pMon + 12)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			object obj = null;
			try
			{
				IntPtr intPtr = new IntPtr((void*)ptr);
				obj = Marshal.GetObjectForIUnknown(intPtr);
			}
			finally
			{
				ICrmMonitorClerks* ptr2 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
			}
			return obj;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000423C File Offset: 0x0000363C
		public unsafe CrmLogControl HoldClerk(object idx)
		{
			CrmLogControl crmLogControl = null;
			tagVARIANT tagVARIANT;
			IntPtr intPtr = new IntPtr(ref tagVARIANT);
			IntPtr intPtr2 = intPtr;
			<Module>.VariantInit(&tagVARIANT);
			tagVARIANT tagVARIANT2;
			<Module>.VariantInit(&tagVARIANT2);
			Marshal.GetNativeVariantForObject(idx, intPtr2);
			ICrmMonitor* pMon = this._pMon;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,tagVARIANT,tagVARIANT*), pMon, tagVARIANT, &tagVARIANT2, (IntPtr)(*(*(int*)pMon + 16)));
			<Module>.VariantClear(&tagVARIANT);
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			IUnknown* ptr = *((ref tagVARIANT2) + 8);
			if (*((ref tagVARIANT2) + 8) != 0)
			{
				try
				{
					IntPtr intPtr3 = new IntPtr(ptr);
					crmLogControl = new CrmLogControl(intPtr3);
				}
				finally
				{
					<Module>.VariantClear(&tagVARIANT2);
				}
			}
			return crmLogControl;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000042E0 File Offset: 0x000036E0
		public unsafe void AddRef()
		{
			ICrmMonitor* pMon = this._pMon;
			uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pMon, (IntPtr)(*(*(int*)pMon + 4)));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004300 File Offset: 0x00003700
		public unsafe void Release()
		{
			ICrmMonitor* pMon = this._pMon;
			uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pMon, (IntPtr)(*(*(int*)pMon + 8)));
		}

		// Token: 0x040000B4 RID: 180
		private unsafe ICrmMonitor* _pMon;
	}
}
