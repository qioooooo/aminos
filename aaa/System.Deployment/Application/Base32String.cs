using System;
using System.Text;

namespace System.Deployment.Application
{
	// Token: 0x020000E6 RID: 230
	internal class Base32String
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x0001E664 File Offset: 0x0001D664
		public static string FromBytes(byte[] bytes)
		{
			int num = bytes.Length;
			if (num <= 0)
			{
				return null;
			}
			int num2 = num << 3;
			int num3 = num2 / 5 << 3;
			if (num2 % 5 != 0)
			{
				num3 += 8;
			}
			int num4 = num3 >> 3;
			StringBuilder stringBuilder = new StringBuilder(num4);
			int i = 0;
			int num5 = 0;
			for (int j = 0; j < num; j++)
			{
				num5 = (num5 << 8) | (int)bytes[j];
				i += 8;
				while (i >= 5)
				{
					i -= 5;
					stringBuilder.Append(Base32String.charList[(num5 >> i) & 31]);
				}
			}
			if (i > 0)
			{
				stringBuilder.Append(Base32String.charList[(num5 << 5 - i) & 31]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040004B4 RID: 1204
		protected static char[] charList = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'G', 'H', 'J', 'K', 'L',
			'M', 'N', 'O', 'P', 'Q', 'R', 'T', 'V', 'W', 'X',
			'Y', 'Z'
		};
	}
}
