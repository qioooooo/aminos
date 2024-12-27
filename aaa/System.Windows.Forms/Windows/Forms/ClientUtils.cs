using System;
using System.Collections;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x0200003B RID: 59
	internal static class ClientUtils
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00006636 File Offset: 0x00005636
		public static bool IsCriticalException(Exception ex)
		{
			return ex is NullReferenceException || ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is ExecutionEngineException || ex is IndexOutOfRangeException || ex is AccessViolationException;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006673 File Offset: 0x00005673
		public static bool IsSecurityOrCriticalException(Exception ex)
		{
			return ex is SecurityException || ClientUtils.IsCriticalException(ex);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006688 File Offset: 0x00005688
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

		// Token: 0x060001A0 RID: 416 RVA: 0x000066AC File Offset: 0x000056AC
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000066CC File Offset: 0x000056CC
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue, int maxNumberOfBitsOn)
		{
			bool flag = value >= minValue && value <= maxValue;
			return flag && ClientUtils.GetBitCount((uint)value) <= maxNumberOfBitsOn;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006700 File Offset: 0x00005700
		public static bool IsEnumValid_Masked(Enum enumValue, int value, uint mask)
		{
			return ((long)value & (long)((ulong)mask)) == (long)value;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006718 File Offset: 0x00005718
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

		// Token: 0x0200003C RID: 60
		internal class WeakRefCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060001A4 RID: 420 RVA: 0x0000673C File Offset: 0x0000573C
			internal WeakRefCollection()
			{
				this._innerList = new ArrayList(4);
			}

			// Token: 0x060001A5 RID: 421 RVA: 0x0000675B File Offset: 0x0000575B
			internal WeakRefCollection(int size)
			{
				this._innerList = new ArrayList(size);
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000677A File Offset: 0x0000577A
			internal ArrayList InnerList
			{
				get
				{
					return this._innerList;
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006782 File Offset: 0x00005782
			// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000678A File Offset: 0x0000578A
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

			// Token: 0x1700004E RID: 78
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

			// Token: 0x060001AB RID: 427 RVA: 0x000067DC File Offset: 0x000057DC
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

			// Token: 0x060001AC RID: 428 RVA: 0x0000681C File Offset: 0x0000581C
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

			// Token: 0x060001AD RID: 429 RVA: 0x000068A4 File Offset: 0x000058A4
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x060001AE RID: 430 RVA: 0x000068AC File Offset: 0x000058AC
			private ClientUtils.WeakRefCollection.WeakRefObject CreateWeakRefObject(object value)
			{
				if (value == null)
				{
					return null;
				}
				return new ClientUtils.WeakRefCollection.WeakRefObject(value);
			}

			// Token: 0x060001AF RID: 431 RVA: 0x000068BC File Offset: 0x000058BC
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

			// Token: 0x060001B0 RID: 432 RVA: 0x00006938 File Offset: 0x00005938
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

			// Token: 0x060001B1 RID: 433 RVA: 0x00006990 File Offset: 0x00005990
			public void Clear()
			{
				this.InnerList.Clear();
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000699D File Offset: 0x0000599D
			public bool IsFixedSize
			{
				get
				{
					return this.InnerList.IsFixedSize;
				}
			}

			// Token: 0x060001B3 RID: 435 RVA: 0x000069AA File Offset: 0x000059AA
			public bool Contains(object value)
			{
				return this.InnerList.Contains(this.CreateWeakRefObject(value));
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x000069BE File Offset: 0x000059BE
			public void RemoveAt(int index)
			{
				this.InnerList.RemoveAt(index);
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x000069CC File Offset: 0x000059CC
			public void Remove(object value)
			{
				this.InnerList.Remove(this.CreateWeakRefObject(value));
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x000069E0 File Offset: 0x000059E0
			public int IndexOf(object value)
			{
				return this.InnerList.IndexOf(this.CreateWeakRefObject(value));
			}

			// Token: 0x060001B7 RID: 439 RVA: 0x000069F4 File Offset: 0x000059F4
			public void Insert(int index, object value)
			{
				this.InnerList.Insert(index, this.CreateWeakRefObject(value));
			}

			// Token: 0x060001B8 RID: 440 RVA: 0x00006A09 File Offset: 0x00005A09
			public int Add(object value)
			{
				if (this.Count > this.RefCheckThreshold)
				{
					this.ScavengeReferences();
				}
				return this.InnerList.Add(this.CreateWeakRefObject(value));
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060001B9 RID: 441 RVA: 0x00006A31 File Offset: 0x00005A31
			public int Count
			{
				get
				{
					return this.InnerList.Count;
				}
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060001BA RID: 442 RVA: 0x00006A3E File Offset: 0x00005A3E
			object ICollection.SyncRoot
			{
				get
				{
					return this.InnerList.SyncRoot;
				}
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060001BB RID: 443 RVA: 0x00006A4B File Offset: 0x00005A4B
			public bool IsReadOnly
			{
				get
				{
					return this.InnerList.IsReadOnly;
				}
			}

			// Token: 0x060001BC RID: 444 RVA: 0x00006A58 File Offset: 0x00005A58
			public void CopyTo(Array array, int index)
			{
				this.InnerList.CopyTo(array, index);
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060001BD RID: 445 RVA: 0x00006A67 File Offset: 0x00005A67
			bool ICollection.IsSynchronized
			{
				get
				{
					return this.InnerList.IsSynchronized;
				}
			}

			// Token: 0x060001BE RID: 446 RVA: 0x00006A74 File Offset: 0x00005A74
			public IEnumerator GetEnumerator()
			{
				return this.InnerList.GetEnumerator();
			}

			// Token: 0x04000B60 RID: 2912
			private int refCheckThreshold = int.MaxValue;

			// Token: 0x04000B61 RID: 2913
			private ArrayList _innerList;

			// Token: 0x0200003D RID: 61
			internal class WeakRefObject
			{
				// Token: 0x060001BF RID: 447 RVA: 0x00006A81 File Offset: 0x00005A81
				internal WeakRefObject(object obj)
				{
					this.weakHolder = new WeakReference(obj);
					this.hash = obj.GetHashCode();
				}

				// Token: 0x17000054 RID: 84
				// (get) Token: 0x060001C0 RID: 448 RVA: 0x00006AA1 File Offset: 0x00005AA1
				internal bool IsAlive
				{
					get
					{
						return this.weakHolder.IsAlive;
					}
				}

				// Token: 0x17000055 RID: 85
				// (get) Token: 0x060001C1 RID: 449 RVA: 0x00006AAE File Offset: 0x00005AAE
				internal object Target
				{
					get
					{
						return this.weakHolder.Target;
					}
				}

				// Token: 0x060001C2 RID: 450 RVA: 0x00006ABB File Offset: 0x00005ABB
				public override int GetHashCode()
				{
					return this.hash;
				}

				// Token: 0x060001C3 RID: 451 RVA: 0x00006AC4 File Offset: 0x00005AC4
				public override bool Equals(object obj)
				{
					ClientUtils.WeakRefCollection.WeakRefObject weakRefObject = obj as ClientUtils.WeakRefCollection.WeakRefObject;
					return weakRefObject == this || (weakRefObject != null && (weakRefObject.Target == this.Target || (this.Target != null && this.Target.Equals(weakRefObject.Target))));
				}

				// Token: 0x04000B62 RID: 2914
				private int hash;

				// Token: 0x04000B63 RID: 2915
				private WeakReference weakHolder;
			}
		}
	}
}
