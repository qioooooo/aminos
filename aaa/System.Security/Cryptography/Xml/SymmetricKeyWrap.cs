using System;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000B9 RID: 185
	internal static class SymmetricKeyWrap
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x0001628C File Offset: 0x0001528C
		internal static byte[] TripleDESKeyWrapEncrypt(byte[] rgbKey, byte[] rgbWrappedKeyData)
		{
			SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] array = sha1CryptoServiceProvider.ComputeHash(rgbWrappedKeyData);
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			byte[] array2 = new byte[8];
			rngcryptoServiceProvider.GetBytes(array2);
			byte[] array3 = new byte[rgbWrappedKeyData.Length + 8];
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
			tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
			ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor(rgbKey, array2);
			Buffer.BlockCopy(rgbWrappedKeyData, 0, array3, 0, rgbWrappedKeyData.Length);
			Buffer.BlockCopy(array, 0, array3, rgbWrappedKeyData.Length, 8);
			byte[] array4 = cryptoTransform.TransformFinalBlock(array3, 0, array3.Length);
			byte[] array5 = new byte[array2.Length + array4.Length];
			Buffer.BlockCopy(array2, 0, array5, 0, array2.Length);
			Buffer.BlockCopy(array4, 0, array5, array2.Length, array4.Length);
			Array.Reverse(array5);
			ICryptoTransform cryptoTransform2 = tripleDESCryptoServiceProvider.CreateEncryptor(rgbKey, SymmetricKeyWrap.s_rgbTripleDES_KW_IV);
			return cryptoTransform2.TransformFinalBlock(array5, 0, array5.Length);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001635C File Offset: 0x0001535C
		internal static byte[] TripleDESKeyWrapDecrypt(byte[] rgbKey, byte[] rgbEncryptedWrappedKeyData)
		{
			if (rgbEncryptedWrappedKeyData.Length != 32 && rgbEncryptedWrappedKeyData.Length != 40 && rgbEncryptedWrappedKeyData.Length != 48)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_KW_BadKeySize"));
			}
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
			tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
			ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor(rgbKey, SymmetricKeyWrap.s_rgbTripleDES_KW_IV);
			byte[] array = cryptoTransform.TransformFinalBlock(rgbEncryptedWrappedKeyData, 0, rgbEncryptedWrappedKeyData.Length);
			Array.Reverse(array);
			byte[] array2 = new byte[8];
			Buffer.BlockCopy(array, 0, array2, 0, 8);
			byte[] array3 = new byte[array.Length - array2.Length];
			Buffer.BlockCopy(array, 8, array3, 0, array3.Length);
			ICryptoTransform cryptoTransform2 = tripleDESCryptoServiceProvider.CreateDecryptor(rgbKey, array2);
			byte[] array4 = cryptoTransform2.TransformFinalBlock(array3, 0, array3.Length);
			byte[] array5 = new byte[array4.Length - 8];
			Buffer.BlockCopy(array4, 0, array5, 0, array5.Length);
			SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] array6 = sha1CryptoServiceProvider.ComputeHash(array5);
			int i = array5.Length;
			int num = 0;
			while (i < array4.Length)
			{
				if (array4[i] != array6[num])
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_BadWrappedKeySize"));
				}
				i++;
				num++;
			}
			return array5;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001646C File Offset: 0x0001546C
		internal static byte[] AESKeyWrapEncrypt(byte[] rgbKey, byte[] rgbWrappedKeyData)
		{
			int num = rgbWrappedKeyData.Length >> 3;
			if (rgbWrappedKeyData.Length % 8 != 0 || num <= 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_KW_BadKeySize"));
			}
			ICryptoTransform cryptoTransform = new RijndaelManaged
			{
				Key = rgbKey,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.None
			}.CreateEncryptor();
			if (num == 1)
			{
				byte[] array = new byte[SymmetricKeyWrap.s_rgbAES_KW_IV.Length + rgbWrappedKeyData.Length];
				Buffer.BlockCopy(SymmetricKeyWrap.s_rgbAES_KW_IV, 0, array, 0, SymmetricKeyWrap.s_rgbAES_KW_IV.Length);
				Buffer.BlockCopy(rgbWrappedKeyData, 0, array, SymmetricKeyWrap.s_rgbAES_KW_IV.Length, rgbWrappedKeyData.Length);
				return cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			}
			byte[] array2 = new byte[num + 1 << 3];
			Buffer.BlockCopy(rgbWrappedKeyData, 0, array2, 8, rgbWrappedKeyData.Length);
			byte[] array3 = new byte[8];
			byte[] array4 = new byte[16];
			Buffer.BlockCopy(SymmetricKeyWrap.s_rgbAES_KW_IV, 0, array3, 0, 8);
			for (int i = 0; i <= 5; i++)
			{
				for (int j = 1; j <= num; j++)
				{
					long num2 = (long)(j + i * num);
					Buffer.BlockCopy(array3, 0, array4, 0, 8);
					Buffer.BlockCopy(array2, 8 * j, array4, 8, 8);
					byte[] array5 = cryptoTransform.TransformFinalBlock(array4, 0, 16);
					for (int k = 0; k < 8; k++)
					{
						byte b = (byte)((num2 >> 8 * (7 - k)) & 255L);
						array3[k] = b ^ array5[k];
					}
					Buffer.BlockCopy(array5, 8, array2, 8 * j, 8);
				}
			}
			Buffer.BlockCopy(array3, 0, array2, 0, 8);
			return array2;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000165EC File Offset: 0x000155EC
		internal static byte[] AESKeyWrapDecrypt(byte[] rgbKey, byte[] rgbEncryptedWrappedKeyData)
		{
			int num = (rgbEncryptedWrappedKeyData.Length >> 3) - 1;
			if (rgbEncryptedWrappedKeyData.Length % 8 != 0 || num <= 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_KW_BadKeySize"));
			}
			byte[] array = new byte[num << 3];
			ICryptoTransform cryptoTransform = new RijndaelManaged
			{
				Key = rgbKey,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.None
			}.CreateDecryptor();
			if (num == 1)
			{
				byte[] array2 = cryptoTransform.TransformFinalBlock(rgbEncryptedWrappedKeyData, 0, rgbEncryptedWrappedKeyData.Length);
				for (int i = 0; i < 8; i++)
				{
					if (array2[i] != SymmetricKeyWrap.s_rgbAES_KW_IV[i])
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_BadWrappedKeySize"));
					}
				}
				Buffer.BlockCopy(array2, 8, array, 0, 8);
				return array;
			}
			Buffer.BlockCopy(rgbEncryptedWrappedKeyData, 8, array, 0, array.Length);
			byte[] array3 = new byte[8];
			byte[] array4 = new byte[16];
			Buffer.BlockCopy(rgbEncryptedWrappedKeyData, 0, array3, 0, 8);
			for (int j = 5; j >= 0; j--)
			{
				for (int k = num; k >= 1; k--)
				{
					long num2 = (long)(k + j * num);
					for (int l = 0; l < 8; l++)
					{
						byte b = (byte)((num2 >> 8 * (7 - l)) & 255L);
						byte[] array5 = array3;
						int num3 = l;
						array5[num3] ^= b;
					}
					Buffer.BlockCopy(array3, 0, array4, 0, 8);
					Buffer.BlockCopy(array, 8 * (k - 1), array4, 8, 8);
					byte[] array6 = cryptoTransform.TransformFinalBlock(array4, 0, 16);
					Buffer.BlockCopy(array6, 8, array, 8 * (k - 1), 8);
					Buffer.BlockCopy(array6, 0, array3, 0, 8);
				}
			}
			for (int m = 0; m < 8; m++)
			{
				if (array3[m] != SymmetricKeyWrap.s_rgbAES_KW_IV[m])
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_BadWrappedKeySize"));
				}
			}
			return array;
		}

		// Token: 0x0400058F RID: 1423
		private static readonly byte[] s_rgbTripleDES_KW_IV = new byte[] { 74, 221, 162, 44, 121, 232, 33, 5 };

		// Token: 0x04000590 RID: 1424
		private static readonly byte[] s_rgbAES_KW_IV = new byte[] { 166, 166, 166, 166, 166, 166, 166, 166 };
	}
}
