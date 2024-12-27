using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x02000406 RID: 1030
	internal struct SecureCredential
	{
		// Token: 0x060020B5 RID: 8373 RVA: 0x00080E28 File Offset: 0x0007FE28
		public SecureCredential(int version, X509Certificate certificate, SecureCredential.Flags flags, SchProtocols protocols)
		{
			this.rootStore = (this.phMappers = (this.palgSupportedAlgs = (this.certContextArray = IntPtr.Zero)));
			this.cCreds = (this.cMappers = (this.cSupportedAlgs = 0));
			this.dwMinimumCipherStrength = (this.dwMaximumCipherStrength = 0);
			this.dwSessionLifespan = (this.reserved = 0);
			this.version = version;
			this.dwFlags = flags;
			this.grbitEnabledProtocols = protocols;
			if (certificate != null)
			{
				this.certContextArray = certificate.Handle;
				this.cCreds = 1;
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00080EC6 File Offset: 0x0007FEC6
		[Conditional("TRAVE")]
		internal void DebugDump()
		{
		}

		// Token: 0x04002072 RID: 8306
		public const int CurrentVersion = 4;

		// Token: 0x04002073 RID: 8307
		public int version;

		// Token: 0x04002074 RID: 8308
		public int cCreds;

		// Token: 0x04002075 RID: 8309
		public IntPtr certContextArray;

		// Token: 0x04002076 RID: 8310
		private readonly IntPtr rootStore;

		// Token: 0x04002077 RID: 8311
		public int cMappers;

		// Token: 0x04002078 RID: 8312
		private readonly IntPtr phMappers;

		// Token: 0x04002079 RID: 8313
		public int cSupportedAlgs;

		// Token: 0x0400207A RID: 8314
		private readonly IntPtr palgSupportedAlgs;

		// Token: 0x0400207B RID: 8315
		public SchProtocols grbitEnabledProtocols;

		// Token: 0x0400207C RID: 8316
		public int dwMinimumCipherStrength;

		// Token: 0x0400207D RID: 8317
		public int dwMaximumCipherStrength;

		// Token: 0x0400207E RID: 8318
		public int dwSessionLifespan;

		// Token: 0x0400207F RID: 8319
		public SecureCredential.Flags dwFlags;

		// Token: 0x04002080 RID: 8320
		public int reserved;

		// Token: 0x02000407 RID: 1031
		[Flags]
		public enum Flags
		{
			// Token: 0x04002082 RID: 8322
			Zero = 0,
			// Token: 0x04002083 RID: 8323
			NoSystemMapper = 2,
			// Token: 0x04002084 RID: 8324
			NoNameCheck = 4,
			// Token: 0x04002085 RID: 8325
			ValidateManual = 8,
			// Token: 0x04002086 RID: 8326
			NoDefaultCred = 16,
			// Token: 0x04002087 RID: 8327
			ValidateAuto = 32,
			// Token: 0x04002088 RID: 8328
			SendAuxRecord = 2097152,
			// Token: 0x04002089 RID: 8329
			UseStrongCrypto = 4194304
		}
	}
}
