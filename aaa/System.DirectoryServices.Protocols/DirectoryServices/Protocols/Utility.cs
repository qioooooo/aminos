using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200006B RID: 107
	internal class Utility
	{
		// Token: 0x06000230 RID: 560 RVA: 0x0000A48C File Offset: 0x0000948C
		static Utility()
		{
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Platform == PlatformID.Win32NT && osversion.Version.Major >= 5)
			{
				Utility.platformSupported = true;
				if (osversion.Version.Major == 5 && osversion.Version.Minor == 0)
				{
					Utility.isWin2kOS = true;
				}
				if (osversion.Version.Major > 5 || osversion.Version.Minor >= 2)
				{
					Utility.isWin2k3Above = true;
				}
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000A4FF File Offset: 0x000094FF
		internal static void CheckOSVersion()
		{
			if (!Utility.platformSupported)
			{
				throw new PlatformNotSupportedException(Res.GetString("SupportedPlatforms"));
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000A518 File Offset: 0x00009518
		internal static bool IsWin2kOS
		{
			get
			{
				return Utility.isWin2kOS;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000A51F File Offset: 0x0000951F
		internal static bool IsWin2k3AboveOS
		{
			get
			{
				return Utility.isWin2k3Above;
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000A526 File Offset: 0x00009526
		internal static bool IsLdapError(LdapError error)
		{
			return error == LdapError.IsLeaf || error == LdapError.InvalidCredentials || error == LdapError.SendTimeOut || (error >= LdapError.ServerDown && error <= LdapError.ReferralLimitExceeded);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000A548 File Offset: 0x00009548
		internal static bool IsResultCode(ResultCode code)
		{
			return (code >= ResultCode.Success && code <= ResultCode.SaslBindInProgress) || (code >= ResultCode.NoSuchAttribute && code <= ResultCode.InvalidAttributeSyntax) || (code >= ResultCode.NoSuchObject && code <= ResultCode.InvalidDNSyntax) || (code >= ResultCode.InsufficientAccessRights && code <= ResultCode.LoopDetect) || (code >= ResultCode.NamingViolation && code <= ResultCode.AffectsMultipleDsas) || (code == ResultCode.AliasDereferencingProblem || code == ResultCode.InappropriateAuthentication || code == ResultCode.SortControlMissing || code == ResultCode.OffsetRangeError || code == ResultCode.VirtualListViewError || code == ResultCode.Other);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000A5B4 File Offset: 0x000095B4
		internal static IntPtr AllocHGlobalIntPtrArray(int size)
		{
			IntPtr intPtr = (IntPtr)0;
			checked
			{
				intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * size);
				for (int i = 0; i < size; i++)
				{
					IntPtr intPtr2 = (IntPtr)((long)intPtr + unchecked((long)(checked(Marshal.SizeOf(typeof(IntPtr)) * i))));
					Marshal.WriteIntPtr(intPtr2, IntPtr.Zero);
				}
				return intPtr;
			}
		}

		// Token: 0x0400021A RID: 538
		private static bool platformSupported;

		// Token: 0x0400021B RID: 539
		private static bool isWin2kOS;

		// Token: 0x0400021C RID: 540
		private static bool isWin2k3Above;
	}
}
