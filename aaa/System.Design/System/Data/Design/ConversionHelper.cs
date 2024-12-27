using System;

namespace System.Data.Design
{
	// Token: 0x02000069 RID: 105
	internal class ConversionHelper
	{
		// Token: 0x06000498 RID: 1176 RVA: 0x00003CD5 File Offset: 0x00002CD5
		private ConversionHelper()
		{
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00003CE0 File Offset: 0x00002CE0
		internal static bool CanConvert(Type sourceUrtType, Type destinationUrtType)
		{
			short num = -1;
			short num2 = -1;
			short num3 = 0;
			while ((int)num3 < ConversionHelper.urtTypeIndexTable.Length)
			{
				if (sourceUrtType == ConversionHelper.urtTypeIndexTable[(int)num3])
				{
					num = num3;
					break;
				}
				num3 += 1;
			}
			short num4 = 0;
			while ((int)num4 < ConversionHelper.urtTypeIndexTable.Length)
			{
				if (destinationUrtType == ConversionHelper.urtTypeIndexTable[(int)num4])
				{
					num2 = num4;
					break;
				}
				num4 += 1;
			}
			if (num != -1 && num2 != -1)
			{
				short num5 = ConversionHelper.urtConversionTable[(int)num];
				short num6 = (short)(16384 >> (int)num2);
				if ((num5 & num6) != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00003D5B File Offset: 0x00002D5B
		internal static string GetConversionMethodName(Type sourceUrtType, Type targetUrtType)
		{
			return "To" + targetUrtType.Name;
		}

		// Token: 0x04000A92 RID: 2706
		private static short[] urtConversionTable = new short[]
		{
			24573, 16353, 32765, 32765, 32765, 32765, 32765, 32765, 32765, 32765,
			24573, 24573, 24573, 3, short.MaxValue
		};

		// Token: 0x04000A93 RID: 2707
		private static short[] urtSafeConversionTable = new short[]
		{
			24573, 16353, 32765, 32765, 32765, 32765, 32765, 32765, 32765, 32765,
			24573, 24573, 24573, 3, 1
		};

		// Token: 0x04000A94 RID: 2708
		private static Type[] urtTypeIndexTable = new Type[]
		{
			typeof(bool),
			typeof(char),
			typeof(sbyte),
			typeof(byte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(DateTime),
			typeof(string)
		};
	}
}
