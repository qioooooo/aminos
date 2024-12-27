using System;

namespace System.Collections.Generic
{
	// Token: 0x02000280 RID: 640
	[Serializable]
	internal class ObjectEqualityComparer<T> : EqualityComparer<T>
	{
		// Token: 0x060019BC RID: 6588 RVA: 0x00043D35 File Offset: 0x00042D35
		public override bool Equals(T x, T y)
		{
			if (x != null)
			{
				return y != null && x.Equals(y);
			}
			return y == null;
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00043D68 File Offset: 0x00042D68
		public override int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00043D84 File Offset: 0x00042D84
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

		// Token: 0x060019BF RID: 6591 RVA: 0x00043DF8 File Offset: 0x00042DF8
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

		// Token: 0x060019C0 RID: 6592 RVA: 0x00043E6C File Offset: 0x00042E6C
		public override bool Equals(object obj)
		{
			ObjectEqualityComparer<T> objectEqualityComparer = obj as ObjectEqualityComparer<T>;
			return objectEqualityComparer != null;
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x00043E87 File Offset: 0x00042E87
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
