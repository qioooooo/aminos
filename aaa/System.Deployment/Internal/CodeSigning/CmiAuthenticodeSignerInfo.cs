using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x020001E7 RID: 487
	internal class CmiAuthenticodeSignerInfo
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x000229C8 File Offset: 0x000219C8
		internal CmiAuthenticodeSignerInfo()
		{
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000229D0 File Offset: 0x000219D0
		internal CmiAuthenticodeSignerInfo(int errorCode)
		{
			this.m_error = errorCode;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x000229E0 File Offset: 0x000219E0
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

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00022ABB File Offset: 0x00021ABB
		internal int ErrorCode
		{
			get
			{
				return this.m_error;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00022AC3 File Offset: 0x00021AC3
		internal uint HashAlgId
		{
			get
			{
				return this.m_algHash;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00022ACB File Offset: 0x00021ACB
		internal string Hash
		{
			get
			{
				return this.m_hash;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00022AD3 File Offset: 0x00021AD3
		internal string Description
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00022ADB File Offset: 0x00021ADB
		internal string DescriptionUrl
		{
			get
			{
				return this.m_descriptionUrl;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00022AE3 File Offset: 0x00021AE3
		internal CmiAuthenticodeTimestamperInfo TimestamperInfo
		{
			get
			{
				return this.m_timestamperInfo;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00022AEB File Offset: 0x00021AEB
		internal X509Chain SignerChain
		{
			get
			{
				return this.m_signerChain;
			}
		}

		// Token: 0x04000832 RID: 2098
		private int m_error;

		// Token: 0x04000833 RID: 2099
		private X509Chain m_signerChain;

		// Token: 0x04000834 RID: 2100
		private uint m_algHash;

		// Token: 0x04000835 RID: 2101
		private string m_hash;

		// Token: 0x04000836 RID: 2102
		private string m_description;

		// Token: 0x04000837 RID: 2103
		private string m_descriptionUrl;

		// Token: 0x04000838 RID: 2104
		private CmiAuthenticodeTimestamperInfo m_timestamperInfo;
	}
}
