using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000002 RID: 2
	internal class IdentityManager
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00001028 File Offset: 0x00000428
		private IdentityManager()
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000016D0 File Offset: 0x00000AD0
		private static void Init()
		{
			int num = <Module>.System.EnterpriseServices.Thunk.InitSpy();
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000016F0 File Offset: 0x00000AF0
		public static bool Enabled
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				int num = <Module>.System.EnterpriseServices.Thunk.InitSpy();
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				return <Module>.System.EnterpriseServices.Thunk.InitializeSpy.GetEnabled(<Module>.System.EnterpriseServices.Thunk.g_pSpy) != 0;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00001720 File Offset: 0x00000B20
		public unsafe static void NoticeApartment()
		{
			int num = <Module>.System.EnterpriseServices.Thunk.InitSpy();
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			int num2 = <Module>.System.EnterpriseServices.Thunk.InitSpy();
			if (num2 < 0)
			{
				Marshal.ThrowExceptionForHR(num2);
			}
			if (<Module>.System.EnterpriseServices.Thunk.InitializeSpy.GetEnabled(<Module>.System.EnterpriseServices.Thunk.g_pSpy) != null)
			{
				InitializeSpy* system.EnterpriseServices.Thunk.g_pSpy = <Module>.System.EnterpriseServices.Thunk.g_pSpy;
				int num3 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), system.EnterpriseServices.Thunk.g_pSpy, (IntPtr)(*(*(int*)system.EnterpriseServices.Thunk.g_pSpy + 40)));
				if (num3 < 0)
				{
					Marshal.ThrowExceptionForHR(num3);
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00001774 File Offset: 0x00000B74
		public unsafe static string CreateIdentityUri(IntPtr pUnk)
		{
			int num = <Module>.System.EnterpriseServices.Thunk.InitSpy();
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			InitializeSpy* system.EnterpriseServices.Thunk.g_pSpy = <Module>.System.EnterpriseServices.Thunk.g_pSpy;
			int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), system.EnterpriseServices.Thunk.g_pSpy, (IntPtr)(*(*(int*)system.EnterpriseServices.Thunk.g_pSpy + 40)));
			if (num2 < 0)
			{
				Marshal.ThrowExceptionForHR(num2);
			}
			int num3 = *(int*)<Module>.System.EnterpriseServices.Thunk.g_pSpy + 32;
			ulong num5;
			ulong num6;
			int num4 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown*,System.UInt64*,System.UInt64*), <Module>.System.EnterpriseServices.Thunk.g_pSpy, pUnk.ToInt32(), &num5, &num6, (IntPtr)(*num3));
			if (num4 < 0)
			{
				Marshal.ThrowExceptionForHR(num4);
			}
			ulong num7 = num6;
			ulong num8 = num5;
			return "servicedcomponent-local-identity://" + num8.ToString(CultureInfo.InvariantCulture) + ":" + num7.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00001808 File Offset: 0x00000C08
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool IsInProcess(IntPtr pUnk)
		{
			int num = <Module>.System.EnterpriseServices.Thunk.InitSpy();
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			int num2 = 1;
			int num3 = *(int*)<Module>.System.EnterpriseServices.Thunk.g_pSpy + 44;
			int num4 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown*,System.Int32*), <Module>.System.EnterpriseServices.Thunk.g_pSpy, pUnk.ToInt32(), &num2, (IntPtr)(*num3));
			if (num4 < 0)
			{
				Marshal.ThrowExceptionForHR(num4);
			}
			return num2 != 0;
		}
	}
}
