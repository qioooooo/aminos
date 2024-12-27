using System;
using System.Security;
using System.Threading;

namespace System
{
	// Token: 0x0200079F RID: 1951
	internal static class ClientUtils
	{
		// Token: 0x06003C0C RID: 15372 RVA: 0x00100D2B File Offset: 0x000FFD2B
		public static bool IsCriticalException(Exception ex)
		{
			return ex is NullReferenceException || ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is ExecutionEngineException || ex is IndexOutOfRangeException || ex is AccessViolationException;
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x00100D68 File Offset: 0x000FFD68
		public static bool IsSecurityOrCriticalException(Exception ex)
		{
			return ex is SecurityException || ClientUtils.IsCriticalException(ex);
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00100D7C File Offset: 0x000FFD7C
		public static int GetBitCount(uint x)
		{
			int num = 0;
			while (x > 0U)
			{
				x &= x - 1U;
				num++;
			}
			return num;
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x00100DA0 File Offset: 0x000FFDA0
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x00100DC0 File Offset: 0x000FFDC0
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue, int maxNumberOfBitsOn)
		{
			bool flag = value >= minValue && value <= maxValue;
			return flag && ClientUtils.GetBitCount((uint)value) <= maxNumberOfBitsOn;
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x00100DF4 File Offset: 0x000FFDF4
		public static bool IsEnumValid_Masked(Enum enumValue, int value, uint mask)
		{
			return ((long)value & (long)((ulong)mask)) == (long)value;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x00100E0C File Offset: 0x000FFE0C
		public static bool IsEnumValid_NotSequential(Enum enumValue, int value, params int[] enumValues)
		{
			for (int i = 0; i < enumValues.Length; i++)
			{
				if (enumValues[i] == value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
