using System;

namespace System.Collections.Generic
{
	// Token: 0x02000273 RID: 627
	[Serializable]
	internal class ObjectComparer<T> : Comparer<T>
	{
		// Token: 0x0600192D RID: 6445 RVA: 0x0004215D File Offset: 0x0004115D
		public override int Compare(T x, T y)
		{
			return Comparer.Default.Compare(x, y);
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00042178 File Offset: 0x00041178
		public override bool Equals(object obj)
		{
			ObjectComparer<T> objectComparer = obj as ObjectComparer<T>;
			return objectComparer != null;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00042193 File Offset: 0x00041193
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
