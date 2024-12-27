using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200013F RID: 319
	internal class CmiAuthenticodeSignerInfo
	{
		// Token: 0x060004B4 RID: 1204 RVA: 0x0000B128 File Offset: 0x0000A128
		internal CmiAuthenticodeSignerInfo()
		{
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000B130 File Offset: 0x0000A130
		internal CmiAuthenticodeSignerInfo(int errorCode)
		{
			this.m_error = errorCode;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000B140 File Offset: 0x0000A140
		internal CmiAuthenticodeSignerInfo(Win32.AXL_SIGNER_INFO signerInfo, Win32.AXL_TIMESTAMPER_INFO timestamperInfo)
		{
			this.m_error = (int)signerInfo.dwError;
			if (signerInfo.pChainContext != IntPtr.Zero)
			{
				this.m_signerChain = new X509Chain(signerInfo.pChainContext);
			}
			this.m_algHash = signerInfo.algHash;
			if (signerInfo.pwszHash != IntPtr.Zero)
			{
				this.m_hash = Marshal.PtrToStringUni(signerInfo.pwszHash);
			}
			if (signerInfo.pwszDescription != IntPtr.Zero)
			{
				this.m_description = Marshal.PtrToStringUni(signerInfo.pwszDescription);
			}
			if (signerInfo.pwszDescriptionUrl != IntPtr.Zero)
			{
				this.m_descriptionUrl = Marshal.PtrToStringUni(signerInfo.pwszDescriptionUrl);
			}
			if (timestamperInfo.dwError != 2148204800U)
			{
				this.m_timestamperInfo = new CmiAuthenticodeTimestamperInfo(timestamperInfo);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0000B21B File Offset: 0x0000A21B
		internal int ErrorCode
		{
			get
			{
				return this.m_error;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0000B223 File Offset: 0x0000A223
		internal uint HashAlgId
		{
			get
			{
				return this.m_algHash;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000B22B File Offset: 0x0000A22B
		internal string Hash
		{
			get
			{
				return this.m_hash;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0000B233 File Offset: 0x0000A233
		internal string Description
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000B23B File Offset: 0x0000A23B
		internal string DescriptionUrl
		{
			get
			{
				return this.m_descriptionUrl;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0000B243 File Offset: 0x0000A243
		internal CmiAuthenticodeTimestamperInfo TimestamperInfo
		{
			get
			{
				return this.m_timestamperInfo;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0000B24B File Offset: 0x0000A24B
		internal X509Chain SignerChain
		{
			get
			{
				return this.m_signerChain;
			}
		}

		// Token: 0x04000ED5 RID: 3797
		private int m_error;

		// Token: 0x04000ED6 RID: 3798
		private X509Chain m_signerChain;

		// Token: 0x04000ED7 RID: 3799
		private uint m_algHash;

		// Token: 0x04000ED8 RID: 3800
		private string m_hash;

		// Token: 0x04000ED9 RID: 3801
		private string m_description;

		// Token: 0x04000EDA RID: 3802
		private string m_descriptionUrl;

		// Token: 0x04000EDB RID: 3803
		private CmiAuthenticodeTimestamperInfo m_timestamperInfo;
	}
}
