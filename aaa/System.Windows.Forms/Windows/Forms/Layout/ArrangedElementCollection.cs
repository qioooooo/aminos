using System;
using System.Collections;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020001F2 RID: 498
	public class ArrangedElementCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x060016F6 RID: 5878 RVA: 0x000230EB File Offset: 0x000220EB
		internal ArrangedElementCollection()
		{
			this._innerList = new ArrayList(4);
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x000230FF File Offset: 0x000220FF
		internal ArrangedElementCollection(ArrayList innerList)
		{
			this._innerList = innerList;
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x0002310E File Offset: 0x0002210E
		private ArrangedElementCollection(int size)
		{
			this._innerList = new ArrayList(size);
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x00023122 File Offset: 0x00022122
		internal ArrayList InnerList
		{
			get
			{
				return this._innerList;
			}
		}

		// Token: 0x1700029E RID: 670
		internal virtual IArrangedElement this[int index]
		{
			get
			{
				return (IArrangedElement)this.InnerList[index];
			}
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00023140 File Offset: 0x00022140
		public override bool Equals(object obj)
		{
			ArrangedElementCollection arrangedElementCollection = obj as ArrangedElementCollection;
			if (arrangedElementCollection == null || this.Count != arrangedElementCollection.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (this.InnerList[i] != arrangedElementCollection.InnerList[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x00023195 File Offset: 0x00022195
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000231A0 File Offset: 0x000221A0
		internal void MoveElement(IArrangedElement element, int fromIndex, int toIndex)
		{
			int num = toIndex - fromIndex;
			switch (num)
			{
			case -1:
			case 1:
				this.InnerList[fromIndex] = this.InnerList[toIndex];
				goto IL_0059;
			}
			int num2;
			int num3;
			if (num > 0)
			{
				num2 = fromIndex + 1;
				num3 = fromIndex;
			}
			else
			{
				num2 = toIndex;
				num3 = toIndex + 1;
				num = -num;
			}
			ArrangedElementCollection.Copy(this, num2, this, num3, num);
			IL_0059:
			this.InnerList[toIndex] = element;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00023214 File Offset: 0x00022214
		private static void Copy(ArrangedElementCollection sourceList, int sourceIndex, ArrangedElementCollection destinationList, int destinationIndex, int length)
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

		// Token: 0x060016FF RID: 5887 RVA: 0x0002328E File Offset: 0x0002228E
		void IList.Clear()
		{
			this.InnerList.Clear();
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06001700 RID: 5888 RVA: 0x0002329B File Offset: 0x0002229B
		bool IList.IsFixedSize
		{
			get
			{
				return this.InnerList.IsFixedSize;
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x000232A8 File Offset: 0x000222A8
		bool IList.Contains(object value)
		{
			return this.InnerList.Contains(value);
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x000232B6 File Offset: 0x000222B6
		public virtual bool IsReadOnly
		{
			get
			{
				return this.InnerList.IsReadOnly;
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x000232C3 File Offset: 0x000222C3
		void IList.RemoveAt(int index)
		{
			this.InnerList.RemoveAt(index);
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x000232D1 File Offset: 0x000222D1
		void IList.Remove(object value)
		{
			this.InnerList.Remove(value);
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000232DF File Offset: 0x000222DF
		int IList.Add(object value)
		{
			return this.InnerList.Add(value);
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000232ED File Offset: 0x000222ED
		int IList.IndexOf(object value)
		{
			return this.InnerList.IndexOf(value);
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x000232FB File Offset: 0x000222FB
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002A1 RID: 673
		object IList.this[int index]
		{
			get
			{
				return this.InnerList[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x00023317 File Offset: 0x00022317
		public virtual int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00023324 File Offset: 0x00022324
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00023331 File Offset: 0x00022331
		public void CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x00023340 File Offset: 0x00022340
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0002334D File Offset: 0x0002234D
		public virtual IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x0400116F RID: 4463
		internal static ArrangedElementCollection Empty = new ArrangedElementCollection(0);

		// Token: 0x04001170 RID: 4464
		private ArrayList _innerList;
	}
}
