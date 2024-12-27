using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020007A5 RID: 1957
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakHashtable : Hashtable
	{
		// Token: 0x06003C23 RID: 15395 RVA: 0x001010BB File Offset: 0x001000BB
		internal WeakHashtable()
			: base(WeakHashtable._comparer)
		{
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x001010C8 File Offset: 0x001000C8
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x001010E0 File Offset: 0x001000E0
		private void ScavengeKeys()
		{
			int count = this.Count;
			if (count == 0)
			{
				return;
			}
			if (this._lastHashCount == 0)
			{
				this._lastHashCount = count;
				return;
			}
			long totalMemory = GC.GetTotalMemory(false);
			if (this._lastGlobalMem == 0L)
			{
				this._lastGlobalMem = totalMemory;
				return;
			}
			float num = (float)(totalMemory - this._lastGlobalMem) / (float)this._lastGlobalMem;
			float num2 = (float)(count - this._lastHashCount) / (float)this._lastHashCount;
			if (num < 0f && num2 >= 0f)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakReference weakReference = obj as WeakReference;
					if (weakReference != null && !weakReference.IsAlive)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(weakReference);
					}
				}
				if (arrayList != null)
				{
					foreach (object obj2 in arrayList)
					{
						this.Remove(obj2);
					}
				}
			}
			this._lastGlobalMem = totalMemory;
			this._lastHashCount = count;
		}

		// Token: 0x0400350A RID: 13578
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x0400350B RID: 13579
		private long _lastGlobalMem;

		// Token: 0x0400350C RID: 13580
		private int _lastHashCount;

		// Token: 0x020007A6 RID: 1958
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x06003C27 RID: 15399 RVA: 0x00101238 File Offset: 0x00100238
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return true;
				}
				if (x == null || y == null)
				{
					return false;
				}
				if (x.GetHashCode() == y.GetHashCode())
				{
					WeakReference weakReference = x as WeakReference;
					WeakReference weakReference2 = y as WeakReference;
					if (weakReference != null)
					{
						if (!weakReference.IsAlive)
						{
							return false;
						}
						x = weakReference.Target;
					}
					if (weakReference2 != null)
					{
						if (!weakReference2.IsAlive)
						{
							return false;
						}
						y = weakReference2.Target;
					}
					return object.ReferenceEquals(x, y);
				}
				return false;
			}

			// Token: 0x06003C28 RID: 15400 RVA: 0x001012A7 File Offset: 0x001002A7
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x020007A7 RID: 1959
		private sealed class EqualityWeakReference : WeakReference
		{
			// Token: 0x06003C2A RID: 15402 RVA: 0x001012B7 File Offset: 0x001002B7
			internal EqualityWeakReference(object o)
				: base(o)
			{
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x06003C2B RID: 15403 RVA: 0x001012CC File Offset: 0x001002CC
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || (this.IsAlive && object.ReferenceEquals(o, this.Target)));
			}

			// Token: 0x06003C2C RID: 15404 RVA: 0x00101300 File Offset: 0x00100300
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x0400350D RID: 13581
			private int _hashCode;
		}
	}
}
