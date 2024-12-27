using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000054 RID: 84
	internal class Callback
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00003444 File Offset: 0x00002844
		private unsafe static int CallbackFunction(tagComCallData* pData)
		{
			UserCallData userCallData = null;
			bool flag = false;
			try
			{
				IntPtr intPtr = new IntPtr(*(int*)(pData + 8 / sizeof(tagComCallData)));
				userCallData = UserCallData.Get(intPtr);
				IProxyInvoke proxyInvoke = (IProxyInvoke)RemotingServices.GetRealProxy(userCallData.otp);
				userCallData.msg = proxyInvoke.LocalInvoke(userCallData.msg);
			}
			catch (Exception ex)
			{
				flag = true;
				if (userCallData != null)
				{
					userCallData.except = ex;
				}
			}
			catch
			{
				flag = true;
			}
			IMethodReturnMessage methodReturnMessage = userCallData.msg as IMethodReturnMessage;
			uint num3;
			if ((methodReturnMessage != null && methodReturnMessage.Exception != null) || flag)
			{
				if (userCallData != null && userCallData.fIsAutoDone)
				{
					IUnknown* pDestCtx = userCallData.pDestCtx;
					IObjectContext* ptr = null;
					if (calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pDestCtx, ref <Module>.IID_IObjectContext, (void**)(&ptr), (IntPtr)(*(*(int*)pDestCtx))) >= 0)
					{
						IObjectContext* ptr2 = ptr;
						int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 20)));
						IObjectContext* ptr3 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
					}
				}
				num3 = 2148734208U;
			}
			else
			{
				num3 = 0U;
			}
			return num3;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003544 File Offset: 0x00002944
		private unsafe static int MarshalCallback(tagComCallData* pData)
		{
			IntPtr intPtr = new IntPtr(*(int*)(pData + 8 / sizeof(tagComCallData)));
			UserMarshalData userMarshalData = UserMarshalData.Get(intPtr);
			uint num = 0U;
			IUnknown* ptr = userMarshalData.pUnk.ToInt32();
			int num2 = <Module>.CoGetMarshalSizeMax((uint*)(&num), ref <Module>.IID_IUnknown, ptr, 3, null, 0);
			if (num2 >= 0)
			{
				num += 4U;
				try
				{
					userMarshalData.buffer = new byte[num];
				}
				catch (OutOfMemoryException)
				{
					num2 = -2147024882;
				}
				if (num2 >= 0)
				{
					fixed (byte* ptr2 = &userMarshalData.buffer[0])
					{
						try
						{
							num2 = <Module>.MarshalInterface(ptr2, (int)num, ptr, 3, 0);
						}
						catch
						{
							ptr2 = null;
							throw;
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000036F8 File Offset: 0x00002AF8
		public unsafe IMessage DoCallback(object otp, IMessage msg, IntPtr ctx, [MarshalAs(UnmanagedType.U1)] bool fIsAutoDone, MemberInfo mb, [MarshalAs(UnmanagedType.U1)] bool bHasGit)
		{
			Proxy.Init();
			IUnknown* ptr = null;
			IContextCallback* ptr2 = null;
			IMessage message = null;
			UserCallData userCallData = null;
			tagComCallData2 tagComCallData = 0;
			*((ref tagComCallData) + 4) = 0;
			*((ref tagComCallData) + 8) = 0;
			*((ref tagComCallData) + 12) = Callback._pfn;
			try
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(otp);
				if (bHasGit)
				{
					ptr = realProxy.GetCOMIUnknown(false).ToInt32();
				}
				IUnknown* ptr3 = ctx.ToInt32();
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr3, ref <Module>.IID_IContextCallback, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr3)));
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				int num2 = (fIsAutoDone ? 7 : 8);
				_GUID iid_IRemoteDispatch = <Module>.IID_IRemoteDispatch;
				Type reflectedType = mb.ReflectedType;
				if (reflectedType.IsInterface)
				{
					Guid guid = Marshal.GenerateGuidForType(reflectedType);
					cpblk(ref iid_IRemoteDispatch, ref guid, 16);
					num2 = Marshal.GetComSlotForMethodInfo(mb);
				}
				userCallData = new UserCallData(otp, msg, ctx, fIsAutoDone, mb);
				IntPtr intPtr = userCallData.Pin();
				*((ref tagComCallData) + 8) = intPtr.ToInt32();
				int num3 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall) (System.EnterpriseServices.Thunk.tagComCallData*),System.EnterpriseServices.Thunk.tagComCallData*,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Int32,IUnknown*), ptr2, <Module>.__unep@?FilteringCallbackFunction@Thunk@EnterpriseServices@System@@$$FYGJPAUtagComCallData@123@@Z, (tagComCallData*)(&tagComCallData), ref iid_IRemoteDispatch, num2, ptr, (IntPtr)(*(*(int*)ptr2 + 12)));
				message = userCallData.msg;
				object except = userCallData.except;
				if (except != null)
				{
					throw except;
				}
				if (num3 < 0 && num3 != -2146233088)
				{
					Marshal.ThrowExceptionForHR(num3);
				}
			}
			finally
			{
				if (*((ref tagComCallData) + 8) != 0)
				{
					IntPtr intPtr2 = new IntPtr(*((ref tagComCallData) + 8));
					userCallData.Unpin(intPtr2);
				}
				if (ptr != null)
				{
					IUnknown* ptr4 = ptr;
					uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
				}
				if (ptr2 != null)
				{
					IContextCallback* ptr5 = ptr2;
					uint num5 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr5, (IntPtr)(*(*(int*)ptr5 + 8)));
				}
			}
			return message;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003600 File Offset: 0x00002A00
		public unsafe byte[] SwitchMarshal(IntPtr ctx, IntPtr pUnk)
		{
			Proxy.Init();
			byte[] array = null;
			IUnknown* ptr = pUnk.ToInt32();
			IContextCallback* ptr2 = null;
			UserMarshalData userMarshalData = null;
			tagComCallData2 tagComCallData = 0;
			*((ref tagComCallData) + 4) = 0;
			*((ref tagComCallData) + 8) = 0;
			*((ref tagComCallData) + 12) = Callback._pfnMarshal;
			try
			{
				IUnknown* ptr3 = ctx.ToInt32();
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr3, ref <Module>.IID_IContextCallback, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr3)));
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				userMarshalData = new UserMarshalData(pUnk);
				IntPtr intPtr = userMarshalData.Pin();
				*((ref tagComCallData) + 8) = intPtr.ToInt32();
				int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall) (System.EnterpriseServices.Thunk.tagComCallData*),System.EnterpriseServices.Thunk.tagComCallData*,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Int32,IUnknown*), ptr2, <Module>.__unep@?FilteringCallbackFunction@Thunk@EnterpriseServices@System@@$$FYGJPAUtagComCallData@123@@Z, (tagComCallData*)(&tagComCallData), ref <Module>.IID_IUnknown, 2, ptr, (IntPtr)(*(*(int*)ptr2 + 12)));
				if (num2 < 0)
				{
					Marshal.ThrowExceptionForHR(num2);
				}
				array = userMarshalData.buffer;
			}
			finally
			{
				if (*((ref tagComCallData) + 8) != 0)
				{
					IntPtr intPtr2 = new IntPtr(*((ref tagComCallData) + 8));
					userMarshalData.Unpin(intPtr2);
				}
				if (ptr2 != null)
				{
					IContextCallback* ptr4 = ptr2;
					uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
				}
			}
			return array;
		}

		// Token: 0x04000097 RID: 151
		private static ContextCallbackFunction _cb = new ContextCallbackFunction(Callback.CallbackFunction);

		// Token: 0x04000098 RID: 152
		private unsafe static delegate* unmanaged[Stdcall, Stdcall]<tagComCallData*, int> _pfn = Marshal.GetFunctionPointerForDelegate(Callback._cb).ToPointer();

		// Token: 0x04000099 RID: 153
		private static ContextCallbackFunction _cbMarshal = new ContextCallbackFunction(Callback.MarshalCallback);

		// Token: 0x0400009A RID: 154
		private unsafe static delegate* unmanaged[Stdcall, Stdcall]<tagComCallData*, int> _pfnMarshal = Marshal.GetFunctionPointerForDelegate(Callback._cbMarshal).ToPointer();
	}
}
