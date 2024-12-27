using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BF RID: 2239
	[SuppressUnmanagedCodeSecurity]
	internal static class Pbkdf2
	{
		// Token: 0x0600521D RID: 21021 RVA: 0x001284D8 File Offset: 0x001274D8
		static Pbkdf2()
		{
			int num = Pbkdf2.BCryptOpenAlgorithmProvider(out Pbkdf2._sha1, "SHA1", "Microsoft Primitive Provider", OpenAlgorithmProviderFlags.BCRYPT_ALG_HANDLE_HMAC_FLAG);
			if (num != 0)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, "A provider could not be found for algorithm '{0}'.", new object[] { "SHA1" }));
			}
			num = Pbkdf2.BCryptOpenAlgorithmProvider(out Pbkdf2._sha256, "SHA256", "Microsoft Primitive Provider", OpenAlgorithmProviderFlags.BCRYPT_ALG_HANDLE_HMAC_FLAG);
			if (num != 0)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, "A provider could not be found for algorithm '{0}'.", new object[] { "SHA256" }));
			}
			num = Pbkdf2.BCryptOpenAlgorithmProvider(out Pbkdf2._sha384, "SHA384", "Microsoft Primitive Provider", OpenAlgorithmProviderFlags.BCRYPT_ALG_HANDLE_HMAC_FLAG);
			if (num != 0)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, "A provider could not be found for algorithm '{0}'.", new object[] { "SHA384" }));
			}
			num = Pbkdf2.BCryptOpenAlgorithmProvider(out Pbkdf2._sha512, "SHA512", "Microsoft Primitive Provider", OpenAlgorithmProviderFlags.BCRYPT_ALG_HANDLE_HMAC_FLAG);
			if (num != 0)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, "A provider could not be found for algorithm '{0}'.", new object[] { "SHA512" }));
			}
		}

		// Token: 0x0600521E RID: 21022
		[DllImport("bcrypt.dll")]
		private static extern int BCryptOpenAlgorithmProvider(out SafeBCryptAlgorithmHandle phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszImplementation, [In] OpenAlgorithmProviderFlags dwFlags);

		// Token: 0x0600521F RID: 21023 RVA: 0x001285E0 File Offset: 0x001275E0
		internal unsafe static byte[] Derive(string hashAlgorithm, byte[] password, byte[] salt, int iterations, int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (iterations <= 0)
			{
				throw new ArgumentOutOfRangeException("iterations");
			}
			KdfWorkLimiter.RecordIterations(iterations);
			byte[] array = new byte[length];
			if (hashAlgorithm != null)
			{
				SafeBCryptAlgorithmHandle safeBCryptAlgorithmHandle;
				if (!(hashAlgorithm == "SHA1"))
				{
					if (!(hashAlgorithm == "SHA256"))
					{
						if (!(hashAlgorithm == "SHA384"))
						{
							if (!(hashAlgorithm == "SHA512"))
							{
								goto IL_008D;
							}
							safeBCryptAlgorithmHandle = Pbkdf2._sha512;
						}
						else
						{
							safeBCryptAlgorithmHandle = Pbkdf2._sha384;
						}
					}
					else
					{
						safeBCryptAlgorithmHandle = Pbkdf2._sha256;
					}
				}
				else
				{
					safeBCryptAlgorithmHandle = Pbkdf2._sha1;
				}
				fixed (byte* ptr = password)
				{
					fixed (byte* ptr2 = salt)
					{
						fixed (byte* ptr3 = array)
						{
							byte b = 0;
							int num = Pbkdf2.BCryptDeriveKeyPBKDF2(safeBCryptAlgorithmHandle, (ptr != null) ? ptr : (&b), password.Length, (ptr2 != null) ? ptr2 : (&b), salt.Length, (ulong)((long)iterations), ptr3, array.Length, 0U);
							if (num != 0)
							{
								throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, "A call to BCryptDeriveKeyPBKDF2 failed with code '{0}'.", new object[] { num }));
							}
						}
					}
				}
				return array;
			}
			IL_008D:
			throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a known hash algorithm.", new object[] { hashAlgorithm }));
		}

		// Token: 0x06005220 RID: 21024
		[DllImport("bcrypt.dll")]
		internal unsafe static extern int BCryptDeriveKeyPBKDF2(SafeBCryptAlgorithmHandle hPrf, byte* pbPassword, int cbPassword, byte* pbSalt, int cbSalt, ulong cIterations, byte* pbDerivedKey, int cbDerivedKey, uint dwFlags);

		// Token: 0x04002A26 RID: 10790
		internal const string BCRYPT_LIB = "bcrypt.dll";

		// Token: 0x04002A27 RID: 10791
		private const string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

		// Token: 0x04002A28 RID: 10792
		private const int NtStatusSuccess = 0;

		// Token: 0x04002A29 RID: 10793
		internal static readonly SafeBCryptAlgorithmHandle _sha1;

		// Token: 0x04002A2A RID: 10794
		internal static readonly SafeBCryptAlgorithmHandle _sha256;

		// Token: 0x04002A2B RID: 10795
		internal static readonly SafeBCryptAlgorithmHandle _sha384;

		// Token: 0x04002A2C RID: 10796
		internal static readonly SafeBCryptAlgorithmHandle _sha512;
	}
}
