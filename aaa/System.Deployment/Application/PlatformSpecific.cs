using System;

namespace System.Deployment.Application
{
	// Token: 0x020000C4 RID: 196
	internal static class PlatformSpecific
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00018830 File Offset: 0x00017830
		public static bool OnWin9x
		{
			get
			{
				OperatingSystem osversion = Environment.OSVersion;
				return osversion.Platform == PlatformID.Win32Windows;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0001884C File Offset: 0x0001784C
		public static bool OnWinMe
		{
			get
			{
				OperatingSystem osversion = Environment.OSVersion;
				return osversion.Platform == PlatformID.Win32Windows && osversion.Version.Major == 4 && osversion.Version.Minor == 90;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00018888 File Offset: 0x00017888
		public static bool OnXPOrAbove
		{
			get
			{
				OperatingSystem osversion = Environment.OSVersion;
				return osversion.Platform == PlatformID.Win32NT && ((osversion.Version.Major == 5 && osversion.Version.Minor >= 1) || osversion.Version.Major >= 6);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x000188D8 File Offset: 0x000178D8
		public static bool OnVistaOrAbove
		{
			get
			{
				OperatingSystem osversion = Environment.OSVersion;
				return osversion.Platform == PlatformID.Win32NT && osversion.Version.Major >= 6;
			}
		}
	}
}
