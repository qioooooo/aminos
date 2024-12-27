using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x0200005A RID: 90
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class ProtectedData
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00003BEF File Offset: 0x00002BEF
		private ProtectedData()
		{
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003BF8 File Offset: 0x00002BF8
		public unsafe static byte[] Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope)
		{
			if (userData == null)
			{
				throw new ArgumentNullException("userData");
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
			GCHandle gchandle = default(GCHandle);
			GCHandle gchandle2 = default(GCHandle);
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
			RuntimeHelpers.PrepareConstrainedRegions();
			byte[] array2;
			try
			{
				gchandle = GCHandle.Alloc(userData, GCHandleType.Pinned);
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB2 = default(CAPIBase.CRYPTOAPI_BLOB);
				cryptoapi_BLOB2.cbData = (uint)userData.Length;
				cryptoapi_BLOB2.pbData = gchandle.AddrOfPinnedObject();
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB3 = default(CAPIBase.CRYPTOAPI_BLOB);
				if (optionalEntropy != null)
				{
					gchandle2 = GCHandle.Alloc(optionalEntropy, GCHandleType.Pinned);
					cryptoapi_BLOB3.cbData = (uint)optionalEntropy.Length;
					cryptoapi_BLOB3.pbData = gchandle2.AddrOfPinnedObject();
				}
				uint num = 1U;
				if (scope == DataProtectionScope.LocalMachine)
				{
					num |= 4U;
				}
				if (!CAPI.CryptProtectData(new IntPtr((void*)(&cryptoapi_BLOB2)), string.Empty, new IntPtr((void*)(&cryptoapi_BLOB3)), IntPtr.Zero, IntPtr.Zero, num, new IntPtr((void*)(&cryptoapi_BLOB))))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				if (cryptoapi_BLOB.pbData == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				byte[] array = new byte[cryptoapi_BLOB.cbData];
				Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, array.Length);
				array2 = array;
			}
			catch (EntryPointNotFoundException)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (gchandle2.IsAllocated)
				{
					gchandle2.Free();
				}
				if (cryptoapi_BLOB.pbData != IntPtr.Zero)
				{
					CAPISafe.ZeroMemory(cryptoapi_BLOB.pbData, cryptoapi_BLOB.cbData);
					CAPISafe.LocalFree(cryptoapi_BLOB.pbData);
				}
			}
			return array2;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003DAC File Offset: 0x00002DAC
		public unsafe static byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy, DataProtectionScope scope)
		{
			if (encryptedData == null)
			{
				throw new ArgumentNullException("encryptedData");
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
			GCHandle gchandle = default(GCHandle);
			GCHandle gchandle2 = default(GCHandle);
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
			RuntimeHelpers.PrepareConstrainedRegions();
			byte[] array2;
			try
			{
				gchandle = GCHandle.Alloc(encryptedData, GCHandleType.Pinned);
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB2 = default(CAPIBase.CRYPTOAPI_BLOB);
				cryptoapi_BLOB2.cbData = (uint)encryptedData.Length;
				cryptoapi_BLOB2.pbData = gchandle.AddrOfPinnedObject();
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB3 = default(CAPIBase.CRYPTOAPI_BLOB);
				if (optionalEntropy != null)
				{
					gchandle2 = GCHandle.Alloc(optionalEntropy, GCHandleType.Pinned);
					cryptoapi_BLOB3.cbData = (uint)optionalEntropy.Length;
					cryptoapi_BLOB3.pbData = gchandle2.AddrOfPinnedObject();
				}
				uint num = 1U;
				if (scope == DataProtectionScope.LocalMachine)
				{
					num |= 4U;
				}
				if (!CAPI.CryptUnprotectData(new IntPtr((void*)(&cryptoapi_BLOB2)), IntPtr.Zero, new IntPtr((void*)(&cryptoapi_BLOB3)), IntPtr.Zero, IntPtr.Zero, num, new IntPtr((void*)(&cryptoapi_BLOB))))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				if (cryptoapi_BLOB.pbData == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				byte[] array = new byte[cryptoapi_BLOB.cbData];
				Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, array.Length);
				array2 = array;
			}
			catch (EntryPointNotFoundException)
			{
				throw new NotSupportedException(SecurityResources.GetResourceString("NotSupported_PlatformRequiresNT"));
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (gchandle2.IsAllocated)
				{
					gchandle2.Free();
				}
				if (cryptoapi_BLOB.pbData != IntPtr.Zero)
				{
					CAPISafe.ZeroMemory(cryptoapi_BLOB.pbData, cryptoapi_BLOB.cbData);
					CAPISafe.LocalFree(cryptoapi_BLOB.pbData);
				}
			}
			return array2;
		}
	}
}
