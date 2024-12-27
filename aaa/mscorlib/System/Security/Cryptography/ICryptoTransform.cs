using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200084F RID: 2127
	[ComVisible(true)]
	public interface ICryptoTransform : IDisposable
	{
		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06004E23 RID: 20003
		int InputBlockSize { get; }

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06004E24 RID: 20004
		int OutputBlockSize { get; }

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06004E25 RID: 20005
		bool CanTransformMultipleBlocks { get; }

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06004E26 RID: 20006
		bool CanReuseTransform { get; }

		// Token: 0x06004E27 RID: 20007
		int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		// Token: 0x06004E28 RID: 20008
		byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
	}
}
