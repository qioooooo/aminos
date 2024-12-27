using System;
using System.Globalization;

namespace System.Collections.Specialized
{
	// Token: 0x0200024E RID: 590
	internal static class FixedStringLookup
	{
		// Token: 0x0600143D RID: 5181 RVA: 0x000433D4 File Offset: 0x000423D4
		internal static bool Contains(string[][] lookupTable, string value, bool ignoreCase)
		{
			int length = value.Length;
			if (length <= 0 || length - 1 >= lookupTable.Length)
			{
				return false;
			}
			string[] array = lookupTable[length - 1];
			return array != null && FixedStringLookup.Contains(array, value, ignoreCase);
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0004340C File Offset: 0x0004240C
		private static bool Contains(string[] array, string value, bool ignoreCase)
		{
			int num = 0;
			int num2 = array.Length;
			int i = 0;
			while (i < value.Length)
			{
				char c;
				if (ignoreCase)
				{
					c = char.ToLower(value[i], CultureInfo.InvariantCulture);
				}
				else
				{
					c = value[i];
				}
				if (num2 - num <= 1)
				{
					if (c != array[num][i])
					{
						return false;
					}
					i++;
				}
				else
				{
					if (!FixedStringLookup.FindCharacter(array, c, i, ref num, ref num2))
					{
						return false;
					}
					i++;
				}
			}
			return true;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0004347C File Offset: 0x0004247C
		private static bool FindCharacter(string[] array, char value, int pos, ref int min, ref int max)
		{
			int num = min;
			while (min < max)
			{
				num = (min + max) / 2;
				char c = array[num][pos];
				if (value == c)
				{
					int num2 = num;
					while (num2 > min && array[num2 - 1][pos] == value)
					{
						num2--;
					}
					min = num2;
					int num3 = num + 1;
					while (num3 < max && array[num3][pos] == value)
					{
						num3++;
					}
					max = num3;
					return true;
				}
				if (value < c)
				{
					max = num;
				}
				else
				{
					min = num + 1;
				}
			}
			return false;
		}
	}
}
