using System;
using System.Runtime.InteropServices;
using <CppImplementationDetails>;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000043 RID: 67
	internal class Security
	{
		// Token: 0x06000099 RID: 153 RVA: 0x0000244C File Offset: 0x0000184C
		private Security()
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002488 File Offset: 0x00001888
		private unsafe static int Init()
		{
			if (Security._fInit == 0)
			{
				lock (typeof(Security))
				{
					if (Security._fInit == 0)
					{
						Security._cPackages = 0U;
						HINSTANCE__* ptr = <Module>.LoadLibraryW((char*)(&<Module>.?A0xb1a0d9a8.unnamed-global-0));
						if (ptr != null && ptr != -1)
						{
							Security.OpenThreadToken = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.?A0xb1a0d9a8.unnamed-global-1));
							Security.SetThreadToken = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.?A0xb1a0d9a8.unnamed-global-2));
						}
						Security._fInit = 1;
					}
				}
			}
			return 0;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002520 File Offset: 0x00001920
		public unsafe static string GetEveryoneAccountName()
		{
			_SID1 sid = 1;
			*((ref sid) + 1) = 1;
			*((ref sid) + 2) = 0;
			*((ref sid) + 3) = 0;
			*((ref sid) + 4) = 0;
			*((ref sid) + 5) = 0;
			*((ref sid) + 6) = 0;
			*((ref sid) + 7) = 1;
			*((ref sid) + 8) = 0;
			uint num = 260U;
			uint num2 = 260U;
			$ArrayType$$$BY0BAE@_W $ArrayType$$$BY0BAE@_W;
			$ArrayType$$$BY0BAE@_W $ArrayType$$$BY0BAE@_W2;
			int num3;
			if (<Module>.LookupAccountSidW(null, (void*)(&sid), (char*)(&$ArrayType$$$BY0BAE@_W), (uint*)(&num2), (char*)(&$ArrayType$$$BY0BAE@_W2), (uint*)(&num), &num3) == null)
			{
				int num4;
				if (<Module>.GetLastError() <= 0)
				{
					num4 = <Module>.GetLastError();
				}
				else
				{
					num4 = (<Module>.GetLastError() & 65535) | -2147024896;
				}
				if (num4 < 0)
				{
					Marshal.ThrowExceptionForHR(num4);
				}
			}
			IntPtr intPtr = new IntPtr(ref $ArrayType$$$BY0BAE@_W);
			return Marshal.PtrToStringUni(intPtr);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000025C0 File Offset: 0x000019C0
		public unsafe static IntPtr SuspendImpersonation()
		{
			void* ptr = null;
			int num = Security.Init();
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			if (Security.OpenThreadToken != null && Security.SetThreadToken != null && calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void*,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.Int32,System.Void**), <Module>.GetCurrentThread(), 4, 1, &ptr, Security.OpenThreadToken) != null)
			{
				int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void**,System.Void*), null, null, Security.SetThreadToken);
				IntPtr intPtr = new IntPtr(ptr);
				return intPtr;
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002620 File Offset: 0x00001A20
		public static void ResumeImpersonation(IntPtr hToken)
		{
			if (Security.OpenThreadToken != null && Security.SetThreadToken != null)
			{
				IntPtr intPtr = new IntPtr(0);
				if (hToken != intPtr)
				{
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void**,System.Void*), null, hToken.ToInt32(), Security.SetThreadToken);
					<Module>.CloseHandle(hToken.ToInt32());
				}
			}
		}

		// Token: 0x04000092 RID: 146
		private static int _fInit = 0;

		// Token: 0x04000093 RID: 147
		private static uint _cPackages;

		// Token: 0x04000094 RID: 148
		private unsafe static _SecPkgInfoW* _pPackageInfo;

		// Token: 0x04000095 RID: 149
		private unsafe static delegate* unmanaged[Stdcall, Stdcall]<void*, uint, int, void**, int> OpenThreadToken = 0;

		// Token: 0x04000096 RID: 150
		private unsafe static delegate* unmanaged[Stdcall, Stdcall]<void**, void*, int> SetThreadToken = 0;
	}
}
