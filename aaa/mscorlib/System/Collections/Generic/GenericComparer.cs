using System;

namespace System.Collections.Generic
{
	// Token: 0x02000271 RID: 625
	[Serializable]
	internal class GenericComparer<T> : Comparer<T> where T : IComparable<T>
	{
		// Token: 0x06001925 RID: 6437 RVA: 0x00042085 File Offset: 0x00041085
		public override int Compare(T x, T y)
		{
			if (x != null)
			{
				if (y != null)
				{
					return x.CompareTo(y);
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

		// Token: 0x06001926 RID: 6438 RVA: 0x000420B4 File Offset: 0x000410B4
		public override bool Equals(object obj)
		{
			GenericComparer<T> genericComparer = obj as GenericComparer<T>;
			return genericComparer != null;
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x000420CF File Offset: 0x000410CF
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
