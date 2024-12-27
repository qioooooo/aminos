using System;
using System.Security.Cryptography;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x020001E6 RID: 486
	internal class CmiStrongNameSignerInfo
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x00022977 File Offset: 0x00021977
		internal CmiStrongNameSignerInfo()
		{
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002297F File Offset: 0x0002197F
		internal CmiStrongNameSignerInfo(int errorCode, string publicKeyToken)
		{
			this.m_error = errorCode;
			this.m_publicKeyToken = publicKeyToken;
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00022995 File Offset: 0x00021995
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x0002299D File Offset: 0x0002199D
		internal int ErrorCode
		{
			get
			{
				return this.m_error;
			}
			set
			{
				this.m_error = value;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x000229A6 File Offset: 0x000219A6
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x000229AE File Offset: 0x000219AE
		internal string PublicKeyToken
		{
			get
			{
				return this.m_publicKeyToken;
			}
			set
			{
				this.m_publicKeyToken = value;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x000229B7 File Offset: 0x000219B7
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x000229BF File Offset: 0x000219BF
		internal AsymmetricAlgorithm PublicKey
		{
			get
			{
				return this.m_snKey;
			}
			set
			{
				this.m_snKey = value;
			}
		}

		// Token: 0x0400082F RID: 2095
		private int m_error;

		// Token: 0x04000830 RID: 2096
		private string m_publicKeyToken;

		// Token: 0x04000831 RID: 2097
		private AsymmetricAlgorithm m_snKey;
	}
}
