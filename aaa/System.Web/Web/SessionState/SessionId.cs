using System;
using System.Security.Cryptography;

namespace System.Web.SessionState
{
	// Token: 0x02000369 RID: 873
	internal static class SessionId
	{
		// Token: 0x06002A40 RID: 10816 RVA: 0x000BC588 File Offset: 0x000BB588
		static SessionId()
		{
			for (int i = SessionId.s_encoding.Length - 1; i >= 0; i--)
			{
				char c = SessionId.s_encoding[i];
				SessionId.s_legalchars[(int)c] = true;
			}
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000BC5E0 File Offset: 0x000BB5E0
		internal static bool IsLegit(string s)
		{
			if (s == null || s.Length != 24)
			{
				return false;
			}
			bool flag;
			try
			{
				int num = 24;
				while (--num >= 0)
				{
					char c = s[num];
					if (!SessionId.s_legalchars[(int)c])
					{
						return false;
					}
				}
				flag = true;
			}
			catch (IndexOutOfRangeException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000BC638 File Offset: 0x000BB638
		private static string Encode(byte[] buffer)
		{
			char[] array = new char[24];
			int num = 0;
			for (int i = 0; i < 15; i += 5)
			{
				int num2 = (int)buffer[i] | ((int)buffer[i + 1] << 8) | ((int)buffer[i + 2] << 16) | ((int)buffer[i + 3] << 24);
				int num3 = num2 & 31;
				array[num++] = SessionId.s_encoding[num3];
				num3 = (num2 >> 5) & 31;
				array[num++] = SessionId.s_encoding[num3];
				num3 = (num2 >> 10) & 31;
				array[num++] = SessionId.s_encoding[num3];
				num3 = (num2 >> 15) & 31;
				array[num++] = SessionId.s_encoding[num3];
				num3 = (num2 >> 20) & 31;
				array[num++] = SessionId.s_encoding[num3];
				num3 = (num2 >> 25) & 31;
				array[num++] = SessionId.s_encoding[num3];
				num2 = (num2 >> 30) | ((int)buffer[i + 4] << 2);
				num3 = num2 & 31;
				array[num++] = SessionId.s_encoding[num3];
				num3 = (num2 >> 5) & 31;
				array[num++] = SessionId.s_encoding[num3];
			}
			return new string(array);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000BC748 File Offset: 0x000BB748
		internal static string Create(ref RandomNumberGenerator randgen)
		{
			if (randgen == null)
			{
				randgen = new RNGCryptoServiceProvider();
			}
			byte[] array = new byte[15];
			randgen.GetBytes(array);
			return SessionId.Encode(array);
		}

		// Token: 0x04001F5C RID: 8028
		internal const int NUM_CHARS_IN_ENCODING = 32;

		// Token: 0x04001F5D RID: 8029
		internal const int ENCODING_BITS_PER_CHAR = 5;

		// Token: 0x04001F5E RID: 8030
		internal const int ID_LENGTH_BITS = 120;

		// Token: 0x04001F5F RID: 8031
		internal const int ID_LENGTH_BYTES = 15;

		// Token: 0x04001F60 RID: 8032
		internal const int ID_LENGTH_CHARS = 24;

		// Token: 0x04001F61 RID: 8033
		private static char[] s_encoding = new char[]
		{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
			'4', '5'
		};

		// Token: 0x04001F62 RID: 8034
		private static bool[] s_legalchars = new bool[128];
	}
}
