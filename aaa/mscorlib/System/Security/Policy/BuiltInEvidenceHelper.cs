using System;

namespace System.Security.Policy
{
	// Token: 0x0200048E RID: 1166
	internal static class BuiltInEvidenceHelper
	{
		// Token: 0x06002EDE RID: 11998 RVA: 0x0009FA42 File Offset: 0x0009EA42
		internal static void CopyIntToCharArray(int value, char[] buffer, int position)
		{
			buffer[position] = (char)((value >> 16) & 65535);
			buffer[position + 1] = (char)(value & 65535);
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x0009FA60 File Offset: 0x0009EA60
		internal static int GetIntFromCharArray(char[] buffer, int position)
		{
			int num = (int)buffer[position];
			num <<= 16;
			return num + (int)buffer[position + 1];
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x0009FA80 File Offset: 0x0009EA80
		internal static void CopyLongToCharArray(long value, char[] buffer, int position)
		{
			buffer[position] = (char)((value >> 48) & 65535L);
			buffer[position + 1] = (char)((value >> 32) & 65535L);
			buffer[position + 2] = (char)((value >> 16) & 65535L);
			buffer[position + 3] = (char)(value & 65535L);
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x0009FACC File Offset: 0x0009EACC
		internal static long GetLongFromCharArray(char[] buffer, int position)
		{
			long num = (long)((ulong)buffer[position]);
			num <<= 16;
			num += (long)((ulong)buffer[position + 1]);
			num <<= 16;
			num += (long)((ulong)buffer[position + 2]);
			num <<= 16;
			return num + (long)((ulong)buffer[position + 3]);
		}

		// Token: 0x040017AB RID: 6059
		internal const char idApplicationDirectory = '\0';

		// Token: 0x040017AC RID: 6060
		internal const char idPublisher = '\u0001';

		// Token: 0x040017AD RID: 6061
		internal const char idStrongName = '\u0002';

		// Token: 0x040017AE RID: 6062
		internal const char idZone = '\u0003';

		// Token: 0x040017AF RID: 6063
		internal const char idUrl = '\u0004';

		// Token: 0x040017B0 RID: 6064
		internal const char idWebPage = '\u0005';

		// Token: 0x040017B1 RID: 6065
		internal const char idSite = '\u0006';

		// Token: 0x040017B2 RID: 6066
		internal const char idPermissionRequestEvidence = '\a';

		// Token: 0x040017B3 RID: 6067
		internal const char idHash = '\b';

		// Token: 0x040017B4 RID: 6068
		internal const char idGac = '\t';
	}
}
