using System;
using System.Collections;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002E3 RID: 739
	public sealed class BehaviorServiceAdornerCollection : CollectionBase
	{
		// Token: 0x06001C61 RID: 7265 RVA: 0x0009F7C9 File Offset: 0x0009E7C9
		public BehaviorServiceAdornerCollection(BehaviorService behaviorService)
		{
			this.behaviorService = behaviorService;
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x0009F7D8 File Offset: 0x0009E7D8
		public BehaviorServiceAdornerCollection(BehaviorServiceAdornerCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x0009F7E7 File Offset: 0x0009E7E7
		public BehaviorServiceAdornerCollection(Adorner[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170004EC RID: 1260
		public Adorner this[int index]
		{
			get
			{
				return (Adorner)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x0009F818 File Offset: 0x0009E818
		public int Add(Adorner value)
		{
			value.BehaviorService = this.behaviorService;
			return base.List.Add(value);
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x0009F834 File Offset: 0x0009E834
		public void AddRange(Adorner[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x0009F85C File Offset: 0x0009E85C
		public void AddRange(BehaviorServiceAdornerCollection value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0009F888 File Offset: 0x0009E888
		public bool Contains(Adorner value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x0009F896 File Offset: 0x0009E896
		public void CopyTo(Adorner[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x0009F8A5 File Offset: 0x0009E8A5
		public int IndexOf(Adorner value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0009F8B3 File Offset: 0x0009E8B3
		public void Insert(int index, Adorner value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x0009F8C2 File Offset: 0x0009E8C2
		public new BehaviorServiceAdornerCollectionEnumerator GetEnumerator()
		{
			return new BehaviorServiceAdornerCollectionEnumerator(this);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0009F8CA File Offset: 0x0009E8CA
		public void Remove(Adorner value)
		{
			base.List.Remove(value);
		}

		// Token: 0x040015E3 RID: 5603
		private BehaviorService behaviorService;
	}
}
