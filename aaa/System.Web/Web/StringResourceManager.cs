using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Web
{
	// Token: 0x020000D7 RID: 215
	internal class StringResourceManager
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x0002B2AC File Offset: 0x0002A2AC
		private StringResourceManager()
		{
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0002B2B4 File Offset: 0x0002A2B4
		internal unsafe static string ResourceToString(IntPtr pv, int offset, int size)
		{
			return new string((sbyte*)(void*)pv, offset, size, Encoding.UTF8);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0002B2C8 File Offset: 0x0002A2C8
		internal static SafeStringResource ReadSafeStringResource(Type t)
		{
			if (HttpRuntime.CodegenDirInternal != null)
			{
				InternalSecurityPermissions.PathDiscovery(HttpRuntime.CodegenDirInternal).Assert();
			}
			string fullyQualifiedName = t.Module.FullyQualifiedName;
			IntPtr intPtr = UnsafeNativeMethods.GetModuleHandle(fullyQualifiedName);
			if (intPtr == IntPtr.Zero)
			{
				intPtr = Marshal.GetHINSTANCE(t.Module);
				if (intPtr == IntPtr.Zero)
				{
					throw new HttpException(SR.GetString("Resource_problem", new object[]
					{
						"GetModuleHandle",
						HttpException.HResultFromLastError(Marshal.GetLastWin32Error()).ToString(CultureInfo.InvariantCulture)
					}));
				}
			}
			IntPtr intPtr2 = UnsafeNativeMethods.FindResource(intPtr, (IntPtr)101, (IntPtr)3771);
			if (intPtr2 == IntPtr.Zero)
			{
				throw new HttpException(SR.GetString("Resource_problem", new object[]
				{
					"FindResource",
					HttpException.HResultFromLastError(Marshal.GetLastWin32Error()).ToString(CultureInfo.InvariantCulture)
				}));
			}
			int num = UnsafeNativeMethods.SizeofResource(intPtr, intPtr2);
			IntPtr intPtr3 = UnsafeNativeMethods.LoadResource(intPtr, intPtr2);
			if (intPtr3 == IntPtr.Zero)
			{
				throw new HttpException(SR.GetString("Resource_problem", new object[]
				{
					"LoadResource",
					HttpException.HResultFromLastError(Marshal.GetLastWin32Error()).ToString(CultureInfo.InvariantCulture)
				}));
			}
			IntPtr intPtr4 = UnsafeNativeMethods.LockResource(intPtr3);
			if (intPtr4 == IntPtr.Zero)
			{
				throw new HttpException(SR.GetString("Resource_problem", new object[]
				{
					"LockResource",
					HttpException.HResultFromLastError(Marshal.GetLastWin32Error()).ToString(CultureInfo.InvariantCulture)
				}));
			}
			if (!UnsafeNativeMethods.IsValidResource(intPtr, intPtr4, num))
			{
				throw new InvalidOperationException();
			}
			return new SafeStringResource(intPtr4, num);
		}

		// Token: 0x04001262 RID: 4706
		internal const int RESOURCE_TYPE = 3771;

		// Token: 0x04001263 RID: 4707
		internal const int RESOURCE_ID = 101;
	}
}
