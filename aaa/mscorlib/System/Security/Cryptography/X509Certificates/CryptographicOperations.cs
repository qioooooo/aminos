using System;
using System.Runtime.CompilerServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008B7 RID: 2231
	internal static class CryptographicOperations
	{
		// Token: 0x060051F3 RID: 20979 RVA: 0x0012707B File Offset: 0x0012607B
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ZeroMemory(Span<byte> buffer)
		{
			buffer.Clear();
		}
	}
}
