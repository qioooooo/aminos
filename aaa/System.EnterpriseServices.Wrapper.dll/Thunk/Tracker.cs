using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000055 RID: 85
	internal class Tracker
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00002680 File Offset: 0x00001A80
		internal unsafe Tracker(ISendMethodEvents* pTracker)
		{
			this._pTracker = pTracker;
			uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), pTracker, (IntPtr)(*(*(int*)pTracker + 4)));
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003370 File Offset: 0x00002770
		public unsafe void SendMethodCall(IntPtr pIdentity, MethodBase method)
		{
			if (this._pTracker != null)
			{
				Guid guid = Marshal.GenerateGuidForType(method.ReflectedType);
				_GUID guid2;
				cpblk(ref guid2, ref guid, 16);
				int num = 4;
				if (method.ReflectedType.IsInterface)
				{
					num = Marshal.GetComSlotForMethodInfo(method);
				}
				int num2 = *(int*)this._pTracker + 12;
				int num3 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Void modopt(System.Runtime.CompilerServices.IsConst)*,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), this._pTracker, (void*)(void*)pIdentity, ref guid2, num, (IntPtr)(*num2));
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000033D4 File Offset: 0x000027D4
		public unsafe void SendMethodReturn(IntPtr pIdentity, MethodBase method, Exception except)
		{
			if (this._pTracker != null)
			{
				Guid guid = Marshal.GenerateGuidForType(method.ReflectedType);
				_GUID guid2;
				cpblk(ref guid2, ref guid, 16);
				int num = 4;
				if (method.ReflectedType.IsInterface)
				{
					num = Marshal.GetComSlotForMethodInfo(method);
				}
				int num2 = 0;
				if (except != null)
				{
					num2 = Marshal.GetHRForException(except);
				}
				int num3 = *(int*)this._pTracker + 16;
				int num4 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Void modopt(System.Runtime.CompilerServices.IsConst)*,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.Int32 modopt(System.Runtime.CompilerServices.IsLong),System.Int32 modopt(System.Runtime.CompilerServices.IsLong)), this._pTracker, (void*)(void*)pIdentity, ref guid2, num, 0, num2, (IntPtr)(*num3));
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000026A8 File Offset: 0x00001AA8
		public unsafe void Release()
		{
			ISendMethodEvents* pTracker = this._pTracker;
			if (pTracker != null)
			{
				ISendMethodEvents* ptr = pTracker;
				uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr, (IntPtr)(*(*(int*)ptr + 8)));
				this._pTracker = null;
			}
		}

		// Token: 0x0400009B RID: 155
		private unsafe ISendMethodEvents* _pTracker;
	}
}
