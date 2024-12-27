using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x02000082 RID: 130
	internal class CrmLogControl
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00003E40 File Offset: 0x00003240
		public unsafe CrmLogControl(IntPtr p)
		{
			IUnknown* ptr = p.ToInt32();
			if (ptr == null)
			{
				throw new NullReferenceException();
			}
			ICrmLogControl* ptr2;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>.IID_ICrmLogControl, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			this._pCtrl = ptr2;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003E00 File Offset: 0x00003200
		public unsafe CrmLogControl()
		{
			this._pCtrl = null;
			ICrmLogControl* ptr;
			int num = <Module>.CoCreateInstance(ref <Module>.CLSID_CRMClerk, null, 21, ref <Module>.IID_ICrmLogControl, (void**)(&ptr));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			this._pCtrl = ptr;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003EB4 File Offset: 0x000032B4
		public unsafe string GetTransactionUOW()
		{
			ICrmLogControl* pCtrl = this._pCtrl;
			char* ptr;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Char**), pCtrl, &ptr, (IntPtr)(*(*(int*)pCtrl + 12)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			IntPtr intPtr = new IntPtr((void*)ptr);
			string text = Marshal.PtrToStringBSTR(intPtr);
			<Module>.SysFreeString(ptr);
			return text;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003EF8 File Offset: 0x000032F8
		public unsafe void RegisterCompensator(string progid, string desc, int flags)
		{
			char* ptr = null;
			char* ptr2 = null;
			try
			{
				char* ptr3 = Marshal.StringToCoTaskMemUni(progid).ToInt32();
				char* ptr4 = Marshal.StringToCoTaskMemUni(desc).ToInt32();
				ICrmLogControl* pCtrl = this._pCtrl;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Char modopt(System.Runtime.CompilerServices.IsConst)*,System.Char modopt(System.Runtime.CompilerServices.IsConst)*,System.Int32 modopt(System.Runtime.CompilerServices.IsLong)), pCtrl, (char*)ptr3, (char*)ptr4, flags, (IntPtr)(*(*(int*)pCtrl + 16)));
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}
			finally
			{
				<Module>.CoTaskMemFree((void*)ptr);
				<Module>.CoTaskMemFree((void*)ptr2);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003F7C File Offset: 0x0000337C
		public unsafe void ForceLog()
		{
			ICrmLogControl* pCtrl = this._pCtrl;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pCtrl, (IntPtr)(*(*(int*)pCtrl + 24)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00003FA8 File Offset: 0x000033A8
		public unsafe void ForgetLogRecord()
		{
			ICrmLogControl* pCtrl = this._pCtrl;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pCtrl, (IntPtr)(*(*(int*)pCtrl + 28)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00003FD4 File Offset: 0x000033D4
		public unsafe void ForceTransactionToAbort()
		{
			ICrmLogControl* pCtrl = this._pCtrl;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pCtrl, (IntPtr)(*(*(int*)pCtrl + 32)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004000 File Offset: 0x00003400
		public unsafe void WriteLogRecord(byte[] b)
		{
			tagBLOB length = b.Length;
			fixed (byte* ptr = &b[0])
			{
				*((ref length) + 4) = ptr;
				ICrmLogControl* pCtrl = this._pCtrl;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,tagBLOB* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), pCtrl, ref length, 1, (IntPtr)(*(*(int*)pCtrl + 36)));
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003E88 File Offset: 0x00003288
		public unsafe void Dispose()
		{
			ICrmLogControl* pCtrl = this._pCtrl;
			if (pCtrl != null)
			{
				ICrmLogControl* ptr = pCtrl;
				uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr, (IntPtr)(*(*(int*)ptr + 8)));
				this._pCtrl = null;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004320 File Offset: 0x00003720
		public CrmMonitorLogRecords GetMonitor()
		{
			IntPtr intPtr = new IntPtr(this._pCtrl);
			return new CrmMonitorLogRecords(intPtr);
		}

		// Token: 0x040000B3 RID: 179
		private unsafe ICrmLogControl* _pCtrl;
	}
}
