using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000092 RID: 146
	internal class ServiceDomainThunk
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00004340 File Offset: 0x00003740
		private ServiceDomainThunk()
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004354 File Offset: 0x00003754
		// Note: this type is marked as 'beforefieldinit'.
		unsafe static ServiceDomainThunk()
		{
			HINSTANCE__* ptr = <Module>.LoadLibraryW((char*)(&<Module>.??_C@_1BI@NMLGLHFF@?$AAc?$AAo?$AAm?$AAs?$AAv?$AAc?$AAs?$AA?4?$AAd?$AAl?$AAl?$AA?$AA@));
			if (ptr == null || ptr == -1)
			{
				int num;
				if (<Module>.GetLastError() <= 0)
				{
					num = <Module>.GetLastError();
				}
				else
				{
					num = (<Module>.GetLastError() & 65535) | -2147024896;
				}
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}
			ServiceDomainThunk.CoEnterServiceDomain = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.??_C@_0BF@EEGEFJCM@CoEnterServiceDomain?$AA@));
			ServiceDomainThunk.CoLeaveServiceDomain = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.??_C@_0BF@JEIDNIFH@CoLeaveServiceDomain?$AA@));
			ServiceDomainThunk.CoCreateActivity = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.??_C@_0BB@LLBGKOGP@CoCreateActivity?$AA@));
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000051B0 File Offset: 0x000045B0
		public unsafe static void EnterServiceDomain(ServiceConfigThunk psct)
		{
			IUnknown* serviceConfigUnknown = psct.ServiceConfigUnknown;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(IUnknown*), serviceConfigUnknown, ServiceDomainThunk.CoEnterServiceDomain);
			IUnknown* ptr = serviceConfigUnknown;
			uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr, (IntPtr)(*(*(int*)ptr + 8)));
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000051E0 File Offset: 0x000045E0
		public unsafe static int LeaveServiceDomain()
		{
			int num = 0;
			TransactionStatus* ptr = <Module>.System.EnterpriseServices.Thunk.TransactionStatus.CreateInstance();
			if (ptr == null)
			{
				throw new OutOfMemoryException();
			}
			num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(IUnknown*), (IUnknown*)ptr, ServiceDomainThunk.CoLeaveServiceDomain);
			if (num >= 0)
			{
				int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong)*), ptr, (int*)(&num), (IntPtr)(*(*(int*)ptr + 16)));
			}
			TransactionStatus* ptr2 = ptr;
			uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
			return num;
		}

		// Token: 0x040000B9 RID: 185
		internal unsafe static delegate* unmanaged[Stdcall, Stdcall]<IUnknown*, int> CoEnterServiceDomain;

		// Token: 0x040000BA RID: 186
		internal unsafe static delegate* unmanaged[Stdcall, Stdcall]<IUnknown*, int> CoLeaveServiceDomain;

		// Token: 0x040000BB RID: 187
		internal unsafe static delegate* unmanaged[Stdcall, Stdcall]<IUnknown*, _GUID*, void**, int> CoCreateActivity;
	}
}
