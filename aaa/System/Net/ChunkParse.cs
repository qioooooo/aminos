using System;

namespace System.Net
{
	// Token: 0x020004B8 RID: 1208
	internal static class ChunkParse
	{
		// Token: 0x06002558 RID: 9560 RVA: 0x00094F1C File Offset: 0x00093F1C
		internal static int SkipPastCRLF(IReadChunkBytes Source)
		{
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num2 = Source.NextByte;
			num++;
			while (num2 != -1)
			{
				if (flag3)
				{
					if (num2 != 10)
					{
						return 0;
					}
					if (flag)
					{
						return 0;
					}
					if (!flag2)
					{
						return num;
					}
					flag4 = true;
					flag = true;
					flag3 = false;
				}
				else if (flag4)
				{
					if (num2 != 32 && num2 != 9)
					{
						return 0;
					}
					flag = true;
					flag4 = false;
				}
				if (!flag)
				{
					int num3 = num2;
					if (num3 <= 13)
					{
						if (num3 == 10)
						{
							return 0;
						}
						if (num3 == 13)
						{
							flag3 = true;
						}
					}
					else if (num3 != 34)
					{
						if (num3 == 92)
						{
							if (flag2)
							{
								flag = true;
							}
						}
					}
					else
					{
						flag2 = !flag2;
					}
				}
				else
				{
					flag = false;
				}
				num2 = Source.NextByte;
				num++;
			}
			return -1;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x00094FD0 File Offset: 0x00093FD0
		internal static int GetChunkSize(IReadChunkBytes Source, out int chunkSize)
		{
			int num = 0;
			int num2 = Source.NextByte;
			int num3 = 0;
			if (num2 == 10 || num2 == 13)
			{
				num3++;
				num2 = Source.NextByte;
			}
			while (num2 != -1)
			{
				if (num2 >= 48 && num2 <= 57)
				{
					num2 -= 48;
				}
				else
				{
					if (num2 >= 97 && num2 <= 102)
					{
						num2 -= 97;
					}
					else
					{
						if (num2 < 65 || num2 > 70)
						{
							Source.NextByte = num2;
							chunkSize = num;
							return num3;
						}
						num2 -= 65;
					}
					num2 += 10;
				}
				num *= 16;
				num += num2;
				num3++;
				num2 = Source.NextByte;
			}
			chunkSize = num;
			return -1;
		}
	}
}
