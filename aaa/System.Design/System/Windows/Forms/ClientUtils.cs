using System;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x0200000D RID: 13
	internal static class ClientUtils
	{
		// Token: 0x06000021 RID: 33 RVA: 0x000026D8 File Offset: 0x000016D8
		public static bool IsCriticalException(Exception ex)
		{
			return ex is NullReferenceException || ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is ExecutionEngineException || ex is IndexOutOfRangeException || ex is AccessViolationException;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002715 File Offset: 0x00001715
		public static bool IsSecurityOrCriticalException(Exception ex)
		{
			return ex is SecurityException || ClientUtils.IsCriticalException(ex);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002728 File Offset: 0x00001728
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

		// Token: 0x06000024 RID: 36 RVA: 0x0000274C File Offset: 0x0000174C
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000276C File Offset: 0x0000176C
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue, int maxNumberOfBitsOn)
		{
			bool flag = value >= minValue && value <= maxValue;
			return flag && ClientUtils.GetBitCount((uint)value) <= maxNumberOfBitsOn;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027A0 File Offset: 0x000017A0
		public static bool IsEnumValid_Masked(Enum enumValue, int value, uint mask)
		{
			return ((long)value & (long)((ulong)mask)) == (long)value;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000027B8 File Offset: 0x000017B8
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
