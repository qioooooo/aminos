using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000043 RID: 67
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakHashtable : Hashtable
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x000070B7 File Offset: 0x000060B7
		internal WeakHashtable()
			: base(WeakHashtable._comparer)
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000070C4 File Offset: 0x000060C4
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000070DC File Offset: 0x000060DC
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

		// Token: 0x04000B6E RID: 2926
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x04000B6F RID: 2927
		private long _lastGlobalMem;

		// Token: 0x04000B70 RID: 2928
		private int _lastHashCount;

		// Token: 0x02000044 RID: 68
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x060001EB RID: 491 RVA: 0x00007234 File Offset: 0x00006234
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

			// Token: 0x060001EC RID: 492 RVA: 0x000072A3 File Offset: 0x000062A3
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x02000045 RID: 69
		private sealed class EqualityWeakReference : WeakReference
		{
			// Token: 0x060001EE RID: 494 RVA: 0x000072B3 File Offset: 0x000062B3
			internal EqualityWeakReference(object o)
				: base(o)
			{
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x060001EF RID: 495 RVA: 0x000072C8 File Offset: 0x000062C8
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || (this.IsAlive && object.ReferenceEquals(o, this.Target)));
			}

			// Token: 0x060001F0 RID: 496 RVA: 0x000072FC File Offset: 0x000062FC
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04000B71 RID: 2929
			private int _hashCode;
		}
	}
}
