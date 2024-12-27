using System;

namespace System.Collections.Generic
{
	// Token: 0x0200027F RID: 639
	[Serializable]
	internal class NullableEqualityComparer<T> : EqualityComparer<T?> where T : struct, IEquatable<T>
	{
		// Token: 0x060019B5 RID: 6581 RVA: 0x00043BC5 File Offset: 0x00042BC5
		public override bool Equals(T? x, T? y)
		{
			if (x != null)
			{
				return y != null && x.value.Equals(y.value);
			}
			return y == null;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x00043C01 File Offset: 0x00042C01
		public override int GetHashCode(T? obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00043C10 File Offset: 0x00042C10
		internal override int IndexOf(T?[] array, T? value, int startIndex, int count)
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
					if (array[j] != null && array[j].value.Equals(value.value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x00043C88 File Offset: 0x00042C88
		internal override int LastIndexOf(T?[] array, T? value, int startIndex, int count)
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
					if (array[j] != null && array[j].value.Equals(value.value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00043D00 File Offset: 0x00042D00
		public override bool Equals(object obj)
		{
			NullableEqualityComparer<T> nullableEqualityComparer = obj as NullableEqualityComparer<T>;
			return nullableEqualityComparer != null;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00043D1B File Offset: 0x00042D1B
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
