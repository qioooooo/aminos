using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000093 RID: 147
	internal class ServiceActivityThunk
	{
		// Token: 0x06000107 RID: 263 RVA: 0x0000522C File Offset: 0x0000462C
		public unsafe ServiceActivityThunk(ServiceConfigThunk psct)
		{
			IUnknown* serviceConfigUnknown = psct.ServiceConfigUnknown;
			this.m_pSA = null;
			IServiceActivity* ptr;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(IUnknown*,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), serviceConfigUnknown, ref <Module>.IID_IServiceActivity, (void**)(&ptr), ServiceDomainThunk.CoCreateActivity);
			IUnknown* ptr2 = serviceConfigUnknown;
			uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
			Marshal.ThrowExceptionForHR(num);
			this.m_pSA = ptr;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005278 File Offset: 0x00004678
		protected unsafe override void Finalize()
		{
			IServiceActivity* pSA = this.m_pSA;
			if (pSA != null)
			{
				IServiceActivity* ptr = pSA;
				uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr, (IntPtr)(*(*(int*)ptr + 8)));
				this.m_pSA = null;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000052A4 File Offset: 0x000046A4
		public unsafe void SynchronousCall(object pObj)
		{
			IUnknown* ptr = null;
			IServiceCall* ptr2 = null;
			try
			{
				ptr = (IUnknown*)(void*)Marshal.GetIUnknownForObject(pObj);
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>.IID_IServiceCall, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
				Marshal.ThrowExceptionForHR(num);
				IServiceActivity* pSA = this.m_pSA;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.EnterpriseServices.Thunk.IServiceCall*), pSA, ptr2, (IntPtr)(*(*(int*)pSA + 12)));
				Marshal.ThrowExceptionForHR(num);
			}
			finally
			{
				if (ptr2 != null)
				{
					IServiceCall* ptr3 = ptr2;
					uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				}
				if (ptr != null)
				{
					IUnknown* ptr4 = ptr;
					uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005330 File Offset: 0x00004730
		public unsafe void AsynchronousCall(object pObj)
		{
			IUnknown* ptr = null;
			IServiceCall* ptr2 = null;
			try
			{
				ptr = (IUnknown*)(void*)Marshal.GetIUnknownForObject(pObj);
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>.IID_IServiceCall, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
				Marshal.ThrowExceptionForHR(num);
				IServiceActivity* pSA = this.m_pSA;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.EnterpriseServices.Thunk.IServiceCall*), pSA, ptr2, (IntPtr)(*(*(int*)pSA + 16)));
				Marshal.ThrowExceptionForHR(num);
			}
			finally
			{
				if (ptr2 != null)
				{
					IServiceCall* ptr3 = ptr2;
					uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				}
				if (ptr != null)
				{
					IUnknown* ptr4 = ptr;
					uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
				}
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000053BC File Offset: 0x000047BC
		public unsafe void BindToCurrentThread()
		{
			IServiceActivity* pSA = this.m_pSA;
			Marshal.ThrowExceptionForHR(calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pSA, (IntPtr)(*(*(int*)pSA + 20))));
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000053E0 File Offset: 0x000047E0
		public unsafe void UnbindFromThread()
		{
			IServiceActivity* pSA = this.m_pSA;
			Marshal.ThrowExceptionForHR(calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pSA, (IntPtr)(*(*(int*)pSA + 24))));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000054A0 File Offset: 0x000048A0
		public void {dtor}()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x040000BC RID: 188
		public unsafe IServiceActivity* m_pSA;
	}
}
