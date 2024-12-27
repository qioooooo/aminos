using System;

namespace System.Net
{
	// Token: 0x02000425 RID: 1061
	public class IPHostEntry
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x00082976 File Offset: 0x00081976
		// (set) Token: 0x06002118 RID: 8472 RVA: 0x0008297E File Offset: 0x0008197E
		public string HostName
		{
			get
			{
				return this.hostName;
			}
			set
			{
				this.hostName = value;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002119 RID: 8473 RVA: 0x00082987 File Offset: 0x00081987
		// (set) Token: 0x0600211A RID: 8474 RVA: 0x0008298F File Offset: 0x0008198F
		public string[] Aliases
		{
			get
			{
				return this.aliases;
			}
			set
			{
				this.aliases = value;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x00082998 File Offset: 0x00081998
		// (set) Token: 0x0600211C RID: 8476 RVA: 0x000829A0 File Offset: 0x000819A0
		public IPAddress[] AddressList
		{
			get
			{
				return this.addressList;
			}
			set
			{
				this.addressList = value;
			}
		}

		// Token: 0x0400215D RID: 8541
		private string hostName;

		// Token: 0x0400215E RID: 8542
		private string[] aliases;

		// Token: 0x0400215F RID: 8543
		private IPAddress[] addressList;
	}
}
