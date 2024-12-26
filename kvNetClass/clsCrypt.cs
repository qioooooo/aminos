using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace kvNetClass
{
	// Token: 0x02000003 RID: 3
	public class clsCrypt
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000264C File Offset: 0x0000084C
		public clsCrypt()
		{
			this.DES = new DESCryptoServiceProvider();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002660 File Offset: 0x00000860
		public void EncryptFileWrite(string sString, FileStream fs, string encryptKey, string encryptIV)
		{
			try
			{
				byte[] array = clsCrypt.ConvertStringToByteArray(sString);
				ICryptoTransform cryptoTransform = this.DES.CreateEncryptor(Encoding.Default.GetBytes(encryptKey), Encoding.Default.GetBytes(encryptIV));
				CryptoStream cryptoStream = new CryptoStream(fs, cryptoTransform, CryptoStreamMode.Write);
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.Close();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000026DC File Offset: 0x000008DC
		public string DecryptFileRead(FileStream fs, string encryptKey, string encryptIV)
		{
			string text;
			try
			{
				ICryptoTransform cryptoTransform = this.DES.CreateDecryptor(Encoding.Default.GetBytes(encryptKey), Encoding.Default.GetBytes(encryptIV));
				CryptoStream cryptoStream = new CryptoStream(fs, cryptoTransform, CryptoStreamMode.Read);
				text = new StreamReader(cryptoStream, new UnicodeEncoding()).ReadToEnd();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return text;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002750 File Offset: 0x00000950
		public static byte[] ConvertStringToByteArray(string s)
		{
			return new UnicodeEncoding().GetBytes(s);
		}

		// Token: 0x04000001 RID: 1
		private DESCryptoServiceProvider DES;
	}
}
