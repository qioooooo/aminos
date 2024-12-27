using System;

namespace System.Collections.Generic
{
	// Token: 0x0200027E RID: 638
	[Serializable]
	internal class GenericEqualityComparer<T> : EqualityComparer<T> where T : IEquatable<T>
	{
		// Token: 0x060019AE RID: 6574 RVA: 0x00043A6C File Offset: 0x00042A6C
		public override bool Equals(T x, T y)
		{
			if (x != null)
			{
				return y != null && x.Equals(y);
			}
			return y == null;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00043A9A File Offset: 0x00042A9A
		public override int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00043AB4 File Offset: 0x00042AB4
		internal override int IndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex + count;
			if (value == null)
			{
				for (int i = startIndex; i < num; i++)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j < num; j++)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00043B20 File Offset: 0x00042B20
		internal override int LastIndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			if (value == null)
			{
				for (int i = startIndex; i >= num; i--)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j >= num; j--)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00043B90 File Offset: 0x00042B90
		public override bool Equals(object obj)
		{
			GenericEqualityComparer<T> genericEqualityComparer = obj as GenericEqualityComparer<T>;
			return genericEqualityComparer != null;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00043BAB File Offset: 0x00042BAB
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
