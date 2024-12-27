using System;

namespace SafeNet.Sentinel
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class VendorCodeType
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002101 File Offset: 0x00000301
		private VendorCodeType()
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000210C File Offset: 0x0000030C
		public VendorCodeType(string vendorCode)
		{
			this.m_vendorCode = vendorCode;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002120 File Offset: 0x00000320
		public override string ToString()
		{
			return this.m_vendorCode;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002138 File Offset: 0x00000338
		public void clear()
		{
			this.m_vendorCode = "";
		}

		// Token: 0x04000001 RID: 1
		private string m_vendorCode;
	}
}
