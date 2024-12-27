using System;
using System.Security.Permissions;

namespace System.Web.Services.Description
{
	// Token: 0x0200011F RID: 287
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class SoapTransportImporter
	{
		// Token: 0x060008C3 RID: 2243
		public abstract bool IsSupportedTransport(string transport);

		// Token: 0x060008C4 RID: 2244
		public abstract void ImportClass();

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00040FCA File Offset: 0x0003FFCA
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x00040FD2 File Offset: 0x0003FFD2
		public SoapProtocolImporter ImportContext
		{
			get
			{
				return this.protocolImporter;
			}
			set
			{
				this.protocolImporter = value;
			}
		}

		// Token: 0x040005C6 RID: 1478
		private SoapProtocolImporter protocolImporter;
	}
}
