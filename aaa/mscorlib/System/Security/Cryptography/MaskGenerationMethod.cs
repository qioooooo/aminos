using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200087B RID: 2171
	[ComVisible(true)]
	public abstract class MaskGenerationMethod
	{
		// Token: 0x06004F79 RID: 20345
		[ComVisible(true)]
		public abstract byte[] GenerateMask(byte[] rgbSeed, int cbReturn);
	}
}
