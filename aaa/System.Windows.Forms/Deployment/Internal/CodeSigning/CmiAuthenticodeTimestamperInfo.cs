using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000140 RID: 320
	internal class CmiAuthenticodeTimestamperInfo
	{
		// Token: 0x060004BE RID: 1214 RVA: 0x0000B253 File Offset: 0x0000A253
		private CmiAuthenticodeTimestamperInfo()
		{
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000B25C File Offset: 0x0000A25C
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

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x0000B2D9 File Offset: 0x0000A2D9
		internal int ErrorCode
		{
			get
			{
				return this.m_error;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0000B2E1 File Offset: 0x0000A2E1
		internal uint HashAlgId
		{
			get
			{
				return this.m_algHash;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0000B2E9 File Offset: 0x0000A2E9
		internal DateTime TimestampTime
		{
			get
			{
				return this.m_timestampTime;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0000B2F1 File Offset: 0x0000A2F1
		internal X509Chain TimestamperChain
		{
			get
			{
				return this.m_timestamperChain;
			}
		}

		// Token: 0x04000EDC RID: 3804
		private int m_error;

		// Token: 0x04000EDD RID: 3805
		private X509Chain m_timestamperChain;

		// Token: 0x04000EDE RID: 3806
		private DateTime m_timestampTime;

		// Token: 0x04000EDF RID: 3807
		private uint m_algHash;
	}
}
