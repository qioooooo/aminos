using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Threading;
using <CppImplementationDetails>;
using Microsoft.Win32;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000056 RID: 86
	internal class Proxy
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000026D4 File Offset: 0x00001AD4
		private Proxy()
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002C04 File Offset: 0x00002004
		[return: MarshalAs(UnmanagedType.U1)]
		private unsafe static bool CheckRegistered(Guid id, Assembly assembly, [MarshalAs(UnmanagedType.U1)] bool checkCache, [MarshalAs(UnmanagedType.U1)] bool cacheOnly)
		{
			if (checkCache && Proxy._regCache[assembly] != null)
			{
				return true;
			}
			if (cacheOnly)
			{
				return false;
			}
			bool flag = false;
			string text = new string((char*)(&<Module>.?A0x5da4b14d.unnamed-global-6)) + id.ToString() + new string((sbyte*)(&<Module>.?A0x5da4b14d.unnamed-global-5));
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(text, false);
			if (registryKey != null)
			{
				Proxy._regCache[assembly] = bool.TrueString;
			}
			else if (Proxy.IsWin64(ref flag))
			{
				IntPtr intPtr = Marshal.StringToHGlobalUni(text);
				char* ptr = (char*)intPtr.ToPointer();
				int num = (flag ? 256 : 512);
				HKEY__* ptr2;
				bool flag2 = <Module>.RegOpenKeyExW(int.MinValue, (char*)ptr, 0, num | 131097, &ptr2) != null;
				Marshal.FreeHGlobal(intPtr);
				if (flag2)
				{
					return false;
				}
				<Module>.RegCloseKey(ptr2);
				return true;
			}
			return ((registryKey != null) ? 1 : 0) != 0;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003870 File Offset: 0x00002C70
		private static void LazyRegister(Guid id, Type serverType, [MarshalAs(UnmanagedType.U1)] bool checkCache)
		{
			if (serverType.Assembly != Proxy._thisAssembly)
			{
				Assembly assembly = serverType.Assembly;
				if (!checkCache || Proxy._regCache[assembly] == null)
				{
					Proxy._regmutex.WaitOne();
					try
					{
						if (!Proxy.CheckRegistered(id, serverType.Assembly, checkCache, false))
						{
							Proxy.RegisterAssembly(serverType.Assembly);
						}
					}
					finally
					{
						Proxy._regmutex.ReleaseMutex();
					}
				}
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00002CD0 File Offset: 0x000020D0
		private unsafe static void RegisterAssembly(Assembly assembly)
		{
			try
			{
				((IThunkInstallation)Activator.CreateInstance(Type.GetType(new string((char*)(&<Module>.?A0x5da4b14d.unnamed-global-7))))).DefaultInstall(assembly.Location);
			}
			finally
			{
				Proxy._regCache[assembly] = bool.TrueString;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002B24 File Offset: 0x00001F24
		[return: MarshalAs(UnmanagedType.U1)]
		private unsafe static bool IsWin64(bool* A_0)
		{
			if (<Module>.?A0x5da4b14d.?fInit@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC == 0)
			{
				*A_0 = 0;
				delegate* unmanaged[Stdcall, Stdcall]<_SYSTEM_INFO*, void> procAddress = <Module>.GetProcAddress(<Module>.GetModuleHandleW((char*)(&<Module>.?A0x5da4b14d.unnamed-global-4)), (sbyte*)(&<Module>.?A0x5da4b14d.unnamed-global-3));
				int num;
				if (procAddress != null)
				{
					_SYSTEM_INFO system_INFO;
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(_SYSTEM_INFO*), &system_INFO, procAddress);
					if (system_INFO != 6 && system_INFO != 9)
					{
						num = 0;
					}
					else
					{
						int num2 = 0;
						delegate* unmanaged[Stdcall, Stdcall]<void*, int*, int> procAddress2 = <Module>.GetProcAddress(<Module>.GetModuleHandleW((char*)(&<Module>.?A0x5da4b14d.unnamed-global-2)), (sbyte*)(&<Module>.?A0x5da4b14d.unnamed-global-1));
						if (procAddress2 == null)
						{
							num2 = 0;
						}
						else if (calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void*,System.Int32*), <Module>.GetCurrentProcess(), &num2, procAddress2) == null)
						{
							num2 = 0;
						}
						else if (num2 == 1)
						{
							*A_0 = 1;
						}
						num = 1;
					}
				}
				else
				{
					num = 0;
				}
				<Module>.?A0x5da4b14d.?fWin64@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC = num;
				<Module>.?A0x5da4b14d.?fIsWow@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC = ((*A_0 != 0) ? 1 : 0);
				<Module>.?A0x5da4b14d.?fInit@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC = 1;
				return num != 0;
			}
			byte b = ((<Module>.?A0x5da4b14d.?fIsWow@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC != 0) ? 1 : 0);
			*A_0 = b;
			return <Module>.?A0x5da4b14d.?fWin64@?1??IsWin64@Proxy@Thunk@EnterpriseServices@System@@CM_NAA_N@Z@4HC != 0;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00002AB8 File Offset: 0x00001EB8
		private unsafe static void IsWow64ProcessInternal(int* A_0)
		{
			delegate* unmanaged[Stdcall, Stdcall]<void*, int*, int> procAddress = <Module>.GetProcAddress(<Module>.GetModuleHandleW((char*)(&<Module>.?A0x5da4b14d.unnamed-global-2)), (sbyte*)(&<Module>.?A0x5da4b14d.unnamed-global-1));
			if (procAddress == null)
			{
				*A_0 = 0;
			}
			else if (calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void*,System.Int32*), <Module>.GetCurrentProcess(), A_0, procAddress) == null)
			{
				*A_0 = 0;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00002AF4 File Offset: 0x00001EF4
		[return: MarshalAs(UnmanagedType.U1)]
		private unsafe static bool GetNativeSystemInfoInternal(_SYSTEM_INFO* A_0)
		{
			delegate* unmanaged[Stdcall, Stdcall]<_SYSTEM_INFO*, void> procAddress = <Module>.GetProcAddress(<Module>.GetModuleHandleW((char*)(&<Module>.?A0x5da4b14d.unnamed-global-4)), (sbyte*)(&<Module>.?A0x5da4b14d.unnamed-global-3));
			if (procAddress == null)
			{
				return false;
			}
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(_SYSTEM_INFO*), A_0, procAddress);
			return true;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00002838 File Offset: 0x00001C38
		public unsafe static void Init()
		{
			int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
			if (Thread.CurrentThread.ApartmentState == ApartmentState.Unknown)
			{
				Thread.CurrentThread.ApartmentState = ApartmentState.MTA;
			}
			if (!Proxy._fInit)
			{
				uint exceptionCode;
				try
				{
					IntPtr intPtr = IntPtr.Zero;
					if (Proxy._classSyncRoot == null)
					{
						object obj = new object();
						Interlocked.CompareExchange(ref Proxy._classSyncRoot, obj, null);
					}
					lock (Proxy._classSyncRoot)
					{
						try
						{
							intPtr = Security.SuspendImpersonation();
							if (!Proxy._fInit)
							{
								Proxy._regCache = new Hashtable();
								IGlobalInterfaceTable* ptr = null;
								int num2 = <Module>.CoCreateInstance(ref <Module>.CLSID_StdGlobalInterfaceTable, null, 1, ref <Module>.IID_IGlobalInterfaceTable, (void**)(&ptr));
								Proxy._pGIT = ptr;
								if (num2 < 0)
								{
									Marshal.ThrowExceptionForHR(num2);
								}
								Proxy._thisAssembly = Assembly.GetExecutingAssembly();
								Proxy._regmutex = new Mutex(false, new string((sbyte*)(&<Module>.?A0x5da4b14d.unnamed-global-0)) + RemotingConfiguration.ProcessId);
								Thread.MemoryBarrier();
								Proxy._fInit = true;
							}
						}
						finally
						{
							Security.ResumeImpersonation(intPtr);
						}
					}
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					exceptionCode = (uint)Marshal.GetExceptionCode();
					endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
				})
				{
					uint num3 = 0U;
					<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
					try
					{
						try
						{
							<Module>._CxxThrowException(null, null);
							goto IL_012A;
						}
						catch when (delegate
						{
							// Failed to create a 'catch-when' expression
							num3 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
							endfilter(num3 != 0U);
						})
						{
						}
						if (num3 != 0U)
						{
							throw;
						}
						IL_012A:;
					}
					finally
					{
						<Module>.__CxxUnregisterExceptionObject(num, (int)num3);
					}
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000029F8 File Offset: 0x00001DF8
		public unsafe static int StoreObject(IntPtr ptr)
		{
			Proxy.Init();
			IUnknown* ptr2 = ptr.ToInt32();
			int num = *(int*)Proxy._pGIT + 12;
			uint num3;
			int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown*,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*), Proxy._pGIT, ptr2, ref <Module>.IID_IUnknown, (uint*)(&num3), (IntPtr)(*num));
			if (num2 < 0)
			{
				Marshal.ThrowExceptionForHR(num2);
			}
			return (int)num3;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00002A3C File Offset: 0x00001E3C
		public unsafe static IntPtr GetObject(int cookie)
		{
			Proxy.Init();
			IUnknown* ptr = null;
			int num = *(int*)Proxy._pGIT + 20;
			int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), Proxy._pGIT, cookie, ref <Module>.IID_IUnknown, (void**)(&ptr), (IntPtr)(*num));
			if (num2 < 0)
			{
				Marshal.ThrowExceptionForHR(num2);
			}
			IntPtr intPtr = new IntPtr(ptr);
			return intPtr;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00002A84 File Offset: 0x00001E84
		public unsafe static void RevokeObject(int cookie)
		{
			Proxy.Init();
			int num = *(int*)Proxy._pGIT + 16;
			int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), Proxy._pGIT, cookie, (IntPtr)(*num));
			if (num2 < 0)
			{
				Marshal.ThrowExceptionForHR(num2);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000038F4 File Offset: 0x00002CF4
		public unsafe static IntPtr CoCreateObject(Type serverType, [MarshalAs(UnmanagedType.U1)] bool bQuerySCInfo, ref bool bIsAnotherProcess, ref string uri)
		{
			Proxy.Init();
			IUnknown* ptr = null;
			bool flag = true;
			Guid guid = Marshal.GenerateGuidForType(serverType);
			do
			{
				IUnknown* ptr2 = null;
				IServicedComponentInfo* ptr3 = null;
				tagSAFEARRAY* ptr4 = null;
				try
				{
					Proxy.LazyRegister(guid, serverType, flag);
					_GUID guid2;
					cpblk(ref guid2, ref guid, 16);
					$ArrayType$$$BY01UtagMULTI_QI@@ $ArrayType$$$BY01UtagMULTI_QI@@ = 0;
					initblk((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 4, 0, 20);
					$ArrayType$$$BY01UtagMULTI_QI@@ = ref <Module>.IID_IUnknown;
					*((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 12) = ref <Module>.IID_IServicedComponentInfo;
					int num;
					if (bQuerySCInfo && !IdentityManager.Enabled)
					{
						num = 2;
					}
					else
					{
						num = 1;
					}
					int num2 = <Module>.CoCreateInstanceEx(ref guid2, null, 23, null, num, (tagMULTI_QI*)(&$ArrayType$$$BY01UtagMULTI_QI@@));
					if (num2 >= 0)
					{
						if (*((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 8) >= 0)
						{
							ptr2 = *((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 4);
						}
						if (bQuerySCInfo && !IdentityManager.Enabled && *((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 20) >= 0)
						{
							ptr3 = *((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 16);
						}
						if (*((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 8) < 0)
						{
							int num3 = *((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 8);
							Marshal.ThrowExceptionForHR(*((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 8));
						}
						if (bQuerySCInfo)
						{
							if (*((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 20) < 0)
							{
								int num4 = *((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 20);
								Marshal.ThrowExceptionForHR(*((ref $ArrayType$$$BY01UtagMULTI_QI@@) + 20));
							}
							if (IdentityManager.Enabled)
							{
								IntPtr intPtr = new IntPtr(ptr2);
								byte b = ((!IdentityManager.IsInProcess(intPtr)) ? 1 : 0);
								bIsAnotherProcess = b != 0;
								if (b != 0)
								{
									IntPtr intPtr2 = new IntPtr(ptr2);
									uri = IdentityManager.CreateIdentityUri(intPtr2);
									goto IL_0209;
								}
								goto IL_0209;
							}
							else if (ptr3 != null)
							{
								char* ptr5 = null;
								char* ptr6 = null;
								int num5 = 0;
								num5 = Proxy.INFO_PROCESSID;
								num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32*,tagSAFEARRAY**), ptr3, &num5, &ptr4, (IntPtr)(*(*(int*)ptr3 + 12)));
								if (num2 < 0)
								{
									Marshal.ThrowExceptionForHR(num2);
								}
								int num6 = 0;
								<Module>.SafeArrayGetElement(ptr4, (int*)(&num6), (void*)(&ptr5));
								IntPtr intPtr3 = new IntPtr((void*)ptr5);
								string text = Marshal.PtrToStringBSTR(intPtr3);
								string processId = RemotingConfiguration.ProcessId;
								if (ptr5 != null)
								{
									<Module>.SysFreeString(ptr5);
								}
								<Module>.SafeArrayDestroy(ptr4);
								ptr4 = null;
								if (string.Compare(processId, text, StringComparison.Ordinal) == 0)
								{
									bIsAnotherProcess = false;
									goto IL_0209;
								}
								bIsAnotherProcess = true;
								num5 = Proxy.INFO_URI;
								num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32*,tagSAFEARRAY**), ptr3, &num5, &ptr4, (IntPtr)(*(*(int*)ptr3 + 12)));
								if (num2 < 0)
								{
									Marshal.ThrowExceptionForHR(num2);
								}
								num6 = 0;
								<Module>.SafeArrayGetElement(ptr4, (int*)(&num6), (void*)(&ptr6));
								IntPtr intPtr4 = new IntPtr((void*)ptr6);
								uri = Marshal.PtrToStringBSTR(intPtr4);
								if (ptr6 != null)
								{
									<Module>.SysFreeString(ptr6);
								}
								<Module>.SafeArrayDestroy(ptr4);
								ptr4 = null;
								goto IL_0209;
							}
						}
						bIsAnotherProcess = true;
					}
					else if (num2 == -2147221164 && flag)
					{
						flag = false;
					}
					else
					{
						Marshal.ThrowExceptionForHR(num2);
					}
					IL_0209:
					ptr = ptr2;
					ptr2 = null;
				}
				finally
				{
					if (ptr2 != null)
					{
						IUnknown* ptr7 = ptr2;
						uint num7 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr7, (IntPtr)(*(*(int*)ptr7 + 8)));
					}
					if (ptr3 != null)
					{
						IServicedComponentInfo* ptr8 = ptr3;
						uint num8 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr8, (IntPtr)(*(*(int*)ptr8 + 8)));
					}
					if (ptr4 != null)
					{
						<Module>.SafeArrayDestroy(ptr4);
					}
				}
			}
			while (ptr == null);
			IntPtr intPtr5 = new IntPtr(ptr);
			return intPtr5;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00002D30 File Offset: 0x00002130
		public unsafe static int GetMarshalSize(object o)
		{
			Proxy.Init();
			IUnknown* ptr = null;
			uint num = 0U;
			try
			{
				ptr = Marshal.GetIUnknownForObject(o).ToInt32();
				if (<Module>.CoGetMarshalSizeMax((uint*)(&num), ref <Module>.IID_IUnknown, ptr, 2, null, 0) >= 0)
				{
					num += 4U;
				}
				else
				{
					num = uint.MaxValue;
				}
			}
			finally
			{
				if (ptr != null)
				{
					IUnknown* ptr2 = ptr;
					uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
				}
			}
			return (int)num;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002E08 File Offset: 0x00002208
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool MarshalObject(object o, byte[] b, int cb)
		{
			Proxy.Init();
			IUnknown* ptr = null;
			fixed (byte* ptr2 = &b[0])
			{
				byte* ptr3 = ptr2;
				try
				{
					ptr = Marshal.GetIUnknownForObject(o).ToInt32();
					int num = <Module>.MarshalInterface(ptr3, cb, ptr, 2, 0);
					if (num < 0)
					{
						Marshal.ThrowExceptionForHR(num);
					}
				}
				finally
				{
					if (ptr != null)
					{
						IUnknown* ptr4 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
					}
				}
				return true;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00002DA0 File Offset: 0x000021A0
		public unsafe static IntPtr UnmarshalObject(byte[] b)
		{
			Proxy.Init();
			IUnknown* ptr = null;
			int length = b.Length;
			fixed (byte* ptr2 = &b[0])
			{
				byte* ptr3 = ptr2;
				try
				{
					int num = <Module>.UnmarshalInterface(ptr3, length, (void**)(&ptr));
					if (num < 0)
					{
						Marshal.ThrowExceptionForHR(num);
					}
				}
				finally
				{
				}
				IntPtr intPtr = new IntPtr(ptr);
				return intPtr;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00002EB4 File Offset: 0x000022B4
		public unsafe static void ReleaseMarshaledObject(byte[] b)
		{
			Proxy.Init();
			fixed (byte* ptr = &b[0])
			{
				byte* ptr2 = ptr;
				try
				{
					int num = <Module>.ReleaseMarshaledInterface(ptr2, b.Length);
					if (num < 0)
					{
						Marshal.ThrowExceptionForHR(num);
					}
				}
				finally
				{
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00002E7C File Offset: 0x0000227C
		public unsafe static IntPtr GetStandardMarshal(IntPtr pUnk)
		{
			IMarshal* ptr;
			int num = <Module>.CoGetStandardMarshal(ref <Module>.IID_IUnknown, pUnk.ToInt32(), 2, null, 0, &ptr);
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			IntPtr intPtr = new IntPtr(ptr);
			return intPtr;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00002F08 File Offset: 0x00002308
		public static IntPtr GetContextCheck()
		{
			Proxy.Init();
			IntPtr intPtr = new IntPtr(<Module>.GetContextCheck());
			return intPtr;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00002F28 File Offset: 0x00002328
		public static IntPtr GetCurrentContextToken()
		{
			Proxy.Init();
			IntPtr intPtr = new IntPtr(<Module>.GetContextToken());
			return intPtr;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00002F48 File Offset: 0x00002348
		public unsafe static IntPtr GetCurrentContext()
		{
			Proxy.Init();
			IUnknown* ptr;
			int context = <Module>.GetContext(ref <Module>.IID_IUnknown, (void**)(&ptr));
			if (context < 0)
			{
				Marshal.ThrowExceptionForHR(context);
			}
			IntPtr intPtr = new IntPtr(ptr);
			return intPtr;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00002F7C File Offset: 0x0000237C
		public unsafe static int CallFunction(IntPtr pfn, IntPtr data)
		{
			void* ptr = data.ToInt32();
			delegate* unmanaged[Stdcall, Stdcall]<void*, int> system.Int32_u0020modopt(System.Runtime.CompilerServices.IsLong)_u0020modopt(System.Runtime.CompilerServices.CallConvStdcall)_u0020(System.Void*) = pfn.ToInt32();
			return calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void*), ptr, system.Int32_u0020modopt(System.Runtime.CompilerServices.IsLong)_u0020modopt(System.Runtime.CompilerServices.CallConvStdcall)_u0020(System.Void*));
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00002FA0 File Offset: 0x000023A0
		public unsafe static void PoolUnmark(IntPtr pPooledObject)
		{
			IManagedPooledObj* ptr = pPooledObject.ToInt32();
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, 0, (IntPtr)(*(*(int*)ptr + 12)));
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00002FC4 File Offset: 0x000023C4
		public unsafe static void PoolMark(IntPtr pPooledObject)
		{
			IManagedPooledObj* ptr = pPooledObject.ToInt32();
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, 1, (IntPtr)(*(*(int*)ptr + 12)));
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00002FE8 File Offset: 0x000023E8
		public unsafe static int GetManagedExts()
		{
			if (<Module>.?A0x5da4b14d.?dwExts@?1??GetManagedExts@Proxy@Thunk@EnterpriseServices@System@@SMHXZ@4KA == 4294967295U)
			{
				uint num = 0U;
				HINSTANCE__* ptr = <Module>.LoadLibraryW((char*)(&<Module>.?A0x5da4b14d.unnamed-global-8));
				if (ptr != null && ptr != -1)
				{
					delegate* unmanaged[Stdcall, Stdcall]<uint*, int> procAddress = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.?A0x5da4b14d.unnamed-global-9));
					if (procAddress != null && calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*), (uint*)(&num), procAddress) < 0)
					{
						num = 0U;
					}
				}
				<Module>.?A0x5da4b14d.?dwExts@?1??GetManagedExts@Proxy@Thunk@EnterpriseServices@System@@SMHXZ@4KA = num;
			}
			return (int)<Module>.?A0x5da4b14d.?dwExts@?1??GetManagedExts@Proxy@Thunk@EnterpriseServices@System@@SMHXZ@4KA;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003038 File Offset: 0x00002438
		public unsafe static void SendCreationEvents(IntPtr ctx, IntPtr stub, [MarshalAs(UnmanagedType.U1)] bool fDist)
		{
			IUnknown* ptr = ctx.ToInt32();
			IObjContext* ptr2 = null;
			IManagedObjectInfo* ptr3 = stub.ToInt32();
			IEnumContextProps* ptr4 = null;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>.System.EnterpriseServices.Thunk.?A0x5da4b14d.IID_IObjContext, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
			if (num >= 0)
			{
				try
				{
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IEnumContextProps**), ptr2, &ptr4, (IntPtr)(*(*(int*)ptr2 + 24)));
					if (num >= 0)
					{
						uint num2 = 0U;
						num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*), ptr4, (uint*)(&num2), (IntPtr)(*(*(int*)ptr4 + 28)));
						if (num < 0)
						{
							Marshal.ThrowExceptionForHR(num);
						}
						for (uint num3 = 0U; num3 < num2; num3 += 1U)
						{
							uint num4 = 0U;
							tagContextProperty tagContextProperty;
							num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),tagContextProperty*,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*), ptr4, 1, &tagContextProperty, (uint*)(&num4), (IntPtr)(*(*(int*)ptr4 + 12)));
							if (num < 0)
							{
								Marshal.ThrowExceptionForHR(num);
							}
							if (num4 != 1U)
							{
								break;
							}
							IManagedActivationEvents* ptr5 = null;
							num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), (IntPtr)(*((ref tagContextProperty) + 20)), ref <Module>.IID_IManagedActivationEvents, (void**)(&ptr5), (IntPtr)(*(*(*((ref tagContextProperty) + 20)))));
							if (num >= 0)
							{
								int num5 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.EnterpriseServices.Thunk.IManagedObjectInfo*,System.Int32), ptr5, ptr3, fDist ? 1 : 0, (IntPtr)(*(*(int*)ptr5 + 12)));
								IManagedActivationEvents* ptr6 = ptr5;
								uint num6 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr6, (IntPtr)(*(*(int*)ptr6 + 8)));
							}
							uint num7 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), (IntPtr)(*((ref tagContextProperty) + 20)), (IntPtr)(*(*(*((ref tagContextProperty) + 20)) + 8)));
						}
					}
				}
				finally
				{
					if (ptr2 != null)
					{
						IObjContext* ptr7 = ptr2;
						uint num8 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr7, (IntPtr)(*(*(int*)ptr7 + 8)));
					}
					if (ptr4 != null)
					{
						IEnumContextProps* ptr8 = ptr4;
						uint num9 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr8, (IntPtr)(*(*(int*)ptr8 + 8)));
					}
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003174 File Offset: 0x00002574
		public unsafe static void SendDestructionEvents(IntPtr ctx, IntPtr stub, [MarshalAs(UnmanagedType.U1)] bool disposing)
		{
			DestructData destructData = ctx.ToInt32();
			*((ref destructData) + 4) = stub.ToInt32();
			tagComCallData tagComCallData = 0;
			*((ref tagComCallData) + 4) = 0;
			*((ref tagComCallData) + 8) = ref destructData;
			IContextCallback* ptr = null;
			int num = 0;
			try
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), destructData, ref <Module>.IID_IContextCallback, (void**)(&ptr), (IntPtr)(*(*destructData)));
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				_GUID* ptr2;
				if (disposing)
				{
					ptr2 = (_GUID*)(&<Module>.IID_IUnknown);
				}
				else
				{
					ptr2 = (_GUID*)(&<Module>.IID_IEnterActivityWithNoLock);
				}
				_GUID guid;
				cpblk(ref guid, ptr2, 16);
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall) (System.EnterpriseServices.Thunk.tagComCallData*),System.EnterpriseServices.Thunk.tagComCallData*,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Int32,IUnknown*), ptr, <Module>.__unep@?SendDestructionEventsCallback@Thunk@EnterpriseServices@System@@$$FYGJPAUtagComCallData@123@@Z, &tagComCallData, ref guid, 2, null, (IntPtr)(*(*(int*)ptr + 12)));
			}
			finally
			{
				if (ptr != null)
				{
					IContextCallback* ptr3 = ptr;
					uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				}
			}
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003238 File Offset: 0x00002638
		public unsafe static Tracker FindTracker(IntPtr ctx)
		{
			_GUID guid = -324292941;
			*((ref guid) + 4) = 32537;
			*((ref guid) + 6) = 4562;
			*((ref guid) + 8) = 151;
			*((ref guid) + 9) = 142;
			*((ref guid) + 10) = 0;
			*((ref guid) + 11) = 0;
			*((ref guid) + 12) = 248;
			*((ref guid) + 13) = 117;
			*((ref guid) + 14) = 126;
			*((ref guid) + 15) = 42;
			IUnknown* ptr = null;
			ISendMethodEvents* ptr2 = null;
			IObjContext* ptr3 = null;
			uint num = 0U;
			Tracker tracker;
			try
			{
				int num2 = ctx.ToInt32();
				int num3 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), (IntPtr)num2, ref <Module>.System.EnterpriseServices.Thunk.?A0x5da4b14d.IID_IObjContext, (void**)(&ptr3), (IntPtr)(*(*num2)));
				if (num3 < 0)
				{
					tracker = null;
				}
				else
				{
					num3 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*,IUnknown**), ptr3, ref guid, (uint*)(&num), &ptr, (IntPtr)(*(*(int*)ptr3 + 20)));
					if (num3 >= 0 && ptr != null)
					{
						num3 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>._GUID_2732fd59_b2b4_4d44_878c_8b8f09626008, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
						if (num3 < 0)
						{
							ptr2 = null;
							tracker = null;
						}
						else
						{
							tracker = new Tracker(ptr2);
						}
					}
					else
					{
						ptr = null;
						tracker = null;
					}
				}
			}
			finally
			{
				if (ptr3 != null)
				{
					IObjContext* ptr4 = ptr3;
					uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
				}
				if (ptr != null)
				{
					IUnknown* ptr5 = ptr;
					uint num5 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr5, (IntPtr)(*(*(int*)ptr5 + 8)));
				}
				if (ptr2 != null)
				{
					ISendMethodEvents* ptr6 = ptr2;
					uint num6 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr6, (IntPtr)(*(*(int*)ptr6 + 8)));
				}
			}
			return tracker;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002824 File Offset: 0x00001C24
		public static int RegisterProxyStub()
		{
			return <Module>.DllRegisterServer();
		}

		// Token: 0x0400009C RID: 156
		private static bool _fInit;

		// Token: 0x0400009D RID: 157
		private static Hashtable _regCache;

		// Token: 0x0400009E RID: 158
		private unsafe static IGlobalInterfaceTable* _pGIT;

		// Token: 0x0400009F RID: 159
		private static Assembly _thisAssembly;

		// Token: 0x040000A0 RID: 160
		private static Mutex _regmutex;

		// Token: 0x040000A1 RID: 161
		private static object _classSyncRoot;

		// Token: 0x040000A2 RID: 162
		public static int INFO_PROCESSID = 1;

		// Token: 0x040000A3 RID: 163
		public static int INFO_APPDOMAINID = 2;

		// Token: 0x040000A4 RID: 164
		public static int INFO_URI = 4;
	}
}
