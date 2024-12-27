using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Data.ProviderBase
{
	// Token: 0x02000278 RID: 632
	[Serializable]
	internal sealed class DbConnectionPoolIdentity
	{
		// Token: 0x0600216B RID: 8555 RVA: 0x002679B8 File Offset: 0x00266DB8
		private DbConnectionPoolIdentity(string sidString, bool isRestricted, bool isNetwork)
		{
			this._sidString = sidString;
			this._isRestricted = isRestricted;
			this._isNetwork = isNetwork;
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x002679E0 File Offset: 0x00266DE0
		internal bool IsRestricted
		{
			get
			{
				return this._isRestricted;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x002679F4 File Offset: 0x00266DF4
		internal bool IsNetwork
		{
			get
			{
				return this._isNetwork;
			}
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x00267A08 File Offset: 0x00266E08
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

		// Token: 0x0600216F RID: 8559 RVA: 0x00267A38 File Offset: 0x00266E38
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

		// Token: 0x06002170 RID: 8560 RVA: 0x00267A98 File Offset: 0x00266E98
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal static WindowsIdentity GetCurrentWindowsIdentity()
		{
			return WindowsIdentity.GetCurrent();
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x00267AAC File Offset: 0x00266EAC
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static IntPtr GetWindowsIdentityToken(WindowsIdentity identity)
		{
			return identity.Token;
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x00267AC0 File Offset: 0x00266EC0
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

		// Token: 0x06002173 RID: 8563 RVA: 0x00267CA0 File Offset: 0x002670A0
		public override int GetHashCode()
		{
			if (this._sidString == null)
			{
				return 0;
			}
			return this._sidString.GetHashCode();
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00267CC4 File Offset: 0x002670C4
		private static void IntegratedSecurityError(int caller)
		{
			int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
			if (1 != caller || -2147023587 != hrforLastWin32Error)
			{
				Marshal.ThrowExceptionForHR(hrforLastWin32Error);
			}
		}

		// Token: 0x040015A8 RID: 5544
		private const int E_NotImpersonationToken = -2147023587;

		// Token: 0x040015A9 RID: 5545
		private const int Win32_CheckTokenMembership = 1;

		// Token: 0x040015AA RID: 5546
		private const int Win32_GetTokenInformation_1 = 2;

		// Token: 0x040015AB RID: 5547
		private const int Win32_GetTokenInformation_2 = 3;

		// Token: 0x040015AC RID: 5548
		private const int Win32_ConvertSidToStringSidW = 4;

		// Token: 0x040015AD RID: 5549
		private const int Win32_CreateWellKnownSid = 5;

		// Token: 0x040015AE RID: 5550
		public static readonly DbConnectionPoolIdentity NoIdentity = new DbConnectionPoolIdentity(string.Empty, false, true);

		// Token: 0x040015AF RID: 5551
		private static readonly byte[] NetworkSid = (ADP.IsWindowsNT ? DbConnectionPoolIdentity.CreateWellKnownSid(WellKnownSidType.NetworkSid) : null);

		// Token: 0x040015B0 RID: 5552
		private readonly string _sidString;

		// Token: 0x040015B1 RID: 5553
		private readonly bool _isRestricted;

		// Token: 0x040015B2 RID: 5554
		private readonly bool _isNetwork;
	}
}
