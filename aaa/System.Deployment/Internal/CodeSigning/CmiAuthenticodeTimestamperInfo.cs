using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x020001E8 RID: 488
	internal class CmiAuthenticodeTimestamperInfo
	{
		// Token: 0x060008BC RID: 2236 RVA: 0x00022AF3 File Offset: 0x00021AF3
		private CmiAuthenticodeTimestamperInfo()
		{
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00022AFC File Offset: 0x00021AFC
		internal CmiAuthenticodeTimestamperInfo(Win32.AXL_TIMESTAMPER_INFO timestamperInfo)
		{
			this.m_error = (int)timestamperInfo.dwError;
			this.m_algHash = timestamperInfo.algHash;
			long num = (long)(((ulong)timestamperInfo.ftTimestamp.dwHighDateTime << 32) | (ulong)timestamperInfo.ftTimestamp.dwLowDateTime);
			this.m_timestampTime = DateTime.FromFileTime(num);
			if (timestamperInfo.pChainContext != IntPtr.Zero)
			{
				this.m_timestamperChain = new X509Chain(timestamperInfo.pChainContext);
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00022B79 File Offset: 0x00021B79
		internal int ErrorCode
		{
			get
			{
				return this.m_error;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00022B81 File Offset: 0x00021B81
		internal uint HashAlgId
		{
			get
			{
				return this.m_algHash;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00022B89 File Offset: 0x00021B89
		internal DateTime TimestampTime
		{
			get
			{
				return this.m_timestampTime;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00022B91 File Offset: 0x00021B91
		internal X509Chain TimestamperChain
		{
			get
			{
				return this.m_timestamperChain;
			}
		}

		// Token: 0x04000839 RID: 2105
		private int m_error;

		// Token: 0x0400083A RID: 2106
		private X509Chain m_timestamperChain;

		// Token: 0x0400083B RID: 2107
		private DateTime m_timestampTime;

		// Token: 0x0400083C RID: 2108
		private uint m_algHash;
	}
}
