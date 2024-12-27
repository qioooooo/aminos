using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x02000932 RID: 2354
	internal sealed class Win32
	{
		// Token: 0x06005566 RID: 21862 RVA: 0x00136DD0 File Offset: 0x00135DD0
		static Win32()
		{
			Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
			if (!Win32Native.GetVersionEx(osversioninfo))
			{
				throw new SystemException(Environment.GetResourceString("InvalidOperation_GetVersion"));
			}
			if (osversioninfo.PlatformId != 2 || osversioninfo.MajorVersion < 5)
			{
				Win32._LsaApisSupported = false;
				Win32._LsaLookupNames2Supported = false;
				Win32._ConvertStringSidToSidSupported = false;
				Win32._WellKnownSidApisSupported = false;
				return;
			}
			Win32._ConvertStringSidToSidSupported = true;
			Win32._LsaApisSupported = true;
			if (osversioninfo.MajorVersion > 5 || osversioninfo.MinorVersion > 0)
			{
				Win32._LsaLookupNames2Supported = true;
				Win32._WellKnownSidApisSupported = true;
				return;
			}
			Win32._LsaLookupNames2Supported = false;
			Win32Native.OSVERSIONINFOEX osversioninfoex = new Win32Native.OSVERSIONINFOEX();
			if (!Win32Native.GetVersionEx(osversioninfoex))
			{
				throw new SystemException(Environment.GetResourceString("InvalidOperation_GetVersion"));
			}
			if (osversioninfoex.ServicePackMajor < 3)
			{
				Win32._WellKnownSidApisSupported = false;
				return;
			}
			Win32._WellKnownSidApisSupported = true;
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x00136E8F File Offset: 0x00135E8F
		private Win32()
		{
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06005568 RID: 21864 RVA: 0x00136E97 File Offset: 0x00135E97
		internal static bool SddlConversionSupported
		{
			get
			{
				return Win32._ConvertStringSidToSidSupported;
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06005569 RID: 21865 RVA: 0x00136E9E File Offset: 0x00135E9E
		internal static bool LsaApisSupported
		{
			get
			{
				return Win32._LsaApisSupported;
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x0600556A RID: 21866 RVA: 0x00136EA5 File Offset: 0x00135EA5
		internal static bool LsaLookupNames2Supported
		{
			get
			{
				return Win32._LsaLookupNames2Supported;
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x0600556B RID: 21867 RVA: 0x00136EAC File Offset: 0x00135EAC
		internal static bool WellKnownSidApisSupported
		{
			get
			{
				return Win32._WellKnownSidApisSupported;
			}
		}

		// Token: 0x0600556C RID: 21868 RVA: 0x00136EB4 File Offset: 0x00135EB4
		internal static SafeLsaPolicyHandle LsaOpenPolicy(string systemName, PolicyRights rights)
		{
			if (!Win32.LsaApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			Win32Native.LSA_OBJECT_ATTRIBUTES lsa_OBJECT_ATTRIBUTES;
			lsa_OBJECT_ATTRIBUTES.Length = Marshal.SizeOf(typeof(Win32Native.LSA_OBJECT_ATTRIBUTES));
			lsa_OBJECT_ATTRIBUTES.RootDirectory = IntPtr.Zero;
			lsa_OBJECT_ATTRIBUTES.ObjectName = IntPtr.Zero;
			lsa_OBJECT_ATTRIBUTES.Attributes = 0;
			lsa_OBJECT_ATTRIBUTES.SecurityDescriptor = IntPtr.Zero;
			lsa_OBJECT_ATTRIBUTES.SecurityQualityOfService = IntPtr.Zero;
			SafeLsaPolicyHandle safeLsaPolicyHandle;
			uint num;
			if ((num = Win32Native.LsaOpenPolicy(systemName, ref lsa_OBJECT_ATTRIBUTES, (int)rights, out safeLsaPolicyHandle)) == 0U)
			{
				return safeLsaPolicyHandle;
			}
			if (num == 3221225506U)
			{
				throw new UnauthorizedAccessException();
			}
			if (num == 3221225626U || num == 3221225495U)
			{
				throw new OutOfMemoryException();
			}
			int num2 = Win32Native.LsaNtStatusToWinError((int)num);
			throw new SystemException(Win32Native.GetMessage(num2));
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x00136F70 File Offset: 0x00135F70
		internal static byte[] ConvertIntPtrSidToByteArraySid(IntPtr binaryForm)
		{
			byte b = Marshal.ReadByte(binaryForm, 0);
			if (b != SecurityIdentifier.Revision)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_InvalidSidRevision"), "binaryForm");
			}
			byte b2 = Marshal.ReadByte(binaryForm, 1);
			if (b2 < 0 || b2 > SecurityIdentifier.MaxSubAuthorities)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_InvalidNumberOfSubauthorities", new object[] { SecurityIdentifier.MaxSubAuthorities }), "binaryForm");
			}
			int num = (int)(8 + b2 * 4);
			byte[] array = new byte[num];
			Marshal.Copy(binaryForm, array, 0, num);
			return array;
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x00136FFC File Offset: 0x00135FFC
		internal static int CreateSidFromString(string stringSid, out byte[] resultSid)
		{
			IntPtr zero = IntPtr.Zero;
			if (!Win32.SddlConversionSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			int lastWin32Error;
			try
			{
				if (1 != Win32Native.ConvertStringSidToSid(stringSid, out zero))
				{
					lastWin32Error = Marshal.GetLastWin32Error();
					goto IL_0044;
				}
				resultSid = Win32.ConvertIntPtrSidToByteArraySid(zero);
			}
			finally
			{
				Win32Native.LocalFree(zero);
			}
			return 0;
			IL_0044:
			resultSid = null;
			return lastWin32Error;
		}

		// Token: 0x0600556F RID: 21871 RVA: 0x00137064 File Offset: 0x00136064
		internal static int CreateWellKnownSid(WellKnownSidType sidType, SecurityIdentifier domainSid, out byte[] resultSid)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			uint maxBinaryLength = (uint)SecurityIdentifier.MaxBinaryLength;
			resultSid = new byte[maxBinaryLength];
			if (Win32Native.CreateWellKnownSid((int)sidType, (domainSid == null) ? null : domainSid.BinaryForm, resultSid, ref maxBinaryLength) != 0)
			{
				return 0;
			}
			resultSid = null;
			return Marshal.GetLastWin32Error();
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x001370C0 File Offset: 0x001360C0
		internal static bool IsEqualDomainSid(SecurityIdentifier sid1, SecurityIdentifier sid2)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			if (sid1 == null || sid2 == null)
			{
				return false;
			}
			byte[] array = new byte[sid1.BinaryLength];
			sid1.GetBinaryForm(array, 0);
			byte[] array2 = new byte[sid2.BinaryLength];
			sid2.GetBinaryForm(array2, 0);
			bool flag;
			return Win32Native.IsEqualDomainSid(array, array2, out flag) != 0 && flag;
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x00137130 File Offset: 0x00136130
		internal static int GetWindowsAccountDomainSid(SecurityIdentifier sid, out SecurityIdentifier resultSid)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			byte[] array = new byte[sid.BinaryLength];
			sid.GetBinaryForm(array, 0);
			uint maxBinaryLength = (uint)SecurityIdentifier.MaxBinaryLength;
			byte[] array2 = new byte[maxBinaryLength];
			if (Win32Native.GetWindowsAccountDomainSid(array, array2, ref maxBinaryLength) != 0)
			{
				resultSid = new SecurityIdentifier(array2, 0);
				return 0;
			}
			resultSid = null;
			return Marshal.GetLastWin32Error();
		}

		// Token: 0x06005572 RID: 21874 RVA: 0x00137194 File Offset: 0x00136194
		internal static bool IsWellKnownSid(SecurityIdentifier sid, WellKnownSidType type)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			byte[] array = new byte[sid.BinaryLength];
			sid.GetBinaryForm(array, 0);
			return Win32Native.IsWellKnownSid(array, (int)type) != 0;
		}

		// Token: 0x06005573 RID: 21875
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int ImpersonateLoggedOnUser(SafeTokenHandle hToken);

		// Token: 0x06005574 RID: 21876
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int OpenThreadToken(TokenAccessLevels dwDesiredAccess, WinSecurityContext OpenAs, out SafeTokenHandle phThreadToken);

		// Token: 0x06005575 RID: 21877
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RevertToSelf();

		// Token: 0x06005576 RID: 21878
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int SetThreadToken(SafeTokenHandle hToken);

		// Token: 0x04002C83 RID: 11395
		internal const int FALSE = 0;

		// Token: 0x04002C84 RID: 11396
		internal const int TRUE = 1;

		// Token: 0x04002C85 RID: 11397
		private static bool _LsaApisSupported;

		// Token: 0x04002C86 RID: 11398
		private static bool _LsaLookupNames2Supported;

		// Token: 0x04002C87 RID: 11399
		private static bool _ConvertStringSidToSidSupported;

		// Token: 0x04002C88 RID: 11400
		private static bool _WellKnownSidApisSupported;
	}
}
