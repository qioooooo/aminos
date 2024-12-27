using System;
using System.Globalization;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008D2 RID: 2258
	internal static class Pkcs12Kdf
	{
		// Token: 0x0600527B RID: 21115 RVA: 0x0012A30B File Offset: 0x0012930B
		internal static void DeriveCipherKey(ReadOnlySpan<char> password, HashAlgorithmName hashAlgorithm, int iterationCount, ReadOnlySpan<byte> salt, Span<byte> destination)
		{
			Pkcs12Kdf.Derive(password, hashAlgorithm, iterationCount, 1, salt, destination);
		}

		// Token: 0x0600527C RID: 21116 RVA: 0x0012A319 File Offset: 0x00129319
		internal static void DeriveIV(ReadOnlySpan<char> password, HashAlgorithmName hashAlgorithm, int iterationCount, ReadOnlySpan<byte> salt, Span<byte> destination)
		{
			Pkcs12Kdf.Derive(password, hashAlgorithm, iterationCount, 2, salt, destination);
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x0012A327 File Offset: 0x00129327
		internal static void DeriveMacKey(ReadOnlySpan<char> password, HashAlgorithmName hashAlgorithm, int iterationCount, ReadOnlySpan<byte> salt, Span<byte> destination)
		{
			Pkcs12Kdf.Derive(password, hashAlgorithm, iterationCount, 3, salt, destination);
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x0012A338 File Offset: 0x00129338
		private static void Derive(ReadOnlySpan<char> password, HashAlgorithmName hashAlgorithm, int iterationCount, byte id, ReadOnlySpan<byte> salt, Span<byte> destination)
		{
			int num = -1;
			int num2 = -1;
			foreach (Triple<HashAlgorithmName, int, int> triple in Pkcs12Kdf.s_uvLookup)
			{
				if (triple.Item1 == hashAlgorithm)
				{
					num = triple.Item2;
					num2 = triple.Item3;
					break;
				}
			}
			if (num == -1)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a known hash algorithm.", new object[] { hashAlgorithm.Name }));
			}
			int num3 = num2 >> 3;
			Span<byte> span = new byte[num3];
			span.Fill(id);
			int num4 = (salt.Length - 1 + num3) / num3 * num3;
			byte[] array2;
			Span<byte> span2;
			IncrementalHash incrementalHash;
			checked
			{
				int num5 = (password.Length + 1) * 2;
				if (password.IsNull)
				{
					num5 = 0;
				}
				int num6 = (num5 - 1 + num3) / num3 * num3;
				int num7 = num4 + num6;
				array2 = CryptoPool.Rent(num7);
				span2 = new Span<byte>(array2, 0, num7);
				KdfWorkLimiter.RecordIterations(iterationCount);
				incrementalHash = IncrementalHash.CreateHash(hashAlgorithm);
			}
			try
			{
				Pkcs12Kdf.CircularCopy(salt, span2.Slice(0, num4));
				Pkcs12Kdf.CircularCopyUtf16BE(password, span2.Slice(num4));
				int num8 = num >> 3;
				Span<byte> span3 = new byte[num8];
				Span<byte> span4 = new byte[num3];
				for (;;)
				{
					incrementalHash.AppendData(span);
					incrementalHash.AppendData(span2);
					for (int j = iterationCount; j > 0; j--)
					{
						int num9;
						if (!incrementalHash.TryGetHashAndReset(span3, out num9) || num9 != span3.Length)
						{
							goto IL_016E;
						}
						if (j != 1)
						{
							incrementalHash.AppendData(span3);
						}
					}
					if (span3.Length >= destination.Length)
					{
						goto Block_9;
					}
					span3.CopyTo(destination);
					destination = destination.Slice(span3.Length);
					Pkcs12Kdf.CircularCopy(span3, span4);
					for (int k = span2.Length / num3 - 1; k >= 0; k--)
					{
						Span<byte> span5 = span2.Slice(k * num3, num3);
						Pkcs12Kdf.AddPlusOne(span5, span4);
					}
				}
				IL_016E:
				throw new CryptographicException();
				Block_9:
				span3.Slice(0, destination.Length).CopyTo(destination);
			}
			finally
			{
				CryptographicOperations.ZeroMemory(span2);
				if (array2 != null)
				{
					CryptoPool.Return(array2, 0);
				}
				incrementalHash.Dispose();
			}
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x0012A598 File Offset: 0x00129598
		private static void AddPlusOne(Span<byte> into, Span<byte> addend)
		{
			int num = 1;
			for (int i = into.Length - 1; i >= 0; i--)
			{
				int num2 = num + (int)into[i] + (int)addend[i];
				into[i] = (byte)num2;
				num = num2 >> 8;
			}
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x0012A5E0 File Offset: 0x001295E0
		private static void CircularCopy(ReadOnlySpan<byte> bytes, Span<byte> destination)
		{
			while (destination.Length > 0)
			{
				if (destination.Length < bytes.Length)
				{
					bytes.Slice(0, destination.Length).CopyTo(destination);
					return;
				}
				bytes.CopyTo(destination);
				destination = destination.Slice(bytes.Length);
			}
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x0012A63C File Offset: 0x0012963C
		private static void CircularCopyUtf16BE(ReadOnlySpan<char> password, Span<byte> destination)
		{
			int num = password.Length * 2;
			Encoding bigEndianUnicode = Encoding.BigEndianUnicode;
			while (destination.Length > 0)
			{
				if (destination.Length >= num)
				{
					int num2 = Utility.EncodingGetBytes(bigEndianUnicode, password, destination);
					if (num2 != num)
					{
						throw new CryptographicException();
					}
					destination = destination.Slice(num2);
					Span<byte> span = destination.Slice(0, Math.Min(2, destination.Length));
					span.Clear();
					destination = destination.Slice(span.Length);
				}
				else
				{
					ReadOnlySpan<char> readOnlySpan = password.Slice(0, destination.Length / 2);
					int num3 = Utility.EncodingGetBytes(bigEndianUnicode, readOnlySpan, destination);
					if (num3 != destination.Length)
					{
						throw new CryptographicException();
					}
					return;
				}
			}
		}

		// Token: 0x04002A5D RID: 10845
		private const byte CipherKeyId = 1;

		// Token: 0x04002A5E RID: 10846
		private const byte IvId = 2;

		// Token: 0x04002A5F RID: 10847
		private const byte MacKeyId = 3;

		// Token: 0x04002A60 RID: 10848
		private static readonly Triple<HashAlgorithmName, int, int>[] s_uvLookup = new Triple<HashAlgorithmName, int, int>[]
		{
			new Triple<HashAlgorithmName, int, int>(HashAlgorithmName.MD5, 128, 512),
			new Triple<HashAlgorithmName, int, int>(HashAlgorithmName.SHA1, 160, 512),
			new Triple<HashAlgorithmName, int, int>(HashAlgorithmName.SHA256, 256, 512),
			new Triple<HashAlgorithmName, int, int>(HashAlgorithmName.SHA384, 384, 1024),
			new Triple<HashAlgorithmName, int, int>(HashAlgorithmName.SHA512, 512, 1024)
		};
	}
}
