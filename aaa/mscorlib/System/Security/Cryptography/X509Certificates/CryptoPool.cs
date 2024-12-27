using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008DC RID: 2268
	internal static class CryptoPool
	{
		// Token: 0x060052BE RID: 21182 RVA: 0x0012B0F8 File Offset: 0x0012A0F8
		public static byte[] Rent(int size)
		{
			return new byte[size];
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x0012B100 File Offset: 0x0012A100
		public static void Return(byte[] array, int clearSize)
		{
			CryptographicOperations.ZeroMemory(new Span<byte>(array, 0, clearSize));
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x0012B10F File Offset: 0x0012A10F
		public static void Return(byte[] array)
		{
			CryptographicOperations.ZeroMemory(new Span<byte>(array));
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x0012B11C File Offset: 0x0012A11C
		public static void Return(ArraySegment<byte> segment, int clearSize)
		{
			CryptographicOperations.ZeroMemory(new Span<byte>(segment).Slice(0, clearSize));
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x0012B13E File Offset: 0x0012A13E
		public static void Return(ArraySegment<byte> segment)
		{
			CryptographicOperations.ZeroMemory(new Span<byte>(segment));
		}
	}
}
