using System;
using System.Collections;
using System.Security;
using System.Threading;

namespace System.Drawing
{
	// Token: 0x02000013 RID: 19
	internal static class ClientUtils
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002E63 File Offset: 0x00001E63
		public static bool IsCriticalException(Exception ex)
		{
			return ex is NullReferenceException || ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is ExecutionEngineException || ex is IndexOutOfRangeException || ex is AccessViolationException;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002EA0 File Offset: 0x00001EA0
		public static bool IsSecurityOrCriticalException(Exception ex)
		{
			return ex is SecurityException || ClientUtils.IsCriticalException(ex);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002EB4 File Offset: 0x00001EB4
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

		// Token: 0x06000040 RID: 64 RVA: 0x00002ED8 File Offset: 0x00001ED8
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002EF8 File Offset: 0x00001EF8
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue, int maxNumberOfBitsOn)
		{
			bool flag = value >= minValue && value <= maxValue;
			return flag && ClientUtils.GetBitCount((uint)value) <= maxNumberOfBitsOn;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002F2C File Offset: 0x00001F2C
		public static bool IsEnumValid_Masked(Enum enumValue, int value, uint mask)
		{
			return ((long)value & (long)((ulong)mask)) == (long)value;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002F44 File Offset: 0x00001F44
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

		// Token: 0x02000014 RID: 20
		internal class WeakRefCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06000044 RID: 68 RVA: 0x00002F68 File Offset: 0x00001F68
			internal WeakRefCollection()
			{
				this._innerList = new ArrayList(4);
			}

			// Token: 0x06000045 RID: 69 RVA: 0x00002F87 File Offset: 0x00001F87
			internal WeakRefCollection(int size)
			{
				this._innerList = new ArrayList(size);
			}

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000046 RID: 70 RVA: 0x00002FA6 File Offset: 0x00001FA6
			internal ArrayList InnerList
			{
				get
				{
					return this._innerList;
				}
			}

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000047 RID: 71 RVA: 0x00002FAE File Offset: 0x00001FAE
			// (set) Token: 0x06000048 RID: 72 RVA: 0x00002FB6 File Offset: 0x00001FB6
			public int RefCheckThreshold
			{
				get
				{
					return this.refCheckThreshold;
				}
				set
				{
					this.refCheckThreshold = value;
				}
			}

			// Token: 0x1700000A RID: 10
			public object this[int index]
			{
				get
				{
					ClientUtils.WeakRefCollection.WeakRefObject weakRefObject = this.InnerList[index] as ClientUtils.WeakRefCollection.WeakRefObject;
					if (weakRefObject != null && weakRefObject.IsAlive)
					{
						return weakRefObject.Target;
					}
					return null;
				}
				set
				{
					this.InnerList[index] = this.CreateWeakRefObject(value);
				}
			}

			// Token: 0x0600004B RID: 75 RVA: 0x00003008 File Offset: 0x00002008
			public void ScavengeReferences()
			{
				int num = 0;
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					if (this[num] == null)
					{
						this.InnerList.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}

			// Token: 0x0600004C RID: 76 RVA: 0x00003048 File Offset: 0x00002048
			public override bool Equals(object obj)
			{
				ClientUtils.WeakRefCollection weakRefCollection = obj as ClientUtils.WeakRefCollection;
				if (weakRefCollection == this)
				{
					return true;
				}
				if (weakRefCollection == null || this.Count != weakRefCollection.Count)
				{
					return false;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (this.InnerList[i] != weakRefCollection.InnerList[i] && (this.InnerList[i] == null || !this.InnerList[i].Equals(weakRefCollection.InnerList[i])))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0600004D RID: 77 RVA: 0x000030D0 File Offset: 0x000020D0
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x0600004E RID: 78 RVA: 0x000030D8 File Offset: 0x000020D8
			private ClientUtils.WeakRefCollection.WeakRefObject CreateWeakRefObject(object value)
			{
				if (value == null)
				{
					return null;
				}
				return new ClientUtils.WeakRefCollection.WeakRefObject(value);
			}

			// Token: 0x0600004F RID: 79 RVA: 0x000030E8 File Offset: 0x000020E8
			private static void Copy(ClientUtils.WeakRefCollection sourceList, int sourceIndex, ClientUtils.WeakRefCollection destinationList, int destinationIndex, int length)
			{
				if (sourceIndex < destinationIndex)
				{
					sourceIndex += length;
					destinationIndex += length;
					while (length > 0)
					{
						destinationList.InnerList[--destinationIndex] = sourceList.InnerList[--sourceIndex];
						length--;
					}
					return;
				}
				while (length > 0)
				{
					destinationList.InnerList[destinationIndex++] = sourceList.InnerList[sourceIndex++];
					length--;
				}
			}

			// Token: 0x06000050 RID: 80 RVA: 0x00003164 File Offset: 0x00002164
			public void RemoveByHashCode(object value)
			{
				if (value == null)
				{
					return;
				}
				int hashCode = value.GetHashCode();
				for (int i = 0; i < this.InnerList.Count; i++)
				{
					if (this.InnerList[i] != null && this.InnerList[i].GetHashCode() == hashCode)
					{
						this.RemoveAt(i);
						return;
					}
				}
			}

			// Token: 0x06000051 RID: 81 RVA: 0x000031BC File Offset: 0x000021BC
			public void Clear()
			{
				this.InnerList.Clear();
			}

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000052 RID: 82 RVA: 0x000031C9 File Offset: 0x000021C9
			public bool IsFixedSize
			{
				get
				{
					return this.InnerList.IsFixedSize;
				}
			}

			// Token: 0x06000053 RID: 83 RVA: 0x000031D6 File Offset: 0x000021D6
			public bool Contains(object value)
			{
				return this.InnerList.Contains(this.CreateWeakRefObject(value));
			}

			// Token: 0x06000054 RID: 84 RVA: 0x000031EA File Offset: 0x000021EA
			public void RemoveAt(int index)
			{
				this.InnerList.RemoveAt(index);
			}

			// Token: 0x06000055 RID: 85 RVA: 0x000031F8 File Offset: 0x000021F8
			public void Remove(object value)
			{
				this.InnerList.Remove(this.CreateWeakRefObject(value));
			}

			// Token: 0x06000056 RID: 86 RVA: 0x0000320C File Offset: 0x0000220C
			public int IndexOf(object value)
			{
				return this.InnerList.IndexOf(this.CreateWeakRefObject(value));
			}

			// Token: 0x06000057 RID: 87 RVA: 0x00003220 File Offset: 0x00002220
			public void Insert(int index, object value)
			{
				this.InnerList.Insert(index, this.CreateWeakRefObject(value));
			}

			// Token: 0x06000058 RID: 88 RVA: 0x00003235 File Offset: 0x00002235
			public int Add(object value)
			{
				if (this.Count > this.RefCheckThreshold)
				{
					this.ScavengeReferences();
				}
				return this.InnerList.Add(this.CreateWeakRefObject(value));
			}

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000059 RID: 89 RVA: 0x0000325D File Offset: 0x0000225D
			public int Count
			{
				get
				{
					return this.InnerList.Count;
				}
			}

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x0600005A RID: 90 RVA: 0x0000326A File Offset: 0x0000226A
			object ICollection.SyncRoot
			{
				get
				{
					return this.InnerList.SyncRoot;
				}
			}

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x0600005B RID: 91 RVA: 0x00003277 File Offset: 0x00002277
			public bool IsReadOnly
			{
				get
				{
					return this.InnerList.IsReadOnly;
				}
			}

			// Token: 0x0600005C RID: 92 RVA: 0x00003284 File Offset: 0x00002284
			public void CopyTo(Array array, int index)
			{
				this.InnerList.CopyTo(array, index);
			}

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600005D RID: 93 RVA: 0x00003293 File Offset: 0x00002293
			bool ICollection.IsSynchronized
			{
				get
				{
					return this.InnerList.IsSynchronized;
				}
			}

			// Token: 0x0600005E RID: 94 RVA: 0x000032A0 File Offset: 0x000022A0
			public IEnumerator GetEnumerator()
			{
				return this.InnerList.GetEnumerator();
			}

			// Token: 0x040000D1 RID: 209
			private int refCheckThreshold = int.MaxValue;

			// Token: 0x040000D2 RID: 210
			private ArrayList _innerList;

			// Token: 0x02000015 RID: 21
			internal class WeakRefObject
			{
				// Token: 0x0600005F RID: 95 RVA: 0x000032AD File Offset: 0x000022AD
				internal WeakRefObject(object obj)
				{
					this.weakHolder = new WeakReference(obj);
					this.hash = obj.GetHashCode();
				}

				// Token: 0x17000010 RID: 16
				// (get) Token: 0x06000060 RID: 96 RVA: 0x000032CD File Offset: 0x000022CD
				internal bool IsAlive
				{
					get
					{
						return this.weakHolder.IsAlive;
					}
				}

				// Token: 0x17000011 RID: 17
				// (get) Token: 0x06000061 RID: 97 RVA: 0x000032DA File Offset: 0x000022DA
				internal object Target
				{
					get
					{
						return this.weakHolder.Target;
					}
				}

				// Token: 0x06000062 RID: 98 RVA: 0x000032E7 File Offset: 0x000022E7
				public override int GetHashCode()
				{
					return this.hash;
				}

				// Token: 0x06000063 RID: 99 RVA: 0x000032F0 File Offset: 0x000022F0
				public override bool Equals(object obj)
				{
					ClientUtils.WeakRefCollection.WeakRefObject weakRefObject = obj as ClientUtils.WeakRefCollection.WeakRefObject;
					return weakRefObject == this || (weakRefObject != null && (weakRefObject.Target == this.Target || (this.Target != null && this.Target.Equals(weakRefObject.Target))));
				}

				// Token: 0x040000D3 RID: 211
				private int hash;

				// Token: 0x040000D4 RID: 212
				private WeakReference weakHolder;
			}
		}
	}
}
