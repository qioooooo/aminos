using System;
using System.ComponentModel;
using System.DirectoryServices.Interop;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000021 RID: 33
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class DirectoryEntryConfiguration
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00004464 File Offset: 0x00003464
		internal DirectoryEntryConfiguration(DirectoryEntry entry)
		{
			this.entry = entry;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004473 File Offset: 0x00003473
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00004490 File Offset: 0x00003490
		public ReferralChasingOption Referral
		{
			get
			{
				return (ReferralChasingOption)((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).GetOption(1);
			}
			set
			{
				if (value != ReferralChasingOption.None && value != ReferralChasingOption.Subordinate && value != ReferralChasingOption.External && value != ReferralChasingOption.All)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ReferralChasingOption));
				}
				((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).SetOption(1, value);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000044E1 File Offset: 0x000034E1
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000044FE File Offset: 0x000034FE
		public SecurityMasks SecurityMasks
		{
			get
			{
				return (SecurityMasks)((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).GetOption(3);
			}
			set
			{
				if (value > (SecurityMasks.Owner | SecurityMasks.Group | SecurityMasks.Dacl | SecurityMasks.Sacl))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SecurityMasks));
				}
				((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).SetOption(3, value);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004537 File Offset: 0x00003537
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00004554 File Offset: 0x00003554
		public int PageSize
		{
			get
			{
				return (int)((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).GetOption(2);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadPageSize"));
				}
				((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).SetOption(2, value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004586 File Offset: 0x00003586
		// (set) Token: 0x060000BD RID: 189 RVA: 0x000045A3 File Offset: 0x000035A3
		public int PasswordPort
		{
			get
			{
				return (int)((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).GetOption(6);
			}
			set
			{
				((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).SetOption(6, value);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000045C1 File Offset: 0x000035C1
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000045DE File Offset: 0x000035DE
		public PasswordEncodingMethod PasswordEncoding
		{
			get
			{
				return (PasswordEncodingMethod)((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).GetOption(7);
			}
			set
			{
				if (value < PasswordEncodingMethod.PasswordEncodingSsl || value > PasswordEncodingMethod.PasswordEncodingClear)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PasswordEncodingMethod));
				}
				((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).SetOption(7, value);
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000461A File Offset: 0x0000361A
		public string GetCurrentServerName()
		{
			return (string)((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).GetOption(0);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004638 File Offset: 0x00003638
		public bool IsMutuallyAuthenticated()
		{
			bool flag;
			try
			{
				int num = (int)((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).GetOption(4);
				if ((num & 2) != 0)
				{
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147463160)
				{
					throw;
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004694 File Offset: 0x00003694
		public void SetUserNameQueryQuota(string accountName)
		{
			((UnsafeNativeMethods.IAdsObjectOptions)this.entry.AdsObject).SetOption(5, accountName);
		}

		// Token: 0x0400017F RID: 383
		private const int ISC_RET_MUTUAL_AUTH = 2;

		// Token: 0x04000180 RID: 384
		private DirectoryEntry entry;
	}
}
