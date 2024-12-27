using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000868 RID: 2152
	[ComVisible(true)]
	public interface ICspAsymmetricAlgorithm
	{
		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06004EE1 RID: 20193
		CspKeyContainerInfo CspKeyContainerInfo { get; }

		// Token: 0x06004EE2 RID: 20194
		byte[] ExportCspBlob(bool includePrivateParameters);

		// Token: 0x06004EE3 RID: 20195
		void ImportCspBlob(byte[] rawData);
	}
}
