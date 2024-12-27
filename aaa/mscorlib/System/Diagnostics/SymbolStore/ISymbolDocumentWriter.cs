using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002B8 RID: 696
	[ComVisible(true)]
	public interface ISymbolDocumentWriter
	{
		// Token: 0x06001B6B RID: 7019
		void SetSource(byte[] source);

		// Token: 0x06001B6C RID: 7020
		void SetCheckSum(Guid algorithmId, byte[] checkSum);
	}
}
