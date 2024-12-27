using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BD RID: 2237
	internal static class Oids
	{
		// Token: 0x04002A0D RID: 10765
		internal static readonly byte[] Pkcs7Data = new byte[] { 42, 134, 72, 134, 247, 13, 1, 7, 1 };

		// Token: 0x04002A0E RID: 10766
		internal static readonly byte[] Pkcs7Encrypted = new byte[] { 42, 134, 72, 134, 247, 13, 1, 7, 6 };

		// Token: 0x04002A0F RID: 10767
		internal static readonly byte[] Pkcs12ShroudedKeyBag = new byte[]
		{
			42, 134, 72, 134, 247, 13, 1, 12, 10, 1,
			2
		};

		// Token: 0x04002A10 RID: 10768
		internal static readonly byte[] PasswordBasedEncryptionScheme2 = new byte[] { 42, 134, 72, 134, 247, 13, 1, 5, 13 };

		// Token: 0x04002A11 RID: 10769
		internal static readonly byte[] Pbkdf2 = new byte[] { 42, 134, 72, 134, 247, 13, 1, 5, 12 };

		// Token: 0x04002A12 RID: 10770
		internal static readonly byte[] PbeWithMD5AndDESCBC = new byte[] { 42, 134, 72, 134, 247, 13, 1, 5, 3 };

		// Token: 0x04002A13 RID: 10771
		internal static readonly byte[] PbeWithMD5AndRC2CBC = new byte[] { 42, 134, 72, 134, 247, 13, 1, 5, 6 };

		// Token: 0x04002A14 RID: 10772
		internal static readonly byte[] PbeWithSha1AndDESCBC = new byte[] { 42, 134, 72, 134, 247, 13, 1, 5, 10 };

		// Token: 0x04002A15 RID: 10773
		internal static readonly byte[] PbeWithSha1AndRC2CBC = new byte[] { 42, 134, 72, 134, 247, 13, 1, 5, 11 };

		// Token: 0x04002A16 RID: 10774
		internal static readonly byte[] Pkcs12PbeWithShaAnd3Key3Des = new byte[] { 42, 134, 72, 134, 247, 13, 1, 12, 1, 3 };

		// Token: 0x04002A17 RID: 10775
		internal static readonly byte[] Pkcs12PbeWithShaAnd2Key3Des = new byte[] { 42, 134, 72, 134, 247, 13, 1, 12, 1, 4 };

		// Token: 0x04002A18 RID: 10776
		internal static readonly byte[] Pkcs12PbeWithShaAnd128BitRC2 = new byte[] { 42, 134, 72, 134, 247, 13, 1, 12, 1, 5 };

		// Token: 0x04002A19 RID: 10777
		internal static readonly byte[] Pkcs12PbeWithShaAnd40BitRC2 = new byte[] { 42, 134, 72, 134, 247, 13, 1, 12, 1, 6 };

		// Token: 0x04002A1A RID: 10778
		internal static readonly byte[] Aes128Cbc = new byte[] { 96, 134, 72, 1, 101, 3, 4, 1, 2 };

		// Token: 0x04002A1B RID: 10779
		internal static readonly byte[] Aes192Cbc = new byte[] { 96, 134, 72, 1, 101, 3, 4, 1, 22 };

		// Token: 0x04002A1C RID: 10780
		internal static readonly byte[] Aes256Cbc = new byte[] { 96, 134, 72, 1, 101, 3, 4, 1, 42 };

		// Token: 0x04002A1D RID: 10781
		internal static readonly byte[] TripleDesCbc = new byte[] { 42, 134, 72, 134, 247, 13, 3, 7 };

		// Token: 0x04002A1E RID: 10782
		internal static readonly byte[] Rc2Cbc = new byte[] { 42, 134, 72, 134, 247, 13, 3, 2 };

		// Token: 0x04002A1F RID: 10783
		internal static readonly byte[] DesCbc = new byte[] { 43, 14, 3, 2, 7 };

		// Token: 0x04002A20 RID: 10784
		internal static readonly byte[] HmacWithSha1 = new byte[] { 42, 134, 72, 134, 247, 13, 2, 7 };

		// Token: 0x04002A21 RID: 10785
		internal static readonly byte[] HmacWithSha256 = new byte[] { 42, 134, 72, 134, 247, 13, 2, 9 };

		// Token: 0x04002A22 RID: 10786
		internal static readonly byte[] HmacWithSha384 = new byte[] { 42, 134, 72, 134, 247, 13, 2, 10 };

		// Token: 0x04002A23 RID: 10787
		internal static readonly byte[] HmacWithSha512 = new byte[] { 42, 134, 72, 134, 247, 13, 2, 11 };
	}
}
