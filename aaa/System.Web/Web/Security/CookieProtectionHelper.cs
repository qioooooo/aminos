using System;
using System.Web.Configuration;

namespace System.Web.Security
{
	// Token: 0x02000330 RID: 816
	internal class CookieProtectionHelper
	{
		// Token: 0x060027FA RID: 10234 RVA: 0x000AF620 File Offset: 0x000AE620
		internal static string Encode(CookieProtection cookieProtection, byte[] buf, int count)
		{
			if (cookieProtection == CookieProtection.All || cookieProtection == CookieProtection.Validation)
			{
				byte[] array = MachineKeySection.HashData(buf, null, 0, count);
				if (array == null || array.Length != 20)
				{
					return null;
				}
				if (buf.Length >= count + 20)
				{
					Buffer.BlockCopy(array, 0, buf, count, 20);
				}
				else
				{
					byte[] array2 = buf;
					buf = new byte[count + 20];
					Buffer.BlockCopy(array2, 0, buf, 0, count);
					Buffer.BlockCopy(array, 0, buf, count, 20);
				}
				count += 20;
			}
			if (cookieProtection == CookieProtection.All || cookieProtection == CookieProtection.Encryption)
			{
				buf = MachineKeySection.EncryptOrDecryptData(true, buf, null, 0, count);
				count = buf.Length;
			}
			if (count < buf.Length)
			{
				byte[] array3 = buf;
				buf = new byte[count];
				Buffer.BlockCopy(array3, 0, buf, 0, count);
			}
			return HttpServerUtility.UrlTokenEncode(buf);
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000AF6C4 File Offset: 0x000AE6C4
		internal static byte[] Decode(CookieProtection cookieProtection, string data)
		{
			byte[] array = HttpServerUtility.UrlTokenDecode(data);
			if (array == null || cookieProtection == CookieProtection.None)
			{
				return array;
			}
			if (cookieProtection == CookieProtection.All || cookieProtection == CookieProtection.Encryption)
			{
				array = MachineKeySection.EncryptOrDecryptData(false, array, null, 0, array.Length);
				if (array == null)
				{
					return null;
				}
			}
			if (cookieProtection == CookieProtection.All || cookieProtection == CookieProtection.Validation)
			{
				if (array.Length <= 20)
				{
					return null;
				}
				byte[] array2 = new byte[array.Length - 20];
				Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
				byte[] array3 = MachineKeySection.HashData(array2, null, 0, array2.Length);
				if (array3 == null || array3.Length != 20)
				{
					return null;
				}
				for (int i = 0; i < 20; i++)
				{
					if (array3[i] != array[array2.Length + i])
					{
						return null;
					}
				}
				array = array2;
			}
			return array;
		}
	}
}
