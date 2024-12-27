using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BA RID: 2234
	internal class IncrementalHash : IDisposable
	{
		// Token: 0x06005203 RID: 20995 RVA: 0x00127234 File Offset: 0x00126234
		private IncrementalHash(HashAlgorithm algorithm)
		{
			this._algorithm = algorithm;
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x00127244 File Offset: 0x00126244
		public static IncrementalHash CreateHash(HashAlgorithmName hashAlgorithm)
		{
			if (hashAlgorithm == HashAlgorithmName.MD5)
			{
				return new IncrementalHash(MD5.Create());
			}
			if (hashAlgorithm == HashAlgorithmName.SHA1)
			{
				return new IncrementalHash(SHA1.Create());
			}
			if (hashAlgorithm == HashAlgorithmName.SHA256)
			{
				return new IncrementalHash(SHA256.Create());
			}
			if (hashAlgorithm == HashAlgorithmName.SHA384)
			{
				return new IncrementalHash(SHA384.Create());
			}
			if (hashAlgorithm == HashAlgorithmName.SHA512)
			{
				return new IncrementalHash(SHA512.Create());
			}
			throw new CryptographicException();
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x001272D0 File Offset: 0x001262D0
		public void AppendData(ReadOnlySpan<byte> data)
		{
			ArraySegment<byte> arraySegment = data.DangerousGetArraySegment();
			this._algorithm.TransformBlock(arraySegment.Array, arraySegment.Offset, arraySegment.Count, null, 0);
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x00127308 File Offset: 0x00126308
		public bool TryGetHashAndReset(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length < this._algorithm.HashSize / 8)
			{
				bytesWritten = 0;
				return false;
			}
			this._algorithm.TransformFinalBlock(IncrementalHash.s_Empty, 0, 0);
			byte[] hash = this._algorithm.Hash;
			this._algorithm.Initialize();
			new ReadOnlyMemory<byte>(hash).CopyTo(destination);
			bytesWritten = hash.Length;
			return true;
		}

		// Token: 0x06005207 RID: 20999 RVA: 0x0012736F File Offset: 0x0012636F
		public void Dispose()
		{
			this._algorithm.Clear();
		}

		// Token: 0x04002A08 RID: 10760
		private readonly HashAlgorithm _algorithm;

		// Token: 0x04002A09 RID: 10761
		private static readonly byte[] s_Empty = new byte[0];
	}
}
