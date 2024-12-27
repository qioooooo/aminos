using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000A2 RID: 162
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public struct X509IssuerSerial
	{
		// Token: 0x06000317 RID: 791 RVA: 0x000101CC File Offset: 0x0000F1CC
		internal X509IssuerSerial(string issuerName, string serialNumber)
		{
			if (issuerName == null || issuerName.Length == 0)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Arg_EmptyOrNullString"), "issuerName");
			}
			if (serialNumber == null || serialNumber.Length == 0)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Arg_EmptyOrNullString"), "serialNumber");
			}
			this.issuerName = issuerName;
			this.serialNumber = serialNumber;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00010227 File Offset: 0x0000F227
		// (set) Token: 0x06000319 RID: 793 RVA: 0x0001022F File Offset: 0x0000F22F
		public string IssuerName
		{
			get
			{
				return this.issuerName;
			}
			set
			{
				this.issuerName = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600031A RID: 794 RVA: 0x00010238 File Offset: 0x0000F238
		// (set) Token: 0x0600031B RID: 795 RVA: 0x00010240 File Offset: 0x0000F240
		public string SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
			set
			{
				this.serialNumber = value;
			}
		}

		// Token: 0x040004FC RID: 1276
		private string issuerName;

		// Token: 0x040004FD RID: 1277
		private string serialNumber;
	}
}
