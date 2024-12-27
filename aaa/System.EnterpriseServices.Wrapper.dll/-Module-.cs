using System;
using System.Diagnostics;
using System.EnterpriseServices.Thunk;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using <CppImplementationDetails>;
using <CrtImplementationDetails>;

// Token: 0x02000001 RID: 1
internal class <Module>
{
	// Token: 0x06000001 RID: 1 RVA: 0x00001130 File Offset: 0x00000530
	internal unsafe static int IsEqualGUID(_GUID* rguid1, _GUID* rguid2)
	{
		uint num = 16U;
		_GUID* ptr = rguid2;
		sbyte b = *rguid1;
		sbyte b2 = *rguid2;
		if (b >= b2)
		{
			int num2 = rguid1 - rguid2;
			while (b <= b2)
			{
				if (num == 1U)
				{
					return 1;
				}
				num -= 1U;
				ptr++;
				b = *(num2 + ptr);
				b2 = *ptr;
				if (b < b2)
				{
					break;
				}
			}
		}
		return 0;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00001188 File Offset: 0x00000588
	internal unsafe static int ==(_GUID* guidOne, _GUID* guidOther)
	{
		return <Module>.IsEqualGUID(guidOne, guidOther);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00001000 File Offset: 0x00000400
	internal unsafe static void* __InlineInterlockedCompareExchangePointer(void** Destination, void* ExChange, void* Comperand)
	{
		return <Module>.InterlockedCompareExchange((int*)Destination, ExChange, Comperand);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00001B8C File Offset: 0x00000F8C
	internal unsafe static IInitializeSpy* IInitializeSpy.{ctor}(IInitializeSpy* A_0)
	{
		return A_0;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00006024 File Offset: 0x00005424
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsInDllMain()
	{
		return (<Module>.__native_dllmain_reason != uint.MaxValue) ? 1 : 0;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00006048 File Offset: 0x00005448
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsInProcessAttach()
	{
		return (<Module>.__native_dllmain_reason == 1U) ? 1 : 0;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00006068 File Offset: 0x00005468
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsInProcessDetach()
	{
		return (<Module>.__native_dllmain_reason == 0U) ? 1 : 0;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00006088 File Offset: 0x00005488
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsInVcclrit()
	{
		return (<Module>.__native_vcclrit_reason != uint.MaxValue) ? 1 : 0;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000060AC File Offset: 0x000054AC
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsSafeForManagedCode()
	{
		if (((<Module>.__native_dllmain_reason != 4294967295U) ? 1 : 0) == 0)
		{
			return 1;
		}
		if (((<Module>.__native_vcclrit_reason != 4294967295U) ? 1 : 0) != 0)
		{
			return 1;
		}
		int num;
		if (((<Module>.__native_dllmain_reason == 1U) ? 1 : 0) == 0 && ((<Module>.__native_dllmain_reason == 0U) ? 1 : 0) == 0)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000068A0 File Offset: 0x00005CA0
	internal static void <CrtImplementationDetails>.ThrowNestedModuleLoadException(global::System.Exception innerException, global::System.Exception nestedException)
	{
		throw new ModuleLoadExceptionHandlerException("A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n", innerException, nestedException);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000062D0 File Offset: 0x000056D0
	internal static void <CrtImplementationDetails>.ThrowModuleLoadException(string errorMessage)
	{
		throw new ModuleLoadException(errorMessage);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000062EC File Offset: 0x000056EC
	internal static void <CrtImplementationDetails>.ThrowModuleLoadException(string errorMessage, global::System.Exception innerException)
	{
		throw new ModuleLoadException(errorMessage, innerException);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00006384 File Offset: 0x00005784
	internal static void <CrtImplementationDetails>.RegisterModuleUninitializer(EventHandler handler)
	{
		ModuleUninitializer._ModuleUninitializer.AddHandler(handler);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000063A4 File Offset: 0x000057A4
	internal unsafe static int __get_default_appdomain(IUnknown** ppUnk)
	{
		int num = 0;
		IUnknown* ptr = null;
		ICorRuntimeHost* ptr2 = null;
		try
		{
			num = <Module>.CoCreateInstance(ref <Module>._GUID_cb2f6723_ab3a_11d2_9c40_00c04fa30a3e, null, 1, ref <Module>._GUID_00000000_0000_0000_c000_000000000046, (void**)(&ptr));
			if (num >= 0)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>._GUID_cb2f6722_ab3a_11d2_9c40_00c04fa30a3e, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
				if (num >= 0)
				{
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown**), ptr2, ppUnk, (IntPtr)(*(*(int*)ptr2 + 52)));
				}
			}
		}
		finally
		{
			if (ptr != null)
			{
				IUnknown* ptr3 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
			}
			if (ptr2 != null)
			{
				ICorRuntimeHost* ptr4 = ptr2;
				uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
			}
		}
		return num;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00006438 File Offset: 0x00005838
	internal unsafe static void __release_appdomain(IUnknown* ppUnk)
	{
		uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ppUnk, (IntPtr)(*(*(int*)ppUnk + 8)));
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00006458 File Offset: 0x00005858
	internal unsafe static AppDomain <CrtImplementationDetails>.GetDefaultDomain()
	{
		IUnknown* ptr = null;
		int num = <Module>.__get_default_appdomain(&ptr);
		if (num >= 0)
		{
			try
			{
				IntPtr intPtr = new IntPtr((void*)ptr);
				return (AppDomain)Marshal.GetObjectForIUnknown(intPtr);
			}
			finally
			{
				IUnknown* ptr2 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
			}
		}
		Marshal.ThrowExceptionForHR(num);
		return null;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000064C4 File Offset: 0x000058C4
	internal unsafe static void <CrtImplementationDetails>.DoCallBackInDefaultDomain(delegate* unmanaged[Stdcall, Stdcall]<void*, int> function, void* cookie)
	{
		ICLRRuntimeHost* ptr = null;
		try
		{
			int num = <Module>.CorBindToRuntimeEx(null, null, 0, ref <Module>._GUID_90f1a06e_7712_4762_86b5_7a5eba6bdb02, ref <Module>._GUID_90f1a06c_7712_4762_86b5_7a5eba6bdb02, (void**)(&ptr));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			AppDomain appDomain = <Module>.<CrtImplementationDetails>.GetDefaultDomain();
			int num2 = *(int*)ptr + 32;
			int num3 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall) (System.Void*),System.Void*), ptr, appDomain.Id, function, cookie, (IntPtr)(*num2));
			if (num3 < 0)
			{
				Marshal.ThrowExceptionForHR(num3);
			}
		}
		finally
		{
			if (ptr != null)
			{
				ICLRRuntimeHost* ptr2 = ptr;
				uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
			}
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00006580 File Offset: 0x00005980
	internal unsafe static int <CrtImplementationDetails>.DefaultDomain.DoNothing(void* cookie)
	{
		GC.KeepAlive(int.MaxValue);
		return 0;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000065A4 File Offset: 0x000059A4
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool <CrtImplementationDetails>.DefaultDomain.HasPerProcess()
	{
		if (<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)2)
		{
			void** ptr = (void**)(&<Module>.?A0xa1cb2edd.__xc_mp_a);
			if ((ref <Module>.?A0xa1cb2edd.__xc_mp_a) < (ref <Module>.?A0xa1cb2edd.__xc_mp_z))
			{
				while (*(int*)ptr == 0)
				{
					ptr += 4 / sizeof(void*);
					if (ptr >= (void**)(&<Module>.?A0xa1cb2edd.__xc_mp_z))
					{
						goto IL_0034;
					}
				}
				<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_0034:
			<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)0;
			return 0;
		}
		return (<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)(-1)) ? 1 : 0;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000065FC File Offset: 0x000059FC
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool <CrtImplementationDetails>.DefaultDomain.HasNative()
	{
		if (<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)2)
		{
			void** ptr = (void**)(&<Module>.__xi_a);
			if ((ref <Module>.__xi_a) < (ref <Module>.__xi_z))
			{
				while (*(int*)ptr == 0)
				{
					ptr += 4 / sizeof(void*);
					if (ptr >= (void**)(&<Module>.__xi_z))
					{
						goto IL_0034;
					}
				}
				<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_0034:
			void** ptr2 = (void**)(&<Module>.__xc_a);
			if ((ref <Module>.__xc_a) < (ref <Module>.__xc_z))
			{
				while (*(int*)ptr2 == 0)
				{
					ptr2 += 4 / sizeof(void*);
					if (ptr2 >= (void**)(&<Module>.__xc_z))
					{
						goto IL_0060;
					}
				}
				<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_0060:
			<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)0;
			return 0;
		}
		return (<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)(-1)) ? 1 : 0;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00006680 File Offset: 0x00005A80
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.DefaultDomain.NeedsInitialization()
	{
		int num;
		if ((<Module>.<CrtImplementationDetails>.DefaultDomain.HasPerProcess() != null && !<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA) || (<Module>.<CrtImplementationDetails>.DefaultDomain.HasNative() != null && !<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA && <Module>.__native_startup_state == (__enative_startup_state)0))
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000066C0 File Offset: 0x00005AC0
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.DefaultDomain.NeedsUninitialization()
	{
		return <Module>.?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000066D8 File Offset: 0x00005AD8
	internal static void <CrtImplementationDetails>.DefaultDomain.Initialize()
	{
		<Module>.<CrtImplementationDetails>.DoCallBackInDefaultDomain(<Module>.__unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z, null);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000872C File Offset: 0x00007B2C
	internal static void ?A0xa1cb2edd.??__E?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00008740 File Offset: 0x00007B40
	internal static void ?A0xa1cb2edd.??__E?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00008754 File Offset: 0x00007B54
	internal static void ?A0xa1cb2edd.??__E?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA@@YMXXZ()
	{
		<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA = false;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00008768 File Offset: 0x00007B68
	internal static void ?A0xa1cb2edd.??__E?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x0000877C File Offset: 0x00007B7C
	internal static void ?A0xa1cb2edd.??__E?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00008790 File Offset: 0x00007B90
	internal static void ?A0xa1cb2edd.??__E?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000087A4 File Offset: 0x00007BA4
	internal static void ?A0xa1cb2edd.??__E?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x0000691C File Offset: 0x00005D1C
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeVtables(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during vtable initialization.\n");
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initterm_m((delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xi_vt_a), (delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xi_vt_z));
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00006958 File Offset: 0x00005D58
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeDefaultAppDomain(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load while attempting to initialize the default appdomain.\n");
		<Module>.<CrtImplementationDetails>.DefaultDomain.Initialize();
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000697C File Offset: 0x00005D7C
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeNative(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during native initialization.\n");
		<Module>.__security_init_cookie();
		<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
		if (<Module>.<CrtImplementationDetails>.NativeDll.IsSafeForManagedCode() == null)
		{
			<Module>._amsg_exit(33);
		}
		if (<Module>.__native_startup_state == (__enative_startup_state)1)
		{
			<Module>._amsg_exit(33);
		}
		else if (<Module>.__native_startup_state == (__enative_startup_state)0)
		{
			<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
			<Module>.__native_startup_state = (__enative_startup_state)1;
			if (<Module>._initterm_e((delegate* unmanaged[Cdecl, Cdecl]<int>*)(&<Module>.__xi_a), (delegate* unmanaged[Cdecl, Cdecl]<int>*)(&<Module>.__xi_z)) != 0)
			{
				throw new ModuleLoadException(<Module>.gcroot<System::String\u0020^>..P$AAVString@System@@(A_0));
			}
			<Module>._initterm((delegate* unmanaged[Cdecl, Cdecl]<void>*)(&<Module>.__xc_a), (delegate* unmanaged[Cdecl, Cdecl]<void>*)(&<Module>.__xc_z));
			<Module>.__native_startup_state = (__enative_startup_state)2;
			<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
			<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00006A20 File Offset: 0x00005E20
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializePerProcess(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during process initialization.\n");
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initatexit_m();
		<Module>._initterm_m((delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xc_mp_a), (delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xc_mp_z));
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
		<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00006A68 File Offset: 0x00005E68
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializePerAppDomain(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during appdomain initialization.\n");
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initatexit_app_domain();
		<Module>._initterm_m((delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xc_ma_a), (delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xc_ma_z));
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00006AA8 File Offset: 0x00005EA8
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeUninitializer(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during registration for the unload events.\n");
		EventHandler eventHandler = new EventHandler(<Module>.<CrtImplementationDetails>.LanguageSupport.DomainUnload);
		ModuleUninitializer._ModuleUninitializer.AddHandler(eventHandler);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00006AE0 File Offset: 0x00005EE0
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport._Initialize(LanguageSupport* A_0)
	{
		<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA = AppDomain.CurrentDomain.IsDefaultAppDomain();
		if (<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA)
		{
			<Module>.?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
		}
		void* ptr = <Module>._getFiberPtrId();
		int num = 0;
		int num2 = 0;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			while (num2 == 0)
			{
				try
				{
				}
				finally
				{
					IntPtr intPtr = (IntPtr)0;
					IntPtr intPtr2 = (IntPtr)ptr;
					void* ptr2 = (void*)Interlocked.CompareExchange(ref <Module>.__native_startup_lock, intPtr2, intPtr);
					if (ptr2 == null)
					{
						num2 = 1;
					}
					else if (ptr2 == ptr)
					{
						num = 1;
						num2 = 1;
					}
				}
				if (num2 == 0)
				{
					<Module>.Sleep(1000);
				}
			}
			if (!<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA && <Module>.<CrtImplementationDetails>.DefaultDomain.NeedsInitialization() != null)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeDefaultAppDomain(A_0);
			}
		}
		finally
		{
			if (num == 0)
			{
				IntPtr intPtr3 = (IntPtr)0;
				Interlocked.Exchange(ref <Module>.__native_startup_lock, intPtr3);
			}
		}
		<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeVtables(A_0);
		if (<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA)
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeNative(A_0);
			<Module>.<CrtImplementationDetails>.LanguageSupport.InitializePerProcess(A_0);
		}
		<Module>.<CrtImplementationDetails>.LanguageSupport.InitializePerAppDomain(A_0);
		<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 1;
		<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeUninitializer(A_0);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000066F8 File Offset: 0x00005AF8
	internal static void <CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain()
	{
		<Module>._app_exit_callback();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00006710 File Offset: 0x00005B10
	internal unsafe static int <CrtImplementationDetails>.LanguageSupport._UninitializeDefaultDomain(void* cookie)
	{
		<Module>._exit_callback();
		<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		if (<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA)
		{
			<Module>._cexit();
			<Module>.__native_startup_state = (__enative_startup_state)0;
			<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		}
		<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		return 0;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00006750 File Offset: 0x00005B50
	internal static void <CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain()
	{
		if (<Module>.?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA)
		{
			if (AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport._UninitializeDefaultDomain(null);
			}
			else
			{
				<Module>.<CrtImplementationDetails>.DoCallBackInDefaultDomain(<Module>.__unep@?_UninitializeDefaultDomain@LanguageSupport@<CrtImplementationDetails>@@$$FCGJPAX@Z, null);
			}
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x0000678C File Offset: 0x00005B8C
	[PrePrepareMethod]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void <CrtImplementationDetails>.LanguageSupport.DomainUnload(object source, EventArgs arguments)
	{
		if (<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA != 0 && Interlocked.Exchange(ref <Module>.?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA, 1) == 0)
		{
			byte b = ((Interlocked.Decrement(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA) == 0) ? 1 : 0);
			<Module>._app_exit_callback();
			if (b != 0)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain();
			}
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00006BF8 File Offset: 0x00005FF8
	[DebuggerStepThrough]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.Cleanup(LanguageSupport* A_0, global::System.Exception innerException)
	{
		try
		{
			bool flag = ((Interlocked.Decrement(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA) == 0) ? 1 : 0) != 0;
			<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain();
			if (flag)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain();
			}
		}
		catch (global::System.Exception ex)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, ex);
		}
		catch (object obj)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, null);
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00006C74 File Offset: 0x00006074
	[DebuggerStepThrough]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.Initialize(LanguageSupport* A_0)
	{
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load.\n");
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				Interlocked.Increment(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA);
				flag = true;
			}
			<Module>.<CrtImplementationDetails>.LanguageSupport._Initialize(A_0);
		}
		catch (global::System.Exception ex)
		{
			if (flag)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.Cleanup(A_0, ex);
			}
			throw new ModuleLoadException(<Module>.gcroot<System::String\u0020^>..P$AAVString@System@@(A_0), ex);
		}
		catch (object obj)
		{
			if (flag)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.Cleanup(A_0, null);
			}
			throw new ModuleLoadException(<Module>.gcroot<System::String\u0020^>..P$AAVString@System@@(A_0), null);
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00006D80 File Offset: 0x00006180
	[DebuggerStepThrough]
	static unsafe <Module>()
	{
		LanguageSupport languageSupport;
		<Module>.<CrtImplementationDetails>.LanguageSupport.{ctor}(ref languageSupport);
		try
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.Initialize(ref languageSupport);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(<CrtImplementationDetails>.LanguageSupport.{dtor}), (void*)(&languageSupport));
			throw;
		}
		<Module>.gcroot<System::String\u0020^>.{dtor}(ref languageSupport);
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00006D3C File Offset: 0x0000613C
	internal unsafe static LanguageSupport* <CrtImplementationDetails>.LanguageSupport.{ctor}(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.{ctor}(A_0);
		return A_0;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00006D60 File Offset: 0x00006160
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.{dtor}(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.{dtor}(A_0);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000067D0 File Offset: 0x00005BD0
	[DebuggerStepThrough]
	internal unsafe static gcroot<System::String\u0020^>* gcroot<System::String\u0020^>.{ctor}(gcroot<System::String\u0020^>* A_0)
	{
		*A_0 = ((IntPtr)GCHandle.Alloc(null)).ToPointer();
		return A_0;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000067FC File Offset: 0x00005BFC
	[DebuggerStepThrough]
	internal unsafe static void gcroot<System::String\u0020^>.{dtor}(gcroot<System::String\u0020^>* A_0)
	{
		IntPtr intPtr = new IntPtr(*A_0);
		((GCHandle)intPtr).Free();
		*A_0 = 0;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000682C File Offset: 0x00005C2C
	[DebuggerStepThrough]
	internal unsafe static gcroot<System::String\u0020^>* gcroot<System::String\u0020^>.=(gcroot<System::String\u0020^>* A_0, string t)
	{
		IntPtr intPtr = new IntPtr(*A_0);
		((GCHandle)intPtr).Target = t;
		return A_0;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00006858 File Offset: 0x00005C58
	internal unsafe static string gcroot<System::String\u0020^>..P$AAVString@System@@(gcroot<System::String\u0020^>* A_0)
	{
		IntPtr intPtr = new IntPtr(*A_0);
		return ((GCHandle)intPtr).Target;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00006DD8 File Offset: 0x000061D8
	[DebuggerStepThrough]
	internal static ValueType <CrtImplementationDetails>.AtExitLock._handle()
	{
		if (<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA != null)
		{
			IntPtr intPtr = new IntPtr(<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA);
			return GCHandle.FromIntPtr(intPtr);
		}
		return null;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00007314 File Offset: 0x00006714
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Construct(object value)
	{
		<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = null;
		<Module>.<CrtImplementationDetails>.AtExitLock._lock_Set(value);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00006E0C File Offset: 0x0000620C
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Set(object value)
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType == null)
		{
			valueType = GCHandle.Alloc(value);
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = GCHandle.ToIntPtr((GCHandle)valueType).ToPointer();
		}
		else
		{
			((GCHandle)valueType).Target = value;
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00006E60 File Offset: 0x00006260
	[DebuggerStepThrough]
	internal static object <CrtImplementationDetails>.AtExitLock._lock_Get()
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType != null)
		{
			return ((GCHandle)valueType).Target;
		}
		return null;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00006E8C File Offset: 0x0000628C
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Destruct()
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType != null)
		{
			((GCHandle)valueType).Free();
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = null;
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00006EBC File Offset: 0x000062BC
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.AtExitLock.IsInitialized()
	{
		return (<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get() != null) ? 1 : 0;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00007334 File Offset: 0x00006734
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.AddRef()
	{
		if (<Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized() == null)
		{
			object obj = new object();
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = null;
			<Module>.<CrtImplementationDetails>.AtExitLock._lock_Set(obj);
			<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA = 0;
		}
		<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA++;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00006EDC File Offset: 0x000062DC
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.RemoveRef()
	{
		<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA--;
		if (<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA == 0)
		{
			<Module>.<CrtImplementationDetails>.AtExitLock._lock_Destruct();
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00006F08 File Offset: 0x00006308
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.Enter()
	{
		Monitor.Enter(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00006F28 File Offset: 0x00006328
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.Exit()
	{
		Monitor.Exit(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00006F48 File Offset: 0x00006348
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool ?A0x90d41ecd.__global_lock()
	{
		bool flag = false;
		if (<Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized() != null)
		{
			Monitor.Enter(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
			flag = true;
		}
		return flag;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00006F74 File Offset: 0x00006374
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool ?A0x90d41ecd.__global_unlock()
	{
		bool flag = false;
		if (<Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized() != null)
		{
			Monitor.Exit(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
			flag = true;
		}
		return flag;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00007370 File Offset: 0x00006770
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool ?A0x90d41ecd.__alloc_global_lock()
	{
		<Module>.<CrtImplementationDetails>.AtExitLock.AddRef();
		return <Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized();
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00006FA0 File Offset: 0x000063A0
	[DebuggerStepThrough]
	internal static void ?A0x90d41ecd.__dealloc_global_lock()
	{
		<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA--;
		if (<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA == 0)
		{
			<Module>.<CrtImplementationDetails>.AtExitLock._lock_Destruct();
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00006FCC File Offset: 0x000063CC
	internal unsafe static int _atexit_helper(delegate*<void> func, uint* __pexit_list_size, delegate*<void>** __ponexitend, delegate*<void>** __ponexitbegin)
	{
		delegate*<void> system.Void_u0020() = 0;
		if (func == null)
		{
			return -1;
		}
		if (<Module>.?A0x90d41ecd.__global_lock() == 1)
		{
			try
			{
				if (*__pexit_list_size - 1U < (uint)(*(int*)__ponexitend - *(int*)__ponexitbegin) >> 2)
				{
					try
					{
						uint num = *__pexit_list_size * 4U;
						uint num2;
						if (num < 2048U)
						{
							num2 = num;
						}
						else
						{
							num2 = 2048U;
						}
						IntPtr intPtr = new IntPtr((int)(num + num2));
						IntPtr intPtr2 = new IntPtr(*(int*)__ponexitbegin);
						IntPtr intPtr3 = Marshal.ReAllocHGlobal(intPtr2, intPtr);
						*(int*)__ponexitend = *(int*)__ponexitend + (byte*)((byte*)intPtr3.ToPointer() - *(int*)__ponexitbegin);
						*(int*)__ponexitbegin = intPtr3.ToPointer();
						uint num3 = *__pexit_list_size;
						uint num4;
						if (512U < num3)
						{
							num4 = 512U;
						}
						else
						{
							num4 = num3;
						}
						*__pexit_list_size = num3 + num4;
					}
					catch (OutOfMemoryException)
					{
						IntPtr intPtr4 = new IntPtr((int)(*__pexit_list_size * 4U + 8U));
						IntPtr intPtr5 = new IntPtr(*(int*)__ponexitbegin);
						IntPtr intPtr6 = Marshal.ReAllocHGlobal(intPtr5, intPtr4);
						*(int*)__ponexitend = *(int*)__ponexitend + (byte*)((byte*)intPtr6.ToPointer() - *(int*)__ponexitbegin);
						*(int*)__ponexitbegin = intPtr6.ToPointer();
						*__pexit_list_size += 4U;
					}
				}
				*(*(int*)__ponexitend) = func;
				*(int*)__ponexitend = *(int*)__ponexitend + 4;
				system.Void_u0020() = func;
			}
			catch (OutOfMemoryException)
			{
			}
			finally
			{
				<Module>.?A0x90d41ecd.__global_unlock();
			}
			if (system.Void_u0020() != null)
			{
				return 0;
			}
		}
		return -1;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00007128 File Offset: 0x00006528
	internal unsafe static void _exit_callback()
	{
		if (<Module>.?A0x90d41ecd.__exit_list_size != 0U)
		{
			delegate*<void>* ptr = <Module>._decode_pointer((void*)<Module>.?A0x90d41ecd.__onexitbegin_m);
			delegate*<void>* ptr2 = <Module>._decode_pointer((void*)<Module>.?A0x90d41ecd.__onexitend_m);
			if (ptr != -1 && ptr != null && ptr2 != null)
			{
				for (;;)
				{
					ptr2 -= 4 / sizeof(delegate*<void>);
					if (ptr2 < ptr)
					{
						break;
					}
					if (*(int*)ptr2 != <Module>._encoded_null())
					{
						IntPtr intPtr = <Module>._decode_pointer(*(int*)ptr2);
						*(int*)ptr2 = <Module>._encoded_null();
						calli(System.Void(), intPtr);
						ptr = <Module>._decode_pointer((void*)<Module>.?A0x90d41ecd.__onexitbegin_m);
						ptr2 = <Module>._decode_pointer((void*)<Module>.?A0x90d41ecd.__onexitend_m);
					}
				}
				IntPtr intPtr2 = new IntPtr((void*)ptr);
				Marshal.FreeHGlobal(intPtr2);
			}
			<Module>.?A0x90d41ecd.__dealloc_global_lock();
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00007390 File Offset: 0x00006790
	[DebuggerStepThrough]
	internal static int _initatexit_m()
	{
		int num = 0;
		if (<Module>.?A0x90d41ecd.__alloc_global_lock() == 1)
		{
			<Module>.?A0x90d41ecd.__onexitbegin_m = <Module>._encode_pointer(Marshal.AllocHGlobal(128).ToPointer());
			<Module>.?A0x90d41ecd.__onexitend_m = <Module>.?A0x90d41ecd.__onexitbegin_m;
			<Module>.?A0x90d41ecd.__exit_list_size = 32U;
			num = 1;
		}
		return num;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000073DC File Offset: 0x000067DC
	internal unsafe static delegate*<int> _onexit_m(delegate*<int> _Function)
	{
		return (<Module>._atexit_m(_Function) == -1) ? 0 : _Function;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000071B4 File Offset: 0x000065B4
	internal unsafe static int _atexit_m(delegate*<void> func)
	{
		delegate*<void>* ptr = <Module>._decode_pointer((void*)<Module>.?A0x90d41ecd.__onexitbegin_m);
		delegate*<void>* ptr2 = <Module>._decode_pointer((void*)<Module>.?A0x90d41ecd.__onexitend_m);
		int num = <Module>._atexit_helper(<Module>._encode_pointer(func), &<Module>.?A0x90d41ecd.__exit_list_size, &ptr2, &ptr);
		<Module>.?A0x90d41ecd.__onexitbegin_m = <Module>._encode_pointer((void*)ptr);
		<Module>.?A0x90d41ecd.__onexitend_m = <Module>._encode_pointer((void*)ptr2);
		return num;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000073FC File Offset: 0x000067FC
	[DebuggerStepThrough]
	internal static int _initatexit_app_domain()
	{
		if (<Module>.?A0x90d41ecd.__alloc_global_lock() == 1)
		{
			<Module>.__onexitbegin_app_domain = <Module>._encode_pointer(Marshal.AllocHGlobal(128).ToPointer());
			<Module>.__onexitend_app_domain = <Module>.__onexitbegin_app_domain;
			<Module>.__exit_list_size_app_domain = 32U;
		}
		return 1;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00007208 File Offset: 0x00006608
	internal unsafe static void _app_exit_callback()
	{
		if (<Module>.__exit_list_size_app_domain != 0U)
		{
			delegate*<void>* ptr = <Module>._decode_pointer((void*)<Module>.__onexitbegin_app_domain);
			delegate*<void>* ptr2 = <Module>._decode_pointer((void*)<Module>.__onexitend_app_domain);
			try
			{
				if (ptr != -1 && ptr != null && ptr2 != null)
				{
					for (;;)
					{
						ptr2 -= 4 / sizeof(delegate*<void>);
						if (ptr2 < ptr || *(int*)ptr2 != <Module>._encoded_null())
						{
							if (ptr2 < ptr)
							{
								break;
							}
							delegate*<void> system.Void_u0020() = <Module>._decode_pointer(*(int*)ptr2);
							*(int*)ptr2 = <Module>._encoded_null();
							calli(System.Void(), system.Void_u0020());
							ptr = <Module>._decode_pointer((void*)<Module>.__onexitbegin_app_domain);
							ptr2 = <Module>._decode_pointer((void*)<Module>.__onexitend_app_domain);
						}
					}
				}
			}
			finally
			{
				IntPtr intPtr = new IntPtr((void*)ptr);
				Marshal.FreeHGlobal(intPtr);
				<Module>.?A0x90d41ecd.__dealloc_global_lock();
			}
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00007448 File Offset: 0x00006848
	internal unsafe static delegate*<int> _onexit_m_appdomain(delegate*<int> _Function)
	{
		return (<Module>._atexit_m_appdomain(_Function) == -1) ? 0 : _Function;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000072C0 File Offset: 0x000066C0
	[DebuggerStepThrough]
	internal unsafe static int _atexit_m_appdomain(delegate*<void> func)
	{
		delegate*<void>* ptr = <Module>._decode_pointer((void*)<Module>.__onexitbegin_app_domain);
		delegate*<void>* ptr2 = <Module>._decode_pointer((void*)<Module>.__onexitend_app_domain);
		int num = <Module>._atexit_helper(<Module>._encode_pointer(func), &<Module>.__exit_list_size_app_domain, &ptr2, &ptr);
		<Module>.__onexitbegin_app_domain = <Module>._encode_pointer((void*)ptr);
		<Module>.__onexitend_app_domain = <Module>._encode_pointer((void*)ptr2);
		return num;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00007468 File Offset: 0x00006868
	[DebuggerStepThrough]
	internal unsafe static int _initterm_e(delegate* unmanaged[Cdecl, Cdecl]<int>* pfbegin, delegate* unmanaged[Cdecl, Cdecl]<int>* pfend)
	{
		int num = 0;
		if (pfbegin < pfend)
		{
			while (num == 0)
			{
				uint num2 = (uint)(*(int*)pfbegin);
				if (num2 != 0U)
				{
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvCdecl)(), (IntPtr)num2);
				}
				pfbegin += 4 / sizeof(delegate* unmanaged[Cdecl, Cdecl]<int>);
				if (pfbegin >= pfend)
				{
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0000749C File Offset: 0x0000689C
	[DebuggerStepThrough]
	internal unsafe static void _initterm(delegate* unmanaged[Cdecl, Cdecl]<void>* pfbegin, delegate* unmanaged[Cdecl, Cdecl]<void>* pfend)
	{
		if (pfbegin < pfend)
		{
			do
			{
				uint num = (uint)(*(int*)pfbegin);
				if (num != 0U)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvCdecl)(), (IntPtr)num);
				}
				pfbegin += 4 / sizeof(delegate* unmanaged[Cdecl, Cdecl]<void>);
			}
			while (pfbegin < pfend);
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000074C8 File Offset: 0x000068C8
	[DebuggerStepThrough]
	internal static ModuleHandle <CrtImplementationDetails>.ThisModule.Handle()
	{
		return typeof(ThisModule).Module.ModuleHandle;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00007524 File Offset: 0x00006924
	[DebuggerStepThrough]
	internal unsafe static void _initterm_m(delegate*<void*>* pfbegin, delegate*<void*>* pfend)
	{
		if (pfbegin < pfend)
		{
			do
			{
				uint num = (uint)(*(int*)pfbegin);
				if (num != 0U)
				{
					void* ptr = calli(System.Void modopt(System.Runtime.CompilerServices.IsConst)*(), <Module>.<CrtImplementationDetails>.ThisModule.ResolveMethod<void\u0020const\u0020*\u0020__clrcall(void)>(num));
				}
				pfbegin += 4 / sizeof(delegate*<void*>);
			}
			while (pfbegin < pfend);
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000074F0 File Offset: 0x000068F0
	[DebuggerStepThrough]
	internal unsafe static delegate*<void*> <CrtImplementationDetails>.ThisModule.ResolveMethod<void\u0020const\u0020*\u0020__clrcall(void)>(delegate*<void*> methodToken)
	{
		return <Module>.<CrtImplementationDetails>.ThisModule.Handle().ResolveMethodHandle(methodToken).GetFunctionPointer()
			.ToPointer();
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00007558 File Offset: 0x00006958
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void ___CxxCallUnwindDtor(delegate*<void*, void> pDtor, void* pThis)
	{
		try
		{
			calli(System.Void(System.Void*), pThis, pDtor);
		}
		catch when (endfilter(<Module>.__FrameUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000075A4 File Offset: 0x000069A4
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void ___CxxCallUnwindDelDtor(delegate*<void*, void> pDtor, void* pThis)
	{
		try
		{
			calli(System.Void(System.Void*), pThis, pDtor);
		}
		catch when (endfilter(<Module>.__FrameUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000075F0 File Offset: 0x000069F0
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void ___CxxCallUnwindVecDtor(delegate*<void*, uint, int, delegate*<void*, void>, void> pVecDtor, void* ptr, uint size, int count, delegate*<void*, void> pDtor)
	{
		try
		{
			calli(System.Void(System.Void*,System.UInt32,System.Int32,System.Void (System.Void*)), ptr, size, count, pDtor, pVecDtor);
		}
		catch when (endfilter(<Module>.__FrameUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00001860 File Offset: 0x00000C60
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int System.EnterpriseServices.Thunk.InitializeSpy.GetEnabled(InitializeSpy*);

	// Token: 0x06000053 RID: 83 RVA: 0x00001040 File Offset: 0x00000440
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public static extern int System.EnterpriseServices.Thunk.InitSpy();

	// Token: 0x06000054 RID: 84 RVA: 0x00006016 File Offset: 0x00005416
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int InterlockedCompareExchange(int*, int, int);

	// Token: 0x06000055 RID: 85 RVA: 0x00005D20 File Offset: 0x00005120
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int GetContext(_GUID*, void**);

	// Token: 0x06000056 RID: 86 RVA: 0x00005D50 File Offset: 0x00005150
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public static extern int IsDefaultContext();

	// Token: 0x06000057 RID: 87 RVA: 0x0000802E File Offset: 0x0000742E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CloseHandle(void*);

	// Token: 0x06000058 RID: 88 RVA: 0x00005FEC File Offset: 0x000053EC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern delegate* unmanaged[Stdcall, Stdcall]<int> GetProcAddress(HINSTANCE__*, sbyte*);

	// Token: 0x06000059 RID: 89 RVA: 0x00005FF8 File Offset: 0x000053F8
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern uint GetLastError();

	// Token: 0x0600005A RID: 90 RVA: 0x000080A0 File Offset: 0x000074A0
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int LookupAccountSidW(char*, void*, char*, uint*, char*, uint*, int*);

	// Token: 0x0600005B RID: 91 RVA: 0x0000601C File Offset: 0x0000541C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern HINSTANCE__* LoadLibraryW(char*);

	// Token: 0x0600005C RID: 92 RVA: 0x00008028 File Offset: 0x00007428
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* GetCurrentThread();

	// Token: 0x0600005D RID: 93 RVA: 0x000080A6 File Offset: 0x000074A6
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int RegCloseKey(HKEY__*);

	// Token: 0x0600005E RID: 94 RVA: 0x00008058 File Offset: 0x00007458
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern int __CxxQueryExceptionSize();

	// Token: 0x0600005F RID: 95 RVA: 0x00008040 File Offset: 0x00007440
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int __CxxDetectRethrow(void*);

	// Token: 0x06000060 RID: 96 RVA: 0x00005D90 File Offset: 0x00005190
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public static extern uint GetContextToken();

	// Token: 0x06000061 RID: 97 RVA: 0x00008088 File Offset: 0x00007488
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void SysFreeString(char*);

	// Token: 0x06000062 RID: 98 RVA: 0x00008046 File Offset: 0x00007446
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void _CxxThrowException(void*, _s__ThrowInfo*);

	// Token: 0x06000063 RID: 99 RVA: 0x000080AC File Offset: 0x000074AC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int RegOpenKeyExW(HKEY__*, char*, uint, uint, HKEY__**);

	// Token: 0x06000064 RID: 100 RVA: 0x0000803A File Offset: 0x0000743A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void __CxxUnregisterExceptionObject(void*, int);

	// Token: 0x06000065 RID: 101 RVA: 0x00008070 File Offset: 0x00007470
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CoGetStandardMarshal(_GUID*, IUnknown*, uint, void*, uint, IMarshal**);

	// Token: 0x06000066 RID: 102 RVA: 0x00008052 File Offset: 0x00007452
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int __CxxExceptionFilter(void*, void*, int, void*);

	// Token: 0x06000067 RID: 103 RVA: 0x00008259 File Offset: 0x00007659
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public static extern int DllRegisterServer();

	// Token: 0x06000068 RID: 104 RVA: 0x0000808E File Offset: 0x0000748E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int SafeArrayGetElement(tagSAFEARRAY*, int*, void*);

	// Token: 0x06000069 RID: 105 RVA: 0x00005B10 File Offset: 0x00004F10
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int ReleaseMarshaledInterface(byte*, int);

	// Token: 0x0600006A RID: 106 RVA: 0x00008076 File Offset: 0x00007476
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CoCreateInstanceEx(_GUID*, IUnknown*, uint, _COSERVERINFO*, uint, tagMULTI_QI*);

	// Token: 0x0600006B RID: 107 RVA: 0x00008082 File Offset: 0x00007482
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int SafeArrayDestroy(tagSAFEARRAY*);

	// Token: 0x0600006C RID: 108 RVA: 0x00007FC8 File Offset: 0x000073C8
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CoGetMarshalSizeMax(uint*, _GUID*, IUnknown*, uint, void*, uint);

	// Token: 0x0600006D RID: 109 RVA: 0x000059E0 File Offset: 0x00004DE0
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int MarshalInterface(byte*, int, IUnknown*, uint, uint);

	// Token: 0x0600006E RID: 110 RVA: 0x0000804C File Offset: 0x0000744C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int __CxxRegisterExceptionObject(void*, void*);

	// Token: 0x0600006F RID: 111 RVA: 0x00008034 File Offset: 0x00007434
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern HINSTANCE__* GetModuleHandleW(char*);

	// Token: 0x06000070 RID: 112 RVA: 0x00005E50 File Offset: 0x00005250
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public static extern uint GetContextCheck();

	// Token: 0x06000071 RID: 113 RVA: 0x00007FF2 File Offset: 0x000073F2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* GetCurrentProcess();

	// Token: 0x06000072 RID: 114 RVA: 0x0000806A File Offset: 0x0000746A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CoCreateInstance(_GUID*, IUnknown*, uint, _GUID*, void**);

	// Token: 0x06000073 RID: 115 RVA: 0x00005A80 File Offset: 0x00004E80
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int UnmarshalInterface(byte*, int, void**);

	// Token: 0x06000074 RID: 116 RVA: 0x00007FBC File Offset: 0x000073BC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void CoTaskMemFree(void*);

	// Token: 0x06000075 RID: 117 RVA: 0x0000809A File Offset: 0x0000749A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void VariantInit(tagVARIANT*);

	// Token: 0x06000076 RID: 118 RVA: 0x00008094 File Offset: 0x00007494
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int VariantClear(tagVARIANT*);

	// Token: 0x06000077 RID: 119 RVA: 0x0000807C File Offset: 0x0000747C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CoInitializeEx(void*, uint);

	// Token: 0x06000078 RID: 120 RVA: 0x00005F60 File Offset: 0x00005360
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern TransactionStatus* System.EnterpriseServices.Thunk.TransactionStatus.CreateInstance();

	// Token: 0x06000079 RID: 121 RVA: 0x00006107 File Offset: 0x00005507
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _getFiberPtrId();

	// Token: 0x0600007A RID: 122 RVA: 0x00007E78 File Offset: 0x00007278
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern void _amsg_exit(int);

	// Token: 0x0600007B RID: 123 RVA: 0x00007EFD File Offset: 0x000072FD
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public static extern void __security_init_cookie();

	// Token: 0x0600007C RID: 124 RVA: 0x00007FE6 File Offset: 0x000073E6
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern void Sleep(uint);

	// Token: 0x0600007D RID: 125 RVA: 0x000080B2 File Offset: 0x000074B2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CorBindToRuntimeEx(char*, char*, uint, _GUID*, _GUID*, void**);

	// Token: 0x0600007E RID: 126 RVA: 0x0000805E File Offset: 0x0000745E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern void _cexit();

	// Token: 0x0600007F RID: 127 RVA: 0x00007D0E File Offset: 0x0000710E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _encode_pointer(void*);

	// Token: 0x06000080 RID: 128 RVA: 0x00007D26 File Offset: 0x00007126
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _decode_pointer(void*);

	// Token: 0x06000081 RID: 129 RVA: 0x00007D20 File Offset: 0x00007120
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _encoded_null();

	// Token: 0x06000082 RID: 130 RVA: 0x00008064 File Offset: 0x00007464
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int __FrameUnwindFilter(_EXCEPTION_POINTERS*);

	// Token: 0x04000001 RID: 1 RVA: 0x0000A294 File Offset: 0x00008094
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY09$$CBD ??_C@_09IMMMOKKM@ole32?4dll?$AA@;

	// Token: 0x04000002 RID: 2 RVA: 0x0000A27C File Offset: 0x0000807C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BI@$$CBD ??_C@_0BI@MFIKFOCM@CoRegisterInitializeSpy?$AA@;

	// Token: 0x04000003 RID: 3 RVA: 0x0000A264 File Offset: 0x00008064
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BG@$$CBD ??_C@_0BG@DGNPCPPD@CoRevokeInitializeSpy?$AA@;

	// Token: 0x04000004 RID: 4 RVA: 0x0001B9B0 File Offset: 0x000197B0
	// Note: this field is marked with 'hasfieldrva'.
	internal static _s__RTTICompleteObjectLocator ??_R4InitializeSpy@Thunk@EnterpriseServices@System@@6B@;

	// Token: 0x04000005 RID: 5 RVA: 0x0001BA38 File Offset: 0x00019838
	// Note: this field is marked with 'hasfieldrva'.
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@IUnknown@@8;

	// Token: 0x04000006 RID: 6 RVA: 0x0001B9D4 File Offset: 0x000197D4
	// Note: this field is marked with 'hasfieldrva'.
	internal static $_s__RTTIBaseClassArray$_extraBytes_12 ??_R2InitializeSpy@Thunk@EnterpriseServices@System@@8;

	// Token: 0x04000007 RID: 7 RVA: 0x0001BA54 File Offset: 0x00019854
	// Note: this field is marked with 'hasfieldrva'.
	internal static _s__RTTIClassHierarchyDescriptor ??_R3IUnknown@@8;

	// Token: 0x04000008 RID: 8 RVA: 0x0001B9E4 File Offset: 0x000197E4
	// Note: this field is marked with 'hasfieldrva'.
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@InitializeSpy@Thunk@EnterpriseServices@System@@8;

	// Token: 0x04000009 RID: 9 RVA: 0x0001BA64 File Offset: 0x00019864
	// Note: this field is marked with 'hasfieldrva'.
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2IUnknown@@8;

	// Token: 0x0400000A RID: 10 RVA: 0x0001BA1C File Offset: 0x0001981C
	// Note: this field is marked with 'hasfieldrva'.
	internal static _s__RTTIClassHierarchyDescriptor ??_R3IInitializeSpy@@8;

	// Token: 0x0400000B RID: 11 RVA: 0x0001D008 File Offset: 0x0001A808
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0N@Q6AXXZ ??_7InitializeSpy@Thunk@EnterpriseServices@System@@6B@;

	// Token: 0x0400000C RID: 12 RVA: 0x0000A254 File Offset: 0x00008054
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_00000144_0000_0000_c000_000000000046;

	// Token: 0x0400000D RID: 13 RVA: 0x0001D074 File Offset: 0x0001A874
	// Note: this field is marked with 'hasfieldrva'.
	internal static $_TypeDescriptor$_extraBytes_21 ??_R0?AUIInitializeSpy@@@8;

	// Token: 0x0400000E RID: 14 RVA: 0x0001D094 File Offset: 0x0001A894
	// Note: this field is marked with 'hasfieldrva'.
	internal static $_TypeDescriptor$_extraBytes_15 ??_R0?AUIUnknown@@@8;

	// Token: 0x0400000F RID: 15 RVA: 0x0001D464 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static InitializeSpy* System.EnterpriseServices.Thunk.g_pSpy;

	// Token: 0x04000010 RID: 16 RVA: 0x0000A24C File Offset: 0x0000804C
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '1024'.
	internal static int ?BUCKET_COUNT@?$SimpleHashtable@K_K@Thunk@EnterpriseServices@System@@0HB;

	// Token: 0x04000011 RID: 17 RVA: 0x0000A250 File Offset: 0x00008050
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '1024'.
	internal static int ?BUCKET_COUNT@?$SimpleHashtable@_KH@Thunk@EnterpriseServices@System@@0HB;

	// Token: 0x04000012 RID: 18 RVA: 0x0001B9C4 File Offset: 0x000197C4
	// Note: this field is marked with 'hasfieldrva'.
	internal static _s__RTTIClassHierarchyDescriptor ??_R3InitializeSpy@Thunk@EnterpriseServices@System@@8;

	// Token: 0x04000013 RID: 19 RVA: 0x0001BA00 File Offset: 0x00019800
	// Note: this field is marked with 'hasfieldrva'.
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@IInitializeSpy@@8;

	// Token: 0x04000014 RID: 20 RVA: 0x0001D038 File Offset: 0x0001A838
	// Note: this field is marked with 'hasfieldrva'.
	internal static $_TypeDescriptor$_extraBytes_52 ??_R0?AVInitializeSpy@Thunk@EnterpriseServices@System@@@8;

	// Token: 0x04000015 RID: 21 RVA: 0x0001BA2C File Offset: 0x0001982C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2IInitializeSpy@@8;

	// Token: 0x04000016 RID: 22 RVA: 0x0000A2F8 File Offset: 0x000080F8
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_7d40fcc8_f81e_462e_bba1_8a99ebdc826c;

	// Token: 0x04000017 RID: 23 RVA: 0x0000A308 File Offset: 0x00008108
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_02558374_df2e_4dae_bd6b_1d5c994f9bdc;

	// Token: 0x04000018 RID: 24 RVA: 0x0000A2E8 File Offset: 0x000080E8
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_0fb15084_af41_11ce_bd2b_204c4f4f5020;

	// Token: 0x04000019 RID: 25 RVA: 0x0000A31C File Offset: 0x0000811C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0N@$$CB_W ?A0xb1a0d9a8.unnamed-global-0;

	// Token: 0x0400001A RID: 26 RVA: 0x0000A338 File Offset: 0x00008138
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BA@$$CBD ?A0xb1a0d9a8.unnamed-global-1;

	// Token: 0x0400001B RID: 27 RVA: 0x0000A348 File Offset: 0x00008148
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0P@$$CBD ?A0xb1a0d9a8.unnamed-global-2;

	// Token: 0x0400001C RID: 28 RVA: 0x0000A394 File Offset: 0x00008194
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY06$$CBD ?A0x5da4b14d.unnamed-global-0;

	// Token: 0x0400001D RID: 29 RVA: 0x0000A39C File Offset: 0x0000819C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0P@$$CBD ?A0x5da4b14d.unnamed-global-1;

	// Token: 0x0400001E RID: 30 RVA: 0x0000A3AC File Offset: 0x000081AC
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0N@$$CB_W ?A0x5da4b14d.unnamed-global-2;

	// Token: 0x0400001F RID: 31 RVA: 0x0000A3C8 File Offset: 0x000081C8
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BE@$$CBD ?A0x5da4b14d.unnamed-global-3;

	// Token: 0x04000020 RID: 32 RVA: 0x0000A3DC File Offset: 0x000081DC
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0N@$$CB_W ?A0x5da4b14d.unnamed-global-4;

	// Token: 0x04000021 RID: 33 RVA: 0x0000A3F8 File Offset: 0x000081F8
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BB@$$CBD ?A0x5da4b14d.unnamed-global-5;

	// Token: 0x04000022 RID: 34 RVA: 0x0000A40C File Offset: 0x0000820C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY07$$CB_W ?A0x5da4b14d.unnamed-global-6;

	// Token: 0x04000023 RID: 35 RVA: 0x0000A420 File Offset: 0x00008220
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0CN@$$CB_W ?A0x5da4b14d.unnamed-global-7;

	// Token: 0x04000024 RID: 36 RVA: 0x0000A47C File Offset: 0x0000827C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0M@$$CB_W ?A0x5da4b14d.unnamed-global-8;

	// Token: 0x04000025 RID: 37 RVA: 0x0000A494 File Offset: 0x00008294
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BF@$$CBD ?A0x5da4b14d.unnamed-global-9;

	// Token: 0x04000026 RID: 38 RVA: 0x0001D48C File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static volatile int ?A0x5da4b14d.?fWin64@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC;

	// Token: 0x04000027 RID: 39 RVA: 0x0001D0B8 File Offset: 0x0001A8B8
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '4294967295'.
	internal static uint ?A0x5da4b14d.?dwExts@?1??GetManagedExts@Proxy@Thunk@EnterpriseServices@System@@SMHXZ@4KA;

	// Token: 0x04000028 RID: 40 RVA: 0x0001D490 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static volatile int ?A0x5da4b14d.?fInit@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC;

	// Token: 0x04000029 RID: 41 RVA: 0x0000A384 File Offset: 0x00008184
	// Note: this field is marked with 'hasfieldrva'.
	internal static _GUID System.EnterpriseServices.Thunk.?A0x5da4b14d.IID_IObjContext;

	// Token: 0x0400002A RID: 42 RVA: 0x0000A4B0 File Offset: 0x000082B0
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_2732fd59_b2b4_4d44_878c_8b8f09626008;

	// Token: 0x0400002B RID: 43 RVA: 0x0001D488 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static volatile int ?A0x5da4b14d.?fIsWow@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC;

	// Token: 0x0400002C RID: 44 RVA: 0x0000A4AC File Offset: 0x000082AC
	// Note: this field is marked with 'hasfieldrva'.
	unsafe static int** __unep@?SendDestructionEventsCallback@Thunk@EnterpriseServices@System@@$$FYGJPAUtagComCallData@123@@Z;

	// Token: 0x0400002D RID: 45 RVA: 0x0000A4C0 File Offset: 0x000082C0
	// Note: this field is marked with 'hasfieldrva'.
	unsafe static int** __unep@?FilteringCallbackFunction@Thunk@EnterpriseServices@System@@$$FYGJPAUtagComCallData@123@@Z;

	// Token: 0x0400002E RID: 46 RVA: 0x0000A51C File Offset: 0x0000831C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0M@$$CB_W ??_C@_1BI@NMLGLHFF@?$AAc?$AAo?$AAm?$AAs?$AAv?$AAc?$AAs?$AA?4?$AAd?$AAl?$AAl?$AA?$AA@;

	// Token: 0x0400002F RID: 47 RVA: 0x0000A504 File Offset: 0x00008304
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BF@$$CBD ??_C@_0BF@EEGEFJCM@CoEnterServiceDomain?$AA@;

	// Token: 0x04000030 RID: 48 RVA: 0x0000A4EC File Offset: 0x000082EC
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BF@$$CBD ??_C@_0BF@JEIDNIFH@CoLeaveServiceDomain?$AA@;

	// Token: 0x04000031 RID: 49 RVA: 0x0000A4D8 File Offset: 0x000082D8
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY0BB@$$CBD ??_C@_0BB@LLBGKOGP@CoCreateActivity?$AA@;

	// Token: 0x04000032 RID: 50 RVA: 0x0000A4D4 File Offset: 0x000082D4
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00$$CBD ?A0x74e5459c.unnamed-global-0;

	// Token: 0x04000033 RID: 51 RVA: 0x0000A4D5 File Offset: 0x000082D5
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00$$CBD ?A0x74e5459c.unnamed-global-1;

	// Token: 0x04000034 RID: 52 RVA: 0x0000A4D6 File Offset: 0x000082D6
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00$$CBD ?A0x74e5459c.unnamed-global-2;

	// Token: 0x04000035 RID: 53 RVA: 0x0000A4D7 File Offset: 0x000082D7
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00$$CBD ?A0x74e5459c.unnamed-global-3;

	// Token: 0x04000036 RID: 54 RVA: 0x0001D494 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?A0x74e5459c.?fSupportsSysTxn@?1??get_SupportsSysTxn@ServiceConfigThunk@Thunk@EnterpriseServices@System@@Q$AAM_NXZ@4_NA;

	// Token: 0x04000037 RID: 55 RVA: 0x0000A534 File Offset: 0x00008334
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_33caf1a1_fcb8_472b_b45e_967448ded6d8;

	// Token: 0x04000038 RID: 56 RVA: 0x0001D495 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?A0x74e5459c.?fInitialized@?1??get_SupportsSysTxn@ServiceConfigThunk@Thunk@EnterpriseServices@System@@Q$AAM_NXZ@4_NA;

	// Token: 0x04000039 RID: 57 RVA: 0x0000A628 File Offset: 0x00008428
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_90f1a06e_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x0400003A RID: 58 RVA: 0x0000A5E8 File Offset: 0x000083E8
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_cb2f6722_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x0400003B RID: 59 RVA: 0x0000A218 File Offset: 0x00008018
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_mp_z;

	// Token: 0x0400003C RID: 60
	[FixedAddressValueType]
	internal static int ?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x0400003D RID: 61 RVA: 0x0000A1F8 File Offset: 0x00007FF8
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?Uninitialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x0400003E RID: 62 RVA: 0x0000A5F8 File Offset: 0x000083F8
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_00000000_0000_0000_c000_000000000046;

	// Token: 0x0400003F RID: 63 RVA: 0x0000A21C File Offset: 0x0000801C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xi_vt_a;

	// Token: 0x04000040 RID: 64
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x04000041 RID: 65 RVA: 0x0000A20C File Offset: 0x0000800C
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedPerAppDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000042 RID: 66
	[FixedAddressValueType]
	internal static bool ?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA;

	// Token: 0x04000043 RID: 67 RVA: 0x0000A1FC File Offset: 0x00007FFC
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?IsDefaultDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000044 RID: 68 RVA: 0x0000A1F0 File Offset: 0x00007FF0
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_ma_a;

	// Token: 0x04000045 RID: 69
	[FixedAddressValueType]
	internal static Progress.State ?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x04000046 RID: 70 RVA: 0x0000A204 File Offset: 0x00008004
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedNative$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000047 RID: 71
	[FixedAddressValueType]
	internal static int ?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x04000048 RID: 72 RVA: 0x0000A1F4 File Offset: 0x00007FF4
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?Initialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000049 RID: 73 RVA: 0x0000A210 File Offset: 0x00008010
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_ma_z;

	// Token: 0x0400004A RID: 74
	[FixedAddressValueType]
	internal static Progress.State ?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x0400004B RID: 75 RVA: 0x0000A200 File Offset: 0x00008000
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedVtables$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x0400004C RID: 76 RVA: 0x0000A608 File Offset: 0x00008408
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_cb2f6723_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x0400004D RID: 77 RVA: 0x0000A220 File Offset: 0x00008020
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xi_vt_z;

	// Token: 0x0400004E RID: 78
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x0400004F RID: 79 RVA: 0x0000A208 File Offset: 0x00008008
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedPerProcess$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000050 RID: 80 RVA: 0x0001D5FB File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x04000051 RID: 81 RVA: 0x0001D5F8 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x04000052 RID: 82 RVA: 0x0001D5F9 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x04000053 RID: 83 RVA: 0x0001D5F4 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static int ?Count@AllDomains@<CrtImplementationDetails>@@2HA;

	// Token: 0x04000054 RID: 84 RVA: 0x0000A5D8 File Offset: 0x000083D8
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '1'.
	internal static uint ?ProcessAttach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x04000055 RID: 85 RVA: 0x0000A5DC File Offset: 0x000083DC
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '2'.
	internal static uint ?ThreadAttach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x04000056 RID: 86 RVA: 0x0001D198 File Offset: 0x0001A998
	// Note: this field is marked with 'hasfieldrva'.
	internal static TriBool.State ?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x04000057 RID: 87 RVA: 0x0000A5D4 File Offset: 0x000083D4
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '0'.
	internal static uint ?ProcessDetach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x04000058 RID: 88 RVA: 0x0000A5E0 File Offset: 0x000083E0
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '3'.
	internal static uint ?ThreadDetach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x04000059 RID: 89 RVA: 0x0000A5E4 File Offset: 0x000083E4
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '4'.
	internal static uint ?ProcessVerifier@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x0400005A RID: 90 RVA: 0x0001D194 File Offset: 0x0001A994
	// Note: this field is marked with 'hasfieldrva'.
	internal static TriBool.State ?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x0400005B RID: 91 RVA: 0x0001D5FA File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x0400005C RID: 92 RVA: 0x0000A214 File Offset: 0x00008014
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_mp_a;

	// Token: 0x0400005D RID: 93 RVA: 0x0000A618 File Offset: 0x00008418
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_90f1a06c_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x0400005E RID: 94 RVA: 0x0000A638 File Offset: 0x00008438
	// Note: this field is marked with 'hasfieldrva'.
	public unsafe static int** __unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x0400005F RID: 95 RVA: 0x0000A63C File Offset: 0x0000843C
	// Note: this field is marked with 'hasfieldrva'.
	public unsafe static int** __unep@?_UninitializeDefaultDomain@LanguageSupport@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x04000060 RID: 96
	[FixedAddressValueType]
	internal static uint __exit_list_size_app_domain;

	// Token: 0x04000061 RID: 97
	[FixedAddressValueType]
	internal unsafe static delegate*<void>* __onexitbegin_app_domain;

	// Token: 0x04000062 RID: 98 RVA: 0x0001D73C File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static uint ?A0x90d41ecd.__exit_list_size;

	// Token: 0x04000063 RID: 99
	[FixedAddressValueType]
	internal unsafe static delegate*<void>* __onexitend_app_domain;

	// Token: 0x04000064 RID: 100 RVA: 0x0001D734 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void>* ?A0x90d41ecd.__onexitbegin_m;

	// Token: 0x04000065 RID: 101 RVA: 0x0001D738 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void>* ?A0x90d41ecd.__onexitend_m;

	// Token: 0x04000066 RID: 102
	[FixedAddressValueType]
	internal unsafe static void* ?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA;

	// Token: 0x04000067 RID: 103
	[FixedAddressValueType]
	internal static int ?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA;

	// Token: 0x04000068 RID: 104 RVA: 0x0000A644 File Offset: 0x00008444
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY01Q6AXXZ ??_7type_info@@6B@;

	// Token: 0x04000069 RID: 105 RVA: 0x0000A6A4 File Offset: 0x000084A4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IComThreadingInfo;

	// Token: 0x0400006A RID: 106 RVA: 0x0000A674 File Offset: 0x00008474
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IUnknown;

	// Token: 0x0400006B RID: 107 RVA: 0x0000A684 File Offset: 0x00008484
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IInitializeSpy;

	// Token: 0x0400006C RID: 108 RVA: 0x0000BC84 File Offset: 0x00009A84
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IObjectContext;

	// Token: 0x0400006D RID: 109 RVA: 0x0000BB74 File Offset: 0x00009974
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IContextState;

	// Token: 0x0400006E RID: 110 RVA: 0x0000BBD4 File Offset: 0x000099D4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IObjectContextInfo;

	// Token: 0x0400006F RID: 111 RVA: 0x0000A884 File Offset: 0x00008684
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IGlobalInterfaceTable;

	// Token: 0x04000070 RID: 112 RVA: 0x0000CEF4 File Offset: 0x0000ACF4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServicedComponentInfo;

	// Token: 0x04000071 RID: 113 RVA: 0x0000CC94 File Offset: 0x0000AA94
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID CLSID_StdGlobalInterfaceTable;

	// Token: 0x04000072 RID: 114 RVA: 0x0000CEE4 File Offset: 0x0000ACE4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IContextCallback;

	// Token: 0x04000073 RID: 115 RVA: 0x0000AED4 File Offset: 0x00008CD4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IEnterActivityWithNoLock;

	// Token: 0x04000074 RID: 116 RVA: 0x0000B924 File Offset: 0x00009724
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IManagedActivationEvents;

	// Token: 0x04000075 RID: 117 RVA: 0x0000CF04 File Offset: 0x0000AD04
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IRemoteDispatch;

	// Token: 0x04000076 RID: 118 RVA: 0x0000BAE4 File Offset: 0x000098E4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_ICrmMonitor;

	// Token: 0x04000077 RID: 119 RVA: 0x0000BB34 File Offset: 0x00009934
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_ICrmLogControl;

	// Token: 0x04000078 RID: 120 RVA: 0x0000BE94 File Offset: 0x00009C94
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID CLSID_CRMRecoveryClerk;

	// Token: 0x04000079 RID: 121 RVA: 0x0000BEA4 File Offset: 0x00009CA4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID CLSID_CRMClerk;

	// Token: 0x0400007A RID: 122 RVA: 0x0000BB04 File Offset: 0x00009904
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_ICrmMonitorLogRecords;

	// Token: 0x0400007B RID: 123 RVA: 0x0000B9F4 File Offset: 0x000097F4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceActivity;

	// Token: 0x0400007C RID: 124 RVA: 0x0000BA34 File Offset: 0x00009834
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceTrackerConfig;

	// Token: 0x0400007D RID: 125 RVA: 0x0000BA54 File Offset: 0x00009854
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceTransactionConfig;

	// Token: 0x0400007E RID: 126 RVA: 0x0000BA84 File Offset: 0x00009884
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceInheritanceConfig;

	// Token: 0x0400007F RID: 127 RVA: 0x0000BA74 File Offset: 0x00009874
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceThreadPoolConfig;

	// Token: 0x04000080 RID: 128 RVA: 0x0000BAB4 File Offset: 0x000098B4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceComTIIntrinsicsConfig;

	// Token: 0x04000081 RID: 129 RVA: 0x0000BAA4 File Offset: 0x000098A4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceSxsConfig;

	// Token: 0x04000082 RID: 130 RVA: 0x0000BA44 File Offset: 0x00009844
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceSynchronizationConfig;

	// Token: 0x04000083 RID: 131 RVA: 0x0000BF54 File Offset: 0x00009D54
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID CLSID_CServiceConfig;

	// Token: 0x04000084 RID: 132 RVA: 0x0000BAC4 File Offset: 0x000098C4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceIISIntrinsicsConfig;

	// Token: 0x04000085 RID: 133 RVA: 0x0000BA24 File Offset: 0x00009824
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServicePartitionConfig;

	// Token: 0x04000086 RID: 134 RVA: 0x0000BA14 File Offset: 0x00009814
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IServiceCall;

	// Token: 0x04000087 RID: 135 RVA: 0x0000A1E0 File Offset: 0x00007FE0
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AXXZ __xc_z;

	// Token: 0x04000088 RID: 136 RVA: 0x0001D2F4 File Offset: 0x0001AAF4
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '4294967295'.
	public static volatile uint __native_vcclrit_reason;

	// Token: 0x04000089 RID: 137 RVA: 0x0000A1DC File Offset: 0x00007FDC
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AXXZ __xc_a;

	// Token: 0x0400008A RID: 138 RVA: 0x0000A1E4 File Offset: 0x00007FE4
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AHXZ __xi_a;

	// Token: 0x0400008B RID: 139 RVA: 0x0001DAB4 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	public static volatile __enative_startup_state __native_startup_state;

	// Token: 0x0400008C RID: 140 RVA: 0x0000A1EC File Offset: 0x00007FEC
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AHXZ __xi_z;

	// Token: 0x0400008D RID: 141 RVA: 0x0001DAB8 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	public unsafe static void* __native_startup_lock;

	// Token: 0x0400008E RID: 142 RVA: 0x0001D2F0 File Offset: 0x0001AAF0
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '4294967295'.
	public static volatile uint __native_dllmain_reason;
}
