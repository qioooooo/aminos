using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Data.ProviderBase
{
	// Token: 0x0200009B RID: 155
	[Serializable]
	internal sealed class DbConnectionPoolIdentity
	{
		// Token: 0x0600084C RID: 2124 RVA: 0x00074E08 File Offset: 0x00074208
		private DbConnectionPoolIdentity(string sidString, bool isRestricted, bool isNetwork)
		{
			this._sidString = sidString;
			this._isRestricted = isRestricted;
			this._isNetwork = isNetwork;
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x00074E30 File Offset: 0x00074230
		internal bool IsRestricted
		{
			get
			{
				return this._isRestricted;
			}
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00074E44 File Offset: 0x00074244
		private static byte[] CreateWellKnownSid(WellKnownSidType sidType)
		{
			uint maxBinaryLength = (uint)SecurityIdentifier.MaxBinaryLength;
			byte[] array = new byte[maxBinaryLength];
			if (UnsafeNativeMethods.CreateWellKnownSid((int)sidType, null, array, ref maxBinaryLength) == 0)
			{
				DbConnectionPoolIdentity.IntegratedSecurityError(5);
			}
			return array;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00074E74 File Offset: 0x00074274
		public override bool Equals(object value)
		{
			bool flag = this == DbConnectionPoolIdentity.NoIdentity || this == value;
			if (!flag && value != null)
			{
				DbConnectionPoolIdentity dbConnectionPoolIdentity = (DbConnectionPoolIdentity)value;
				flag = this._sidString == dbConnectionPoolIdentity._sidString && this._isRestricted == dbConnectionPoolIdentity._isRestricted && this._isNetwork == dbConnectionPoolIdentity._isNetwork;
			}
			return flag;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00074ED4 File Offset: 0x000742D4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal static WindowsIdentity GetCurrentWindowsIdentity()
		{
			return WindowsIdentity.GetCurrent();
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00074EE8 File Offset: 0x000742E8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static IntPtr GetWindowsIdentityToken(WindowsIdentity identity)
		{
			return identity.Token;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00074EFC File Offset: 0x000742FC
		internal static DbConnectionPoolIdentity GetCurrent()
		{
			if (!ADP.IsWindowsNT)
			{
				return DbConnectionPoolIdentity.NoIdentity;
			}
			WindowsIdentity currentWindowsIdentity = DbConnectionPoolIdentity.GetCurrentWindowsIdentity();
			IntPtr windowsIdentityToken = DbConnectionPoolIdentity.GetWindowsIdentityToken(currentWindowsIdentity);
			uint num = 2048U;
			uint num2 = 0U;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			UnsafeNativeMethods.SetLastError(0);
			bool flag = UnsafeNativeMethods.IsTokenRestricted(windowsIdentityToken);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error != 0)
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
			DbConnectionPoolIdentity dbConnectionPoolIdentity = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				bool flag2;
				if (!UnsafeNativeMethods.CheckTokenMembership(windowsIdentityToken, DbConnectionPoolIdentity.NetworkSid, out flag2))
				{
					DbConnectionPoolIdentity.IntegratedSecurityError(1);
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					intPtr = SafeNativeMethods.LocalAlloc(0, (IntPtr)((long)((ulong)num)));
				}
				if (IntPtr.Zero == intPtr)
				{
					throw new OutOfMemoryException();
				}
				if (!UnsafeNativeMethods.GetTokenInformation(windowsIdentityToken, 1U, intPtr, num, ref num2))
				{
					if (num2 > num)
					{
						num = num2;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
						}
						finally
						{
							SafeNativeMethods.LocalFree(intPtr);
							intPtr = IntPtr.Zero;
							intPtr = SafeNativeMethods.LocalAlloc(0, (IntPtr)((long)((ulong)num)));
						}
						if (IntPtr.Zero == intPtr)
						{
							throw new OutOfMemoryException();
						}
						if (!UnsafeNativeMethods.GetTokenInformation(windowsIdentityToken, 1U, intPtr, num, ref num2))
						{
							DbConnectionPoolIdentity.IntegratedSecurityError(2);
						}
					}
					else
					{
						DbConnectionPoolIdentity.IntegratedSecurityError(3);
					}
				}
				currentWindowsIdentity.Dispose();
				IntPtr intPtr3 = Marshal.ReadIntPtr(intPtr, 0);
				if (!UnsafeNativeMethods.ConvertSidToStringSidW(intPtr3, out intPtr2))
				{
					DbConnectionPoolIdentity.IntegratedSecurityError(4);
				}
				if (IntPtr.Zero == intPtr2)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.ConvertSidToStringSidWReturnedNull);
				}
				string text = Marshal.PtrToStringUni(intPtr2);
				dbConnectionPoolIdentity = new DbConnectionPoolIdentity(text, flag, flag2);
			}
			finally
			{
				if (IntPtr.Zero != intPtr)
				{
					SafeNativeMethods.LocalFree(intPtr);
					intPtr = IntPtr.Zero;
				}
				if (IntPtr.Zero != intPtr2)
				{
					SafeNativeMethods.LocalFree(intPtr2);
					intPtr2 = IntPtr.Zero;
				}
			}
			return dbConnectionPoolIdentity;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x000750DC File Offset: 0x000744DC
		public override int GetHashCode()
		{
			if (this._sidString == null)
			{
				return 0;
			}
			return this._sidString.GetHashCode();
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00075100 File Offset: 0x00074500
		private static void IntegratedSecurityError(int caller)
		{
			int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
			if (1 != caller || -2147023587 != hrforLastWin32Error)
			{
				Marshal.ThrowExceptionForHR(hrforLastWin32Error);
			}
		}

		// Token: 0x04000554 RID: 1364
		private const int E_NotImpersonationToken = -2147023587;

		// Token: 0x04000555 RID: 1365
		private const int Win32_CheckTokenMembership = 1;

		// Token: 0x04000556 RID: 1366
		private const int Win32_GetTokenInformation_1 = 2;

		// Token: 0x04000557 RID: 1367
		private const int Win32_GetTokenInformation_2 = 3;

		// Token: 0x04000558 RID: 1368
		private const int Win32_ConvertSidToStringSidW = 4;

		// Token: 0x04000559 RID: 1369
		private const int Win32_CreateWellKnownSid = 5;

		// Token: 0x0400055A RID: 1370
		public static readonly DbConnectionPoolIdentity NoIdentity = new DbConnectionPoolIdentity(string.Empty, false, true);

		// Token: 0x0400055B RID: 1371
		private static readonly byte[] NetworkSid = (ADP.IsWindowsNT ? DbConnectionPoolIdentity.CreateWellKnownSid(WellKnownSidType.NetworkSid) : null);

		// Token: 0x0400055C RID: 1372
		private readonly string _sidString;

		// Token: 0x0400055D RID: 1373
		private readonly bool _isRestricted;

		// Token: 0x0400055E RID: 1374
		private readonly bool _isNetwork;
	}
}
