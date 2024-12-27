using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008DB RID: 2267
	internal static class BinaryPrimitives
	{
		// Token: 0x060052BC RID: 21180 RVA: 0x0012B0BA File Offset: 0x0012A0BA
		public static bool TryReadUInt16BigEndian(ReadOnlySpan<byte> bytes, out ushort value)
		{
			if (bytes.Length < 2)
			{
				value = 0;
				return false;
			}
			value = (ushort)((int)bytes[1] | ((int)bytes[0] << 8));
			return true;
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x0012B0E2 File Offset: 0x0012A0E2
		public static short ReadInt16BigEndian(ReadOnlySpan<byte> bytes)
		{
			return (short)((int)bytes[1] | ((int)bytes[0] << 8));
		}
	}
}
