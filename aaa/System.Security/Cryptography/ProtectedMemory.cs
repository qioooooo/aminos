using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x0200005B RID: 91
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class ProtectedMemory
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00003F60 File Offset: 0x00002F60
		private ProtectedMemory()
		{
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003F68 File Offset: 0x00002F68
		public static void Protect(byte[] userData, MemoryProtectionScope scope)
		{
			if (userData == null)
			{
				throw new ArgumentNullException("userData");
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
			ProtectedMemory.VerifyScope(scope);
			if (userData.Length == 0 || (long)userData.Length % 16L != 0L)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_DpApi_InvalidMemoryLength"));
			}
			try
			{
				int num = CAPI.SystemFunction040(userData, (uint)userData.Length, (uint)scope);
				if (num < 0)
				{
					throw new CryptographicException(CAPISafe.LsaNtStatusToWinError(num));
				}
			}
			catch (EntryPointNotFoundException)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004008 File Offset: 0x00003008
		public static void Unprotect(byte[] encryptedData, MemoryProtectionScope scope)
		{
			if (encryptedData == null)
			{
				throw new ArgumentNullException("encryptedData");
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
			ProtectedMemory.VerifyScope(scope);
			if (encryptedData.Length == 0 || (long)encryptedData.Length % 16L != 0L)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_DpApi_InvalidMemoryLength"));
			}
			try
			{
				int num = CAPI.SystemFunction041(encryptedData, (uint)encryptedData.Length, (uint)scope);
				if (num < 0)
				{
					throw new CryptographicException(CAPISafe.LsaNtStatusToWinError(num));
				}
			}
			catch (EntryPointNotFoundException)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000040A8 File Offset: 0x000030A8
		private static void VerifyScope(MemoryProtectionScope scope)
		{
			if (scope != MemoryProtectionScope.SameProcess && scope != MemoryProtectionScope.CrossProcess && scope != MemoryProtectionScope.SameLogon)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int)scope }));
			}
		}
	}
}
