using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000AC RID: 172
	public class ForestTrustDomainInformation
	{
		// Token: 0x060005B7 RID: 1463 RVA: 0x0002090C File Offset: 0x0001F90C
		internal ForestTrustDomainInformation(int flag, LSA_FOREST_TRUST_DOMAIN_INFO domainInfo, LARGE_INTEGER time)
		{
			this.status = (ForestTrustDomainStatus)flag;
			this.dnsName = Marshal.PtrToStringUni(domainInfo.DNSNameBuffer, (int)(domainInfo.DNSNameLength / 2));
			this.nbName = Marshal.PtrToStringUni(domainInfo.NetBIOSNameBuffer, (int)(domainInfo.NetBIOSNameLength / 2));
			IntPtr intPtr = (IntPtr)0;
			if (UnsafeNativeMethods.ConvertSidToStringSidW(domainInfo.sid, ref intPtr) == 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			try
			{
				this.sid = Marshal.PtrToStringUni(intPtr);
			}
			finally
			{
				UnsafeNativeMethods.LocalFree(intPtr);
			}
			this.time = time;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x000209A8 File Offset: 0x0001F9A8
		public string DnsName
		{
			get
			{
				return this.dnsName;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000209B0 File Offset: 0x0001F9B0
		public string NetBiosName
		{
			get
			{
				return this.nbName;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000209B8 File Offset: 0x0001F9B8
		public string DomainSid
		{
			get
			{
				return this.sid;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x000209C0 File Offset: 0x0001F9C0
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x000209C8 File Offset: 0x0001F9C8
		public ForestTrustDomainStatus Status
		{
			get
			{
				return this.status;
			}
			set
			{
				if (value != ForestTrustDomainStatus.Enabled && value != ForestTrustDomainStatus.SidAdminDisabled && value != ForestTrustDomainStatus.SidConflictDisabled && value != ForestTrustDomainStatus.NetBiosNameAdminDisabled && value != ForestTrustDomainStatus.NetBiosNameConflictDisabled)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ForestTrustDomainStatus));
				}
				this.status = value;
			}
		}

		// Token: 0x0400046A RID: 1130
		private string dnsName;

		// Token: 0x0400046B RID: 1131
		private string nbName;

		// Token: 0x0400046C RID: 1132
		private string sid;

		// Token: 0x0400046D RID: 1133
		private ForestTrustDomainStatus status;

		// Token: 0x0400046E RID: 1134
		internal LARGE_INTEGER time;
	}
}
