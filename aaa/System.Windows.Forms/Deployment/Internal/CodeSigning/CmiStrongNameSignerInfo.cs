using System;
using System.Security.Cryptography;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200013E RID: 318
	internal class CmiStrongNameSignerInfo
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x0000B0D7 File Offset: 0x0000A0D7
		internal CmiStrongNameSignerInfo()
		{
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000B0DF File Offset: 0x0000A0DF
		internal CmiStrongNameSignerInfo(int errorCode, string publicKeyToken)
		{
			this.m_error = errorCode;
			this.m_publicKeyToken = publicKeyToken;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0000B0F5 File Offset: 0x0000A0F5
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x0000B0FD File Offset: 0x0000A0FD
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

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0000B106 File Offset: 0x0000A106
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x0000B10E File Offset: 0x0000A10E
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

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0000B117 File Offset: 0x0000A117
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x0000B11F File Offset: 0x0000A11F
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

		// Token: 0x04000ED2 RID: 3794
		private int m_error;

		// Token: 0x04000ED3 RID: 3795
		private string m_publicKeyToken;

		// Token: 0x04000ED4 RID: 3796
		private AsymmetricAlgorithm m_snKey;
	}
}
