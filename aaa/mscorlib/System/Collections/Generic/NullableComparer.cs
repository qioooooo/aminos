using System;

namespace System.Collections.Generic
{
	// Token: 0x02000272 RID: 626
	[Serializable]
	internal class NullableComparer<T> : Comparer<T?> where T : struct, IComparable<T>
	{
		// Token: 0x06001929 RID: 6441 RVA: 0x000420E9 File Offset: 0x000410E9
		public override int Compare(T? x, T? y)
		{
			if (x != null)
			{
				if (y != null)
				{
					return x.value.CompareTo(y.value);
				}
				return 1;
			}
			else
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x00042128 File Offset: 0x00041128
		public override bool Equals(object obj)
		{
			NullableComparer<T> nullableComparer = obj as NullableComparer<T>;
			return nullableComparer != null;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x00042143 File Offset: 0x00041143
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
