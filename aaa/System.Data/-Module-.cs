using System;
using System.Diagnostics;
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
	// Token: 0x06000001 RID: 1 RVA: 0x001C3EFC File Offset: 0x001C32FC
	internal unsafe static SNI_CLIENT_CONSUMER_INFO* SNI_CLIENT_CONSUMER_INFO.{ctor}(SNI_CLIENT_CONSUMER_INFO* A_0)
	{
		*(A_0 + 32) = 0;
		*(A_0 + 36) = 0;
		*(A_0 + 40) = 0;
		*(A_0 + 44) = 0;
		*(A_0 + 48) = 0;
		*(A_0 + 52) = 0;
		*(A_0 + 56) = 0;
		*(A_0 + 60) = 0;
		*(A_0 + 64) = -1;
		*(A_0 + 68) = 0;
		return A_0;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x001C3F48 File Offset: 0x001C3348
	internal static ref char PtrToStringChars(string s)
	{
		ref byte ptr = s;
		if ((ref ptr) != null)
		{
			ptr = RuntimeHelpers.OffsetToStringData + (ref ptr);
		}
		return ref ptr;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x001C54A8 File Offset: 0x001C48A8
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

	// Token: 0x06000004 RID: 4 RVA: 0x001C5B38 File Offset: 0x001C4F38
	internal static void <CrtImplementationDetails>.ThrowNestedModuleLoadException(Exception innerException, Exception nestedException)
	{
		throw new ModuleLoadExceptionHandlerException("A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n", innerException, nestedException);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x001C56FC File Offset: 0x001C4AFC
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

	// Token: 0x06000006 RID: 6 RVA: 0x001C5788 File Offset: 0x001C4B88
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

	// Token: 0x06000007 RID: 7 RVA: 0x001C57EC File Offset: 0x001C4BEC
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

	// Token: 0x06000008 RID: 8 RVA: 0x001C5874 File Offset: 0x001C4C74
	internal unsafe static int <CrtImplementationDetails>.DefaultDomain.DoNothing(void* cookie)
	{
		GC.KeepAlive(int.MaxValue);
		return 0;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x001C5898 File Offset: 0x001C4C98
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

	// Token: 0x0600000A RID: 10 RVA: 0x001C58EC File Offset: 0x001C4CEC
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

	// Token: 0x0600000B RID: 11 RVA: 0x001C596C File Offset: 0x001C4D6C
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

	// Token: 0x0600000C RID: 12 RVA: 0x001C59A8 File Offset: 0x001C4DA8
	internal static void <CrtImplementationDetails>.DefaultDomain.Initialize()
	{
		<Module>.<CrtImplementationDetails>.DoCallBackInDefaultDomain(<Module>.__unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z, null);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x002B898C File Offset: 0x002B7D8C
	internal static void ?A0xa1cb2edd.??__E?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x002B89A0 File Offset: 0x002B7DA0
	internal static void ?A0xa1cb2edd.??__E?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x002B89B4 File Offset: 0x002B7DB4
	internal static void ?A0xa1cb2edd.??__E?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA@@YMXXZ()
	{
		<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA = false;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x002B89C8 File Offset: 0x002B7DC8
	internal static void ?A0xa1cb2edd.??__E?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x002B89DC File Offset: 0x002B7DDC
	internal static void ?A0xa1cb2edd.??__E?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x002B89F0 File Offset: 0x002B7DF0
	internal static void ?A0xa1cb2edd.??__E?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x002B8A04 File Offset: 0x002B7E04
	internal static void ?A0xa1cb2edd.??__E?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x001C5BB0 File Offset: 0x001C4FB0
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeVtables(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during vtable initialization.\n");
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initterm_m((delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xi_vt_a), (delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xi_vt_z));
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x001C5BE4 File Offset: 0x001C4FE4
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeDefaultAppDomain(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load while attempting to initialize the default appdomain.\n");
		<Module>.<CrtImplementationDetails>.DefaultDomain.Initialize();
	}

	// Token: 0x06000016 RID: 22 RVA: 0x001C5C04 File Offset: 0x001C5004
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

	// Token: 0x06000017 RID: 23 RVA: 0x001C5CA0 File Offset: 0x001C50A0
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

	// Token: 0x06000018 RID: 24 RVA: 0x001C5CE0 File Offset: 0x001C50E0
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializePerAppDomain(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during appdomain initialization.\n");
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initatexit_app_domain();
		<Module>._initterm_m((delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xc_ma_a), (delegate*<void*>*)(&<Module>.?A0xa1cb2edd.__xc_ma_z));
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x001C5D1C File Offset: 0x001C511C
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeUninitializer(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during registration for the unload events.\n");
		EventHandler eventHandler = new EventHandler(<Module>.<CrtImplementationDetails>.LanguageSupport.DomainUnload);
		ModuleUninitializer._ModuleUninitializer.AddHandler(eventHandler);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x001C5D50 File Offset: 0x001C5150
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

	// Token: 0x0600001B RID: 27 RVA: 0x001C59C0 File Offset: 0x001C4DC0
	internal static void <CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain()
	{
		<Module>._app_exit_callback();
	}

	// Token: 0x0600001C RID: 28 RVA: 0x001C59D4 File Offset: 0x001C4DD4
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

	// Token: 0x0600001D RID: 29 RVA: 0x001C5A14 File Offset: 0x001C4E14
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

	// Token: 0x0600001E RID: 30 RVA: 0x001C5A48 File Offset: 0x001C4E48
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[PrePrepareMethod]
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

	// Token: 0x0600001F RID: 31 RVA: 0x001C5E60 File Offset: 0x001C5260
	[DebuggerStepThrough]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.Cleanup(LanguageSupport* A_0, Exception innerException)
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
		catch (Exception ex)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, ex);
		}
		catch (object obj)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, null);
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x001C5ED4 File Offset: 0x001C52D4
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[DebuggerStepThrough]
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
		catch (Exception ex)
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

	// Token: 0x06000021 RID: 33 RVA: 0x001C5FC8 File Offset: 0x001C53C8
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

	// Token: 0x06000022 RID: 34 RVA: 0x001C5F94 File Offset: 0x001C5394
	internal unsafe static LanguageSupport* <CrtImplementationDetails>.LanguageSupport.{ctor}(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.{ctor}(A_0);
		return A_0;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x001C5FB0 File Offset: 0x001C53B0
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.{dtor}(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.{dtor}(A_0);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x001C5A84 File Offset: 0x001C4E84
	[DebuggerStepThrough]
	internal unsafe static gcroot<System::String\u0020^>* gcroot<System::String\u0020^>.{ctor}(gcroot<System::String\u0020^>* A_0)
	{
		*A_0 = ((IntPtr)GCHandle.Alloc(null)).ToPointer();
		return A_0;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x001C5AA8 File Offset: 0x001C4EA8
	[DebuggerStepThrough]
	internal unsafe static void gcroot<System::String\u0020^>.{dtor}(gcroot<System::String\u0020^>* A_0)
	{
		IntPtr intPtr = new IntPtr(*A_0);
		((GCHandle)intPtr).Free();
		*A_0 = 0;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x001C5AD0 File Offset: 0x001C4ED0
	[DebuggerStepThrough]
	internal unsafe static gcroot<System::String\u0020^>* gcroot<System::String\u0020^>.=(gcroot<System::String\u0020^>* A_0, string t)
	{
		IntPtr intPtr = new IntPtr(*A_0);
		((GCHandle)intPtr).Target = t;
		return A_0;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x001C5AF8 File Offset: 0x001C4EF8
	internal unsafe static string gcroot<System::String\u0020^>..P$AAVString@System@@(gcroot<System::String\u0020^>* A_0)
	{
		IntPtr intPtr = new IntPtr(*A_0);
		return ((GCHandle)intPtr).Target;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x001C6018 File Offset: 0x001C5418
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

	// Token: 0x06000029 RID: 41 RVA: 0x001C6048 File Offset: 0x001C5448
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

	// Token: 0x0600002A RID: 42 RVA: 0x001C6098 File Offset: 0x001C5498
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

	// Token: 0x0600002B RID: 43 RVA: 0x001C60BC File Offset: 0x001C54BC
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

	// Token: 0x0600002C RID: 44 RVA: 0x001C60E4 File Offset: 0x001C54E4
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.AtExitLock.IsInitialized()
	{
		return (<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get() != null) ? 1 : 0;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x001C625C File Offset: 0x001C565C
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

	// Token: 0x0600002E RID: 46 RVA: 0x001C6294 File Offset: 0x001C5694
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool ?A0x90d41ecd.__alloc_global_lock()
	{
		<Module>.<CrtImplementationDetails>.AtExitLock.AddRef();
		return <Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x001C6100 File Offset: 0x001C5500
	[DebuggerStepThrough]
	internal static void ?A0x90d41ecd.__dealloc_global_lock()
	{
		<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA--;
		if (<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA == 0)
		{
			<Module>.<CrtImplementationDetails>.AtExitLock._lock_Destruct();
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x001C6128 File Offset: 0x001C5528
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

	// Token: 0x06000031 RID: 49 RVA: 0x001C62AC File Offset: 0x001C56AC
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

	// Token: 0x06000032 RID: 50 RVA: 0x001C62F4 File Offset: 0x001C56F4
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

	// Token: 0x06000033 RID: 51 RVA: 0x001C61AC File Offset: 0x001C55AC
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

	// Token: 0x06000034 RID: 52 RVA: 0x001C6338 File Offset: 0x001C5738
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

	// Token: 0x06000035 RID: 53 RVA: 0x001C6368 File Offset: 0x001C5768
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

	// Token: 0x06000036 RID: 54 RVA: 0x001C6390 File Offset: 0x001C5790
	[DebuggerStepThrough]
	internal static ModuleHandle <CrtImplementationDetails>.ThisModule.Handle()
	{
		return typeof(ThisModule).Module.ModuleHandle;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x001C63E0 File Offset: 0x001C57E0
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

	// Token: 0x06000038 RID: 56 RVA: 0x001C63B4 File Offset: 0x001C57B4
	[DebuggerStepThrough]
	internal unsafe static delegate*<void*> <CrtImplementationDetails>.ThisModule.ResolveMethod<void\u0020const\u0020*\u0020__clrcall(void)>(delegate*<void*> methodToken)
	{
		return <Module>.<CrtImplementationDetails>.ThisModule.Handle().ResolveMethodHandle(methodToken).GetFunctionPointer()
			.ToPointer();
	}

	// Token: 0x06000039 RID: 57 RVA: 0x001C640C File Offset: 0x001C580C
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

	// Token: 0x0600003A RID: 58 RVA: 0x001C3F9E File Offset: 0x001C339E
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* SqlDependencyProcessDispatcherStorage.NativeGetData(int*);

	// Token: 0x0600003B RID: 59 RVA: 0x001C3FD6 File Offset: 0x001C33D6
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe static extern bool SqlDependencyProcessDispatcherStorage.NativeSetData(void*, int);

	// Token: 0x0600003C RID: 60 RVA: 0x001C4066 File Offset: 0x001C3466
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern IUnknown* SqlDependencyProcessDispatcherStorage.NativeGetDefaultAppDomain();

	// Token: 0x0600003D RID: 61 RVA: 0x001B9D93 File Offset: 0x001B9193
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int SNIServerEnumRead(void*, ushort*, int, int*);

	// Token: 0x0600003E RID: 62 RVA: 0x001BABD7 File Offset: 0x001B9FD7
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* SNIServerEnumOpen(ushort*, int);

	// Token: 0x0600003F RID: 63 RVA: 0x0019D196 File Offset: 0x0019C596
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern SNI_Conn* SNIPacketGetConnection(SNI_Packet*);

	// Token: 0x06000040 RID: 64 RVA: 0x001AF37A File Offset: 0x001AE77A
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIInitialize(void*);

	// Token: 0x06000041 RID: 65 RVA: 0x001AD181 File Offset: 0x001AC581
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIWriteSync(SNI_Conn*, SNI_Packet*, SNI_ProvInfo*);

	// Token: 0x06000042 RID: 66 RVA: 0x001AD69C File Offset: 0x001ACA9C
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIRemoveProvider(SNI_Conn*, ProviderNum);

	// Token: 0x06000043 RID: 67 RVA: 0x001B1B19 File Offset: 0x001B0F19
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNISecGenClientContext(SNI_Conn*, byte*, uint, byte*, uint*, int*, sbyte*, uint, ushort*, ushort*);

	// Token: 0x06000044 RID: 68 RVA: 0x001B0E23 File Offset: 0x001B0223
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void SNIGetLastError(SNI_ERROR*);

	// Token: 0x06000045 RID: 69 RVA: 0x001B0559 File Offset: 0x001AF959
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIOpen(SNI_CONSUMER_INFO*, sbyte*, void*, SNI_Conn**, int);

	// Token: 0x06000046 RID: 70 RVA: 0x001AE633 File Offset: 0x001ADA33
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIAddProvider(SNI_Conn*, ProviderNum, void*);

	// Token: 0x06000047 RID: 71 RVA: 0x001A3F91 File Offset: 0x001A3391
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIOpenSyncEx(SNI_CLIENT_CONSUMER_INFO*, SNI_Conn**);

	// Token: 0x06000048 RID: 72 RVA: 0x001AF729 File Offset: 0x001AEB29
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIReadAsync(SNI_Conn*, SNI_Packet**, void*);

	// Token: 0x06000049 RID: 73 RVA: 0x0019D16F File Offset: 0x0019C56F
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void SNIPacketSetData(SNI_Packet*, byte*, uint);

	// Token: 0x0600004A RID: 74 RVA: 0x0019D12D File Offset: 0x0019C52D
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void SNIPacketReset(SNI_Conn*, uint, SNI_Packet*);

	// Token: 0x0600004B RID: 75 RVA: 0x001AD0D0 File Offset: 0x001AC4D0
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIReadSync(SNI_Conn*, SNI_Packet**, int);

	// Token: 0x0600004C RID: 76 RVA: 0x001AD3B4 File Offset: 0x001AC7B4
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIWaitForSSLHandshakeToComplete(SNI_Conn*, uint);

	// Token: 0x0600004D RID: 77 RVA: 0x0019D152 File Offset: 0x0019C552
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void SNIPacketGetData(SNI_Packet*, byte**, uint*);

	// Token: 0x0600004E RID: 78 RVA: 0x001AD443 File Offset: 0x001AC843
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNISetInfo(SNI_Conn*, uint, void*);

	// Token: 0x0600004F RID: 79 RVA: 0x001B0835 File Offset: 0x001AFC35
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIWriteAsync(SNI_Conn*, SNI_Packet*, SNI_ProvInfo*);

	// Token: 0x06000050 RID: 80 RVA: 0x001AD227 File Offset: 0x001AC627
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNIQueryInfo(uint, void*);

	// Token: 0x06000051 RID: 81 RVA: 0x001B2825 File Offset: 0x001B1C25
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern uint SNISecInitPackage(uint*);

	// Token: 0x06000052 RID: 82 RVA: 0x001C54FD File Offset: 0x001C48FD
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _getFiberPtrId();

	// Token: 0x06000053 RID: 83 RVA: 0x001C6BA8 File Offset: 0x001C5FA8
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern void _amsg_exit(int);

	// Token: 0x06000054 RID: 84 RVA: 0x001C6BD4 File Offset: 0x001C5FD4
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Native)]
	public static extern void __security_init_cookie();

	// Token: 0x06000055 RID: 85 RVA: 0x001C6D96 File Offset: 0x001C6196
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern void Sleep(uint);

	// Token: 0x06000056 RID: 86 RVA: 0x001C6D90 File Offset: 0x001C6190
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CorBindToRuntimeEx(char*, char*, uint, _GUID*, _GUID*, void**);

	// Token: 0x06000057 RID: 87 RVA: 0x001C6DAE File Offset: 0x001C61AE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public static extern void _cexit();

	// Token: 0x06000058 RID: 88 RVA: 0x001C6DBA File Offset: 0x001C61BA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int CoCreateInstance(_GUID*, IUnknown*, uint, _GUID*, void**);

	// Token: 0x06000059 RID: 89 RVA: 0x001C6A4A File Offset: 0x001C5E4A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _encode_pointer(void*);

	// Token: 0x0600005A RID: 90 RVA: 0x001C6A56 File Offset: 0x001C5E56
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _decode_pointer(void*);

	// Token: 0x0600005B RID: 91 RVA: 0x001C6A50 File Offset: 0x001C5E50
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern void* _encoded_null();

	// Token: 0x0600005C RID: 92 RVA: 0x001C6DB4 File Offset: 0x001C61B4
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
	public unsafe static extern int __FrameUnwindFilter(_EXCEPTION_POINTERS*);

	// Token: 0x04000001 RID: 1 RVA: 0x002BD8C4 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static volatile int ?lock@SqlDependencyProcessDispatcherStorage@@0JC;

	// Token: 0x04000002 RID: 2 RVA: 0x002BDBEC File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static void* ?data@SqlDependencyProcessDispatcherStorage@@0PAXA;

	// Token: 0x04000003 RID: 3 RVA: 0x000140C0 File Offset: 0x000134C0
	// Note: this field is marked with 'hasfieldrva'.
	internal static _GUID IID_ICorRuntimeHost;

	// Token: 0x04000004 RID: 4 RVA: 0x000140B0 File Offset: 0x000134B0
	// Note: this field is marked with 'hasfieldrva'.
	internal static _GUID CLSID_CorRuntimeHost;

	// Token: 0x04000005 RID: 5 RVA: 0x002BDC38 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static int ?size@SqlDependencyProcessDispatcherStorage@@0HA;

	// Token: 0x04000006 RID: 6 RVA: 0x000140D0 File Offset: 0x000134D0
	// Note: this field is marked with 'hasfieldrva'.
	unsafe static int** __unep@?SNIServerEnumClose@@$$J14YGXPAX@Z;

	// Token: 0x04000007 RID: 7 RVA: 0x000140D4 File Offset: 0x000134D4
	// Note: this field is marked with 'hasfieldrva'.
	unsafe static int** __unep@?SNIClose@@$$J14YGKPAVSNI_Conn@@@Z;

	// Token: 0x04000008 RID: 8 RVA: 0x000140D8 File Offset: 0x000134D8
	// Note: this field is marked with 'hasfieldrva'.
	unsafe static int** __unep@?SNIPacketAllocate@@$$J18YGPAVSNI_Packet@@PAVSNI_Conn@@K@Z;

	// Token: 0x04000009 RID: 9 RVA: 0x000140DC File Offset: 0x000134DC
	// Note: this field is marked with 'hasfieldrva'.
	unsafe static int** __unep@?SNIPacketRelease@@$$J14YGXPAVSNI_Packet@@@Z;

	// Token: 0x0400000A RID: 10 RVA: 0x000140E0 File Offset: 0x000134E0
	// Note: this field is marked with 'hasfieldrva'.
	unsafe static int** __unep@?SNITerminate@@$$J10YGKXZ;

	// Token: 0x0400000B RID: 11 RVA: 0x00014210 File Offset: 0x00013610
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_90f1a06e_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x0400000C RID: 12 RVA: 0x000141D0 File Offset: 0x000135D0
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_cb2f6722_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x0400000D RID: 13 RVA: 0x00001314 File Offset: 0x00000714
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_mp_z;

	// Token: 0x0400000E RID: 14
	[FixedAddressValueType]
	internal static int ?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x0400000F RID: 15 RVA: 0x000012F4 File Offset: 0x000006F4
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?Uninitialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000010 RID: 16 RVA: 0x000141E0 File Offset: 0x000135E0
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_00000000_0000_0000_c000_000000000046;

	// Token: 0x04000011 RID: 17 RVA: 0x00001318 File Offset: 0x00000718
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xi_vt_a;

	// Token: 0x04000012 RID: 18
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x04000013 RID: 19 RVA: 0x00001308 File Offset: 0x00000708
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedPerAppDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000014 RID: 20
	[FixedAddressValueType]
	internal static bool ?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA;

	// Token: 0x04000015 RID: 21 RVA: 0x000012F8 File Offset: 0x000006F8
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?IsDefaultDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000016 RID: 22 RVA: 0x000012EC File Offset: 0x000006EC
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_ma_a;

	// Token: 0x04000017 RID: 23
	[FixedAddressValueType]
	internal static Progress.State ?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x04000018 RID: 24 RVA: 0x00001300 File Offset: 0x00000700
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedNative$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000019 RID: 25
	[FixedAddressValueType]
	internal static int ?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x0400001A RID: 26 RVA: 0x000012F0 File Offset: 0x000006F0
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?Initialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x0400001B RID: 27 RVA: 0x0000130C File Offset: 0x0000070C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_ma_z;

	// Token: 0x0400001C RID: 28
	[FixedAddressValueType]
	internal static Progress.State ?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x0400001D RID: 29 RVA: 0x000012FC File Offset: 0x000006FC
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedVtables$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x0400001E RID: 30 RVA: 0x000141F0 File Offset: 0x000135F0
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_cb2f6723_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x0400001F RID: 31 RVA: 0x0000131C File Offset: 0x0000071C
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xi_vt_z;

	// Token: 0x04000020 RID: 32
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x04000021 RID: 33 RVA: 0x00001304 File Offset: 0x00000704
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void> ?A0xa1cb2edd.?InitializedPerProcess$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000022 RID: 34 RVA: 0x002BE11F File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x04000023 RID: 35 RVA: 0x002BE11C File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x04000024 RID: 36 RVA: 0x002BE11D File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x04000025 RID: 37 RVA: 0x002BE118 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static int ?Count@AllDomains@<CrtImplementationDetails>@@2HA;

	// Token: 0x04000026 RID: 38 RVA: 0x002BA394 File Offset: 0x002B9394
	// Note: this field is marked with 'hasfieldrva'.
	internal static TriBool.State ?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x04000027 RID: 39 RVA: 0x002BA390 File Offset: 0x002B9390
	// Note: this field is marked with 'hasfieldrva'.
	internal static TriBool.State ?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x04000028 RID: 40 RVA: 0x002BE11E File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of 'True'.
	internal static bool ?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x04000029 RID: 41 RVA: 0x00001310 File Offset: 0x00000710
	// Note: this field is marked with 'hasfieldrva'.
	internal static $ArrayType$$$BY00Q6MPBXXZ ?A0xa1cb2edd.__xc_mp_a;

	// Token: 0x0400002A RID: 42 RVA: 0x00014200 File Offset: 0x00013600
	// Note: this field is marked with 'hasfieldrva'.
	internal static __s_GUID _GUID_90f1a06c_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x0400002B RID: 43 RVA: 0x00014220 File Offset: 0x00013620
	// Note: this field is marked with 'hasfieldrva'.
	public unsafe static int** __unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x0400002C RID: 44 RVA: 0x00014224 File Offset: 0x00013624
	// Note: this field is marked with 'hasfieldrva'.
	public unsafe static int** __unep@?_UninitializeDefaultDomain@LanguageSupport@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x0400002D RID: 45
	[FixedAddressValueType]
	internal static uint __exit_list_size_app_domain;

	// Token: 0x0400002E RID: 46
	[FixedAddressValueType]
	internal unsafe static delegate*<void>* __onexitbegin_app_domain;

	// Token: 0x0400002F RID: 47 RVA: 0x002BE264 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '9460301'.
	internal static uint ?A0x90d41ecd.__exit_list_size;

	// Token: 0x04000030 RID: 48
	[FixedAddressValueType]
	internal unsafe static delegate*<void>* __onexitend_app_domain;

	// Token: 0x04000031 RID: 49 RVA: 0x002BE25C File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void>* ?A0x90d41ecd.__onexitbegin_m;

	// Token: 0x04000032 RID: 50 RVA: 0x002BE260 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	internal unsafe static delegate*<void>* ?A0x90d41ecd.__onexitend_m;

	// Token: 0x04000033 RID: 51
	[FixedAddressValueType]
	internal unsafe static void* ?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA;

	// Token: 0x04000034 RID: 52
	[FixedAddressValueType]
	internal static int ?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA;

	// Token: 0x04000035 RID: 53 RVA: 0x00014294 File Offset: 0x00013694
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_IChapteredRowset;

	// Token: 0x04000036 RID: 54 RVA: 0x000145C4 File Offset: 0x000139C4
	// Note: this field is marked with 'hasfieldrva'.
	public static _GUID IID_ITransactionLocal;

	// Token: 0x04000037 RID: 55 RVA: 0x0000C738 File Offset: 0x0000BB38
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '1290'.
	public static uint SNI_MAX_COMPOSED_SPN;

	// Token: 0x04000038 RID: 56 RVA: 0x000012DC File Offset: 0x000006DC
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AXXZ __xc_z;

	// Token: 0x04000039 RID: 57 RVA: 0x002BA3B4 File Offset: 0x002B93B4
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '4294967295'.
	public static volatile uint __native_vcclrit_reason;

	// Token: 0x0400003A RID: 58 RVA: 0x000012D4 File Offset: 0x000006D4
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AXXZ __xc_a;

	// Token: 0x0400003B RID: 59 RVA: 0x000012E0 File Offset: 0x000006E0
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AHXZ __xi_a;

	// Token: 0x0400003C RID: 60 RVA: 0x002BE5C8 File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	public static volatile __enative_startup_state __native_startup_state;

	// Token: 0x0400003D RID: 61 RVA: 0x000012E8 File Offset: 0x000006E8
	// Note: this field is marked with 'hasfieldrva'.
	public static $ArrayType$$$BY0A@P6AHXZ __xi_z;

	// Token: 0x0400003E RID: 62 RVA: 0x002BE5CC File Offset: 0x00000000
	// Note: this field is marked with 'hasfieldrva'.
	public unsafe static void* __native_startup_lock;

	// Token: 0x0400003F RID: 63 RVA: 0x002BA3B0 File Offset: 0x002B93B0
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '4294967295'.
	public static volatile uint __native_dllmain_reason;
}
